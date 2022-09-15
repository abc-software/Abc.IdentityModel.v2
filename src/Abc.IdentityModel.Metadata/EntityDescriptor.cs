// ----------------------------------------------------------------------------
// <copyright file="EntityDescriptor.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using Microsoft.IdentityModel.Tokens.Saml2;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// The EntitiesDescriptor element contains the metadata for an optionally named group of SAML entities.
    /// </summary>
    public class EntityDescriptor : DescriptorBase {
        private readonly Collection<ContactPerson> contactPersons = new Collection<ContactPerson>();
        private readonly Collection<SigningMethod> signingMethods = new Collection<SigningMethod>();
        private readonly Collection<DigestMethod> digestMethods = new Collection<DigestMethod>();
        private readonly Collection<Saml2Attribute> entityAttributes = new Collection<Saml2Attribute>();
        private readonly Collection<RoleDescriptor> roleDescriptors = new Collection<RoleDescriptor>();
        private readonly Collection<Uri> additionalMetadataLocations = new Collection<Uri>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDescriptor"/> class that has the specified entity ID.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        public EntityDescriptor(EntityId entityId) {
            this.EntityId = entityId;
        }

        /// <summary>
        /// Specifies the unique identifier of the SAML entity whose metadata is described by the element's contents.
        /// This is a required element.
        /// </summary>
        public EntityId EntityId { get; }

        /// <summary>
        /// Provides an identifier for the federation to which the federation metadata applies.
        /// </summary>
        public string FederationId { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="SigningMethod"/> for this descriptor.
        /// </summary>
        /// <value>
        /// The collection of signing methods. The default is an empty collection.
        /// </value>
        public ICollection<SigningMethod> SigningMethods => this.signingMethods;

        /// <summary>
        /// Gets the collection of <see cref="DigestMethod"/> for this descriptor.
        /// </summary>
        /// <value>
        /// The collection of digest methods. The default is an empty collection.
        /// </value>
        public ICollection<DigestMethod> DigestMethods => this.digestMethods;

        /// <summary>
        /// Gets the collection of <see cref="Saml2Attribute"/> for this descriptor.
        /// </summary>
        /// <value>
        /// The collection of entity attributes. The default is an empty collection.
        /// </value>
        public ICollection<Saml2Attribute> EntityAttributes => this.entityAttributes;

        /// <summary>Gets the collection of one or more role descriptors,or a specialized descriptor that defines an affiliation.</summary>
        /// <returns>The collection of role descriptors.</returns>
        public ICollection<RoleDescriptor> RoleDescriptors => this.roleDescriptors;

        /// <summary>
        /// Gets or sets one or more role descriptor elements, or a specialized descriptor that defines an affiliation
        /// </summary>
        public AffiliationDescriptor AffiliationDescriptor { get; set; }

        /// <summary>
        /// Gets the collection of namespace-qualified locations where additional metadata exists for the entity.
        /// </summary>
        public ICollection<Uri> AdditionalMetadataLocations => this.additionalMetadataLocations;

        /// <summary>
        /// Gets or sets the organization for the entity described.
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// Gets the sequence identifying various kinds of contact personnel.
        /// </summary>
        public ICollection<ContactPerson> ContactPersons => this.contactPersons;
    }
}