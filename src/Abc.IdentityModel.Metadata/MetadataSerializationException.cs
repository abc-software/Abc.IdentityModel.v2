// ----------------------------------------------------------------------------
// <copyright file="MetadataSerializationException.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when an error occurs while serializing or deserializing metadata.
    /// </summary>
    [Serializable]
    public class MetadataSerializationException : Exception {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataSerializationException" /> class.
        /// </summary>
        public MetadataSerializationException()
            : this("An exception occurred while serializing or deserializing metadata.") {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataSerializationException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public MetadataSerializationException(string message)
            : base(message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataSerializationException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The <see cref="T:System.Exception" /> that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
        public MetadataSerializationException(string message, Exception innerException)
            : base(message, innerException) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataSerializationException" /> class with serialized data.
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo" /> object that holds the serialized object data.</param>
        /// <param name="context">A <see cref="StreamingContext" /> object that contains the contextual information about the source or destination.</param>
        protected MetadataSerializationException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
    }
}