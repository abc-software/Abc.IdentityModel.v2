using Microsoft.IdentityModel.Logging;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Abc.IdentityModel.Xml.UnitTests {
    [TestFixture()]
    public class EncryptionSerialzierFixture {
        [TestCaseSource("ReadEncryptedDataTestSource")]
        public void ReadWriteKeyInfoTest(EncryptedDataTestSet testSet) {
            IdentityModelEventSource.ShowPII = true;

            var target = new EncryptionSerializer();

            var encryptedData = target.ReadEncryptedData(XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(testSet.Xml), XmlDictionaryReaderQuotas.Max));
            //todo: Assert.That(encryptedData, Is.EqualTo(testSet.EncryptedData));

            var ms = new MemoryStream();
            var writer = XmlDictionaryWriter.CreateTextWriter(ms);
            target.WriteEncryptedData(writer, encryptedData);
            writer.Flush();
            var xml = Encoding.UTF8.GetString(ms.ToArray());
            target.ReadEncryptedData(XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(xml), XmlDictionaryReaderQuotas.Max));

            Assert.That(xml, Is.EqualTo(Normalize(testSet.Xml)));
        }

        public static IEnumerable<EncryptedDataTestSet> ReadEncryptedDataTestSource {
            get {
                yield return EncryptedDataTestSet.EncryptedDataFullyPopulated;
            }
        }

        private static string Normalize(string xml) {
            return Regex.Replace(xml, ">[\\s\\r\\n]*<", "><");
        }
    }
}