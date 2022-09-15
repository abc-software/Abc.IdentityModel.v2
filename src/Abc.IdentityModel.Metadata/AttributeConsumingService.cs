// ----------------------------------------------------------------------------
// <copyright file="AttributeConsumingService.cs" company="ABC software Ltd">
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
    /// The AttributeConsumingService element defines a particular service offered by the service
    /// provider in terms of the attributes the service requires or desires.
    /// </summary>
    public class AttributeConsumingService {
        private readonly Collection<LocalizedName> serviceNames = new Collection<LocalizedName>();
        private readonly Collection<LocalizedName> serviceDescriptions = new Collection<LocalizedName>();
        private readonly Collection<RequestedAttribute> requestedAttributes = new Collection<RequestedAttribute>();

        /// <summary>
        /// A required attribute that assigns a unique integer value to the endpoint so that it can be
        /// referenced in a protocol message.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// An optional boolean attribute used to designate the default endpoint among an indexed set. If
        /// omitted, the value is assumed to be false.
        /// </summary>
        public bool? IsDefault { get; set; }

        /// <summary>
        /// One or more language-qualified names for the service.
        /// </summary>
        public ICollection<LocalizedName> ServiceNames => this.serviceNames;

        /// <summary>
        /// Zero or more language-qualified strings that describe the service.
        /// </summary>
        public ICollection<LocalizedName> ServiceDescriptions => this.serviceDescriptions;

        /// <summary>
        /// [Required]
        /// A required element specifying attributes required or desired by this service.
        /// </summary>
        public ICollection<RequestedAttribute> RequestedAttributes => this.requestedAttributes;
    }
}