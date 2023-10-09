// ----------------------------------------------------------------------------
// <copyright file="SamlConstants.cs" company="ABC Software Ltd">
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

#pragma warning disable 1591

namespace Abc.IdentityModel.Protocols.Saml11 {
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Constants is not commented.")]
    [ExcludeFromCodeCoverage]
    public sealed class SamlConstants {
        /// <summary>
        /// The major vesion of SAML1.1
        /// </summary>
        public const int MajorVersion = 1;

        /// <summary>
        /// The minor version of SAML1.1
        /// </summary>
        public const int MinorVersion = 1;

        private SamlConstants() {
        }

        public sealed class IdPrefixes {
            /// <summary>
            /// The SAML Assertion Id prefix.
            /// </summary>
            public const string Assertion = "SamlSecurityToken-";

            /// <summary>
            /// The SAML Request Id prefix.
            /// </summary>
            public const string Request = "SamlRequest-";

            /// <summary>
            /// The SAML Response Id prefix.
            /// </summary>
            public const string Response = "SamlResponse-";

            private IdPrefixes() {
            }
        }

        public sealed class Prefixes {
            /// <summary>
            /// The SAML1.x assertion namespace prefix.
            /// </summary>
            public const string Assertion = "saml";

            /// <summary>
            /// The SAML1.x request-response protocol namespace prefix.
            /// </summary>
            public const string Protocol = "samlp";

            /// <summary>
            /// The SAML1.1 subject based profile namespace prefix.
            /// </summary>
            public const string AssertionSubject = "samlsap";

            private Prefixes() {
            }
        }

        public sealed class Namespaces {
            /// <summary>
            /// The SAML1.x assertion namespace.
            /// </summary>
            public const string Assertion = "urn:oasis:names:tc:SAML:1.0:assertion";

            /// <summary>
            /// The SAML1.x request-response protocol namespace.
            /// </summary>
            public const string Protocol = "urn:oasis:names:tc:SAML:1.0:protocol";

            /// <summary>
            /// The SAML1.1 subject based profile namespace.
            /// </summary>
            public const string AssertionSubject = "urn:oasis:names:tc:SAML:1.1:profiles:assertion:subject";

            private Namespaces() {
            }
        }

        public sealed class Parameters {
            public const string Target = "TARGET";
            public const string Artifact = "SAMLart";
            public const string Response = "SAMLResponse";
            public const string ProviderId = "providerId";
            public const string Shire = "shire";
            public const string Time = "time";
            public const string TargetLower = "target";

            private Parameters() {
            }
        }

        public sealed class ProtocolBindings {
            /// <summary>
            /// browser/artifact (Type 1) profile.
            /// </summary>
            public static readonly Uri HttpArtifact01 = new Uri(HttpArtifact01String);

            /// <summary>
            /// browser/artifact (Type 2) profile.
            /// </summary>
            public static readonly Uri HttpArtifact02 = new Uri(HttpArtifact02String);

            /// <summary>
            /// browser/POST Profile of SAML.
            /// </summary>
            public static readonly Uri HttpPost = new Uri(HttpPostString);

            /// <summary>
            /// SOAP Profile of SAML.
            /// </summary>
            public static readonly Uri Soap = new Uri(SoapString);

            /// <summary>
            ///  Shibboleth Profile of SAML.
            /// </summary>
            public static readonly Uri HttpShibboleth = new Uri(HttpShibbolethString);

            private const string HttpArtifact01String = "urn:oasis:names:tc:SAML:1.0:protocol:artifact-01";
            private const string HttpArtifact02String = "urn:oasis:names:tc:SAML:1.0:protocol:artifact-02";
            private const string HttpPostString = "urn:oasis:names:tc:SAML:1.0:protocol:browser-post";
            private const string SoapString = "urn:oasis:names:tc:SAML:1.0:bindings:SOAP-binding";
            private const string HttpShibbolethString = "urn:mace:shibboleth:1.0:profiles:AuthnRequest";

            private ProtocolBindings() {
            }
        }

        public sealed class AuthenticationMethods {
            // [SamlCore, 7.1.1]
            public static readonly Uri Password = new Uri(PasswordString);

            // [SamlCore, 7.1.2]
            public static readonly Uri Kerberos = new Uri(KerberosString);

            // [SamlCore, 7.1.3]
            public static readonly Uri SecureRemotePassword = new Uri(SecureRemotePasswordString);

            // [SamlCore, 7.1.4]
            public static readonly Uri HardwareToken = new Uri(HardwareTokenString);

            // [SamlCore, 7.1.5]
            public static readonly Uri TlsClient = new Uri(TlsClientString);

            // [SamlCore, 7.1.6]
            public static readonly Uri X509 = new Uri(X509String);

