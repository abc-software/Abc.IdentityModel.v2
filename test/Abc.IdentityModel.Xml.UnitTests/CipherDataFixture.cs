using NUnit.Framework;
using System;

namespace Abc.IdentityModel.Xml.UnitTests {
    [TestFixture]
    public class CipherDataFixture {

        [Test]
        public void DefaultConstructor() {
            var target = new CipherData(new byte[0]);
            Assert.That(target.CipherValue, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => new CipherData(null));
        }

    }
}
