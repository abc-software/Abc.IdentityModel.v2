// ----------------------------------------------------------------------------
// <copyright file="AffiliationDescriptor.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class AffiliationDescriptor : DescriptorBase {
        private readonly Collection<EntityId> affiliateMembers = new Collection<EntityId>();
        private readonly Collection<KeyDescriptor> keyDescriptors = new Collection<KeyDescriptor>();

        /// <summary>
        /// Specifies the unique identifier of the entity responsible for the affiliation.
        /// </summary>
        public EntityId AffiliationOwnerId { get; }

         /// <summary>
        /// One or more elements enumerating the members of the affiliation by specifying each member's
        /// unique identifier.
        /// </summary>
        public ICollection<EntityId> AffiliateMembers => this.affiliateMembers;

        /// <summary>
        /// Optional sequence of elements that provides information about the cryptographic keys that the
        /// affiliation uses as a whole, as distinct from keys used by individual members of the affiliation,
        /// which are published in the metadata for those entities.
        /// </summary>
        public ICollection<KeyDescriptor> KeyDescriptors => this.keyDescriptors;
    }
}