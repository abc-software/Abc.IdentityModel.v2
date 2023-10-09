// ----------------------------------------------------------------------------
// <copyright file="JwtSecurityTokenHandler.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml;

namespace Abc.IdentityModel.Tokens.Jwt {
    /// <summary>
    /// A <see cref="JwtSecurityTokenHandler" /> designed for creating and validating Json Web Tokens.
    /// See: https://datatracker.ietf.org/doc/html/rfc7519 and http://www.rfc-editor.org/info/rfc7515
    /// </summary>
    public class JwtSecurityTokenHandler : System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler {
        /// <inheritdoc/>
        public override void WriteToken(XmlWriter writer, SecurityToken token) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (token is null) {
                throw new ArgumentNullException(nameof(token));
            }

            if (!(token is JwtSecurityToken)) {
                throw new ArgumentException($"Jwt10200: '{this.GetType()}' can only write SecurityTokens of type: '{typeof(JwtSecurityToken)}', 'token' type is: '{token.GetType()}'.");
            }

            byte[] bytes = Encoding.UTF8.GetBytes(this.WriteToken(token));
            writer.WriteStartElement(WSSecurityConstants.Prefixes.Wsse, WSSecurityConstants.ElementNames.BinarySecurityToken, WSSecurityConstants.Namespaces.Wsse);
            if (token.Id != null) {
                writer.WriteAttributeString(WSSecurityConstants.Prefixes.Wsu, WSSecurityConstants.AttributeNames.Id, WSSecurityConstants.Namespaces.Wsu, token.Id);
            }

            writer.WriteAttributeString(WSSecurityConstants.AttributeNames.ValueType, null, WSSecurityConstants.TokenTypes.JwtAlt);
            writer.WriteAttributeString(WSSecurityConstants.AttributeNames.EncodingType, null, WSSecurityConstants.EncodingTypes.Base64Binary);
            writer.WriteBase64(bytes, 0, bytes.Length);
            writer.WriteEndElement();
        }

        /// <inheritdoc/>
        public override bool CanReadToken(XmlReader reader) {
            if (reader is null) {
                return false;
            }

            reader.MoveToContent();
            if (reader.IsStartElement(WSSecurityConstants.ElementNames.BinarySecurityToken, WSSecurityConstants.Namespaces.Wsse)) {
                string encodingType = reader.GetAttribute(WSSecurityConstants.AttributeNames.EncodingType);
                if (!string.Equals(encodingType, WSSecurityConstants.EncodingTypes.Base64Binary, StringComparison.Ordinal)) {
                    return false;
                }

                string valueType = reader.GetAttribute(WSSecurityConstants.AttributeNames.ValueType);
                if (string.Equals(valueType, WSSecurityConstants.TokenTypes.JwtAlt, StringComparison.Ordinal)
                    || string.Equals(valueType, WSSecurityConstants.TokenTypes.Jwt, StringComparison.OrdinalIgnoreCase)) {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public override SecurityToken ReadToken(XmlReader reader) {
            if (reader is null) {
                throw new ArgumentNullException(nameof(reader));
            }

            if (!this.CanReadToken(reader)) {
                throw new ArgumentException($"Jwt10203: '{this.GetType()}' cannot read this xml: '{reader.ReadOuterXml()}'. The reader needs to be positioned at an element: '{WSSecurityConstants.ElementNames.BinarySecurityToken}', within the namespace: '{WSSecurityConstants.Namespaces.Wsse}', with an attribute: '{WSSecurityConstants.AttributeNames.ValueType}' equal to one of the following: '{"urn:ietf:params:oauth:token-type:jwt"}', '{"JWT"}'.");
            }

            var xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(reader);
            //string id = xmlDictionaryReader.GetAttribute(WSSecurityConstants.AttributeNames.Id, WSSecurityConstants.Namespaces.Wsu);
            var jwtSecurityToken = this.ReadToken(Encoding.UTF8.GetString(xmlDictionaryReader.ReadElementContentAsBase64())) as JwtSecurityToken;
            //if (id != null && ) {
            //    jwtSecurityToken?.SetId(id);
            //}

            return jwtSecurityToken;
        }

        /// <inheritdoc/>
        public override SecurityToken ReadToken(XmlReader reader, TokenValidationParameters validationParameters) {
            this.ValidateToken(reader, validationParameters, out var securityToken);
            return securityToken;
        }

        /// <inheritdoc/>
        public override ClaimsPrincipal ValidateToken(XmlReader reader, TokenValidationParameters validationParameters, out SecurityToken validatedToken) {
            if (reader is null) {
                throw new ArgumentNullException(nameof(reader));
            }

            if (validationParameters is null) {
                throw new ArgumentNullException(nameof(validationParameters));
            }

            if (!this.CanReadToken(reader)) {
                throw new ArgumentException($"Jwt10203: '{this.GetType()}' cannot read this xml: '{reader.ReadOuterXml()}'. The reader needs to be positioned at an element: '{WSSecurityConstants.ElementNames.BinarySecurityToken}', within the namespace: '{WSSecurityConstants.Namespaces.Wsse}', with an attribute: '{WSSecurityConstants.AttributeNames.ValueType}' equal to one of the following: '{"urn:ietf:params:oauth:token-type:jwt"}', '{"JWT"}'.");
            }

            using var xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(reader);
            string @string = Encoding.UTF8.GetString(xmlDictionaryReader.ReadElementContentAsBase64());
            return this.ValidateToken(@string, validationParameters, out validatedToken);
        }

        /// <inheritdoc/>
        public override SecurityKeyIdentifierClause CreateSecurityTokenReference(SecurityToken token, bool attached) {
            return null;
        }
    }
}