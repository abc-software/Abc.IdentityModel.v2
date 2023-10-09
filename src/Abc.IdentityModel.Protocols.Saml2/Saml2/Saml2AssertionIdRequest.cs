// ----------------------------------------------------------------------------
// <copyright file="Saml2AssertionIdRequest.cs" company="ABC Software Ltd">
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
    /// The <c>Saml2AssertionIdRequest</c> class requests the assertions with a given set of identifiers.
    /// </summary>
    /// <remarks>See the samlp:AssertionIDRequest element defined in [SamlCore, 3.3.1] for more details.</remarks>
    internal class Saml2AssertionIdRequest : Saml2Request {
        private readonly Collection<Saml2Id> assertionIdReferences = new Collection<Saml2Id>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2AssertionIdRequest"/> class.
        /// </summary>
        /// <param name="reference">The AssertionID for this request.</param>
        public Saml2AssertionIdRequest(Saml2Id reference)
            : base() {
            if (reference == null) {
                throw new ArgumentNullException(nameof(reference));
            }

            this.assertionIdReferences.Add(reference);
        }

        /// <summary>
        /// Gets a collection of assertion IDs to be requested.
        /// </summary>
        /// <value>The AssertionIDs of this request.</value>
        public Collection<Saml2Id> AssertionIdReferences {
            get {
                return this.assertionIdReferences;
            }
        }
    }
}
