namespace Abc.IdentityModel.Xml {
    using Microsoft.IdentityModel.Xml;
    using System.Xml;
    using System.Xml.Linq;
    using static Microsoft.IdentityModel.Logging.LogHelper;

    /// <summary>
    /// Reads and writes XML conforming to https://www.w3.org/TR/2001/PR-xmldsig-core-20010820
    /// </summary>
    public class EncryptedKeyDSigSerialzier : DSigSerializer {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedKeyDSigSerialzier"/> class.
        /// </summary>
        /// <param name="encryptionSerializer">The encryption serializer.</param>
        public EncryptedKeyDSigSerialzier(EncryptionSerializer encryptionSerializer) {
            this.EncryptionSerializer = encryptionSerializer;
        }

        /// <summary>
        /// Returns the <see cref="EncryptionSerializer"/>.
        /// </summary>
        public EncryptionSerializer EncryptionSerializer { get; }

        ///<inheritdoc/>
        public override void WriteKeyInfo(XmlWriter writer, KeyInfo keyInfo) {
            if (keyInfo is EncryptedKeyKeyInfo encryptedKeyInfo) {
                writer.WriteStartElement(XmlSignatureConstants.Elements.KeyInfo, XmlSignatureConstants.Namespace);
                EncryptionSerializer.WriteEncryptedKey(writer, encryptedKeyInfo.EncryptedKey);
                writer.WriteEndElement();
                return;
            }

            base.WriteKeyInfo(writer, keyInfo);
        }

        ///<inheritdoc/>
        public override KeyInfo ReadKeyInfo(XmlReader reader) {
            XmlUtil.CheckReaderOnEntry(reader, XmlSignatureConstants.Elements.KeyInfo, XmlSignatureConstants.Namespace);

            // not possible read KeyInfo elements make a copy of node
            var doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            var node = doc.ReadNode(reader);

            {
                // return EncryptedKeyKeyInfo if first <KeyInfo> element <EncryptedKey>
                var inner = new XmlNodeReader(node);
                inner.ReadStartElement();
                if (inner.IsStartElement(XmlEncryptionConstants.ElementNames.EncryptedKey, XmlEncryptionConstants.Namespace)) {
                    return new EncryptedKeyKeyInfo(EncryptionSerializer.ReadEncryptedKey(inner));
                }
            }

            return base.ReadKeyInfo(new XmlNodeReader(node));
        }
    }
}
