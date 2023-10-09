// ----------------------------------------------------------------------------
// <copyright file="HttpConverterBase.cs" company="ABC Software Ltd">
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
    using System.ComponentModel;

    /// <summary>
    /// Provides a base type converter for Http message parts.
    /// </summary>
    public abstract class HttpConverterBase : TypeConverter {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpConverterBase"/> class.
        /// </summary>
        protected HttpConverterBase() {
        }

        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return sourceType == typeof(string) /*|| base.CanConvertFrom(context, sourceType)*/;
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return /*destinationType == typeof(InstanceDescriptor) || */base.CanConvertTo(context, destinationType);
        }

        internal void ValidateType(object value, Type expected) {
            if (value != null && value.GetType() != expected) {
                throw new ArgumentException($"Invalid argument type. Expected '{expected.Name}'.", nameof(value));
            }
        }
    }
}
