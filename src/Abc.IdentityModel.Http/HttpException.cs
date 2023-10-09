#if NETCOREAPP1_0_OR_GREATER

namespace Abc.IdentityModel.Http {
    using System;
#if !NETSTANDARD1_6
    using System.Runtime.Serialization;
#endif

    /// <summary>
    /// Describes an exception that occurred during the processing of HTTP requests.
    /// </summary>
    /// <seealso cref="System.Exception" />
#if !NETSTANDARD1_6
    [Serializable]
#endif
    public class HttpException : Exception {
        private readonly int httpStatusCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class and creates an empty HttpException object.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP response status code sent to the client corresponding to this error.</param>
        public HttpException(int httpStatusCode) {
            this.httpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class using an error message and the InnerException property.
        /// </summary>
        /// <param name="message">The error message displayed to the client when the exception is thrown. </param>
        /// <param name="inner">The InnerException, if any, that threw the current exception. </param>
        public HttpException(string message, Exception inner)
            : base(message, inner) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException" /> class using an HTTP response status code and an error message.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP response status code sent to the client corresponding to this error.</param>
        /// <param name="message">The error message displayed to the client when the exception is thrown. </param>
        public HttpException(int httpStatusCode, string message)
            : base(message) {
            this.httpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class using an HTTP response status code, an error message, and the InnerException property.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP response status code sent to the client corresponding to this error.</param>
        /// <param name="message">The error message displayed to the client when the exception is thrown. </param>
        /// <param name="inner">The InnerException, if any, that threw the current exception. </param>
        public HttpException(int httpStatusCode, string message, Exception inner)
            : base(message, inner) {
            this.httpStatusCode = httpStatusCode;
        }

#if !NETSTANDARD1_6
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"></see> class, using the specified serialization information and streaming context.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object for the exception.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> object for the exception.</param>
        protected HttpException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
#endif

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public int StatusCode {
            get { return this.httpStatusCode; }
        }
    }
}

#endif