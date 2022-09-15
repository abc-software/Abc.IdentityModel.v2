// ----------------------------------------------------------------------------
// <copyright file="AttributeServiceDescriptor.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class AttributeServiceDescriptor : WebServiceDescriptor {
        private readonly Collection<EndpointReference> attributeServiceEndpoints = new Collection<EndpointReference>();
        private readonly Collection<EndpointReference> singleSignOutNotificationEndpoints = new Collection<EndpointReference>();

        /// <summary>
        /// Gets the collection of attribute service endpoints.
        /// </summary>
        public ICollection<EndpointReference> AttributeServiceEndpoints => this.attributeServiceEndpoints;

        /// <summary>
        /// Gets the collection of single sign out notification endpoints.
        /// </summary>
        public ICollection<EndpointReference> SingleSignOutNotificationEndpoints => this.singleSignOutNotificationEndpoints;
    }
}