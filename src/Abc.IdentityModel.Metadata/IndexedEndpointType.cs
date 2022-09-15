// ----------------------------------------------------------------------------
// <copyright file="IndexedEndpointType.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System;

    /// <summary>
    /// The complex type IndexedEndpointType extends EndpointType with a pair of attributes to permit the
    /// indexing of otherwise identical endpoints so that they can be referenced by protocol messages.
    /// </summary>
    public class IndexedEndpointType : EndpointType {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedEndpointType" /> class that has the specified index, binding, and location.
        /// </summary>
        /// <param name="index">The index for the new instance.</param>
        /// <param name="binding">The URI that represents the binding for the new instance.</param>
        /// <param name="location">The URI that represents the location for the new instance.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="binding" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="location" /> is <c>null</c>.</exception>
        public IndexedEndpointType(ushort index, Uri binding, Uri location)
            : base(binding, location) {
            this.Index = index;
        }

        /// <summary>
        /// Gets or sets unique integer value to the endpoint.
        /// </summary>
        public ushort Index { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether this is the default endpoint.
        /// </summary>
        public bool? IsDefault { get; set; }
    }
}