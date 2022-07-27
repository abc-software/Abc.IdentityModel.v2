using Abc.IdentityModel.Tokens.Saml2;
using Abc.IdentityModel.Tokens.UnitTests;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml2;
using System;
using Xunit;

namespace Abc.IdentityModel.Tokens.Saml2.UnitTests {
    public class EncryptedSaml2AssertionFixture {
        private X509EncryptingCredentials encryptingCredentials;
        private Saml2SecurityToken token = ReferenceSaml2.SecurityToken;

        public EncryptedSaml2AssertionFixture() {
            encryptingCredentials = new X509EncryptingCredentials(Default.CertSelfSigned2048_SHA256_Public);
        }

        [Fact]
        public void IvalidConstructor() {
            Assert.Throws<ArgumentNullException>(() => new EncryptedSaml2Assertion((Saml2NameIdentifier)null));
            Assert.Throws<ArgumentNullException>(() => new EncryptedSaml2Assertion((Saml2Assertion)null));
        }

        [Fact]
        public void DefaultConstructor() {
            var assertion = token.Assertion;
            var target = new EncryptedSaml2Assertion(assertion) {
                EncryptingCredentials = encryptingCredentials,
            };

            Assert.Equal(target.Id, assertion.Id);
            Assert.Equal(target.Issuer, assertion.Issuer);
            Assert.Equal(target.IssueInstant, assertion.IssueInstant);
            Assert.Equal(target.SigningCredentials, assertion.SigningCredentials);
            Assert.Equal(target.Subject, assertion.Subject);
            Assert.Equal(target.Signature, assertion.Signature);
            Assert.Equal(target.Advice, assertion.Advice);
            Assert.Equal(target.CanonicalString, assertion.CanonicalString);
            Assert.Equal(target.Conditions, assertion.Conditions);
            Assert.Equal(target.Statements, assertion.Statements);
            Assert.Equal(target.Version, assertion.Version);
            Assert.NotNull(target.EncryptingCredentials);
        }
    }
}