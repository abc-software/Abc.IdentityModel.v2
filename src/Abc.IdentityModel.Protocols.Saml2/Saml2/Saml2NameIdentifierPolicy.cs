// ----------------------------------------------------------------------------
// <copyright file="Saml2NameIdentifierPolicy.cs" company="ABC Software Ltd">
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
    /// The <c>NameIdentifierPolicy</c> class tailors the name identifier in the subjects of assertions resulting
    /// from an AuthnRequest.
    /// </summary>
    /// <remarks>See the samlp:NameIDPolicy element defined in [SamlCore, 3.4.1.1].</remarks>
    internal class Saml2NameIdentifierPolicy {
        private Uri format = Saml2Constants.NameIdentifierFormats.Unspecified;
        private string nameQualifier;
        private bool allowCreate;

        /// <summary>
        /// Gets or sets the name identifier format.
        /// </summary>
        /// <remarks>See [SamlCore, 3.4.1.1] for more details.</remarks>
        /// <value>The name identifier format.</value>
        public Uri Format {
            get {
                return this.format;
            }

            set {
                if (null == value) {
                    this.format = Saml2Constants.NameIdentifierFormats.Unspecified;
                }
                else {
                    this.format = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets an optional namespace that the assertion subject's identifier be 
        /// returned (or created) in.
        /// </summary>
        /// <remarks>See [SamlCore, 3.4.1.1] for more details.</remarks>
        /// <value>An optional namespace that the assertion subject's identifier be 
        /// returned (or created) in.</value>
        public string SPNameQualifier {
            get { return this.nameQualifier; }
            set { this.nameQualifier = !string.IsNullOrEmpty(value) ? value : null; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the identity provider is allowed, in the course
        /// of fulfilling the request, to create a new identifier to represent
        /// the principal.
        /// </summary>
        /// <remarks>
        /// See [SamlCore, 3.4.1.1] for more details.
        /// </remarks>
        /// <value>Whether the identity provider is allowed, in the course
        /// of fulfilling the request, to create a new identifier to represent
        /// the principal.</value>
        public bool AllowCreate {
            get { return this.allowCreate; }
            set { this.allowCreate = value; }
        }
    }
}
