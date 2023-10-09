// ----------------------------------------------------------------------------
// <copyright file="Saml2ProtocolSerializer.cs" company="ABC Software Ltd">
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

namespace Abc.IdentityModel.Protocols.Saml2 {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Xml;
    using Abc.IdentityModel.Tokens;
    using Abc.IdentityModel.Protocols.Metadata;
#if WIF35
    using System.IdentityModel.Policy;
    using System.IdentityModel.Selectors;
    using System.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Protocols.XmlSignature;
    using Microsoft.IdentityModel.SecurityTokenService;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Tokens.Saml2;
#elif AZUREAD
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Tokens.Saml2;
    using Microsoft.IdentityModel.Xml;
    using SecurityKeyIdentifierClause = Microsoft.IdentityModel.Tokens.SecurityKeyIdentifierClause;
    using XmlUtil = Abc.IdentityModel.Protocols.XmlUtil;
#else
    using System.IdentityModel.Policy;
    using System.IdentityModel.Selectors;
    using System.IdentityModel.Tokens;
    using System.IdentityModel;
#endif
#if NET40 || NET35
    using Diagnostic;
#else
    using Abc.Diagnostics;
#endif
    using XmlSignatureConstants = Abc.IdentityModel.Protocols.XmlSignature.XmlSignatureConstants;

    /// <summary>
    /// The <c>Saml2ProtocolSerializer</c> serializes and deserializes XML elements representing SAML messages to and from their representation in the
    /// object model.
    /// </summary>
    internal class Saml2ProtocolSerializer {
        private static string[] supportedSamlMessageElements = new string[] {
            Saml2Constants.ElementNames.ArtifactResolve,
            Saml2Constants.ElementNames.ArtifactResponse,
            Saml2Constants.ElementNames.AuthnRequest,
            Saml2Constants.ElementNames.Response,
            Saml2Constants.ElementNames.LogoutRequest,
            Saml2Constants.ElementNames.LogoutResponse,
            Saml2Constants.ElementNames.AttributeQuery,
            Saml2Constants.ElementNames.AuthnQuery,
            Saml2Constants.ElementNames.AuthzDecisionQuery,
            Saml2Constants.ElementNames.ManageNameIDRequest,
            Saml2Constants.ElementNames.ManageNameIDResponse,
            Saml2Constants.ElementNames.NameIDMappingRequest,
            Saml2Constants.ElementNames.NameIDMappingResponse,
            Saml2Constants.ElementNames.AssertionIDRequest,
        };

        private readonly ISaml2TokenToSerializerAdaptor serializerAdaptor;
        private readonly SecurityTokenHandlerCollection securityTokenHandlerCollection;
#if AZUREAD
        private readonly TokenValidationParameters validationParameters;
#else
        private readonly SecurityTokenSerializer keyInfoSerializer;
        private readonly SecurityTokenResolver encryptionTokenResolver;
        private readonly SecurityTokenResolver signatureTokenResolver;
#endif
        private readonly bool handleSignature;

#if AZUREAD
        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ProtocolSerializer"/> class.
        /// </summary>
        /// <remarks>The serializer will not be able to decrypt incoming messages.</remarks>
        public Saml2ProtocolSerializer()
            : this(null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ProtocolSerializer"/> class.
        /// </summary>
        /// <remarks>The serializer will not be able to decrypt incoming messages.</remarks>
        /// <param name="signatureTokenResolver">The resolver to use when resolving security tokens used in XML signatures. Use null to indicate signatures should not be validated.</param>
        public Saml2ProtocolSerializer(TokenValidationParameters validationParameters)
            : this(validationParameters, new Saml2TokenToSerializerAdaptor(), null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ProtocolSerializer" /> class.
        /// </summary>
        /// <param name="serializerAdaptor">The serializer adaptor.</param>
        /// <param name="securityTokenHandlerCollection">The security token handler collection.</param>
        public Saml2ProtocolSerializer(TokenValidationParameters validationParameters, ISaml2TokenToSerializerAdaptor serializerAdaptor, SecurityTokenHandlerCollection securityTokenHandlerCollection) {
            this.validationParameters = validationParameters;
            this.serializerAdaptor = serializerAdaptor ?? throw new ArgumentNullException(nameof(serializerAdaptor));
            this.handleSignature = validationParameters != null;
            this.securityTokenHandlerCollection = securityTokenHandlerCollection;
        }
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ProtocolSerializer"/> class.
        /// </summary>
        public Saml2ProtocolSerializer()
            : this(null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ProtocolSerializer"/> class.
        /// </summary>
        /// <remarks>The serializer will not be able to decrypt incoming messages.</remarks>
        /// <param name="signatureTokenResolver">The resolver to use when resolving security tokens used in XML signatures. Use null to indicate signatures should not be validated.</param>
        public Saml2ProtocolSerializer(SecurityTokenResolver signatureTokenResolver)
            : this(signatureTokenResolver, null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ProtocolSerializer"/> class.
        /// </summary>
        /// <remarks>The serializer will not be able to decrypt incoming messages.</remarks>
        /// <param name="signatureTokenResolver">The resolver to use when resolving security tokens used in XML signatures. Use null to indicate signatures should not be validated.</param>
        /// <param name="encryptionTokenResolver">The resolver to use when resolving security tokens used in XML encryption.</param>
        public Saml2ProtocolSerializer(SecurityTokenResolver signatureTokenResolver, SecurityTokenResolver encryptionTokenResolver)
            : this(signatureTokenResolver, encryptionTokenResolver, SamlUtil.CreateDefaultX509KeyInfoSerializer(encryptionTokenResolver)) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ProtocolSerializer"/> class.
        /// </summary>
        /// <remarks>The serializer will not be able to decrypt incoming messages.</remarks>
        /// <param name="signatureTokenResolver">The resolver to use when resolving security tokens used in XML signatures. Use null to indicate signatures should not be validated.</param>
        /// <param name="encryptionTokenResolver">The resolver to use when resolving security tokens used in XML encryption.</param>
        /// <param name="keyInfoSerializer">The serializer to use when serializing ds:KeyInfo elements.</param>
        public Saml2ProtocolSerializer(SecurityTokenResolver signatureTokenResolver, SecurityTokenResolver encryptionTokenResolver, SecurityTokenSerializer keyInfoSerializer)
            : this(signatureTokenResolver, encryptionTokenResolver, keyInfoSerializer, SamlUtil.CreateDefaultX509SecurityTokenHandlerCollection(encryptionTokenResolver)) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ProtocolSerializer"/> class.
        /// </summary>
        /// <param name="signatureTokenResolver">The resolver to use when resolving security tokens used in XML signatures. Use null to indicate signatures should not be validated.</param>
        /// <param name="encryptionTokenResolver">The resolver to use when resolving security tokens used in XML encryption.</param>
        /// <param name="keyInfoSerializer">The serializer to use when serializing ds:KeyInfo elements.</param>
        /// <param name="securityTokenHandlerCollection">The security token handler collection.</param>
        public Saml2ProtocolSerializer(SecurityTokenResolver signatureTokenResolver, SecurityTokenResolver encryptionTokenResolver, SecurityTokenSerializer keyInfoSerializer, SecurityTokenHandlerCollection securityTokenHandlerCollection)
            : this(signatureTokenResolver, encryptionTokenResolver, keyInfoSerializer, new AbcSaml2SecurityTokenHandler(new SamlSecurityTokenRequirement()).IntitializeAdaptor(signatureTokenResolver, encryptionTokenResolver, keyInfoSerializer), securityTokenHandlerCollection) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ProtocolSerializer" /> class.
        /// </summary>
        /// <param name="signatureTokenResolver">The resolver to use when resolving security tokens used in XML signatures. Use null to indicate signatures should not be validated.</param>
        /// <param name="encryptionTokenResolver">The resolver to use when resolving security tokens used in XML encryption.</param>
        /// <param name="keyInfoSerializer">The serializer to use when serializing ds:KeyInfo elements.</param>
        /// <param name="serializerAdaptor">The serializer adaptor.</param>
        /// <param name="securityTokenHandlerCollection">The security token handler collection.</param>
        public Saml2ProtocolSerializer(SecurityTokenResolver signatureTokenResolver, SecurityTokenResolver encryptionTokenResolver, SecurityTokenSerializer keyInfoSerializer, ISaml2TokenToSerializerAdaptor serializerAdaptor, SecurityTokenHandlerCollection securityTokenHandlerCollection) {
            this.signatureTokenResolver = signatureTokenResolver;
            this.encryptionTokenResolver = encryptionTokenResolver;
            this.keyInfoSerializer = keyInfoSerializer ?? throw new ArgumentNullException(nameof(keyInfoSerializer));
            this.serializerAdaptor = serializerAdaptor ?? throw new ArgumentNullException(nameof(serializerAdaptor));
            this.handleSignature = signatureTokenResolver != null;
            this.securityTokenHandlerCollection = securityTokenHandlerCollection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ProtocolSerializer"/> class.
        /// </summary>
        /// <param name="tokenResolver">The resolver to use when resolving security tokens used in XML signatures and to use when resolving security tokens used in XML encryption.</param>
        /// <param name="keyInfoSerializer">The serializer to use when serializing ds:KeyInfo elements.</param>
        /// <param name="securityTokenHandlerCollection">The security token handler collection.</param>
        /// <remarks> For backward compatibility.</remarks>
        internal Saml2ProtocolSerializer(SecurityTokenResolver tokenResolver, SecurityTokenSerializer keyInfoSerializer, SecurityTokenHandlerCollection securityTokenHandlerCollection)
            : this(tokenResolver, tokenResolver, keyInfoSerializer, new AbcSaml2SecurityTokenHandler(new SamlSecurityTokenRequirement()).IntitializeAdaptor(tokenResolver, tokenResolver, keyInfoSerializer), securityTokenHandlerCollection) {
        }

        /// <summary>
        /// Gets the <c>SecurityTokenResolver</c> to be used when resolving signatures on incoming SAML messages.
        /// </summary>
        /// <value>The <c>SecurityTokenResolver</c> to be used when resolving signatures on incoming SAML messages.</value>
        public SecurityTokenResolver SignatureTokenResolver {
            get { return this.signatureTokenResolver; }
        }

        /// <summary>
        /// Gets the <c>SecurityTokenResolver</c> to be used when resolving encrypted keys on incoming SAML messages.
        /// </summary>
        /// <value>The <c>SecurityTokenResolver</c> to be used when resolving encrypted keys on incoming SAML messages.</value>
        public SecurityTokenResolver EncryptionTokenResolver {
            get { return this.encryptionTokenResolver; }
        }

        /// <summary>
        /// Gets the <c>SecurityTokenSerializer</c> to use when serializing KeyInfo clauses.
        /// </summary>
        /// <remarks>
        /// Defaults to a serializer that can write X509RawData certificates and can read encrypted keys.
        /// </remarks>
        /// <value>The <c>SecurityTokenSerializer</c> to use when serializing KeyInfo clauses.</value>
        public SecurityTokenSerializer KeyInfoSerializer {
            get { return this.keyInfoSerializer; }
        }
#endif
        /// <summary>
        /// Gets a value indicating whether or not this serializer will validate signatures on the XML documents it deserializes.
        /// </summary>
        /// <value>Whether or not this serializer will validate signatures on the XML documents it deserializes.</value>
        public bool HandleSignature {
            get { return this.handleSignature; }
        }

        internal SecurityTokenHandlerCollection SecurityTokenHandlers {
            get { return this.securityTokenHandlerCollection; }
        }

        /// <summary>
        /// Determines whether this instance can read SAML2 message.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>
        /// <c>true</c> if this instance can read SAML2 message; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanReadSamlMessage(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            foreach (string str in supportedSamlMessageElements) {
                if (reader.IsStartElement(str, Saml2Constants.Namespaces.Protocol)) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Deserializes a SAML protocol message.
        /// </summary>
        /// <param name="reader">An instance of an <c>XmlReader</c> positioned on the opening tag of the SAML protocol message.</param>
        /// <returns>The deserialized protocol message.</returns>
        public virtual Saml2Message ReadSamlMessage(XmlReader reader/*, NamespaceContext context*/) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            //// TODO: XmlNamespaceReader reader2 = new XmlNamespaceReader(reader);
            //// return this.ReadSamlMessage(reader2, reader2.NamespaceContext);

            if (reader.IsStartElement(Saml2Constants.ElementNames.ArtifactResolve, Saml2Constants.Namespaces.Protocol)) {
                return this.ReadArtifactResolve(reader);
            }

            if (reader.IsStartElement(Saml2Constants.ElementNames.ArtifactResponse, Saml2Constants.Namespaces.Protocol)) {
                return this.ReadArtifactResponse(reader/*, context*/);
            }

            if (reader.IsStartElement(Saml2Constants.ElementNames.AuthnRequest, Saml2Constants.Namespaces.Protocol)) {
                return this.ReadAuthnRequest(reader);
            }

            if (reader.IsStartElement(Saml2Constants.ElementNames.LogoutRequest, Saml2Constants.Namespaces.Protocol)) {
                return this.ReadLogoutRequest(reader);
            }

            if (reader.IsStartElement(Saml2Constants.ElementNames.LogoutResponse, Saml2Constants.Namespaces.Protocol)) {
                return this.ReadLogoutResponse(reader);
            }

            if (reader.IsStartElement(Saml2Constants.ElementNames.Response, Saml2Constants.Namespaces.Protocol)) {
                return this.ReadResponse(reader/*, context*/);
            }

            if (reader.IsStartElement(Saml2Constants.ElementNames.AttributeQuery, Saml2Constants.Namespaces.Protocol)) {
                return this.ReadAttributeQuery(reader);
            }

            if (reader.IsStartElement(Saml2Constants.ElementNames.AuthnQuery, Saml2Constants.Namespaces.Protocol)) {
                return this.ReadAuthenticationQuery(reader);
            }

            if (reader.IsStartElement(Saml2Constants.ElementNames.AuthzDecisionQuery, Saml2Constants.Namespaces.Protocol)) {
                return this.ReadAuthorizationDecisionQuery(reader);
            }

            if (reader.IsStartElement(Saml2Constants.ElementNames.AssertionIDRequest, Saml2Constants.Namespaces.Protocol)) {
                return this.ReadAssertionIDRequest(reader);
            }

            if (reader.IsStartElement(Saml2Constants.ElementNames.ManageNameIDRequest, Saml2Constants.Namespaces.Protocol)) {
                return this.ReadManageNameIDRequest(reader);
            }

            if (reader.IsStartElement(Saml2Constants.ElementNames.ManageNameIDResponse, Saml2Constants.Namespaces.Protocol)) {
                return this.ReadManageNameIDResponse(reader);
            }

            if (reader.IsStartElement(Saml2Constants.ElementNames.NameIDMappingRequest, Saml2Constants.Namespaces.Protocol)) {
                return this.ReadNameIDMappingRequest(reader);
            }

            if (reader.IsStartElement(Saml2Constants.ElementNames.NameIDMappingResponse, Saml2Constants.Namespaces.Protocol)) {
                return this.ReadNameIDMappingResponse(reader);
            }

            throw DiagnosticTools.ExceptionUtil.ThrowHelperError(new Saml2SerializationException(SR.ID2010Format(reader.Value, reader.NamespaceURI)));
        }

        /// <summary>
        /// Serializes a SAML protocol message.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize the message to.</param>
        /// <param name="message">The message to be serialized.</param>
        public virtual void WriteSamlMessage(XmlWriter writer, Saml2Message message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            Type type = message.GetType();
            if (type == typeof(Saml2ArtifactResolve)) {
                this.WriteArtifactResolve(writer, message as Saml2ArtifactResolve);
            }
            else if (type == typeof(Saml2ArtifactResponse)) {
                this.WriteArtifactResponse(writer, message as Saml2ArtifactResponse);
            }
            else if (type == typeof(Saml2AuthenticationRequest)) {
                this.WriteAuthnRequest(writer, message as Saml2AuthenticationRequest);
            }
            else if (type == typeof(Saml2LogoutRequest)) {
                this.WriteLogoutRequest(writer, message as Saml2LogoutRequest);
            }
            else if (type == typeof(Saml2LogoutResponse)) {
                this.WriteLogoutResponse(writer, message as Saml2LogoutResponse);
            }
            else if (type == typeof(Saml2Response)) {
                this.WriteResponse(writer, message as Saml2Response);
            }
            else if (type == typeof(Saml2AttributeQuery)) {
                this.WriteAttributeQuery(writer, message as Saml2AttributeQuery);
            }
            else if (type == typeof(Saml2AuthenticationQuery)) {
                this.WriteAuthenticationQuery(writer, message as Saml2AuthenticationQuery);
            }
            else if (type == typeof(Saml2AuthorizationDecisionQuery)) {
                this.WriteAuthorizationDecisionQuery(writer, message as Saml2AuthorizationDecisionQuery);
            }
            else if (type == typeof(Saml2AssertionIdRequest)) {
                this.WriteAssertionIDRequest(writer, message as Saml2AssertionIdRequest);
            }
            else if (type == typeof(Saml2ManageNameIdRequest)) {
                this.WriteManageNameIDRequest(writer, message as Saml2ManageNameIdRequest);
            }
            else if (type == typeof(Saml2ManageNameIdResponse)) {
                this.WriteManageNameIDResponse(writer, message as Saml2ManageNameIdResponse);
            }
            else if (type == typeof(Saml2NameIdMappingRequest)) {
                this.WriteNameIDMappingRequest(writer, message as Saml2NameIdMappingRequest);
            }
            else if (type == typeof(Saml2NameIdMappingResponse)) {
                this.WriteNameIDMappingResponse(writer, message as Saml2NameIdMappingResponse);
            }
            else {
                throw DiagnosticTools.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.ID2002Format(type.FullName)));
            }
        }

#region Read

        /// <summary>
        /// Deserializes a samlp:ArtifactResolve element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on a samlp:ArtifactResolve opening tag.</param>
        /// <returns>Returns a <c>Saml2ArtifactResolve</c> object.</returns>
        protected virtual Saml2ArtifactResolve ReadArtifactResolve(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                Saml2ArtifactResolve message = new Saml2ArtifactResolve("_DUMMY_");
                if (this.handleSignature) {
                    reader = this.CreateEnvelopedSignatureReader(reader, message);
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.ArtifactResolve, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.ArtifactResolve, Saml2Constants.Namespaces.Protocol);
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.ArtifactResolveType, Saml2Constants.Namespaces.Protocol);

                this.ReadCommonAttributes(reader, message);

                reader.ReadStartElement();
                this.ReadCommonElements(reader, message);

                message.Artifact = this.ReadAssertionArtifact(reader);
                reader.ReadEndElement();

                if (this.handleSignature) {
                    this.SetSigningCredentials(reader, message);
                }

                return message;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:ArtifactResponse message.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a samlp:ArtifactResponse element.</param>
        /// <returns>An <c>Saml2ArtifactResponse</c> object.</returns>
        protected virtual Saml2ArtifactResponse ReadArtifactResponse(XmlReader reader/*, NamespaceContext context*/) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                Saml2ArtifactResponse message = new Saml2ArtifactResponse(new Saml2Status(Saml2StatusCode.Responder));
                if (this.handleSignature) {
                    reader = this.CreateEnvelopedSignatureReader(reader, message);
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.ArtifactResponse, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.ArtifactResponse, Saml2Constants.Namespaces.Protocol);
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.ArtifactResponseType, Saml2Constants.Namespaces.Protocol);

                // Saml2Message Attributes
                this.ReadCommonAttributes(reader, message);

                // InResponseTo
                string attribute = reader.GetAttribute(Saml2Constants.AttributeNames.InResponseTo);
                if (!string.IsNullOrEmpty(attribute)) {
                    message.InResponseTo = new Saml2Id(attribute);
                }

                reader.ReadStartElement();

                // Saml2Message elements
                this.ReadCommonElements(reader, message);

                // Read SamlStatus
                message.Status = this.ReadStatus(reader);

                if (this.CanReadSamlMessage(reader)) {
                    XmlReader nodeReader = new XmlNodeReader(XmlUtil.ReadInnerXml(reader, null/*, context.GetNamespaces()*/));
                    reader.MoveToContent();
                    message.Response = new Saml2ArtifactResponseContent(this.ReadSamlMessage(nodeReader));
                }
                else {
                    if (reader.NodeType != XmlNodeType.EndElement) {
                        message.Response = new Saml2ArtifactResponseContent(XmlUtil.ReadInnerXml(reader, null/*context.GetNamespaces()*/));
                    }
                }

                reader.ReadEndElement();

                if (this.handleSignature) {
                    this.SetSigningCredentials(reader, message);
                }

                return message;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:AuthnRequest message.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a samlp:AuthnRequest element.</param>
        /// <returns>An <c>Saml2AuthenticationRequest</c> object.</returns>
        protected virtual Saml2AuthenticationRequest ReadAuthnRequest(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                Saml2AuthenticationRequest message = new Saml2AuthenticationRequest();
                if (this.handleSignature) {
                    reader = this.CreateEnvelopedSignatureReader(reader, message);
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.AuthnRequest, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.AuthnRequest, Saml2Constants.Namespaces.Protocol);
                }

                bool isEmptyElement = reader.IsEmptyElement;

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.AuthnRequestType, Saml2Constants.Namespaces.Protocol);

