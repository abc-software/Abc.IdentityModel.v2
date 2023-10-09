namespace Abc.IdentityModel.Http {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Abc.IdentityModel.Http.Bindings;
    using System.Threading;
    using System.Threading.Tasks;
#if NETCOREAPP1_0_OR_GREATER
    using Microsoft.AspNetCore.Http;
    using HttpRequestBase = Microsoft.AspNetCore.Http.HttpRequest;
    using HttpResponseBase = Microsoft.AspNetCore.Http.HttpResponse;
    using HttpContextBase = Microsoft.AspNetCore.Http.HttpContext;
    using Microsoft.Extensions.Primitives;

#endif
#if NETFRAMEWORK
    using System.Collections.Specialized;
    using System.Web;
#endif

    /// <summary>
    /// The <see cref="HttpMessageSerializer"/> extension class.
    /// </summary>
    public static class HttpMessageSerializerExtension {
        private const string BaseUriParameter = "BaseUri";
        private const string HttpVerbParameter = "HttpVerb";
        private const char PairDelimiter = '=';
        private const char ParamDelimiter = '\\';

        /// <summary>
        /// Gets a string representation of the URL that corresponds to this message.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="message">The message.</param>
        /// <returns>A URL serialized from the current instance.</returns>
        public static string GetRequestUrl(this HttpMessageSerializer messageSerializer, IHttpProtocolMessage message) {
            if (messageSerializer == null) {
                throw new ArgumentNullException(nameof(messageSerializer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            if ((message.HttpMethods & HttpDeliveryMethods.GetRequest) != HttpDeliveryMethods.GetRequest) {
                throw new ArgumentException("Invalid message delivery methods.", nameof(message));
            }

            var httpResponse = new RedirectHttpResponse();
            messageSerializer.SendMessage(httpResponse, message);
            return httpResponse.RequestUrl;
        }

        /// <summary>
        /// Gets a string representation of the HTML body that corresponds to this message.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="message">The message.</param>
        /// <returns>A HTML body serialized from the current instance.</returns>
        public static string GetPostBody(this HttpMessageSerializer messageSerializer, IHttpProtocolMessage message) {
            if (messageSerializer == null) {
                throw new ArgumentNullException(nameof(messageSerializer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            if ((message.HttpMethods & HttpDeliveryMethods.PostRequest) != HttpDeliveryMethods.PostRequest) {
                throw new ArgumentException("Invalid message delivery methods.", nameof(message));
            }

            var httpResponse = new PostHttpResponse();
            messageSerializer.SendMessage(httpResponse, message);
            return httpResponse.Html;
        }

        /// <summary>
        /// Gets the protocol message from URL.
        /// </summary>
        /// <typeparam name="TRequest">The expected type of the message to be received.</typeparam>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="url">The URL.</param>
        /// <returns>The deserialized message.  Never null.</returns>
        /// <exception cref="T:HttpMessageException">Thrown if the expected message was not recognized in the response.</exception>
        public static TRequest ReadMessage<TRequest>(this HttpMessageSerializer messageSerializer, Uri url) where TRequest : class, IHttpProtocolMessage {
            if (messageSerializer == null) {
                throw new ArgumentNullException(nameof(messageSerializer));
            }

            if (url == null) {
                throw new ArgumentNullException(nameof(url));
            }

            var httpRequest = new RedirectHttpRequest(url);
            return messageSerializer.ReadMessage<TRequest>(httpRequest);
        }

        /// <summary>
        /// Gets the protocol message from URL, if present.
        /// </summary>
        /// <typeparam name="TRequest">The expected type of the message to be received.</typeparam>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="url">The URL.</param>
        /// <param name="request">The deserialized message, if one is found. <c>Null</c> otherwise.</param>
        /// <returns>
        /// <c>True</c> if the expected message was recognized and deserialized. <c>False</c> otherwise.
        /// </returns>
        /// <exception cref="T:HttpMessageException">Thrown when a request message of an unexpected type is received.</exception>
        public static bool TryReadMessage<TRequest>(this HttpMessageSerializer messageSerializer, Uri url, out TRequest request) where TRequest : class, IHttpProtocolMessage {
            if (messageSerializer == null) {
                throw new ArgumentNullException(nameof(messageSerializer));
            }

            if (url == null) {
                throw new ArgumentNullException(nameof(url));
            }

            var httpRequest = new RedirectHttpRequest(url);
            return messageSerializer.TryReadMessage<TRequest>(httpRequest, out request);
        }

#if NETFRAMEWORK
        /// <summary>
        /// Adds the extra data.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="message">The message.</param>
        /// <param name="query">The query.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static void AddExtraData(this HttpMessageSerializer messageSerializer, IHttpProtocolMessage message, string query) {
            if (messageSerializer == null) {
                throw new ArgumentNullException(nameof(messageSerializer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            var description = messageSerializer.MessageFactory.GetMessageDescription(message.GetType());
            if (description != null) {
                var queryString = HttpUtil.ParseQueryString(query);
                foreach (var key in queryString.AllKeys) {
                    if (description.Mapping.ContainsKey(key)) {
                        throw new InvalidOperationException(string.Format("The parameter '{0}' can not be set.", key));
                    }
                    else {
                        message.ExtraData.Add(key, queryString[key]);
                    }
                }
            }
        }
#endif

        /// <summary>
        /// Encodes the message.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="message">The message.</param>
        /// <returns>
        /// The encoded message dictionary.
        /// </returns>
        public static IDictionary<string, string> EncodeToDictionary(this HttpMessageSerializer messageSerializer, IHttpProtocolMessage message) {
            if (messageSerializer == null) {
                throw new ArgumentNullException(nameof(messageSerializer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            // TODO: check execute payload binding if Data exists.
            var bindings = messageSerializer.OutgoingBindings.Where(b => {
                var t = b.GetType().BaseType;
                return (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(HttpPayloadMessageBinding<>)))
                || typeof(HttpEncodingBinding).IsAssignableFrom(t);
            });

            foreach (var binding in bindings) {
                binding.ProcessOutgoingMessage(message);
            }

            var dictionary = messageSerializer.ToDictionary(message);
            dictionary.Add(BaseUriParameter, message.BaseUri.AbsoluteUri);
            dictionary.Add(HttpVerbParameter, ((int)message.HttpMethods).ToString(CultureInfo.InvariantCulture));

            return dictionary;
        }

        /// <summary>
        /// Decodes the message.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="fields">The encoded message fields.</param>
        /// <returns>
        /// The passive federation request message.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// No base Uri parameter.
        /// or
        /// No HTTP verb parameter.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// Base Uri must be absolute URI.
        /// or
        /// HTTP verb bad format.
        /// </exception>
        public static IHttpProtocolMessage DecodeFromDictionary(this HttpMessageSerializer messageSerializer, IDictionary<string, string> fields) {
            if (messageSerializer == null) {
                throw new ArgumentNullException(nameof(messageSerializer));
            }

            if (fields == null) {
                throw new ArgumentNullException(nameof(fields));
            }

            if (!fields.ContainsKey(BaseUriParameter)) {
                throw new InvalidOperationException("No base Uri parameter.");
            }

            Uri baseUri;
            if (!Uri.TryCreate(fields[BaseUriParameter], UriKind.Absolute, out baseUri)) {
                throw new FormatException("Base Uri must be absolute URI.");
            }

            if (!fields.ContainsKey(HttpVerbParameter)) {
                throw new InvalidOperationException("No HTTP verb parameter.");
            }

            int deliveryMethod;
            if (!int.TryParse(fields[HttpVerbParameter], out deliveryMethod)) {
                throw new FormatException("HTTP verb bad format.");
            }

            fields.Remove(BaseUriParameter);
            fields.Remove(HttpVerbParameter);

            var message = messageSerializer.MessageFactory.CreateMessage(baseUri, (HttpDeliveryMethods)deliveryMethod, fields) as IHttpProtocolMessage;

            // FIX: Restore payload
            if (message != null) {
                var bindings = messageSerializer.IncomingBidnings.Where(b => {
                    var t = b.GetType().BaseType;
                    return (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(HttpPayloadMessageBinding<>)))
                    || typeof(HttpEncodingBinding).IsAssignableFrom(t);
                });

                foreach (var binding in bindings) {
                    binding.ProcessIncomingMessage(message);
                }
            }

            return message;
        }

        /// <summary>
        /// Encodes the context.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="message">The message.</param>
        /// <returns>
        /// The encoded context value.
        /// </returns>
        public static string EncodeContext(this HttpMessageSerializer messageSerializer, IHttpProtocolMessage message) {
            if (messageSerializer == null) {
                throw new ArgumentNullException(nameof(messageSerializer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            var bindings = messageSerializer.OutgoingBindings.Where(b => {
                var t = b.GetType();
                return typeof(HttpEncodingBinding).IsAssignableFrom(t);
            });

            foreach (var binding in bindings) {
                binding.ProcessOutgoingMessage(message);
            }

            var dictionary = messageSerializer.ToDictionary(message);
            dictionary.Add(BaseUriParameter, message.BaseUri.AbsoluteUri);
            dictionary.Add(HttpVerbParameter, ((int)message.HttpMethods).ToString(CultureInfo.InvariantCulture));

            var builder = new StringBuilder(dictionary.Count<KeyValuePair<string, string>>() * 10);
            foreach (KeyValuePair<string, string> pair in dictionary) {
                Debug.Assert(!string.IsNullOrEmpty(pair.Key), "UnexpectedNullOrEmptyKey");

                builder.Append(pair.Key);
                builder.Append(PairDelimiter);
                builder.Append(pair.Value);
                builder.Append(ParamDelimiter);
            }

            builder.Length--;
            return builder.ToString();
        }

        /// <summary>
        /// Decodes the context.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="encodedValue">The encoded value.</param>
        /// <returns>
        /// The passive federation request message.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// No base Uri parameter.
        /// or
        /// No HTTP verb parameter.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// Base Uri must be absolute URI.
        /// or
        /// HTTP verb bad format.
        /// </exception>
        public static IHttpProtocolMessage DecodeContext(this HttpMessageSerializer messageSerializer, string encodedValue) {
            if (messageSerializer == null) {
                throw new ArgumentNullException(nameof(messageSerializer));
            }

            if (encodedValue == null) {
                throw new ArgumentNullException(nameof(encodedValue));
            }

            Dictionary<string, string> fields = FillFromString(encodedValue);

            if (!fields.ContainsKey(BaseUriParameter)) {
                throw new InvalidOperationException("No base Uri parameter."); 
            }

            Uri baseUri;
            if (!Uri.TryCreate(fields[BaseUriParameter], UriKind.Absolute, out baseUri)) {
                throw new FormatException("Base Uri must be absolute URI."); 
            }

            if (!fields.ContainsKey(HttpVerbParameter)) {
                throw new InvalidOperationException("No HTTP verb parameter.");
            }

            int deliveryMethod;
            if (!int.TryParse(fields[HttpVerbParameter], out deliveryMethod)) {
                throw new FormatException("HTTP verb bad format."); 
            }

            fields.Remove(BaseUriParameter);
            fields.Remove(HttpVerbParameter);

            var message = messageSerializer.MessageFactory.CreateMessage(baseUri, (HttpDeliveryMethods)deliveryMethod, fields) as IHttpProtocolMessage;

            // FIX: Restore payload
            if (message != null) {
                var bindings = messageSerializer.IncomingBidnings.Where(b => {
                    var t = b.GetType().BaseType;
                    return (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(HttpPayloadMessageBinding<>))) 
                    || typeof(HttpEncodingBinding).IsAssignableFrom(t);
                });

                foreach (var binding in bindings) {
                    binding.ProcessIncomingMessage(message);
                }
            }

            return message;
        }

        private static Dictionary<string, string> FillFromString(string s) {
            var dictionary = new Dictionary<string, string>();

            int num = (s != null) ? s.Length : 0;

            for (int i = 0; i < num; i++) {
                int startIndex = i;
                int index = -1;
                while (i < num) {
                    char ch = s[i];
                    if (ch == PairDelimiter) {
                        if (index < 0) {
                            index = i;
                        }
                    }
                    else if (ch == ParamDelimiter) {
                        break;
                    }

                    i++;
                }

                string key = null;
                string value;
                if (index >= 0) {
                    key = s.Substring(startIndex, index - startIndex);
                    value = s.Substring(index + 1, (i - index) - 1);
                }
                else {
                    value = s.Substring(startIndex, i - startIndex);
                }

                dictionary.Add(key, value);

                if ((i == (num - 1)) && (s[i] == ParamDelimiter)) {
                    dictionary.Add(null, string.Empty);
                }
            }

            return dictionary;
        }

        private class PostHttpResponse : HttpResponseBase {
#if NETFRAMEWORK
            private readonly StringBuilder stringBuilder = new StringBuilder();

            public string Html {
                get {
                    return stringBuilder.ToString();
                }
            }

            public override HttpCachePolicyBase Cache => new HttpCachePolicy();

            public override void Write(string s) {
                stringBuilder.Append(s);
            }

            public override void End() {
                // do nothing
            }

            private class HttpCachePolicy : HttpCachePolicyBase {
                public override void SetMaxAge(TimeSpan delta) {
                    // do nothing
                }

                public override void SetProxyMaxAge(TimeSpan delta) {
                    // do nothing
                }

                public override void SetCacheability(HttpCacheability cacheability) {
                    // do nothing
                }

                public override void SetNoStore() {
                    // do nothing
                }
            }
#endif
#if NETCOREAPP1_0_OR_GREATER
            private readonly MemoryStream stream = new MemoryStream();

            public string Html {
                get {
                    return Encoding.UTF8.GetString(stream.ToArray());
                }
            }

            public override void Redirect(string location, bool permanent) {
                throw new NotImplementedException();
            }

            public override HttpContextBase HttpContext {
                get {
                    throw new NotImplementedException();
                }
            }

            public override int StatusCode {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override IHeaderDictionary Headers {
                get {
                    return new HeaderDictionary();
                }
            }

            public override Stream Body {
                get {
                    return stream;
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override long? ContentLength {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override string ContentType {
                get {
                    throw new NotImplementedException();
                }

                set {
                    // do noting
                }
            }

            public override IResponseCookies Cookies {
                get {
                    throw new NotImplementedException();
                }
            }

            public override bool HasStarted {
                get {
                    throw new NotImplementedException();
                }
            }

            public override void OnStarting(Func<object, Task> callback, object state) {
                throw new NotImplementedException();
            }

            public override void OnCompleted(Func<object, Task> callback, object state) {
                throw new NotImplementedException();
            }

            private class HeaderDictionary : IHeaderDictionary {
                public StringValues this[string key] {
                    get {
                        throw new NotImplementedException();
                    }
                    set {
                        // ignore
                    }
                }

                public long? ContentLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

                public ICollection<string> Keys => throw new NotImplementedException();

                public ICollection<StringValues> Values => throw new NotImplementedException();

                public int Count => throw new NotImplementedException();

                public bool IsReadOnly => throw new NotImplementedException();

                public void Add(string key, StringValues value) {
                    throw new NotImplementedException();
                }

                public void Add(KeyValuePair<string, StringValues> item) {
                    throw new NotImplementedException();
                }

                public void Clear() {
                    throw new NotImplementedException();
                }

                public bool Contains(KeyValuePair<string, StringValues> item) {
                    throw new NotImplementedException();
                }

                public bool ContainsKey(string key) {
                    throw new NotImplementedException();
                }

                public void CopyTo(KeyValuePair<string, StringValues>[] array, int arrayIndex) {
                    throw new NotImplementedException();
                }

                public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator() {
                    throw new NotImplementedException();
                }

                public bool Remove(string key) {
                    throw new NotImplementedException();
                }

                public bool Remove(KeyValuePair<string, StringValues> item) {
                    throw new NotImplementedException();
                }

                public bool TryGetValue(string key, [MaybeNullWhen(false)] out StringValues value) {
                    throw new NotImplementedException();
                }

                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                    throw new NotImplementedException();
                }
            }
#endif
        }

        private class RedirectHttpResponse : HttpResponseBase {
            public string RequestUrl { get; private set; }

            public override void Redirect(string url, bool endResponse) {
                this.Redirect(url);
            }

            public override void Redirect(string url) {
                this.RequestUrl = url;
            }

#if NETCOREAPP1_0_OR_GREATER
            public override HttpContextBase HttpContext {
                get {
                    throw new NotImplementedException();
                }
            }

            public override int StatusCode {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override IHeaderDictionary Headers {
                get {
                    throw new NotImplementedException();
                }
            }

            public override Stream Body {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override long? ContentLength {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override string ContentType {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override IResponseCookies Cookies {
                get {
                    throw new NotImplementedException();
                }
            }

            public override bool HasStarted {
                get {
                    throw new NotImplementedException();
                }
            }

            public override void OnStarting(Func<object, Task> callback, object state) {
                throw new NotImplementedException();
            }

            public override void OnCompleted(Func<object, Task> callback, object state) {
                throw new NotImplementedException();
            }
#endif
        }

        private class RedirectHttpRequest : HttpRequestBase {
            private readonly Uri url;

            public RedirectHttpRequest(Uri url) {
                this.url = url ?? throw new ArgumentNullException(nameof(url));
            }

#if NETFRAMEWORK
            public override Uri Url {
                get { return this.url; }
            }

            public override NameValueCollection Form {
                get { return new NameValueCollection(); }
            }

            public override NameValueCollection QueryString {
                get {
                    return HttpUtil.ParseQueryString(url.Query);
                }
            }

            public override string HttpMethod {
                get { return "GET"; }
            }
#else 
            public override string Method {
                get {
                    return "GET";
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override HttpContextBase HttpContext {
                get {
                    throw new NotImplementedException();
                }
            }

            public override string Scheme {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override bool IsHttps {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override HostString Host {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override PathString PathBase {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override PathString Path {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override QueryString QueryString {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override IQueryCollection Query {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override string Protocol {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override IHeaderDictionary Headers {
                get {
                    throw new NotImplementedException();
                }
            }

            public override IRequestCookieCollection Cookies {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override long? ContentLength {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override string ContentType {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override Stream Body {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override bool HasFormContentType {
                get {
                    throw new NotImplementedException();
                }
            }

            public override IFormCollection Form {
                get {
                    throw new NotImplementedException();
                }

                set {
                    throw new NotImplementedException();
                }
            }

            public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default(CancellationToken)) {
                throw new NotImplementedException();
            }

#endif
        }
    }
}
