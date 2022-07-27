using NUnit.Framework;
using Abc.IdentityModel.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Microsoft.IdentityModel.Logging;
using System.Text.RegularExpressions;

namespace Abc.IdentityModel.Xml.UnitTests {
    [TestFixture()]
    public class EncryptedKeyDSigSerialzierFixture {
        [TestCaseSource("ReadKeyInfoTestSource")]
        public void ReadWriteKeyInfoTest(KeyInfoTestSet testSet) {
            IdentityModelEventSource.ShowPII = true;

            var target = new EncryptedKeyDSigSerialzier(new EncryptionSerializer());

            var keyInfo = target.ReadKeyInfo(XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(testSet.Xml), XmlDictionaryReaderQuotas.Max));
            Assert.That(keyInfo, Is.EqualTo(testSet.KeyInfo));

            var ms = new MemoryStream();
            var writer = XmlDictionaryWriter.CreateTextWriter(ms);
            target.WriteKeyInfo(writer, keyInfo);
            writer.Flush();
            var xml = Encoding.UTF8.GetString(ms.ToArray());
            target.ReadKeyInfo(XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(xml), XmlDictionaryReaderQuotas.Max));

            Assert.That(xml, Is.EqualTo(Normalize(testSet.Xml)));
        }

        public static IEnumerable<KeyInfoTestSet> ReadKeyInfoTestSource  {
            get {
                yield return KeyInfoTestSet.KeyInfoFullyPopulated;
                yield return KeyInfoTestSet.KeyInfoEncryptedKey;
            }
        }

        private static string Normalize(string xml) {
            return Regex.Replace(xml, ">[\\s\\r\\n]*<", "><");
        }
    }
}