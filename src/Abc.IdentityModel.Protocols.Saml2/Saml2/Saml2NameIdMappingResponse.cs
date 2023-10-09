// ----------------------------------------------------------------------------
// <copyright file="Saml2NameIdMappingResponse.cs" company="ABC Software Ltd">
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
    /// The <c>NameIdMappingResponse</c> class represents a response to a request to change a subject's SAML name identifier.
    /// </summary>
    /// <remarks>See the samlp:NameIDMappingResponse element defined in [SamlCore, 3.8.2] for more details.</remarks>
    internal class Saml2NameIdMappingResponse : Saml2StatusResponse {
        private Saml2NameIdentifier nameId;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2NameIdMappingResponse"/> class.
        /// </summary>
        /// <param name="status">The SAML status code associated with this response.</param>
        /// <param name="nameId">The mapped name identifier.</param>
        public Saml2NameIdMappingResponse(Saml2Status status, Saml2NameIdentifier nameId)
            : base(status) {
            this.nameId = nameId ?? throw new ArgumentNullException(nameof(nameId));
        }

        /// <summary>
        /// Gets or sets the mapped name identifier.
        /// </summary>
        /// <value>The mapped name identifier.</value>
        public Saml2NameIdentifier NameId {
            get {
                return this.nameId;
            }

            set {
                this.nameId = value ?? throw new ArgumentNullException(nameof(value));
            }
        }
    }
}