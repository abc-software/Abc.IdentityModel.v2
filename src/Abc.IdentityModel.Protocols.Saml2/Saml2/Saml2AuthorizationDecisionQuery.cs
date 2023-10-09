// ----------------------------------------------------------------------------
// <copyright file="Saml2AuthorizationDecisionQuery.cs" company="ABC Software Ltd">
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
    /// The <c>Saml2AuthorizationDecisionQuery</c> class requests whether a user has permission to access a given resource.
    /// </summary>
    /// <remarks>See the samlp:AuthzDecisionQuery element defined in [SamlCore, 3.3.2.4] for more details.</remarks>
    internal class Saml2AuthorizationDecisionQuery : Saml2SubjectQuery {
        private readonly Collection<Saml2Action> actions = new Collection<Saml2Action>();
        private Saml2Evidence evidence;
        private Uri resource;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2AuthorizationDecisionQuery"/> class.
        /// </summary>
        /// <param name="samlSubject">The subject of the query.</param>
        /// <param name="resource">The Uri indicating the resource for which authorization is requested.</param>
        public Saml2AuthorizationDecisionQuery(Saml2Subject samlSubject, Uri resource)
            : this(samlSubject, resource, null, null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2AuthorizationDecisionQuery"/> class.
        /// </summary>
        /// <param name="samlSubject">The subject of the query.</param>
        /// <param name="resource">The Uri indicating the resource for which authorization is requested.</param>
        /// <param name="samlActions">The actions for which authorization is requested.</param>
        /// <param name="samlEvidence">The assertions that the SAML authority MAY rely on in making its authorization decision.</param>
        public Saml2AuthorizationDecisionQuery(Saml2Subject samlSubject, Uri resource, IEnumerable<Saml2Action> samlActions, Saml2Evidence samlEvidence)
            : base(samlSubject) {
            if (resource == null) {
                throw new ArgumentNullException(nameof(resource));
            }

            if (!resource.IsAbsoluteUri) {
                throw new ArgumentException("!resource.IsAbsoluteUri", nameof(resource));
            }

            this.resource = resource;
            this.evidence = samlEvidence;

            if (samlActions != null) {
                foreach (var item in samlActions) {
                    this.actions.Add(item);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Uri indicating the resource for which authorization is requested.
        /// </summary>
        /// <value>The Uri indicating the resource for which authorization is requested.</value>
        public Uri Resource {
            get {
                return this.resource;
            }

            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!value.IsAbsoluteUri) {
                    throw new ArgumentException("!value.IsAbsoluteUri", nameof(value));
                }

                this.resource = value;
            }
        }

        /// <summary>
        /// Gets the actions for which authorization is requested.
        /// </summary>
        /// <value>The actions for which authorization is requested.</value>
        public Collection<Saml2Action> Actions {
            get {
                return this.actions;
            }
        }

        /// <summary>
        /// Gets or sets a set of assertions that the SAML authority MAY rely on in making its authorization decision.
        /// </summary>
        /// <value>The assertions that the SAML authority MAY rely on in making its authorization decision.</value>
        public Saml2Evidence Evidence {
            get {
                return this.evidence;
            }

            set {
                this.evidence = value;
            }
        }
    }
}
