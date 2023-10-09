namespace Abc.IdentityModel.Protocols.Saml2 {
    using System;
    using System.Diagnostics;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using Abc.IdentityModel.Http;
    using Abc.IdentityModel.Security;
#if WIF35
    using System.IdentityModel.Tokens;
    using Microsoft.IdentityModel.SecurityTokenService;
#elif AZUREAD
    using Microsoft.IdentityModel.Tokens;
#else
    using System.IdentityModel.Tokens;
#endif
#if NET40 || NET35
    using Diagnostic;
#else
    using Abc.Diagnostics;
#endif

    internal class HttpSaml2SignatureBinding : IHttpBinding {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSaml2SignatureBinding"/> class.
        /// </summary>
        public HttpSaml2SignatureBinding() {
        }

        /// <summary>
        /// Gets or sets the delegate that will initialize the non-serialized properties necessary on a signed
        /// message so that its signature can be correctly calculated for verification.
        /// </summary>
        public Action<IHttpSaml2TamperResistanMessage> SignatureCallback { get; set; }

        /// <inheritdoc/>
        public void ProcessIncomingMessage(IHttpProtocolMessage message) {
            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            if ((message.HttpMethods & HttpDeliveryMethods.GetRequest) != HttpDeliveryMethods.GetRequest) {
                return;
            }

            var tamperResistanceMessage = message as IHttpSaml2TamperResistanMessage;
            if (tamperResistanceMessage == null || string.IsNullOrEmpty(tamperResistanceMessage.SignatureAlgorithm) || tamperResistanceMessage.Signature == null) {
                return;
            }

            if (this.SignatureCallback != null) {
                this.SignatureCallback(tamperResistanceMessage);
            }

            // Calculate message hash
            var hashAlgorithm = CryptoUtil.GetHashAlgorithm(tamperResistanceMessage.SignatureAlgorithm);
            byte[] rgbHash = hashAlgorithm.ComputeHash(NormalizeQueryString(tamperResistanceMessage));
            tamperResistanceMessage.SignatureObject = rgbHash;
        }

        /// <inheritdoc/>
        public void ProcessOutgoingMessage(IHttpProtocolMessage message) {
            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            if ((message.HttpMethods & HttpDeliveryMethods.GetRequest) != HttpDeliveryMethods.GetRequest) {
                return;
            }

            var tamperResistanceMessage = message as IHttpSaml2TamperResistanMessage;
            if (tamperResistanceMessage == null) {
                return;
            }

            if (this.SignatureCallback != null) {
                this.SignatureCallback(tamperResistanceMessage);
            }

            if (tamperResistanceMessage.SignatureObject == null) {
                return; 
            }

            Debug.Assert(tamperResistanceMessage.SignatureObject is SigningCredentials);
            var signingCredetials = tamperResistanceMessage.SignatureObject as SigningCredentials;

#if AZUREAD
            tamperResistanceMessage.SignatureAlgorithm = signingCredetials.Algorithm;
#else
            tamperResistanceMessage.SignatureAlgorithm = signingCredetials.SignatureAlgorithm;
#endif
            tamperResistanceMessage.Signature = this.GetSignature(tamperResistanceMessage, signingCredetials);
        }

        /// <summary>
        /// Calculates a signature for a given message.
        /// </summary>
        /// <param name="message">The message to sign.</param>
        /// <param name="signingCredentials">The signing credentials.</param>
        /// <returns>
        /// The signature for the message.
        /// </returns>
        protected virtual byte[] GetSignature(IHttpSaml2TamperResistanMessage message, SigningCredentials signingCredentials) {
            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            if (signingCredentials == null) {
                throw new ArgumentNullException(nameof(signingCredentials));
            }

#if AZUREAD
            var signatureAlgorithm = signingCredentials.Algorithm;
            var signingKey = signingCredentials.Key as X509SecurityKey;
#else
            var signatureAlgorithm = signingCredentials.SignatureAlgorithm;
            var signingKey = signingCredentials.SigningKey as AsymmetricSecurityKey;
#endif
            Debug.Assert(!string.IsNullOrEmpty(signatureAlgorithm));

            if (signingKey == null) {
                throw DiagnosticTools.ExceptionUtil.ThrowHelperInvalidOperation("Signature generation failed. Unsupported signature credentials.");
            }

#if AZUREAD
            if (signingKey.PrivateKeyStatus != PrivateKeyStatus.Exists) {
#else
            if (!signingKey.HasPrivateKey()) {
#endif
                throw DiagnosticTools.ExceptionUtil.ThrowHelperInvalidOperation("Signature generation failed. Signing certificate does not have private key.");
            }

            // Calculate message hash
            var hashAlgorithm = CryptoUtil.GetHashAlgorithm(signatureAlgorithm);
            byte[] rgbHash = hashAlgorithm.ComputeHash(NormalizeQueryString(message));

#if AZUREAD
            
            var formatter = CryptoUtil.GetSignatureFormatter(signingKey.PrivateKey, signatureAlgorithm);
#else
            var formatter = signingKey.GetSignatureFormatter(signatureAlgorithm);
            formatter.SetHashAlgorithm(CryptoUtil.MapAlgorithmToOidName(signatureAlgorithm));
#endif
            return formatter.CreateSignature(rgbHash);
        }

        /// <summary>
        /// Normalize the query string.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// The query string.
        /// </returns>
        /// <exception cref="Abc.IdentityModel.Http.HttpMessageException">Cannot create a SAML binding message from the given HttpRequest. Check if the request contains a valid Uri or Form Post that contains protocol parameters for SAML HTTP bindings.</exception>
        /// <exception cref="HttpMessageException">Cannot create a SAML binding message from the given HttpRequest
        /// or
        /// Not supported signatureAlgorithm</exception>
        private static byte[] NormalizeQueryString(IHttpSaml2TamperResistanMessage message) {
            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            var saml2Message = message as HttpSaml2Message2;
            if (saml2Message == null) {
                throw new ArgumentException($"Invalid argument type. Expected '{nameof(HttpSaml2Message2)}'", nameof(message));
            }

            var builder = new StringBuilder();

            // Url Encoding Case Insensitive, cant use HttpUtility.UrlEncode to validate signature
            if (saml2Message.ExtraData.ContainsKey("Query")) {
                var query = saml2Message.ExtraData["Query"];
                var strArray = query.TrimStart(new char[] { '?' }).Split(new char[] { '&' });

                string request = null;
                string relayState = null;
                string signAlg = null;

                for (int i = 0; i < strArray.Length; i++) {
                    int len = strArray[i].Length;
                    int index = strArray[i].IndexOf('=');
                    if (index > 0 && index < len) {
                        string key = strArray[i].Substring(0, index);
                        string value = strArray[i].Substring(index + 1, (len - index) - 1);

                        switch (key) {
                            case Saml2.Saml2Constants.Parameters.SamlRequest:
                            case Saml2.Saml2Constants.Parameters.SamlResponse:
                                request = string.Concat(key, '=', value);
                                break;
                            case Saml2.Saml2Constants.Parameters.RelayState:
                                relayState = string.Concat(key, '=', value);
                                break;
                            case Saml2.Saml2Constants.Parameters.SignatureAlgorithm:
                                signAlg = string.Concat(key, '=', value);
                                break;
                        }
                    }
                }

                // Build from Query
                builder.Append(request);
                if (relayState != null) {
                    builder.Append('&');
                    builder.Append(relayState);
                }

                builder.Append('&');
                builder.Append(signAlg);

                saml2Message.ExtraData.Remove("Query");
            }
            else {
                // Request/Response
                if (message is HttpSaml2RequestMessage2) {
                    builder.Append(Saml2.Saml2Constants.Parameters.SamlRequest);
                }
                else if (message is HttpSaml2ResponseMessage2) {
                    builder.Append(Saml2.Saml2Constants.Parameters.SamlResponse);
                }
                else {
                    throw new HttpMessageException("Cannot create a SAML binding message from the given HttpRequest. Check if the request contains a valid Uri or Form Post that contains protocol parameters for SAML HTTP bindings.");
                }

                builder.Append('=');
                builder.Append(HttpUtil.UrlEncode(message.Data));

                // Relay State
                var relayState = (message as HttpSaml2Message2).RelayState;
                if (!string.IsNullOrEmpty(relayState)) {
                    builder.AppendFormat("&{0}={1}", Saml2.Saml2Constants.Parameters.RelayState, HttpUtil.UrlEncode(relayState));
                }

                // Signature Algorithm
                var signatureAlgorithm = message.SignatureAlgorithm;
                builder.AppendFormat("&{0}={1}", Saml2.Saml2Constants.Parameters.SignatureAlgorithm, HttpUtil.UrlEncode(signatureAlgorithm));
            }

            return Encoding.UTF8.GetBytes(builder.ToString());
        }
    }
}