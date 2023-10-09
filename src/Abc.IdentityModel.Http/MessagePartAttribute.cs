// ----------------------------------------------------------------------------
// <copyright file="MessagePartAttribute.cs" company="ABC Software Ltd">
//    Copyright © 2010-2019 ABC Software Ltd. All rights reserved.
//
//    This library is free software; you can redistribute it and/or.
//    modify it under the terms of the GNU Lesser General Public
//    License  as published by the Free Software Foundation, either
//    version 3 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with the library. If not, see http://www.gnu.org/licenses/.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Http {
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Applied to fields and properties that form a key/value in a protocol message.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    [DebuggerDisplay("MessagePartAttribute {Name}")]
    public sealed class MessagePartAttribute : Attribute {
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePartAttribute"/> class.
        /// </summary>
        public MessagePartAttribute() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePartAttribute"/> class.
        /// </summary>
        /// <param name="name">
        /// A special name to give the value of this member in the serialized message.
        /// When null or empty, the name of the member will be used in the serialized message.
        /// </param>
        public MessagePartAttribute(string name)
            : this() {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the serialized form of this member in the message.
        /// </summary>
        public string Name {
            get { return this.name; }
            private set { this.name = string.IsNullOrEmpty(value) ? null : value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this member is a required part of the serialized message.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the string value is allowed to be empty in the serialized message.
        /// </summary>
        /// <value>Default is true.</value>
        public bool AllowEmpty { get; set; }
    }
}
