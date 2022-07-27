using Abc.IdentityModel.Tokens.Saml.UnitTests;
using Abc.IdentityModel.Tokens.UnitTests;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml2;
using System;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Xunit;

namespace Abc.IdentityModel.Tokens.Saml2.UnitTests {
    public class Saml2SecurityTokenHandlerFixture {
        
        [Fact]
        public void Constructors() {
            var handler = new Saml2SecurityTokenHandler();
        }


        [Theory, MemberData(nameof(CanReadTokenSourceData))]
        public void CanReadToken(Saml2TheoryData theoryData) {
            var handler = new Saml2SecurityTokenHandler();
            Assert.Equal(handler.CanReadToken(theoryData.Token), theoryData.CanRead);
        }

        public static TheoryData<Saml2TheoryData> CanReadTokenSourceData {
            get => new TheoryData<Saml2TheoryData>
            {
                new Saml2TheoryData {
                    CanRead = false,
                    Token = null,
                    TestId = nameof(CanReadToken) + "_" + "Null token",
                },

                new Saml2TheoryData {
                    CanRead = false,
                    Token = new string('S', TokenValidationParameters.DefaultMaximumTokenSizeInBytes + 2),
                    TestId = nameof(CanReadToken) + "_" + "DefaultMaximumTokenSizeInBytes + 1",
                },

                new Saml2TheoryData {
                    CanRead = true,
                    Token = ReferenceTokens.Saml2Token_Valid,
                    TestId = nameof(CanReadToken) + "_" + nameof(ReferenceTokens.Saml2Token_Valid),
                },

                new Saml2TheoryData {
                    CanRead = true,
                    Token = ReferenceTokens.EncryptedSaml2Token_Valid,
                    TestId = nameof(CanReadToken) + "_" + nameof(ReferenceTokens.EncryptedSaml2Token_Valid),
                },
            };
        }

        [Theory, MemberData(nameof(CanReadXmlTokenSourceData))]
        public void CanReadXmlToken(Saml2TheoryData theoryData) {
            var handler = new Saml2SecurityTokenHandler();
            Assert.Equal(handler.CanReadToken(theoryData.XmlReader), theoryData.CanRead);
        }

        public static TheoryData<Saml2TheoryData> CanReadXmlTokenSourceData {
            get => new TheoryData<Saml2TheoryData>
            {
                new Saml2TheoryData {
                    CanRead = false,
                    XmlReader = null,
                    TestId = nameof(CanReadXmlToken) + "_" + "Null token",
                },

                new Saml2TheoryData {
                    CanRead = true,
                    XmlReader = XmlUtilities.CreateDictionaryReader(ReferenceTokens.Saml2Token_Valid),
                    TestId = nameof(CanReadXmlToken) + "_" + nameof(ReferenceTokens.Saml2Token_Valid),
                },

                new Saml2TheoryData {
                    CanRead = true,
                    XmlReader = XmlUtilities.CreateDictionaryReader(ReferenceTokens.EncryptedSaml2Token_Valid),
                    TestId = nameof(CanReadXmlToken) + "_" + nameof(ReferenceTokens.EncryptedSaml2Token_Valid),
                },
            };
        }

        [Theory, MemberData(nameof(ReadTokenSourceData))]
        public void ReadToken(Saml2TheoryData theoryData) {
            var handler = new Saml2SecurityTokenHandler();

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
            }
        }

        public static TheoryData<Saml2TheoryData> ReadTokenSourceData {
            get => new TheoryData<Saml2TheoryData>
            {
                new Saml2TheoryData {
                    Token = null,
                    XmlReader = XmlUtilities.CreateXmlReader(ReferenceTokens.Saml2Token_Valid),
                    ExpectedException = new ArgumentNullException("token"),
                    TestId = nameof(ReadToken) + "_" + "Null token",
                },

                new Saml2TheoryData {
                    Token = ReferenceTokens.Saml2Token_Valid,
                    XmlReader = null,
                    ExpectedException = new ArgumentNullException("reader"),
                    TestId = nameof(ReadToken) + "_" + "Null XmlReader",
                },

                new Saml2TheoryData {
                    Token = ReferenceTokens.Saml2Token_Valid,
                    XmlReader = XmlUtilities.CreateXmlReader(ReferenceTokens.Saml2Token_Valid),
                    TestId = nameof(ReadToken) + "_" + nameof(ReferenceTokens.Saml2Token_Valid),
                },
            };
        }

