// ----------------------------------------------------------------------------
// <copyright file="PseudonymServiceDescriptor.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class PseudonymServiceDescriptor : WebServiceDescriptor {
        private readonly Collection<EndpointReference> pseudonymServiceEndpoints = new Collection<EndpointReference>();
        private readonly Collection<EndpointReference> singleSignOutNotificationEndpoints = new Collection<EndpointReference>();

        /// <summary>
        /// Gets the collection of pseudonym service endpoints.
        /// </summary>
        public ICollection<EndpointReference> PseudonymServiceEndpoints => this.pseudonymServiceEndpoints;

        /// <summary>
        /// Gets the collection of single sign out notification endpoints.
        /// </summary>
        public ICollection<EndpointReference> SingleSignOutNotificationEndpoints => this.singleSignOutNotificationEndpoints;
    }
}