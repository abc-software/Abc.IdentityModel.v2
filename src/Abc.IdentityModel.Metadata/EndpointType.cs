// ----------------------------------------------------------------------------
// <copyright file="EndpointType.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System;

    /// <summary>
    /// The complex type EndpointType describes a SAML protocol binding endpoint at which a SAML entity can be sent
    /// protocol messages.
    /// </summary>
    public class EndpointType {
        private Uri binding;
        private Uri location;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointType"/> class that has the specified binding and location.
        /// </summary>
        /// <param name="binding">The URI that represents the binding for the new instance.</param>
        /// <param name="location">The URI that represents the location for the new instance.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="binding" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="location" /> is <c>null</c>.</exception>
        public EndpointType(Uri binding, Uri location) {
            this.binding = binding ?? throw new ArgumentNullException(nameof(binding));
            this.location = location ?? throw new ArgumentNullException(nameof(location));
        }

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="value" /> is <c>null</c>.</exception>
        public Uri Binding {
            get => this.binding;
            set => this.binding = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="value" /> is <c>null</c>.</exception>
        public Uri Location {
            get => this.location;
            set => this.location = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the response location.
        /// </summary>
        public Uri ResponseLocation { get; set; }
    }
}