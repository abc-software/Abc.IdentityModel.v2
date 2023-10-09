// ----------------------------------------------------------------------------
// <copyright file="Saml2Constants.cs" company="ABC Software Ltd">
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

namespace Abc.IdentityModel.Protocols.Saml2 {
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Constants is not commented.")]
    [ExcludeFromCodeCoverage]
    public sealed class Saml2Constants {
        private Saml2Constants() {
        }

        /// <summary>
        /// The SAML2.0 namespaces.
        /// </summary>
        public sealed class Namespaces {
            /// <summary>
            /// The SAML2.0 protocol namespace.
            /// </summary>
            public const string Protocol = "urn:oasis:names:tc:SAML:2.0:protocol";

            /// <summary>
            /// The SAML2.0 assertion namespace.
            /// </summary>
            public const string Assertion = "urn:oasis:names:tc:SAML:2.0:assertion";

            /// <summary>
            /// The SAML2.0 protocol extension for requesting attributes per request namespace.
            /// </summary>
            public const string ReqAttr = "urn:oasis:names:tc:SAML:protocol:ext:req-attr";

            private Namespaces() {
            }
        }

        /// <summary>
        /// The SAML2.0 prefixes.
        /// </summary>
        public sealed class Prefixes {
            /// <summary>
            /// The SAML2.0 protocol namespace prefix.
            /// </summary>
            public const string Protocol = "samlp";

            /// <summary>
            /// The SAML2.0 assertion namespace prefix.
            /// </summary>
            public const string Assertion = "saml";

            /// <summary>
            /// The SAML2.0 protocol extension for requesting attributes per request prefix.
            /// </summary>
            public const string ReqAttr = "req-attr";

