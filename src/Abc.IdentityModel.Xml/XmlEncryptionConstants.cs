// ----------------------------------------------------------------------------
// <copyright file="XmlEncryptionConstants.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

#pragma warning disable 1591

namespace Abc.IdentityModel.Xml {
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Constants for XML Encryption
    /// </summary>
    /// <remarks>
    /// Definitions for namespace, attributes and elements as defined in http://www.w3.org/TR/xmlenc-core1/
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public class XmlEncryptionConstants {
        public const string Namespace = "http://www.w3.org/2001/04/xmlenc#";
        public const string Prefix = "xenc";

        private XmlEncryptionConstants() {
        }

        public sealed class ElementNames {
            public const string CarriedKeyName = "CarriedKeyName";
            public const string CipherData = "CipherData";
            public const string CipherValue = "CipherValue";
            public const string CipherReference = "CipherReference";
            public const string EncryptionMethod = "EncryptionMethod";
            public const string EncryptionProperties = "EncryptionProperties";
            public const string DataReference = "DataReference";
            public const string EncryptedData = "EncryptedData";
            public const string EncryptedKey = "EncryptedKey";
            public const string KeySize = "KeySize";
            public const string OaepParams = "OAEPparams";
            public const string KeyReference = "KeyReference";
            public const string Recipient = "Recipient";
            public const string ReferenceList = "ReferenceList";

            private ElementNames() {
            }
        }

        public sealed class AttributeNames {
            public const string Id = "Id";
            public const string Type = "Type";
            public const string MimeType = "MimeType";
            public const string Encoding = "Encoding";
            public const string Algorithm = "Algorithm";
            public const string Uri = "URI";
            public const string Recipient = "Recipient";

            private AttributeNames() {
            }
        }

        public sealed class EncryptedDataTypes {
            public static readonly Uri Content = new Uri(ContentString);
            public static readonly Uri Element = new Uri(ElementString);
            public static readonly Uri EncryptedKey = new Uri(EncryptedKeyString);

            internal const string ContentString = Namespace + "Content";
            internal const string ElementString = Namespace +  "Element";
            internal const string EncryptedKeyString = Namespace + "EncryptedKey";

            private EncryptedDataTypes() {
            }
        }

        public sealed class Encodings {
            public static readonly Uri Base64 = new Uri(Base64String);

            internal const string Base64String = "http://www.w3.org/2000/09/xmldsig#base64";

            private Encodings() {
            }
        }

    }
}

#pragma warning restore 1591