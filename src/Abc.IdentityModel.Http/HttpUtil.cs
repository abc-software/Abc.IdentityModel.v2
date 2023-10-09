// ----------------------------------------------------------------------------
// <copyright file="HttpUtil.cs" company="ABC Software Ltd">
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
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text;
#if NETCOREAPP1_0_OR_GREATER
    using HttpRequestBase = Microsoft.AspNetCore.Http.HttpRequest;
    using HttpResponseBase = Microsoft.AspNetCore.Http.HttpResponse;
    using HttpContextBase = Microsoft.AspNetCore.Http.HttpContext;
#endif
#if NETFRAMEWORK
    using System.Collections.Specialized;
    using System.Web;
#endif

    /// <summary>
    /// The HTTP utilities.
    /// </summary>
    internal static class HttpUtil {
#if NETFRAMEWORK
        /// <summary>
        /// Converts a <see cref="T:System.Collections.Specialized.NameValueCollection"/> to an IDictionary&lt;string, string&gt;.
        /// </summary>
        /// <param name="nvc">The NameValueCollection to convert.  May be null.</param>
        /// <returns>
        /// The generated dictionary, or null if <paramref name="nvc"/> is null.
        /// </returns>
        /// <remarks>
        /// If a <c>null</c> key is encountered, its value is ignored since
        /// <c>Dictionary&lt;string, string&gt;</c> does not allow null keys.
        /// </remarks>
        internal static Dictionary<string, string> ToDictionary(this NameValueCollection nvc) {
            if (nvc == null) {
                throw new ArgumentNullException(nameof(nvc));
            }

            return nvc.ToDictionary(false);
        }

        /// <summary>
        /// Converts a <see cref="T:System.Collections.Specialized.NameValueCollection"/> to an IDictionary&lt;string, string&gt;.
        /// </summary>
        /// <param name="nvc">The NameValueCollection to convert.  May be null.</param>
        /// <param name="throwOnNullKey">A value indicating whether a null key in the <see cref="T:System.Collections.Specialized.NameValueCollection"/> should be silently skipped since it is not a valid key in a Dictionary.
        /// Use <c>true</c> to throw an exception if a null key is encountered.
        /// Use <c>false</c> to silently continue converting the valid keys.</param>
        /// <returns>
        /// The generated dictionary, or null if <paramref name="nvc"/> is null.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">Thrown if <paramref name="throwOnNullKey"/> is <c>true</c> and a null key is encountered.</exception>
        internal static Dictionary<string, string> ToDictionary(this NameValueCollection nvc, bool throwOnNullKey) {
            if (nvc == null) {
                throw new ArgumentNullException(nameof(nvc));
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string str in nvc) {
                if (str == null) {
                    if (throwOnNullKey) {
                        throw new ArgumentException("UnexpectedNullKey");
                    }

                    if (!string.IsNullOrEmpty(nvc[str])) {
                        // Logger.OpenId.WarnFormat("Null key with value {0} encountered while translating NameValueCollection to Dictionary.", nvc[str]);
                    }

                    continue;
                }

                dictionary.Add(str, nvc[str]);
            }

            return dictionary;
        }

        /// <summary>
        /// Gets the query data from the original request (before any URL rewriting has occurred.)
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// A <see cref="T:System.Collections.Specialized.NameValueCollection" /> containing all the parameters in the query string.
        /// </returns>
        internal static NameValueCollection GetQueryStringBeforeRewriting(this HttpRequestBase request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }

            Debug.Assert(request.ServerVariables == null, "Server variables is null.");

            Uri publicFacingUrl = request.GetPublicFacingUrl();
            if (publicFacingUrl == request.Url) {
                return request.QueryString;
            }
            
            Debug.Assert(publicFacingUrl != null, "UrlBeforeRewriting is null, so the query string cannot be determined.");

            return HttpUtility.ParseQueryString(publicFacingUrl.Query);
        }

        /// <summary>
        /// Gets the public facing URL for the given incoming HTTP request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The URI that the outside world used to create this request.
        /// </returns>
        internal static Uri GetPublicFacingUrl(this HttpRequestBase request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }

            Debug.Assert(request.ServerVariables == null, "Server variables is null.");

            var serverVariables = request.ServerVariables;
            if (serverVariables["HTTP_HOST"] == null) {
                return new Uri(request.Url, request.RawUrl);
            }

            Debug.Assert(request.Url.Scheme == Uri.UriSchemeHttps || request.Url.Scheme == Uri.UriSchemeHttp, "Only HTTP and HTTPS are supported protocols.");
            
            string str = serverVariables["HTTP_X_FORWARDED_PROTO"] ?? request.Url.Scheme;
            Uri uri = new Uri(str + Uri.SchemeDelimiter + serverVariables["HTTP_HOST"]);
            return new UriBuilder(request.Url) { Scheme = str, Host = uri.Host, Port = uri.Port }.Uri;
        }

        /// <summary>
        /// Parses a query string into a <see cref="NameValueCollection"/> using UTF8 encoding.
        /// </summary>
        /// <param name="query">The query string to parse.</param>
        /// <returns>A <see cref="NameValueCollection"/> of query parameters and values.</returns>
        internal static NameValueCollection ParseQueryString(string query) {
            if (query == null) {
                throw new ArgumentNullException(nameof(query));
            }

            return HttpUtility.ParseQueryString(query);
        }
