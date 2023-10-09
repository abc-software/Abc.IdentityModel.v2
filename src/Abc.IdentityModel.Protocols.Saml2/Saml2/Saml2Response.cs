// ----------------------------------------------------------------------------
// <copyright file="Saml2Response.cs" company="ABC Software Ltd">
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
#if WIF35
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Tokens.Saml2;
#elif AZUREAD
    using Microsoft.IdentityModel.Tokens.Saml2;
#else
    using System.IdentityModel.Tokens;
#endif

    /// <summary>
    /// The <c>Saml2Response</c> class contains the result of an <c>AuthenticationRequest</c>.
    /// </summary>
    /// <remarks>See the samlp:Response element defined in [SamlCore, 3.3.3] for more details.</remarks>
    internal class Saml2Response : Saml2StatusResponse {
        private readonly Collection<SecurityTokenElement> assertions = new Collection<SecurityTokenElement>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2Response"/> class.
        /// </summary>
        /// <param name="status">The SAML status code associated with this response.</param>
        public Saml2Response(Saml2Status status)
            : base(status) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2Response"/> class.
        /// </summary>
        /// <param name="status">The SAML status code associated with this response.</param>
        /// <param name="assertions">A list of assertions contained in this response.</param>
        public Saml2Response(Saml2Status status, IEnumerable<Saml2Assertion> assertions)
            : base(status) {
            if (assertions != null) {
                foreach (Saml2Assertion assertion in assertions) {
                    this.assertions.Add(new SecurityTokenElement(new Saml2SecurityToken(assertion)));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2Response"/> class.
        /// </summary>
        /// <param name="status">The SAML status code associated with this response.</param>
        /// <param name="assertions">A list of assertions contained in this response.</param>
        public Saml2Response(Saml2Status status, IEnumerable<SecurityTokenElement> assertions)
            : base(status) {
            if (assertions != null) {
                foreach (SecurityTokenElement element in assertions) {
                    if (element != null) {
                        if (element.SecurityTokenXml == null && !(element.GetSecurityToken() is Saml2SecurityToken)) {
                            throw new ArgumentException("Assertions must be SAML 2.0."); 
                        }

                        this.assertions.Add(element);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the set of assertions that satisfy the request. See [SamlCore, 3.3.3] for
        /// more details.
        /// </summary>
        /// <value>The set of assertions that satisfy the request.</value>
        public Collection<SecurityTokenElement> Assertions {
            get {
                return this.assertions;
            }
        }
    }
}
