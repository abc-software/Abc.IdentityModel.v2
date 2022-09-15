// ----------------------------------------------------------------------------
// <copyright file="MetadataConstants.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

#pragma warning disable 1591

namespace Abc.IdentityModel.Metadata {
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Constants is not commented.")]
    [ExcludeFromCodeCoverage]
    public sealed class MetadataConstants {
        private MetadataConstants() {
        }

        /// <summary>
        /// The SAML2.0 metadata namespaces.
        /// </summary>
        public sealed class Namespaces {
            /// <summary>
            /// The SAML2.0 protocol namespace.
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

            /// <summary>
            /// The WS-Federation V1.2 namespace.
            /// </summary>
            public const string Federation = "http://docs.oasis-open.org/wsfed/federation/200706";

            /// <summary>
            /// The WS-Federation V1.2 authorization namespace.
            /// </summary>
            public const string Authorization = "http://docs.oasis-open.org/wsfed/authorization/200706";

            /// <summary>
            /// The WS-Addressing V1.0 namespace.
            /// </summary>
            public const string WsAddressing10 = "http://www.w3.org/2005/08/addressing";

            /// <summary>
            /// The XML Schema namespace.
            /// </summary>
            public const string XmlSchema = "http://www.w3.org/2001/XMLSchema-instance";

            /// <summary>
            /// The XML namespace.
            /// </summary>
            public const string Xml = "http://www.w3.org/XML/1998/namespace";

            private Namespaces() {
            }
        }

        /// <summary>
        /// The SAML2.0 metadata prefixes.
        /// </summary>
        public sealed class Prefixes {
            /// <summary>
            /// The SAML2.0 protocol namespace prefix.
            /// </summary>
            public const string Metadata = "mt";

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

            /// <summary>
            /// The WS-Federation V1.2 namespace prefix.
            /// </summary>
            public const string Federation = "fed";

            /// <summary>
            /// The WS-Federation V1.2 authorization prefix.
            /// </summary>
            public const string Authorization = "auth";

            /// <summary>
            /// The WS-Addressing V1.0 prefix.
            /// </summary>
            public const string WsAddressing10 = "wsa";

            /// <summary>
            /// The XML Schema prefix.
            /// </summary>
            public const string XmlSchema = "xsi";

            /// <summary>
            /// The XML prefix.
            /// </summary>
            public const string Xml = "xml";

            /// <summary>
            /// The XML namespace prefix.
            /// </summary>
            public const string Xmlns = "xmlns";

            private Prefixes() {
            }
        }

        internal sealed class AttributeNames {
            public const string EntityId = "entityID";
            public const string Id = "ID";
            public const string Name = "Name";
            public const string ValidUntil = "ValidUntil";
            public const string CacheDuration = "CacheDuration";
            public const string Index = "index";
            public const string IsDefault = "isDefault";
            public const string NameFormat = "NameFormat";
            public const string IsRequired = "isRequired";
            public const string Use = "use";
            public const string Binding = "Binding";
            public const string Location = "Location";
            public const string ResponseLocation = "ResponseLocation";
            public const string ContactType = "contactType";
            public const string WantAuthnRequestsSigned = "WantAuthnRequestsSigned";
            public const string AuthnRequestsSigned = "AuthnRequestsSigned";
            public const string WantAssertionsSigned = "WantAssertionsSigned";
            public const string ErrorURL = "errorURL";
            public const string ProtocolSupportEnumeration = "protocolSupportEnumeration";
            public const string AffiliationOwnerId = "AffiliationOwnerID";

            // The SAML V2.0 metadata extension for algorithm attributes.
            public const string Algorithm = "Algorithm";
            public const string MinKeySize = "MinKeySize";
            public const string MaxKeySize = "MaxKeySize";

            // WS-Federation V1.2 attributes
            public const string Uri = "Uri";
            public const string Optional = "Optional";
            public const string FederationId = "FederationID";

            // XML Schema attributes
            public const string Type = "type";

            // XML attributes
            public const string Lang = "lang";

            private AttributeNames() {
            }
        }

