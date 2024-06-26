﻿using Abc.IdentityModel.Tokens.UnitTests;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml;
using System;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Xunit;

namespace Abc.IdentityModel.Tokens.Saml.UnitTests {
    public class SamlSecurityTokenHandlerFixture {
        [Fact]
        public void Constructors() {
            var handler = new SamlSecurityTokenHandler();
        }

        [Theory, MemberData(nameof(CanReadTokenSourceData))]
        public void CanReadToken(SamlTheoryData theoryData) {
            var handler = new SamlSecurityTokenHandler();
            Assert.Equal(handler.CanReadToken(theoryData.Token), theoryData.CanRead);
        }

        public static TheoryData<SamlTheoryData> CanReadTokenSourceData {
            get => new TheoryData<SamlTheoryData>
            {
                    new SamlTheoryData {
                        CanRead = false,
                        Token = null,
                        TestId = nameof(CanReadToken) + "_" + "Null token",
                    },

                    new SamlTheoryData {
                        CanRead = false,
                        Token = new string('S', TokenValidationParameters.DefaultMaximumTokenSizeInBytes + 2),
                        TestId = nameof(CanReadToken) + "_" + "DefaultMaximumTokenSizeInBytes + 1",
                    },

                    new SamlTheoryData {
                        CanRead = true,
                        Token = ReferenceTokens.SamlToken_Valid,
                        TestId = nameof(CanReadToken) + "_" + nameof(ReferenceTokens.SamlToken_Valid),
                    },

                    new SamlTheoryData {
                        CanRead = true,
                        Token = ReferenceTokens.EncryptedSamlToken_Valid,
                        TestId = nameof(CanReadToken) + "_" + nameof(ReferenceTokens.EncryptedSamlToken_Valid),
                    },
            };
        }

        [Theory, MemberData(nameof(CanReadXmlTokenSourceData))]
        public void CanReadXmlToken(SamlTheoryData theoryData) {
            var handler = new SamlSecurityTokenHandler();
            Assert.Equal(handler.CanReadToken(theoryData.XmlReader), theoryData.CanRead);
        }

        public static TheoryData<SamlTheoryData> CanReadXmlTokenSourceData {
            get => new TheoryData<SamlTheoryData>
            {
                    new SamlTheoryData {
                        CanRead = false,
                        XmlReader = null,
                        TestId = nameof(CanReadXmlToken) + "_" + "Null token",
                    },

                    new SamlTheoryData {
                        CanRead = true,
                        XmlReader =  XmlUtilities.CreateDictionaryReader(ReferenceTokens.SamlToken_Valid),
                        TestId = nameof(CanReadXmlToken) + "_" + nameof(ReferenceTokens.SamlToken_Valid),
                    },

                    new SamlTheoryData {
                        CanRead = true,
                        XmlReader = XmlUtilities.CreateDictionaryReader(ReferenceTokens.EncryptedSamlToken_Valid),
                        TestId = nameof(CanReadXmlToken) + "_" + nameof(ReferenceTokens.EncryptedSamlToken_Valid),
                    },
            };
        }

        [Theory, MemberData(nameof(ReadTokenSourceData))]
        public void ReadToken(SamlTheoryData theoryData) {
            var handler = new SamlSecurityTokenHandler();

            try {
                var samlTokenFromString = handler.ReadToken(theoryData.Token);
                var samlTokenFromXmlReader = handler.ReadToken(theoryData.XmlReader);

                Assert.Equal(samlTokenFromString.Id, samlTokenFromXmlReader.Id);
                Assert.Equal(samlTokenFromString.Issuer, samlTokenFromXmlReader.Issuer);
                Assert.Equal(samlTokenFromString.SecurityKey, samlTokenFromXmlReader.SecurityKey);
                Assert.Equal(samlTokenFromString.SigningKey, samlTokenFromXmlReader.SigningKey);
                Assert.Equal(samlTokenFromString.ValidFrom, samlTokenFromXmlReader.ValidFrom);
                Assert.Equal(samlTokenFromString.ValidTo, samlTokenFromXmlReader.ValidTo);
            }
            catch (Exception ex) when (theoryData.ExpectedException != null) {
                Assert.IsType(theoryData.ExpectedException.GetType(), ex);
                Assert.StartsWith(theoryData.ExpectedException.Message, ex.Message);
            }
        }

