// ----------------------------------------------------------------------------
// <copyright file="IdpSsoDescriptor.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using Microsoft.IdentityModel.Tokens.Saml2;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// The IDPSSODescriptor element extends SSODescriptorType with content reflecting profiles specific
    /// to identity providers supporting SSO.
    /// </summary>
    public class IdpSsoDescriptor : SsoDescriptor {
        internal const int SourceIdLenght = 20;

        private readonly Collection<EndpointType> singleSignOnServices = new Collection<EndpointType>();
        private readonly Collection<EndpointType> nameIdMappingServices = new Collection<EndpointType>();
        private readonly Collection<EndpointType> assertionIdRequestServices = new Collection<EndpointType>();
        private readonly Collection<Uri> attributeProfiles = new Collection<Uri>();
        private readonly Collection<Saml2Attribute> attributes = new Collection<Saml2Attribute>();
        private byte[] sourceId;

        /// <summary>
        /// Optional attribute that indicates to service providers whether or not they can expect an
        /// unsigned &lt;AuthnRequest&gt; message to be accepted by the identity provider.
        /// If omitted, the value is assumed to be false.
        /// </summary>
        public bool? WantAuthnRequestsSigned { get; set; }

        /// <summary>
        /// One or more elements of type EndpointType that describe endpoints that support the profiles of the
        /// Authentication Request protocol defined in [SAMLProf]. All identity providers support at least one
        /// such endpoint, by definition. The ResponseLocation attribute MUST be omitted.
        /// </summary>
        public ICollection<EndpointType> SingleSignOnServices => this.singleSignOnServices;

        /// <summary>
        /// Zero or more elements of type EndpointType that describe endpoints that support the Name
        /// Identifier Mapping profile defined in [SAMLProf]. The ResponseLocation attribute MUST be
        /// omitted.
        /// </summary>
        public ICollection<EndpointType> NameIdMappingServices => this.nameIdMappingServices;

        /// <summary>
        /// Zero or more elements of type EndpointType that describe endpoints that support the profile of
        /// the Assertion[E33]Query/Request protocol defined in [SAMLProf] or the special URI binding for
        /// assertion requests defined in [SAMLBind].
        /// </summary>
        public ICollection<EndpointType> AssertionIdRequestServices => this.assertionIdRequestServices;

        /// <summary>
        /// Zero or more elements of type anyURI that enumerate the attribute profiles supported by this
        /// identity provider.See [SAMLProf] for some possible values for this element.
        /// </summary>
        public ICollection<Uri> AttributeProfiles => this.attributeProfiles;

        /// <summary>
        /// Zero or more elements that identify the SAML attributes supported by the identity provider.
        /// Specific values MAY optionally be included, indicating that only certain values permitted by the
        /// attribute's definition are supported. In this context, "support" for an attribute means that the identity
        /// provider has the capability to include it when delivering assertions during single sign-on.
        /// </summary>
        public ICollection<Saml2Attribute> Attributes => this.attributes;

        /// <summary>
        /// Gets or sets the source identifier.
        /// </summary>
        /// <value>
        /// The source identifier.
        /// </value>
        public byte[] SourceId {
            get {
                return this.sourceId;
            }

            set {
                if (value != null && value.Length != SourceIdLenght) {
                    throw new InvalidOperationException($"Source id parameter has wrong length {value.Length}, expected {SourceIdLenght}.");
                }

                this.sourceId = value;
            }
        }
    }
}