namespace Abc.IdentityModel.Protocols.Saml2 {
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;
    using Abc.IdentityModel.Http;
    using Abc.IdentityModel.Http.Bindings;
    using Abc.IdentityModel.Security;
#if AZUREAD
    using Microsoft.IdentityModel.Tokens;
#else
    using System.IdentityModel.Selectors;
    using System.IdentityModel.Tokens;
#endif

    internal class HttpSaml2PayloadMessageBinding : HttpPayloadMessageBinding<Saml2Message> {
        private readonly Saml2ProtocolSerializer protocolSerializer;
#if AZUREAD
        private readonly TokenValidationParameters tokenValidationParamters;
#else
        private readonly SecurityTokenResolver signatureTokenResolver;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSaml2PayloadMessageBinding"/> class.
        /// </summary>
        public HttpSaml2PayloadMessageBinding()
            : this(new Saml2ProtocolSerializer(), null) {
        }

#if AZUREAD
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSaml2PayloadMessageBinding" /> class.
        /// </summary>
        /// <param name="protocolSerializer">The protocol serializer.</param>
        /// <param name="signatureTokenResolver">The signature token resolver.</param>
        public HttpSaml2PayloadMessageBinding(Saml2ProtocolSerializer protocolSerializer, TokenValidationParameters tokenValidationParamters) {
            this.protocolSerializer = protocolSerializer;
            this.tokenValidationParamters = tokenValidationParamters;
        }
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSaml2PayloadMessageBinding" /> class.
        /// </summary>
        /// <param name="signatureTokenResolver">The signature token resolver.</param>
        public HttpSaml2PayloadMessageBinding(SecurityTokenResolver signatureTokenResolver)
            : this(new Saml2ProtocolSerializer(signatureTokenResolver), signatureTokenResolver) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSaml2PayloadMessageBinding" /> class.
        /// </summary>
        /// <param name="protocolSerializer">The protocol serializer.</param>
        /// <param name="signatureTokenResolver">The signature token resolver.</param>
        public HttpSaml2PayloadMessageBinding(Saml2ProtocolSerializer protocolSerializer, SecurityTokenResolver signatureTokenResolver) {
            this.signatureTokenResolver = signatureTokenResolver;
            this.protocolSerializer = protocolSerializer;
        }
#endif

        /// <summary>
        /// Gets the SAML2 protocol serializer.
        /// </summary>
        /// <value>
        /// The SAML2 protocol serializer.
        /// </value>
        internal Saml2ProtocolSerializer ProtocolSerializer {
            get { return this.protocolSerializer; }
        }

#if !AZUREAD
        /// <summary>
        /// Gets the signature token resolver.
        /// </summary>
        /// <value>
        /// The signature token resolver.
        /// </value>
        internal SecurityTokenResolver SignatureTokenResolver {
            get { return this.signatureTokenResolver; }
        }
#endif

        /// <inheritdoc/>
        public override void ProcessOutgoingMessage(IHttpProtocolMessage message) {
            var payloadMessage = message as IHttpPayloadMessage<Saml2Message>;
            if (payloadMessage != null && payloadMessage.Payload != null) { // TODO: payload == null
                var signingCredentials = payloadMessage.Payload.SigningCredentials;

                // Tamper resistant message
                var tamperResistantMessage = message as IHttpSaml2TamperResistanMessage;
                if (tamperResistantMessage != null && (message.HttpMethods & HttpDeliveryMethods.GetRequest) == HttpDeliveryMethods.GetRequest && signingCredentials != null) {
                    // Set signing credentials
                    tamperResistantMessage.SignatureObject = signingCredentials;
                    payloadMessage.Payload.SigningCredentials = null;
                }

                payloadMessage.Data = SerializeMessage(payloadMessage.Payload);

                // Set message as decoded
                var encodedMessage = message as IHttpSaml2EncodedMessage;
                if (encodedMessage != null) {
                    encodedMessage.Decoded = true;
                }
            }
        }

