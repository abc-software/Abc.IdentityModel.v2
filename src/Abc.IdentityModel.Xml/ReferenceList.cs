// ----------------------------------------------------------------------------
// <copyright file="ReferenceList.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Xml {
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the ReferenceList element specified in https://www.w3.org/TR/xmlenc-core1/#sec-ReferenceList.
    /// </summary>
    public class ReferenceList {
        /// <summary>
        /// Gets the referers to EncryptedData elements that were encrypted using the key defined in the enclosing EncryptedKey or DerivedKey element.
        /// </summary>
        public Collection<Uri> DataReferences { get; } = new Collection<Uri>();

        /// <summary>
        /// Gets the referers to EncryptedKey elements that were encrypted using the key defined in the enclosing EncryptedKey or DerivedKey element.
        /// </summary>
        public Collection<Uri> KeyReferences { get; } = new Collection<Uri>();
    }
}