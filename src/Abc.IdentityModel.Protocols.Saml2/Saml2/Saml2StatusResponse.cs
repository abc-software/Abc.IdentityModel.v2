// ----------------------------------------------------------------------------
// <copyright file="Saml2StatusResponse.cs" company="ABC Software Ltd">
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
    /// The <c>StatusResponse</c> class contains information common to all SAML responses.
    /// </summary>
    /// <remarks>See the samlp:StatusResponseType defined in [SamlCore, 3.2.2] for more details.</remarks>
    internal abstract class Saml2StatusResponse : Saml2Message {
        private Saml2Id inResponseTo;
        private Saml2Status status;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2StatusResponse"/> class.
        /// </summary>
        /// <param name="status">The SAML status code associated with this response.</param>
        protected Saml2StatusResponse(Saml2Status status) {
            this.status = status ?? throw new ArgumentNullException(nameof(status));
        }

        /// <summary>
        /// Gets or sets the identifier of the request to which the 
        /// response corresponds, if any.
        /// </summary>
        /// <remarks>See [SamlCore, 3.2.1] for more details.</remarks>
        /// <value>The identifier of the request to which this response corresponds.</value>
        public Saml2Id InResponseTo {
            get {
                return this.inResponseTo;
            }

            set {
                this.inResponseTo = value;
            }
        }

        /// <summary>
        /// Gets or sets the status of the response.
        /// </summary>
        /// <value>The status of the response.</value>
        public Saml2Status Status {
            get {
                return this.status;
            }

            set {
                this.status = value ?? throw new ArgumentNullException(nameof(value));
            }
        }
    }
}