            private Prefixes() {
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
            public static readonly XmlQualifiedName Success = new XmlQualifiedName(SuccessString, Namespace);
            internal const string SuccessString = "Success";

            /// <summary>
            /// The SAML responder could not process the request because the version of the request message was incorrect.
            /// </summary>
            public static readonly XmlQualifiedName VersionMismatch = new XmlQualifiedName(VersionMismatchString, Namespace);
            internal const string VersionMismatchString = "VersionMismatch";

            /// <summary>
            /// The request could not be performed due to an error on the part of the requester.
            /// </summary>
            public static readonly XmlQualifiedName Requester = new XmlQualifiedName(RequesterString, Namespace);
            internal const string RequesterString = "Requester";

            /// <summary>
            /// The request could not be performed due to an error on the part of the SAML responder or SAML authority.
            /// </summary>
            public static readonly XmlQualifiedName Responder = new XmlQualifiedName(ResponderString, Namespace);
            internal const string ResponderString = "Responder";

            /*
             * The second-level status codes
             */

            /// <summary>
            /// The responding provider was unable to successfully authenticate the principal.
            /// </summary>
            public static readonly XmlQualifiedName AuthnFailed = new XmlQualifiedName(AuthnFailedString, Namespace);
            internal const string AuthnFailedString = "AuthnFailed";

            /// <summary>
            /// Unexpected or invalid content was encountered within a &lt;saml:Attribute&gt; or
            /// &lt;saml:AttributeValue&gt; element.
            /// </summary>
            public static readonly XmlQualifiedName InvalidAttrNameOrValue = new XmlQualifiedName(InvalidAttrNameOrValueString, Namespace);
            internal const string InvalidAttrNameOrValueString = "InvalidAttrNameOrValue";

            /// <summary>
            /// The responding provider cannot or will not support the requested name identifier policy.
            /// </summary>
            public static readonly XmlQualifiedName InvalidNameIDPolicy = new XmlQualifiedName(InvalidNameIDPolicyString, Namespace);
            internal const string InvalidNameIDPolicyString = "InvalidNameIDPolicy";

            /// <summary>
            /// The specified authentication context requirements cannot be met by the responder.
            /// </summary>
            public static readonly XmlQualifiedName NoAuthnContext = new XmlQualifiedName(NoAuthnContextString, Namespace);
            internal const string NoAuthnContextString = "NoAuthnContext";

            /// <summary>
            /// Used by an intermediary to indicate that none of the supported identity provider &lt;Loc&gt; elements in an
            /// &lt;IDPList&gt; can be resolved or that none of the supported identity providers are available.
            /// </summary>
            public static readonly XmlQualifiedName NoAvailableIDP = new XmlQualifiedName(NoAvailableIDPString, Namespace);
            internal const string NoAvailableIDPString = "NoAvailableIDP";

            /// <summary>
            /// Indicates the responding provider cannot authenticate the principal passively, as has been requested.
            /// </summary>
            public static readonly XmlQualifiedName NoPassive = new XmlQualifiedName(NoPassiveString, Namespace);
            internal const string NoPassiveString = "NoPassive";

            /// <summary>
            /// Used by an intermediary to indicate that none of the identity providers in an &lt;IDPList&gt; are
            /// supported by the intermediary.
            /// </summary>
            public static readonly XmlQualifiedName NoSupportedIDP = new XmlQualifiedName(NoSupportedIDPString, Namespace);
            internal const string NoSupportedIDPString = "NoSupportedIDP";

            /// <summary>
            /// Used by a session authority to indicate to a session participant that it was not able to propagate logout
            /// to all other session participants.
            /// </summary>
            public static readonly XmlQualifiedName PartialLogout = new XmlQualifiedName(PartialLogoutString, Namespace);
            internal const string PartialLogoutString = "PartialLogout";

            /// <summary>
            /// Indicates that a responding provider cannot authenticate the principal directly and is not permitted to
            /// proxy the request further.
            /// </summary>
            public static readonly XmlQualifiedName ProxyCountExceeded = new XmlQualifiedName(ProxyCountExceededString, Namespace);
            internal const string ProxyCountExceededString = "ProxyCountExceeded";

            /// <summary>
            /// The SAML responder or SAML authority is able to process the request but has chosen not to respond.
            /// This status code MAY be used when there is concern about the security context of the request
            /// message or the sequence of request messages received from a particular requester
            /// </summary>
            public static readonly XmlQualifiedName RequestDenied = new XmlQualifiedName(RequestDeniedString, Namespace);
            internal const string RequestDeniedString = "RequestDenied";

            /// <summary>
            /// The SAML responder or SAML authority does not support the request.
            /// </summary>
            public static readonly XmlQualifiedName RequestUnsupported = new XmlQualifiedName(RequestUnsupportedString, Namespace);
            internal const string RequestUnsupportedString = "RequestUnsupported";

            /// <summary>
            /// The SAML responder cannot process any requests with the protocol version specified in the request.
            /// </summary>
            public static readonly XmlQualifiedName RequestVersionDeprecated = new XmlQualifiedName(RequestVersionDeprecatedString, Namespace);
            internal const string RequestVersionDeprecatedString = "RequestVersionDeprecated";

            /// <summary>
            /// The SAML responder cannot process the request because the protocol version specified in the
            /// request message is a major upgrade from the highest protocol version supported by the responder.
            /// </summary>
            public static readonly XmlQualifiedName RequestVersionTooHigh = new XmlQualifiedName(RequestVersionTooHighString, Namespace);
            internal const string RequestVersionTooHighString = "RequestVersionTooHigh";

            /// <summary>
            /// The SAML responder cannot process the request because the protocol version specified in the
            /// request message is too low.
            /// </summary>
            public static readonly XmlQualifiedName RequestVersionTooLow = new XmlQualifiedName(RequestVersionTooLowString, Namespace);
            internal const string RequestVersionTooLowString = "RequestVersionTooLow";

            /// <summary>
            /// The resource value provided in the request message is invalid or unrecognized.
            /// </summary>
            public static readonly XmlQualifiedName ResourceNotRecognized = new XmlQualifiedName(ResourceNotRecognizedString, Namespace);
            internal const string ResourceNotRecognizedString = "ResourceNotRecognized";

            /// <summary>
            /// The response message would contain more elements than the SAML responder is able to return.
            /// </summary>
            public static readonly XmlQualifiedName TooManyResponses = new XmlQualifiedName(TooManyResponsesString, Namespace);
            internal const string TooManyResponsesString = "TooManyResponses";

            /// <summary>
            /// An entity that has no knowledge of a particular attribute profile has been presented with an attribute
            /// drawn from that profile.
            /// </summary>
            public static readonly XmlQualifiedName UnknownAttrProfile = new XmlQualifiedName(UnknownAttrProfileString, Namespace);
            internal const string UnknownAttrProfileString = "UnknownAttrProfile";

            /// <summary>
            /// The responding provider does not recognize the principal specified or implied by the request.
            /// </summary>
            public static readonly XmlQualifiedName UnknownPrincipal = new XmlQualifiedName(UnknownPrincipalString, Namespace);
            internal const string UnknownPrincipalString = "UnknownPrincipal";

            /// <summary>
            /// The SAML responder cannot properly fulfill the request using the protocol binding specified in the
            /// request.
            /// </summary>
            public static readonly XmlQualifiedName UnsupportedBinding = new XmlQualifiedName(UnsupportedBindingString, Namespace);
            internal const string UnsupportedBindingString = "UnsupportedBinding";

            /// <summary>
            /// Collection of TopLevel codes.
            /// </summary>
            public static readonly ReadOnlyCollection<XmlQualifiedName> TopLevelCodes = new ReadOnlyCollection<XmlQualifiedName>(new XmlQualifiedName[] { Success, VersionMismatch, Requester, Responder });

            internal const string Namespace = "urn:oasis:names:tc:SAML:2.0:status";

            private StatusCodes() {
            }
        }

