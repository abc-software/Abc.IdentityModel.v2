// ----------------------------------------------------------------------------
// <copyright file="HttpMessageDescription.cs" company="ABC Software Ltd">
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
    using System.Runtime.Serialization;

    /// <summary>
    /// A mapping between serialized key names and <see cref="HttpMessagePart"/> instances describing
    /// those key/values pairs.
    /// </summary>
    internal class HttpMessageDescription {
        private readonly Dictionary<string, HttpMessagePart> mapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessageDescription"/> class.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        internal HttpMessageDescription(Type messageType) {
            this.mapping = new Dictionary<string, HttpMessagePart>();
            this.MessageType = messageType ?? throw new ArgumentNullException(nameof(messageType));
            this.ReflectMessageType();
        }

        /// <summary>
        /// Gets the mapping between the serialized key names and their describing <see cref="HttpMessagePart"/> instances.
        /// </summary>
        internal IDictionary<string, HttpMessagePart> Mapping {
            get { return this.mapping; }
        }

        /// <summary>
        /// Gets the type of message this instance was generated from.
        /// </summary>
        /// <value>The type of the described message.</value>
        internal Type MessageType { get; private set; }

        /// <summary>
        /// Gets the constructors available on the message type.
        /// </summary>
        internal ConstructorInfo[] Constructors { get; private set; }

        /// <summary>
        /// Ensures the message parts pass basic validation.
        /// </summary>
        /// <param name="parts">The key/value pairs of the serialized message.</param>
        internal void EnsureMessagePartsPassBasicValidation(IDictionary<string, string> parts) {
            if (parts == null) {
                throw new ArgumentNullException(nameof(parts));
            }

            this.CheckRequiredMessagePartsArePresent(parts.Keys, true);
            this.CheckRequiredProtocolMessagePartsAreNotEmpty(parts, true);
            this.CheckMessagePartsConstantValues(parts, true);
        }

        /// <summary>
        /// Tests whether all the required message parts pass basic validation for the given data.
        /// </summary>
        /// <param name="parts">The key/value pairs of the serialized message.</param>
        /// <returns>A value indicating whether the provided data fits the message's basic requirements.</returns>
        internal bool CheckMessagePartsPassBasicValidation(IDictionary<string, string> parts) {
            if (parts == null) {
                throw new ArgumentNullException(nameof(parts));
            }

            return this.CheckRequiredMessagePartsArePresent(parts.Keys, false) && this.CheckRequiredProtocolMessagePartsAreNotEmpty(parts, false) && this.CheckMessagePartsConstantValues(parts, false);
        }

        /// <summary>
        /// Verifies that a given set of keys include all the required parameters
        /// for this message type or throws an exception.
        /// </summary>
        /// <param name="keys">The names of all parameters included in a message.</param>
        /// <param name="throwOnFailure">if set to <c>true</c> an exception is thrown on failure with details.</param>
        /// <returns>A value indicating whether the provided data fits the message's basic requirements.</returns>
        /// <exception cref="HttpMessageException">
        /// Thrown when required parts of a message are not in <paramref name="keys"/>
        /// if <paramref name="throwOnFailure"/> is <c>true</c>.
        /// </exception>
        private bool CheckRequiredMessagePartsArePresent(IEnumerable<string> keys, bool throwOnFailure) {
            if (keys == null) {
                throw new ArgumentNullException(nameof(keys));
            }

            var missingKeys = (from part in this.Mapping.Values
                               where part.IsRequired && !keys.Contains(part.Name)
                               select part.Name).ToArray();
            if (missingKeys.Length > 0) {
                if (throwOnFailure) {
                    // RequiredParametersMissin
                    throw new HttpMessageException(string.Format("The following required parameters were missing from the {0} message: {1}", this.MessageType.FullName, string.Join(", ", missingKeys)));
                }
                else {
                    // Logger.Messaging.DebugFormat(MessagingStrings.RequiredParametersMissing, this.MessageType.FullName, missingKeys.ToStringDeferred());
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Ensures the protocol message parts that must not be empty are in fact not empty.
        /// </summary>
        /// <param name="partValues">A dictionary of key/value pairs that make up the serialized message.</param>
        /// <param name="throwOnFailure">if set to <c>true</c> an exception is thrown on failure with details.</param>
        /// <returns>A value indicating whether the provided data fits the message's basic requirements.</returns>
        /// <exception cref="HttpMessageException">
        /// Thrown when required parts of a message are not in <paramref name="partValues"/>
        /// if <paramref name="throwOnFailure"/> is <c>true</c>.
        /// </exception>
        private bool CheckRequiredProtocolMessagePartsAreNotEmpty(IDictionary<string, string> partValues, bool throwOnFailure) {
            if (partValues == null) {
                throw new ArgumentNullException(nameof(partValues));
            }

            string value;
            var emptyValuedKeys = (from part in this.Mapping.Values
                                   where !part.AllowEmpty && partValues.TryGetValue(part.Name, out value) && value != null && value.Length == 0
                                   select part.Name).ToArray();
            if (emptyValuedKeys.Length > 0) {
                if (throwOnFailure) {
                    // RequiredNonEmptyParameterWasEmpty=
                    throw new HttpMessageException(string.Format("The following required non-empty parameters were empty in the {0} message: {1}", this.MessageType.FullName, string.Join(", ", emptyValuedKeys)));
                }
                else {
                    // Logger.Messaging.DebugFormat(MessagingStrings.RequiredNonEmptyParameterWasEmpty, this.MessageType.FullName, emptyValuedKeys.ToStringDeferred());
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks that a bunch of message part values meet the constant value requirements of this message description.
        /// </summary>
        /// <param name="partValues">The part values.</param>
        /// <param name="throwOnFailure">if set to <c>true</c>, this method will throw on failure.</param>
        /// <returns>A value indicating whether all the requirements are met.</returns>
        private bool CheckMessagePartsConstantValues(IDictionary<string, string> partValues, bool throwOnFailure) {
            if (partValues == null) {
                throw new ArgumentNullException(nameof(partValues));
            }

            var badConstantValues = (from part in this.Mapping.Values
                                     where part.IsStaticConstantValue
                                     where partValues.ContainsKey(part.Name)
                                     where !string.Equals(partValues[part.Name], part.DefaultValue.ToString(), StringComparison.Ordinal)
                                     select part.Name).ToArray();
            if (badConstantValues.Length > 0) {
                if (throwOnFailure) {
                    // RequiredMessagePartConstantIncorrect=
                    throw new HttpMessageException(string.Format("The following message parts had constant value requirements that were unsatisfied: {0}", this.MessageType.FullName, string.Join(", ", badConstantValues)));
                }
                else {
                    // Logger.Messaging.DebugFormat(MessagingStrings.RequiredMessagePartConstantIncorrect, this.MessageType.FullName, badConstantValues.ToStringDeferred());
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Reflects over some <see cref="IHttpMessage"/>-implementing type
        /// and prepares to serialize/deserialize instances of that type.
        /// </summary>
        private void ReflectMessageType() {
            Type currentType = this.MessageType;
            do {
                var members = currentType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly)
                                          .Where(m => (m is PropertyInfo || m is FieldInfo) 
                                              && (Attribute.GetCustomAttribute(m, typeof(MessagePartAttribute)) is MessagePartAttribute || Attribute.GetCustomAttribute(m, typeof(DataMemberAttribute)) is DataMemberAttribute));
                foreach (MemberInfo member in members) {
                    HttpMessagePart part = new HttpMessagePart(member);
                    if (this.mapping.ContainsKey(part.Name)) {
                        // Logger.Messaging.WarnFormat("Message type {0} has more than one message part named {1}.  Inherited members will be hidden.", this.MessageType.Name, part.Name);
                    }
                    else {
                        this.mapping.Add(part.Name, part);
                    }
                }

                currentType = currentType.BaseType;
            } while (currentType != null);

            this.Constructors = Array.FindAll(
                            this.MessageType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic),
                            ctor => !ctor.IsFamily && !ctor.IsPrivate);  // Filter out protected and private constructors
        }
    }
}