        public static TheoryData<SamlTheoryData> ReadTokenSourceData {
            get => new TheoryData<SamlTheoryData>
            {
                new SamlTheoryData {
                    Token = null,
                    XmlReader = XmlUtilities.CreateXmlReader(ReferenceTokens.SamlToken_Valid),
                    ExpectedException = new ArgumentNullException("", "IDX10000:"),
                    TestId = nameof(ReadToken) + "_" + "Null token",
                },

                new SamlTheoryData {
                    Token = ReferenceTokens.SamlToken_Valid,
                    XmlReader = null,
                    ExpectedException = new ArgumentNullException("", "IDX10000:"),
                    TestId = nameof(ReadToken) + "_" + "Null XmlReader",
                },

                new SamlTheoryData {
                    Token = ReferenceTokens.SamlToken_Valid,
                    XmlReader = XmlUtilities.CreateXmlReader(ReferenceTokens.SamlToken_Valid),
                    TestId = nameof(ReadToken) + "_" + nameof(ReferenceTokens.SamlToken_Valid),
                },
            };
        }

        [Theory, MemberData(nameof(ValidateTokenSourceData))]
        public void ValidateToken(SamlTheoryData theoryData) {
            var handler = new SamlSecurityTokenHandler();
            try {
                handler.ValidateToken(theoryData.Token, theoryData.ValidationParameters, out SecurityToken validatedToken);
            }
            catch (Exception ex) when (theoryData.ExpectedException != null) {
                Assert.IsType(theoryData.ExpectedException.GetType(), ex);
                Assert.StartsWith(theoryData.ExpectedException.Message, ex.Message);
            }
        }

        public static TheoryData<SamlTheoryData> ValidateTokenSourceData {
            get {
                var validationParameters = new TokenValidationParameters {
                    AuthenticationType = "Federation",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = Default.X509SecurityKeySelfSigned2048_SHA256,
                    TokenDecryptionKey = new RsaSecurityKey(Default.CertSelfSigned2048_SHA256.GetRSAPrivateKey().ExportParameters(true)),
                };

                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData {
                        Token = null,
                        ValidationParameters = validationParameters,
                        ExpectedException = new ArgumentNullException("", "IDX10000:"),
                        TestId = nameof(ValidateToken) + "_" + "Null-Token",
                    },

                    new SamlTheoryData {
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = null,
                        ExpectedException = new ArgumentNullException("", "IDX10000:"),
                        TestId = nameof(ValidateToken) + "_" + "Null-TokenValidationParameters",
                    },

                    new SamlTheoryData {
                        Token = new string('S', TokenValidationParameters.DefaultMaximumTokenSizeInBytes + 2),
                        ValidationParameters = validationParameters,
                        ExpectedException = new ArgumentException("IDX10209:"),
                        TestId = nameof(ValidateToken) + "_" + "DefaultMaximumTokenSizeInBytes + 1",
                    },

                    new SamlTheoryData {
                        Token = ReferenceTokens.SamlToken_Valid,
                        ValidationParameters = validationParameters,
                        ExpectedException = null,
                        TestId = nameof(ValidateToken) + "_" + nameof(ReferenceTokens.SamlToken_Valid),
                    },

                    new SamlTheoryData {
                        Token = ReferenceTokens.EncryptedSamlToken_Valid,
                        ValidationParameters = validationParameters,
                        ExpectedException = null,
                        TestId = nameof(ValidateToken) + "_" + nameof(ReferenceTokens.EncryptedSamlToken_Valid),
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ValidateXmlTokenSourceData))]
        public void ValidateXmlToken(SamlTheoryData theoryData) {
            var handler = new SamlSecurityTokenHandler();
            try {
                handler.ValidateToken(theoryData.XmlReader, theoryData.ValidationParameters, out SecurityToken validatedToken);
            }
            catch (Exception ex) when (theoryData.ExpectedException != null) {
                Assert.IsType(theoryData.ExpectedException.GetType(), ex);
                Assert.StartsWith(theoryData.ExpectedException.Message, ex.Message);
            }
        }

        public static TheoryData<SamlTheoryData> ValidateXmlTokenSourceData {
            get {
                var validationParameters = new TokenValidationParameters {
                    AuthenticationType = "Federation",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = Default.X509SecurityKeySelfSigned2048_SHA256,
                    TokenDecryptionKey = new RsaSecurityKey(Default.CertSelfSigned2048_SHA256.GetRSAPrivateKey().ExportParameters(true)),
                };

                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData {
                        XmlReader = null,
                        ValidationParameters = validationParameters,
                        ExpectedException = new ArgumentNullException("", "IDX10000:"),
                        TestId = nameof(ValidateXmlToken) + "_" + "Null-XmlReader",
                    },

                    new SamlTheoryData {
                        XmlReader = XmlUtilities.CreateXmlReader(ReferenceTokens.Saml2Token_Valid),
                        ValidationParameters = validationParameters,
                        ExpectedException = new Microsoft.IdentityModel.Xml.XmlReadException("IDX30011:"),
                        TestId = nameof(ValidateXmlToken) + "_" + "InvalidSaml2Token",
                    },

                    new SamlTheoryData {
                        XmlReader = XmlUtilities.CreateXmlReader(ReferenceTokens.SamlToken_Valid),
                        ValidationParameters = null,
                        ExpectedException = new ArgumentNullException("", "IDX10000:"),
                        TestId = nameof(ValidateXmlToken) + "_" + "Null-TokenValidationParameters",
                    },

                    new SamlTheoryData {
                        XmlReader = XmlUtilities.CreateXmlReader(ReferenceTokens.SamlToken_Valid),
                        ValidationParameters = validationParameters,
                        ExpectedException = null,
                        TestId = nameof(ValidateXmlToken) + "_" + nameof(ReferenceTokens.SamlToken_Valid),
                    },

                    new SamlTheoryData {
                        XmlReader = XmlUtilities.CreateXmlReader(ReferenceTokens.EncryptedSamlToken_Valid),
                        ValidationParameters = validationParameters,
                        ExpectedException = null,
                        TestId = nameof(ValidateXmlToken) + "_" + nameof(ReferenceTokens.EncryptedSamlToken_Valid),
                    },
                };
            }
        }

