// ----------------------------------------------------------------------------
// <copyright file="SamlSecurityTokenHandler.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Tokens.Saml {
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Tokens.Saml;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Xml;

    public class SamlSecurityTokenHandler : Microsoft.IdentityModel.Tokens.Saml.SamlSecurityTokenHandler {
        internal virtual EncryptedSecurityTokenHandler EncryptedSecurityTokenHandler => new EncryptedSecurityTokenHandler(this);

        /// <inheritdoc/>
        public override SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor, AuthenticationInformation authenticationInformation) {
            var securityToken = base.CreateToken(tokenDescriptor, authenticationInformation);

            var encryptingCredentials = tokenDescriptor.EncryptingCredentials;
            if (encryptingCredentials != null) {
                securityToken = new EncryptedSecurityToken(securityToken, encryptingCredentials);
            }

            return securityToken;
        }

        /// <inheritdoc/>
        public override bool CanReadToken(XmlReader reader) {
            if (reader is null) {
                return false;
            }

            return base.CanReadToken(reader)
                || this.EncryptedSecurityTokenHandler.CanReadToken(reader);
        }

        /// <inheritdoc/>
        public override SamlSecurityToken ReadSamlToken(XmlReader reader) {
            if (this.EncryptedSecurityTokenHandler.CanReadToken(reader)) {
                return (SamlSecurityToken)this.EncryptedSecurityTokenHandler.ReadToken(reader);
            }

            return base.ReadSamlToken(reader);
        }

        /// <inheritdoc/>
        public override string WriteToken(SecurityToken token) {
            if (token is EncryptedSecurityToken encryptedSecurityToken) {
                using (var memoryStream = new MemoryStream())
                using (var writer = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, ownsStream: false)) { 
                    this.EncryptedSecurityTokenHandler.WriteToken(writer, encryptedSecurityToken);
                    writer.Flush();
                    return Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
            }

            return base.WriteToken(token);
        }

        /// <inheritdoc/>
        public override void WriteToken(XmlWriter writer, SecurityToken token) {
            if (token is EncryptedSecurityToken encryptedSecurityToken) {
                this.EncryptedSecurityTokenHandler.WriteToken(writer, encryptedSecurityToken);
                return;
            }

            base.WriteToken(writer, token);
        }

        /// <inheritdoc/>
        public override ClaimsPrincipal ValidateToken(XmlReader reader, TokenValidationParameters validationParameters, out SecurityToken validatedToken) {
            if (this.EncryptedSecurityTokenHandler.CanReadToken(reader)) {
                return this.EncryptedSecurityTokenHandler.ValidateToken(reader, validationParameters, out validatedToken);
            }

            return base.ValidateToken(reader, validationParameters, out validatedToken);
        }

        /// <inheritdoc/>
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken) {
            if (this.EncryptedSecurityTokenHandler.CanReadToken(token)) {
                return this.EncryptedSecurityTokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }

            return base.ValidateToken(token, validationParameters, out validatedToken);
        }

        /// <inheritdoc/>
        public override SecurityKeyIdentifierClause CreateSecurityTokenReference(SecurityToken token, bool attached) {
            return new WsSecuritySecurityKeyIdentifierClause(token.Id, "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.0#SAMLAssertionID", "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV1.1");
        }

        /// <inheritdoc/>
        protected override SamlSubject CreateSubject(SecurityTokenDescriptor tokenDescriptor) {
            var samlSubject = base.CreateSubject(tokenDescriptor);

            // set NameFormat and NameQualifier
            var identityClaim = tokenDescriptor.Subject.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            if (identityClaim != null) {
                if (identityClaim.Properties.TryGetValue(ClaimProperties.SamlNameIdentifierFormat, out var nameFormat)) {
                    samlSubject.NameFormat = nameFormat;
                }

                if (identityClaim.Properties.TryGetValue(ClaimProperties.SamlNameIdentifierNameQualifier, out var nameQualifier)) {
                    samlSubject.NameQualifier = nameQualifier;
                }
            }

            return samlSubject;
        }

        /// <inheritdoc/>
        protected override ICollection<SamlStatement> CreateStatements(SecurityTokenDescriptor tokenDescriptor, AuthenticationInformation authenticationInformation) {
            if (authenticationInformation == null) {
                // Search for an Authentication Claim.
                var authenticationMethod = tokenDescriptor.Subject.Claims
                    .FirstOrDefault(claim => claim.Type == ClaimTypes.AuthenticationMethod)?.Value;
                var authenticationInstant = tokenDescriptor.Subject.Claims
                    .FirstOrDefault(claim => claim.Type == ClaimTypes.AuthenticationInstant)?.Value;

                if (authenticationMethod != null && authenticationInstant != null) {
                    var authInstantTime = DateTime.ParseExact(authenticationInstant,
                                                              SamlConstants.AcceptedDateTimeFormats,
                                                              DateTimeFormatInfo.InvariantInfo,
                                                              DateTimeStyles.None).ToUniversalTime();

                    authenticationInformation = new AuthenticationInformation(new Uri(authenticationMethod), authInstantTime);
                }
            }

            return base.CreateStatements(tokenDescriptor, authenticationInformation);
        }
    }
}
