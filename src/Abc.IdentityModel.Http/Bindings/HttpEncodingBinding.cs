namespace Abc.IdentityModel.Http.Bindings {
    using System;

    /// <summary>
    /// A binding that encode/decode message.
    /// </summary>
    public abstract class HttpEncodingBinding : IHttpBinding {
        /// <inheritdoc/>
        public virtual void ProcessIncomingMessage(IHttpProtocolMessage message) {
            var encodedMessage = message as IHttpEncodedMessage;
            if (encodedMessage != null && encodedMessage.Data != null) {
                encodedMessage.Data = DecodeMessage(encodedMessage.Data, encodedMessage.EncodingAlgorithm, message.HttpMethods);
            }
        }

        /// <inheritdoc/>
        public virtual void ProcessOutgoingMessage(IHttpProtocolMessage message) {
            var decodedMessage = message as IHttpEncodedMessage;
            if (decodedMessage != null && decodedMessage.Data != null) {
                decodedMessage.Data = EncodeMessage(decodedMessage.Data, decodedMessage.EncodingAlgorithm, message.HttpMethods);
            }
        }

        /// <summary>
        /// Encodes the message.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="encodingAlgorithm">The encoding algorithm.</param>
        /// <param name="methods">The HTTP method(s).</param>
        /// <returns>
        /// The encoded data.
        /// </returns>
        protected virtual string EncodeMessage(string data, string encodingAlgorithm, HttpDeliveryMethods methods) {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }

            return data;
        }

        /// <summary>
        /// Decodes the message.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="encodingAlgorithm">The encoding algorithm.</param>
        /// <param name="methods">The HTTP method(s).</param>
        /// <returns>
        /// The decoded data.
        /// </returns>
        protected virtual string DecodeMessage(string data, string encodingAlgorithm, HttpDeliveryMethods methods) {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }

            return data;
        }
    }
}