#endif

        /// <summary>
        /// Concatenates a list of name-value pairs as key=value&amp;key=value,
        /// taking care to properly encode each key and value for URL
        /// transmission according to RFC 3986.  
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="args">The dictionary of key/values to read from.</param>
        /// <returns>
        /// The formulated query string style string.
        /// </returns>
        internal static string CreateQueryString(Uri baseUri, IEnumerable<KeyValuePair<string, string>> args) {
            if (baseUri == null) {
                throw new ArgumentNullException(nameof(baseUri));
            }

            if (args == null) {
                throw new ArgumentNullException(nameof(args));
            }

            if (!baseUri.IsAbsoluteUri) {
                throw new ArgumentException("Must be absolute Uri.", nameof(baseUri));
            }

            if (!args.Any<KeyValuePair<string, string>>()) {
                return string.Empty;
            }

            var builder = new StringBuilder(args.Count<KeyValuePair<string, string>>() * 10);
            builder.Append(baseUri.OriginalString);
            builder.Append('?');

            foreach (KeyValuePair<string, string> pair in args) {
                Debug.Assert(!string.IsNullOrEmpty(pair.Key), "UnexpectedNullOrEmptyKey");
                Debug.Assert(pair.Value != null, "UnexpectedNullValue");

                builder.Append(UrlEncode(pair.Key));
                builder.Append('=');
                builder.Append(UrlEncode(pair.Value));
                builder.Append('&');
            }

            builder.Length--;
            return builder.ToString();
        }

        /// <summary>
        /// Creates the post form.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The formulated POST form style string.
        /// </returns>
        internal static string CreatePostForm(Uri baseUri, IEnumerable<KeyValuePair<string, string>> args) {
            if (baseUri == null) {
                throw new ArgumentNullException(nameof(baseUri));
            }

            if (args == null) {
                throw new ArgumentNullException(nameof(args));
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("<!DOCTYPE html><html>");
            builder.AppendFormat(CultureInfo.InvariantCulture, "<head><title>{0}</title><style>.load {{ {1} }}</style></head>", SR.HtmlPostTitle, SR.HtmlPostStyle);
            builder.AppendFormat(CultureInfo.InvariantCulture, "<body><form class=\"load\" method=\"POST\" name=\"hiddenform\" action=\"{0}\">", HtmlAttributeEncode(baseUri.OriginalString));
            foreach (KeyValuePair<string, string> pair in args) {
                builder.AppendFormat(CultureInfo.InvariantCulture, "<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", HtmlAttributeEncode(pair.Key), HtmlAttributeEncode(pair.Value));
            }

            builder.AppendFormat(CultureInfo.InvariantCulture, "<noscript><p>{0}</p><input type=\"submit\" value=\"{1}\" /></noscript>", SR.HtmlPostNoScriptMessage, SR.HtmlPostNoScriptButtonText);
            builder.Append("</form>");
            builder.Append("<script>window.setTimeout(function() {document.forms[0].submit();}, 0);</script>");
            builder.Append("</body></html>");
            return builder.ToString();
        }

        /// <summary>
        /// Creates the redirect post form.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="frameSourceUri">The frame URI.</param>
        /// <returns>
        /// The formulated POST form style string.
        /// </returns>
        internal static string CreateRedirectPostForm(Uri baseUri, Uri frameSourceUri) {
            if (baseUri == null) {
                throw new ArgumentNullException(nameof(baseUri));
            }

            if (frameSourceUri is null) {
                throw new ArgumentNullException(nameof(frameSourceUri));
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("<!DOCTYPE html><html>");
            builder.AppendFormat(CultureInfo.InvariantCulture, "<head><title>{0}</title><style>.load {{ {1} }}</style></head>", SR.HtmlPostTitle, SR.HtmlPostStyle);
            builder.AppendFormat(CultureInfo.InvariantCulture, "<body onload=\"document.location='{0}';\">", HtmlAttributeEncode(baseUri.OriginalString));
            builder.Append("<div class=\"load\"></div>");
            builder.AppendFormat(CultureInfo.InvariantCulture, "<iframe src=\"{0}\" style=\"visibility: hidden; width: 1px; height: 1px;\"></iframe>", HtmlAttributeEncode(frameSourceUri.OriginalString));
            builder.Append("</body></html>");
            return builder.ToString();
        }

        /* Customize POST form
        internal static string CreatePostForm(Uri baseUri, IEnumerable<KeyValuePair<string, string>> args, System.Web.UI.Page page, Uri frameSourceUri = null) {
            Contract.Requires<ArgumentNullException>(baseUri != null);
            Contract.Requires<ArgumentNullException>(args != null);
            Contract.Requires<ArgumentNullException>(page != null);
            
            page.Form.Action = baseUri.AbsoluteUri;
            foreach (KeyValuePair<string, string> pair in args) {
                page.Controls.Add(new System.Web.UI.WebControls.HiddenField() { ID = pair.Key, Value = pair.Value });
            }

            using (var writer = new System.IO.StringWriter()) {
                page.Server.Execute(page, writer, false);
                return writer.ToString();
            }
        }
         */

        /// <summary>
        /// Gets the base URI of the token, if applicable.
        /// </summary>
        /// <param name="uri">The base URI of the token.</param>
        /// <returns>The base URI of a given element.</returns>
        internal static Uri GetBaseUrl(Uri uri) {
            if (uri == null) {
                throw new ArgumentNullException(nameof(uri));
            }

            if (!uri.IsAbsoluteUri) {
                throw new ArgumentException("Must be absolute Uri.", nameof(uri));
            }

            return new Uri(uri.GetLeftPart(UriPartial.Path));

            /*
            string absoluteUri = uri.AbsoluteUri;
            int length = absoluteUri.IndexOf("?", 0, StringComparison.Ordinal);
            if (length > -1) {
                return new Uri(absoluteUri.Substring(0, length));
            }

            return uri;
             */ 
        }

        /// <summary>
        /// Converts a string that has been encoded for transmission in a URL into a decoded string.
        /// </summary>
        /// <param name="encodedValue">The string to decode.</param>
        /// <returns>A decoded string.</returns>
        internal static string UrlDecode(string encodedValue) {
#if NET40 || NET35
            return HttpUtility.UrlDecode(encodedValue);
#else
            return WebUtility.UrlDecode(encodedValue);
#endif
        }

        /// <summary>
        /// Converts a text string into a URL-encoded string.
        /// </summary>
        /// <param name="value">The text to URL-encode.</param>
        /// <returns>A URL-encoded string.</returns>
        internal static string UrlEncode(string value) {
#if NET40 || NET35
            return HttpUtility.UrlEncode(value);
#else
            return WebUtility.UrlEncode(value);
#endif
        }

        /// <summary>
        /// Converts a string that has been HTML-encoded for HTTP transmission into a decoded string.
        /// </summary>
        /// <param name="encodedValue">The string to decode.</param>
        /// <returns>A decoded string.</returns>
        internal static string HtmlDecode(string encodedValue) {
#if NET40 || NET35
            return HttpUtility.HtmlDecode(encodedValue);
#else
            return WebUtility.HtmlDecode(encodedValue);
#endif
        }

        /// <summary>
        /// Converts a text string into a HTML-encoded string.
        /// </summary>
        /// <param name="value">The text to HTML-encode.</param>
        /// <returns>A HTML-encoded string.</returns>
        internal static string HtmlEncode(string value) {
#if NET40 || NET35
            return HttpUtility.HtmlEncode(value);
#else
            return WebUtility.HtmlEncode(value);
#endif
        }

        /// <summary>
        /// Minimally converts a string into an HTML-encoded string.
        /// </summary>
        /// <param name="value">The string to encode.</param>
        /// <returns>An encoded string.</returns>
        internal static string HtmlAttributeEncode(string value) {
#if NET40 || NET35
            return HttpUtility.HtmlAttributeEncode(value);
#else
            return WebUtility.HtmlEncode(value);
#endif
        }
    }
}
