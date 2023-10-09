﻿// ----------------------------------------------------------------------------
// <copyright file="VersionConverter.cs" company="ABC Software Ltd">
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

namespace Abc.IdentityModel.Http.Converters {
    using System;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// Provides a type converter to convert <see cref="Version"/> objects to and from other representations.
    /// </summary>
    internal sealed class VersionConverter : HttpConverterBase {
        /// <inhertidoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            return new Version((string)value);
        }

        /// <inhertidoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            this.ValidateType(value, typeof(Version));

            Version version = (Version)value;
            return version.ToString();
        }
    }
}
