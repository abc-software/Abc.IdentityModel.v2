// ----------------------------------------------------------------------------
// <copyright file="EncryptedKey.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Xml {
    /// <summary>
    /// Represents the <see cref="EncryptedKey"/> element in XML encryption. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="EncryptedType" />
    public sealed class EncryptedKey : EncryptedType {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedKey"/> class.
        /// </summary>
        public EncryptedKey() {
        }

        /// <summary>
        /// Gets the associated user readable name with the key value.
        /// </summary>
        /// <value>
        /// The associated user readable name with the key value.
        /// </value>
        public string CarriedKeyName { get; set; }

        /// <summary>
        /// Gets or sets the hint as to which recipient this encrypted key value is intended for.
        /// </summary>
        /// <value>
        /// The hint as to which recipient this encrypted key value is intended for.
        /// </value>
        public string Recipient { get; set; }

        /// <summary>
        /// Gets the reference list.
        /// </summary>
        /// <value>
        /// The reference list.
        /// </value>
        public ReferenceList ReferenceList { get; internal set; } = new ReferenceList();
    }
}
