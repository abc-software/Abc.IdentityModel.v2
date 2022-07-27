using Abc.IdentityModel.Tokens.UnitTests;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml2;
using Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Abc.IdentityModel.Tokens.Saml2.UnitTests {
    public class Saml2SecurityTokenHandlerFixture {
        
        public Saml2SecurityTokenHandlerFixture() {
            IdentityModelEventSource.ShowPII = true;
        }

        [Fact]
        public void Constructors() {
            var handler = new Saml2SecurityTokenHandler();
        }

        [TestCaseSource(nameof(CanReadTokenSourceData))]
        public void CanReadToken(bool canRead, string token) {
            var handler = new Saml2SecurityTokenHandler();
            Assert.That(handler.CanReadToken(token), Is.EqualTo(canRead), $"Token: {token}");
        }

        public static IEnumerable<TestCaseData> CanReadTokenSourceData {
            get {
                yield return 
                    new TestCaseData(false, null)
                        .SetName(nameof(CanReadToken) + "_" + "Null token");

                yield return 
                    new TestCaseData(false, new string('S', TokenValidationParameters.DefaultMaximumTokenSizeInBytes + 2))
                        .SetName(nameof(CanReadToken) + "_" + "DefaultMaximumTokenSizeInBytes + 1");

                yield return 
                    new TestCaseData(true, ReferenceTokens.Saml2Token_Valid)
                        .SetName(nameof(CanReadToken) + "_" + nameof(ReferenceTokens.Saml2Token_Valid));

                yield return
                    new TestCaseData(true, ReferenceTokens.EncryptedSaml2Token_Valid)
                        .SetName(nameof(CanReadToken) + "_" + nameof(ReferenceTokens.EncryptedSaml2Token_Valid));

            }
        }

        [TestCaseSource(nameof(CanReadXmlTokenSourceData))]
        public void CanReadXmlToken(bool canRead, XmlReader xmlReader) {
            var handler = new Saml2SecurityTokenHandler();
            Assert.That(handler.CanReadToken(xmlReader), Is.EqualTo(canRead));
        }

        public static IEnumerable<TestCaseData> CanReadXmlTokenSourceData {
            get {
                yield return
                    new TestCaseData(
                        false, 
                        null
                        ).SetName(nameof(CanReadXmlToken) + "_" + "Null token");

                yield return
                    new TestCaseData(
                        true, 
                        XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(ReferenceTokens.Saml2Token_Valid), XmlDictionaryReaderQuotas.Max)
                        ).SetName(nameof(CanReadXmlToken) + "_" + nameof(ReferenceTokens.Saml2Token_Valid));

                yield return
                    new TestCaseData(
                        true,
                        XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(ReferenceTokens.EncryptedSaml2Token_Valid), XmlDictionaryReaderQuotas.Max)
                    ).SetName(nameof(CanReadXmlToken) + "_" + nameof(ReferenceTokens.EncryptedSaml2Token_Valid));
            }
        }

        [TestCaseSource(nameof(ReadTokenSourceData))]
        public void ReadToken(string token, XmlReader xmlReader, Exception expectedException) {
            var handler = new Saml2SecurityTokenHandler();

            try {
                var samlTokenFromString = handler.ReadToken(token);
                var samlTokenFromXmlReader = handler.ReadToken(xmlReader);

                Assert.That(samlTokenFromString.Id, Is.EqualTo(samlTokenFromXmlReader.Id));
                Assert.That(samlTokenFromString.Issuer, Is.EqualTo(samlTokenFromXmlReader.Issuer));
                Assert.That(samlTokenFromString.SecurityKey, Is.EqualTo(samlTokenFromXmlReader.SecurityKey));
                Assert.That(samlTokenFromString.SigningKey, Is.EqualTo(samlTokenFromXmlReader.SigningKey));
                Assert.That(samlTokenFromString.ValidFrom, Is.EqualTo(samlTokenFromXmlReader.ValidFrom));
                Assert.That(samlTokenFromString.ValidTo, Is.EqualTo(samlTokenFromXmlReader.ValidTo));
            }
            catch (Exception ex) when (expectedException != null) {
                Assert.IsInstanceOf(expectedException.GetType(), ex);
            }
        }

        public static IEnumerable<TestCaseData> ReadTokenSourceData {
            get {
                yield return new TestCaseData(
                    ReferenceTokens.Saml2Token_Valid,
                    XmlReader.Create(new StringReader(ReferenceTokens.Saml2Token_Valid), new XmlReaderSettings() { XmlResolver = null }),
                    null
                    ).SetName(nameof(ReadToken) + "_" + nameof(ReferenceTokens.Saml2Token_Valid));

                /*
                yield return new TestCaseData(
                    ReferenceTokens.EncryptedSaml2Token_Valid,
                    XmlReader.Create(new StringReader(ReferenceTokens.EncryptedSaml2Token_Valid), new XmlReaderSettings() { XmlResolver = null }),
                    null
                    ).SetName(nameof(ReadToken) + "_" + nameof(ReferenceTokens.EncryptedSaml2Token_Valid));
                */

            }
        }

        [TestCaseSource(nameof(ValidateTokenSourceData))]
        public void ValidateToken(string token, TokenValidationParameters validationParameters, Exception expectedException) {
            var handler = new Saml2SecurityTokenHandler();
            try {
                handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            }
            catch (Exception ex) when (expectedException != null) {
                Assert.IsInstanceOf(expectedException.GetType(), ex);
            }
        }

        public static IEnumerable<TestCaseData> ValidateTokenSourceData {
            get {
                var validationParameters = new TokenValidationParameters {
                    AuthenticationType = "Federation",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = Default.X509SecurityKeySelfSigned2048_SHA256,
                    TokenDecryptionKey = new RsaSecurityKey(Default.CertSelfSigned2048_SHA256.GetRSAPrivateKey().ExportParameters(true)),
                };

                yield return new TestCaseData(
                    null,
                    validationParameters,
                    new ArgumentNullException("token")
                    ).SetName(nameof(ValidateToken) + "_" + "Null-Token");

                yield return new TestCaseData(
                    ReferenceTokens.Saml2Token_Valid,
                    null,
                    new ArgumentNullException("validationParameters")
                    ).SetName(nameof(ValidateToken) + "_" + "Null-TokenValidationParameters");

                yield return new TestCaseData(
                    ReferenceTokens.Saml2Token_Valid,
                    validationParameters,
                    null
                    ).SetName(nameof(ValidateToken) + "_" + nameof(ReferenceTokens.Saml2Token_Valid));

                yield return new TestCaseData(
                    ReferenceTokens.EncryptedSaml2Token_Valid,
                    validationParameters,
                    null
                    ).SetName(nameof(ValidateToken) + "_" + nameof(ReferenceTokens.EncryptedSaml2Token_Valid));
            }
        }

        [TestCaseSource(nameof(ValidateXmlTokenSourceData))]
        public void ValidateXmlToken(XmlReader xmlReader, TokenValidationParameters validationParameters, Exception expectedException) {
            var handler = new Saml2SecurityTokenHandler();
            try {
                handler.ValidateToken(xmlReader, validationParameters, out SecurityToken validatedToken);
            }
            catch (Exception ex) when (expectedException != null) {
                Assert.IsInstanceOf(expectedException.GetType(), ex);
            }
        }

        public static IEnumerable<TestCaseData> ValidateXmlTokenSourceData {
            get {
                var validationParameters = new TokenValidationParameters {
                    AuthenticationType = "Federation",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = Default.X509SecurityKeySelfSigned2048_SHA256,
                    TokenDecryptionKey = new RsaSecurityKey(Default.CertSelfSigned2048_SHA256.GetRSAPrivateKey().ExportParameters(true)),
                };

                yield return new TestCaseData(
                    null,
                    validationParameters,
                    new ArgumentNullException("xmlReader")
                    ).SetName(nameof(ValidateXmlToken) + "_" + "Null-XmlReader");

                yield return new TestCaseData(
                    XmlReader.Create(new StringReader(ReferenceTokens.Saml2Token_Valid), new XmlReaderSettings() { XmlResolver = null }),
                    null,
                    new ArgumentNullException("validationParameters")
                    ).SetName(nameof(ValidateXmlToken) + "_" + "Null-TokenValidationParameters");

                yield return new TestCaseData(
                    XmlReader.Create(new StringReader(ReferenceTokens.Saml2Token_Valid), new XmlReaderSettings() { XmlResolver = null }),
                    validationParameters,
                    null
                    ).SetName(nameof(ValidateXmlToken) + "_" + nameof(ReferenceTokens.Saml2Token_Valid));

                yield return new TestCaseData(
                    XmlReader.Create(new StringReader(ReferenceTokens.EncryptedSaml2Token_Valid), new XmlReaderSettings() { XmlResolver = null }),
                    validationParameters,
                    null
                    ).SetName(nameof(ValidateXmlToken) + "_" + nameof(ReferenceTokens.EncryptedSaml2Token_Valid));

            }
        }

        [TestCaseSource(nameof(WriteTokenSourceData))]
        public void WriteToken(SecurityToken securityToken, TokenValidationParameters validationParameters, Exception expectedException) {
            var handler = new Saml2SecurityTokenHandler();

            try {
                var token = handler.WriteToken(securityToken);
                handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                Assert.That(validatedToken.Id, Is.EqualTo(securityToken.Id));
                Assert.That(validatedToken.Issuer, Is.EqualTo(securityToken.Issuer));
                Assert.That(validatedToken.SecurityKey, Is.EqualTo(securityToken.SecurityKey));
                //Assert.That(validatedToken.SigningKey, Is.EqualTo(securityToken.SigningKey));
                Assert.That(validatedToken.ValidFrom, Is.EqualTo(securityToken.ValidFrom));
                Assert.That(validatedToken.ValidTo, Is.EqualTo(securityToken.ValidTo));
            }
            catch (Exception ex) when (expectedException != null) {
                Assert.IsInstanceOf(expectedException.GetType(), ex);
            }
        }

        public static IEnumerable<TestCaseData> WriteTokenSourceData {
            get {
                var key = Default.X509SecurityKeySelfSigned2048_SHA256;
                var tokenDescriptor = new SecurityTokenDescriptor {
                    Audience = Default.Audience,
                    NotBefore = Default.NotBefore,
                    Expires = Default.Expires,
                    Issuer = Default.Issuer,
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                    Subject = new ClaimsIdentity(Default.DefaultClaims),
                };

                var validationParameters = new TokenValidationParameters {
                    AuthenticationType = "Federation",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = key
                };

                var tokenHandler = new Saml2SecurityTokenHandler();

                yield return new TestCaseData(
                    null,
                    validationParameters,
                    new ArgumentNullException("token")
                    ).SetName(nameof(WriteToken) + "_" + "Null token");

                yield return new TestCaseData(
                    ReferenceJwt.JwtToken,
                    validationParameters,
                    new ArgumentException("IDX13400")
                    ).SetName(nameof(WriteToken) + "_" + nameof(ReferenceJwt.JwtToken));

                yield return new TestCaseData(
                    tokenHandler.CreateToken(tokenDescriptor),
                    validationParameters,
                    null
                    ).SetName(nameof(WriteToken) + "_" + nameof(Saml2SecurityToken));

                /*
                tokenDescriptor.EncryptingCredentials = new X509EncryptingCredentials(
                    Default.CertSelfSigned2048_SHA256_Public,
                    SecurityAlgorithms.RsaOaepKeyWrap,
                    SecurityAlgorithms.Aes256Encryption);

                yield return new TestCaseData(
                    tokenHandler.CreateToken(tokenDescriptor),
                    validationParameters,
                    null
                    ).SetName(nameof(WriteToken) + "_" + "EncryptedSecurityToken");
                */
                
            }
        }

        [TestCaseSource(nameof(WriteTokenXmlSourceData))]
        public void WriteTokenXml(XmlWriter xmlWriter, SecurityToken securityToken, Exception exceptedException) {
            var handler = new Saml2SecurityTokenHandler();
            try {
                handler.WriteToken(xmlWriter, securityToken);
            }
            catch (Exception ex) when (exceptedException != null) {
                Assert.IsInstanceOf(exceptedException.GetType(), ex);
            }
        }

        public static IEnumerable<TestCaseData> WriteTokenXmlSourceData {
            get {
                var key = Default.X509SecurityKeySelfSigned2048_SHA256;
                var tokenDescriptor = new SecurityTokenDescriptor {
                    Audience = Default.Audience,
                    NotBefore = Default.NotBefore,
                    Expires = Default.Expires,
                    Issuer = Default.Issuer,
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                    Subject = new ClaimsIdentity(Default.DefaultClaims),
                };

                var tokenHandler = new Saml2SecurityTokenHandler();

                yield return new TestCaseData(
                    XmlDictionaryWriter.CreateTextWriter(new MemoryStream()),
                    null,
                    new ArgumentNullException("token")
                    ).SetName(nameof(WriteTokenXml) + "_" + "Null Token");

                yield return new TestCaseData(
                    null,
                    null,
                    new ArgumentNullException("writer")
                    ).SetName(nameof(WriteTokenXml) + "_" + "Null XmlWriter");

                yield return new TestCaseData(
                    XmlDictionaryWriter.CreateTextWriter(new MemoryStream()),
                    ReferenceJwt.JwtToken,
                    new Saml2SecurityTokenWriteException("IDX13150:") // not ArgumentException
                    ).SetName(nameof(WriteTokenXml) + "_" + nameof(ReferenceJwt.JwtToken));

                yield return new TestCaseData(
                    XmlDictionaryWriter.CreateTextWriter(new MemoryStream()),
                    tokenHandler.CreateToken(tokenDescriptor),
                    null
                    ).SetName(nameof(WriteTokenXml) + "_" + "WithoutInclusivePrefixList");

                tokenDescriptor.EncryptingCredentials = new X509EncryptingCredentials(
                    Default.CertSelfSigned2048_SHA256_Public,
                    SecurityAlgorithms.RsaOaepKeyWrap, 
                    SecurityAlgorithms.Aes256Encryption);

                yield return new TestCaseData(
                    XmlDictionaryWriter.CreateTextWriter(new MemoryStream()),
                    tokenHandler.CreateToken(tokenDescriptor),
                    null
                    ).SetName(nameof(WriteTokenXml) + "_" + "EncryptedSecurityToken");
            }
        }
    }
}
