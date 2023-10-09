// ----------------------------------------------------------------------------
// <copyright file="HttpMessageSerializer.cs" company="ABC Software Ltd">
//    Copyright © 2010-2019 ABC Software Ltd. All rights reserved.
//
//    This library is free software; you can redistribute it and/or.
//    modify it under the terms of the GNU Lesser General Public
//    License  as published by the Free Software Foundation, either
//    version 3 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with the library. If not, see http://www.gnu.org/licenses/.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Http {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
#if NETCOREAPP1_0_OR_GREATER
    using HttpRequestBase = Microsoft.AspNetCore.Http.HttpRequest;
    using HttpResponseBase = Microsoft.AspNetCore.Http.HttpResponse;
    using HttpContextBase = Microsoft.AspNetCore.Http.HttpContext;
    using System.Threading.Tasks;
#endif
#if NETFRAMEWORK
    using System.Web;
#endif

    /// <summary>
    /// Manages sending messages to a remote party and receiving responses.
    /// </summary>
    public abstract class HttpMessageSerializer {
        private readonly List<IHttpBinding> outgoingBindings;
        private readonly List<IHttpBinding> incomingBindings;
        private readonly HttpMessageFactory messageFactory;
        private int maximumRedirectUrlLength = 2048;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessageSerializer"/> class.
        /// </summary>
        /// <param name="factory">
        /// A class prepared to analyze incoming messages and indicate what concrete
        /// message types can deserialize from it.
        /// </param>
        /// <param name="bindings">The binding elements to use in sending and receiving messages.</param>
        protected HttpMessageSerializer(HttpMessageFactory factory, params IHttpBinding[] bindings) {
            if (factory == null) {
                throw new ArgumentNullException(nameof(factory));
            }

            if (bindings == null) {
                throw new ArgumentNullException(nameof(bindings));
            }

            this.outgoingBindings = new List<IHttpBinding>(bindings);
            this.incomingBindings = new List<IHttpBinding>(this.outgoingBindings);
            this.incomingBindings.Reverse();
            this.messageFactory = factory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessageSerializer" /> class.
        /// </summary>
        /// <param name="messageTypes">The message types that might be encountered.</param>
        /// <param name="bindings">The binding elements to apply to the channel.</param>
        protected HttpMessageSerializer(ICollection<Type> messageTypes, params IHttpBinding[] bindings)
            : this(new HttpMessageFactory(), bindings) {
            if (messageTypes == null) {
                throw new ArgumentNullException(nameof(messageTypes));
            }

            foreach (var messageType in messageTypes) {
                this.messageFactory.AddMessageType(messageType);
            }
        }

        /// <summary>
        /// Gets or sets the maximum allowable size for a 301 Redirect response before we send
        /// a 200 OK response with a scripted form POST with the parameters instead
        /// in order to ensure successfully sending a large payload to another server
        /// that might have a maximum allowable size restriction on its GET request.
        /// </summary>
        /// <value>The default value is 2048.</value>
        public int MaximumRedirectUrlLength {
            get {
                return this.maximumRedirectUrlLength;
            }

            set {
                if (value < 500 || value > 4096) {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.maximumRedirectUrlLength = value;
            }
        }

        internal HttpMessageFactory MessageFactory {
            get { 
                return this.messageFactory; 
            }
        }

        internal ReadOnlyCollection<IHttpBinding> IncomingBidnings {
            get {
                return new ReadOnlyCollection<IHttpBinding>(this.incomingBindings);
            }
        }

        internal ReadOnlyCollection<IHttpBinding> OutgoingBindings {
            get {
                return new ReadOnlyCollection<IHttpBinding>(this.outgoingBindings);
            }
        }

        /// <summary>
        /// Gets the protocol message embedded in the given HTTP request.
        /// </summary>
        /// <typeparam name="TRequest">The expected type of the message to be received.</typeparam>
        /// <param name="httpRequest">The request to search for an embedded message.</param>
        /// <returns>The deserialized message.  Never null.</returns>
        /// <exception cref="T:HttpMessageException">Thrown if the expected message was not recognized in the response.</exception>
        public TRequest ReadMessage<TRequest>(HttpRequestBase httpRequest)
            where TRequest : class, IHttpProtocolMessage
        {
            if (httpRequest == null) {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            TRequest message;
            if (!this.TryReadMessage<TRequest>(httpRequest, out message)) {
                throw new HttpMessageException("UnexpectedMessageReceived.");
            }

            return message;
        }

#if NETFRAMEWORK
        /// <summary>
        /// Gets the protocol message embedded in the current HTTP request.
        /// </summary>
        /// <typeparam name="TRequest">The expected type of the message to be received.</typeparam>
        /// <returns>The deserialized message.  Never null.</returns>
        /// <remarks>
        /// Requires an HttpContext.Current context.
        /// </remarks>
        /// <exception cref="T:System.InvalidOperationException">Thrown when <see cref="P:System.Web.HttpContext.Current" /> is null.</exception>
        /// <exception cref="T:HttpMessageException">Thrown if the expected message was not recognized in the response.</exception>
        public TRequest ReadMessage<TRequest>()
            where TRequest : class, IHttpProtocolMessage
        {
            return this.ReadMessage<TRequest>(this.GetRequestFromContext());
        }
#endif

        /// <summary>
        /// Gets the protocol message embedded in the given HTTP request, if present.
        /// </summary>
        /// <returns>The deserialized message, if one is found.  Null otherwise.</returns>
        public IHttpProtocolMessage ReadMessage(HttpRequestBase httpRequest) {
            if (httpRequest == null) {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            return this.ReadMessageCore(httpRequest);
        }

#if NETFRAMEWORK
        /// <summary>
        /// Gets the protocol message embedded in the given HTTP request, if present.
        /// </summary>
        /// <returns>The deserialized message, if one is found.  Null otherwise.</returns>
        /// <remarks>
        /// Requires an HttpContext.Current context.
        /// </remarks>
        /// <exception cref="T:System.InvalidOperationException">Thrown when <see cref="P:System.Web.HttpContext.Current" /> is null.</exception>
        public IHttpProtocolMessage ReadMessage() {
            return this.ReadMessage(this.GetRequestFromContext());
        }
#endif

        /// <summary>
        /// Gets the protocol message embedded in the given HTTP request, if present.
        /// </summary>
        /// <typeparam name="TRequest">The expected type of the message to be received.</typeparam>
        /// <param name="httpRequest">The request to search for an embedded message.</param>
        /// <param name="request">The deserialized message, if one is found.  Null otherwise.</param>
        /// <returns>True if the expected message was recognized and deserialized.  False otherwise.</returns>
        /// <exception cref="T:HttpMessageException">Thrown when a request message of an unexpected type is received.</exception>
        public bool TryReadMessage<TRequest>(HttpRequestBase httpRequest, out TRequest request)
            where TRequest : class, IHttpProtocolMessage
        {
            if (httpRequest == null) {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            var message = this.ReadMessageCore(httpRequest);
            if (message == null) {
                request = default(TRequest);
                return false;
            }

            request = message as TRequest;

            // ErrorUtilities.VerifyProtocol(((TRequest)request) != null, MessagingStrings.UnexpectedMessageReceived, new object[] { typeof(TRequest), message.GetType() });
            if (request == null) {
                throw new HttpMessageException("UnexpectedMessageReceived");
            }

            return true;
        }

#if NETFRAMEWORK
        /// <summary>
        /// Gets the protocol message embedded in the given HTTP request, if present.
        /// </summary>
        /// <typeparam name="TRequest">The expected type of the message to be received.</typeparam>
        /// <param name="request">The deserialized message, if one is found.  Null otherwise.</param>
        /// <returns>True if the expected message was recognized and deserialized.  False otherwise.</returns>
        /// <remarks>
        /// Requires an HttpContext.Current context.
        /// </remarks>
        /// <exception cref="T:System.InvalidOperationException">Thrown when <see cref="P:System.Web.HttpContext.Current" /> is null.</exception>
        /// <exception cref="T:HttpMessageException">Thrown when a request message of an unexpected type is received.</exception>
        public bool TryReadMessage<TRequest>(out TRequest request)
            where TRequest : class, IHttpProtocolMessage
        {
            return this.TryReadMessage<TRequest>(this.GetRequestFromContext(), out request);
        }
#endif

        /// <summary>
        /// Sends an indirect message (either a request or response)
        /// or direct message response for transmission to a remote party
        /// and ends execution on the current page or handler.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="response">The one-way message to send</param>
        public void SendMessage(HttpResponseBase httpResponse, IHttpProtocolMessage response) {
            if (httpResponse == null) {
                throw new ArgumentNullException(nameof(httpResponse));
            }

            if (response == null) {
                throw new ArgumentNullException(nameof(response));
            }

            SendMessageCore(httpResponse, response);
        }

#if NETCOREAPP1_0_OR_GREATER
        /// <summary>
        /// Asynchronously sends an indirect message (either a request or response)
        /// or direct message response for transmission to a remote party
        /// and ends execution on the current page or handler.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="response">The one-way message to send</param>
        public Task SendMessageAsync(HttpResponseBase httpResponse, IHttpProtocolMessage response) {
            if (httpResponse == null) {
                throw new ArgumentNullException(nameof(httpResponse));
            }

            if (response == null) {
                throw new ArgumentNullException(nameof(response));
            }

            return SendMessageCoreAsync(httpResponse, response);
        }
#endif

#if NETFRAMEWORK
        /// <summary>
        /// Sends an indirect message (either a request or response) 
        /// or direct message response for transmission to a remote party
        /// and ends execution on the current page or handler.
        /// </summary>
        /// <param name="response">The one-way message to send</param>
        /// <remarks>
        /// Requires an HttpContext.Current context.
        /// </remarks>
        public void SendMessage(IHttpProtocolMessage response) {
            if (response == null) {
                throw new ArgumentNullException(nameof(response));
            }

            SendMessage(this.GetResponseFromContext(), response);
        }
#endif

#if NETFRAMEWORK
        /// <summary>
        /// Gets the current HTTP request being processed.
        /// </summary>
        /// <returns>The HttpRequestInfo for the current request.</returns>
        /// <remarks>
        /// Requires an <see cref="P:System.Web.HttpContext.Current" /> context.
        /// </remarks>
        /// <exception cref="T:System.InvalidOperationException">Thrown if <see cref="P:System.Web.HttpContext.Current">HttpContext.Current</see> == <c>null</c>.</exception>
        protected internal virtual HttpRequestBase GetRequestFromContext() {
            if (HttpContext.Current == null || HttpContext.Current.Request == null) {
                throw new InvalidOperationException("HttpContext or HttpRequest is null.");
            }

            return new HttpRequestWrapper(HttpContext.Current.Request);
        }

        /// <summary>
        /// Gets the current HTTP response being processed.
        /// </summary>
        /// <returns>The HttpResponseInfo for the current request.</returns>
        /// <remarks>
        /// Requires an <see cref="P:System.Web.HttpContext.Current" /> context.
        /// </remarks>
        /// <exception cref="T:System.InvalidOperationException">Thrown if <see cref="P:System.Web.HttpContext.Current">HttpContext.Current</see> == <c>null</c>.</exception>
        protected internal virtual HttpResponseBase GetResponseFromContext() {
            if (HttpContext.Current == null || HttpContext.Current.Request == null) {
                throw new InvalidOperationException("HttpContext or HttpRequest is null.");
            }

            return new HttpResponseWrapper(HttpContext.Current.Response);
        }
#endif

        /// <summary>
        /// Writes the form post.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The encoded HTTP response.</returns>
        protected virtual string WriteFormPost(IHttpProtocolMessage message) {
            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            Uri frameSourceUri = null;
            if (message.ExtraData.ContainsKey("FrameSourceUrl")
                && Uri.TryCreate(message.ExtraData["FrameSourceUrl"], UriKind.Absolute, out frameSourceUri)) {
                message.ExtraData.Remove("FrameSourceUrl");

                var baseUri = HttpUtil.CreateQueryString(message.BaseUri, this.ToDictionary(message));
                return HttpUtil.CreateRedirectPostForm(new Uri(baseUri), frameSourceUri);
            }

            return HttpUtil.CreatePostForm(message.BaseUri, this.ToDictionary(message));
        }

        /// <summary>
        /// Writes the query string.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The encoded HTTP response.</returns>
        protected virtual string WriteQueryString(IHttpProtocolMessage message) {
            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            return HttpUtil.CreateQueryString(message.BaseUri, this.ToDictionary(message));
        }

        /// <summary>
        /// Automatics the dictionary.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">No found description</exception>
        protected internal virtual IDictionary<string, string> ToDictionary(IHttpMessage message) {
            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            var dictionary = new Dictionary<string, string>();

            var description = messageFactory.GetMessageDescription(message.GetType());
            if (description == null) {
                throw new NotSupportedException("No found description");
            }

            foreach (KeyValuePair<string, HttpMessagePart> pair in description.Mapping) {
                HttpMessagePart part = pair.Value;
                if (part.IsStaticConstantValue || part.IsRequired || /*part.AllowEmpty || */part.IsNondefaultValueSet(message)) {
                    dictionary.Add(part.Name, part.GetValue(message));
                }
            }

            foreach (KeyValuePair<string, string> pair in message.ExtraData) {
                dictionary.Add(pair.Key, pair.Value);
            }

            return dictionary;
        }

        /// <summary>
        /// Gets the protocol message that may be embedded in the given HTTP request.
        /// </summary>
        /// <param name="httpRequest">The request to search for an embedded message.</param>
        /// <returns>The deserialized message, if one is found.  Null otherwise.</returns>
        protected virtual IHttpProtocolMessage ReadMessageCore(HttpRequestBase httpRequest) {
            if (httpRequest == null) {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            Dictionary<string, string> fields;
            HttpDeliveryMethods deliveryMehtod;
            string httpMethod;
#if NETCOREAPP1_0_OR_GREATER
            httpMethod = httpRequest.Method;
            fields = new Dictionary<string, string>();
            if (httpRequest.HasFormContentType) {
                deliveryMehtod = HttpDeliveryMethods.PostRequest;
                foreach (var item in httpRequest.Form) {
                    fields.Add(item.Key, item.Value);
                }
            }
            else {
                deliveryMehtod = HttpDeliveryMethods.GetRequest;
                foreach (var item in httpRequest.Query) {
                    fields.Add(item.Key, item.Value);
                }
            }
#endif
#if NETFRAMEWORK
            httpMethod = httpRequest.HttpMethod;
            fields = httpRequest.Form.ToDictionary();
            deliveryMehtod = HttpDeliveryMethods.PostRequest;
            if (fields.Count == 0 && httpMethod != "POST") {
                fields = httpRequest.QueryString.ToDictionary(); // Before rewriting
                deliveryMehtod = HttpDeliveryMethods.GetRequest;
            }
#endif

            Uri baseUri;
            /*
            try {
                baseUri = request.GetPublicFacingUrl();
            }
            catch (ArgumentException exception) {
                // Logger.Messaging.WarnFormat("Unrecognized HTTP request: {0}", exception);
                return null;
            }
             */
#if NETCOREAPP1_0_OR_GREATER
            string path = (httpRequest.PathBase.HasValue || httpRequest.Path.HasValue) ? (httpRequest.PathBase + httpRequest.Path).ToString() : "/";
            baseUri = new Uri(httpRequest.Scheme + "://" + httpRequest.Host + path);
#endif
#if NETFRAMEWORK
            baseUri = new Uri(httpRequest.Url.GetLeftPart(UriPartial.Path));
#endif

            var message = messageFactory.CreateMessage(baseUri, deliveryMehtod, fields) as IHttpProtocolMessage;
            if (message != null) {
                if ((message.HttpMethods & deliveryMehtod) == HttpDeliveryMethods.None) {
                    throw new HttpMessageException(string.Format("'{0}' messages cannot be received with HTTP verb '{1}'.", message.GetType().Name, httpMethod));
                }

                this.ProcessIncomingMessage(message);
            }

            return message;
        }

        /// <summary>
        /// Verifies the integrity and applicability of an incoming message.
        /// </summary>
        /// <param name="message">The message just received.</param>
        /// <exception cref="T:DotNetOpenAuth.Messaging.ProtocolException">
        /// Thrown when the message is somehow invalid.
        /// This can be due to tampering, replay attack or expiration, among other things.
        /// </exception>
        protected virtual void ProcessIncomingMessage(IHttpProtocolMessage message) {
            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            foreach (var binding in this.incomingBindings) {
                binding.ProcessIncomingMessage(message);
            }

            var messageWithEvents = message as IHttpMessageWithEvents;
            if (messageWithEvents != null) {
                messageWithEvents.OnReceiving();
            }

            message.Validate();
        }

        /// <summary>
        /// Prepares a message for transmit by applying signatures, nonces, etc.
        /// </summary>
        /// <param name="message">The message to prepare for sending.</param>
        /// <remarks>
        /// This method should NOT be called by derived types
        /// except when sending ONE WAY request messages.
        /// </remarks>
        protected virtual void ProcessOutgoingMessage(IHttpProtocolMessage message) {
            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            var messageWithEvents = message as IHttpMessageWithEvents;
            if (messageWithEvents != null) {
                messageWithEvents.OnSending();
            }

            foreach (var binding in this.outgoingBindings) {
                binding.ProcessOutgoingMessage(message);
            }

            // Validate message parts
            var decription = this.messageFactory.GetMessageDescription(message.GetType());
            if (decription == null) {
                throw new NotSupportedException("Not found description");
            }

            decription.EnsureMessagePartsPassBasicValidation(this.ToDictionary(message));

            // Validate message
            message.Validate();
        }

        /// <summary>
        /// Sends an indirect message (either a request or response)
        /// or direct message response for transmission to a remote party
        /// and ends execution on the current page or handler.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="response">The one-way message to send.</param>
        protected virtual void SendMessageCore(HttpResponseBase httpResponse, IHttpProtocolMessage response) {
            if (httpResponse == null) {
                throw new ArgumentNullException(nameof(httpResponse));
            }

            if (response == null) {
                throw new ArgumentNullException(nameof(response));
            }

            this.ProcessOutgoingMessage(response);

            bool frame = response.ExtraData.ContainsKey("FrameSourceUrl");
            bool flag = false;
            if (!frame && (response.HttpMethods & HttpDeliveryMethods.GetRequest) == HttpDeliveryMethods.GetRequest) {
                var redirectUrl = this.WriteQueryString(response);

                if (redirectUrl.Length >= this.MaximumRedirectUrlLength) {
                    if ((response.HttpMethods & HttpDeliveryMethods.PostRequest) != HttpDeliveryMethods.PostRequest) {
                        throw new HttpMessageException("Message too large for a HTTP GET, and HTTP POST is not allowed for this message type.");
                    }
                }
                else {
#if NETFRAMEWORK
                    try {
                        httpResponse.Redirect(redirectUrl, true); // UNDONE: 
                    }
                    catch (System.Threading.ThreadAbortException) {
                        // To suppress ThreadAbortException
                    }
#endif
#if NETCOREAPP1_0_OR_GREATER
                    httpResponse.Redirect(redirectUrl, false);
#endif

                    flag = true;
                }
            }

            if (frame || (!flag && (response.HttpMethods & HttpDeliveryMethods.PostRequest) == HttpDeliveryMethods.PostRequest)) {
                var postForm = this.WriteFormPost(response);

#if NETFRAMEWORK
                httpResponse.Cache.SetMaxAge(TimeSpan.Zero);
                httpResponse.Cache.SetProxyMaxAge(TimeSpan.Zero);
                httpResponse.Cache.SetNoStore();
                httpResponse.Cache.SetCacheability(HttpCacheability.NoCache);
                httpResponse.Write(postForm);
                httpResponse.End(); // Flush called in End Method
#endif
#if NETCOREAPP1_0_OR_GREATER
                httpResponse.Headers["Cache-Control"] = "no-cache, no-store, max-age=0, s-maxage=0";
                httpResponse.ContentType = "text/html; charset=UTF-8";
                var bytes = System.Text.Encoding.UTF8.GetBytes(postForm);
                httpResponse.Body.Write(bytes, 0, bytes.Length);
#endif
            }
        }

#if NETCOREAPP1_0_OR_GREATER
        /// <summary>
        /// Asynchronously sends an indirect message (either a request or response)
        /// or direct message response for transmission to a remote party
        /// and ends execution on the current page or handler.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="response">The one-way message to send.</param>
        protected virtual async Task SendMessageCoreAsync(HttpResponseBase httpResponse, IHttpProtocolMessage response) {
            this.ProcessOutgoingMessage(response);

            bool frame = response.ExtraData.ContainsKey("FrameSourceUrl");
            bool flag = false;
            if (!frame && (response.HttpMethods & HttpDeliveryMethods.GetRequest) == HttpDeliveryMethods.GetRequest) {
                var redirectUrl = this.WriteQueryString(response);

                if (redirectUrl.Length >= this.MaximumRedirectUrlLength) {
                    if ((response.HttpMethods & HttpDeliveryMethods.PostRequest) != HttpDeliveryMethods.PostRequest) {
                        throw new HttpMessageException("Message too large for a HTTP GET, and HTTP POST is not allowed for this message type.");
                    }
                }
                else {
                    httpResponse.Redirect(redirectUrl, false);
                    flag = true;
                }
            }

            if (frame || (!flag && (response.HttpMethods & HttpDeliveryMethods.PostRequest) == HttpDeliveryMethods.PostRequest)) {
                var postForm = this.WriteFormPost(response);

                httpResponse.Headers["Cache-Control"] = "no-cache, no-store, max-age=0, s-maxage=0";
                httpResponse.ContentType = "text/html; charset=UTF-8";
                var bytes = System.Text.Encoding.UTF8.GetBytes(postForm);
                await httpResponse.Body.WriteAsync(bytes, 0, bytes.Length);
                await httpResponse.Body.FlushAsync();
            }
        }
#endif
    }
}