            // [SamlCore, 7.1.7]
            public static readonly Uri Pgp = new Uri(PgpString);

            // [SamlCore, 7.1.8]
            public static readonly Uri Spki = new Uri(SpkiString);

            // [SamlCore, 7.1.9]
            public static readonly Uri Xkms = new Uri(XkmsString);

            // [SamlCore, 7.1.10]
            public static readonly Uri XmlDSig = new Uri(SignatureString);

            // [SamlCore, 7.1.11]
            public static readonly Uri Unspecified = new Uri(UnspecifiedString);

            internal const string PasswordString = "urn:oasis:names:tc:SAML:1.0:am:password";
            internal const string KerberosString = "urn:ietf:rfc:1510";
            internal const string SecureRemotePasswordString = "urn:ietf:rfc:2945";
            internal const string HardwareTokenString = "URI:urn:oasis:names:tc:SAML:1.0:am:HardwareToken";
            internal const string TlsClientString = "urn:ietf:rfc:2246";
            internal const string X509String = "urn:oasis:names:tc:SAML:1.0:am:X509-PKI";
            internal const string PgpString = "urn:oasis:names:tc:SAML:1.0:am:PGP";
            internal const string SpkiString = "urn:oasis:names:tc:SAML:1.0:am:SPKI";
            internal const string XkmsString = "urn:oasis:names:tc:SAML:1.0:am:XKMS";
            internal const string SignatureString = "urn:ietf:rfc:3075";
            internal const string UnspecifiedString = "urn:oasis:names:tc:SAML:1.0:am:unspecified";

            private AuthenticationMethods() {
            }
        }

        /// <summary>
        /// SAML1.1 confirmations methods.
        /// </summary>
        public sealed class ConfirmationMethods {
            /// <summary>
            /// As described in XmlSignature, the KeyInfo element holds a key or information that enables an 
            /// application to obtain a key.
            /// </summary>
            public const string HolderOfKey = "urn:oasis:names:tc:SAML:1.0:cm:holder-of-key";

            /// <summary>
            /// Indicates that no other information is available about the context of use of the assertion.
            /// </summary>
            public const string SenderVouches = "urn:oasis:names:tc:SAML:1.0:cm:sender-vouches";

            /// <summary>
            /// The subject of the assertion is the party that presented a SAML artifact, which the relying party used to 
            /// obtain the assertion from the party that created the artifact.
            /// </summary>
            /// <remarks>
            /// The "Artifact" confirmation method identifier is used by SAML Browser/Artifact profile.
            /// </remarks>
            public const string Assertion = "urn:oasis:names:tc:SAML:1.0:cm:artifact";

            /// <summary>
            /// The subject of the assertion is the bearer of the assertion.
            /// </summary>
            /// <remarks>
            /// The "Bearer" confirmation method identifier is used by SAML Browser/POST Or WS-Federation profile.
            /// </remarks> 
            public const string MethodBearer = "urn:oasis:names:tc:SAML:1.0:cm:bearer";

            private ConfirmationMethods() {
            }
        }

        /// <summary>
        /// SAML1.1 NameIdentifier Format Identifiers.
        /// </summary>
        public sealed class NameIdentifierFormats {
            /// <summary>
            /// The interpretation of the content of the NameQualifier element is left to individual implementations.
            /// </summary>
            public static readonly Uri Unspecified = new Uri(UnspecifiedString);

            /// <summary>
            /// Indicates that the content of the NameIdentifier element is in the form of an email address.
            /// </summary>
            public static readonly Uri EmailAddress = new Uri(EmailAddressString);

            /// <summary>
            /// Indicates that the content of the NameIdentifier element is in the form 
            /// specified for the contents of the X509SubjectName element.
            /// </summary>
            public static readonly Uri X509SubjectName = new Uri(X509SubjectNameString);

            /// <summary>
            /// Indicates that the content of the NameIdentifier element is a Windows domain qualified name. 
            /// </summary>
            public static readonly Uri WindowsDomainQualifiedName = new Uri(WindowsDomainQualifiedNameString);

            /// <summary>
            /// Indicates that the content of the NameIdentifier element is a Shibboleth handle. 
            /// </summary>
            public static readonly Uri ShibbolethQualifiedName = new Uri(ShibbolethNameString);

            internal const string UnspecifiedString = "urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified";
            internal const string EmailAddressString = "urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress";
            internal const string X509SubjectNameString = "urn:oasis:names:tc:SAML:1.1:nameid-format:X509SubjectName";
            internal const string WindowsDomainQualifiedNameString = "urn:oasis:names:tc:SAML:1.1:nameid-format:WindowsDomainQualifiedName";
            internal const string ShibbolethNameString = "urn:mace:shibboleth:1.0:nameIdentifier";

