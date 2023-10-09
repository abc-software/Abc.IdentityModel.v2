// ----------------------------------------------------------------------------
// <copyright file="HttpMessageException.cs" company="ABC Software Ltd">
//    Copyright © 2010-2019 ABC Software Ltd. All rights reserved.
//
//    This library is free software; you can redistribute it and/or.
//    modify it under the terms of the GNU Lesser General Public
//    License  as published by the Free Software Foundation, either
//    version 3 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with the library. If not, see http://www.gnu.org/licenses/.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Http {
    using System;
#if !NETSTANDARD1_6
    using System.Runtime.Serialization;
    using System.Web;
#endif

    /// <summary>
    /// The exception that is thrown when an error occurs while processing a HTTP message.
    /// </summary>
#if !NETSTANDARD1_6
    [Serializable]
#endif
    public class HttpMessageException : HttpException {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessageException"/> class.
        /// </summary>
        public HttpMessageException()
            : this("HTTP Message Exception") {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessageException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public HttpMessageException(string message)
            : base(400, message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessageException"></see> class, using a specified error message and an underlying exception object.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="inner">An <see cref="T:System.Exception"></see> object that contains underlying exception information.</param>
        public HttpMessageException(string message, Exception inner)
            : base(message, inner) {
        }

#if !NETSTANDARD1_6
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessageException"></see> class, using the specified serialization information and streaming context.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object for the exception.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> object for the exception.</param>
        protected HttpMessageException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
#endif
    }
}
