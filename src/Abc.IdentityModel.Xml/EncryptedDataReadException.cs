namespace Abc.IdentityModel.Xml {
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is thrown when reading a <see cref="EncryptedData"/>.
    /// </summary>
    [Serializable]
    public class EncryptedDataReadException : Exception {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedDataReadException"/> class.
        /// </summary>
        public EncryptedDataReadException() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedDataReadException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        public EncryptedDataReadException(string message) : base(message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedDataReadException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public EncryptedDataReadException(string message, Exception innerException) : base(message, innerException) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedDataReadException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected EncryptedDataReadException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}