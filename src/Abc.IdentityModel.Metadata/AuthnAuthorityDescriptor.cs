// ----------------------------------------------------------------------------
// <copyright file="AuthnAuthorityDescriptor.cs" company="ABC software Ltd">
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

    public class AuthnAuthorityDescriptor : RoleDescriptor {
        private readonly Collection<EndpointType> authnQueryServices = new Collection<EndpointType>();
        private readonly Collection<EndpointType> assertionIdRequestServices = new Collection<EndpointType>();
        private readonly Collection<Uri> nameIdFormats = new Collection<Uri>();

        /// <summary>
        /// One or more elements of type EndpointType that describe endpoints that support the profile of
        /// the Authentication Query protocol defined in [SAMLProf]. All authentication authorities support at
        /// least one such endpoint, by definition.
        /// </summary>
        public ICollection<EndpointType> AuthnQueryServices => this.authnQueryServices;

        /// <summary>
        /// Zero or more elements of type EndpointType that describe endpoints that support the profile of
        /// the Assertion[E33]Query/Request protocol defined in [SAMLProf] or the special URI binding for
        /// assertion requests defined in [SAMLBind].
        /// </summary>
        public ICollection<EndpointType> AssertionIdRequestServices => this.assertionIdRequestServices;

        /// <summary>
        /// Zero or more elements of type anyURI that enumerate the name identifier formats supported by
        /// this authority.See Section 8.3 of[SAMLCore] for some possible values for this element.
        /// </summary>
        public ICollection<Uri> NameIdFormats => this.nameIdFormats;
    }
}