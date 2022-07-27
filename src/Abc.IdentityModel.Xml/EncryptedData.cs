// ----------------------------------------------------------------------------
// <copyright file="EncryptedData.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Xml {
    /// <summary>
    /// Represents the <see cref="EncryptedData"/> element in XML encryption. This class cannot be inherited.
    /// </summary>
    /// <remarks>http://www.w3.org/TR/xmlenc-core/#sec-EncryptedData</remarks>
    public sealed class EncryptedData : EncryptedType {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedData"/> class.
        /// </summary>
        public EncryptedData() {
        }
    }
}
