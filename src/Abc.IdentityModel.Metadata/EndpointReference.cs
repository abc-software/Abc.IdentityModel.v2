// ----------------------------------------------------------------------------
// <copyright file="EndpointReference.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml;

    /// <summary>
    /// Represents a wsa:EndpointReference element.
    /// </summary>
    public class EndpointReference {
        private readonly Collection<XmlElement> details = new Collection<XmlElement>();
        private readonly Uri uri;

        /// <summary>Initializes a new instance of the <see cref="EndpointReference" /> class with the specified URI.</summary>
        /// <param name="uri">An absolute URI that specifies the address of the endpoint reference.</param>
        /// <exception cref="ArgumentNullException"><paramref name="uri" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="uri" /> is not an absolute URI.</exception>
        public EndpointReference(string uri) {
            if (uri == null) {
                throw new ArgumentNullException(nameof(uri));
            }

            var u = new Uri(uri, UriKind.RelativeOrAbsolute);
            if (!u.IsAbsoluteUri) {
                throw new ArgumentException("Must be absolute Uri.", nameof(uri));
            }

            this.uri = u;
        }

        /// <summary>
        /// Gets the URI that specifies the address of the endpoint reference.
        /// </summary>
        /// <returns>
        /// The address of the endpoint reference.
        /// </returns>
        public Uri Uri => this.uri;

        /// <summary>
        /// Gets a collection of the XML elements that are contained in the endpoint reference.
        /// </summary>
        public ICollection<XmlElement> Details => this.details;
    }
}