        [Theory, MemberData(nameof(ValidateTokenSourceData))]
        public void ValidateToken(Saml2TheoryData theoryData) {
            var handler = new Saml2SecurityTokenHandler();
            try {
                handler.ValidateToken(theoryData.Token, theoryData.ValidationParameters, out SecurityToken validatedToken);
            }
            catch (Exception ex) when (theoryData.ExpectedException != null) {
                Assert.IsType(theoryData.ExpectedException.GetType(), ex);
            }
        }

        public static TheoryData<Saml2TheoryData> ValidateTokenSourceData {
            get {
                var validationParameters = new TokenValidationParameters {
                    AuthenticationType = "Federation",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = Default.X509SecurityKeySelfSigned2048_SHA256,
                    TokenDecryptionKey = new RsaSecurityKey(Default.CertSelfSigned2048_SHA256.GetRSAPrivateKey().ExportParameters(true)),
                };

                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData {
                        Token = null,
                        ValidationParameters = validationParameters,
                        ExpectedException = new ArgumentNullException("token"),
                        TestId = nameof(ValidateToken) + "_" + "Null-Token",
                    },

                    new Saml2TheoryData {
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = null,
                        ExpectedException = new ArgumentNullException("validationParameters"),
                        TestId = nameof(ValidateToken) + "_" + "Null-TokenValidationParameters",
                    },

                    new Saml2TheoryData {
                        Token = new string('S', TokenValidationParameters.DefaultMaximumTokenSizeInBytes + 2),
                        ValidationParameters = validationParameters,
                        ExpectedException = new ArgumentException("IDX10209:"),
                        TestId = nameof(ValidateToken) + "_" + "DefaultMaximumTokenSizeInBytes + 1",
                    },

                    new Saml2TheoryData {
                        Token = ReferenceTokens.Saml2Token_Valid,
                        ValidationParameters = validationParameters,
                        ExpectedException = null,
                        TestId = nameof(ValidateToken) + "_" + nameof(ReferenceTokens.Saml2Token_Valid),
                    },

                    new Saml2TheoryData {
                        Token = ReferenceTokens.EncryptedSaml2Token_Valid,
                        ValidationParameters = validationParameters,
                        ExpectedException = null,
                        TestId = nameof(ValidateToken) + "_" + nameof(ReferenceTokens.EncryptedSaml2Token_Valid),
                    },

                };
            }
        }

        [Theory, MemberData(nameof(ValidateXmlTokenSourceData))]
        public void ValidateXmlToken(Saml2TheoryData theoryData) {
            var handler = new Saml2SecurityTokenHandler();
            try {
                handler.ValidateToken(theoryData.XmlReader, theoryData.ValidationParameters, out SecurityToken validatedToken);
            }
            catch (Exception ex) when (theoryData.ExpectedException != null) {
                Assert.IsType(theoryData.ExpectedException.GetType(), ex);
            }
        }

        public static TheoryData<Saml2TheoryData> ValidateXmlTokenSourceData {
            get {
                var validationParameters = new TokenValidationParameters {
                    AuthenticationType = "Federation",
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = Default.X509SecurityKeySelfSigned2048_SHA256,
                    TokenDecryptionKey = new RsaSecurityKey(Default.CertSelfSigned2048_SHA256.GetRSAPrivateKey().ExportParameters(true)),
                };

                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData {
                        XmlReader = null,
                        ValidationParameters = validationParameters,
                        ExpectedException = new ArgumentNullException("xmlReader"),
                        TestId = nameof(ValidateXmlToken) + "_" + "Null-XmlReader",
                    },

                    new Saml2TheoryData {
                        XmlReader = XmlUtilities.CreateXmlReader(ReferenceTokens.SamlToken_Valid),
                        ValidationParameters = validationParameters,
                        ExpectedException = new Microsoft.IdentityModel.Xml.XmlReadException(),
                        TestId = nameof(ValidateXmlToken) + "_" + "InvalidSaml2Token",
                    },

                    new Saml2TheoryData {
                        XmlReader = XmlUtilities.CreateXmlReader(ReferenceTokens.Saml2Token_Valid),
                        ValidationParameters = null,
                        ExpectedException = new ArgumentNullException("validationParameters"),
                        TestId = nameof(ValidateXmlToken) + "_" + "Null-TokenValidationParameters",
                    },

                    new Saml2TheoryData {
                        XmlReader = XmlUtilities.CreateXmlReader(ReferenceTokens.Saml2Token_Valid),
                        ValidationParameters = validationParameters,
                        ExpectedException = null,
                        TestId = nameof(ValidateXmlToken) + "_" + nameof(ReferenceTokens.Saml2Token_Valid),
                    },

                    new Saml2TheoryData {
                        XmlReader = XmlUtilities.CreateXmlReader(ReferenceTokens.EncryptedSaml2Token_Valid),
                        ValidationParameters = validationParameters,
                        ExpectedException = null,
                        TestId = nameof(ValidateXmlToken) + "_" + nameof(ReferenceTokens.EncryptedSaml2Token_Valid),
                    },

                };
            }
        }

        [Theory, MemberData(nameof(WriteTokenSourceData))]
        public void WriteToken(Saml2TheoryData theoryData) {
            var handler = new Saml2SecurityTokenHandler();

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
                //Assert.StartsWith(theoryData.ExpectedException.Message, ex.Message);
            }
        }

        public static TheoryData<Saml2TheoryData> WriteTokenSourceData {
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

                var theoryData = new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData {
                        SecurityToken = null,
                        ValidationParameters = validationParameters,
                        ExpectedException = new ArgumentNullException("securityToken", "IDX10000:"),
                        TestId = nameof(WriteToken) + "_" + "Null securityToken",
                    },

                    new Saml2TheoryData {
                        SecurityToken = ReferenceJwt.JwtToken,
                        ValidationParameters = validationParameters,
                        ExpectedException = new ArgumentException("IDX13400:"),
                        TestId = nameof(WriteToken) + "_" + nameof(ReferenceJwt.JwtToken),
                    },

                    new Saml2TheoryData {
                        SecurityToken = tokenHandler.CreateToken(tokenDescriptor),
                        ValidationParameters = validationParameters,
                        ExpectedException = null,
                        TestId = nameof(WriteToken) + "_" + nameof(Saml2SecurityToken),
                    },
                };

                tokenDescriptor.EncryptingCredentials = new X509EncryptingCredentials(
                    Default.CertSelfSigned2048_SHA256_Public,
                    SecurityAlgorithms.RsaOaepKeyWrap,
                    SecurityAlgorithms.Aes256Encryption);

                validationParameters.TokenDecryptionKey = new RsaSecurityKey(Default.CertSelfSigned2048_SHA256.GetRSAPrivateKey().ExportParameters(true));

                theoryData.Add(new Saml2TheoryData {
                    SecurityToken = tokenHandler.CreateToken(tokenDescriptor),
                    ValidationParameters = validationParameters,
                    ExpectedException = null,
                    TestId = nameof(WriteToken) + "_" + "EncryptedSecurityToken",
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(WriteTokenXmlSourceData))]
        public void WriteTokenXml(Saml2TheoryData theoryData) {
            var handler = new Saml2SecurityTokenHandler();
            try {
                handler.WriteToken(theoryData.XmlWriter, theoryData.SecurityToken);
            }
            catch (Exception ex) when (theoryData.ExpectedException != null) {
                Assert.IsType(theoryData.ExpectedException.GetType(), ex);
                //Assert.StartsWith(theoryData.ExpectedException.Message, ex.Message);
            }
        }

        public static TheoryData<Saml2TheoryData> WriteTokenXmlSourceData {
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

                var theoryData = new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData {
                        XmlWriter = XmlDictionaryWriter.CreateTextWriter(new MemoryStream()),
                        SecurityToken = null,
                        ExpectedException = new ArgumentNullException("securityToken", "IDX10000:"),
                        TestId = nameof(WriteTokenXml) + "_" + "Null securityToken",
                    },

                    new Saml2TheoryData {
                        XmlWriter = null,
                        SecurityToken = ReferenceJwt.JwtToken,
                        ExpectedException = new ArgumentNullException("writer", "IDX10000:"),
                        TestId = nameof(WriteTokenXml) + "_" + "Null XmlWriter",
                    },

                    new Saml2TheoryData {
                        XmlWriter = XmlDictionaryWriter.CreateTextWriter(new MemoryStream()),
                        SecurityToken = ReferenceJwt.JwtToken,
                        ExpectedException = new Saml2SecurityTokenWriteException("IDX13150:"),
                        TestId = nameof(WriteTokenXml) + "_" + nameof(ReferenceJwt.JwtToken),
                    },

                    new Saml2TheoryData {
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

                theoryData.Add(new Saml2TheoryData {
                    XmlWriter = XmlDictionaryWriter.CreateTextWriter(new MemoryStream()),
                    SecurityToken = tokenHandler.CreateToken(tokenDescriptor),
                    ExpectedException = null,
                    TestId = nameof(WriteTokenXml) + "_" + "EncryptedSecurityToken",
                });

                return theoryData;
            }
        }
    }
}