                this.ReadCommonAttributes(reader, message);

                string attribute = reader.GetAttribute(Saml2Constants.AttributeNames.ForceAuthn);
                if (!string.IsNullOrEmpty(attribute)) {
                    message.ForceAuthentication = XmlConvert.ToBoolean(attribute);
                }

                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.IsPassive);
                if (!string.IsNullOrEmpty(attribute)) {
                    message.IsPassive = XmlConvert.ToBoolean(attribute);
                }

                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.ProtocolBinding);
                if (!string.IsNullOrEmpty(attribute)) {
                    message.ProtocolBinding = new Uri(attribute);
                }

                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.AssertionConsumerServiceIndex);
                if (!string.IsNullOrEmpty(attribute)) {
                    message.AssertionConsumerServiceIndex = new ushort?(XmlConvert.ToUInt16(attribute));
                }

                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.AssertionConsumerServiceURL);
                if (!string.IsNullOrEmpty(attribute)) {
                    message.AssertionConsumerServiceUrl = new Uri(attribute);
                }

                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.AttributeConsumingServiceIndex);
                if (!string.IsNullOrEmpty(attribute)) {
                    message.AttributeConsumingServiceIndex = new ushort?(XmlConvert.ToUInt16(attribute));
                }

                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.ProviderName);
                if (!string.IsNullOrEmpty(attribute)) {
                    message.ProviderName = attribute;
                }

                reader.ReadStartElement();
                if (!isEmptyElement) {
                    this.ReadCommonElements(reader, message);

                    if (reader.IsStartElement(Saml2Constants.ElementNames.Subject, Saml2Constants.Namespaces.Assertion)) {
                        message.Subject = this.ReadSubject(reader);
                    }

                    if (reader.IsStartElement(Saml2Constants.ElementNames.NameIDPolicy, Saml2Constants.Namespaces.Protocol)) {
                        message.NameIdentifierPolicy = this.ReadNameIDPolicy(reader);
                    }

                    if (reader.IsStartElement(Saml2Constants.ElementNames.Conditions, Saml2Constants.Namespaces.Assertion)) {
                        message.Conditions = this.ReadConditions(reader);
                    }

                    if (reader.IsStartElement(Saml2Constants.ElementNames.RequestedAuthnContext, Saml2Constants.Namespaces.Protocol)) {
                        message.RequestedAuthenticationContext = this.ReadRequestedAuthnContext(reader);
                    }

                    if (reader.IsStartElement(Saml2Constants.ElementNames.Scoping, Saml2Constants.Namespaces.Protocol)) {
                        message.Scoping = this.ReadScoping(reader);
                    }

                    reader.ReadEndElement();
                }

                if (this.handleSignature) {
                    this.SetSigningCredentials(reader, message);
                }

                return message;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:LogoutRequest element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on a samlp:LogoutRequest opening tag.</param>
        /// <returns>Returns a <c>Saml2LogoutRequest</c> object.</returns>
        protected virtual Saml2LogoutRequest ReadLogoutRequest(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                Saml2LogoutRequest data = new Saml2LogoutRequest(new Saml2NameIdentifier("__TEMP__"));
                if (this.handleSignature) {
                    reader = this.CreateEnvelopedSignatureReader(reader, data);
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.LogoutRequest, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.LogoutRequest, Saml2Constants.Namespaces.Protocol);
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.LogoutRequestType, Saml2Constants.Namespaces.Protocol);

                bool isEmptyElement = reader.IsEmptyElement;
                this.ReadCommonAttributes(reader, data);

                // Reason attribute
                string attribute = reader.GetAttribute(Saml2Constants.AttributeNames.Reason);
                if (!string.IsNullOrEmpty(attribute)) {
                    data.Reason = attribute;
                }

                // NotOnOrAfter attribute
                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.NotOnOrAfter);
                if (!string.IsNullOrEmpty(attribute)) {
                    data.NotOnOrAfter = new DateTime?(XmlConvert.ToDateTime(attribute, DateTimeFormats.Accepted));
                }

                reader.ReadStartElement();
                if (isEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                // Saml2Message elements
                this.ReadCommonElements(reader, data);
                data.NameId = this.ReadNameId(reader, Saml2Constants.ElementNames.LogoutRequest);

                while (reader.IsStartElement(Saml2Constants.ElementNames.SessionIndex, Saml2Constants.Namespaces.Protocol)) {
                    data.SessionIndex.Add(ReadSimpleStringElement(reader));
                }

                reader.ReadEndElement();

                if (this.handleSignature) {
                    this.SetSigningCredentials(reader, data);
                }

                return data;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:LogoutResponse element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on a samlp:LogoutResponse opening tag.</param>
        /// <returns>Returns a <c>Saml2LogoutResponse</c> object.</returns>
        protected virtual Saml2LogoutResponse ReadLogoutResponse(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                Saml2LogoutResponse message = new Saml2LogoutResponse(new Saml2Status(Saml2StatusCode.Responder));
                if (this.handleSignature) {
                    reader = this.CreateEnvelopedSignatureReader(reader, message);
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.LogoutResponse, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.LogoutResponse, Saml2Constants.Namespaces.Protocol);
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.StatusResponseType, Saml2Constants.Namespaces.Protocol);

                if (reader.IsEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                // SamlMessahe attributes
                this.ReadCommonAttributes(reader, message);

                // InResponseTo attribute
                string attribute = reader.GetAttribute(Saml2Constants.AttributeNames.InResponseTo);
                if (!string.IsNullOrEmpty(attribute)) {
                    message.InResponseTo = new Saml2Id(attribute);
                }

                // Saml Message elements
                reader.ReadStartElement();
                this.ReadCommonElements(reader, message);

                message.Status = this.ReadStatus(reader);
                reader.ReadEndElement();

                if (this.handleSignature) {
                    this.SetSigningCredentials(reader, message);
                }

                return message;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:AssertionIDRequest.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a samlp:AssertionIDRequest element.</param>
        /// <returns>The deserialized <c>Saml2AssertionIdRequest</c>.</returns>
        protected virtual Saml2AssertionIdRequest ReadAssertionIDRequest(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                Saml2AssertionIdRequest message = new Saml2AssertionIdRequest(new Saml2Id("_DUMMY_"));
                if (this.handleSignature) {
                    reader = this.CreateEnvelopedSignatureReader(reader, message);
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.AssertionIDRequest, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.AssertionIDRequest, Saml2Constants.Namespaces.Protocol);
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.AssertionIDRequestType, Saml2Constants.Namespaces.Protocol);

                if (reader.IsEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                this.ReadCommonAttributes(reader, message);
                reader.ReadStartElement();

                this.ReadCommonElements(reader, message);

                message.AssertionIdReferences.Clear();
                while (reader.IsStartElement(Saml2Constants.ElementNames.AssertionIDRef, Saml2Constants.Namespaces.Protocol)) {
                    message.AssertionIdReferences.Add(new Saml2Id(ReadSimpleStringElement(reader)));
                }

                if (message.AssertionIdReferences.Count == 0) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2021Format(Saml2Constants.ElementNames.AssertionIDRef));
                }

                reader.ReadEndElement();

                if (this.handleSignature) {
                    this.SetSigningCredentials(reader, message);
                }

                return message;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:ManageNameIDRequest.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a samlp:ManageNameIDRequest element.</param>
        /// <returns>The deserialized <c>ManageNameIdRequest</c>.</returns>
        protected virtual Saml2ManageNameIdRequest ReadManageNameIDRequest(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                Saml2ManageNameIdRequest message = new Saml2ManageNameIdRequest(new Saml2NameIdentifier("_DUMMY_"));
                if (this.handleSignature) {
                    reader = this.CreateEnvelopedSignatureReader(reader, message);
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.ManageNameIDRequest, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.ManageNameIDRequest, Saml2Constants.Namespaces.Protocol);
                }

                if (reader.IsEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.ManageNameIDRequestType, Saml2Constants.Namespaces.Protocol);

                this.ReadCommonAttributes(reader, message);

                reader.ReadStartElement();
                this.ReadCommonElements(reader, message);

                message.Identifier = this.ReadNameId(reader, Saml2Constants.ElementNames.ManageNameIDRequest);

                if (reader.IsStartElement(Saml2Constants.ElementNames.Terminate, Saml2Constants.Namespaces.Protocol)) {
                    message.Terminate = true;
                    bool isEmptyElement = reader.IsEmptyElement;
                    reader.ReadStartElement(Saml2Constants.ElementNames.Terminate, Saml2Constants.Namespaces.Protocol);
                    if (!isEmptyElement) {
                        reader.ReadEndElement();
                    }
                }
                else {
                    message.NewIdentifier = this.ReadNewID(reader);
                }

                if (this.handleSignature) {
                    this.SetSigningCredentials(reader, message);
                }

                return message;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:ManageNameIDResponse.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a samlp:ManageNameIDResponse element.</param>
        /// <returns>The deserialized <c>Saml2ManageNameIdResponse</c>.</returns>
        protected virtual Saml2ManageNameIdResponse ReadManageNameIDResponse(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                Saml2ManageNameIdResponse message = new Saml2ManageNameIdResponse(new Saml2Status(Saml2StatusCode.Responder));
                if (this.handleSignature) {
                    reader = this.CreateEnvelopedSignatureReader(reader, message);
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.ManageNameIDResponse, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.ManageNameIDResponse, Saml2Constants.Namespaces.Protocol);
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.ManageNameIDResponseType, Saml2Constants.Namespaces.Protocol);

                this.ReadCommonAttributes(reader, message);
                string attribute = reader.GetAttribute(Saml2Constants.AttributeNames.InResponseTo);

                if (!string.IsNullOrEmpty(attribute)) {
                    message.InResponseTo = new Saml2Id(attribute);
                }

                reader.ReadStartElement();

                this.ReadCommonElements(reader, message);

                message.Status = this.ReadStatus(reader);

                reader.ReadEndElement();

                if (this.handleSignature) {
                    this.SetSigningCredentials(reader, message);
                }

                return message;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:NameIDMappingRequest.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a samlp:NameIDMappingRequest element.</param>
        /// <returns>The deserialized <c>Saml2NameIdMappingRequest</c>.</returns>
        protected virtual Saml2NameIdMappingRequest ReadNameIDMappingRequest(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                Saml2NameIdMappingRequest message = new Saml2NameIdMappingRequest(new Saml2NameIdentifier("_DUMMY_"));
                if (this.handleSignature) {
                    reader = this.CreateEnvelopedSignatureReader(reader, message);
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.NameIDMappingRequest, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.NameIDMappingRequest, Saml2Constants.Namespaces.Protocol);
                }

                if (reader.IsEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.NameIDMappingRequestType, Saml2Constants.Namespaces.Protocol);

                this.ReadCommonAttributes(reader, message);

                reader.ReadStartElement();
                this.ReadCommonElements(reader, message);

                message.Identifier = this.ReadNameId(reader, Saml2Constants.ElementNames.NameIDMappingRequest);

                message.NameIdentifierPolicy = this.ReadNameIDPolicy(reader);
                reader.ReadEndElement();

                if (this.handleSignature) {
                    this.SetSigningCredentials(reader, message);
                }

                return message;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:NameIDMappingResponse.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a samlp:NameIDMappingResponse element.</param>
        /// <returns>The deserialized <c>Saml2NameIdMappingResponse</c>.</returns>
        protected virtual Saml2NameIdMappingResponse ReadNameIDMappingResponse(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                Saml2NameIdMappingResponse message = new Saml2NameIdMappingResponse(new Saml2Status(Saml2StatusCode.Responder), new Saml2NameIdentifier("_DUMMY_"));
                if (this.handleSignature) {
                    reader = this.CreateEnvelopedSignatureReader(reader, message);
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.NameIDMappingResponse, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.NameIDMappingResponse, Saml2Constants.Namespaces.Protocol);
                }

                if (reader.IsEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.NameIDMappingResponseType, Saml2Constants.Namespaces.Protocol);

                this.ReadCommonAttributes(reader, message);
                string attribute = reader.GetAttribute(Saml2Constants.AttributeNames.InResponseTo);
                if (!string.IsNullOrEmpty(attribute)) {
                    message.InResponseTo = new Saml2Id(attribute);
                }

                reader.ReadStartElement();

                this.ReadCommonElements(reader, message);

                message.Status = this.ReadStatus(reader);

                message.NameId = this.ReadNameId(reader, Saml2Constants.ElementNames.NameIDMappingResponse);

                reader.ReadEndElement();

                if (this.handleSignature) {
                    this.SetSigningCredentials(reader, message);
                }

                return message;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:NewID or samlp:NewEncryptedID element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a samlp:NewID or samlp:NewEncryptedID element.</param>
        /// <returns>The deserialized <c>Saml2NewNameIdentifier</c>.</returns>
        protected virtual Saml2NewNameIdentifier ReadNewID(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                EncryptingCredentials encryptingCredentials = null;
                if (reader.IsStartElement(Saml2Constants.ElementNames.NewEncryptedID, Saml2Constants.Namespaces.Protocol)) {
                    using (XmlReader encryptedReader = reader) {
                        XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.EncryptedElementType, Saml2Constants.Namespaces.Protocol);

                        if (encryptedReader.IsEmptyElement) {
                            throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                        }

                        reader.ReadStartElement(Saml2Constants.ElementNames.NewEncryptedID, Saml2Constants.Namespaces.Protocol);

#if AZUREAD
                        throw new NotImplementedException();
#else
                        var handlers = new SecurityTokenHandlerCollection(new SecurityTokenHandler[] {
                            new GenericXmlSecurityTokenHandler(),
                            new EncryptedSecurityTokenHandler() { KeyInfoSerializer = this.KeyInfoSerializer },
                        }) {
                            Configuration = { ServiceTokenResolver = this.EncryptionTokenResolver },
                        };

                        var token = (GenericXmlSecurityToken)handlers.ReadToken(encryptedReader);
                        reader = XmlReader.Create(new System.IO.StringReader(token.TokenXml.OuterXml));

                        encryptedReader.ReadEndElement();
#endif
                    }
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.NewID, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.NewID, Saml2Constants.Namespaces.Protocol);
                }

                if (reader.IsEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                XmlUtil.ValidateXsiType(reader, "string", System.Xml.Schema.XmlSchema.Namespace);

                string newId = reader.ReadElementContentAsString(Saml2Constants.ElementNames.NewID, Saml2Constants.Namespaces.Protocol);

                Saml2NewNameIdentifier identifier = new Saml2NewNameIdentifier(newId);
                identifier.EncryptingCredentials = encryptingCredentials;
                return identifier;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:Response element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on a samlp:Response opening tag.</param>
        /// <returns>Returns a <c>Response</c> object.</returns>
        protected virtual Saml2Response ReadResponse(XmlReader reader/*, NamespaceContext context*/) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                Saml2Response message = new Saml2Response(new Saml2Status(Saml2StatusCode.Responder));
                if (this.handleSignature) {
                    reader = this.CreateEnvelopedSignatureReader(reader, message);
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.Response, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.Response, Saml2Constants.Namespaces.Protocol);
                }

                if (reader.IsEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.ResponseType, Saml2Constants.Namespaces.Protocol);

                // SAML Message attributes
                this.ReadCommonAttributes(reader, message);

                // InResponseTo attribute
                string attribute = reader.GetAttribute(Saml2Constants.AttributeNames.InResponseTo);
                if (!string.IsNullOrEmpty(attribute)) {
                    message.InResponseTo = new Saml2Id(attribute);
                }

                // SAML message elements
                reader.ReadStartElement();
                this.ReadCommonElements(reader, message);

                // Status Element
                message.Status = this.ReadStatus(reader);

                // Assertion Or EncryptedAssertion
                while (reader.IsStartElement()) {
                    if (!reader.IsStartElement(Saml2Constants.ElementNames.Assertion, Saml2Constants.Namespaces.Assertion)
                        && !reader.IsStartElement(Saml2Constants.ElementNames.EncryptedAssertion, Saml2Constants.Namespaces.Assertion)) {
                        break;
                    }

                    SecurityTokenElement item = new SecurityTokenElement(XmlUtil.ReadInnerXml(reader, null/*context.GetNamespaces()*/), this.securityTokenHandlerCollection);
                    message.Assertions.Add(item);
                }

                reader.ReadEndElement();

                if (this.handleSignature) {
                    this.SetSigningCredentials(reader, message);
                }

                return message;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:Extensions element.
        /// </summary>
        /// <remarks>By default, this skips the element and traces an Information </remarks>
        /// <param name="reader">An <c>XmlReader</c> positioned on a samlp:Extensions opening tag.</param>
        /// <param name="message">The <c>Saml2Message</c> object to update with extensions.</param>
        protected virtual void ReadExtensions(XmlReader reader, Saml2Message message) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            try {
                if (!reader.IsStartElement(Saml2Constants.ElementNames.Extensions, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.Extensions, Saml2Constants.Namespaces.Protocol);
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.ExtensionsType, Saml2Constants.Namespaces.Protocol);

                bool isEmptyElement = reader.IsEmptyElement;

                reader.ReadStartElement();
                if (!isEmptyElement) {
                    var authenticationRequest = message as Saml2AuthenticationRequest;
                    if (authenticationRequest != null) {
                        // Requested Attributes for request message
                        if (reader.IsStartElement(Saml2Constants.ElementNames.RequestedAttributes, Saml2Constants.Namespaces.ReqAttr)) {
                            foreach (var requestedAttribute in this.ReadRequestedAttributes(reader)) {
                                authenticationRequest.RequestedAttributes.Add(requestedAttribute);
                            }
                        }
                    }
                    else {
                        reader.Skip();
                    }

                    reader.ReadEndElement();
                }
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:AuthnQuery.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a samlp:AuthnQuery element.</param>
        /// <returns>The deserialized <c>Saml2AuthenticationQuery</c>.</returns>
        protected virtual Saml2AuthenticationQuery ReadAuthenticationQuery(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                Saml2AuthenticationQuery message = new Saml2AuthenticationQuery(new Saml2Subject(new Saml2NameIdentifier("_DUMMY_")));
                if (this.handleSignature) {
                    reader = this.CreateEnvelopedSignatureReader(reader, message);
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.AuthnQuery, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.AuthnQuery, Saml2Constants.Namespaces.Protocol);
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.AuthnQueryType, Saml2Constants.Namespaces.Protocol);

                bool isEmptyElement = reader.IsEmptyElement;
                this.ReadCommonAttributes(reader, message);

                // SessionIndex
                string value = reader.GetAttribute(Saml2Constants.AttributeNames.SessionIndex);
                if (!string.IsNullOrEmpty(value)) {
                    message.SessionIndex = value;
                }

                reader.ReadStartElement();
                if (isEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                // Saml2Message elements
                this.ReadCommonElements(reader, message);

                // Subject
                message.Subject = this.ReadSubject(reader);

                if (reader.IsStartElement(Saml2Constants.ElementNames.RequestedAuthnContext, Saml2Constants.Namespaces.Protocol)) {
                    message.RequestedAuthenticationContext = this.ReadRequestedAuthnContext(reader);
                }

                reader.ReadEndElement();

                if (this.handleSignature) {
                    this.SetSigningCredentials(reader, message);
                }

                return message;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:AuthzDecisionQuery.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a samlp:AuthzDecisionQuery element.</param>
        /// <returns>The deserialized <c>Saml2AuthorizationDecisionQuery</c>.</returns>
        protected virtual Saml2AuthorizationDecisionQuery ReadAuthorizationDecisionQuery(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                Saml2AuthorizationDecisionQuery message = new Saml2AuthorizationDecisionQuery(new Saml2Subject(new Saml2NameIdentifier("_DUMMY_")), new Uri("http://temp"));
                if (this.handleSignature) {
                    reader = this.CreateEnvelopedSignatureReader(reader, message);
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.AuthzDecisionQuery, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.AuthzDecisionQuery, Saml2Constants.Namespaces.Protocol);
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.AuthzDecisionQueryType, Saml2Constants.Namespaces.Protocol);

                bool isEmptyElement = reader.IsEmptyElement;
                this.ReadCommonAttributes(reader, message);

                // Resource
                string value = reader.GetAttribute(Saml2Constants.AttributeNames.Resource);
                if (string.IsNullOrEmpty(value)) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2006Format(Saml2Constants.AttributeNames.Resource));
                }

                message.Resource = new Uri(value);

                reader.ReadStartElement();
                if (isEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                // Saml2Message elements
                this.ReadCommonElements(reader, message);

                message.Subject = this.ReadSubject(reader);

                // Actions
                reader.Read();
                do {
                    message.Actions.Add(this.ReadAction(reader));
                }
                while (reader.IsStartElement(Saml2Constants.ElementNames.Action, Saml2Constants.Namespaces.Assertion));

                // Evidence
                if (reader.IsStartElement(Saml2Constants.ElementNames.Evidence, Saml2Constants.Namespaces.Assertion)) {
                    message.Evidence = this.ReadEvidence(reader);
                }

                reader.ReadEndElement();

                if (this.handleSignature) {
                    this.SetSigningCredentials(reader, message);
                }

                if (message.Actions.Count == 0) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2011Format(Saml2Constants.ElementNames.Action));
                }

                return message;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:AttributeQuery.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a samlp:AttributeQuery element.</param>
        /// <returns>The deserialized <c>Saml2AttributeQuery</c>.</returns>
        protected virtual Saml2AttributeQuery ReadAttributeQuery(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                Saml2AttributeQuery message = new Saml2AttributeQuery(new Saml2Subject(new Saml2NameIdentifier("_DUMMY_")));
                if (this.handleSignature) {
                    reader = this.CreateEnvelopedSignatureReader(reader, message);
                }

                if (!reader.IsStartElement(Saml2Constants.ElementNames.AttributeQuery, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.AttributeQuery, Saml2Constants.Namespaces.Protocol);
                }

                bool isEmptyElement = reader.IsEmptyElement;

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.AttributeQueryType, Saml2Constants.Namespaces.Protocol);

                this.ReadCommonAttributes(reader, message);

                reader.ReadStartElement();
                if (isEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                this.ReadCommonElements(reader, message);

                message.Subject = this.ReadSubject(reader);

                // Attribute
                while (reader.IsStartElement(Saml2Constants.ElementNames.Attribute, Saml2Constants.Namespaces.Assertion)) {
                    message.Attributes.Add(this.ReadAttribute(reader));
                }

                reader.ReadEndElement();

                if (this.handleSignature) {
                    this.SetSigningCredentials(reader, message);
                }

                return message;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:Artifact element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> on the samlp:Artifact opening tag.</param>
        /// <returns>The base64-encoded artifact that is being requested.</returns>
        protected virtual string ReadAssertionArtifact(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                if (!reader.IsStartElement(Saml2Constants.ElementNames.Artifact, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.Artifact, Saml2Constants.Namespaces.Protocol);
                }

                string value = ReadSimpleStringElement(reader);
                if (string.IsNullOrEmpty(value)) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                return value;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:Status element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on a samlp:Status opening tag.</param>
        /// <returns>Returns a <c>Saml2Status</c> object.</returns>
        protected virtual Saml2Status ReadStatus(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                // Status
                if (!reader.IsStartElement(Saml2Constants.ElementNames.Status, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.Status, Saml2Constants.Namespaces.Protocol);
                }

                if (reader.IsEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.StatusType, Saml2Constants.Namespaces.Protocol);

                reader.ReadStartElement();
                Saml2Status status = new Saml2Status(this.ReadStatusCode(reader));
                if (reader.IsStartElement(Saml2Constants.ElementNames.StatusMessage, Saml2Constants.Namespaces.Protocol)) {
                    status.StatusMessage = ReadSimpleStringElement(reader);
                }

                if (reader.IsStartElement(Saml2Constants.ElementNames.StatusDetail, Saml2Constants.Namespaces.Protocol)) {
                    bool isEmptyElement = reader.IsEmptyElement;

                    XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.StatusDetailType, Saml2Constants.Namespaces.Protocol);

                    if (isEmptyElement) {
                        reader.Read();
                    }
                    else {
                        XmlDocument document = new XmlDocument() { XmlResolver = null, PreserveWhitespace = true };
                        document.Load(reader.ReadSubtree());
                        foreach (XmlElement element in document.DocumentElement.ChildNodes) {
                            status.StatusDetail.Add(element);
                        }

                        reader.ReadEndElement();
                    }
                }

                reader.ReadEndElement();
                return status;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:StatusCode element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on a samlp:StatusCode opening tag.</param>
        /// <returns>Returns a <c>Saml2StatusCode</c> object.</returns>
        protected virtual Saml2StatusCode ReadStatusCode(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                // StatusCode
                if (!reader.IsStartElement(Saml2Constants.ElementNames.StatusCode, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.StatusCode, Saml2Constants.Namespaces.Protocol);
                }

                bool isEmptyElement = reader.IsEmptyElement;

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.StatusCodeType, Saml2Constants.Namespaces.Protocol);

                string attribute = reader.GetAttribute(Saml2Constants.AttributeNames.Value);
                if (string.IsNullOrEmpty(attribute)) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2006Format(Saml2Constants.AttributeNames.Value));
                }

                Saml2StatusCode code = new Saml2StatusCode(XmlUtil.ResolveQName(reader, attribute));
                reader.ReadStartElement();
                if (!isEmptyElement) {
                    if (reader.IsStartElement(Saml2Constants.ElementNames.StatusCode, Saml2Constants.Namespaces.Protocol)) {
                        code.SubStatus = this.ReadStatusCode(reader);
                    }

                    reader.ReadEndElement();
                }

                return code;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:NameIDPolicy element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on a samlp:NameIDPolicy opening tag.</param>
        /// <returns>Returns a <c>Saml2NameIdentifierPolicy</c> object.</returns>
        protected virtual Saml2NameIdentifierPolicy ReadNameIDPolicy(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                if (!reader.IsStartElement(Saml2Constants.ElementNames.NameIDPolicy, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.NameIDPolicy, Saml2Constants.Namespaces.Protocol);
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.NameIDPolicyType, Saml2Constants.Namespaces.Protocol);

                Saml2NameIdentifierPolicy policy = new Saml2NameIdentifierPolicy();
                bool isEmptyElement = reader.IsEmptyElement;

                string attribute = reader.GetAttribute(Saml2Constants.AttributeNames.Format);
                if (!string.IsNullOrEmpty(attribute)) {
                    policy.Format = new Uri(attribute);
                }

                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.SPNameQualifier);
                if (!string.IsNullOrEmpty(attribute)) {
                    policy.SPNameQualifier = attribute;
                }

                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.AllowCreate);
                if (!string.IsNullOrEmpty(attribute)) {
                    policy.AllowCreate = XmlConvert.ToBoolean(attribute);
                }

                reader.ReadStartElement();

                if (!isEmptyElement) {
                    reader.ReadEndElement();
                }

                return policy;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:RequestedAuthnContext element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on a samlp:RequestedAuthnContext opening tag.</param>
        /// <returns>Returns a <c>Saml2RequestedAuthenticationContext</c> object.</returns>
        protected virtual Saml2RequestedAuthenticationContext ReadRequestedAuthnContext(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                if (!reader.IsStartElement(Saml2Constants.ElementNames.RequestedAuthnContext, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.RequestedAuthnContext, Saml2Constants.Namespaces.Protocol);
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.RequestedAuthnContextType, Saml2Constants.Namespaces.Protocol);

                if (reader.IsEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                Saml2RequestedAuthenticationContext context = new Saml2RequestedAuthenticationContext();

                string attribute = reader.GetAttribute(Saml2Constants.AttributeNames.Comparison);
                switch (attribute) {
                    case "maximum":
                        context.Comparison = Saml2AuthenticationContextComparisonType.Maximum;
                        break;
                    case "minimum":
                        context.Comparison = Saml2AuthenticationContextComparisonType.Minimum;
                        break;
                    case "better":
                        context.Comparison = Saml2AuthenticationContextComparisonType.Better;
                        break;
                    case "exact":
                    case null:
                        context.Comparison = Saml2AuthenticationContextComparisonType.Exact;
                        break;
                    default:
                        throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2022Format(attribute));
                }

                reader.ReadStartElement();
                if (reader.IsStartElement(Saml2Constants.ElementNames.AuthnContextClassRef, Saml2Constants.Namespaces.Assertion)) {
                    context.ReferenceType = Saml2AuthenticationContextReferenceType.Class;

                    while (reader.IsStartElement(Saml2Constants.ElementNames.AuthnContextClassRef, Saml2Constants.Namespaces.Assertion)) {
                        context.References.Add(ReadSimpleUriElement(reader));
                    }
                }
                else if (reader.IsStartElement(Saml2Constants.ElementNames.AuthnContextDeclRef, Saml2Constants.Namespaces.Assertion)) {
                    context.ReferenceType = Saml2AuthenticationContextReferenceType.Declaration;

                    while (reader.IsStartElement(Saml2Constants.ElementNames.AuthnContextDeclRef, Saml2Constants.Namespaces.Assertion)) {
                        context.References.Add(ReadSimpleUriElement(reader));
                    }
                }
                else {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2021Format(Saml2Constants.ElementNames.AuthnContextClassRef));
                }

                reader.ReadEndElement();

                return context;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:Scoping element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on a samlp:Scoping opening tag.</param>
        /// <returns>Returns a <c>Saml2Scoping</c> object.</returns>
        protected virtual Saml2Scoping ReadScoping(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                if (!reader.IsStartElement(Saml2Constants.ElementNames.Scoping, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.Scoping, Saml2Constants.Namespaces.Protocol);
                }

                bool isEmptyElement = reader.IsEmptyElement;

                Saml2Scoping scoping = new Saml2Scoping();

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.ScopingType, Saml2Constants.Namespaces.Protocol);

                string attribute = reader.GetAttribute(Saml2Constants.AttributeNames.ProxyCount);
                if (!string.IsNullOrEmpty(attribute)) {
                    scoping.ProxyCount = new uint?(XmlConvert.ToUInt32(attribute));
                }

                reader.ReadStartElement();
                if (!isEmptyElement) {
                    if (reader.IsStartElement(Saml2Constants.ElementNames.IDPList, Saml2Constants.Namespaces.Protocol)) {
                        scoping = new Saml2Scoping(this.ReadIDPList(reader)) {
                            ProxyCount = scoping.ProxyCount
                        };
                    }

                    while (reader.IsStartElement(Saml2Constants.ElementNames.RequesterID, Saml2Constants.Namespaces.Protocol)) {
                        scoping.RequesterIds.Add(ReadSimpleUriElement(reader));
                    }

                    reader.ReadEndElement();
                }

                return scoping;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:IDPList element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on a samlp:IDPList opening tag.</param>
        /// <returns>Returns a <c>Saml2IdentityProviderList</c> object.</returns>
        protected virtual Saml2IdentityProviderList ReadIDPList(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                if (!reader.IsStartElement(Saml2Constants.ElementNames.IDPList, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.IDPList, Saml2Constants.Namespaces.Protocol);
                }

                if (reader.IsEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.IDPListType, Saml2Constants.Namespaces.Protocol);

                Saml2IdentityProviderList providers = new Saml2IdentityProviderList();

                reader.ReadStartElement();
                while (reader.IsStartElement(Saml2Constants.ElementNames.IDPEntry, Saml2Constants.Namespaces.Protocol)) {
                    providers.Add(this.ReadIDPEntry(reader));
                }

                if (providers.Count == 0) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2021Format(Saml2Constants.ElementNames.IDPEntry));
                }

                if (reader.IsStartElement(Saml2Constants.ElementNames.GetComplete, Saml2Constants.Namespaces.Protocol)) {
                    providers.GetComplete = ReadSimpleUriElement(reader);
                }

                reader.ReadEndElement();
                return providers;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a samlp:IDPEntry element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on a samlp:IDPEntry opening tag.</param>
        /// <returns>Returns a <c>Saml2IdentityProviderEntry</c> object.</returns>
        protected virtual Saml2IdentityProviderEntry ReadIDPEntry(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                if (!reader.IsStartElement(Saml2Constants.ElementNames.IDPEntry, Saml2Constants.Namespaces.Protocol)) {
                    reader.ReadStartElement(Saml2Constants.ElementNames.IDPEntry, Saml2Constants.Namespaces.Protocol);
                }

                bool isEmptyElement = reader.IsEmptyElement;

                XmlUtil.ValidateXsiType(reader, Saml2Constants.XmlTypes.IDPEntryType, Saml2Constants.Namespaces.Protocol);

                string attribute = reader.GetAttribute(Saml2Constants.AttributeNames.ProviderID);
                if (string.IsNullOrEmpty(attribute)) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2006Format(Saml2Constants.AttributeNames.ProviderID));
                }

                Saml2IdentityProviderEntry entry = new Saml2IdentityProviderEntry(new Uri(attribute));

                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.Name);
                if (!string.IsNullOrEmpty(attribute)) {
                    entry.Name = attribute;
                }

                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.Loc);
                if (!string.IsNullOrEmpty(attribute)) {
                    entry.Location = new Uri(attribute);
                }

                reader.ReadStartElement();

                if (!isEmptyElement) {
                    reader.ReadEndElement();
                }

                return entry;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes the req-attr:RequestedAttributes element.
        /// </summary>
        /// <param name="reader">An <see cref="XmlReader"/> positioned at the element to read.</param>
        /// <returns>The <see cref="ICollection&lt;RequestedAttribute&gt;"/> represent requested attribute.</returns>
        protected virtual ICollection<RequestedAttribute> ReadRequestedAttributes(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            var requestedAttributes = new List<RequestedAttribute>();
            if (reader.IsStartElement(Saml2Constants.ElementNames.RequestedAttributes, Saml2Constants.Namespaces.ReqAttr)) {
                reader.ReadStartElement(Saml2Constants.ElementNames.RequestedAttributes, Saml2Constants.Namespaces.ReqAttr);

                while (reader.IsStartElement(MetadataConstants.ElementNames.RequestedAttribute, MetadataConstants.Namespaces.Metadata)) {
                    requestedAttributes.Add(this.ReadRequestedAttribute(reader));
                }

                reader.ReadEndElement();
            }

            return requestedAttributes;
        }

        /// <summary>
        /// Deserializes the md:RequestedAttribute element.
        /// </summary>
        /// <param name="reader">An <see cref="XmlReader"/> positioned at the element to read.</param>
        /// <returns>The <see cref="RequestedAttribute"/> represent requested attribute.</returns>
        protected virtual RequestedAttribute ReadRequestedAttribute(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            if (!reader.IsStartElement(MetadataConstants.ElementNames.RequestedAttribute, MetadataConstants.Namespaces.Metadata)) {
                reader.ReadStartElement(MetadataConstants.ElementNames.RequestedAttribute, MetadataConstants.Namespaces.Metadata);
            }

            try {
                bool isEmptyElement = reader.IsEmptyElement;
                XmlUtil.ValidateXsiType(reader, MetadataConstants.XmlTypes.RequestedAttributeType, MetadataConstants.Namespaces.Metadata);

                // Name
                string name = reader.GetAttribute(Saml2Constants.AttributeNames.Name);
                if (string.IsNullOrEmpty(name)) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2906Format(Saml2Constants.AttributeNames.Name));
                }

                // NameFormat
                string attribute = reader.GetAttribute(Saml2Constants.AttributeNames.NameFormat);
                if (string.IsNullOrEmpty(attribute)) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2906Format(Saml2Constants.AttributeNames.NameFormat));
                }

                Uri nameFormat;
                if (!Uri.TryCreate(attribute, UriKind.Absolute, out nameFormat)) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2903Format(Saml2Constants.AttributeNames.NameFormat));
                }

                var requestedAttribute = new RequestedAttribute(name, nameFormat);

                // FrendlyName
                requestedAttribute.FriendlyName = reader.GetAttribute(Saml2Constants.AttributeNames.FriendlyName);

                // IsRequired
                attribute = reader.GetAttribute(MetadataConstants.AttributeNames.IsRequired);
                if (!string.IsNullOrEmpty(attribute)) {
                    requestedAttribute.IsRequired = XmlConvert.ToBoolean(attribute);
                }

                reader.Read();

                // Attribute Value
                if (!isEmptyElement) {
                    while (reader.IsStartElement(Saml2Constants.ElementNames.AttributeValue, Saml2Constants.Namespaces.Assertion)) {
                        isEmptyElement = reader.IsEmptyElement;
                        if (isEmptyElement) {
                            reader.Read();
                            requestedAttribute.Values.Add(string.Empty);
                        }
                        else {
                            requestedAttribute.Values.Add(this.ReadAttributeValue(reader, requestedAttribute));
                        }
                    }

                    reader.ReadEndElement();
                }

                return requestedAttribute;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }
