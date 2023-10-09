// ----------------------------------------------------------------------------
// <copyright file="Saml2IdentityProviderEntry.cs" company="ABC Software Ltd">
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
    /// The <c>Saml2IdentityProviderEntry</c> class specifies a single identity provider trusted by the requester to
    /// authenticate the presenter. 
    /// </summary>
    /// <remarks>See the samlp:IDPEntry element defined in [SamlCore, 3.4.1.3.1] for more details.</remarks>
    internal class Saml2IdentityProviderEntry {
        private Uri providerId;
        private string name;
        private Uri location;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2IdentityProviderEntry"/> class.
        /// </summary>
        /// <param name="providerId">The unique identifier for the Identity Provider.</param>
        public Saml2IdentityProviderEntry(Uri providerId) {
            this.providerId = providerId ?? throw new ArgumentNullException(nameof(providerId));
        }

        /// <summary>
        /// Gets or sets the location of a profile-specific endpoint supporting the 
        /// authentication request protocol. See [SamlCore, 3.4.1.3.1] for more details.
        /// </summary>
        /// <value>The location of a profile-specific endpoint supporting the 
        /// authentication request protocol.</value>
        public Uri Location {
            get { return this.location; }
            set { this.location = value; }
        }

        /// <summary>
        /// Gets or sets a human-readable name for the identity provider. See [SamlCore, 3.4.1.3.1] for more details.
        /// </summary>
        /// <value>A human-readable name for the identity provider.</value>
        public string Name {
            get { return this.name; }
            set { this.name = !string.IsNullOrEmpty(value) ? value : null; }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the identity provider. See [SamlCore, 3.4.1.3.1] for more details.
        /// </summary>
        /// <details>
        /// This property may not be null.
        /// </details>
        /// <value>The unique identifier of the identity provider.</value>
        public Uri ProviderId {
            get {
                return this.providerId;
            }

            set {
                this.providerId = value ?? throw new ArgumentNullException(nameof(value));
            }
        }
    }
}
