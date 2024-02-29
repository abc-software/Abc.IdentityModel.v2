// ----------------------------------------------------------------------------
// <copyright file="EidasLightMessage.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols.EidasLight {
    using System;

    public abstract class EidasLightMessage {
        /// <summary>
        /// A unique id that is used internally to correlate with the Response.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the issuer of the previous hop, like the Connector provided for the Specific Proxy.
        /// Please do not rely on this information.
        /// Not used in version 2.0.
        /// </summary>
        public string Issuer { get; set; }

        public string RelayState { get; set; }
    }
}