#endregion

#region Common Read

        /// <summary>
        /// Deserializes the attributes found on all SAML protocol messages.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a SAML protocol message.</param>
        /// <param name="message">The <c>Saml2Message</c> object to update.</param>
        protected virtual void ReadCommonAttributes(XmlReader reader, Saml2Message message) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            try {
                // ID
                string attribute = reader.GetAttribute(Saml2Constants.AttributeNames.ID);
                if (string.IsNullOrEmpty(attribute)) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2006Format(Saml2Constants.AttributeNames.ID));
                }

                message.Id = new Saml2Id(attribute);

                // Version
                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.Version);
                if (string.IsNullOrEmpty(attribute)) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2006Format(Saml2Constants.AttributeNames.Version));
                }

                if (!StringComparer.Ordinal.Equals(message.Version, attribute)) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2023Format(attribute));
                }

                // IssueInstant
                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.IssueInstant);
                if (string.IsNullOrEmpty(attribute)) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2006Format(Saml2Constants.AttributeNames.IssueInstant));
                }

                message.IssueInstant = XmlConvert.ToDateTime(attribute, DateTimeFormats.Accepted);

                // Destination
                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.Destination);
                if (!string.IsNullOrEmpty(attribute)) {
                    message.Destination = new Uri(attribute);
                }

                attribute = reader.GetAttribute(Saml2Constants.AttributeNames.Consent);
                if (!string.IsNullOrEmpty(attribute)) {
                    message.Consent = new Uri(attribute);
                }
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Reads the common child elements of all SAML protocol messages.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the &lt;Issuer&gt; opening tag.</param>
        /// <param name="message">The <c>Saml2Message</c> object to update.</param>
        protected virtual void ReadCommonElements(XmlReader reader, Saml2Message message) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            try {
                if (reader.IsStartElement(Saml2Constants.ElementNames.Issuer, Saml2Constants.Namespaces.Assertion)) {
                    message.Issuer = this.ReadIssuer(reader);
                }

                if (reader.IsStartElement(XmlSignatureConstants.ElementNames.Signature, XmlSignatureConstants.Namespace)) {
                    EnvelopedSignatureReader envelopedReader = reader as EnvelopedSignatureReader;
                    if (envelopedReader != null) {
#if AZUREAD
                        envelopedReader.Read();
#else
                        envelopedReader.TryReadSignature();
#endif
                    }
                    else {
                        reader.Skip();
                    }
                }

                if (reader.IsStartElement(Saml2Constants.ElementNames.Extensions, Saml2Constants.Namespaces.Protocol)) {
                    this.ReadExtensions(reader, message);
                }
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        /// <summary>
        /// Deserializes a saml:Subject element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a saml:Subject element.</param>
        /// <returns>The deserialized <c>Saml2Subject</c> object.</returns>
        protected virtual Saml2Subject ReadSubject(XmlReader reader) {
            return this.serializerAdaptor.ReadSubjectFromReader(reader);
        }

        /// <summary>
        /// Deserializes a saml:Conditions element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a saml:Conditions element.</param>
        /// <returns>The deserialized <c>Saml2Conditions</c> object.</returns>
        protected virtual Saml2Conditions ReadConditions(XmlReader reader) {
            return this.serializerAdaptor.ReadConditionsFromReader(reader);
        }

        /// <summary>
        /// Deserializes a saml:Assertion element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on a saml:Assertion opening tag.</param>
        /// <returns>Returns a <c>Saml2Assertion</c> object.</returns>
        protected virtual Saml2Assertion ReadAssertion(XmlReader reader) {
            return this.serializerAdaptor.ReadAssertionFromReader(reader);
        }

        /// <summary>
        /// Deserializes a saml:Action element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a saml:Action element.</param>
        /// <returns>The deserialized <c>Saml2Action</c> object.</returns>
        protected virtual Saml2Action ReadAction(XmlReader reader) {
            return this.serializerAdaptor.ReadActionFromReader(reader);
        }

        /// <summary>
        /// Deserializes a saml:Evidence element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a saml:Evidence element.</param>
        /// <returns>The deserialized <c>Saml2Evidence</c> object.</returns>
        protected virtual Saml2Evidence ReadEvidence(XmlReader reader) {
            return this.serializerAdaptor.ReadEvidenceFromReader(reader);
        }

        /// <summary>
        /// Deserializes a saml:Attribute.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a samlp:Attribute element.</param>
        /// <returns>The deserialized <c>Saml2Attribute</c>.</returns>
        protected virtual Saml2Attribute ReadAttribute(XmlReader reader) {
            return this.serializerAdaptor.ReadAttributeFromReader(reader);
        }

        /// <summary>
        /// Deserializes a saml:Issuer tag.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on an opening tag of a saml:Issuer element.</param>
        /// <returns>A <c>Saml2NameIdentifier</c> representing the issuer.</returns>
        protected virtual Saml2NameIdentifier ReadIssuer(XmlReader reader) {
            return this.serializerAdaptor.ReadIssuerFromReader(reader);
        }

        /// <summary>
        /// Deserializes a saml:NameId or saml:EncryptedId element.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a saml:NameId or saml:EncryptedId element.</param>
        /// <param name="parentElement">The local name of this element's parent element.</param>
        /// <returns>The deserialized <c>Saml2NameIdentifier</c>.</returns>
        protected virtual Saml2NameIdentifier ReadNameId(XmlReader reader, string parentElement) {
            return this.serializerAdaptor.ReadNameIdFromReader(reader, parentElement);
        }

        /// <summary>
        /// Reads the attribute value.
        /// </summary>
        /// <param name="reader">An <c>XmlReader</c> positioned on the opening tag of a saml:AttributeValue element.</param>
        /// <param name="attribute">The <see cref="Saml2Attribute"/>.</param>
        /// <returns>The attribute value as a string.</returns>
        protected virtual string ReadAttributeValue(XmlReader reader, Saml2Attribute attribute) {
            return this.serializerAdaptor.ReadAttributeValueFromReader(reader, attribute);
        }

#endregion

#region Write
        
        /// <summary>
        /// Serializes a samlp:ArtifactResolve element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2ArtifactResolve</c> message to be serialized.</param>
        protected virtual void WriteArtifactResolve(XmlWriter writer, Saml2ArtifactResolve message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer = this.CreateEnvelopedSignatureWriter(writer, message);
            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.ArtifactResolve, Saml2Constants.Namespaces.Protocol);
            this.WriteCommonAttributes(writer, message);
            this.WriteCommonElements(writer, message);
            this.WriteArtifact(writer, message.Artifact);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:ArtifactResponse element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2ArtifactResponse</c> message to be serialized.</param>
        protected virtual void WriteArtifactResponse(XmlWriter writer, Saml2ArtifactResponse message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer = this.CreateEnvelopedSignatureWriter(writer, message);
            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.ArtifactResponse, Saml2Constants.Namespaces.Protocol);
            this.WriteCommonAttributes(writer, message);

            // InResponseTo attribute
            if (message.InResponseTo != null) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.InResponseTo, message.InResponseTo.Value);
            }

            this.WriteCommonElements(writer, message);
            this.WriteStatus(writer, message.Status);

            if (message.Response != null) {
                if (message.Response.ResponseXml != null) {
                    message.Response.ResponseXml.WriteTo(writer);
                }
                else {
                    Debug.Assert(message.Response.SamlMessage != null);
                    XmlWriter nodeWriter = XmlWriter.Create(writer);
                    this.WriteSamlMessage(nodeWriter, message.Response.SamlMessage);
                }
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:AuthnRequest.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize the message to.</param>
        /// <param name="message">The <c>Saml2AuthenticationRequest</c> to be serialized.</param>
        protected virtual void WriteAuthnRequest(XmlWriter writer, Saml2AuthenticationRequest message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer = this.CreateEnvelopedSignatureWriter(writer, message);
            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.AuthnRequest, Saml2Constants.Namespaces.Protocol);
            this.WriteCommonAttributes(writer, message);

            if (message.ForceAuthentication) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.ForceAuthn, XmlConvert.ToString(message.ForceAuthentication));
            }

            if (message.IsPassive) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.IsPassive, XmlConvert.ToString(message.IsPassive));
            }

            if (message.ProtocolBinding != null) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.ProtocolBinding, message.ProtocolBinding.OriginalString);
            }

            if (message.AssertionConsumerServiceIndex.HasValue) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.AssertionConsumerServiceIndex, XmlConvert.ToString(message.AssertionConsumerServiceIndex.Value));
            }

            if (message.AssertionConsumerServiceUrl != null) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.AssertionConsumerServiceURL, message.AssertionConsumerServiceUrl.OriginalString);
            }

            if (message.AttributeConsumingServiceIndex.HasValue) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.AttributeConsumingServiceIndex, XmlConvert.ToString(message.AttributeConsumingServiceIndex.Value));
            }

            if (!string.IsNullOrEmpty(message.ProviderName)) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.ProviderName, message.ProviderName);
            }

            this.WriteCommonElements(writer, message);

            if (message.Subject != null) {
                this.WriteSubject(writer, message.Subject);
            }

            if (message.NameIdentifierPolicy != null) {
                this.WriteNameIDPolicy(writer, message.NameIdentifierPolicy);
            }

            if (message.Conditions != null) {
                this.WriteConditions(writer, message.Conditions);
            }

            if (message.RequestedAuthenticationContext != null) {
                this.WriteRequestedAuthnContext(writer, message.RequestedAuthenticationContext);
            }

            if (message.Scoping != null) {
                this.WriteScoping(writer, message.Scoping);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:LogoutRequest element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2LogoutRequest</c> to serialize.</param>
        protected virtual void WriteLogoutRequest(XmlWriter writer, Saml2LogoutRequest message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer = this.CreateEnvelopedSignatureWriter(writer, message);
            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.LogoutRequest, Saml2Constants.Namespaces.Protocol);
            this.WriteCommonAttributes(writer, message);

            if (message.NotOnOrAfter.HasValue) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.NotOnOrAfter, XmlConvert.ToString(message.NotOnOrAfter.Value, DateTimeFormats.Generated));
            }

            if (!string.IsNullOrEmpty(message.Reason)) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.Reason, message.Reason);
            }

            this.WriteCommonElements(writer, message);
            this.WriteNameId(writer, message.NameId);
            foreach (string index in message.SessionIndex) {
                writer.WriteElementString(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.SessionIndex, Saml2Constants.Namespaces.Protocol, index);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:LogoutResponse element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2LogoutResponse</c> to serialize.</param>
        protected virtual void WriteLogoutResponse(XmlWriter writer, Saml2LogoutResponse message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer = this.CreateEnvelopedSignatureWriter(writer, message);
            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.LogoutResponse, Saml2Constants.Namespaces.Protocol);

            this.WriteCommonAttributes(writer, message);

            if (message.InResponseTo != null) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.InResponseTo, message.InResponseTo.Value);
            }

            this.WriteCommonElements(writer, message);
            this.WriteStatus(writer, message.Status);

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:Response element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2Response</c> to serialize.</param>
        protected virtual void WriteResponse(XmlWriter writer, Saml2Response message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer = this.CreateEnvelopedSignatureWriter(writer, message);
            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.Response, Saml2Constants.Namespaces.Protocol);

            this.WriteCommonAttributes(writer, message);

            if (message.InResponseTo != null) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.InResponseTo, message.InResponseTo.Value);
            }

            this.WriteCommonElements(writer, message);
            this.WriteStatus(writer, message.Status);

            foreach (SecurityTokenElement element in message.Assertions) {
                if (element.SecurityTokenXml != null) {
                    element.SecurityTokenXml.WriteTo(writer);
                }
                else {
                    Saml2SecurityToken securityToken = element.GetSecurityToken() as Saml2SecurityToken;
                    Debug.Assert(securityToken != null);
                    this.WriteAssertion(writer, securityToken.Assertion);
                }
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:Artifact element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="artifact">The artifact to be serialized.</param>
        protected virtual void WriteArtifact(XmlWriter writer, string artifact) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (artifact == null) {
                throw new ArgumentNullException(nameof(artifact));
            }

            writer.WriteElementString(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.Artifact, Saml2Constants.Namespaces.Protocol, artifact);
        }

        /// <summary>
        /// Serializes a samlp:AuthnQuery element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2AuthenticationQuery</c> to serialize.</param>
        protected virtual void WriteAuthenticationQuery(XmlWriter writer, Saml2AuthenticationQuery message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer = this.CreateEnvelopedSignatureWriter(writer, message);
            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.AuthnQuery, Saml2Constants.Namespaces.Protocol);

            this.WriteCommonAttributes(writer, message);

            // SessionIndex
            if (!string.IsNullOrEmpty(message.SessionIndex)) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.SessionIndex, message.SessionIndex);
            }

            this.WriteCommonElements(writer, message);

            // Subject
            this.WriteSubject(writer, message.Subject);

            // samlp:RequestedAuthnContext
            if (message.RequestedAuthenticationContext != null) {
                this.WriteRequestedAuthnContext(writer, message.RequestedAuthenticationContext);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:AuthzDecisionQuery element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2AuthorizationDecisionQuery</c> to serialize.</param>
        protected virtual void WriteAuthorizationDecisionQuery(XmlWriter writer, Saml2AuthorizationDecisionQuery message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer = this.CreateEnvelopedSignatureWriter(writer, message);
            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.AuthzDecisionQuery, Saml2Constants.Namespaces.Protocol);

            this.WriteCommonAttributes(writer, message);

            // Resource
            if (message.Resource != null) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.Resource, message.Resource.OriginalString);
            }

            this.WriteCommonElements(writer, message);

            // Subject
            this.WriteSubject(writer, message.Subject);

            // Action
            if (message.Actions != null) {
                foreach (var action in message.Actions) {
                    this.WriteAction(writer, action);
                }
            }

            // Evidence
            if (message.Evidence != null) {
                this.WriteEvidence(writer, message.Evidence);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:AttributeQuery element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2AttributeQuery</c> to serialize.</param>
        protected virtual void WriteAttributeQuery(XmlWriter writer, Saml2AttributeQuery message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer = this.CreateEnvelopedSignatureWriter(writer, message);
            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.AttributeQuery, Saml2Constants.Namespaces.Protocol);

            this.WriteCommonAttributes(writer, message);

            this.WriteCommonElements(writer, message);

            // Subject
            this.WriteSubject(writer, message.Subject);

            // Attributes
            if (message.Attributes != null) {
                foreach (var attribute in message.Attributes) {
                    this.WriteAttribute(writer, attribute);
                }
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:AssertionIDRequest element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2AssertionIdRequest</c> to serialize.</param>
        protected virtual void WriteAssertionIDRequest(XmlWriter writer, Saml2AssertionIdRequest message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer = this.CreateEnvelopedSignatureWriter(writer, message);
            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.AssertionIDRequest, Saml2Constants.Namespaces.Protocol);

            this.WriteCommonAttributes(writer, message);
            this.WriteCommonElements(writer, message);

            foreach (var reference in message.AssertionIdReferences) {
                writer.WriteElementString(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.AssertionIDRef, Saml2Constants.Namespaces.Protocol, reference.Value);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:ManageNameIDRequest element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2ManageNameIdRequest</c> to serialize.</param>
        protected virtual void WriteManageNameIDRequest(XmlWriter writer, Saml2ManageNameIdRequest message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            if (!message.Terminate && message.NewIdentifier == null) {
                throw DiagnosticTools.ExceptionUtil.ThrowHelperArgument("ID4359", "message");
            }

            writer = this.CreateEnvelopedSignatureWriter(writer, message);
            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.ManageNameIDRequest, Saml2Constants.Namespaces.Protocol);

            this.WriteCommonAttributes(writer, message);

            this.WriteCommonElements(writer, message);
            this.WriteNameId(writer, message.Identifier);

            if (message.Terminate) {
                writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.Terminate, Saml2Constants.Namespaces.Protocol);
                writer.WriteEndElement();
            }
            else {
                this.WriteNewID(writer, message.NewIdentifier);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:ManageNameIDResponse element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2ManageNameIdResponse</c> to serialize.</param>
        protected virtual void WriteManageNameIDResponse(XmlWriter writer, Saml2ManageNameIdResponse message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer = this.CreateEnvelopedSignatureWriter(writer, message);
            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.ManageNameIDResponse, Saml2Constants.Namespaces.Protocol);

            this.WriteCommonAttributes(writer, message);

            if (message.InResponseTo != null) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.InResponseTo, message.InResponseTo.Value);
            }

            this.WriteCommonElements(writer, message);
            this.WriteStatus(writer, message.Status);

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:NameIDMappingRequest element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2NameIdMappingRequest</c> to serialize.</param>
        protected virtual void WriteNameIDMappingRequest(XmlWriter writer, Saml2NameIdMappingRequest message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer = this.CreateEnvelopedSignatureWriter(writer, message);
            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.NameIDMappingRequest, Saml2Constants.Namespaces.Protocol);

            this.WriteCommonAttributes(writer, message);
            this.WriteCommonElements(writer, message);

            this.WriteNameId(writer, message.Identifier);

            this.WriteNameIDPolicy(writer, message.NameIdentifierPolicy);

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:NameIDMappingResponse element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2NameIdMappingResponse</c> to serialize.</param>
        protected virtual void WriteNameIDMappingResponse(XmlWriter writer, Saml2NameIdMappingResponse message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer = this.CreateEnvelopedSignatureWriter(writer, message);
            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.NameIDMappingResponse, Saml2Constants.Namespaces.Protocol);

            this.WriteCommonAttributes(writer, message);

            if (message.InResponseTo != null) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.InResponseTo, message.InResponseTo.Value);
            }

            this.WriteCommonElements(writer, message);

            this.WriteStatus(writer, message.Status);

            this.WriteNameId(writer, message.NameId);

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:NewID or samlp:NewEncryptedID element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>NewNameIdentifier</c> to serialize.</param>
        protected virtual void WriteNewID(XmlWriter writer, Saml2NewNameIdentifier message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            if (message.EncryptingCredentials == null) {
                writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.NewID, Saml2Constants.Namespaces.Protocol);
                writer.WriteString(message.Value);
                writer.WriteEndElement();
            }
            else {
#if AZUREAD
                throw new NotImplementedException();
#else
                SecurityTokenHandlerCollection handlers = new SecurityTokenHandlerCollection(new SecurityTokenHandler[] { new GenericXmlSecurityTokenHandler(), new EncryptedSecurityTokenHandler() { KeyInfoSerializer = this.KeyInfoSerializer } });

                var newNameidentifier = new Saml2NewNameIdentifier(message.Value);

                var doc = new XmlDocument() { XmlResolver = null };
                using (var writer2 = doc.CreateNavigator().AppendChild()) {
                    this.WriteNewID(writer2, newNameidentifier);
                }

                var token = new GenericXmlSecurityToken(doc.DocumentElement, null, DateTime.MinValue, DateTime.MaxValue, null, null, new ReadOnlyCollection<IAuthorizationPolicy>(new IAuthorizationPolicy[0]));
                EncryptedSecurityToken encryptedToken = new EncryptedSecurityToken(token, message.EncryptingCredentials);
                writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.NewEncryptedID, Saml2Constants.Namespaces.Protocol);
                handlers.WriteToken(writer, encryptedToken);
                writer.WriteEndElement();
