// ----------------------------------------------------------------------------
// <copyright file="HttpSaml2Message2.cs" company="ABC Software Ltd">
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

namespace Abc.IdentityModel.Protocols.Saml2 {
    using System;
    using System.ComponentModel;
    using Abc.IdentityModel.Http;
    using Abc.IdentityModel.Http.Converters;

    /// <summary>
    /// The <c>HttpSaml2Message2</c> class represents a SAML message to be sent or that has been received over an HTTP binding.
    /// </summary>
    public abstract class HttpSaml2Message2 : HttpMessageBase {
        private string relayState;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSaml2Message2" /> class.
        /// </summary>
        /// <param name="baseUrl">A URI identifying the address which this message will be sent to, or which it was received on.</param>
        /// <param name="method">The HTTP method(s) allowed.</param>
        protected HttpSaml2Message2(Uri baseUrl, HttpDeliveryMethods method)
            : base(baseUrl, method) {
        }

        [MessagePart(Saml2Constants.Parameters.RelayState, IsRequired = false)]
        public string RelayState {
            get {
                return this.relayState;
            }

            set {
                if (value != null && value.Length > 80) {
                    throw new InvalidOperationException($"Relay state parameter has wrong length {value.Length}, expected less than {80}.");
                }

                this.relayState = value;
            }
        }

        [MessagePart(Saml2Constants.Parameters.SamlEncoding, AllowEmpty = false)]
        public string Encoding { get; set; }

        public override void Validate() {
            base.Validate();

            string encoding = this.Encoding;
            if (!string.IsNullOrEmpty(encoding) && !string.Equals(encoding, Saml2Constants.ProtocolBindings.DeflateEncoding.ToString(), StringComparison.Ordinal)) { // TODO: to constants
                throw new HttpMessageException(string.Format("The specified encoding method '{0}' is not supported.", encoding));
            }
        }
    }
}
