// ----------------------------------------------------------------------------
// <copyright file="IHttpMessage.cs" company="ABC Software Ltd">
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
    using System.Collections.Generic;

    /// <summary>
    /// The interface that classes must implement to be serialized/deserialized
    /// as protocol or extension messages.
    /// </summary>
    public interface IHttpMessage {
        /// <summary>
        /// Gets the extra, non-standard Protocol parameters included in the message.
        /// </summary>
        /// <remarks>
        /// Implementations of this interface should ensure that this property never returns null.
        /// </remarks>
        IDictionary<string, string> ExtraData { get; }

        /// <summary>
        /// Checks the message state for conformity to the protocol specification
        /// and throws an exception if the message is invalid.
        /// </summary>
        /// <exception cref="T:HttpMessageException">Thrown if the message is invalid.</exception>
        void Validate();
    }
}