#endif
            }
        }

        /// <summary>
        /// Serializes a samlp:Status element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2Status</c> object to be serialized.</param>
        protected virtual void WriteStatus(XmlWriter writer, Saml2Status message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.Status, Saml2Constants.Namespaces.Protocol);
            this.WriteStatusCode(writer, message.StatusCode);

            if (!string.IsNullOrEmpty(message.StatusMessage)) {
                writer.WriteElementString(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.StatusMessage, Saml2Constants.Namespaces.Protocol, message.StatusMessage);
            }

            if (message.StatusDetail.Count > 0) {
                writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.StatusDetail, Saml2Constants.Namespaces.Protocol);

                foreach (XmlElement element in message.StatusDetail) {
                    element.WriteTo(writer);
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:StatusCode element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2StatusCode</c> object to be serialized.</param>
        protected virtual void WriteStatusCode(XmlWriter writer, Saml2StatusCode message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            string value;
            if (message.Value.Namespace == Saml2Constants.Namespaces.Protocol) {
                value = XmlQualifiedName.ToString(message.Value.Name, Saml2Constants.Prefixes.Protocol);
            }
            else {
                value = message.Value.ToString();
            }

            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.StatusCode, Saml2Constants.Namespaces.Protocol);
            writer.WriteAttributeString(Saml2Constants.AttributeNames.Value, value);
            if (message.SubStatus != null) {
                this.WriteStatusCode(writer, message.SubStatus);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:NameIDPolicy element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2NameIdentifierPolicy</c> to serialize.</param>
        protected virtual void WriteNameIDPolicy(XmlWriter writer, Saml2NameIdentifierPolicy message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.NameIDPolicy, Saml2Constants.Namespaces.Protocol);

            if (message.Format != null) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.Format, message.Format.OriginalString);
            }

            if (!string.IsNullOrEmpty(message.SPNameQualifier)) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.SPNameQualifier, message.SPNameQualifier);
            }

            if (message.AllowCreate) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.AllowCreate, XmlConvert.ToString(message.AllowCreate));
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:RequestedAuthnContext element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2RequestedAuthenticationContext</c> to serialize.</param>
        protected virtual void WriteRequestedAuthnContext(XmlWriter writer, Saml2RequestedAuthenticationContext message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            if (message.References.Count == 0) {
                throw DiagnosticTools.ExceptionUtil.ThrowHelperInvalidOperation(SR.ID2020);
            }

            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.RequestedAuthnContext, Saml2Constants.Namespaces.Protocol);
            string comparison;
            switch (message.Comparison) {
                case Saml2AuthenticationContextComparisonType.Maximum:
                    comparison = "maximum";
                    break;
                case Saml2AuthenticationContextComparisonType.Minimum:
                    comparison = "minimum";
                    break;
                case Saml2AuthenticationContextComparisonType.Better:
                    comparison = "better";
                    break;
                case Saml2AuthenticationContextComparisonType.Exact:
                    comparison = "exact";
                    break;
                default:
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperError(new ArgumentOutOfRangeException("message", string.Format("The RequestedAuthenticationContext cannot be serialized because the Comparison property is set to a value that is not valid: {0}", message.Comparison)));
            }

            writer.WriteAttributeString(Saml2Constants.AttributeNames.Comparison, comparison);

            string refElementName = null;
            if (message.ReferenceType == Saml2AuthenticationContextReferenceType.Class) {
                refElementName = Saml2Constants.ElementNames.AuthnContextClassRef;
            }
            else if (message.ReferenceType == Saml2AuthenticationContextReferenceType.Declaration) {
                refElementName = Saml2Constants.ElementNames.AuthnContextDeclRef;
            }
            else {
                throw DiagnosticTools.ExceptionUtil.ThrowHelperError(new ArgumentOutOfRangeException("message", SR.ID2019Format(message.ReferenceType)));
            }

            foreach (Uri uri in message.References) {
                writer.WriteElementString(Saml2Constants.Prefixes.Protocol, refElementName, Saml2Constants.Namespaces.Assertion, uri.OriginalString);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:Scoping element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2Scoping</c> object to be serialized.</param>
        protected virtual void WriteScoping(XmlWriter writer, Saml2Scoping message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.Scoping, Saml2Constants.Namespaces.Protocol);
            if (message.ProxyCount.HasValue) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.ProxyCount, XmlConvert.ToString(message.ProxyCount.Value));
            }

            this.WriteIDPList(writer, message.IdentityProviderList);

            foreach (Uri uri in message.RequesterIds) {
                writer.WriteElementString(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.RequesterID, Saml2Constants.Namespaces.Protocol, uri.OriginalString);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:IDPList element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2IdentityProviderList</c> to be serialized.</param>
        protected virtual void WriteIDPList(XmlWriter writer, Saml2IdentityProviderList message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            if (message.Count > 0) {
                writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.IDPList, Saml2Constants.Namespaces.Protocol);
                foreach (Saml2IdentityProviderEntry entry in message) {
                    this.WriteIDPEntry(writer, entry);
                }

                if (message.GetComplete != null) {
                    writer.WriteElementString(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.GetComplete, Saml2Constants.Namespaces.Protocol, message.GetComplete.OriginalString);
                }

                writer.WriteEndElement();
            }
            else if (message.GetComplete != null) {
                throw DiagnosticTools.ExceptionUtil.ThrowHelperInvalidOperation(SR.ID2018);
            }
        }

        /// <summary>
        /// Serializes a samlp:IDPEntry element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2IdentityProviderEntry</c> to be serialized.</param>
        protected virtual void WriteIDPEntry(XmlWriter writer, Saml2IdentityProviderEntry message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer.WriteStartElement(Saml2Constants.Prefixes.Protocol, Saml2Constants.ElementNames.IDPEntry, Saml2Constants.Namespaces.Protocol);
            writer.WriteAttributeString(Saml2Constants.AttributeNames.ProviderID, message.ProviderId.OriginalString);

            if (!string.IsNullOrEmpty(message.Name)) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.Name, message.Name);
            }

            if (message.Location != null) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.Loc, message.Location.OriginalString);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a samlp:Extensions element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2Message</c> whose extensions should be serialized.</param>
        /// <remarks>Implementers need to serialize the opening and closings Extensions tag in the SAML protocol namespace.</remarks>
        protected virtual void WriteExtensions(XmlWriter writer, Saml2Message message) {
            var authenticationRequest = message as Saml2AuthenticationRequest;
            if (authenticationRequest != null && authenticationRequest.RequestedAttributes.Count > 0) {
                writer.WriteStartElement(Saml2Constants.ElementNames.Extensions, Saml2Constants.Namespaces.Protocol);

                this.WriteRequestedAttributes(writer, authenticationRequest.RequestedAttributes);

                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Writes the req-attr:RequestedAttributes element.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> with which to write the data.</param>
        /// <param name="message">The <see cref="ICollection&lt;RequestedAttribute&gt;"/> to write.</param>
        protected virtual void WriteRequestedAttributes(XmlWriter writer, ICollection<RequestedAttribute> message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            // Requested Attributes
            if (message.Count > 0) {
                writer.WriteStartElement(Saml2Constants.Prefixes.ReqAttr, Saml2Constants.ElementNames.RequestedAttributes, Saml2Constants.Namespaces.ReqAttr);
                writer.WriteAttributeString("xmlns", MetadataConstants.Prefixes.Metadata, null, MetadataConstants.Namespaces.Metadata);

                foreach (var item in message) {
                    this.WriteRequestedAttribute(writer, item);
                }

                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Writes the md:RequestedAttribute element.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> with which to write the data.</param>
        /// <param name="message">The <see cref="RequestedAttribute"/> to write.</param>
        protected void WriteRequestedAttribute(XmlWriter writer, RequestedAttribute message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer.WriteStartElement(MetadataConstants.Prefixes.Metadata, MetadataConstants.ElementNames.RequestedAttribute, MetadataConstants.Namespaces.Metadata);

            // Name
            writer.WriteAttributeString(Saml2Constants.AttributeNames.Name, message.Name);

            // NameFormat
            writer.WriteAttributeString(Saml2Constants.AttributeNames.NameFormat, message.NameFormat.AbsoluteUri);

            // Frendly Name
            if (message.FriendlyName != null) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.FriendlyName, message.FriendlyName);
            }

            // IsRequried
            if (message.IsRequired.HasValue) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.IsRequired, XmlConvert.ToString(message.IsRequired.Value));
            }

            // Values
            foreach (string item in message.Values) {
                writer.WriteStartElement(Saml2Constants.Prefixes.Assertion, Saml2Constants.ElementNames.AttributeValue, Saml2Constants.Namespaces.Assertion);
                writer.WriteString(item);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

#endregion

#region Write Common
        
        /// <summary>
        /// Writes attributes common to all SAML messages.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2Message</c> to be serialized.</param>
        protected virtual void WriteCommonAttributes(XmlWriter writer, Saml2Message message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            writer.WriteAttributeString(Saml2Constants.AttributeNames.ID, message.Id.Value);
            writer.WriteAttributeString(Saml2Constants.AttributeNames.Version, message.Version);
            writer.WriteAttributeString(Saml2Constants.AttributeNames.IssueInstant, XmlConvert.ToString(message.IssueInstant, DateTimeFormats.Generated));

            if (message.Destination != null) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.Destination, message.Destination.OriginalString);
            }

            if (message.Consent != null) {
                writer.WriteAttributeString(Saml2Constants.AttributeNames.Consent, message.Consent.OriginalString);
            }
        }

        /// <summary>
        /// Writes elements common to all SAML messages.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="message">The <c>Saml2Message</c> being serialized.</param>
        protected virtual void WriteCommonElements(XmlWriter writer, Saml2Message message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            if (message.Issuer != null) {
                this.WriteIssuer(writer, message.Issuer);
            }

            EnvelopedSignatureWriter envelopedWriter = writer as EnvelopedSignatureWriter;
            if (envelopedWriter != null) {
                envelopedWriter.WriteSignature();
            }

            this.WriteExtensions(writer, message);
        }

        /// <summary>
        /// Serializes a saml:Subject element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="subject">The <c>Saml2Subject</c> to serialize.</param>
        protected virtual void WriteSubject(XmlWriter writer, Saml2Subject subject) {
            this.serializerAdaptor.WriteSubjectToWriter(writer, subject);
        }

        /// <summary>
        /// Serializes a saml:Conditions element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="conditions">The <c>Saml2Conditions</c> to serialize.</param>
        protected virtual void WriteConditions(XmlWriter writer, Saml2Conditions conditions) {
            this.serializerAdaptor.WriteConditionsToWriter(writer, conditions);
        }

        /// <summary>
        /// Serializes a saml:Assertion or saml:EncryptedAssertion element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="assertion">The <c>Saml2Assertion</c> to serialize.</param>
        protected virtual void WriteAssertion(XmlWriter writer, Saml2Assertion assertion) {
            this.serializerAdaptor.WriteAssertionToWriter(writer, assertion);
        }

        /// <summary>
        /// Serializes a saml:Action element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="action">The <c>Saml2Action</c> to serialize.</param>
        protected virtual void WriteAction(XmlWriter writer, Saml2Action action) {
            this.serializerAdaptor.WriteActionToWriter(writer, action);
        }

        /// <summary>
        /// Serializes a saml:Evidence element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="evidence">The <c>Saml2Evidence</c> to serialize.</param>
        protected virtual void WriteEvidence(XmlWriter writer, Saml2Evidence evidence) {
            this.serializerAdaptor.WriteEvidenceToWriter(writer, evidence);
        }

        /// <summary>
        /// Serializes a saml:Attribute element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="attribute">The <c>Saml2Attribute</c> to serialize.</param>
        protected virtual void WriteAttribute(XmlWriter writer, Saml2Attribute attribute) {
            this.serializerAdaptor.WriteAttributeToWriter(writer, attribute);
        }

        /// <summary>
        /// Serializes a saml:Issuer element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="issuer">The <c>Saml2Issuer</c> to serialize.</param>
        protected virtual void WriteIssuer(XmlWriter writer, Saml2NameIdentifier issuer) {
            this.serializerAdaptor.WriteIssuerToWriter(writer, issuer);
        }

        /// <summary>
        /// Serializes a saml:NameID element.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to serialize to.</param>
        /// <param name="nameId">The <c>Saml2NameIdentifier</c> to serialize.</param>
        protected virtual void WriteNameId(XmlWriter writer, Saml2NameIdentifier nameId) {
            this.serializerAdaptor.WriteNameIdToWriter(writer, nameId);
        }

