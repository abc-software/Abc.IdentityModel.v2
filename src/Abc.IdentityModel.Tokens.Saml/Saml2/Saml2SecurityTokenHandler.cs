// ----------------------------------------------------------------------------
// <copyright file="Saml2SecurityTokenHandler.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Tokens.Saml2 {
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Tokens.Saml2;
    using System;
    using System.Security.Claims;
    using System.Text;
    using System.Xml;
    using static Microsoft.IdentityModel.Logging.LogHelper;

    public class Saml2SecurityTokenHandler : Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler {
        private const string _className = "Abc.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler";

        public Saml2SecurityTokenHandler() {
            base.Serializer = new EncryptedSaml2Serializer();
        }

        /// <summary>
        /// Gets or set the <see cref="EncryptedSaml2Serializer" /> that will be used to read and write a <see cref="Saml2SecurityToken" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">'value' is null.</exception>
        public new EncryptedSaml2Serializer Serializer {
            get {
                return base.Serializer as EncryptedSaml2Serializer;
            }
            set {
                if (!value.GetType().IsAssignableFrom(typeof(EncryptedSaml2Serializer))) {
                    throw LogExceptionMessage(new ArgumentException(nameof(value), ""));
                }

                this.Serializer = value;
            }
        }

        /// <inheritdoc/>
        public override bool CanReadToken(XmlReader reader) {
            if (reader is null) {
                return false;
            }

            return base.CanReadToken(reader)
                || reader.IsStartElement(Saml2Constants.Elements.EncryptedAssertion, Saml2Constants.Namespace);
        }

        /// <inheritdoc/>
        public override SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor, AuthenticationInformation authenticationInformation) {
            var securityToken = base.CreateToken(tokenDescriptor, authenticationInformation) as Saml2SecurityToken;

            var encryptingCredentials = tokenDescriptor.EncryptingCredentials;
            if (encryptingCredentials != null) {
                securityToken = new Saml2SecurityToken(new EncryptedSaml2Assertion(securityToken.Assertion) { EncryptingCredentials = encryptingCredentials });
            }

            return securityToken;
        }

        /// <inheritdoc/>
        public override ClaimsPrincipal ValidateToken(XmlReader reader, TokenValidationParameters validationParameters, out SecurityToken validatedToken) {
            if (reader is null) {
                throw LogArgumentNullException(nameof(reader));
            }

            if (validationParameters is null) {
                throw LogArgumentNullException(nameof(validationParameters));
            }

            var saml2SecurityToken = ReadSaml2Token(reader, validationParameters);
            if (saml2SecurityToken == null) {
                throw LogExceptionMessage(
                    new SecurityTokenValidationException(FormatInvariant(LogMessages.IDX50254, MarkAsNonPII(_className), MarkAsNonPII("ValidateToken"), MarkAsNonPII(_className), MarkAsNonPII("ReadSaml2Token"), MarkAsNonPII(typeof(Saml2Assertion)))));
            }

            var handlerType = typeof(Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler);
            var validateSignature = handlerType.GetMethod("ValidateSignature", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, Type.DefaultBinder, new Type[] { typeof(Saml2SecurityToken), typeof(string), typeof(TokenValidationParameters) }, null);
            var validateToken = handlerType.GetMethod("ValidateToken", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            validateSignature.Invoke(this, new object[] { saml2SecurityToken, saml2SecurityToken.Assertion.CanonicalString, validationParameters });
            
            var parameters = new object[] { saml2SecurityToken, saml2SecurityToken.Assertion.CanonicalString, validationParameters, null };
            var principal = (ClaimsPrincipal)validateToken.Invoke(this, parameters);
            validatedToken = (SecurityToken)parameters[3];
            return principal;
        }

        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken) {
            if (string.IsNullOrEmpty(token)) {
                throw LogArgumentNullException(nameof(token));
            }

            if (token.Length > MaximumTokenSizeInBytes) {
                throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX50209, MarkAsNonPII(token.Length), MarkAsNonPII(MaximumTokenSizeInBytes))));
            }

            if (validationParameters is null) {
                throw LogArgumentNullException(nameof(validationParameters));
            }

            using XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(token), XmlDictionaryReaderQuotas.Max);
            return ValidateToken(reader, validationParameters, out validatedToken);
        }

        public virtual Saml2SecurityToken ReadSaml2Token(XmlReader reader, TokenValidationParameters validationParameters) {
            if (reader is null) {
                throw LogArgumentNullException(nameof(reader));
            }

            if (validationParameters is null) {
                throw LogArgumentNullException(nameof(validationParameters));
            }

            var saml2Assertion = Serializer.ReadAssertion(reader, validationParameters);
            if (saml2Assertion == null) {
                throw LogExceptionMessage(
                    new Saml2SecurityTokenReadException(FormatInvariant(LogMessages.IDX50254, MarkAsNonPII(_className), MarkAsNonPII("ReadSaml2Token"), MarkAsNonPII(Serializer.GetType()), MarkAsNonPII("ReadAssertion"), MarkAsNonPII(typeof(Saml2Assertion)))));
            }

            return new Saml2SecurityToken(saml2Assertion);
        }
    }
}
