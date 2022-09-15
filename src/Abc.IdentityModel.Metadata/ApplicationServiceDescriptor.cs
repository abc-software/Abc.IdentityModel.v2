// ----------------------------------------------------------------------------
// <copyright file="ApplicationServiceDescriptor.cs" company="ABC software Ltd">
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
    /// Defines the WS-Federation based application service.
    /// </summary>
    public class ApplicationServiceDescriptor : WebServiceDescriptor {
        private readonly Collection<EndpointReference> applicationServiceEndpoints = new Collection<EndpointReference>();
        private readonly Collection<EndpointReference> singleSignOutNotificationEndpoint = new Collection<EndpointReference>();
        private readonly Collection<EndpointReference> passiveRequestorEndpoints = new Collection<EndpointReference>();

        /// <summary>
        /// Gets the collection of endpoint address of a Relying Party application service that supports the WS-Federation and WS-Trust interfaces.
        /// </summary>
        public ICollection<EndpointReference> ApplicationServiceEndpoints => this.applicationServiceEndpoints;

        /// <summary>
        /// Gets the collection of endpoint address of a service to which push notifications of sign-out are to be sent.
        /// </summary>
        public ICollection<EndpointReference> SingleSignOutNotificationEndpoint => this.singleSignOutNotificationEndpoint;

        /// <summary>
        /// Gets the collection of endpoint address of a service that supports the WS-1215 Federation Web (Passive) Requestor protocol.
        /// </summary>
        public ICollection<EndpointReference> PassiveRequestorEndpoints => this.passiveRequestorEndpoints;
    }
}