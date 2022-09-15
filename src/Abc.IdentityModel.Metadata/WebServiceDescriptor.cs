// ----------------------------------------------------------------------------
// <copyright file="WebServiceDescriptor.cs" company="ABC software Ltd">
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
    /// Defines web descriptor for Security Token, Attribute and Pseudonym services.
    /// </summary>
    public abstract class WebServiceDescriptor : RoleDescriptor {
        private readonly Collection<Uri> tokenTypesOffered = new Collection<Uri>();
        private readonly Collection<Uri> claimDialectsOffered = new Collection<Uri>();
        private readonly Collection<ClaimType> claimTypesOffered = new Collection<ClaimType>();
        private readonly Collection<ClaimType> claimTypesRequested = new Collection<ClaimType>();
        private readonly Collection<EndpointReference> targetScopes = new Collection<EndpointReference>();
        private readonly Collection<Uri> logicalServiceNamesOffered = new Collection<Uri>();

        /// <summary>
        /// Gets or sets the friendly name for this service instance that can be shown in user interfaces.
        /// </summary>
        public string ServiceDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description for this service instance that can be shown 1001 in user interfaces.
        /// </summary>
        public string ServiceDescription { get; set; }

        //// <summary>
        //// Gets the collection of 'logical names' that is associated with the service.
        //// </summary>
        public ICollection<Uri> LogicalServiceNamesOffered => this.logicalServiceNamesOffered;

        /// <summary>
        /// Gets the collection of token types that can be issued by the service.
        /// </summary>
        public ICollection<Uri> TokenTypesOffered => this.tokenTypesOffered;

        /// <summary>
        /// Gets the collection of offered claim types, using the schema provided by the common claim dialect
        /// defined in this specification that can be asserted in security tokens issued by the service.
        /// </summary>
        public ICollection<ClaimType> ClaimTypesOffered => this.claimTypesOffered;

        /// <summary>
        /// Gets the collection of claim types, using the schema provided by the common claim dialect defined in this specification,
        /// that MAY or MUST be present in security tokens requested by the service.
        /// </summary>
        public ICollection<ClaimType> ClaimTypesRequested => this.claimTypesRequested;

        /// <summary>
        /// Gets the collection of dialects, via URI(s), that are accepted in token requests to express the syntax for requested claims.
        /// </summary>
        public ICollection<Uri> ClaimDialectsOffered => this.claimDialectsOffered;

        /// <summary>
        /// Gets or sets the indicator if it automatically maps pseudonyms or applies some form of identity mapping.
        /// </summary>
        public bool? AutomaticPseudonyms { get; set; }

        /// <summary>
        /// Gets the collection of the EPRs that are associated with token scopes of the relying party or STS.
        /// </summary>
        public ICollection<EndpointReference> TargetScopes => this.targetScopes;
    }
}