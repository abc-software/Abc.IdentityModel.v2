using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Abc.IdentityModel.Tokens.Saml.UnitTests {
    public static class XmlUtilities {
        /// <summary>
        /// This XmlReader when wrapped as an XmlDictionaryReader will not be able to Canonicalize.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static XmlReader CreateXmlReader(string xml) {
            if (string.IsNullOrEmpty(xml))
                return null;

            return XmlReader.Create(new StringReader(xml), new XmlReaderSettings() { XmlResolver = null });
        }

        /// <summary>
        /// This XmlReader will be able to Canonicalize.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static XmlDictionaryReader CreateDictionaryReader(string xml) {
            if (string.IsNullOrEmpty(xml))
                return null;

            return XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(xml), XmlDictionaryReaderQuotas.Max);
        }

        public static XmlReader CreateXDocumentReader(string xml) {
            if (string.IsNullOrEmpty(xml))
                return null;

            return XDocument.Parse(xml).CreateReader();
        }
    }
}
