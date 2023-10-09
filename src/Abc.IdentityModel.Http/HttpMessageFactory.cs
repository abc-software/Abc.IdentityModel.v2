// ----------------------------------------------------------------------------
// <copyright file="HttpMessageFactory.cs" company="ABC Software Ltd">
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
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// A message factory that automatically selects the message type based on the incoming data.
    /// </summary>
    public class HttpMessageFactory : IHttpMessageFactory {
        private readonly Dictionary<HttpMessageDescription, ConstructorInfo> MessageTypes = new Dictionary<HttpMessageDescription, ConstructorInfo>();

        /// <summary>
        /// Analyzes an incoming request message payload to discover what kind of
        /// message is embedded in it and returns the type, or null if no match is found.
        /// </summary>
        /// <param name="baseUrl">The intended or actual recipient of the request message.</param>
        /// <param name="method">The HTTP method(s) allowed.</param>
        /// <param name="fields">The name/value pairs that make up the message payload.</param>
        /// <returns>
        /// A newly instantiated <see cref="T:IHttpProtocolMessage" />-derived object that this message can
        /// deserialize to.  Null if the request isn't recognized as a valid protocol message.
        /// </returns>
        public IHttpMessage CreateMessage(Uri baseUrl, HttpDeliveryMethods method, IDictionary<string, string> fields) {
            if (baseUrl == null) {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            if (fields == null) {
                throw new ArgumentNullException(nameof(fields));
            }

            if (!baseUrl.IsAbsoluteUri) {
                throw new ArgumentException("Must be absolute Uri.", nameof(baseUrl));
            }

            HttpMessageDescription description = GetMessageDescription(fields);
            if (description != null) {
                ConstructorInfo info = this.MessageTypes[description];
                var message = (IHttpMessage)info.Invoke(new object[] { baseUrl, method });

                foreach (KeyValuePair<string, string> pair in fields) {
                    HttpMessagePart part;
                    if (description.Mapping.TryGetValue(pair.Key, out part)) {
                        part.SetValue(message, pair.Value);
                    }
                    else {
                        message.ExtraData.Add(pair);
                    }
                }

                message.Validate();
                return message;
            }

            return null;
        }

        internal HttpMessageDescription GetMessageDescription(Type messageType) {
            if (messageType == null) {
                throw new ArgumentNullException(nameof(messageType));
            }

            return this.MessageTypes.Keys.FirstOrDefault(x => x.MessageType == messageType);
        }

        internal virtual void AddMessageType(Type messageType) {
            if (messageType == null) {
                throw new ArgumentNullException(nameof(messageType));
            }

            AddMessageType(this.MessageTypes, messageType);
        }

        private HttpMessageDescription GetMessageDescription(IDictionary<string, string> fields) {
            if (fields == null) {
                throw new ArgumentNullException(nameof(fields));
            }

            var source = (from message in this.MessageTypes.Keys
                          where message.CheckMessagePartsPassBasicValidation(fields)
                          orderby CountInCommon(message.Mapping.Keys, fields.Keys, StringComparison.Ordinal) descending
                          select message).ThenByDescending<HttpMessageDescription, int>(message => message.Mapping.Count).ToList();

            ////var source = (from message in this.requestMessageTypes.Keys
            ////              where message.CheckMessagePartsPassBasicValidation(fields)
            ////              select message).ToList();

            var description = source.FirstOrDefault();
            if (description == null) {
                return null;
            }

            ////if (source.Count() > 1) {
            ////    //Logger.Messaging.WarnFormat("Multiple message types seemed to fit the incoming data: {0}", source.ToStringDeferred<MessageDescription>());
            ////    throw new InvalidOperationException("Multiple message types seemed to fit the incoming data");
            ////}

            return description;
        }

        /// <summary>
        /// Counts how many strings are in the intersection of two collections.
        /// </summary>
        /// <param name="collection1">The first collection.</param>
        /// <param name="collection2">The second collection.</param>
        /// <param name="comparison">The string comparison method to use.</param>
        /// <returns>A non-negative integer no greater than the count of elements in the smallest collection.</returns>
        private static int CountInCommon(ICollection<string> collection1, ICollection<string> collection2, StringComparison comparison = StringComparison.Ordinal) {
            if (collection1 == null) {
                throw new ArgumentNullException(nameof(collection1));
            }

            if (collection2 == null) {
                throw new ArgumentNullException(nameof(collection2));
            }

            return collection1.Count<string>(value1 => collection2.Any<string>(value2 => string.Equals(value1, value2, comparison)));
        }

        private static void AddMessageType(IDictionary<HttpMessageDescription, ConstructorInfo> messageTypes, Type messageType) {
            if (messageTypes == null) {
                throw new ArgumentNullException(nameof(messageTypes));
            }

            if (messageType == null) {
                throw new ArgumentNullException(nameof(messageType));
            }

            var description = new HttpMessageDescription(messageType);

            bool flag = false;
            if (typeof(IHttpMessage).IsAssignableFrom(description.MessageType)) {
                foreach (ConstructorInfo info in description.Constructors) {
                    ParameterInfo[] parameters = info.GetParameters();
                    if (parameters.Length == 2 && parameters[0].ParameterType == typeof(Uri) && parameters[1].ParameterType == typeof(HttpDeliveryMethods)) {
                        flag = true;
                        messageTypes.Add(description, info);
                        break;
                    }
                }
            }

            if (!flag) {
                throw new NotSupportedException();
            }
        }
    }
}