            private NameIdentifierFormats() {
            }
        } 

        public sealed class ActionIdentifiers {
            /// <summary>
            /// Read/Write/Execute/1898 Delete/Control
            /// </summary>
            public const string Actions = "urn:oasis:names:tc:SAML:1.0:action:rwedc";

            /// <summary>
            /// Read/Write/Execute/Delete/Control with Negation
            /// </summary>
            public const string ActionsWithNegation = "urn:oasis:names:tc:SAML:1.0:action:rwedc-negation";

            /// <summary>
            /// GET HEAD PUT POST
            /// </summary>
            public const string ActionsGhpp = "urn:oasis:names:tc:SAML:1.0:action:ghpp";
            
            /// <summary>
            /// The defined actions are the set of UNIX file access permissions expressed in the numeric (octal) notation.
            /// </summary>
            public const string ActionsUnix = "urn:oasis:names:tc:SAML:1.0:action:unix";

            private ActionIdentifiers() {
            }
        }

        /*
        public sealed class Uris {
            public const string SOAPAction = "http://www.oasis-open.org/committees/security";

            private Uris() {
            }
        }*/

        public sealed class AttributeNames {
            public const string AssertionId = "AssertionID";
            public const string AttributeName = "AttributeName";
            public const string AttributeNamespace = "AttributeNamespace";
            public const string AuthenticationInstant = "AuthenticationInstant";
            public const string AuthenticationMethod = "AuthenticationMethod";
            public const string AuthorityKind = "AuthorityKind";
            public const string Binding = "Binding";
            public const string Format = "Format";
            public const string InResponseTo = "InResponseTo";
            public const string Issuer = "Issuer";
            public const string IssueInstant = "IssueInstant";
            public const string Location = "Location";
            public const string MajorVersion = "MajorVersion";
            public const string MinorVersion = "MinorVersion";
            public const string NameQualifier = "NameQualifier";
            public const string NotBefore = "NotBefore";
            public const string NotOnOrAfter = "NotOnOrAfter";
            public const string IPAdress = "IPAddress";
            public const string DnsAddress = "DNSAddress";
            public const string Resource = "Resource";
            public const string DecisionType = "Decision";
            public const string Namespace = "Namespace";
            public const string ResponseId = "ResponseID";
            public const string Recipient = "Recipient";
            public const string Value = "Value";
            public const string RequestId = "RequestID";

            private AttributeNames() {
            }
        }

        public sealed class ElementNames {
            public const string Action = "Action";
            public const string Advice = "Advice";
            public const string Assertion = "Assertion";
            public const string AssertionIdReference = "AssertionIDReference";
            public const string Attribute = "Attribute";
            public const string AttributeDesignator = "AttributeDesignator";
            public const string AttributeStatement = "AttributeStatement";
            public const string AttributeValue = "AttributeValue";
            public const string Audience = "Audience";
            public const string AudienceRestrictionCondition = "AudienceRestrictionCondition";
            public const string AuthenticationStatement = "AuthenticationStatement";
            public const string AuthorityBinding = "AuthorityBinding";
            public const string AuthorizationDecisionStatement = "AuthorizationDecisionStatement";
            public const string Condition = "Condition";
            public const string Conditions = "Conditions";
            public const string ConfirmationMethod = "ConfirmationMethod";
            public const string DoNotCacheCondition = "DoNotCacheCondition";
            public const string Evidence = "Evidence";
            public const string NameIdentifier = "NameIdentifier";
            public const string Profile = "Profile";
            public const string Request = "Request";
            public const string Response = "Response";
            public const string RespondWith = "RespondWith";
            public const string Statement = "Statement";
            public const string Status = "Status";
            public const string StatusCode = "StatusCode";
            public const string StatusMessage = "StatusMessage";
            public const string StatusDetail = "StatusDetail";
            public const string Subject = "Subject";
            public const string SubjectConfirmation = "SubjectConfirmation";
            public const string SubjectConfirmationData = "SubjectConfirmationData";
            public const string SubjectLocality = "SubjectLocality";
            public const string SubjectStatement = "SubjectStatement";
            public const string Query = "Query";
            public const string SubjectQuery = "SubjectQuery";
            public const string AuthenticationQuery = "AuthenticationQuery";
            public const string AttributeQuery = "AttributeQuery";
            public const string AuthorizationDecisionQuery = "AuthorizationDecisionQuery";
            public const string AssertionArtifact = "AssertionArtifact";

            private ElementNames() {
            }
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Reviewed.")]
        public sealed class StatusCodes {
            /*
             * The top-level <StatusCode> values
             */

