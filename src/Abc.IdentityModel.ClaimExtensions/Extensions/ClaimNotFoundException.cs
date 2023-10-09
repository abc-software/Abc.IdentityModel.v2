﻿// ----------------------------------------------------------------------------
// <copyright file="ClaimNotFoundException.cs" company="ABC Software Ltd">
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

namespace Abc.IdentityModel.Extensions {
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception for failed claims search operations
    /// </summary>
    /// <remarks>
    /// The idea of Dominick Baier.
    /// </remarks>
    [Serializable]
    public class ClaimNotFoundException : Exception {
        // For guidelines regarding the creation of new exception types, see
        // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimNotFoundException"/> class.
        /// </summary>
        public ClaimNotFoundException() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimNotFoundException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public ClaimNotFoundException(string message) 
            : base(message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimNotFoundException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="inner">
        /// The inner.
        /// </param>
        public ClaimNotFoundException(string message, Exception inner) 
            : base(message, inner) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimNotFoundException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected ClaimNotFoundException(SerializationInfo info,  StreamingContext context)
            : base(info, context) {
        }

        #endregion
    }
}