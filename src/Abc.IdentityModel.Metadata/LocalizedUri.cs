// ----------------------------------------------------------------------------
// <copyright file="LocalizedUri.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System;
    using System.Globalization;

    /// <summary>
    /// Defines a localized URI.
    /// </summary>
    public class LocalizedUri : LocalizedType {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedUri"/> class with the URI and specified culture.
        /// </summary>
        /// <param name="uri">The URI for this instance.</param>
        /// <param name="language">The <see cref="CultureInfo" /> that defines the language for this instance.</param>
        public LocalizedUri(Uri uri, CultureInfo language)
            : base(language) {
            this.Uri = uri;
        }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <returns>The URI.</returns>
        public Uri Uri { get; set; }
    }
}