// ----------------------------------------------------------------------------
// <copyright file="CipherData.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Xml {
    using System;
    using static Microsoft.IdentityModel.Logging.LogHelper;

    /// <summary>
    /// Represents the <see cref="CipherData"/> element in XML encryption. This class cannot be inherited.
    /// </summary>
    /// <remarks>https://www.w3.org/TR/xmlenc-core/#sec-CipherData</remarks>
    public class CipherData {
        /// <summary>
        /// Initializes a new instance of the <see cref="CipherData"/> class with cipher value.
        /// </summary>
        /// <param name="cipherValue">The cipher value.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="cipherValue"/> is null.</exception>
        public CipherData(byte[] cipherValue) {
            CipherValue = cipherValue ?? throw LogArgumentNullException(nameof(cipherValue));
        }

        /// <summary>
        /// Gets the cipher value.
        /// </summary>
        /// <value>
        /// The cipher value.
        /// </value>
        public byte[] CipherValue { get; }
    }
}