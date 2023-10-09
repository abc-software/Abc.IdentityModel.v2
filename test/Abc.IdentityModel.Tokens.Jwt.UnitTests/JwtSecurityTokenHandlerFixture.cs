using Microsoft.IdentityModel.Tokens;
using System.Xml;

namespace Abc.IdentityModel.Tokens.Jwt.UnitTests {
    public class JwtSecurityTokenHandlerFixture
    {
        [Fact]
        public void CreateSecurityTokenReference() {
            var handler = new JwtSecurityTokenHandler();
            handler.CreateSecurityTokenReference(null, false).Should().BeNull();   
        }

        [Theory, MemberData(nameof(CanReadTokenTheoryData))]
        public void CanReadToken(JwtTheoryData theoryData) {
            theoryData.Handler
                .CanReadToken(theoryData.XmlReader)
                .Should().Be(theoryData.CanRead);
        }

        public static TheoryData<JwtTheoryData> CanReadTokenTheoryData {
            get => new TheoryData<JwtTheoryData>
            {
                new JwtTheoryData
                {
                    CanRead = false,
                    TestId = "Null Token",
                    XmlReader = null
                },
                new JwtTheoryData
                {
                    CanRead = true,
                    TestId = nameof(ReferenceJwt.JwtToken_Valid),
                    XmlReader = XmlUtilities.CreateXmlReader(ReferenceJwt.JwtToken_Valid)
                },
                new JwtTheoryData
                {
                    CanRead = true,
                    TestId = nameof(ReferenceJwt.JwtTokenAlt_Valid),
                    XmlReader = XmlUtilities.CreateXmlReader(ReferenceJwt.JwtTokenAlt_Valid)
                },
                new JwtTheoryData
                {
                    CanRead = false,
                    TestId = nameof(ReferenceJwt.Saml2Token_Valid),
                    XmlReader =  XmlUtilities.CreateXmlReader(ReferenceJwt.Saml2Token_Valid)
                }
            };
        }

        [Theory, MemberData(nameof(ReadTokenXmlTheoryData))]
        public void ReadTokenXml(JwtTheoryData theoryData) {
            Action act = () => theoryData.Handler.ReadToken(theoryData.XmlReader);

            if (theoryData.ExpectedException != null) {
                var ex = act.Should().Throw<ArgumentException>();
                ex.And.Should().BeOfType(theoryData.ExpectedException.GetType());
                ex.And.ParamName.Should().Be(theoryData.ExpectedException.ParamName);
                ex.And.Message.Should().StartWith(theoryData.ExpectedException.Message);
            }
            else {
                act.Should().NotThrow();
            }
        }

        public static TheoryData<JwtTheoryData> ReadTokenXmlTheoryData {
            get {
                return new TheoryData<JwtTheoryData>
                {
                    new JwtTheoryData
                    {
                        ExpectedException = new ArgumentNullException("reader"),
                        TestId = "XmlReader: null",
                        XmlReader = null,
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(ReferenceJwt.JwtToken_Valid),
                        XmlReader = XmlUtilities.CreateXmlReader(ReferenceJwt.JwtToken_Valid),
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(ReferenceJwt.JwtTokenAlt_Valid),
                        XmlReader = XmlUtilities.CreateXmlReader(ReferenceJwt.JwtTokenAlt_Valid),
                    },
                    new JwtTheoryData
                    {
                        ExpectedException = new ArgumentException("Jwt10203:"),
                        TestId = nameof(ReferenceJwt.Saml2Token_Valid),
                        XmlReader = XmlUtilities.CreateXmlReader(ReferenceJwt.Saml2Token_Valid),
                    },
                };
            }
        }

        [Theory, MemberData(nameof(WriteTokenXmlTheoryData))]
        public void WriteTokenXml(JwtTheoryData theoryData) {
            Action act = () => theoryData.Handler.WriteToken(theoryData.XmlWriter, theoryData.SecurityToken);

            if (theoryData.ExpectedException != null) {
                var ex = act.Should().Throw<ArgumentException>();
                ex.And.Should().BeOfType(theoryData.ExpectedException.GetType());
                ex.And.ParamName.Should().Be(theoryData.ExpectedException.ParamName);
                ex.And.Message.Should().StartWith(theoryData.ExpectedException.Message);
            }
            else {
                act.Should().NotThrow();
            }
        }