        [Theory, MemberData(nameof(WriteTokenSourceData))]
        public void WriteToken(SamlTheoryData theoryData) {
            var handler = new SamlSecurityTokenHandler();

            try {
                var securityToken = theoryData.SecurityToken;
                var token = handler.WriteToken(securityToken);
                handler.ValidateToken(token, theoryData.ValidationParameters, out SecurityToken validatedToken);

                Assert.Equal(validatedToken.Id, securityToken.Id);
                Assert.Equal(validatedToken.Issuer, securityToken.Issuer);
                Assert.Equal(validatedToken.SecurityKey, securityToken.SecurityKey);
                //Assert.Equal(validatedToken.SigningKey, securityToken.SigningKey);
                Assert.Equal(validatedToken.ValidFrom, securityToken.ValidFrom);
                Assert.Equal(validatedToken.ValidTo, securityToken.ValidTo);
            }
            catch (Exception ex) when (theoryData.ExpectedException != null) {
                Assert.IsType(theoryData.ExpectedException.GetType(), ex);
                Assert.StartsWith(theoryData.ExpectedException.Message, ex.Message);
            }
        }

        public static TheoryData<SamlTheoryData> WriteTokenSourceData {
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

                var tokenHandler = new SamlSecurityTokenHandler();

                var theoryData = new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData {
                        SecurityToken = null,
                        ValidationParameters = validationParameters,
                        ExpectedException = new ArgumentNullException("", "IDX10000:"),
                        TestId = nameof(WriteToken) + "_" + "Null securityToken",
                    },

                    new SamlTheoryData {
                        SecurityToken = ReferenceJwt.JwtToken,
                        ValidationParameters = validationParameters,
                        ExpectedException = new ArgumentException("IDX11400:"),
                        TestId = nameof(WriteToken) + "_" + nameof(ReferenceJwt.JwtToken),
                    },

                    new SamlTheoryData {
                        SecurityToken = tokenHandler.CreateToken(tokenDescriptor),
                        ValidationParameters = validationParameters,
                        ExpectedException = null,
                        TestId = nameof(WriteToken) + "_" + nameof(SamlSecurityToken),
                    },
                };

                tokenDescriptor.EncryptingCredentials = new X509EncryptingCredentials(
                    Default.CertSelfSigned2048_SHA256_Public,
                    SecurityAlgorithms.RsaOaepKeyWrap,
                    SecurityAlgorithms.Aes256Encryption);

                validationParameters.TokenDecryptionKey = new RsaSecurityKey(Default.CertSelfSigned2048_SHA256.GetRSAPrivateKey().ExportParameters(true));

                theoryData.Add(new SamlTheoryData {
                    SecurityToken = tokenHandler.CreateToken(tokenDescriptor),
                    ValidationParameters = validationParameters,
                    ExpectedException = null,
                    TestId = nameof(WriteToken) + "_" + "EncryptedSecurityToken",
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(WriteTokenXmlSourceData))]
        public void WriteTokenXml(SamlTheoryData theoryData) {
            var handler = new SamlSecurityTokenHandler();
            try {
                handler.WriteToken(theoryData.XmlWriter, theoryData.SecurityToken);
            }
            catch (Exception ex) when (theoryData.ExpectedException != null) {
                Assert.IsType(theoryData.ExpectedException.GetType(), ex);
                Assert.StartsWith(theoryData.ExpectedException.Message, ex.Message);
            }
        }

