// ----------------------------------------------------------------------------
// <copyright file="Organization.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Specifies basic information about an organization.
    /// </summary>
    public class Organization {
        private Collection<LocalizedName> names = new Collection<LocalizedName>();
        private Collection<LocalizedName> displayNames = new Collection<LocalizedName>();
        private Collection<LocalizedUri> urls = new Collection<LocalizedUri>();

        /// <summary>
        /// Gets the collection of names associated with the organization.
        /// </summary>
        /// <returns>The collection of names.</returns>
        public ICollection<LocalizedName> Names => this.names;

        /// <summary>
        /// Gets the collection of display names associated with the organization.
        /// </summary>
        /// <returns>The collection of display names.</returns>
        public ICollection<LocalizedName> DisplayNames => this.displayNames;

        /// <summary>
        /// Gets the collection of URLs associated with the organization.
        /// </summary>
        /// <returns>The collection of URL entries.</returns>
        public ICollection<LocalizedUri> Urls => this.urls;
    }
}