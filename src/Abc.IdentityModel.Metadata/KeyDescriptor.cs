// ----------------------------------------------------------------------------
// <copyright file="KeyDescriptor.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using Microsoft.IdentityModel.Xml;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>Defines the key descriptor.</summary>
    public class KeyDescriptor {
        private KeyInfo keyInfo;
        private Collection<EncryptionMethod> encryptionMethods = new Collection<EncryptionMethod>();

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyDescriptor" /> class by using the specified key info.
        /// </summary>
        /// <param name="keyInfo">The key info for this instance.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="value" /> is <c>null</c>.</exception>
        public KeyDescriptor(KeyInfo keyInfo) {
            this.keyInfo = keyInfo ?? throw new System.ArgumentNullException(nameof(keyInfo));
        }

        /// <summary>
        /// Gets or sets the key info.
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="value" /> is <c>null</c>.</exception>
        public KeyInfo KeyInfo {
            get => this.keyInfo;
            set => this.keyInfo = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the purpose of the key.
        /// </summary>
        public KeyType? Use { get; set; }

        /// <summary>Gets the collection of an algorithms and algorithm-specific settings supported by the entity.</summary>
        /// <returns>The collection of an algorithms and algorithm-specific settings supported by the entity.</returns>
        public ICollection<EncryptionMethod> EncryptionMethods => this.encryptionMethods;
    }
}