        public static TheoryData<SamlTheoryData> WriteTokenXmlSourceData {
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

                var tokenHandler = new SamlSecurityTokenHandler();

                var theoryData = new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData {
                        XmlWriter = XmlDictionaryWriter.CreateTextWriter(new MemoryStream()),
                        SecurityToken = null,
                        ExpectedException = new ArgumentNullException("", "IDX10000:"),
                        TestId = nameof(WriteTokenXml) + "_" + "Null securityToken",
                    },

                    new SamlTheoryData {
                        XmlWriter = null,
                        SecurityToken = ReferenceJwt.JwtToken,
                        ExpectedException = new ArgumentNullException("", "IDX10000:"),
                        TestId = nameof(WriteTokenXml) + "_" + "Null XmlWriter",
                    },

                    new SamlTheoryData {
                        XmlWriter = XmlDictionaryWriter.CreateTextWriter(new MemoryStream()),
                        SecurityToken = ReferenceJwt.JwtToken,
                        ExpectedException = new ArgumentException("IDX11400:"),
                        TestId = nameof(WriteTokenXml) + "_" + nameof(ReferenceJwt.JwtToken),
                    },

                    new SamlTheoryData {
                        XmlWriter = XmlDictionaryWriter.CreateTextWriter(new MemoryStream()),
                        SecurityToken = tokenHandler.CreateToken(tokenDescriptor),
                        ExpectedException = null,
                        TestId = nameof(WriteTokenXml) + "_" + "WithoutInclusivePrefixList",
                    },
                };

                tokenDescriptor.EncryptingCredentials = new X509EncryptingCredentials(
                    Default.CertSelfSigned2048_SHA256_Public,
                    SecurityAlgorithms.RsaOaepKeyWrap,
                    SecurityAlgorithms.Aes256Encryption);

                theoryData.Add(new SamlTheoryData {
                    XmlWriter = XmlDictionaryWriter.CreateTextWriter(new MemoryStream()),
                    SecurityToken = tokenHandler.CreateToken(tokenDescriptor),
                    ExpectedException = null,
                    TestId = nameof(WriteTokenXml) + "_" + "EncryptedSecurityToken",
                });

                return theoryData;
            }
        }


        [Theory, MemberData(nameof(CreateAttributeTheoryData))]
        public void CreateAttribute(SamlTheoryData theoryData) {
            try {
                var attribute = (theoryData.Handler as SamlSecurityTokenHandlerPublic).CreateAttributePublic(theoryData.Claim);

                Assert.Equal(attribute.Name, theoryData.SamlAttribute.Name);
                Assert.Equal(attribute.Namespace, theoryData.SamlAttribute.Namespace);
                Assert.Equal(attribute.Values, theoryData.SamlAttribute.Values);
                Assert.Equal(attribute.AttributeValueXsiType, theoryData.SamlAttribute.AttributeValueXsiType);
                Assert.Equal(attribute.ClaimType, theoryData.SamlAttribute.ClaimType);
                Assert.Equal(attribute.OriginalIssuer, theoryData.SamlAttribute.OriginalIssuer);
            }
            catch (Exception ex) when (theoryData.ExpectedException != null) {
                Assert.IsType(theoryData.ExpectedException.GetType(), ex);
                Assert.StartsWith(theoryData.ExpectedException.Message, ex.Message);
            }
        }

        public static TheoryData<SamlTheoryData> CreateAttributeTheoryData {
            get {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        ExpectedException = new ArgumentNullException("", "IDX10000:"),
                        Handler = new SamlSecurityTokenHandlerPublic(),
                        TestId = nameof(CreateAttribute) + "_" + "Null claim",
                        Claim = null,
                    },
                    new SamlTheoryData
                    {
                        ExpectedException = new SamlSecurityTokenException("IDX11523:"),
                        Handler = new SamlSecurityTokenHandlerPublic(),
                        TestId = nameof(CreateAttribute) + "_" + "claim type without namespace",
                        Claim = new Claim("sub", "value"),
                    },
                    new SamlTheoryData
                    {
                        Handler = new SamlSecurityTokenHandlerPublic(),
                        TestId = nameof(CreateAttribute),
                        Claim = new Claim(ClaimTypes.Name, "value", ClaimValueTypes.String),
                        SamlAttribute = new SamlAttribute("http://schemas.xmlsoap.org/ws/2005/05/identity/claims", "name", "value")
                    },
                    new SamlTheoryData
                    {
                        Handler = new SamlSecurityTokenHandlerPublic(),
                        TestId = nameof(CreateAttribute) + "_" + "with OriginalIssuer",
                        Claim = new Claim(ClaimTypes.Name, "value", ClaimValueTypes.String, null, "issuer"),
                        SamlAttribute = new SamlAttribute("http://schemas.xmlsoap.org/ws/2005/05/identity/claims", "name", "value") { OriginalIssuer = "issuer" },
                    },
                };
            }
        }

        private class SamlSecurityTokenHandlerPublic : SamlSecurityTokenHandler {
            public SamlAttribute CreateAttributePublic(Claim claim) {
                return base.CreateAttribute(claim);
            }
        }
    }
}
