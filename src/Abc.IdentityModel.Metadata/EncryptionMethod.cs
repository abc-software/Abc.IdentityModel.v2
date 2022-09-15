// ----------------------------------------------------------------------------
// <copyright file="EncryptionMethod.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System;

    /// <summary>
    /// EncryptionMethod is an optional element that describes the encryption algorithm applied to the cipher data.
    /// </summary>
    public class EncryptionMethod {
        private Uri algorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionMethod"/> class that has the specified encryption algorithm.
        /// </summary>
        /// <param name="algorithm">The encryption algorithm URI.</param>
        public EncryptionMethod(Uri algorithm) {
            if (algorithm == null) {
                throw new ArgumentNullException(nameof(algorithm));
            }

            if (!algorithm.IsAbsoluteUri) {
                throw new ArgumentException("Must be absolute Uri.", nameof(algorithm));
            }

            this.algorithm = algorithm;
        }

        /// <summary>
        /// Gets or sets the encryption method algorithm attribute.
        /// </summary>
        public Uri Algorithm {
            get {
                return this.algorithm;
            }

            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!value.IsAbsoluteUri) {
                    throw new ArgumentException("Must be absolute Uri.", nameof(value));
                }

                this.algorithm = value;
            }
        }
    }
}