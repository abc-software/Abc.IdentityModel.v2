#pragma warning disable 1591

namespace Abc.IdentityModel.Xml {
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Constants for XML Encryption
    /// </summary>
    /// <remarks>
    /// Definitions for namespace, attributes and elements as defined in http://www.w3.org/TR/xmlenc-core1/
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public class XmlEncryption11Constants {
        public const string Namespace = "http://www.w3.org/2009/xmlenc11#";
        public const string Prefix = "xenc11";

        private XmlEncryption11Constants() {
        }

        public sealed class ElementNames {
            public const string MaskGenerationFunction = "MGF";
            public const string Parameters = "Parameters";
            public const string KeyLength = "KeyLength";
            public const string IterationCount = "IterationCount";
            public const string OtherSource = "OtherSource";
            public const string Specified = "Specified";
            public const string Salt = "Salt";
            public const string Pbkdf2Params = "PBKDF2-params";

            private ElementNames() {
            }
        }

        public sealed class AttributeNames {
            public const string Algorithm = "Algorithm";

            private AttributeNames() {
            }
        }

        public sealed class EncryptedDataTypes {
            public const string Exi = "http://www.w3.org/2009/xmlenc11#EXI";

            private EncryptedDataTypes() {
            }
        }
    }
}

#pragma warning restore 1591