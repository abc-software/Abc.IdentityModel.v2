// ----------------------------------------------------------------------------
// <copyright file="EidasLightProtocolSerializer.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols.EidasLight {
    using System;
    using System.Collections.ObjectModel;
    using System.Xml;
    using static Microsoft.IdentityModel.Logging.LogHelper;

    /// <summary>
    /// Reads and writes EidasLight request and reponse.
    /// </summary
    public class EidasLightProtocolSerializer {
        internal static Exception LogException(string format, params object[] args) {
            return LogExceptionMessage(new EidasSerializationException(FormatInvariant(format, args)));
        }

        internal static Exception LogException(string format, Exception inner, params object[] args) {
            return LogExceptionMessage(new EidasSerializationException(FormatInvariant(format, args), inner));
        }

        /// <summary>
        /// Determines whether a URI is valid and can be created using the specified UriKind.
        /// Uri.TryCreate is used here, which is more lax than Uri.IsWellFormedUriString.
        /// The reason we use this function is because IsWellFormedUriString will reject valid URIs if they are IPv6 or require escaping.
        /// </summary>
        /// <param name="uriString">The string to check.</param>
        /// <param name="uriKind">The type of URI (usually UriKind.Absolute)</param>
        /// <returns>True if the URI is valid, false otherwise.</returns>
        internal static bool CanCreateValidUri(string uriString, UriKind uriKind) {
            return Uri.TryCreate(uriString, uriKind, out Uri tempUri);
        }

        #region Read
        public EidasLightMessage ReadMessage(XmlReader reader) {
            if (reader is null) {
                throw new ArgumentNullException(nameof(reader));
            }

            if (reader.IsStartElement(EidasLightConstants.ElementNames.LightRequest, EidasLightConstants.Namespaces.Request)) {
                return ReadRequest(reader);
            }

            if (reader.IsStartElement(EidasLightConstants.ElementNames.LightResponse, EidasLightConstants.Namespaces.Response)) {
                return ReadResponse(reader);
            }

            throw LogException(LogMessages.ID8005, reader.Value, reader.NamespaceURI);
        }

        protected virtual EidasLightRequest ReadRequest(XmlReader reader) {
            if (reader is null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                var lightRequest = new EidasLightRequest();

                if (!reader.IsStartElement(EidasLightConstants.ElementNames.LightRequest, EidasLightConstants.Namespaces.Request)) {
                    reader.ReadStartElement(EidasLightConstants.ElementNames.LightRequest, EidasLightConstants.Namespaces.Request);
                }

                if (reader.IsEmptyElement) {
                    throw LogException(LogMessages.ID8003, reader.LocalName, reader.NamespaceURI);
                }

                reader.ReadStartElement();
                reader.MoveToContent();
                lightRequest.CitizenCountryCode = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.CitizenCountryCode, EidasLightConstants.Namespaces.Request);

                reader.MoveToContent();
                lightRequest.Id = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.Id, EidasLightConstants.Namespaces.Request);

                reader.MoveToContent();
                lightRequest.Issuer = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.Issuer, EidasLightConstants.Namespaces.Request);

                while (reader.IsStartElement(EidasLightConstants.ElementNames.LevelOfAssurance)) {
                    lightRequest.LevelsOfAssurance.Add(ReadLevelOfAssurance(reader, EidasLightConstants.Namespaces.Request));
                }

                if (lightRequest.LevelsOfAssurance.Count == 0) {
                    throw LogException(LogMessages.ID8009);
                }

                if (reader.IsStartElement(EidasLightConstants.ElementNames.NameIdFormat, EidasLightConstants.Namespaces.Request)) {
                    var val = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.NameIdFormat, EidasLightConstants.Namespaces.Request);
                    switch (val) {
                        case Saml2.Saml2Constants.NameIdentifierFormats.PersistentString:
                        case Saml2.Saml2Constants.NameIdentifierFormats.TransientString:
                        case Saml11.SamlConstants.NameIdentifierFormats.UnspecifiedString:
                        case Saml11.SamlConstants.NameIdentifierFormats.EmailAddressString:
                        case Saml11.SamlConstants.NameIdentifierFormats.X509SubjectNameString:
                        case Saml11.SamlConstants.NameIdentifierFormats.WindowsDomainQualifiedNameString:
                        case Saml2.Saml2Constants.NameIdentifierFormats.KerberosString:
                        case Saml2.Saml2Constants.NameIdentifierFormats.EntityString:
                        case Saml2.Saml2Constants.NameIdentifierFormats.EncryptedString:
                            lightRequest.NameIdFormat = new Uri(val);
                            break;
                        default:
                            throw LogException(LogMessages.ID8008, EidasLightConstants.ElementNames.NameIdFormat, val);
                    }
                }

                if (reader.IsStartElement(EidasLightConstants.ElementNames.ProviderName, EidasLightConstants.Namespaces.Request)) {
                    lightRequest.ProviderName = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.ProviderName, EidasLightConstants.Namespaces.Request);
                }

                if (reader.IsStartElement(EidasLightConstants.ElementNames.SPType, EidasLightConstants.Namespaces.Request)) {
                    var val = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.SPType, EidasLightConstants.Namespaces.Request);
                    switch (val) {
                        case "private":
                            lightRequest.SpType = EidasSpType.Private;
                            break;
                        case "public":
                            lightRequest.SpType = EidasSpType.Public;
                            break;
                        default:
                            throw LogException(LogMessages.ID8008, EidasLightConstants.ElementNames.SPType, val);
                    }
                }

                if (reader.IsStartElement(EidasLightConstants.ElementNames.SPCountryCode, EidasLightConstants.Namespaces.Request)) {
                    lightRequest.SpCountryCode = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.SPCountryCode, EidasLightConstants.Namespaces.Request);
                }

                if (reader.IsStartElement(EidasLightConstants.ElementNames.RequesterId, EidasLightConstants.Namespaces.Request)) {
                    var val = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.RequesterId, EidasLightConstants.Namespaces.Request);
                    /* REMOVED BY Customer
                    if (!CanCreateValidUri(val, UriKind.Absolute)) {
                        throw LogException(LogMessages.ID8007, EidasLightConstants.ElementNames.RequesterId, val);
                    }
                    */

                    lightRequest.RequesterId = val;
                }

                if (reader.IsStartElement(EidasLightConstants.ElementNames.RelayState, EidasLightConstants.Namespaces.Request)) {
                    lightRequest.RelayState = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.RelayState, EidasLightConstants.Namespaces.Request);
                }

                if (!reader.IsStartElement(EidasLightConstants.ElementNames.RequestedAttributes, EidasLightConstants.Namespaces.Request)) {
                    throw LogException(LogMessages.ID8004, EidasLightConstants.ElementNames.RequestedAttributes, EidasLightConstants.Namespaces.Request, reader.LocalName, reader.NamespaceURI);
                }

                reader.ReadStartElement(EidasLightConstants.ElementNames.RequestedAttributes, EidasLightConstants.Namespaces.Request);
                while (reader.IsStartElement(EidasLightConstants.ElementNames.Attribute, EidasLightConstants.Namespaces.Request)) {
                    lightRequest.RequestedAttributes.Add(ReadAttribute(reader, EidasLightConstants.Namespaces.Request));
                }

                reader.ReadEndElement();

                if (lightRequest.RequestedAttributes.Count == 0) {
                    throw LogException(LogMessages.ID8010);
                }

                return lightRequest;
            }
            catch (Exception exception) {
                if (exception is EidasSerializationException) {
                    throw;
                }

                throw LogException(LogMessages.ID8011, EidasLightConstants.ElementNames.LightRequest, exception);
            }
        }

        protected virtual EidasLightResponse ReadResponse(XmlReader reader) {
            if (reader is null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                var lightResponse = new EidasLightResponse();

                if (!reader.IsStartElement(EidasLightConstants.ElementNames.LightResponse, EidasLightConstants.Namespaces.Response)) {
                    reader.ReadStartElement(EidasLightConstants.ElementNames.LightResponse, EidasLightConstants.Namespaces.Response);
                }

                if (reader.IsEmptyElement) {
                    throw LogException(LogMessages.ID8003, reader.LocalName, reader.NamespaceURI);
                }

                reader.ReadStartElement();
                reader.MoveToContent();
                lightResponse.Id = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.Id, EidasLightConstants.Namespaces.Response);

                reader.MoveToContent();
                lightResponse.InResponseToId = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.InResponseToId, EidasLightConstants.Namespaces.Response);

                if (reader.IsStartElement(EidasLightConstants.ElementNames.Consent, EidasLightConstants.Namespaces.Response)) {
                    var val = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.Consent, EidasLightConstants.Namespaces.Response);
                    switch (val) {
                        case Saml2.Saml2Constants.ConsentIdentifiers.UnspecifiedString:
                        case Saml2.Saml2Constants.ConsentIdentifiers.ObtainedString:
                        case Saml2.Saml2Constants.ConsentIdentifiers.PriorString:
                        case Saml2.Saml2Constants.ConsentIdentifiers.ImplicitString:
                        case Saml2.Saml2Constants.ConsentIdentifiers.ExplicitString:
                        case Saml2.Saml2Constants.ConsentIdentifiers.UnavailableString:
                        case Saml2.Saml2Constants.ConsentIdentifiers.InapplicableString:
                            lightResponse.Consent = new Uri(val);
                            break;
                        default:
                            throw LogException(LogMessages.ID8008, EidasLightConstants.ElementNames.Consent, val);
                    }
                }

                reader.MoveToContent();
                lightResponse.Issuer = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.Issuer, EidasLightConstants.Namespaces.Response);

                if (reader.IsStartElement(EidasLightConstants.ElementNames.IpAddress, EidasLightConstants.Namespaces.Response)) {
                    lightResponse.IpAddress = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.IpAddress, EidasLightConstants.Namespaces.Response);
                }

                if (reader.IsStartElement(EidasLightConstants.ElementNames.RelayState, EidasLightConstants.Namespaces.Response)) {
                    lightResponse.RelayState = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.RelayState, EidasLightConstants.Namespaces.Response);
                }

                if (reader.IsStartElement(EidasLightConstants.ElementNames.Subject, EidasLightConstants.Namespaces.Response)) {
                    lightResponse.Subject = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.Subject, EidasLightConstants.Namespaces.Response);
                }

                if (reader.IsStartElement(EidasLightConstants.ElementNames.SubjectNameIdFormat, EidasLightConstants.Namespaces.Response)) {
                    var val = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.SubjectNameIdFormat, EidasLightConstants.Namespaces.Response);
                    switch (val) {
                        case Saml2.Saml2Constants.NameIdentifierFormats.PersistentString:
                        case Saml2.Saml2Constants.NameIdentifierFormats.TransientString:
                        case Saml11.SamlConstants.NameIdentifierFormats.UnspecifiedString:
                        case Saml11.SamlConstants.NameIdentifierFormats.EmailAddressString:
                        case Saml11.SamlConstants.NameIdentifierFormats.X509SubjectNameString:
                        case Saml11.SamlConstants.NameIdentifierFormats.WindowsDomainQualifiedNameString:
                        case Saml2.Saml2Constants.NameIdentifierFormats.KerberosString:
                        case Saml2.Saml2Constants.NameIdentifierFormats.EntityString:
                        case Saml2.Saml2Constants.NameIdentifierFormats.EncryptedString:
                            lightResponse.SubjectNameIdFormat = new Uri(val);
                            break;
                        default:
                            throw LogException(LogMessages.ID8008, EidasLightConstants.ElementNames.SubjectNameIdFormat, val);
                    }
                }

                if (reader.IsStartElement(EidasLightConstants.ElementNames.LevelOfAssurance, EidasLightConstants.Namespaces.Response)) {
                    var val = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.LevelOfAssurance, EidasLightConstants.Namespaces.Response);
                    switch (val) {
                        case EidasConstants.AuthenticationContextClasses.LevelOfAssuranceLowString:
                        case EidasConstants.AuthenticationContextClasses.LevelOfAssurancesubSubstantialString:
                        case EidasConstants.AuthenticationContextClasses.LevelOfAssuranceHighString:
                            lightResponse.LevelOfAssurance = new LevelOfAssurance(new Uri(val));
                            break;
                        default:
                            throw LogException(LogMessages.ID8008, EidasLightConstants.ElementNames.LevelOfAssurance, val);
                    }
                }

                if (!reader.IsStartElement(EidasLightConstants.ElementNames.Status, EidasLightConstants.Namespaces.Response)) {
                    throw LogException(LogMessages.ID8004, EidasLightConstants.ElementNames.Status, EidasLightConstants.Namespaces.Response, reader.LocalName, reader.NamespaceURI);
                }

                lightResponse.Status = ReadResponseStatus(reader);

                if (!reader.IsStartElement(EidasLightConstants.ElementNames.Attributes, EidasLightConstants.Namespaces.Response)) {
                    throw LogException(LogMessages.ID8004, EidasLightConstants.ElementNames.Attributes, EidasLightConstants.Namespaces.Response, reader.LocalName, reader.NamespaceURI);
                }

                if (!reader.IsEmptyElement) {
                    reader.ReadStartElement(EidasLightConstants.ElementNames.Attributes, EidasLightConstants.Namespaces.Response);
                    while (reader.IsStartElement(EidasLightConstants.ElementNames.Attribute, EidasLightConstants.Namespaces.Response)) {
                        lightResponse.Attributes.Add(ReadAttribute(reader, EidasLightConstants.Namespaces.Response));
                    }

                    reader.ReadEndElement();
                }

                return lightResponse;
            }
            catch (Exception exception) {
                if (exception is EidasSerializationException) {
                    throw;
                }

                throw LogException(LogMessages.ID8011, EidasLightConstants.ElementNames.LightResponse, exception);
            }
        }

        private static AttributeDefinition ReadAttribute(XmlReader reader, string ns) {
            reader.ReadStartElement(EidasLightConstants.ElementNames.Attribute, ns);
            reader.MoveToContent();
            var definition = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.Definition, ns);
            var values = new Collection<string>();

            while (reader.IsStartElement(EidasLightConstants.ElementNames.Value, ns)) {
                values.Add(reader.ReadElementContentAsString(EidasLightConstants.ElementNames.Value, ns));
            }

            reader.ReadEndElement();

            return new AttributeDefinition(definition, values);
        }

        private static LevelOfAssurance ReadLevelOfAssurance(XmlReader reader, string ns) {
            var type = reader.GetAttribute(EidasLightConstants.AttributeNames.Type);
            var val = LevelOfAssuranceType.Notified;
            switch (type) {
                case "notified":
                    val = LevelOfAssuranceType.Notified;
                    break;
                case "nonNotified":
                    val = LevelOfAssuranceType.NonNotified;
                    break;
                case null:
                    break;
                default:
                    throw LogException(LogMessages.ID8008, EidasLightConstants.AttributeNames.Type, val);
            }

            if (!Uri.TryCreate(reader.ReadElementContentAsString(EidasLightConstants.ElementNames.LevelOfAssurance, ns), UriKind.Absolute, out var uri)) {
                throw LogException(LogMessages.ID8007, EidasLightConstants.ElementNames.LevelOfAssurance, val);
            }

            return new LevelOfAssurance(uri, val);
        }

        private static EidasLightResponseStatus ReadResponseStatus(XmlReader reader) {
            if (!reader.IsStartElement(EidasLightConstants.ElementNames.Status, EidasLightConstants.Namespaces.Response)) {

            }

            var isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement(EidasLightConstants.ElementNames.Status, EidasLightConstants.Namespaces.Response);

            var status = new EidasLightResponseStatus();
            if (!isEmptyElement) {
                if (reader.IsStartElement(EidasLightConstants.ElementNames.Failure, EidasLightConstants.Namespaces.Response)) {
                    status.Failure = XmlConvert.ToBoolean(reader.ReadElementContentAsString(EidasLightConstants.ElementNames.Failure, EidasLightConstants.Namespaces.Response));
                }

                Uri code = null;
                Uri subCode = null;
                if (reader.IsStartElement(EidasLightConstants.ElementNames.StatusCode, EidasLightConstants.Namespaces.Response)) {
                    var val = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.StatusCode, EidasLightConstants.Namespaces.Response);
                    switch (val) {
                        case Saml2.Saml2Constants.StatusCodes.Namespace + ":" + Saml2.Saml2Constants.StatusCodes.SuccessString:
                        case Saml2.Saml2Constants.StatusCodes.Namespace + ":" + Saml2.Saml2Constants.StatusCodes.RequesterString:
                        case Saml2.Saml2Constants.StatusCodes.Namespace + ":" + Saml2.Saml2Constants.StatusCodes.ResponderString:
                        case Saml2.Saml2Constants.StatusCodes.Namespace + ":" + Saml2.Saml2Constants.StatusCodes.VersionMismatchString:
                            code = new Uri(val);
                            break;
                        default:
                            throw LogException(LogMessages.ID8008, EidasLightConstants.ElementNames.StatusCode, val);
                    }
                }

                if (reader.IsStartElement(EidasLightConstants.ElementNames.SubStatusCode, EidasLightConstants.Namespaces.Response)) {
                    var val = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.SubStatusCode, EidasLightConstants.Namespaces.Response);
                    switch (val) {
                        case Saml2.Saml2Constants.StatusCodes.Namespace + ":" + Saml2.Saml2Constants.StatusCodes.AuthnFailedString:
                        case Saml2.Saml2Constants.StatusCodes.Namespace + ":" + Saml2.Saml2Constants.StatusCodes.InvalidAttrNameOrValueString:
                        case Saml2.Saml2Constants.StatusCodes.Namespace + ":" + Saml2.Saml2Constants.StatusCodes.InvalidNameIDPolicyString:
                        case Saml2.Saml2Constants.StatusCodes.Namespace + ":" + Saml2.Saml2Constants.StatusCodes.RequestDeniedString:
                            subCode = new Uri(val);
                            break;
                        default:
                            throw LogException(LogMessages.ID8008, EidasLightConstants.ElementNames.SubStatusCode, val);
                    }
                }

                if (code != null) {
                    status.StatusCode = Tuple.Create(code, subCode);
                }

                if (reader.IsStartElement(EidasLightConstants.ElementNames.StatusMessage, EidasLightConstants.Namespaces.Response)) {
                    status.StatusMessage = reader.ReadElementContentAsString(EidasLightConstants.ElementNames.StatusMessage, EidasLightConstants.Namespaces.Response);
                }

                reader.ReadEndElement();
            }

            return status;
        }

        private static Exception TryWrapReadException(XmlReader reader, Exception innerException) {
            if (innerException is FormatException
                || innerException is ArgumentException
                || innerException is InvalidOperationException
                || innerException is OverflowException) {
                return LogException(LogMessages.ID8001, innerException);
            }

            return null;
        }
        #endregion

        #region Write
        public void WriteMessage(XmlWriter writer, EidasLightMessage message) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message is null) {
                throw new ArgumentNullException(nameof(message));
            }

            if (message is EidasLightRequest request) {
                WriteRequest(writer, request);
            } else if (message is EidasLightResponse response) {
                WriteResponse(writer, response);
            }
            else {
                throw LogException(LogMessages.ID8002, message.GetType().FullName);
            }
        }

        protected virtual void WriteRequest(XmlWriter writer, EidasLightRequest request) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.LevelsOfAssurance.Count == 0) {
                throw LogException(LogMessages.ID8109);
            }

            if (request.RequestedAttributes.Count == 0) {
                throw LogException(LogMessages.ID8110);
            }

            writer.WriteStartElement(EidasLightConstants.Prefixes.Request, EidasLightConstants.ElementNames.LightRequest, EidasLightConstants.Namespaces.Request);
            writer.WriteElementString(EidasLightConstants.Prefixes.Request, EidasLightConstants.ElementNames.CitizenCountryCode, EidasLightConstants.Namespaces.Request, request.CitizenCountryCode);
            writer.WriteElementString(EidasLightConstants.Prefixes.Request, EidasLightConstants.ElementNames.Id, EidasLightConstants.Namespaces.Request, request.Id);
            writer.WriteElementString(EidasLightConstants.Prefixes.Request, EidasLightConstants.ElementNames.Issuer, EidasLightConstants.Namespaces.Request, request.Issuer);

            foreach (var levelOfAssurance in request.LevelsOfAssurance) {
                WriteLevelOfAssurance(writer, levelOfAssurance);
            }

            if (request.NameIdFormat != null) {
                writer.WriteElementString(EidasLightConstants.Prefixes.Request, EidasLightConstants.ElementNames.NameIdFormat, EidasLightConstants.Namespaces.Request, request.NameIdFormat.OriginalString);
            }

            if (request.ProviderName != null) {
                writer.WriteElementString(EidasLightConstants.Prefixes.Request, EidasLightConstants.ElementNames.ProviderName, EidasLightConstants.Namespaces.Request, request.ProviderName);
            }

            // spType
            if (request.SpType.HasValue) {
                string val;
                switch (request.SpType.Value) {
                    case EidasSpType.Private:
                        val = "private";
                        break;
                    case EidasSpType.Public:
                        val = "public";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(request), $"The EidasSaml2Extension cannot be serialized because the SpType property is set to a value that is not valid: {request.SpType}");
                }

                writer.WriteElementString(EidasLightConstants.Prefixes.Request, EidasLightConstants.ElementNames.SPType, EidasLightConstants.Namespaces.Request, val);
            }

            if (!string.IsNullOrEmpty(request.SpCountryCode)) {
                writer.WriteElementString(EidasLightConstants.Prefixes.Request, EidasLightConstants.ElementNames.SPCountryCode, EidasLightConstants.Namespaces.Request, request.SpCountryCode);
            }

            if (!string.IsNullOrEmpty(request.RequesterId)) {
                writer.WriteElementString(EidasLightConstants.Prefixes.Request, EidasLightConstants.ElementNames.RequesterId, EidasLightConstants.Namespaces.Request, request.RequesterId);
            }

            if (!string.IsNullOrEmpty(request.RelayState)) {
                writer.WriteElementString(EidasLightConstants.Prefixes.Request, EidasLightConstants.ElementNames.RelayState, EidasLightConstants.Namespaces.Request, request.RelayState);
            }

            writer.WriteStartElement(EidasLightConstants.Prefixes.Request, EidasLightConstants.ElementNames.RequestedAttributes, EidasLightConstants.Namespaces.Request);
            foreach (var attribute in request.RequestedAttributes) {
                WriteAttribute(writer, attribute, EidasLightConstants.Namespaces.Request);
            }

            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        protected virtual void WriteResponse(XmlWriter writer, EidasLightResponse response) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (response is null) {
                throw new ArgumentNullException(nameof(response));
            }

            writer.WriteStartElement(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.LightResponse, EidasLightConstants.Namespaces.Response);
            writer.WriteElementString(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.Id, EidasLightConstants.Namespaces.Response, response.Id);
            writer.WriteElementString(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.InResponseToId, EidasLightConstants.Namespaces.Response, response.InResponseToId);

            if (response.Consent != null) {
                writer.WriteElementString(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.Consent, EidasLightConstants.Namespaces.Response, response.Consent.OriginalString);
            }

            writer.WriteElementString(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.Issuer, EidasLightConstants.Namespaces.Response, response.Issuer);

            if (response.IpAddress != null) {
                writer.WriteElementString(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.IpAddress, EidasLightConstants.Namespaces.Response, response.IpAddress);
            }

            if (response.RelayState != null) {
                writer.WriteElementString(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.RelayState, EidasLightConstants.Namespaces.Response, response.RelayState);
            }

            if (response.Subject != null) {
                writer.WriteElementString(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.Subject, EidasLightConstants.Namespaces.Response, response.Subject);
            }

            if (response.SubjectNameIdFormat != null) {
                writer.WriteElementString(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.SubjectNameIdFormat, EidasLightConstants.Namespaces.Response, response.SubjectNameIdFormat.OriginalString);
            }

            if (response.LevelOfAssurance != null) {
                writer.WriteElementString(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.LevelOfAssurance, EidasLightConstants.Namespaces.Response, response.LevelOfAssurance.Value.OriginalString);
            }

            WriteResponseStatus(writer, response.Status);

            writer.WriteStartElement(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.Attributes, EidasLightConstants.Namespaces.Response);
            foreach (var attribute in response.Attributes) {
                WriteAttribute(writer, attribute, EidasLightConstants.Namespaces.Response);
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        private static void WriteResponseStatus(XmlWriter writer, EidasLightResponseStatus status) {
            writer.WriteStartElement(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.Status, EidasLightConstants.Namespaces.Response);

            if (status != null) {
                if (status.Failure != null) {
                    writer.WriteElementString(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.Failure, EidasLightConstants.Namespaces.Response, XmlConvert.ToString(status.Failure.Value));
                }

                if (status.StatusCode != null) {
                    writer.WriteElementString(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.StatusCode, EidasLightConstants.Namespaces.Response, status.StatusCode.Item1.OriginalString);
                    if (status.StatusCode.Item2 != null) {
                        writer.WriteElementString(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.SubStatusCode, EidasLightConstants.Namespaces.Response, status.StatusCode.Item2.OriginalString);
                    }
                }

                if (!string.IsNullOrEmpty(status.StatusMessage)) {
                    writer.WriteElementString(EidasLightConstants.Prefixes.Response, EidasLightConstants.ElementNames.StatusMessage, EidasLightConstants.Namespaces.Response, status.StatusMessage);
                }
            }

            writer.WriteEndElement();
        }

        private static void WriteLevelOfAssurance(XmlWriter writer, LevelOfAssurance levelOfAssurance) {
            if (levelOfAssurance is null) {
                throw new ArgumentNullException(nameof(levelOfAssurance));
            }

            writer.WriteStartElement(EidasLightConstants.Prefixes.Request, EidasLightConstants.ElementNames.LevelOfAssurance, EidasLightConstants.Namespaces.Request);

            if (levelOfAssurance.Type.HasValue) {
                string val;
                switch (levelOfAssurance.Type.Value) {
                    case LevelOfAssuranceType.Notified:
                        val = "notified";
                        break;
                    case LevelOfAssuranceType.NonNotified:
                        val = "nonNotified";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(levelOfAssurance), $"The EidasLightResponse cannot be serialized because the SpType property is set to a value that is not valid: {levelOfAssurance.Type}");
                }

                writer.WriteAttributeString(EidasLightConstants.AttributeNames.Type, val);
            }

            writer.WriteString(levelOfAssurance.Value.OriginalString);
            writer.WriteEndElement();
        }

        private static void WriteAttribute(XmlWriter writer, AttributeDefinition attribute, string ns) {
            writer.WriteStartElement(EidasLightConstants.ElementNames.Attribute, ns);
            writer.WriteElementString(EidasLightConstants.ElementNames.Definition, ns, attribute.Definition);
            foreach (var value in attribute.Values) {
                writer.WriteElementString(EidasLightConstants.ElementNames.Value, ns, value);
            }

            writer.WriteEndElement();
        }
        #endregion
    }
}