// -----------------------------------------------------------------------
// ----------------------------------------------------------------------------
// <copyright file="Base64UrlConverter.cs" company="ABC Software Ltd">
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
// -----------------------------------------------------------------------

namespace Abc.IdentityModel.Http.Converters {
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Provides a type converter to convert BASE64URL string to and from other representations.
    /// </summary>
    public class Base64UrlConverter : HttpConverterBase {
        private const char Character62 = '+';
        private const char UrlCharacter62 = '-';
        private const char Character63 = '/';
        private const char UrlCharacter63 = '_';
        private const char PadCharacter = '=';
        private static string DoublePadCharacter = string.Concat(PadCharacter, PadCharacter);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (value is string) {
                string input = ((string)value).Trim();
                if (input.Length == 0) {
                    return new byte[0];
                }

                return Decode(input);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            this.ValidateType(value, typeof(byte[]));

            if (value == null) {
                return null;
            }

            return Encode((byte[])value);
        }

        /// <summary>
        /// Converts the specified string, which encodes binary data as urlbase-64 digits, to an equivalent 8-bit unsigned integer array.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>An array of 8-bit unsigned integers that is equivalent to <paramref name="s"/>.</returns>
        /// <exception cref="System.FormatException">Illegal base64url string!</exception>
        public static byte[] Decode(string s) {
            if (s == null) {
                throw new ArgumentNullException(nameof(s));
            }

            var length = s.Length;
            if (length < 1) {
                return new byte[0];
            }

            int padCount = 0;
            switch (length % 4) {
                case 0:
                    break;

                case 2:
                    padCount = 2;
                    break;

                case 3:
                    padCount = 1;
                    break;

                default:
                    throw new FormatException("Illegal base64url string!");
            }

            char[] inArray = new char[length + padCount];
            for (int i = 0; i < length; i++) {
                char ch = s[i];
                switch (ch) {
                    case UrlCharacter62:
                        inArray[i] = Character62;
                        break;

                    case UrlCharacter63:
                        inArray[i] = Character63;
                        break;

                    default:
                        inArray[i] = ch;
                        break;
                }
            }

            for (int j = length; j < inArray.Length; j++) {
                inArray[j] = PadCharacter;
            }

            return Convert.FromBase64CharArray(inArray, 0, inArray.Length);
        }

        /// <summary>
        /// Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with urlbase-64 digits.
        /// </summary>
        /// <param name="inArray">An array of 8-bit unsigned integers.</param>
        /// <returns>The string representation, in base 64, of the contents of <paramref name="inArray"/>.</returns>
        public static string Encode(byte[] inArray) {
            if (inArray == null) {
                throw new ArgumentNullException(nameof(inArray));
            }

            string str = Convert.ToBase64String(inArray);
            if (str == null) {
                return null;
            }

            var index = str.Length;
            while (index > 0) {
                if (str[index - 1] != PadCharacter) {
                    break;
                }

                index--;
            }

            var chArray = new char[index];
            for (int i = 0; i < index; i++) {
                var ch = str[i];
                switch (ch) {
                    case Character62:
                        chArray[i] = UrlCharacter62;
                        break;

                    case Character63:
                        chArray[i] = UrlCharacter63;
                        break;

                    default:
                        chArray[i] = ch;
                        break;
                }
            }

            return new string(chArray);
        }
    }
}
