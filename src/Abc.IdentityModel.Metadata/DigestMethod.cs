// ----------------------------------------------------------------------------
// <copyright file="DigestMethod.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System;

    /// <summary>
    /// Defines the digest method.
    /// </summary>
    public class DigestMethod {
        private Uri algorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigestMethod"/> class.
        /// </summary>
        /// <param name="algorithm">The digest algorithm URI.</param>
        public DigestMethod(Uri algorithm) {
            if (algorithm == null) {
                throw new ArgumentNullException(nameof(algorithm));
            }

            if (!algorithm.IsAbsoluteUri) {
                throw new ArgumentException("Must be absolute Uri.", nameof(algorithm));
            }

            this.algorithm = algorithm;
        }

        /// <summary>
        /// Gets or sets the digest method algorithm attribute.
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