// ----------------------------------------------------------------------------
// <copyright file="EidasSerializationException.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols.EidasLight {
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is thrown when reading or writing a <see cref="EidasLightMessage"/>.
    /// </summary>
    [Serializable]
    public class EidasSerializationException : Exception {
        /// <summary>
        /// Initializes a new instance of the <see cref="EidasSerializationException"/> class.
        /// </summary>
        public EidasSerializationException() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EidasSerializationException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        public EidasSerializationException(string message)
            : base(message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EidasSerializationException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public EidasSerializationException(string message, Exception innerException)
            : base(message, innerException) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EidasSerializationException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected EidasSerializationException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
    }
}