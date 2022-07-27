using Microsoft.IdentityModel.Xml;
using NUnit.Framework;
using System;

namespace Abc.IdentityModel.Xml.UnitTests {
    [TestFixture]
    public class EncryptedKeyFixture {
        [Test]
        public void DefaultConstructor() {
            var target = new EncryptedKey();

            Assert.That(target.Id, Is.Null);
            Assert.That(target.Type, Is.Null);
            Assert.That(target.MimeType, Is.Null);
            Assert.That(target.Encoding, Is.Null);
            Assert.That(target.KeyInfo, Is.Null);
            Assert.That(target.EncryptionMethod, Is.Null);
            Assert.That(target.CipherData, Is.Null);
            Assert.That(target.CarriedKeyName, Is.Null);
            Assert.That(target.Recipient, Is.Null);
            Assert.That(target.ReferenceList, Is.Not.Null);
        }

        [Test]
        public void GetSet() {
            var target = new EncryptedKey() {
                Id = "id",
                Type = new Uri("urn:type"),
                MimeType = "mimeType",
                Encoding = new Uri("urn:encoding"),
                KeyInfo = new KeyInfo(),
                EncryptionMethod = new EncryptionMethod(new Uri("urn:method")),
                CipherData = new CipherData(new byte[0]),
                CarriedKeyName = "ckn",
                Recipient = "recipient",
            };

            Assert.That(target.Id, Is.EqualTo("id"));
            Assert.That(target.Type, Is.EqualTo(new Uri("urn:type")));
            Assert.That(target.MimeType, Is.EqualTo("mimeType"));
            Assert.That(target.Encoding, Is.EqualTo(new Uri("urn:encoding")));
            Assert.That(target.KeyInfo, Is.EqualTo(new KeyInfo()));
            //Assert.That(target.EncryptionMethod, Is.Null);
            //Assert.That(target.CipherData, Is.Null);
            Assert.That(target.CarriedKeyName, Is.EqualTo("ckn"));
            Assert.That(target.Recipient, Is.EqualTo("recipient"));
        }
    }
}
