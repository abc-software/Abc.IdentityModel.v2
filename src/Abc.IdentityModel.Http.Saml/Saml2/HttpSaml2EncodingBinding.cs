namespace Abc.IdentityModel.Protocols.Saml2 {
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using Abc.IdentityModel.Http;
    using Abc.IdentityModel.Http.Bindings;
#if WIF35
    using Microsoft.IdentityModel.Web;
#elif AZUREAD
#else
    using System.IdentityModel;
#endif

    /// <summary>
    /// The SAML2 message encode/decode binding.
    /// </summary>
    internal class HttpSaml2EncodingBinding : HttpEncodingBinding {
        /// <inheritdoc/>
        public override void ProcessIncomingMessage(IHttpProtocolMessage message) {
            var encodedMessage = message as IHttpSaml2EncodedMessage;
            if (encodedMessage != null && encodedMessage.Data != null && !encodedMessage.Decoded) {
                encodedMessage.Data = DecodeMessage(encodedMessage.Data, encodedMessage.EncodingAlgorithm, message.HttpMethods);
                encodedMessage.Decoded = true;
            }
        }

        /// <inheritdoc/>
        public override void ProcessOutgoingMessage(IHttpProtocolMessage message) {
            var encodedMessage = message as IHttpSaml2EncodedMessage;
            if (encodedMessage != null && encodedMessage.Data != null && encodedMessage.Decoded) {
                encodedMessage.Data = EncodeMessage(encodedMessage.Data, encodedMessage.EncodingAlgorithm, message.HttpMethods);
                encodedMessage.Decoded = false;
            }
        }

        /// <inheritdoc/>
        protected override string EncodeMessage(string data, string encodingAlgorithm, HttpDeliveryMethods methods) {
            if ((methods & HttpDeliveryMethods.GetRequest) == HttpDeliveryMethods.GetRequest) {
                if (string.IsNullOrEmpty(encodingAlgorithm) || string.Equals(encodingAlgorithm, Saml2Constants.EncodingAlgorithms.DeflateString)) {
                    return DeflateEncode(data);
                }
                else { 
                    throw new NotSupportedException();
                }
            }
            else if ((methods & HttpDeliveryMethods.PostRequest) == HttpDeliveryMethods.PostRequest) {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
            }
            else {
                throw new NotSupportedException();
            }
        }

        /// <inheritdoc/>
        protected override string DecodeMessage(string data, string encodingAlgorithm, HttpDeliveryMethods methods) {
            if ((methods & HttpDeliveryMethods.GetRequest) == HttpDeliveryMethods.GetRequest) {
                if (string.IsNullOrEmpty(encodingAlgorithm) || string.Equals(encodingAlgorithm, Saml2Constants.EncodingAlgorithms.DeflateString)) {
                    return DeflateDecode(data);
                }
                else {
                    throw new NotSupportedException();
                }
            }
            else if ((methods & HttpDeliveryMethods.PostRequest) == HttpDeliveryMethods.PostRequest) {
                return Encoding.UTF8.GetString(Convert.FromBase64String(data));
            }
            else {
                throw new NotSupportedException();
            }
        }

        private static string DeflateEncode(string data) {
            var value = Encoding.UTF8.GetBytes(data);

            using (var compressedStream = new MemoryStream()) {
                using (var deflateStream = new DeflateStream(compressedStream, CompressionMode.Compress, true)) {
                    deflateStream.Write(value, 0, value.Length);
                }

                return Convert.ToBase64String(compressedStream.ToArray());
            }
        }

        private static string DeflateDecode(string data) {
            using (MemoryStream decompressedStream = new MemoryStream()) {
                using (MemoryStream compressStream = new MemoryStream(Convert.FromBase64String(data)))
                using (DeflateStream deflateStream = new DeflateStream(compressStream, CompressionMode.Decompress)) {
                    deflateStream.CopyTo(decompressedStream);
                }

                return Encoding.UTF8.GetString(decompressedStream.ToArray());
            }
        }
    }
}
