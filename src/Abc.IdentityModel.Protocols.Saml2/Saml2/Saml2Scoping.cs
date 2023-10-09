// ----------------------------------------------------------------------------
// <copyright file="Saml2Scoping.cs" company="ABC Software Ltd">
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
    /// The <c>Saml2Scoping</c> class specifies the identity providers trusted by the requester to authenticate the presenter.
    /// </summary>
    /// <remarks>See the samlp:Scoping element defined in [SamlCore, 3.4.1.2] for more details.</remarks>
    internal class Saml2Scoping {
        private uint? proxyCount;
        private Saml2IdentityProviderList identityProviderList = new Saml2IdentityProviderList();
        private Collection<Uri> requesterIds = new Collection<Uri>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2Scoping"/> class.
        /// </summary>
        public Saml2Scoping() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2Scoping"/> class.
        /// </summary>
        /// <param name="identityProviderList">The list of identity providers.</param>
        /// <remarks>
        /// Use this constructor if you want to use a type derived from
        /// IdentityProviderCollection for the IdentityProviderList.
        /// </remarks>
        public Saml2Scoping(Saml2IdentityProviderList identityProviderList) {
            this.identityProviderList = identityProviderList ?? throw new ArgumentNullException(nameof(identityProviderList));
        }

        /// <summary>
        /// Gets or sets the number of proxying indirections permissible between 
        /// the identity provider that receives this AuthenticationRequest
        /// and the identity provider that ultimately authenticates the principal.
        /// </summary>
        /// <remarks>See [SamlCore, 3.4.1.2] for more details.</remarks>
        /// <details>
        /// A value of 0 permits no proxying, but null expresses no restriction.
        /// </details>
        /// <value>The number of proxying indirections permissible between the identity provider that receives
        /// this request and the identity provider that ultimately authenticates the principal.</value>
        public uint? ProxyCount {
            get { return this.proxyCount; }
            set { this.proxyCount = value; }
        }

        /// <summary>
        /// Gets the list of identity providers and associated information 
        /// that the requester deems acceptable to respond to the request.
        /// </summary>
        /// <remarks>See [SamlCore, 3.4.1.2] for more details.</remarks>
        /// <value>The list of identity providers that the requester deems acceptable to respond to the request.</value>
        public Saml2IdentityProviderList IdentityProviderList {
            get { return this.identityProviderList; }
        }

        /// <summary>
        /// Gets the set of requesting entities on whose behalf the requester is 
        /// acting.
        /// </summary>
        /// <remarks>See [SamlCore, 3.4.1.2] for more details.</remarks>
        /// <value>The set of requesting entities on whose behalf the requester is acting.</value>
        public Collection<Uri> RequesterIds {
            get { return this.requesterIds; }
        }
    }
}
