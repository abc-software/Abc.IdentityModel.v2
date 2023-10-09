// ----------------------------------------------------------------------------
// <copyright file="Saml2SubjectQuery.cs" company="ABC Software Ltd">
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
    /// The <c>Saml2SubjectQuery</c> class contains information common to all requests for additional information about a given SAML subject.
    /// </summary>
    /// <remarks>See the samlp:SubjectQueryAbstractType type defined in [SamlCore, 3.3.2.1] for more details.</remarks>
    internal abstract class Saml2SubjectQuery : Saml2Request {
        private Saml2Subject subject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2SubjectQuery"/> class.
        /// </summary>
        /// <param name="samlSubject">The subject of the query.</param>
        protected Saml2SubjectQuery(Saml2Subject samlSubject)
            : base() {
            this.subject = samlSubject ?? throw new ArgumentNullException(nameof(samlSubject));
        }

        /// <summary>
        /// Gets or sets the SAML subject being queried.
        /// </summary>
        /// <remarks>See [SamlCore, 3.3.2.1] for more details.</remarks>
        /// <value>The SAML subject being queried.</value>
        public Saml2Subject Subject {
            get {
                return this.subject; 
            }

            set {
                this.subject = value ?? throw new ArgumentNullException(nameof(value));
            }
        }
    }
}
