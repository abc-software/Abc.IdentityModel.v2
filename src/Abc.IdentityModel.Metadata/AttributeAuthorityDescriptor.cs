// ----------------------------------------------------------------------------
// <copyright file="AttributeAuthorityDescriptor.cs" company="ABC software Ltd">
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
    /// The <AttributeAuthorityDescriptor> element extends RoleDescriptorType with content
    /// reflecting profiles specific to attribute authorities, SAML authorities that respond to
    /// <samlp:AttributeQuery> messages.
    /// </summary>
    public class AttributeAuthorityDescriptor : RoleDescriptor {
        private readonly Collection<EndpointType> attributeServices = new Collection<EndpointType>();
        private readonly Collection<EndpointType> assertionIdRequestService = new Collection<EndpointType>();
        private readonly Collection<Uri> nameIdFormats = new Collection<Uri>();
        private readonly Collection<Uri> attributeProfiles = new Collection<Uri>();
        private readonly Collection<Saml2Attribute> attributes = new Collection<Saml2Attribute>();

        /// <summary>
        /// One or more elements of type EndpointType that describe endpoints that support the profile of
        /// the Attribute Query protocol defined in [SAMLProf]. All attribute authorities support at least one
        /// such endpoint, by definition.
        /// </summary>
        public ICollection<EndpointType> AttributeServices => this.attributeServices;

        /// <summary>
        /// Zero or more elements of type EndpointType that describe endpoints that support the profile of
        /// the Assertion[E33]Query/Request protocol defined in [SAMLProf] or the special URI binding for
        /// assertion requests defined in [SAMLBind].
        /// </summary>
        public ICollection<EndpointType> AssertionIdRequestServices => this.assertionIdRequestService;

        /// <summary>
        /// Zero or more elements of type anyURI that enumerate the name identifier formats supported by
        /// this authority.See Section 8.3 of[SAMLCore] for some possible values for this element.
        /// </summary>
        public ICollection<Uri> NameIdFormats => this.nameIdFormats;

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
    }
}