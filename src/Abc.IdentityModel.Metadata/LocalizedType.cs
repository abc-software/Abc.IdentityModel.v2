// ----------------------------------------------------------------------------
// <copyright file="LocalizedType.cs" company="ABC software Ltd">
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
    /// The abstract base class that defines a localized type.
    /// </summary>
    public abstract class LocalizedType {
        private CultureInfo language;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedType" /> class for the specified culture.
        /// </summary>
        /// <param name="language">The culture information.</param>
        protected LocalizedType(CultureInfo language) {
            this.language = language ?? throw new ArgumentNullException(nameof(language));
        }

        /// <summary>
        /// Gets or sets the culture information.
        /// </summary>
        /// <returns>The culture information.</returns>
        /// <exception cref="ArgumentNullException">An attempt to set the property to <c>null</c> occurs.</exception>
        public CultureInfo Language {
            get => this.language;
            set => this.language = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}