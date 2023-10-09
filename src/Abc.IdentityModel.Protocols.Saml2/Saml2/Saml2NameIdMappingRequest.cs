// ----------------------------------------------------------------------------
// <copyright file="Saml2NameIdMappingRequest.cs" company="ABC Software Ltd">
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
#if WIF35
    using Microsoft.IdentityModel.Tokens.Saml2;
#elif AZUREAD
    using Microsoft.IdentityModel.Tokens.Saml2;
#else
    using System.IdentityModel.Tokens;
#endif

    /// <summary>
    /// The <c>Saml2NameIdMappingRequest</c> class requests a partner modify the mapping of a subject's SAML name identifier.
    /// </summary>
    /// <remarks>See the samlp:NameIDMappingRequest element defined in [SamlCore, 3.8.1].</remarks>
    internal class Saml2NameIdMappingRequest : Saml2Request {
        private Saml2NameIdentifier identifier;
        private Saml2NameIdentifierPolicy nameIdentifierPolicy = new Saml2NameIdentifierPolicy();

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2NameIdMappingRequest"/> class.
        /// </summary>
        /// <param name="identifier">The name identifier to be mapped.</param>
        public Saml2NameIdMappingRequest(Saml2NameIdentifier identifier) {
            this.identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        /// <summary>
        /// Gets or sets the identifier and associated descriptive data that specify the principal as currently recognized by the
        /// requester and the responder.
        /// </summary>
        /// <value>The identifier and associated descriptive data that specify the principal as currently recognized by the
        /// requester and the responder.</value>
        public Saml2NameIdentifier Identifier {
            get {
                return this.identifier;
            }

            set {
                this.identifier = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Gets or sets requirements regarding the format and optional name qualifier for the identifier to be returned.
        /// </summary>
        /// <value>The requirements regarding the format and optional name qualifier for the identifier to be returned.</value>
        public Saml2NameIdentifierPolicy NameIdentifierPolicy {
            get {
                return this.nameIdentifierPolicy;
            }

            set {
                this.nameIdentifierPolicy = value ?? throw new ArgumentNullException(nameof(value));
            }
        }
    }
}
