// ----------------------------------------------------------------------------
// <copyright file="EntitiesDescriptor.cs" company="ABC software Ltd">
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

    public class EntitiesDescriptor : DescriptorBase {
        private readonly Collection<EntitiesDescriptor> entityGroups = new Collection<EntitiesDescriptor>();
        private readonly Collection<EntityDescriptor> entities = new Collection<EntityDescriptor>();

         /// <summary>
        /// Gets or sets the name that identifies a group of entities in the context of some deployment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>Gets the child <see cref="EntityDescriptor" /> for this entities collection.</summary>
        /// <returns>The collection of child <see cref="EntityDescriptor" />.</returns>
        public ICollection<EntityDescriptor> Entities => this.entities;

        /// <summary>Gets the child <see cref="EntitiesDescriptor" /> for this entities collection.</summary>
        /// <returns>The collection of child <see cref="EntitiesDescriptor" /> for this entity.</returns>
        public ICollection<EntitiesDescriptor> EntityGroups => this.entityGroups;
    }
}