        internal sealed class ElementNames {
            public const string EntitiesDescriptor = "EntitiesDescriptor";
            public const string EntityDescriptor = "EntityDescriptor";
            public const string SpSsoDescriptor = "SPSSODescriptor";
            public const string IdpSsoDescriptor = "IDPSSODescriptor";
            public const string PdpDescriptor = "PDPDescriptor";
            public const string AttributeAuthorityDescriptor = "AttributeAuthorityDescriptor";
            public const string AuthnAuthorityDescriptor = "AuthnAuthorityDescriptor";
            public const string ContactPerson = "ContactPerson";
            public const string Company = "Company";
            public const string GivenName = "GivenName";
            public const string SurName = "SurName";
            public const string EmailAddress = "EmailAddress";
            public const string TelephoneNumber = "TelephoneNumber";
            public const string KeyDescriptor = "KeyDescriptor";
            public const string EncryptionMethod = "EncryptionMethod";
            public const string KeyInfo = "KeyInfo";
            public const string SingleLogoutService = "SingleLogoutService";
            public const string SingleSignOnService = "SingleSignOnService";
            public const string ArtifactResolutionService = "ArtifactResolutionService";
            public const string NameIDFormat = "NameIDFormat";
            public const string AssertionConsumerService = "AssertionConsumerService";
            public const string AttributeConsumingService = "AttributeConsumingService";
            public const string ServiceName = "ServiceName";
            public const string RequestedAttribute = "RequestedAttribute";
            public const string Organization = "Organization";
            public const string OrganizationName = "OrganizationName";
            public const string OrganizationDisplayName = "OrganizationDisplayName";
            public const string OrganizationURL = "OrganizationURL";
            public const string AttributeProfile = "AttributeProfile";
            public const string NameIDMappingService = "NameIDMappingService";
            public const string AssertionIDRequestService = "AssertionIDRequestService";
            public const string AttributeService = "AttributeService";
            public const string AuthnQueryService = "AuthnQueryService";
            public const string AuthzService = "AuthzService";
            public const string ManageNameIDService = "ManageNameIDService";
            public const string ServiceDescription = "ServiceDescription";
            public const string Extensions = "Extensions";
            public const string AffiliationDescriptor = "AffiliationDescriptor";
            public const string RoleDescriptor = "RoleDescriptor";

            // The SAML V2.0 metadata extension for algorithm elements.
            public const string DigestMethod = "DigestMethod";
            public const string SigningMethod = "SigningMethod";

            // The SAML V2.0 metadata extension for entity attributes elements.
            public const string EntityAttributes = "EntityAttributes";

            // The SAML V2.0 metadata SAML v1.x profile elements.
            public const string SourceID = "SourceID";

            // WS-Federation V1.2 elements
            public const string ClaimType = "ClaimType";
            public const string DisplayName = "DisplayName";
            public const string Description = "Description";
            public const string DisplayValue = "DisplayValue";
            public const string Value = "Value";
            public const string AutomaticPseudonyms = "AutomaticPseudonyms";
            public const string ClaimDialect = "ClaimDialect";
            public const string ClaimDialectsOffered = "ClaimDialectsOffered";
            public const string TokenType = "TokenType";

            public const string ClaimTypesOffered = "ClaimTypesOffered";
            public const string ClaimTypesRequested = "ClaimTypesRequested";
            public const string TargetScopes = "TargetScopes";
            public const string TokenTypesOffered = "TokenTypesOffered";
            public const string ApplicationServiceType = "ApplicationServiceType";
            public const string SecurityTokenServiceType = "SecurityTokenServiceType";
            public const string ApplicationServiceEndpoint = "ApplicationServiceEndpoint";
            public const string PassiveRequestorEndpoint = "PassiveRequestorEndpoint";
            public const string SecurityTokenServiceEndpoint = "SecurityTokenServiceEndpoint";

            // WS-Addressing V1.0 elements
            public const string EndpointReference = "EndpointReference";
            public const string Address = "Address";

            private ElementNames() {
            }
        }

        internal sealed class XmlTypes {
            public const string ArtifactResolveType = "ArtifactResolveType";

            private XmlTypes() {
            }
        }
    }
}

#pragma warning restore 1591