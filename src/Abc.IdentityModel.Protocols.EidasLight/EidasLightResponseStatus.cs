// ----------------------------------------------------------------------------
// <copyright file="EidasLightResponseStatus.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols.EidasLight {
    using System;

    /// <summary>
    /// The status information from IdP.
    /// </summary>
    public class EidasLightResponseStatus {
        /// <summary>
        /// Gets or sets if the authentication was a failure.
        /// </summary>
        public bool? Failure { get; set; }

        /// <summary>
        /// Enforced by the SAML2 specifications.
        /// </summary>
        public Tuple<Uri, Uri> StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status message. Optional.
        /// </summary>
        public string StatusMessage { get; set; }
    }
}