        /// <summary>
        /// The SAML2.0 NameIdentifier Format Identifiers.
        /// </summary>
        public sealed class NameIdentifierFormats {
            /// <summary>
            /// The interpretation of the content of the NameQualifier element is left to individual implementations.
            /// </summary>
            public static readonly Uri Unspecified = Saml11.SamlConstants.NameIdentifierFormats.Unspecified;

            /// <summary>
            /// Indicates that the content of the NameIdentifier element is in the form of an email address.
            /// </summary>
            public static readonly Uri EmailAddress = Saml11.SamlConstants.NameIdentifierFormats.EmailAddress;

            /// <summary>
            /// Indicates that the content of the NameIdentifier element is in the form 
            /// specified for the contents of the X509SubjectName element.
            /// </summary>
            public static readonly Uri X509SubjectName = Saml11.SamlConstants.NameIdentifierFormats.X509SubjectName;   

            /// <summary>
            /// Indicates that the content of the NameIdentifier element is a Windows domain qualified name. 
            /// </summary>
            public static readonly Uri WindowsDomainQualifiedName = Saml11.SamlConstants.NameIdentifierFormats.WindowsDomainQualifiedName;

            /// <summary>
            /// Indicates that the content of the element is in the form of a Kerberos principal name using the format name[/instance]@REALM.
            /// </summary>
            public static readonly Uri Kerberos = new Uri(KerberosString);

            /// <summary>
            /// Indicates that the content of the element is the identifier of an entity that provides SAML-based services
            /// or is a participant in SAML profiles
            /// </summary>
            public static readonly Uri Entity = new Uri(EntityString);

            /// <summary>
            /// Indicates that the content of the element is a persistent opaque identifier for a principal that is specific to
            /// an identity provider and a service provider or affiliation of service providers.
            /// </summary>
            public static readonly Uri Persistent = new Uri(PersistentString);

            /// <summary>
            /// Indicates that the content of the element is an identifier with transient semantics and SHOULD be treated
            /// as an opaque and temporary value by the relying party.
            /// </summary>
            public static readonly Uri Transient = new Uri(TransientString);

            /// <summary>
            /// Indicate a request that the resulting identifier be encrypted.
            /// </summary>
            public static readonly Uri Encrypted = new Uri(EncryptedString);

            internal const string TransientString = "urn:oasis:names:tc:SAML:2.0:nameid-format:transient";
            internal const string PersistentString = "urn:oasis:names:tc:SAML:2.0:nameid-format:persistent";
            internal const string EncryptedString = "urn:oasis:names:tc:SAML:2.0:nameid-format:encrypted";
            internal const string EntityString = "urn:oasis:names:tc:SAML:2.0:nameid-format:entity";
            internal const string KerberosString = "urn:oasis:names:tc:SAML:2.0:nameid-format:kerberos";

            private NameIdentifierFormats() {
            }
        }

        /// <summary>
        /// The SAML2.0 attribute name formats.
        /// </summary>
        public sealed class AttributeNameFormats {
            public static readonly Uri Basic = new Uri(BasicString);

            public static readonly Uri Uri = new Uri(UriString);

            private const string BasicString = "urn:oasis:names:tc:SAML:2.0:profiles:attribute:basic";
            private const string UriString = "urn:oasis:names:tc:SAML:2.0:attrname-format:uri";

            private AttributeNameFormats() {
            }
        }

