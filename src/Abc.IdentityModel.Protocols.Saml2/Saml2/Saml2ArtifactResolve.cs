// ----------------------------------------------------------------------------
// <copyright file="Saml2ArtifactResolve.cs" company="ABC Software Ltd">
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
    /// The <c>Saml2ArtifactResolve</c> class requests that an artifact handle be exchanged for its original SAML message.
    /// </summary>
    /// <remarks>See the samlp:ArtifactResolve element defined in [SamlCore, 3.5.1] for more details.</remarks>
    internal class Saml2ArtifactResolve : Saml2Request {
        private string artifact;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ArtifactResolve"/> class.
        /// </summary>
        /// <param name="artifact">The artifact to be resolved.</param>
        public Saml2ArtifactResolve(string artifact)
        {
            this.artifact = artifact ?? throw new ArgumentNullException(nameof(artifact));
        }

        /// <summary>
        /// Gets or sets the artifact value to be resolved.
        /// </summary>
        /// <value>The base64-encoded artifact value to be resolved.</value>
        public string Artifact {
            get {
                return this.artifact;
            }

            set {
                this.artifact = value ?? throw new ArgumentNullException(nameof(value));
            }
        }
    }
}
