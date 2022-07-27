using Microsoft.IdentityModel.Xml;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abc.IdentityModel.Xml.UnitTests {
    [TestFixture]
    public class EncryptedDataFixture {

        [Test]
        public void DefaultConstructor() {
            var target = new EncryptedData();

            Assert.That(target.Id, Is.Null);
            Assert.That(target.Type, Is.Null);
            Assert.That(target.MimeType, Is.Null);
            Assert.That(target.Encoding, Is.Null);
            Assert.That(target.KeyInfo, Is.Null);
            Assert.That(target.EncryptionMethod, Is.Null);
            Assert.That(target.CipherData, Is.Null);
        }

        [Test]
        public void GetSet() {
            var target = new EncryptedData() {
                Id = "id",
                Type = new Uri("urn:type"),
                MimeType = "mimeType",
                Encoding = new Uri("urn:encoding"),
                KeyInfo = new KeyInfo(),
                EncryptionMethod = new EncryptionMethod(new Uri("urn:method")),
                CipherData = new CipherData(new byte[0]),
            };

            Assert.That(target.Id, Is.EqualTo("id"));
            Assert.That(target.Type, Is.EqualTo(new Uri("urn:type")));
            Assert.That(target.MimeType, Is.EqualTo("mimeType"));
            Assert.That(target.Encoding, Is.EqualTo(new Uri("urn:encoding")));
            Assert.That(target.KeyInfo, Is.EqualTo(new KeyInfo()));
            Assert.That(target.EncryptionMethod, Is.Not.Null);
            Assert.That(target.CipherData, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => new EncryptedData() { CipherData = null, });
        }
    }
}
