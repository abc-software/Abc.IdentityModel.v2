// ----------------------------------------------------------------------------
// <copyright file="Saml2LogoutRequest.cs" company="ABC Software Ltd">
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
    using System.Collections.ObjectModel;
#if WIF35
    using Microsoft.IdentityModel.Tokens.Saml2;
#elif AZUREAD
    using Microsoft.IdentityModel.Tokens.Saml2;
#else
    using System.IdentityModel.Tokens;
#endif

    /// <summary>
    /// The <c>Saml2LogoutRequest</c> class requests that a subject's session at a provider be terminated.
    /// </summary>
    /// <remarks>See the samlp:LogoutRequest element defined in [SamlCore, 3.7.1] for more details.</remarks>
    internal class Saml2LogoutRequest : Saml2Request {
        private readonly Collection<string> sessionIndex = new Collection<string>();
        private Saml2NameIdentifier nameId;
        private DateTime? notOnOrAfter;
        private string reason;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2LogoutRequest"/> class.
        /// </summary>
        /// <param name="nameId">The principal to be logged out.</param>
        public Saml2LogoutRequest(Saml2NameIdentifier nameId)
            : base() {
            this.nameId = nameId ?? throw new ArgumentNullException(nameof(nameId));
        }

        /// <summary>
        /// Gets or sets the identifier and associated attributes that specify the principal as
        /// currently recognized by the identity and service providers prior to this request.
        /// </summary>
        /// <value>The identifier and associated attributes that specify the principal.</value>
        /// <remarks>See [SamlCore, 3.7.1] for more details.</remarks>
        public Saml2NameIdentifier NameId {
            get {
                return this.nameId;
            }

            set {
                this.nameId = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Gets or sets the time at which the request expires, after which the recipient may discard the message.
        /// </summary>
        /// <value>The time at which the request expires.</value>
        /// <remarks>See [SamlCore, 3.7.1] for more details.</remarks>
        public DateTime? NotOnOrAfter {
            get {
                return this.notOnOrAfter;
            }

            set {
                this.notOnOrAfter = value;
            }
        }

        /// <summary>
        /// Gets or sets the reason for the logout, in the form of a URI.
        /// </summary>
        /// <remarks>See [SamlCore, 3.7.1] for more details.</remarks>
        /// <value>The reason for the logout.</value>
        public string Reason {
            get {
                return this.reason;
            }

            set {
                this.reason = value;
            }
        }

        /// <summary>
        /// Gets the identifiers that indexes this session at the message recipient.
        /// </summary>
        /// <remarks>See [SamlCore, 3.7.1] for more details.</remarks>
        /// <value>The identifiers that indexes this session at the message recipient.</value>
        public Collection<string> SessionIndex {
            get {
                return this.sessionIndex;
            }
        }
    }
}
