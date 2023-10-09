// ----------------------------------------------------------------------------
// <copyright file="Saml2AttributeQuery.cs" company="ABC Software Ltd">
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
    using Microsoft.IdentityModel.Tokens.Saml2;
#elif AZUREAD
    using Microsoft.IdentityModel.Tokens.Saml2;
#else
    using System.IdentityModel.Tokens;
#endif

    /// <summary>
    /// The <c>AttributeQuery</c> class requests additional information about a subject.
    /// </summary>
    /// <remarks>See the samlp:AttributeQuery element defined in [SamlCore, 3.3.2.3] for more details.</remarks>
    internal class Saml2AttributeQuery : Saml2SubjectQuery {
        private readonly Collection<Saml2Attribute> attributes = new Collection<Saml2Attribute>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2AttributeQuery"/> class.
        /// </summary>
        /// <param name="samlSubject">The SAML subject whose attributes are being queried.</param>
        public Saml2AttributeQuery(Saml2Subject samlSubject)
            : base(samlSubject) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2AttributeQuery"/> class.
        /// </summary>
        /// <param name="samlSubject">The SAML subject whose attributes are being queried.</param>
        /// <param name="attributes">the attributes to be returned.</param>
        public Saml2AttributeQuery(Saml2Subject samlSubject, IEnumerable<Saml2Attribute> attributes)
            : this(samlSubject) {
            if (attributes != null) {
                foreach (var item in attributes) {
                    this.attributes.Add(item);
                }
            }
        }

        /// <summary>
        /// Gets the attributes to be returned. If no attributes are specified, it indicates that all
        /// attributes allowed by policy are requested.
        /// </summary>
        /// <value>The list of attributes to be returned.</value>
        public Collection<Saml2Attribute> Attributes {
            get {
                return this.attributes; 
            }
        }
    }
}