#endregion

#region Common methods

        /// <summary>
        /// Creates an <c>EnvelopedSignatureReader</c> to validate the signature of a signed XML element.
        /// </summary>
        /// <param name="reader">The non-validating <c>XmlReader</c> to be wrapped.</param>
        /// <param name="message">The SAML message that is being deserialized.</param>
        /// <returns>A new reader that can recognize and validate the signature.</returns>
        protected virtual XmlReader CreateEnvelopedSignatureReader(XmlReader reader, Saml2Message message) {
#if AZUREAD
            return new EnvelopedSignatureReader(reader);
#else
            return new EnvelopedSignatureReader(reader, new WrappedSecurityTokenSerializer(this.keyInfoSerializer, message), this.signatureTokenResolver, false, false, false);
#endif
        }

        /// <summary>
        /// Creates an <c>EnvelopedSignatureWriter</c> if the message contains <c>SigningCredentials</c> and proper Id.
        /// </summary>
        /// <param name="writer">The <c>XmlWriter</c> to be wrapped.</param>
        /// <param name="message">The SAML message that is being serialized.</param>
        /// <returns>A new <c>XmlWriter</c> that should be used to produce signed message.</returns>
        protected virtual XmlWriter CreateEnvelopedSignatureWriter(XmlWriter writer, Saml2Message message) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            if (message.SigningCredentials != null) {
#if AZUREAD
                return new EnvelopedSignatureWriter(writer, message.SigningCredentials, message.Id.Value);
#else
                return new EnvelopedSignatureWriter(writer, message.SigningCredentials, message.Id.Value, this.keyInfoSerializer);
#endif
            }

            return writer;
        }

        /// <summary>
        /// Sets <c>SigningCredentials</c> property if deserialized message was signed.
        /// </summary>
        /// <param name="reader">The <c>XmlReader</c> that the signed element is being read from.</param>
        /// <param name="message">The <c>Saml2Message</c> object to update with the <c>SigningCredentials</c>.</param>
        protected virtual void SetSigningCredentials(XmlReader reader, Saml2Message message) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            EnvelopedSignatureReader envelopedReader = reader as EnvelopedSignatureReader;
            Debug.Assert(envelopedReader != null, "XmlReader is not EnvelopedSignatureReader when reading the signature");
