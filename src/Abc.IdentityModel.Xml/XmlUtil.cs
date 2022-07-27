// ----------------------------------------------------------------------------
// <copyright file="XmlUtil.cs" company="ABC software">
//    Copyright © ABC SOFTWARE. All rights reserved.
//    The source code or its parts to use, reproduce, transfer, copy or
//    keep in an electronic form only from written agreement ABC SOFTWARE.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Xml {
    using System;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// XML utilities.
    /// </summary>
    internal static class XmlUtil2 {
        /// <summary>
        /// Verifies that the ID is a valid ID according to the W3C Extended Markup Language recommendation.
        /// </summary>
        /// <param name="val">The ID to verify.</param>
        /// <returns>
        /// <c>true</c> if it is a valid XML ID, otherwise <c>false</c>.
        /// </returns>
        public static bool VerifyID(string val) {
            if (string.IsNullOrEmpty(val)) {
                return false;
            }

            char c = val[0];
            if ((c < 'A' || c > 'Z') && (c < 'a' || c > 'z') && (c != '_') && (c != ':')) {
                return false;
            }

            return true;
        }
    }

    internal static class XmlReaderExtensions { 
        /// <summary>
        /// Converts a node's content to a array of Base64 bytes..
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The node's content represented as an array of Base64 bytes.</returns>
        internal static byte[] ReadElementContentAsBase64(this XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            if (!reader.CanReadBinaryContent) {
                return Convert.FromBase64String(reader.ReadElementContentAsString());
            }

            var buffer = new byte[1024];
            int count;
            using (var stream = new MemoryStream()) {
                while ((count = reader.ReadElementContentAsBase64(buffer, 0, buffer.Length)) > 0) {
                    stream.Write(buffer, 0, count);
                }

                return stream.ToArray();
            }
        }
    }
}