        /// <inheritdoc/>
        public override void ProcessIncomingMessage(IHttpProtocolMessage message) {
            var payloadMessage = message as IHttpPayloadMessage<Saml2Message>;
            if (payloadMessage != null) {
                payloadMessage.Payload = DeserializeMessage(payloadMessage.Data);

                // Tamper resistant message
                var tamperResistantMessage = message as IHttpSaml2TamperResistanMessage;
                if (tamperResistantMessage != null && tamperResistantMessage.SignatureObject != null) {
                    // validate RGB hash
                    Debug.Assert(tamperResistantMessage.SignatureObject is byte[]);
                    var rgbHash = (byte[])tamperResistantMessage.SignatureObject;
                    if (!IsQueryStringSignatureValid(tamperResistantMessage, rgbHash)) {
                        // throw new InvalidSignatureException(message); 
                        throw new HttpMessageException("Invalid signature");
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes the specified string into a <see cref="Saml2Message" />.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The <see cref="Saml2Message" /> object.
        /// </returns>
        protected override Saml2Message DeserializeMessage(string data) {
            XmlReaderSettings settings = new XmlReaderSettings() {
                DtdProcessing = DtdProcessing.Prohibit,
            };

            using (StringReader reader = new StringReader(data)) {
                using (XmlReader reader2 = XmlReader.Create(reader, settings)) {
                    try {
                        return protocolSerializer.ReadSamlMessage(reader2);
                    }
                    catch (XmlException exception) {
                        throw new HttpMessageException("The data retrieved must contain valid XML for a SAML2.0 message.", exception);
                    }

                }
            }
        }

        /// <summary>
        /// Serializes the specified <see cref="Saml2Message" /> object into a string.
        /// </summary>
        /// <param name="payloadMessage">The <see cref="Saml2Message" /> object to serialize.</param>
        /// <returns>
        /// A serialized string representation of the <see cref="Saml2Message" /> object.
        /// </returns>
        protected override string SerializeMessage(Saml2Message payloadMessage) {
            using (StringWriter writer = new StringWriter()) {
                using (XmlTextWriter textWriter = new XmlTextWriter(writer)) {
                    protocolSerializer.WriteSamlMessage(textWriter, payloadMessage);
                    textWriter.Flush();
                    return writer.ToString();
                }
            }
        }

        /// <summary>
        /// Determines whether the signature on some message is valid.
        /// </summary>
        /// <param name="message">The message to check the signature on.</param>
        /// <param name="rgbHash">The RGB hash.</param>
        /// <returns>
        ///   <c>true</c> if the signature on the message is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// Signature validation failed. No signing certificate
        /// or
        /// Signature validation failed. Public key is not RSA key.
        /// </exception>
        protected virtual bool IsQueryStringSignatureValid(IHttpSaml2TamperResistanMessage message, byte[] rgbHash) {
            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            var payloadMessage = message as IHttpPayloadMessage<Saml2Message>;
            if (payloadMessage == null) {
                throw new ArgumentException($"Invalid argument type. Expected '{nameof(IHttpPayloadMessage<Saml2Message>)}'.", nameof(message));
            }

            Debug.Assert(payloadMessage.Payload != null);

#if AZUREAD
            // Do not validate id not specified token validation parameters
            if (this.tokenValidationParamters == null) {
                return true;
            }

            var signingKey = SamlUtil.ResolveSigningKey(payloadMessage.Payload, this.tokenValidationParamters) as X509SecurityKey;
#else
            // Do not validate id not specified signature resolver
            if (this.signatureTokenResolver == null) {
                return true;
            }

            var signingKey = this.signatureTokenResolver.ResolveSecurityKey(new Saml2MessageSecurityKeyIdentifierClause(payloadMessage.Payload)) as AsymmetricSecurityKey;
#endif
            if (signingKey == null) {
                throw new InvalidOperationException("Signature validation failed. No signing certificate");
            }

            var signatureAlgorithm = message.SignatureAlgorithm;

#if AZUREAD
            var key = signingKey.Certificate.PublicKey.Key as System.Security.Cryptography.RSACryptoServiceProvider;
            if (key == null) {
                throw new InvalidOperationException("Signature validation failed. Public key is not RSA key.");
            }

            var deformatter = CryptoUtil.GetSignatureDeformatter(key, message.SignatureAlgorithm);
#else
            var deformatter = signingKey.GetSignatureDeformatter(signatureAlgorithm);
            deformatter.SetHashAlgorithm(CryptoUtil.MapAlgorithmToOidName(signatureAlgorithm));
#endif
            return deformatter.VerifySignature(rgbHash, message.Signature);
        }
    }
}
