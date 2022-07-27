using NUnit.Framework;
using Abc.IdentityModel.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abc.IdentityModel.Xml.UnitTests {
    [TestFixture]
    public class EncryptedKeyKeyInfoFixture {
        [Test]
        public void Constructor() {
            Assert.Throws<ArgumentNullException>(() => new EncryptedKeyKeyInfo(null));
        }

        [Test]
        public void DefaultConstructor() {
            var target = new EncryptedKeyKeyInfo(new EncryptedKey());
            Assert.That(target.Id, Is.Null);
            Assert.That(target.KeyName, Is.Null);
            Assert.That(target.Prefix, Is.Empty);
            Assert.That(target.RetrievalMethodUri, Is.Null);
            Assert.That(target.RSAKeyValue, Is.Null);
            Assert.That(target.X509Data, Is.Not.Null);
            Assert.That(target.EncryptedKey, Is.Not.Null);
        }
    }
}