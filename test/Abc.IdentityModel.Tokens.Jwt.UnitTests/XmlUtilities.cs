using System.Text;
using System.Xml;

namespace Abc.IdentityModel.Tokens.Jwt.UnitTests {
    internal static class XmlUtilities {
        /// <summary>
        /// This XmlReader when wrapped as an XmlDictionaryReader will not be able to Canonicalize.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static XmlReader CreateXmlReader(string xml) {
            if (string.IsNullOrEmpty(xml))
                return null;

            return new XmlTextReader(new StringReader(xml));
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
    }
}
