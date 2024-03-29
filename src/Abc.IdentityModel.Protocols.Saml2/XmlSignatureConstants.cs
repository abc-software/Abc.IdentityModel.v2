﻿#pragma warning disable 1591

namespace Abc.IdentityModel.Protocols.XmlSignature {
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Constants for XML Signature
    /// </summary>
    /// <remarks>
    /// Definitions for namespace, attributes and elements as defined in http://www.w3.org/TR/xmldsig-core/
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public sealed class XmlSignatureConstants {
        public const string Namespace = "http://www.w3.org/2000/09/xmldsig#";
        public const string Prefix = "ds";

        private XmlSignatureConstants() {
        }

        public sealed class Algorithms {
            // Canonicalization.
            public const string ExcC14N = "http://www.w3.org/2001/10/xml-exc-c14n#";
            public const string ExcC14NWithComments = "http://www.w3.org/2001/10/xml-exc-c14n#WithComments";
            
            // Message Digest
            public const string Sha1 = Namespace + "sha1";
            
            // Enveloped Signature.
            public const string EnvelopedSignature = Namespace + "enveloped-signature";

            private Algorithms() {
            }
        }

        public sealed class AttributeNames {
            public const string Algorithm = "Algorithm";
            public const string Id = "Id";
            public const string Type = "Type";
            public const string Uri = "URI";

            private AttributeNames() {
            }
        }

        public sealed class ElementNames {
            public const string CanonicalizationMethod = "CanonicalizationMethod";
            public const string DigestMethod = "DigestMethod";
            public const string DigestValue = "DigestValue";
            public const string Exponent = "Exponent";
            public const string KeyInfo = "KeyInfo";
            public const string KeyName = "KeyName";
            public const string KeyValue = "KeyValue";
            public const string Modulus = "Modulus";
            public const string Object = "Object";
            public const string Reference = "Reference";
            public const string RetrievalMethod = "RetrievalMethod";
            public const string RsaKeyValue = "RsaKeyValue";
            public const string Signature = "Signature";
            public const string SignatureMethod = "SignatureMethod";
            public const string SignatureValue = "SignatureValue";
            public const string SignedInfo = "SignedInfo";
            public const string Transform = "Transform";
            public const string Transforms = "Transforms";
            public const string X509Certificate = "X509Certificate";
            public const string X509Data = "X509Data";
            public const string X509IssuerName = "X509IssuerName";
            public const string X509IssuerSerial = "X509IssuerSerial";
            public const string X509SerialNumber = "X509SerialNumber";
            public const string X509SKI = "X509SKI";
            public const string X509SubjectName = "X509SubjectName";

            private ElementNames() {
            }
        }
    }
}

#pragma warning restore 1591