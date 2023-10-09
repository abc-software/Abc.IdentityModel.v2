// ----------------------------------------------------------------------------
// <copyright file="HttpMessageBase.cs" company="ABC Software Ltd">
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
    
    /// <summary>
    /// Http protocol message base class.
    /// </summary>
    public abstract class HttpMessageBase : IHttpProtocolMessage {
        private Dictionary<string, string> extraData = new Dictionary<string, string>();
        private Uri baseUrl;
        private HttpDeliveryMethods method;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessageBase" /> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="method">The HTTP method(s) allowed.</param>
        protected HttpMessageBase(Uri baseUrl, HttpDeliveryMethods method) {
            if (baseUrl == null) {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            if (!baseUrl.IsAbsoluteUri) {
                throw new ArgumentException("Must be absolute Uri.", nameof(baseUrl));
            }

            this.baseUrl = HttpUtil.GetBaseUrl(baseUrl);
            this.method = method;
        }

        /// <summary>
        /// Gets the URL of the intended receiver of this message.
        /// </summary>
        public Uri BaseUri {
            get {
                return this.baseUrl;
            }
        }

        /// <summary>
        /// Gets the extra, non-standard Protocol parameters included in the message.
        /// </summary>
        /// <remarks>
        /// Implementations of this interface should ensure that this property never returns null.
        /// </remarks>
        public IDictionary<string, string> ExtraData {
            get { return this.extraData; }
        }

        /// <summary>
        /// Gets the preferred method of transport for the message.
        /// </summary>
        /// <remarks>
        /// For indirect messages this will likely be GET+POST, which both can be simulated in the user agent:
        /// the GET with a simple 301 Redirect, and the POST with an HTML form in the response with javascript
        /// to automate submission.
        /// </remarks>
        public virtual HttpDeliveryMethods HttpMethods {
            get {
                return this.method;
            }
        }

        /// <summary>
        /// Checks the message state for conformity to the protocol specification
        /// and throws an exception if the message is invalid.
        /// </summary>
        public virtual void Validate() {
        }
    }
}
