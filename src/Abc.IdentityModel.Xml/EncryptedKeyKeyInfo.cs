// ----------------------------------------------------------------------------
// <copyright file="EncryptedKeyKeyInfo.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Xml {
    using Microsoft.IdentityModel.Xml;
    using static Microsoft.IdentityModel.Logging.LogHelper;

    /// <summary>
    /// Represents a XmlDsig KeyInfo element as per: https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-KeyInfo
    /// </summary>
    public class EncryptedKeyKeyInfo : KeyInfo {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedKeyKeyInfo"/> class.
        /// </summary>
        /// <param name="encryptedKey">The encrypted key.</param>
        public EncryptedKeyKeyInfo(EncryptedKey encryptedKey) {
            this.EncryptedKey = encryptedKey ?? throw LogArgumentNullException(nameof(encryptedKey));
        }

        /// <summary>
        /// Gets the encrypted key.
        /// </summary>
        /// <value>
        /// The encrypted key.
        /// </value>
        public EncryptedKey EncryptedKey { get; }
    }
}
