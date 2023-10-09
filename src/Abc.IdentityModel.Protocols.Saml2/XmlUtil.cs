// ----------------------------------------------------------------------------
// <copyright file="XmlUtil.cs" company="ABC Software Ltd">
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

namespace Abc.IdentityModel.Protocols {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;
#if NET40
    using Diagnostic;
#else
    using Abc.Diagnostics;
#endif

    /// <summary>
    /// XML utilities.
    /// </summary>
    internal static class XmlUtil {
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

        /// <summary>
        /// Validates the type of the XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="expectedTypeName">Expected name of the XML type.</param>
        /// <param name="expectedTypeNamespace">The expected XML type namespace.</param>
        /// <param name="requireDeclaration">if set to <c>true</c> require declaration.</param>
        /// <exception cref="XmlException">validation fail.</exception>
        public static void ValidateXsiType(XmlReader reader, string expectedTypeName, string expectedTypeNamespace, bool requireDeclaration) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            string value = reader.GetAttribute("type", XmlSchema.InstanceNamespace);
            if (!string.IsNullOrEmpty(value)) {
                XmlQualifiedName name = ResolveQName(reader, value);
                if (!StringComparer.Ordinal.Equals(expectedTypeNamespace, name.Namespace) || !StringComparer.Ordinal.Equals(expectedTypeName, name.Name)) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2003Format(expectedTypeName, expectedTypeNamespace, name.Name, name.Namespace));
                }
            }
            else if (requireDeclaration) {
                throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2026Format(expectedTypeName, expectedTypeNamespace));
            }
        }

        /// <summary>
        /// Validates the type of the XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="expectedTypeName">Expected name of the XML type.</param>
        /// <param name="expectedTypeNamespace">The expected XML type namespace.</param>
        /// <exception cref="XmlException">validation fail.</exception>
        public static void ValidateXsiType(XmlReader reader, string expectedTypeName, string expectedTypeNamespace) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            ValidateXsiType(reader, expectedTypeName, expectedTypeNamespace, false);
        }

        /// <summary>
        /// Resolves the Qualified name of the value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="XmlQualifiedName"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="reader"/> is null 
        /// -or- 
        /// <paramref name="value"/> is null or String.Empty.
        /// </exception>
        public static XmlQualifiedName ResolveQName(XmlReader reader, string value) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            if (string.IsNullOrEmpty(value)) {
                throw new ArgumentNullException(nameof(value));
            }

            string name = value;
            string prefix = string.Empty;
            int index = value.LastIndexOf(':');
            
            if (index > -1) {
                prefix = value.Substring(0, index);
                name = value.Substring(index + 1, value.Length - (index + 1));
            }

            string ns = reader.LookupNamespace(prefix);
            if (ns == null) {
                ns = prefix;
            }

            return new XmlQualifiedName(name, ns);
        }

        /// <summary>
        /// Reads the inner XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="namespaces">The namespaces.</param>
        /// <returns>The inner XML.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="reader"/> is null
        /// </exception>
        internal static XmlElement ReadInnerXml(XmlReader reader, List<XmlNamespaceDeclaration> namespaces) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            string localName = reader.LocalName;
            string namespaceURI = reader.NamespaceURI;
            if (reader.IsEmptyElement) {
                throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(localName, namespaceURI));
            }

            reader.MoveToContent();

            XmlReader reader2 = reader.ReadSubtree();
            reader2.Read();

            MemoryStream w = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(w, Encoding.UTF8);

            writer.WriteStartElement(reader2.Prefix, reader2.LocalName, reader2.NamespaceURI);
            HashSet<string> set = new HashSet<string>();

            if (reader2.HasAttributes) {
                for (int i = 0; i < reader2.AttributeCount; i++) {
                    reader2.MoveToAttribute(i);
                    writer.WriteAttributeString(reader2.Prefix, reader2.LocalName, reader2.NamespaceURI, reader2.Value);
                    if (reader2.Prefix.Equals("xmlns")) {
                        set.Add(reader2.LocalName);
                    }
                }
            }

            if (namespaces != null) {
                foreach (XmlNamespaceDeclaration declaration in namespaces) {
                    if (!set.Contains(declaration.Prefix)) {
                        writer.WriteAttributeString("xmlns", declaration.Prefix, null, declaration.Namespace);
                    }
                }
            }

            reader2.Read();

            bool flag;
            do {
                flag = reader2.NodeType == XmlNodeType.EndElement;
                writer.WriteNode(reader2, true);
            }
            while (!flag);

            reader2.Close();
            reader.Read();
            writer.Flush();
            w.Seek(0L, SeekOrigin.Begin);

            if (w.Length == 0L) {
                throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(localName, namespaceURI));
            }

            using (var reader3 = XmlReader.Create(w, new XmlReaderSettings() { XmlResolver = null, DtdProcessing = DtdProcessing.Prohibit })) {
                XmlDocument document = new XmlDocument() { XmlResolver = null, PreserveWhitespace = true };
                document.Load(reader3);
                return document.DocumentElement;
            }
        }

        /// <summary>
        /// Converts a node's content to a array of Base64 bytes..
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The node's content represented as an array of Base64 bytes.</returns>
        internal static byte[] ReadElementContentAsBase64(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            var buffer = new byte[1024];
            int readbytes = 0;
            using (var stream = new MemoryStream()) {
                while ((readbytes = reader.ReadElementContentAsBase64(buffer, 0, buffer.Length)) > 0) {
                    stream.Write(buffer, 0, readbytes);
                }

                return stream.ToArray();
            }
        }
    }
}
