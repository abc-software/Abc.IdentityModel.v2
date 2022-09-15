// ----------------------------------------------------------------------------
// <copyright file="ContactType.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    /// <summary>
    /// Specifies the type of a contact.
    /// </summary>
    public enum ContactType {
        /// <summary>
        /// A contact with a type other than administrative, billing, support, technical, or unspecified.
        /// </summary>
        Other,

        /// <summary>
        /// A technical contact.
        /// </summary>
        Technical,

        /// <summary>
        /// A support contact.
        /// </summary>
        Support,

        /// <summary>
        /// An administrative contact.
        /// </summary>
        Administrative,

        /// <summary>
        /// A billing contact.
        /// </summary>
        Billing,
    }
}