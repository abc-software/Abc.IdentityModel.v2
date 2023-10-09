// ----------------------------------------------------------------------------
// <copyright file="Saml2AuthenticationQuery.cs" company="ABC Software Ltd">
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
    /// The <c>Saml2AuthenticationQuery</c> class requests the set of assertions that a provider has issued.
    /// </summary>
    /// <remarks>See the samlp:AuthnQuery element defined in [SamlCore, 3.3.2.2] for more details.</remarks>
    internal class Saml2AuthenticationQuery : Saml2SubjectQuery {
        private Saml2RequestedAuthenticationContext authenticationContext;
        private string sessionIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2AuthenticationQuery"/> class.
        /// </summary>
        /// <param name="samlSubject">The subject of the query.</param>
        public Saml2AuthenticationQuery(Saml2Subject samlSubject)
            : base(samlSubject) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2AuthenticationQuery"/> class.
        /// </summary>
        /// <param name="samlSubject">The saml subject.</param>
        /// <param name="authenticationContext">The authentication context.</param>
        public Saml2AuthenticationQuery(Saml2Subject samlSubject, Saml2RequestedAuthenticationContext authenticationContext)
            : this(samlSubject) {
            this.authenticationContext = authenticationContext;
        }

        /// <summary>
        /// Gets or sets the requested authentication context.
        /// </summary>
        /// <value>The requested authentication context.</value>
        public Saml2RequestedAuthenticationContext RequestedAuthenticationContext {
            get { return this.authenticationContext; }
            set { this.authenticationContext = value; }
        }

        /// <summary>
        /// Gets or sets an optional filter for possible responses.
        /// </summary>
        /// <value>The filter for possible responses.</value>
        public string SessionIndex {
            get { return this.sessionIndex; }
            set { this.sessionIndex = value; }
        }
    }
}
