// ----------------------------------------------------------------------------
// <copyright file="Saml2RequestedAuthenticationContext.cs" company="ABC Software Ltd">
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

    /// <summary>
    /// The <c>RequestedAuthenticationContext</c> class specifies the authentication context requirements of authentication
    /// statements returned in response to a request or query.
    /// </summary>
    /// <remarks> See the samlp:RequestedAuthnContext element defined in [SamlCore, 3.3.2.2.1] for more details.</remarks>
    internal class Saml2RequestedAuthenticationContext {
        private Saml2AuthenticationContextComparisonType comparison;
        private Collection<Uri> references = new Collection<Uri>();
        private Saml2AuthenticationContextReferenceType referenceType;

        /// <summary>
        /// Gets or sets the comparison rule used by the responder to match the 
        /// references.
        /// </summary>
        /// <remarks>See [SamlCore, 3.3.2.2.1] for more details.</remarks>
        /// <details>
        /// The default is Exact.
        /// </details>
        /// <value>The comparison rule used by the responder to match the references.</value>
        public Saml2AuthenticationContextComparisonType Comparison {
            get {
                return this.comparison;
            }

            set {
                this.comparison = value;
            }
        }

        /// <summary>
        /// Gets the ordered list of URIs identifying acceptable authentication
        /// context classes or declarations, most preferred first.
        /// </summary>
        /// <remarks>See [SamlCore, 3.3.2.2.1] for more details.</remarks>
        /// <details>
        /// If this collection is empty an exception will occur during serialization.
        /// </details>
        /// <value>The ordered list of URIs identifying acceptable authentication
        /// context classes or declarations, most preferred first.</value>
        public Collection<Uri> References {
            get {
                return this.references;
            }
        }

        /// <summary>
        /// Gets or sets whether the References refer to authentication context classes or 
        /// authentication context declarations.
        /// </summary>
        /// <value>Whether the References refer to authentication context classes or 
        /// authentication context declarations.</value>
        public Saml2AuthenticationContextReferenceType ReferenceType {
            get {
                return this.referenceType;
            }

            set {
                this.referenceType = value;
            }
        }
    }
}