            /// <summary>
            /// The request succeeded.
            /// </summary>
            public static readonly XmlQualifiedName Success = new XmlQualifiedName(SuccessString, SamlConstants.Namespaces.Protocol);
            private const string SuccessString = "Success";

            /// <summary>
            /// The SAML responder could not process the request because the version of the request message was incorrect.
            /// </summary>
            public static readonly XmlQualifiedName VersionMismatch = new XmlQualifiedName(VersionMismatchString, SamlConstants.Namespaces.Protocol);
            private const string VersionMismatchString = "VersionMismatch";

            /// <summary>
            /// The request could not be performed due to an error on the part of the requester.
            /// </summary>
            public static readonly XmlQualifiedName Requester = new XmlQualifiedName(RequesterString, SamlConstants.Namespaces.Protocol);
            private const string RequesterString = "Requester";

            /// <summary>
            /// The request could not be performed due to an error on the part of the SAML responder or SAML authority.
            /// </summary>
            public static readonly XmlQualifiedName Responder = new XmlQualifiedName(ResponderString, SamlConstants.Namespaces.Protocol);
            private const string ResponderString = "Responder";

            /*
             * The second-level status codes
             */

            /// <summary>
            /// The SAML responder cannot process the request because the protocol version specified in the
            /// request message is a major upgrade from the highest protocol version supported by the responder.
            /// </summary>
            public static readonly XmlQualifiedName RequestVersionTooHigh = new XmlQualifiedName(RequestVersionTooHighString, SamlConstants.Namespaces.Protocol);
            private const string RequestVersionTooHighString = "RequestVersionTooHigh";

            /// <summary>
            /// The SAML responder cannot process the request because the protocol version specified in the
            /// request message is too low.
            /// </summary>
            public static readonly XmlQualifiedName RequestVersionTooLow = new XmlQualifiedName(RequestVersionTooLowString, SamlConstants.Namespaces.Protocol);
            private const string RequestVersionTooLowString = "RequestVersionTooLow";

            /// <summary>
            /// The SAML responder can not process any requests with the protocol version specified in the request.
            /// </summary>
            public static readonly XmlQualifiedName RequestVersionDeprecated = new XmlQualifiedName(RequestVersionDeprecatedString, SamlConstants.Namespaces.Protocol);
            private const string RequestVersionDeprecatedString = "RequestVersionDeprecated";

            /// <summary>
            /// The response message would contain more elements than the SAML responder will return.
            /// </summary>
            public static readonly XmlQualifiedName TooManyResponses = new XmlQualifiedName(TooManyResponsesString, SamlConstants.Namespaces.Protocol);
            private const string TooManyResponsesString = "TooManyResponses";

            /// <summary>
            /// The SAML responder or SAML authority is able to process the request but has chosen not to
            /// respond. This status code MAY be used when there is concern about the secXmlQualifiedNamety context of the
            /// request message or the sequence of request messages received from a particular requester.
            /// </summary>
            public static readonly XmlQualifiedName RequestDenied = new XmlQualifiedName(RequestDeniedString, SamlConstants.Namespaces.Protocol);
            private const string RequestDeniedString = "RequestDenied";

            /// <summary>
            /// The SAML authority does not wish to support resource-specific attribute queries, or the resource
            /// value provided in the request message is invalid or unrecognized.
            /// </summary>
            public static readonly XmlQualifiedName ResourceNotRecognized = new XmlQualifiedName(ResourceNotRecognizedString, SamlConstants.Namespaces.Protocol);
            private const string ResourceNotRecognizedString = "ResourceNotRecognized";

            /// <summary>
            /// Collection of TopLevel codes.
            /// </summary>
            public static readonly ReadOnlyCollection<XmlQualifiedName> TopLevelCodes = new ReadOnlyCollection<XmlQualifiedName>(new XmlQualifiedName[] { Success, VersionMismatch, Requester, Responder });

            private StatusCodes() {
            }
        }

        public sealed class XmlTypes {
            public const string RequestType = "RequestType";
            public const string ResponseType = "ResponseType";
            public const string StatusCodeType = "StatusCodeType";
            public const string StatusDetailType = "StatusDetailType";
            public const string StatusType = "StatusType";
            public const string AuthenticationQueryType = "AuthenticationQueryType";
            public const string AttributeQueryType = "AttributeQueryType";
            public const string AuthorizationDecisionQueryType = "AuthorizationDecisionQueryType";
            public const string SubjectStatementAbstractType = "SubjectStatementAbstractType";

            /// <summary>
            /// The subject subject-based assertions with no statements. 
            /// </summary>
            public const string SubjectStatementType = "SubjectStatementType";

            private XmlTypes() {
            }
        }
    }
}

#pragma warning restore 1591