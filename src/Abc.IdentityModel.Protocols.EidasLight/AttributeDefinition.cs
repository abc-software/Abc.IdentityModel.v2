// ----------------------------------------------------------------------------
// <copyright file="AttributeDefinition.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols.EidasLight {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class AttributeDefinition {
        public AttributeDefinition(string definition)
            : this(definition, (IEnumerable<string>)null) {
        }

        public AttributeDefinition(string definition, string value) 
            : this(definition, new string[] { value }) {
        }

        public AttributeDefinition(string definition, IEnumerable<string> values) {
            if (string.IsNullOrWhiteSpace(definition)) {
                throw new ArgumentException($"'{nameof(definition)}' cannot be null or whitespace", nameof(definition));
            }

            this.Definition = definition;

            if (values != null) {
                foreach (var value in values) {
                    this.Values.Add(value);
                }
            }
        }

        public string Definition { get; }

        public Collection<string> Values { get; } = new Collection<string>();
    }
}