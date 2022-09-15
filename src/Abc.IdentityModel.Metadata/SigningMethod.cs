// ----------------------------------------------------------------------------
// <copyright file="SigningMethod.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System;

    /// <summary>
    /// Defines the signing method.
    /// </summary>
    public class SigningMethod {
        private Uri algorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="SigningMethod"/> class that has the specified signing algorithm.
        /// </summary>
        /// <param name="algorithm">The signing algorithm URI.</param>
        public SigningMethod(Uri algorithm)
            : this(algorithm, null, null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SigningMethod" /> class that has the specified signing algorithm and key sizes.
        /// </summary>
        /// <param name="algorithm">The signing algorithm URI.</param>
        /// <param name="minKeySize">The smallest key size.</param>
        /// <param name="maxKeySize">The largest key size.</param>
        public SigningMethod(Uri algorithm, uint? minKeySize, uint? maxKeySize) {
            if (algorithm == null) {
                throw new ArgumentNullException(nameof(algorithm));
            }

            if (!algorithm.IsAbsoluteUri) {
                throw new ArgumentException("Must be absolute Uri.", nameof(algorithm));
            }

            this.algorithm = algorithm;
            this.MinKeySize = minKeySize;
            this.MaxKeySize = maxKeySize;
        }

        /// <summary>
        /// Gets or sets the signing method algorithm attribute.
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

        /// <summary>
        /// Gets or sets smallest key size.
        /// </summary>
        public uint? MinKeySize { get; set; }

        /// <summary>
        /// Gets or sets largest key size.
        /// </summary>
        public uint? MaxKeySize { get; set; }
    }
}