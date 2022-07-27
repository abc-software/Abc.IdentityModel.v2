// ----------------------------------------------------------------------------
// <copyright file="EncryptedType.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Xml {
    using Microsoft.IdentityModel.Xml;
    using System;
    using static Microsoft.IdentityModel.Logging.LogHelper;

    /// <summary>
    /// Represents the abstract base class from which the classes EncryptedData and EncryptedKey derive.
    /// </summary>
    /// <remarks>https://www.w3.org/TR/xmlenc-core/#sec-EncryptedType</remarks>
    public abstract class EncryptedType {
        private CipherData cipherData;
        //private readonly List<string> encryptionProperties = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedType"/> class.
        /// </summary>
        protected EncryptedType() {
        }

        /// <summary>
        /// Gets or sets the identifier to the element within the document context.
        /// </summary>
        /// <value>
        /// The identifier to the element within the document context.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the media type of the data which has been encrypted.
        /// </summary>
        /// <value>
        /// The media type of the data which has been encrypted.
        /// </value>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the type information about the plaintext form of the encrypted content.
        /// </summary>
        /// <value>
        /// The type information about the plaintext form of the encrypted content.
        /// </value>
        public Uri Type { get; set; }

        /// <summary>
        /// Gets or sets the transfer encoding of the data that has been encrypted.
        /// </summary>
        /// <value>
        /// The transfer encoding of the data that has been encrypted.
        /// </value>
        public Uri Encoding { get; set; }

        /// <summary>
        /// Gets or sets the encryption algorithm applied to the cipher data.
        /// </summary>
        /// <value>
        /// The encryption algorithm applied to the cipher data.
        /// </value>
        public EncryptionMethod EncryptionMethod { get; set; }

        /// <summary>
        /// Gets of sets the <see cref="KeyInfo"/> element in XML encryption.
        /// </summary>
        public KeyInfo KeyInfo { get; set; }

        /// <summary>
        /// Gets the <see cref="CipherData"/> value for an instance of an <see cref="EncryptedType"/> class.
        /// </summary>
        public CipherData CipherData {
            get => cipherData;
            set => cipherData = value ?? throw LogArgumentNullException(nameof(value));
        }
    }
}
