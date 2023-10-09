// ----------------------------------------------------------------------------
// <copyright file="Saml2NewNameIdentifier.cs" company="ABC Software Ltd">
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
    using Microsoft.IdentityModel.SecurityTokenService;
#elif AZUREAD
    using Microsoft.IdentityModel.Tokens;
#else
    using System.IdentityModel.Tokens;
#endif
    /// <summary>
    /// The <c>Saml2NewNameIdentifier</c> class represents the new SAML name identifier that will be used to identify a subject.
    /// </summary>
    /// <remarks>See the saml:NewID or saml:NewEncryptedID elements defined in [SamlCore, 3.6.1; SamlCoreErrata, 3.6.1] for more details.</remarks>
    internal class Saml2NewNameIdentifier {
        private string identifier;
        private EncryptingCredentials encryptingCredentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2NewNameIdentifier"/> class.
        /// </summary>
        /// <param name="identifier">The new identifier.</param>
        public Saml2NewNameIdentifier(string identifier) {
            if (string.IsNullOrEmpty(identifier)) {
                throw new ArgumentNullException(nameof(identifier));
            }
            
            this.identifier = identifier;
        }

        /// <summary>
        /// Gets or sets the credentials to use when encrypting the NewID element in a NewEncryptedID element.
        /// </summary>
        /// <value>The credentials to use when encrypting the NewID element.</value>
        public EncryptingCredentials EncryptingCredentials {
            get { return this.encryptingCredentials; }
            set { this.encryptingCredentials = value; }
        }

        /// <summary>
        /// Gets or sets the value of NewID/NewEncryptedID element.
        /// </summary>
        /// <value>The value of the NewID element.</value>
        public string Value {
            get {
                return this.identifier;
            }

            set {
                if (string.IsNullOrEmpty(value)) {
                    throw new ArgumentException("string.IsNullOrEmpty(value)", nameof(value));
                }

                this.identifier = value;
            }
        }
    }
}
