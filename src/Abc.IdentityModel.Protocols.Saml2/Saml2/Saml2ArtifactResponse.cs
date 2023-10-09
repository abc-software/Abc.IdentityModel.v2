// ----------------------------------------------------------------------------
// <copyright file="Saml2ArtifactResponse.cs" company="ABC Software Ltd">
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

    /// <summary>
    /// The <c>Saml2ArtifactResponse</c> class contains the result of an artifact resolution request.
    /// </summary>
    /// <remarks>See the samlp:ArtifactResponse element defined in [SamlCore, 3.5.2] for more details.</remarks>
    internal class Saml2ArtifactResponse : Saml2StatusResponse {
        private Saml2ArtifactResponseContent responseContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ArtifactResponse"/> class.
        /// </summary>
        /// <param name="status">The SAML status associated with this response.</param>
        public Saml2ArtifactResponse(Saml2Status status)
            : base(status) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ArtifactResponse"/> class.
        /// </summary>
        /// <param name="status">The SAML status associated with this response.</param>
        /// <param name="responseContent">The SAML message contained in this artifact response.</param>
        public Saml2ArtifactResponse(Saml2Status status, Saml2ArtifactResponseContent responseContent)
            : base(status) {
            this.responseContent = responseContent ?? throw new ArgumentNullException(nameof(responseContent));
        }

        /// <summary>
        /// Gets or sets the SAML message returned in the artifact response.
        /// </summary>
        /// <value>The SAML message to be returned in the artifact response.</value>
        public Saml2ArtifactResponseContent Response {
            get {
                return this.responseContent;
            }

            set {
                this.responseContent = value ?? throw new ArgumentNullException(nameof(value));
            }
        }
    }
}