        /// <summary>
        /// The following identifiers MAY be used in the Consent attribute defined on the RequestAbstractType and
        /// StatusResponseType complex types to communicate whether a principal gave consent, and under what
        /// conditions, for the message
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Reviewed.")]
        public sealed class ConsentIdentifiers {
            /// <summary>
            /// No claim as to principal consent is being made
            /// </summary>
            public static readonly Uri Unspecified = new Uri(UnspecifiedString);
            internal const string UnspecifiedString = "urn:oasis:names:tc:SAML:2.0:consent:unspecified";

            /// <summary>
            /// Indicates that a principal’s consent has been obtained by the issuer of the message
            /// </summary>
            public static readonly Uri Obtained = new Uri(ObtainedString);
            internal const string ObtainedString = "urn:oasis:names:tc:SAML:2.0:consent:obtained";

            /// <summary>
            /// Indicates that a principal’s consent has been obtained by the issuer of the message at some point prior 
            /// to the action that initiated the message
            /// </summary>
            public static readonly Uri Prior = new Uri(PriorString);
            internal const string PriorString = "urn:oasis:names:tc:SAML:2.0:consent:prior";

            /// <summary>
            /// Indicates that a principal’s consent has been implicitly obtained by the issuer of the message during the
            /// action that initiated the message, as part of a broader indication of consent. Implicit consent is typically
            /// more proximal to the action in time and presentation than prior consent, such as part of a session of
            /// activities.
            /// </summary>
            public static readonly Uri Implicit = new Uri(ImplicitString);
            internal const string ImplicitString = "urn:oasis:names:tc:SAML:2.0:consent:current-implicit";

            /// <summary>
            /// Indicates that a principal’s consent has been explicitly obtained by the issuer of the message during the
            /// action that initiated the message
            /// </summary>
            public static readonly Uri Explicit = new Uri(ExplicitString);
            internal const string ExplicitString = "urn:oasis:names:tc:SAML:2.0:consent:current-explicit";

            /// <summary>
            /// Indicates that the issuer of the message did not obtain consent
            /// </summary>
            public static readonly Uri Unavailable = new Uri(UnavailableString);
            internal const string UnavailableString = "urn:oasis:names:tc:SAML:2.0:consent:unavailable";

            /// <summary>
            /// Indicates that the issuer of the message does not believe that they need to obtain or report consent
            /// </summary>
            public static readonly Uri Inapplicable = new Uri(InapplicableString);
            internal const string InapplicableString = "urn:oasis:names:tc:SAML:2.0:consent:inapplicable";

            private ConsentIdentifiers() {
            }
        }

        /// <summary>
        /// The SAML2.0 protocol bindings.
        /// </summary>
        public sealed class ProtocolBindings {
            public static readonly Uri HttpArtifact = new Uri(HttpArtifactString);
            public static readonly Uri HttpPost = new Uri(HttpPostString);
            public static readonly Uri HttpRedirect = new Uri(HttpRedirectString);
            public static readonly Uri ReverseSoap = new Uri(ReverseSoapString);
            public static readonly Uri Soap = new Uri(SoapString);
            public static readonly Uri Uri = new Uri(UriString);
            public static readonly Uri DeflateEncoding = new Uri(DeflateEncodingString);

            internal const string HttpArtifactString = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Artifact";
            internal const string HttpPostString = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST";
            internal const string HttpRedirectString = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect";
            internal const string ReverseSoapString = "urn:oasis:names:tc:SAML:2.0:bindings:PAOS";
            internal const string SoapString = "urn:oasis:names:tc:SAML:2.0:bindings:SOAP";
            internal const string UriString = "urn:oasis:names:tc:SAML:2.0:bindings:URI";
            internal const string DeflateEncodingString = "urn:oasis:names:tc:SAML:2.0:bindings:URL-Encoding:DEFLATE";

            private ProtocolBindings() {
            }
        }

        public sealed class AuthenticationContextClasses {
            // [Saml2AuthnContext, 3.4.1]
            public static readonly Uri InternetProtocol = new Uri(InternetProtocolString);

            // [Saml2AuthnContext, 3.4.2]
            public static readonly Uri InternetProtocolPassword = new Uri(InternetProtocolPasswordString);

            // [Saml2AuthnContext, 3.4.3]
            public static readonly Uri Kerberos = new Uri(KerberosString);

