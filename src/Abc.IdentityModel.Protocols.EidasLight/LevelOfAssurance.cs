// ----------------------------------------------------------------------------
// <copyright file="LevelOfAssurance.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols.EidasLight {
    using System;

    /// <summary>
    /// Level of assurance required to fulfill the request
    /// </summary>
    public class LevelOfAssurance {
        public LevelOfAssurance(Uri value) {
            this.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public LevelOfAssurance(Uri value, LevelOfAssuranceType type) {
            this.Value = value ?? throw new ArgumentNullException(nameof(value));
            this.Type = type;
        }

        public Uri Value { get;  }

        public LevelOfAssuranceType? Type { get; }
    }
}