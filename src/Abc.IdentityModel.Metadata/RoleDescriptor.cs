// ----------------------------------------------------------------------------
// <copyright file="RoleDescriptor.cs" company="ABC software Ltd">
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

    public abstract class RoleDescriptor : DescriptorBase {
        private readonly Collection<Uri> protocolsSupported = new Collection<Uri>();
        private readonly Collection<ContactPerson> contactPersons = new Collection<ContactPerson>();
        private readonly Collection<KeyDescriptor> keyDescriptors = new Collection<KeyDescriptor>();

        /// <summary>Gets the collection of protocols supported.</summary>
        /// <returns>The collection of supported protocols for this role descriptor.</returns>
        public ICollection<Uri> ProtocolsSupported => this.protocolsSupported;

        /// <summary>Gets or sets the location to direct a user for problem resolution and additional support related to this role.</summary>
        /// <returns>The location to direct a user for problem resolution and additional support related to this role.</returns>
        public Uri ErrorUrl { get; set; }

        /// <summary>
        /// Gets the collection of informations about the cryptographic keys that the entity uses when acting in this role.
        /// </summary>
        public ICollection<KeyDescriptor> KeyDescriptors => this.keyDescriptors;

        /// <summary>
        /// Gets the organization associated with this role.
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// Gets the collection of contacts associated with this role.
        /// </summary>
        public ICollection<ContactPerson> ContactPersons => this.contactPersons;
    }
}