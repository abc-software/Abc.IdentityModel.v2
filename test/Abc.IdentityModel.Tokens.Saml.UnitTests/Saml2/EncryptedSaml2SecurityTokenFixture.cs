using Abc.IdentityModel.Tokens.UnitTests;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml2;
using Xunit;
using System;

namespace Abc.IdentityModel.Tokens.Saml2.UnitTests {
    public class EncryptedSaml2SecurityTokenFixture
    {
        private X509EncryptingCredentials encryptingCredentials;
        private Saml2SecurityToken token = ReferenceSaml2.SecurityToken;

        public EncryptedSaml2SecurityTokenFixture() {
            encryptingCredentials = new X509EncryptingCredentials(Default.CertSelfSigned2048_SHA256_Public);
        }

        [Fact]
        public void IvalidConstructor() {
            Assert.Throws<ArgumentNullException>(() => new EncryptedSaml2SecurityToken(null, encryptingCredentials));
            Assert.Throws<ArgumentNullException>(() => new EncryptedSaml2SecurityToken(token, null));
        }

        [Fact]
        public void DefaultConstructor() {
            var target = new EncryptedSaml2SecurityToken(token, encryptingCredentials);

            Assert.That(target.Id, Is.EqualTo(token.Id));
            Assert.That(target.Issuer, Is.EqualTo(token.Issuer));
            Assert.That(target.SecurityKey, Is.EqualTo(token.SecurityKey));
            Assert.That(target.SigningKey, Is.EqualTo(token.SigningKey));
            Assert.That(target.ValidFrom, Is.EqualTo(token.ValidFrom));
            Assert.That(target.ValidTo, Is.EqualTo(token.ValidTo));
            Assert.That(target.Token, Is.Not.Null);
            Assert.That(target.EncryptingCredentials, Is.Not.Null);
        }

        [Fact]
        public void GetSet_Invalid() {
            var target = new EncryptedSaml2SecurityToken(token, encryptingCredentials);

            Assert.Throws<NotSupportedException>(() => target.SigningKey = null);
        }
    }
}