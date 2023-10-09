#pragma warning disable 1591

namespace Abc.IdentityModel.Protocols.Metadata {
    using System;

    internal sealed class MetadataConstants {
        public sealed class Prefixes {
            /// <summary>
            /// The SAML V2.0 metadata prefix.
            /// </summary>
            public const string Metadata = "md";

            /// <summary>
            /// The SAML V2.0 metadata extension for algorithm prefix.
            /// </summary>
            public const string Alg = "alg";

            /// <summary>
            /// The SAML V2.0 metadata extension for entity attributes prefix.
            /// </summary>
            public const string Attributes = "mdattr";

            /// <summary>
            /// The SAML V2.0 metadata SAML v1.x profile namespace.
            /// </summary>
            public const string Saml1 = "saml1md";

            private Prefixes() {
            }
        }

        public sealed class Namespaces {
            /// <summary>
            /// The SAML V2.0 metadata namespace.
            /// </summary>
            public const string Metadata = "urn:oasis:names:tc:SAML:2.0:metadata";

            /// <summary>
            /// The SAML V2.0 metadata extension for algorithm namespace.
            /// </summary>
            public const string Alg = "urn:oasis:names:tc:SAML:metadata:algsupport";

            /// <summary>
            /// The SAML V2.0 metadata extension for entity attributes namespace.
            /// </summary>
            public const string Attributes = "urn:oasis:names:tc:SAML:metadata:attribute";

            /// <summary>
            /// The SAML V2.0 metadata SAML v1.x profile namespace.
            /// </summary>
            public const string Saml1 = "urn:oasis:names:tc:SAML:profiles:v1metadata";

            private Namespaces() {
            }
        }

        public sealed class AttributeNames {
            public const string Id = "ID";
            public const string EntityId = "entityID";
            public const string ValidUntil = "validUntil";
            public const string CacheDuration = "cacheDuration";
            public const string IsRequired = "isRequired";

            // The SAML V2.0 metadata extension for algorithm attributes.
            public const string Algorithm = "Algorithm";
            public const string MinKeySize = "MinKeySize";
            public const string MaxKeySize = "MaxKeySize";

            // SAML V2.0 protocol extension for requesting attributes per request
            public const string SupportsRequestedAttributes = "supportsRequestedAttributes";

            private AttributeNames() {
            }
        }

        public sealed class ElementNames {
            public const string EntityDescriptor = "EntityDescriptor";
            public const string RoleDescriptor = "RoleDescriptor";
            public const string Extensions = "Extensions";
            public const string RequestedAttribute = "RequestedAttribute";

            // The SAML V2.0 metadata extension for algorithm elements.
            public const string DigestMethod = "DigestMethod";
            public const string SigningMethod = "SigningMethod";

            // The SAML V2.0 metadata extension for entity attributes elements.
            public const string EntityAttributes = "EntityAttributes";

            // The SAML V2.0 metadata SAML v1.x profile elemens.
            public const string SourceID = "SourceID";

            private ElementNames() {
            }
        }

        public sealed class XmlTypes {
            public const string RequestedAttributeType = "RequestedAttributeType";

            // The SAML V2.0 metadata extension for algorithm XSD types.
            public const string DigestMethodType = "DigestMethodType";
            public const string SigningMethodType = "SigningMethodType";

            // The SAML V2.0 metadata extension for entity attributes XSD types.
            public const string EntityAttributesType = "EntityAttributesType";

            private XmlTypes() {
            }
        }

        private MetadataConstants() {
        }
    }
}

#pragma warning restore 1591