// ----------------------------------------------------------------------------
// <copyright file="WsSecuritySecurityKeyIdentifierClause.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

using Microsoft.IdentityModel.Tokens;
using System;

namespace Abc.IdentityModel.Tokens {
    /// <summary>
    /// Represents a <see langword="&lt;KeyIndentifier&gt;" /> element that references a <see langword="&lt;Assertion&gt;" /> element in a SOAP message.
    /// </summary>
    public class WsSecuritySecurityKeyIdentifierClause : SecurityKeyIdentifierClause {
        /// <summary>
        /// Initializes a new instance of the <see cref="WsSecuritySecurityKeyIdentifierClause" /> class using the specified SAML assertion identifier, value type and token type.
        /// </summary>
        /// <param name="id">The identifier of the SAML assertion that contains the key identifier.</param>
        /// <param name="valueType">The value type of the SAML assertion.</param>
        /// <param name="tokenType">The token type of the SAML assertion.</param>
        /// <exception cref="ArgumentException">if <paramref name="id"/> or <paramref name="tokenType"/> or <paramref name="valueType"/> is null or empty.</exception>
        public WsSecuritySecurityKeyIdentifierClause(string id, string valueType, string tokenType) {
            if (string.IsNullOrEmpty(id)) {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));
            }

            if (string.IsNullOrEmpty(valueType)) {
                throw new ArgumentException($"'{nameof(valueType)}' cannot be null or empty.", nameof(valueType));
            }

            if (string.IsNullOrEmpty(tokenType)) {
                throw new ArgumentException($"'{nameof(tokenType)}' cannot be null or empty.", nameof(tokenType));
            }

            this.Id = id;
            this.ValueType = valueType;
            this.TokenType = tokenType;
        }

        /// <summary>
        /// Gets the identifier for the SAML assertion that contains the key identifier.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the value type for the SAML assertion that contains the key identifier.
        /// </summary>
        public string ValueType { get; }

        /// <summary>
        /// Gets the token type for the SAML assertion that contains the key identifier.
        /// </summary>
        public string TokenType { get; }
    }
}