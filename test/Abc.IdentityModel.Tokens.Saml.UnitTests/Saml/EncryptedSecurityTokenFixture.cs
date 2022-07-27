using Abc.IdentityModel.Tokens.UnitTests;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml;
using System;
using Xunit;

namespace Abc.IdentityModel.Tokens.Saml.UnitTests {
    public class EncryptedSecurityTokenFixture
    {
        private SamlSecurityToken token = ReferenceSaml.SecurityToken;
        private X509EncryptingCredentials encryptingCredentials;

        public EncryptedSecurityTokenFixture() {
            encryptingCredentials = new X509EncryptingCredentials(Default.CertSelfSigned2048_SHA256_Public);
        }

        [Fact]
        public void InvalidConstructor() {
            Assert.Throws<ArgumentNullException>(() => new EncryptedSecurityToken(null, encryptingCredentials));
            Assert.Throws<ArgumentNullException>(() => new EncryptedSecurityToken(token, null));
        }

        [Fact]
        public void DefaultConstructor() {
            var target = new EncryptedSecurityToken(token, encryptingCredentials);

            Assert.Equal(target.Id, token.Id);
            Assert.Equal(target.Issuer, token.Issuer);
            Assert.Equal(target.SecurityKey, token.SecurityKey);
            Assert.Equal(target.SigningKey, token.SigningKey);
            Assert.Equal(target.ValidFrom, token.ValidFrom);
            Assert.Equal(target.ValidTo, token.ValidTo);
            Assert.NotNull(target.Token);
            Assert.NotNull(target.EncryptingCredentials);
        }
    }
}