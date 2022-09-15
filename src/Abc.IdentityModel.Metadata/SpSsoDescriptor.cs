// ----------------------------------------------------------------------------
// <copyright file="SpSsoDescriptor.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// The SPSSODescriptor element extends SSODescriptorType with content reflecting profiles specific
    /// to service providers.
    /// </summary>
    public class SpSsoDescriptor : SsoDescriptor
    {
        private readonly Collection<IndexedEndpointType> assertionConsumerServices = new Collection<IndexedEndpointType>();
        private readonly Collection<AttributeConsumingService> attributeConsumingServices = new Collection<AttributeConsumingService>();

        /// <summary>
        /// [Optional]
        /// Optional attribute that indicates whether the samlp:AuthnRequest messages sent by this
        /// service provider will be signed. If omitted, the value is assumed to be false.
        /// </summary>
        public bool? AuthnRequestsSigned { get; set; }

        /// <summary>
        /// [Optional]
        /// Optional attribute that indicates a requirement for the saml:Assertion elements received by
        /// this service provider to be signed. If omitted, the value is assumed to be false. This requirement
        /// is in addition to any requirement for signing derived from the use of a particular profile/binding
        /// combination.
        /// </summary>
        public bool? WantAssertionsSigned { get; set; }

        /// <summary>
        /// [Required]
        /// One element that describe indexed endpoints that support the profiles of the
        /// Authentication Request protocol defined in [SAMLProf]. All service providers support at least one
        /// such endpoint, by definition.
        /// </summary>
        public ICollection<IndexedEndpointType> AssertionConsumerServices => this.assertionConsumerServices;

        /// <summary>
        /// [Optional]
        /// Zero or one element that describe an application or service provided by the service provider
        /// that requires or desires the use of SAML attributes.
        /// </summary>
        public ICollection<AttributeConsumingService> AttributeConsumingServices => this.attributeConsumingServices;
    }
}