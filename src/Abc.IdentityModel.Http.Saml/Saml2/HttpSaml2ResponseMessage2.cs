namespace Abc.IdentityModel.Protocols.Saml2 {
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using Abc.IdentityModel.Http;
    using Abc.IdentityModel.Http.Converters;

    public class HttpSaml2ResponseMessage2 : HttpSaml2Message2, IHttpSaml2TamperResistanMessage, IHttpPayloadMessage<Saml2Message>, IHttpSaml2EncodedMessage {
        private string response;
        private Saml2StatusResponse samlResponse;

        public HttpSaml2ResponseMessage2(Uri baseUrl, string response, HttpDeliveryMethods allowedMethods = HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.PostRequest)
            : base(baseUrl, allowedMethods) {
            if (string.IsNullOrEmpty(response)) {
                throw new ArgumentNullException(nameof(response));
            }

            this.response = response;
            if ((allowedMethods & HttpDeliveryMethods.VerbMask) == HttpDeliveryMethods.None) {
                throw new ArgumentOutOfRangeException(nameof(allowedMethods));
            }
        }

        internal HttpSaml2ResponseMessage2(Uri baseUrl, Saml2StatusResponse samlResponse, HttpDeliveryMethods allowedMethods = HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.PostRequest)
            : base(baseUrl, allowedMethods) {
            this.samlResponse = samlResponse ?? throw new ArgumentNullException(nameof(samlResponse));
            if ((allowedMethods & HttpDeliveryMethods.VerbMask) == HttpDeliveryMethods.None) {
                throw new ArgumentOutOfRangeException(nameof(allowedMethods));
            }
        }

        internal HttpSaml2ResponseMessage2(Uri baseUrl, HttpDeliveryMethods allowedMethods)
            : base(baseUrl, allowedMethods) {
            if ((allowedMethods & HttpDeliveryMethods.VerbMask) == HttpDeliveryMethods.None) {
                throw new ArgumentOutOfRangeException(nameof(allowedMethods));
            }
        }

        internal Saml2StatusResponse Saml2Response {
            get { return this.samlResponse; }
        }

        [MessagePart(Saml2Constants.Parameters.SamlResponse, IsRequired = true)]
        private string Response {
            get {
                return this.response;
            }

            set {
                if (string.IsNullOrEmpty(value)) {
                    throw new ArgumentNullException(nameof(value));
                }

                this.response = value;
            }
        }

        string IHttpSaml2TamperResistanMessage.Data {
            get { return this.Response; }
            set { this.Response = value; }
        }

        [MessagePart(Saml2Constants.Parameters.Signature, AllowEmpty = false)]
        [TypeConverter(typeof(Base64Converter))]
        byte[] IHttpSaml2TamperResistanMessage.Signature { get; set; }

        [MessagePart(Saml2Constants.Parameters.SignatureAlgorithm, AllowEmpty = false)]
        string IHttpSaml2TamperResistanMessage.SignatureAlgorithm { get; set; }

        object IHttpSaml2TamperResistanMessage.SignatureObject { get; set; }

        string IHttpEncodedMessage.Data {
            get { return this.Response; }
            set { this.Response = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        string IHttpEncodedMessage.EncodingAlgorithm {
            get { return this.Encoding; }
        }

        bool IHttpSaml2EncodedMessage.Decoded { get; set; }

        Saml2Message IHttpPayloadMessage<Saml2Message>.Payload {
            get {
                return this.samlResponse;
            }

            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                Debug.Assert(value is Saml2StatusResponse);
                this.samlResponse = (Saml2StatusResponse)value;
            }
        }

        string IHttpPayloadMessage<Saml2Message>.Data {
            get { return this.Response; }
            set { this.Response = value ?? throw new ArgumentNullException(nameof(value)); }
        }
    }
}
