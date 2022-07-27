using Abc.IdentityModel.Tokens.Saml.UnitTests;
using Abc.IdentityModel.Tokens.UnitTests;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml2;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace Abc.IdentityModel.Tokens.Saml2.UnitTests {
    public class EncryptedSaml2SerializerFixture {
        [Fact()]
        public void WriteAssertionTest() {

        }

        [Theory, MemberData(nameof(ReadAssertionTheoryData))]
        public void ReadEncryptedAssertion(Saml2TheoryData theoryData) {
            var reader = XmlUtilities.CreateXmlReader(theoryData.Xml);
            var assertion = (theoryData.Saml2Serializer as EncryptedSaml2Serializer).ReadAssertion(reader, theoryData.ValidationParameters);
        }

        [Theory, MemberData(nameof(ReadAssertionTheoryData))]
        public void ReadAssertionUsingDictionaryReader(Saml2TheoryData theoryData) {
            var reader = XmlUtilities.CreateDictionaryReader(theoryData.Xml);
            var assertion = (theoryData.Saml2Serializer as EncryptedSaml2Serializer).ReadAssertion(reader, theoryData.ValidationParameters);
        }

        [Theory, MemberData(nameof(ReadAssertionTheoryData))]
        public void ReadAssertionUsingXDocumentReader(Saml2TheoryData theoryData) {
            var reader = XmlUtilities.CreateXDocumentReader(theoryData.Xml);
            var assertion = (theoryData.Saml2Serializer as EncryptedSaml2Serializer).ReadAssertion(reader, theoryData.ValidationParameters);
        }

        public static TheoryData<Saml2TheoryData> ReadAssertionTheoryData {
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
                    new Saml2TheoryData
                    {
                        //First = true,
                        Assertion = new Saml2Assertion(new Saml2NameIdentifier(Default.Issuer)),
                        Xml = ReferenceTokens.EncryptedSaml2Token_Valid,
                        //ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13102", typeof(XmlReadException)),
                        Saml2Serializer = new EncryptedSaml2Serializer(),
                        ValidationParameters = validationParameters,
                        TestId = "ReadEncryptedSaml2Assertion"
                    },
                };
            }
        }
    }
}