// ----------------------------------------------------------------------------
// <copyright file="Saml2AuthenticationRequest.cs" company="ABC Software Ltd">
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
#if NET40 || NET35
    using Diagnostic;
#else
    using Abc.Diagnostics;
#endif

    /// <summary>
    /// The <c>AuthenticationRequest</c> class requests an identity provider authenticate the user and return a 
    /// response with the user's claims.
    /// </summary>
    /// <remarks>See the samlp:AuthnRequest element defined in [SamlCore, 3.4.1] for more details.</remarks>
    internal class Saml2AuthenticationRequest : Saml2Request {
        private readonly ICollection<Metadata.RequestedAttribute> requestedAttributes = new Collection<Metadata.RequestedAttribute>();
        private bool forceAuthentication;
        private bool isPassive;
        private Uri protocolBinding;
        private ushort? assertionConsumerServiceIndex;
        private Uri assertionConsumerServiceUrl;
        private ushort? attributeConsumingServiceIndex;
        private string providerName;
        private Saml2Subject subject;
        private Saml2NameIdentifierPolicy nameIdentifierPolicy;
        private Saml2Conditions conditions;
        private Saml2RequestedAuthenticationContext requestedAuthenticationContext;
        private Saml2Scoping scoping;

        /// <summary>
        /// Gets or sets a value indicating whether the identity provider MUST authenticate the presenter 
        /// directly rather than rely on a previous security context.
        /// </summary>
        /// <remarks>See [SamlCore, 3.4.1] for more details.</remarks>
        /// <value>Whether the identity provider MUST authenticate the presenter 
        /// directly rather than rely on a previous security context.</value>
        public bool ForceAuthentication {
            get { return this.forceAuthentication; }
            set { this.forceAuthentication = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the identity provider and the user agent itself MUST NOT 
        /// visibly take control of the user interface from the requester and
        /// interact with the presenter in a noticeable fashion. 
        /// </summary>
        /// <remarks>See [SamlCore, 3.4.1] for more details.</remarks>
        /// <value>Whether the identity provider and the user agent itself MUST NOT 
        /// visibly take control of the user interface from the requester and
        /// interact with the presenter in a noticeable fashion.</value>
        public bool IsPassive {
            get { return this.isPassive; }
            set { this.isPassive = value; }
        }

        /// <summary>
        /// Gets or sets a URI that identifies the SAML protocol binding to be used 
        /// when returning the Response message. 
        /// </summary>
        /// <remarks>See [SamlCore, 3.4.1] for more details.</remarks>
        /// <details>
        /// This property may not be set when the AssertionConsumerServiceIndex 
        /// property is set.
        /// </details>
        /// <value>A URI that identifies the SAML protocol binding to be used 
        /// when returning the Response message.</value>
        public Uri ProtocolBinding {
            get {
                return this.protocolBinding;
            }

            set {
                if (value != null && this.assertionConsumerServiceIndex.HasValue) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperInvalidOperation("ProtocolBinding cannot be set when AssertionConsumerServiceIndex is set.");
                }

                this.protocolBinding = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that indirectly identifies the location to which the Response message 
        /// should be returned to the requester.
        /// </summary>
        /// <remarks>See [SamlCore, 3.4.1] for more details.</remarks>
        /// <details>
        /// This property may not be set when either ProtocolBinding or 
        /// AssertionConsumingServiceUrl is set.
        /// </details>
        /// <value>A value that indirectly identifies the location to which the Response message 
        /// should be returned to the requester.</value>
        public ushort? AssertionConsumerServiceIndex {
            get {
                return this.assertionConsumerServiceIndex;
            }

            set {
                if (value.HasValue && (this.protocolBinding != null || this.assertionConsumerServiceUrl != null)) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperInvalidOperation("AssertionConsumerServiceIndex cannot be sent when ProtocolBinding or AssertionConsumerServiceUrl are set.");
                }

                this.assertionConsumerServiceIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the location to which the Response message MUST 
        /// be returned to the requester.
        /// </summary>
        /// <remarks>See [SamlCore, 3.4.1] for more details.</remarks>
        /// <details>
        /// This property may not be set when AssertionConsumerServiceIndex is
        /// set.
        /// </details>
        /// <value>The location to which the Response message MUST 
        /// be returned to the requester.</value>
        public Uri AssertionConsumerServiceUrl {
            get {
                return this.assertionConsumerServiceUrl;
            }

            set {
                if (value != null && this.assertionConsumerServiceIndex.HasValue) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperInvalidOperation("AssertionConsumerServiceUrl cannot be set when AssertionConsumerServiceIndex is set.");
                }

                this.assertionConsumerServiceUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that indirectly identifies the SAML attributes the requester desires or requires
        /// to be supplied by the identity provider in the Response message.
        /// </summary>
        /// <remarks>See [SamlCore, 3.4.1] for more details.</remarks>
        /// <value>A value that indirectly identifies the SAML attributes the requester desires or requires
        /// to be supplied by the identity provider in the Response message.</value>
        public ushort? AttributeConsumingServiceIndex {
            get { return this.attributeConsumingServiceIndex; }
            set { this.attributeConsumingServiceIndex = value; }
        }

        /// <summary>
        /// Gets or sets the constraints on the name identifier to be used to 
        /// represent the requested subject. 
        /// </summary>
        /// <remarks>See [SamlCore, 3.4.1] for more details.</remarks>
        /// <value>The constraints on the name identifier to be used to 
        /// represent the requested subject.</value>
        public string ProviderName {
            get { return this.providerName; }
            set { this.providerName = !string.IsNullOrEmpty(value) ? value : null; }
        }

        /// <summary>
        /// Gets or sets the requested subject of the resulting assertion(s).
        /// </summary>
        /// <value>
        /// The requested subject of the resulting assertion(s).
        /// </value>
        /// <remarks>
        /// See [SamlCore, 3.4.1] for more details.
        /// </remarks>
        public Saml2Subject Subject {
            get { return this.subject; }
            set { this.subject = value; }
        }

        /// <summary>
        /// Gets or sets the constraints on the name identifier to be used to
        /// represent the requested subject.
        /// </summary>
        /// <value>
        /// The constraints on the name identifier to be used to
        /// represent the requested subject.
        /// </value>
        /// <remarks>
        /// See [SamlCore, 3.4.1] for more details.
        /// </remarks>
        public Saml2NameIdentifierPolicy NameIdentifierPolicy {
            get { return this.nameIdentifierPolicy; }
            set { this.nameIdentifierPolicy = value; }
        }

        /// <summary>
        /// Gets or sets the SAML conditions the requester expects to limit the
        /// validity and/or use of the resulting assertion.
        /// </summary>
        /// <value>
        /// The SAML conditions the requester expects to limit the
        /// validity and/or use of the resulting assertion.
        /// </value>
        /// <remarks>
        /// See [SamlCore, 3.4.1] for more details.
        /// </remarks>
        public Saml2Conditions Conditions {
            get { return this.conditions; }
            set { this.conditions = value; }
        }

        /// <summary>
        /// Gets or sets the requirements, if any, that the requester places on the
        /// authentication context that applies to the responding provider's
        /// authentication of the presenter.
        /// </summary>
        /// <value>
        /// The requirements, if any, that the requester places on the
        /// authentication context that applies to the responding provider's
        /// authentication of the presenter.
        /// </value>
        /// <remarks>
        /// See [SamlCore, 3.4.1] for more details.
        /// </remarks>
        public Saml2RequestedAuthenticationContext RequestedAuthenticationContext {
            get { return this.requestedAuthenticationContext; }
            set { this.requestedAuthenticationContext = value; }
        }

        /// <summary>
        /// Gets or sets a set of identity providers trusted by the requester to
        /// authenticate the presenter, as well as limitations and context
        /// related to proxying of the AuthnRequest message to subsequent
        /// identity providers by the responder.
        /// </summary>
        /// <value>
        /// The set of identity providers trusted by the requester to
        /// authenticate the presenter, as well as limitations and context
        /// related to proxying of the AuthnRequest message to subsequent
        /// identity providers by the responder.
        /// </value>
        public Saml2Scoping Scoping {
            get { return this.scoping; }
            set { this.scoping = value; }
        }

        /// <summary>
        /// Gets the requested attributes.
        /// </summary>
        /// <value>
        /// The requested attributes.
        /// </value>
        /// <remarks>
        /// See [SamlReqAttr, 2.1] for more details.
        /// </remarks>
        public ICollection<Metadata.RequestedAttribute> RequestedAttributes {
            get {
                return this.requestedAttributes;
            }
        }
    }
}
