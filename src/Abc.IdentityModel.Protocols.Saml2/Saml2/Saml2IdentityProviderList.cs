// ----------------------------------------------------------------------------
// <copyright file="Saml2IdentityProviderList.cs" company="ABC Software Ltd">
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
    /// The <c>IdentityProviderCollection</c> class specifies the identity providers trusted by the requester to 
    /// authenticate the presenter.
    /// </summary>
    /// <remarks>See the samlp:IDPList element defined in [SamlCore, 3.4.1.3] for more details.</remarks>
    /// <details>
    /// If this collection is empty, it will not be serialized.
    /// </details>
    internal class Saml2IdentityProviderList : Collection<Saml2IdentityProviderEntry> {
        /// <summary>
        /// Gets or sets a URI identifying a location where the complete list of Identity Providers can be retrieved
        /// from. See [SamlCore, 3.4.1.3] for more details.
        /// </summary>
        /// <value>A URI identifying a location where the complete list of Identity Providers can be retrieved
        /// from.</value>
        public Uri GetComplete { get; set; }
    }
}
