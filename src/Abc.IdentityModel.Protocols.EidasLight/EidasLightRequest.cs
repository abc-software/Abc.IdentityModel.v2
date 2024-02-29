// ----------------------------------------------------------------------------
// <copyright file="EidasLightRequest.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols.EidasLight {
    using System;
    using System.Collections.ObjectModel;

    public class EidasLightRequest : EidasLightMessage {
        /// <summary>
        /// Gets or sets country code of the requesting citizen.
        /// In version 2.0 and prior, for Specific Proxy it is derived from the last part of domain string of subject of the certificate used for signing. 
        /// ISO ALPHA-2 format.
        /// </summary>
        public string CitizenCountryCode { get; set; }

        /// <summary>
        /// Gets LevelOfAssurance are represented with a type and a value.
        /// </summary>
        public Collection<LevelOfAssurance> LevelsOfAssurance { get; } = new Collection<LevelOfAssurance>();

        /// <summary>
        /// 
        /// </summary>
        public Uri NameIdFormat { get; set; }
        public string ProviderName { get; set; }
        public EidasSpType? SpType { get; set; }
        public string SpCountryCode { get; set; }
        public string RequesterId { get; set; }

        /// <summary>
        /// Gets the list of requested attributes.
        /// </summary>
        public Collection<AttributeDefinition> RequestedAttributes { get; } = new Collection<AttributeDefinition>();
    }
}

