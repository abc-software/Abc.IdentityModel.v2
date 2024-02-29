// ----------------------------------------------------------------------------
// <copyright file="EidasLightResponse.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols.EidasLight {
    using System;
    using System.Collections.ObjectModel;

    public class EidasLightResponse : EidasLightMessage {
        public string InResponseToId { get; set; }
        public Uri Consent { get; set; }
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the level of assurance required to fulfill the request.
        /// </summary>
        public LevelOfAssurance LevelOfAssurance { get; set; }
        public Uri SubjectNameIdFormat { get; set; }
        public string Subject { get; set; }
        public EidasLightResponseStatus Status { get; set; } = new EidasLightResponseStatus();
        public Collection<AttributeDefinition> Attributes { get; } = new Collection<AttributeDefinition>();
    }
}