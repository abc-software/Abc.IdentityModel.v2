namespace Abc.IdentityModel.Protocols.Saml2 {
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using Abc.IdentityModel.Http;
    using Abc.IdentityModel.Http.Converters;

    public class HttpSaml2RequestMessage2 : HttpSaml2Message2, IHttpSaml2TamperResistanMessage, IHttpPayloadMessage<Saml2Message>, IHttpSaml2EncodedMessage {
        private string request;
        private Saml2Request samlRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSaml2RequestMessage2"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="request">The request.</param>
        /// <param name="allowedMethods">The allowed methods.</param>
        public HttpSaml2RequestMessage2(Uri baseUrl, string request, HttpDeliveryMethods allowedMethods = HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.PostRequest)
            : base(baseUrl, allowedMethods) {
            if (string.IsNullOrEmpty(request)) {
                throw new ArgumentNullException(nameof(request));
            }

            this.request = request;

            if ((allowedMethods & HttpDeliveryMethods.VerbMask) == HttpDeliveryMethods.None) {
                throw new ArgumentOutOfRangeException(nameof(allowedMethods));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSaml2RequestMessage2"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="samlRequest">The SAML2 request.</param>
        /// <param name="allowedMethods">The allowed methods.</param>
        internal HttpSaml2RequestMessage2(Uri baseUrl, Saml2Request samlRequest, HttpDeliveryMethods allowedMethods = HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.PostRequest)
            : base(baseUrl, allowedMethods) {
            this.samlRequest = samlRequest ?? throw new ArgumentNullException(nameof(samlRequest));

            if ((allowedMethods & HttpDeliveryMethods.VerbMask) == HttpDeliveryMethods.None) {
                throw new ArgumentOutOfRangeException(nameof(allowedMethods));
            }
        }

        internal HttpSaml2RequestMessage2(Uri baseUrl, HttpDeliveryMethods allowedMethods)
            : base(baseUrl, allowedMethods) {
            if ((allowedMethods & HttpDeliveryMethods.VerbMask) == HttpDeliveryMethods.None) {
                throw new ArgumentOutOfRangeException(nameof(allowedMethods));
            }
        }

        /// <summary>
        /// Gets the SAML2 request.
        /// </summary>
        internal Saml2Request Saml2Request {
            get { return this.samlRequest; }
        }

        [MessagePart(Saml2Constants.Parameters.SamlRequest, IsRequired = true)]
        string IHttpSaml2TamperResistanMessage.Data {
            get {
                return this.request;
            }

            set {
                if (string.IsNullOrEmpty(value)) {
                    throw new ArgumentNullException(nameof(value));
                }

                this.request = value;
            }
        }

        [MessagePart(Saml2Constants.Parameters.Signature, AllowEmpty = false)]
        [TypeConverter(typeof(Base64Converter))]
        byte[] IHttpSaml2TamperResistanMessage.Signature { get; set; }

        [MessagePart(Saml2Constants.Parameters.SignatureAlgorithm, AllowEmpty = false)]
        string IHttpSaml2TamperResistanMessage.SignatureAlgorithm { get; set; }

        object IHttpSaml2TamperResistanMessage.SignatureObject { get; set; }

        string IHttpEncodedMessage.Data {
            get { return this.request; }
            set { this.request = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        string IHttpEncodedMessage.EncodingAlgorithm {
            get { return this.Encoding; }
        }

        bool IHttpSaml2EncodedMessage.Decoded { get; set; }

        Saml2Message IHttpPayloadMessage<Saml2Message>.Payload {
            get {
                return this.samlRequest;
            }

            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                Debug.Assert(value is Saml2Request);
                this.samlRequest = (Saml2Request)value;
            }
        }

        string IHttpPayloadMessage<Saml2Message>.Data {
            get { return this.request; }
            set { this.request = value ?? throw new ArgumentNullException(nameof(value)); }
        }
    }
}