#if AZUREAD
            message.SigningCredentials = SamlUtil.ResolveSigningCredentials(envelopedReader.Signature, this.validationParameters);
#else
            message.SigningCredentials = envelopedReader.SigningCredentials;
#endif
        }
#endregion

        private static string ReadSimpleStringElement(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                if (!reader.IsStartElement()) {
                    reader.ReadStartElement();
                }

                if (reader.IsEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                XmlUtil.ValidateXsiType(reader, "string", System.Xml.Schema.XmlSchema.Namespace);

                reader.ReadStartElement();
                string value = reader.ReadContentAsString();
                reader.ReadEndElement();

                return value;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        private static Uri ReadSimpleUriElement(XmlReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            try {
                if (!reader.IsStartElement()) {
                    reader.ReadStartElement();
                }

                if (reader.IsEmptyElement) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2005Format(reader.LocalName, reader.NamespaceURI));
                }

                XmlUtil.ValidateXsiType(reader, "anyURI", System.Xml.Schema.XmlSchema.Namespace);
                reader.ReadStartElement();

                Uri uri = new Uri(reader.ReadContentAsString());

                reader.ReadEndElement();

                return uri;
            }
            catch (Exception exception) {
                Exception wrapedException = TryWrapReadException(reader, exception);
                if (wrapedException != null) {
                    throw wrapedException;
                }

                throw;
            }
        }

        private static Exception TryWrapReadException(XmlReader reader, Exception innerException) {
            if (innerException is FormatException
                || innerException is ArgumentException
                || innerException is InvalidOperationException
                || innerException is OverflowException) {
                return DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2001, innerException);
            }

            return null;
        }

