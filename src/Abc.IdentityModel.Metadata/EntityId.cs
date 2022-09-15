// ----------------------------------------------------------------------------
// <copyright file="EntityId.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System;

    /// <summary>
    /// Represents an entity ID.
    /// </summary>
    public class EntityId {
        private const int MaxLength = 1024;
        private string id;

        /// <summary>Initializes a new instance of the <see cref="EntityId" /> class that has the specified ID.</summary>
        /// <param name="id">The ID with which to initialize the new instance.</param>
        public EntityId(string id) {
            if (id is null) {
                throw new ArgumentNullException(nameof(id));
            }

            if (id.Length > MaxLength) {
                throw new ArgumentException(nameof(id), $"Must be less than {MaxLength} characters.");
            }

            this.id = id;
        }

        /// <summary>Gets or sets the entity ID.</summary>
        /// <returns>The entity ID.</returns>
        /// <exception cref="T:System.ArgumentException">An attempt to set an entity ID longer than 1024 characters occurs.</exception>
        public string Id {
            get {
                return this.id;
            }

            set {
                if (value is null) {
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Length > MaxLength) {
                    throw new ArgumentException(nameof(value), $"Must be less than {MaxLength} characters.");
                }

                this.id = value;
            }
        }
    }
}