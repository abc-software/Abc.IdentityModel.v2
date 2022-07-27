using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abc.IdentityModel.Xml.UnitTests {
    [TestFixture]
    public class EncryptionMethodFixture {
        [Test]
        public void Constructor() {
            Assert.Throws<ArgumentNullException>(() => new EncryptionMethod(null));
        }

        [Test]
        public void DefaultConstructor() {
            var target = new EncryptionMethod(new Uri("urn:uri"));

            Assert.That(target.Algorithm, Is.EqualTo(new Uri("urn:uri")));
            Assert.That(target.KeySize, Is.Null);
            Assert.That(target.MaskGenerationFunction, Is.Null);
            Assert.That(target.OaepParams, Is.Null);
        }

        [Test]
        public void GetSet() {
            var target = new EncryptionMethod(new Uri("urn:uri")) {
                KeySize = 256,
                MaskGenerationFunction = new Uri("urn:mask"),
                OaepParams = new byte[] { 1, 2 },
            };

            Assert.That(target.Algorithm, Is.EqualTo(new Uri("urn:uri")));
            Assert.That(target.KeySize, Is.EqualTo(256));
            Assert.That(target.MaskGenerationFunction, Is.EqualTo(new Uri("urn:mask")));
            Assert.That(target.OaepParams, Is.EqualTo(new byte[] { 1, 2 }));
        }

    }
}
