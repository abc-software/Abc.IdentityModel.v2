// ----------------------------------------------------------------------------
// <copyright file="LocalizedName.cs" company="ABC software Ltd">
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
    /// Defines a localized name.
    /// </summary>
    public class LocalizedName : LocalizedType {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedName"/> class with the name and specified culture.
        /// </summary>
        /// <param name="name">The name for this instance.</param>
        /// <param name="language">The <see cref="CultureInfo" /> that defines the language for this instance.</param>
        public LocalizedName(string name, CultureInfo language)
            : base(language) {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <returns>The name.</returns>
        public string Name { get; set; }
    }
}