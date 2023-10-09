namespace Abc.IdentityModel.Protocols.Saml2 {
    using System;
    using Abc.IdentityModel.Http;
#if NETCOREAPP1_0_OR_GREATER
    using HttpRequestBase = Microsoft.AspNetCore.Http.HttpRequest;
#endif
#if NETFRAMEWORK
    using System.IdentityModel.Selectors;
    using System.Web;
#endif
#if AZUREAD
    using Microsoft.IdentityModel.Tokens;
#endif
#if WIF35
    using Microsoft.IdentityModel.Tokens;
#endif

    /// <summary>
    /// The messaging serializer used by SAML2.0 passive profile.
    /// </summary>
    public class HttpSaml2MessageSerializer : HttpMessageSerializer {
        private static readonly Type[] MessageTypes = new Type[] { typeof(HttpSaml2RequestMessage2), typeof(HttpSaml2ArtifactMessage2), typeof(HttpSaml2ResponseMessage2) };

#if AZUREAD
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSaml2MessageSerializer"/> class.
        /// </summary>
        /// <param name="signatureTokenResolver">The signature token resolver.</param>
        public HttpSaml2MessageSerializer(TokenValidationParameters validationParameters)
            : this(new Saml2ProtocolSerializer(validationParameters), validationParameters) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSaml2MessageSerializer" /> class.
        /// </summary>
        /// <param name="protocolSerializer">The protocol serializer.</param>
        /// <param name="signatureTokenResolver">The signature token resolver.</param>
        internal HttpSaml2MessageSerializer(Saml2ProtocolSerializer protocolSerializer, TokenValidationParameters validationParameters)
            : base(MessageTypes, new HttpSaml2PayloadMessageBinding(protocolSerializer, validationParameters), new HttpSaml2EncodingBinding(), new HttpSaml2SignatureBinding()) {
        }
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSaml2MessageSerializer"/> class.
        /// </summary>
        /// <param name="signatureTokenResolver">The signature token resolver.</param>
        public HttpSaml2MessageSerializer(SecurityTokenResolver signatureTokenResolver)
            : this(new Saml2ProtocolSerializer(signatureTokenResolver), signatureTokenResolver) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSaml2MessageSerializer" /> class.
        /// </summary>
        /// <param name="protocolSerializer">The protocol serializer.</param>
        /// <param name="signatureTokenResolver">The signature token resolver.</param>
        internal HttpSaml2MessageSerializer(Saml2ProtocolSerializer protocolSerializer, SecurityTokenResolver signatureTokenResolver)
            : base(MessageTypes, new HttpSaml2PayloadMessageBinding(protocolSerializer, signatureTokenResolver), new HttpSaml2EncodingBinding(), new HttpSaml2SignatureBinding()) {
        }
#endif

        protected override IHttpProtocolMessage ReadMessageCore(HttpRequestBase httpRequest) {
            var message = base.ReadMessageCore(httpRequest);
            if (message != null) {
                // Save Original Query to validate signature
                if ((message.HttpMethods & HttpDeliveryMethods.GetRequest) == HttpDeliveryMethods.GetRequest) {
#if AZUREAD
                    message.ExtraData.Add("Query", httpRequest.QueryString.Value);
#else
                    message.ExtraData.Add("Query", httpRequest.Url.Query);
#endif
                }

                base.ProcessIncomingMessage(message);
            }

            return message;
        }

        protected override void ProcessIncomingMessage(IHttpProtocolMessage message) {
            // Do nothing
        }
    }
}