            // [Saml2AuthnContext, 3.4.4]
            public static readonly Uri MobileOneFactorUnregistered = new Uri(MobileOneFactorUnregisteredString);

            // [Saml2AuthnContext, 3.4.5]
            public static readonly Uri MobileTwoFactorUnregistered = new Uri(MobileTwoFactorUnregisteredString);

            // [Saml2AuthnContext, 3.4.6]
            public static readonly Uri MobileOneFactorContract = new Uri(MobileOneFactorContractString);

            // [Saml2AuthnContext, 3.4.7]
            public static readonly Uri MobileTwoFactorContract = new Uri(MobileTwoFactorContractString);

            // [Saml2AuthnContext, 3.4.8]
            public static readonly Uri Password = new Uri(PasswordString);

            // [Saml2AuthnContext, 3.4.9]
            public static readonly Uri PasswordProtectedTransport = new Uri(PasswordProtectedTransportString);

            // [Saml2AuthnContext, 3.4.10]
            public static readonly Uri PreviousSession = new Uri(PreviousSessionString);

            // [Saml2AuthnContext, 3.4.11]
            public static readonly Uri X509 = new Uri(X509String);

            // [Saml2AuthnContext, 3.4.12]
            public static readonly Uri Pgp = new Uri(PgpString);

            // [Saml2AuthnContext, 3.4.13]
            public static readonly Uri Spki = new Uri(SpkiString);

            // [Saml2AuthnContext, 3.4.14]
            public static readonly Uri XmlDSig = new Uri(XmlDsigString);

            // [Saml2AuthnContext, 3.4.15]
            public static readonly Uri Smartcard = new Uri(SmartcardString);

            // [Saml2AuthnContext, 3.4.16]
            public static readonly Uri SmartcardPki = new Uri(SmartcardPkiString);

            // [Saml2AuthnContext, 3.4.17]
            public static readonly Uri SoftwarePki = new Uri(SoftwarePkiString);

            // [Saml2AuthnContext, 3.4.18]
            public static readonly Uri Telephony = new Uri(TelephonyString);

            // [Saml2AuthnContext, 3.4.19]
            public static readonly Uri NomadTelephony = new Uri(NomadTelephonyString);

            // [Saml2AuthnContext, 3.4.20]
            public static readonly Uri PersonalTelephony = new Uri(PersonalTelephonyString);

            // [Saml2AuthnContext, 3.4.21]
            public static readonly Uri AuthenticatedTelephony = new Uri(AuthenticatedTelephonyString);

            // [Saml2AuthnContext, 3.4.22]
            public static readonly Uri SecureRemotePassword = new Uri(SecureRemotePasswordString);

            // [Saml2AuthnContext, 3.4.23]
            public static readonly Uri TlsClient = new Uri(TlsClientString);

            // [Saml2AuthnContext, 3.4.24]
            public static readonly Uri TimeSyncToken = new Uri(TimeSyncTokenString);

            // [Saml2AuthnContext, 3.4.25]
            public static readonly Uri Unspecified = new Uri(UnspecifiedString);