        public static TheoryData<JwtTheoryData> WriteTokenXmlTheoryData {
            get {
                var theoryData = new TheoryData<JwtTheoryData>();
                var memoryStream = new MemoryStream();
                theoryData.Add(new JwtTheoryData {
                    ExpectedException = new ArgumentNullException("token"),
                    TestId = "Null token",
                    SecurityToken = null,
                    XmlWriter = XmlDictionaryWriter.CreateTextWriter(memoryStream)
                });

                theoryData.Add(new JwtTheoryData {
                    ExpectedException = new ArgumentNullException("writer"),
                    TestId = "Null XmlWriter",
                    SecurityToken = ReferenceJwt.JwtSecurityToken
                });

                theoryData.Add(new JwtTheoryData {
                    TestId = nameof(ReferenceJwt.JwtToken_Valid),
                    SecurityToken = ReferenceJwt.JwtSecurityToken,
                    XmlWriter = XmlDictionaryWriter.CreateTextWriter(memoryStream),
                    MemoryStream = memoryStream
                });

                memoryStream = new MemoryStream();
                theoryData.Add(new JwtTheoryData {
                    ExpectedException = new ArgumentException("Jwt10200:"),
                    TestId = nameof(ReferenceJwt.Saml2Token_Valid),
                    SecurityToken = ReferenceJwt.SamlSecurityToken,
                    XmlWriter = XmlDictionaryWriter.CreateTextWriter(memoryStream)
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateTokenXmlTheoryData))]
        public void ValidateTokenXml(JwtTheoryData theoryData) {
            Action act = () => theoryData.Handler.ValidateToken(theoryData.XmlReader, theoryData.ValidationParameters, out SecurityToken validatedToken);

            if (theoryData.ExpectedException != null) {
                var ex = act.Should().Throw<ArgumentException>();
                ex.And.Should().BeOfType(theoryData.ExpectedException.GetType());
                ex.And.ParamName.Should().Be(theoryData.ExpectedException.ParamName);
                ex.And.Message.Should().StartWith(theoryData.ExpectedException.Message);
            }
            else {
                act.Should().NotThrow();
            }
        }

        [Theory, MemberData(nameof(ValidateTokenXmlTheoryData))]
        public void ReadToken(JwtTheoryData theoryData) {
            Action act = () => theoryData.Handler.ReadToken(theoryData.XmlReader, theoryData.ValidationParameters);

            if (theoryData.ExpectedException != null) {
                var ex = act.Should().Throw<ArgumentException>();
                ex.And.Should().BeOfType(theoryData.ExpectedException.GetType());
                ex.And.ParamName.Should().Be(theoryData.ExpectedException.ParamName);
                ex.And.Message.Should().StartWith(theoryData.ExpectedException.Message);
            }
            else {
                act.Should().NotThrow();
            }
        }

        public static TheoryData<JwtTheoryData> ValidateTokenXmlTheoryData {
            get {
                var validationParameters = new TokenValidationParameters {
                    RequireSignedTokens = false,
                    ValidAudience = Default.Audience,
                    ValidIssuer = Default.Issuer,
                    ValidateLifetime = false,
                };

                return new TheoryData<JwtTheoryData>
                {
                    new JwtTheoryData
                    {
                        ExpectedException = new ArgumentNullException("reader"),
                        TestId = "XmlReader: null",
                        ValidationParameters = new TokenValidationParameters(),
                        XmlReader = null,
                    },
                    new JwtTheoryData
                    {
                        ExpectedException = new ArgumentNullException("validationParameters"),
                        TestId = "TokenValidationParameters: null",
                        ValidationParameters = null,
                        XmlReader = XmlUtilities.CreateXmlReader(ReferenceJwt.JwtToken_Valid),
                    },
                    new JwtTheoryData
                    {
                        TestId = nameof(ReferenceJwt.JwtToken_Valid),
                        ValidationParameters = validationParameters,
                        XmlReader = XmlUtilities.CreateXmlReader(ReferenceJwt.JwtToken_Valid),
                    },
                    new JwtTheoryData
                    {
                        ExpectedException = new ArgumentException("Jwt10203:"),
                        TestId = nameof(ReferenceJwt.Saml2Token_Valid),
                        ValidationParameters = validationParameters,
                        XmlReader = XmlUtilities.CreateXmlReader(ReferenceJwt.Saml2Token_Valid),
                    },
                    //new JwtTheoryData
                    //{
                    //    ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException(substringExpected: "IDX10508:", innerTypeExpected: typeof(FormatException)),
                    //    TestId = "Token: Invalid Format",
                    //    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.e30.f",
                    //    ValidationParameters = new TokenValidationParameters()
                    //},
                };
            }
        }
    }
}