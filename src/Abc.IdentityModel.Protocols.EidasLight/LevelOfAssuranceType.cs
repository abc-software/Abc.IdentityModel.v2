// ----------------------------------------------------------------------------
// <copyright file="LevelOfAssuranceType.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols.EidasLight {
    public enum LevelOfAssuranceType {
        /// <summary>
        /// Default value, only one notified level of assurance should be given and should have a valid value (regarding specs).
        /// </summary>
        Notified,

        /// <summary>
        /// Non notified levels of Assurance, the prefix of notified level of assurance cannot be used for these levels of assurance.
        /// </summary>
        NonNotified,
    }
}