            internal const string InternetProtocolString = "urn:oasis:names:tc:SAML:2.0:ac:classes:InternetProtocol";
            internal const string InternetProtocolPasswordString = "urn:oasis:names:tc:SAML:2.0:ac:classes:InternetProtocolPassword";
            internal const string KerberosString = "urn:oasis:names:tc:SAML:2.0:ac:classes:Kerberos";
            internal const string MobileOneFactorUnregisteredString = "urn:oasis:names:tc:SAML:2.0:ac:classes:MobileOneFactorUnregistered";
            internal const string MobileTwoFactorUnregisteredString = "urn:oasis:names:tc:SAML:2.0:ac:classes:MobileTwoFactorUnregistered";
            internal const string MobileOneFactorContractString = "urn:oasis:names:tc:SAML:2.0:ac:classes:MobileOneFactorContract";
            internal const string MobileTwoFactorContractString = "urn:oasis:names:tc:SAML:2.0:ac:classes:MobileTwoFactorContract";
            internal const string PasswordString = "urn:oasis:names:tc:SAML:2.0:ac:classes:Password";
            internal const string PasswordProtectedTransportString = "urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport";
            internal const string PreviousSessionString = "urn:oasis:names:tc:SAML:2.0:ac:classes:PreviousSession";
            internal const string X509String = "urn:oasis:names:tc:SAML:2.0:ac:classes:X509";
            internal const string PgpString = "urn:oasis:names:tc:SAML:2.0:ac:classes:PGP";
            internal const string SpkiString = "urn:oasis:names:tc:SAML:2.0:ac:classes:SPKI";
            internal const string XmlDsigString = "urn:oasis:names:tc:SAML:2.0:ac:classes:XMLDSig";
            internal const string SecureRempotePasswordString = "urn:oasis:names:tc:SAML:2.0:ac:classes:SecureRemotePassword";
            internal const string SmartcardString = "urn:oasis:names:tc:SAML:2.0:ac:classes:Smartcard";
            internal const string SmartcardPkiString = "urn:oasis:names:tc:SAML:2.0:ac:classes:SmartcardPKI";
            internal const string SoftwarePkiString = "urn:oasis:names:tc:SAML:2.0:ac:classes:SoftwarePKI";
            internal const string TelephonyString = "urn:oasis:names:tc:SAML:2.0:ac:classes:Telephony";
            internal const string NomadTelephonyString = "urn:oasis:names:tc:SAML:2.0:ac:classes:NomadTelephony";
            internal const string PersonalTelephonyString = "urn:oasis:names:tc:SAML:2.0:ac:classes:PersonalTelephony";
            internal const string AuthenticatedTelephonyString = "urn:oasis:names:tc:SAML:2.0:ac:classes:AuthenticatedTelephony";
            internal const string SecureRemotePasswordString = "urn:oasis:names:tc:SAML:2.0:ac:classes:SecureRemotePassword";
            internal const string TlsClientString = "urn:oasis:names:tc:SAML:2.0:ac:classes:TLSClient";
            internal const string TimeSyncTokenString = "urn:oasis:names:tc:SAML:2.0:ac:classes:TimeSyncToken";
            internal const string UnspecifiedString = "urn:oasis:names:tc:SAML:2.0:ac:classes:Unspecified";

            private AuthenticationContextClasses() {
            }
        }

        internal sealed class LogoutReasons {
            // [Saml2Core, 3.7.3]

            /// <summary>
            /// The user terminates session and initiates logout.
            /// </summary>
            public static readonly Uri User = new Uri(UserString);

            /// <summary>
            /// The administrator terminates session and initiates logout.
            /// </summary>
            public static readonly Uri Admin = new Uri(AdminString);

            internal const string UserString = "urn:oasis:names:tc:SAML:2.0:logout:user";
            internal const string AdminString = "urn:oasis:names:tc:SAML:2.0:logout:admin";

            private LogoutReasons() {
            }
        }

        internal sealed class EncodingAlgorithms {
            // [Saml2Bindings, 3.4.4]
            public static readonly Uri Deflate = new Uri(DeflateString);

            internal const string DeflateString = "urn:oasis:names:tc:SAML:2.0:bindings:URL-Encoding:DEFLATE";

            private EncodingAlgorithms() {
            }
        }

        internal static class ConfirmationMethods {
            public const string BearerString = "urn:oasis:names:tc:SAML:2.0:cm:bearer";
            public const string HolderOfKeyString = "urn:oasis:names:tc:SAML:2.0:cm:holder-of-key";
            public const string SenderVouchesString = "urn:oasis:names:tc:SAML:2.0:cm:sender-vouches";

            /// <summary>
            /// The subject of the assertion is the bearer of the assertion. [Saml2Prof, 3.3]
            /// </summary>
            public static readonly Uri Bearer = new Uri(BearerString);

            /// <summary>
            /// The holder of a specified key is considered to be the subject of the assertion
            /// by the asserting party. [Saml2Prof, 3.1]
            /// </summary>
            public static readonly Uri HolderOfKey = new Uri(HolderOfKeyString);

            /// <summary>
            /// Indicates that no other information is available about the context of use of the 
            /// assertion. [Saml2Prof, 3.2]
            /// </summary>
            public static readonly Uri SenderVouches = new Uri(SenderVouchesString);
        }

        internal sealed class Parameters {
            public const string RelayState = "RelayState";
            public const string SamlRequest = "SAMLRequest";
            public const string SamlResponse = "SAMLResponse";
            public const string SamlArtifact = "SAMLart";
            public const string Signature = "Signature";
            public const string SignatureAlgorithm = "SigAlg";
            public const string SamlEncoding = "SAMLEncoding";

