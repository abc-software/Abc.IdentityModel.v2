// ----------------------------------------------------------------------------
// <copyright file="KeyType.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    /// <summary>
    /// Defines the key types for the <see cref="KeyDescriptor.Use" /> property.
    /// </summary>
    public enum KeyType {
        /// <summary>
        /// The key is used for signing.
        /// </summary>
        Signing,

        /// <summary>
        /// The key is used for encryption.
        /// </summary>
        Encryption,
    }
}