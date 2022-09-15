// ----------------------------------------------------------------------------
// <copyright file="DescriptorBase.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Tokens.Saml2;
    using System;

    /// <summary>
    /// Defines the descriptor base class.
    /// </summary>
    public abstract class DescriptorBase {
        /// <summary>
        /// Gets or sets the document-unique identifier for the element, typically used as a reference point when signing.
        /// </summary>
        /// <value>
        /// The document-unique identifier for the element, typically used as a reference point when signing.
        /// </value>
        public Saml2Id Id { get; set; }

        /// <summary>
        /// Gets or sets the expiration time of the metadata contained in the element and any contained elements.
        /// </summary>
        /// <value>
        /// The expiration time of the metadata contained in the element and any contained elements.
        /// </value>
        public DateTime? ValidUntil { get; set; }

        /// <summary>
        /// Gets or sets the duration of the cache.
        /// </summary>
        /// <value>
        /// The duration of the cache.
        /// </value>
        public TimeSpan? CacheDuration { get; set; }

        /// <summary>
        /// Gets or sets the signing credentials.
        /// </summary>
        /// <returns>The signing credentials.</returns>
        public SigningCredentials SigningCredentials { get; set; }
    }
}