            private Parameters() {
            }
        }

        internal sealed class AttributeNames {
            public const string AllowCreate = "AllowCreate";
            public const string AssertionConsumerServiceIndex = "AssertionConsumerServiceIndex";
            public const string AssertionConsumerServiceURL = "AssertionConsumerServiceURL";
            public const string AttributeConsumingServiceIndex = "AttributeConsumingServiceIndex";
            public const string Comparison = "Comparison";
            public const string Consent = "Consent";
            public const string Destination = "Destination";
            public const string ForceAuthn = "ForceAuthn";
            public const string Format = "Format";
            public const string ID = "ID";
            public const string InResponseTo = "InResponseTo";
            public const string IsPassive = "IsPassive";
            public const string IssueInstant = "IssueInstant";
            public const string Loc = "Loc";
            public const string Name = "Name";
            public const string NotOnOrAfter = "NotOnOrAfter";
            public const string ProtocolBinding = "ProtocolBinding";
            public const string ProviderID = "ProviderID";
            public const string ProviderName = "ProviderName";
            public const string ProxyCount = "ProxyCount";
            public const string Reason = "Reason";
            public const string Resource = "Resource";
            public const string SessionIndex = "SessionIndex";
            public const string SPNameQualifier = "SPNameQualifier";
            public const string Value = "Value";
            public const string Version = "Version";
            public const string NameFormat = "NameFormat";
            public const string FriendlyName = "FriendlyName";

            public const string Address = "Address";
            public const string AuthnInstant = "AuthnInstant";
            public const string Count = "Count";
            public const string Decision = "Decision";
            public const string DNSName = "DNSName";
            public const string Method = "Method";
            public const string NameQualifier = "NameQualifier";
            public const string Namespace = "Namespace";
            public const string NotBefore = "NotBefore";
            public const string OriginalIssuer = "OriginalIssuer";
            public const string Recipient = "Recipient";
            public const string SessionNotOnOrAfter = "SessionNotOnOrAfter";
            public const string SPProvidedID = "SPProvidedID";

            private AttributeNames() {
            }
        }

        internal sealed class ElementNames {
            public const string Artifact = "Artifact";
            public const string ArtifactResolve = "ArtifactResolve";
            public const string ArtifactResponse = "ArtifactResponse";
            public const string AssertionIDRequest = "AssertionIDRequest";
            public const string AssertionIDRef = "AssertionIDRef";
            public const string AuthnRequest = "AuthnRequest";
            public const string AuthnQuery = "AuthnQuery";
            public const string AuthzDecisionQuery = "AuthzDecisionQuery";
            public const string AttributeQuery = "AttributeQuery";
            public const string Conditions = "Conditions";
            public const string Extensions = "Extensions";
            public const string GetComplete = "GetComplete";
            public const string IDPEntry = "IDPEntry";
            public const string IDPList = "IDPList";
            public const string LogoutRequest = "LogoutRequest";
            public const string LogoutResponse = "LogoutResponse";
            public const string ManageNameIDRequest = "ManageNameIDRequest";
            public const string ManageNameIDResponse = "ManageNameIDResponse";
            public const string NameIDMappingRequest = "NameIDMappingRequest";
            public const string NameIDMappingResponse = "NameIDMappingResponse";
            public const string NameIDPolicy = "NameIDPolicy";
            public const string NewID = "NewID";
            public const string NewEncryptedID = "NewEncryptedID";
            public const string RequestedAuthnContext = "RequestedAuthnContext";
            public const string RequesterID = "RequesterID";
            public const string Response = "Response";
            public const string Scoping = "Scoping";
            public const string SessionIndex = "SessionIndex";
            public const string Subject = "Subject";
            public const string Status = "Status";
            public const string StatusCode = "StatusCode";
            public const string StatusDetail = "StatusDetail";
            public const string StatusMessage = "StatusMessage";
            public const string Terminate = "Terminate";
            public const string Assertion = "Assertion";
            public const string EncryptedAssertion = "EncryptedAssertion";
            public const string Evidence = "Evidence";
            public const string Action = "Action";
            public const string Attribute = "Attribute";
            public const string AttributeValue = "AttributeValue";
            public const string AuthnContext = "AuthnContext";
            public const string AuthnContextClassRef = "AuthnContextClassRef";
            public const string AuthnContextDecl = "AuthnContextDecl";
            public const string AuthnContextDeclRef = "AuthnContextDeclRef";
            public const string Issuer = "Issuer";
            public const string Statement = "Statement";
            public const string AttributeStatement = "AttributeStatement";
            public const string AuthnStatement = "AuthnStatement";
            public const string AuthzDecisionStatement = "AuthzDecisionStatement";

