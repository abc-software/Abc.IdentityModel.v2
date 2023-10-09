namespace Abc.IdentityModel.Protocols.Saml11 {
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when an error occurs while serializing or deserializing a SAML1.1 message. 
    /// </summary>
    [Serializable]
    internal class SamlSerializationException : Exception {
        /// <summary>
        /// Initializes a new instance of the <see cref="SamlSerializationException"/> class.
        /// </summary>
        public SamlSerializationException() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamlSerializationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SamlSerializationException(string message)
            : base(message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamlSerializationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public SamlSerializationException(string message, Exception inner)
            : base(message, inner) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamlSerializationException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected SamlSerializationException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
    }
}
