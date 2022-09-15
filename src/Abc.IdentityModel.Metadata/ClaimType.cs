// ----------------------------------------------------------------------------
// <copyright file="ClaimType.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System;

    /// <summary>
    /// Represents a specific claim.
    /// </summary>
    public class ClaimType {
        private Uri uri;

        public ClaimType(Uri uri) {
            if (uri == null) {
                throw new ArgumentNullException(nameof(uri));
            }

            if (!uri.IsAbsoluteUri) {
                throw new ArgumentException("Must be absolute Uri.", nameof(uri));
            }

            this.uri = uri;
        }

        /// <summary>
        /// Gets or sets the URI attribute specifies the kind of the claim.
        /// </summary>
        public Uri Uri {
            get {
                return this.uri;
            }

            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!value.IsAbsoluteUri) {
                    throw new ArgumentException("Must be absolute Uri.", nameof(value));
                }

                this.uri = value;
            }
        }

        /// <summary>
        /// Gets or sets the the claim is optional <c>true</c> or required <c>false</c>.
        /// </summary>
        public bool? IsOptional { get; set; }

        /// <summary>
        /// Gets or sets the friendly name for this claim type that can be shown in user interfaces.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description of the semantics for this claim type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the displayable value for a claim returned in a security token.
        /// </summary>
        public string DisplayValue { get; set; }

        /// <summary>
        /// Gets or sets the specific string value to be specified for the claim.
        /// </summary>
        public string Value { get; set; }
    }
}