#if !AZUREAD
        /// <summary>
        /// This class is used to decrypt NewEncryptedIDs.
        /// </summary>
        private class GenericXmlSecurityTokenHandler : SecurityTokenHandler {
            private static readonly string[] Identifiers = new string[] { typeof(GenericXmlSecurityToken).ToString() };

            public override Type TokenType {
                get { return typeof(GenericXmlSecurityToken); }
            }

            public override bool CanWriteToken {
                get { return true; }
            }

            public override bool CanReadToken(XmlReader reader) {
                return true;
            }

            public override string[] GetTokenTypeIdentifiers() {
                return Identifiers;
            }

            public override SecurityToken ReadToken(XmlReader reader) {
                reader.MoveToContent();

                string xml = reader.ReadOuterXml();
                XmlDocument document = new XmlDocument() { XmlResolver = null };
                using (var reader0 = XmlReader.Create(new System.IO.StringReader(xml), new XmlReaderSettings() { XmlResolver = null })) {
                    document.Load(reader);
                }

                return new GenericXmlSecurityToken(document.DocumentElement, null, DateTime.MinValue, DateTime.MaxValue, null, null, new ReadOnlyCollection<IAuthorizationPolicy>(new IAuthorizationPolicy[0]));
            }

            public override void WriteToken(XmlWriter writer, SecurityToken token) {
                GenericXmlSecurityToken token2 = token as GenericXmlSecurityToken;
                using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(token2.TokenXml.OuterXml))) {
                    writer.WriteNode(reader, false);
                }
            }
        }

        /// <summary>
        /// This class used to read SecurityTokenSerializer.
        /// </summary>
        private class WrappedSecurityTokenSerializer : SecurityTokenSerializer {
            private readonly Saml2Message message;
            private readonly SecurityTokenSerializer wrappedSerializer;

            public WrappedSecurityTokenSerializer(SecurityTokenSerializer wrappedSerializer, Saml2Message message) {
                this.message = message;
                this.wrappedSerializer = wrappedSerializer ?? throw new ArgumentNullException(nameof(wrappedSerializer));
            }

            protected override bool CanReadKeyIdentifierClauseCore(XmlReader reader) {
                return this.wrappedSerializer.CanReadKeyIdentifierClause(reader);
            }

            protected override bool CanReadKeyIdentifierCore(XmlReader reader) {
                return true;
            }

            protected override bool CanReadTokenCore(XmlReader reader) {
                return this.wrappedSerializer.CanReadToken(reader);
            }

            protected override bool CanWriteKeyIdentifierClauseCore(SecurityKeyIdentifierClause keyIdentifierClause) {
                return false;
            }

            protected override bool CanWriteKeyIdentifierCore(SecurityKeyIdentifier keyIdentifier) {
                return false;
            }

            protected override bool CanWriteTokenCore(SecurityToken token) {
                return false;
            }

            protected override SecurityKeyIdentifierClause ReadKeyIdentifierClauseCore(XmlReader reader) {
                return this.wrappedSerializer.ReadKeyIdentifierClause(reader);
            }

            protected override SecurityKeyIdentifier ReadKeyIdentifierCore(XmlReader reader) {
                if (this.wrappedSerializer.CanReadKeyIdentifier(reader)) {
                    return this.wrappedSerializer.ReadKeyIdentifier(reader);
                }

                if (this.message.Issuer == null) {
                    DiagnosticTools.LogUtil.Write("Reading a SAML2.0 message with no KeyInfo clause and no Issuer attribute - signature validation will likely fail.", "Diagnostic", 1, 0, System.Diagnostics.TraceEventType.Warning);
                }

                return new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[] { new Saml2MessageSecurityKeyIdentifierClause(this.message) });
            }

            protected override SecurityToken ReadTokenCore(XmlReader reader, SecurityTokenResolver tokenResolver) {
                return this.wrappedSerializer.ReadToken(reader, tokenResolver);
            }

            protected override void WriteKeyIdentifierClauseCore(XmlWriter writer, SecurityKeyIdentifierClause keyIdentifierClause) {
                throw new NotSupportedException();
            }

            protected override void WriteKeyIdentifierCore(XmlWriter writer, SecurityKeyIdentifier keyIdentifier) {
                throw new NotSupportedException();
            }

            protected override void WriteTokenCore(XmlWriter writer, SecurityToken token) {
                throw new NotSupportedException();
            }
        }
#endif
    }
}