            public const string Advice = "Advice";
            public const string AssertionURIRef = "AssertionURIRef";
            public const string Audience = "Audience";
            public const string AudienceRestriction = "AudienceRestriction";
            public const string AuthenticatingAuthority = "AuthenticatingAuthority";
            public const string BaseID = "BaseID";
            public const string Condition = "Condition";
            public const string EncryptedAttribute = "EncryptedAttribute";
            public const string EncryptedID = "EncryptedID";
            public const string NameID = "NameID";
            public const string OneTimeUse = "OneTimeUse";
            public const string ProxyRestricton = "ProxyRestriction";
            public const string SubjectConfirmation = "SubjectConfirmation";
            public const string SubjectConfirmationData = "SubjectConfirmationData";
            public const string SubjectLocality = "SubjectLocality";

            // SAML V2.0 Protocol Extension For Requesting  Attributes Per Request
            public const string RequestedAttributes = "RequestedAttributes";

            public ElementNames() {
            }
        }

        internal sealed class XmlTypes {
            public const string ArtifactResolveType = "ArtifactResolveType";
            public const string ArtifactResponseType = "ArtifactResponseType";
            public const string AssertionIDRequestType = "AssertionIDRequestType";
            public const string AuthnRequestType = "AuthnRequestType";
            public const string AuthnQueryType = "AuthnQueryType";
            public const string AuthzDecisionQueryType = "AuthzDecisionQueryType";
            public const string AttributeQueryType = "AttributeQueryType";
            public const string ExtensionsType = "ExtensionsType";
            public const string IDPEntryType = "IDPEntryType";
            public const string IDPListType = "IDPListType";
            public const string LogoutRequestType = "LogoutRequestType";
            public const string NameIDPolicyType = "NameIDPolicyType";
            public const string RequestedAuthnContextType = "RequestedAuthnContextType";
            public const string ResponseType = "ResponseType";
            public const string ScopingType = "ScopingType";
            public const string StatusCodeType = "StatusCodeType";
            public const string StatusDetailType = "StatusDetailType";
            public const string StatusResponseType = "StatusResponseType";
            public const string StatusType = "StatusType";
            public const string ManageNameIDRequestType = "ManageNameIDRequestType";
            public const string ManageNameIDResponseType = "ManageNameIDResponseType";
            public const string NameIDMappingRequestType = "NameIDMappingRequestType";
            public const string NameIDMappingResponseType = "NameIDMappingResponseType";
            public const string NewEncryptedIDType = "NewEncryptedIDType";
            public const string EncryptedElementType = "EncryptedElementType";
            public const string AuthnContextType = "AuthnContextType";
            public const string AttributeType = "AttributeType";
            public const string StatementAbstractType = "StatementAbstractType";

            public const string ActionType = "ActionType";
            public const string AdviceType = "AdviceType";
            public const string AssertionType = "AssertionType";
            public const string AttributeStatementType = "AttributeStatementType";
            public const string AudienceRestrictionType = "AudienceRestrictionType";
            public const string AuthnStatementType = "AuthnStatementType";
            public const string AuthzDecisionStatementType = "AuthzDecisionStatementType";
            public const string BaseIDAbstractType = "BaseIDAbstractType";
            public const string ConditionAbstractType = "ConditionAbstractType";
            public const string ConditionsType = "ConditionsType";
            public const string EvidenceType = "EvidenceType";
            public const string KeyInfoConfirmationDataType = "KeyInfoConfirmationDataType";
            public const string NameIDType = "NameIDType";
            public const string OneTimeUseType = "OneTimeUseType";
            public const string ProxyRestrictionType = "ProxyRestrictionType";
            public const string SubjectConfirmationDataType = "SubjectConfirmationDataType";
            public const string SubjectConfirmationType = "SubjectConfirmationType";
            public const string SubjectLocalityType = "SubjectLocalityType";
            public const string SubjectType = "SubjectType";

            private XmlTypes() {
            }
        }
    }
}

#pragma warning restore 1591