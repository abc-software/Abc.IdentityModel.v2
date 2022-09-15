// ----------------------------------------------------------------------------
// <copyright file="SsoDescriptor.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// The SSODescriptorType abstract type is a common base type for the concrete types
    /// SPSSODescriptorType and IDPSSODescriptorType, described in subsequent sections. It extends
    /// RoleDescriptorType with elements reflecting profiles common to both identity providers and service
    /// providers that support SSO.
    /// </summary>
    public abstract class SsoDescriptor : RoleDescriptor
    {
        private Collection<IndexedEndpointType> artifactResolutionServices = new Collection<IndexedEndpointType>();
        private Collection<EndpointType> singleLogoutServices = new Collection<EndpointType>();
        private Collection<EndpointType> manageNameIDServices = new Collection<EndpointType>();
        private Collection<Uri> nameIdFormats = new Collection<Uri>();

        /// <summary>
        /// Zero or more elements of type IndexedEndpointType that describe indexed endpoints that
        /// support the Artifact Resolution profile defined in [SAMLProf].
        /// </summary>
        public ICollection<IndexedEndpointType> ArtifactResolutionServices => this.artifactResolutionServices;

        /// <summary>
        /// Zero or one element of type EndpointType that describe endpoints that support the Single
        /// Logout profiles defined in [SAMLProf].
        /// </summary>
        public ICollection<EndpointType> SingleLogoutServices => this.singleLogoutServices;

        /// <summary>
        /// Zero or more elements of type EndpointType that describe endpoints that support the Name
        /// Identifier Management profiles defined in [SAMLProf].
        /// </summary>
        public ICollection<EndpointType> ManageNameIDServices => this.manageNameIDServices;

        /// <summary>
        /// Zero or one element of type anyURI that enumerate the name identifier formats supported by
        /// this system entity acting in this role. See Section 8.3 of [SAMLCore] for some possible values for
        /// this element.
        /// </summary>
        public ICollection<Uri> NameIdFormats => this.nameIdFormats;
    }
}