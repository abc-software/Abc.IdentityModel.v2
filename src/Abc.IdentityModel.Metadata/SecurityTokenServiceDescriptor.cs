// ----------------------------------------------------------------------------
// <copyright file="SecurityTokenServiceDescriptor.cs" company="ABC software Ltd">
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
    /// Defines the WS-Federation based security token service.
    /// </summary>
    public class SecurityTokenServiceDescriptor : WebServiceDescriptor {
        private readonly Collection<EndpointReference> securityTokenServiceEndpoints = new Collection<EndpointReference>();
        private readonly Collection<EndpointReference> singleSignOutSubscriptionEndpoint = new Collection<EndpointReference>();
        private readonly Collection<EndpointReference> singleSignOutNotificationEndpoint = new Collection<EndpointReference>();
        private readonly Collection<EndpointReference> passiveRequestorEndpoints = new Collection<EndpointReference>();

        /// <summary>
        /// Gets the collection of endpoint address of a security token service that supports the WS-Federation and WS-Trust interfaces.
        /// </summary>
        public ICollection<EndpointReference> SecurityTokenServiceEndpoints => this.securityTokenServiceEndpoints;

        /// <summary>
        /// Gets the collection of endpoint address of a service which can be used to subscribe to federated sign-out messages.
        /// </summary>
        public ICollection<EndpointReference> SingleSignOutSubscriptionEndpoint => this.singleSignOutSubscriptionEndpoint;

        /// <summary>
        /// Gets the collection of endpoint address of a service to which push notifications of sign-out are to be sent.
        /// </summary>
        public ICollection<EndpointReference> SingleSignOutNotificationEndpoint => this.singleSignOutNotificationEndpoint;

        /// <summary>
        /// Gets the collection of endpoint address of a service that supports the WS-Federation Web (Passive) Requestor protocol.
        /// </summary>
        public ICollection<EndpointReference> PassiveRequestorEndpoints => this.passiveRequestorEndpoints;
    }
}