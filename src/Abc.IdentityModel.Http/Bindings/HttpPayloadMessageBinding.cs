// ----------------------------------------------------------------------------
// <copyright file="HttpPayloadMessageBinding.cs" company="ABC Software Ltd">
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

namespace Abc.IdentityModel.Http.Bindings {
    using System;

    /// <summary>
    /// A binding that serialize/deserialize message payload.
    /// </summary>
    /// <typeparam name="T">The message payload type.</typeparam>
    public abstract class HttpPayloadMessageBinding<T> : IHttpBinding {
        /// <inheritdoc/>
        public virtual void ProcessIncomingMessage(IHttpProtocolMessage message) {
            var payloadMessage = message as IHttpPayloadMessage<T>;
            if (payloadMessage != null) {
                payloadMessage.Payload = this.DeserializeMessage(payloadMessage.Data);
            }
        }

        /// <inheritdoc/>
        public virtual void ProcessOutgoingMessage(IHttpProtocolMessage message) {
            var payloadMessage = message as IHttpPayloadMessage<T>;
            if (payloadMessage != null && payloadMessage.Payload != null) { // TODO: payload == null
                payloadMessage.Data = this.SerializeMessage(payloadMessage.Payload);
            }
        }

        /// <summary>
        /// Deserializes the message.
        /// </summary>
        /// <param name="data">The message data.</param>
        /// <returns>The message payload.</returns>
        protected abstract T DeserializeMessage(string data);

        /// <summary>
        /// Serializes the message.
        /// </summary>
        /// <param name="payloadMessage">The message payload.</param>
        /// <returns>The message data</returns>
        protected abstract string SerializeMessage(T payloadMessage);
    }
}
