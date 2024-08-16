// ----------------------------------------------------------------------------
// <copyright file="EncryptionSerializer.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Xml {
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Xml;
    using System;
    using System.Xml;
    using static Microsoft.IdentityModel.Logging.LogHelper;

    /// <summary>
    /// Serializes and deserializes <see cref="EncryptedData"/> objects into and from XML documents.
    /// </summary>
    public class EncryptionSerializer {
        private static EncryptionSerializer encryptionSerializer;
        private DSigSerializer dsigSerializer;

        static EncryptionSerializer() {
            Default = new EncryptionSerializer();
        }

        /// <summary>
        /// Gets or sets the default <see cref="EncryptionSerializer" /> instance.
        /// </summary>
        public static EncryptionSerializer Default {
            get => encryptionSerializer;
            set => encryptionSerializer = value ?? throw LogArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the <see cref="DSigSerializer"/> to use for reading/writing the <see cref="Signature"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">if value is null.</exception>
        /// <remarks>Will be passed to readers that process xmlDsig such as <see cref="EnvelopedSignatureReader"/> and <see cref="EnvelopedSignatureWriter"/>.</remarks>
        public DSigSerializer DSigSerializer {
            get {
                if (dsigSerializer == null) {
                    dsigSerializer = new EncryptedKeyDSigSerialzier(this);
                }

                return dsigSerializer;
            }

            set {
                dsigSerializer = value ?? throw LogArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Determines whether a URI is valid and can be created using the specified UriKind.
        /// Uri.TryCreate is used here, which is more lax than Uri.IsWellFormedUriString.
        /// The reason we use this function is because IsWellFormedUriString will reject valid URIs if they are IPv6 or require escaping.
        /// </summary>
        /// <param name="uriString">The string to check.</param>
        /// <param name="uriKind">The type of URI (usually UriKind.Absolute)</param>
        /// <returns>True if the URI is valid, false otherwise.</returns>
        internal static bool CanCreateValidUri(string uriString, UriKind uriKind) {
            return Uri.TryCreate(uriString, uriKind, out Uri _);
        }

        /// <summary>
        /// Read the &lt;xenc:EncryptedData> element.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="EncryptedData"/> element.</param>
        /// <returns>A <see cref="EncryptedData"/> instance.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        public EncryptedData ReadEncryptedData(XmlReader reader) {
            if (reader is null)
                throw LogArgumentNullException(nameof(reader));

            if (!reader.IsStartElement(XmlEncryptionConstants.ElementNames.EncryptedData, XmlEncryptionConstants.Namespace))
                throw XmlUtil.LogReadException("LogMessages.IDX30011", XmlEncryptionConstants.Namespace, XmlEncryptionConstants.ElementNames.EncryptedData, reader.NamespaceURI, reader.LocalName);

            var encryptedData = new EncryptedData();

            ReadEncryptedType(reader, encryptedData);

            reader.ReadEndElement();

            return encryptedData;
        }

        /// <summary>
        /// Writes the &lt;xenc:EncryptedData> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="EncryptedData"/>.</param>
        /// <param name="encryptedData">The <see cref="EncryptedData"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptedData"/> is null.</exception>
        public void WriteEncryptedData(XmlWriter writer, EncryptedData encryptedData) {
            if (writer is null)
                throw LogArgumentNullException(nameof(writer));

            if (encryptedData is null)
                throw LogArgumentNullException(nameof(encryptedData));

            // <EncryptedData>
            writer.WriteStartElement(XmlEncryptionConstants.Prefix, XmlEncryptionConstants.ElementNames.EncryptedData, XmlEncryptionConstants.Namespace);

            // Write out encrypted type
            WriteEncryptedType(writer, encryptedData);

            // </EncryptedData>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Read the &lt;xenc:EncryptedKey> element.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="EncryptedKey"/> element.</param>
        /// <returns>A <see cref="EncryptedKey"/> instance.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        public EncryptedKey ReadEncryptedKey(XmlReader reader) {
            if (reader is null)
                throw LogArgumentNullException(nameof(reader));

            var encryptedKey = new EncryptedKey();

            if (reader.IsStartElement(XmlEncryptionConstants.ElementNames.EncryptedKey, XmlEncryptionConstants.Namespace)) {
                // @Recipient - optional
                encryptedKey.Recipient = reader.GetAttribute(XmlEncryptionConstants.AttributeNames.Recipient);

                // <EncryptedType>
                ReadEncryptedType(reader, encryptedKey);

                // <ReferenceList> 0-1
                reader.MoveToContent();
                if (reader.IsStartElement(XmlEncryptionConstants.ElementNames.ReferenceList, XmlEncryptionConstants.Namespace)) {
                    encryptedKey.ReferenceList = ReadReferenceList(reader);
                }

                // <CarriedKeyName> 0-1
                reader.MoveToContent();
                if (reader.IsStartElement(XmlEncryptionConstants.ElementNames.CarriedKeyName, XmlEncryptionConstants.Namespace)) {
                    encryptedKey.CarriedKeyName = reader.ReadElementContentAsString();
                }

                reader.ReadEndElement();
            }

            return encryptedKey;
        }

        /// <summary>
        /// Writes the &lt;xenc:EncryptedKey> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="EncryptedKey"/>.</param>
        /// <param name="encryptedKey">The <see cref="EncryptedKey"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptedKey"/> is null.</exception>
        public void WriteEncryptedKey(XmlWriter writer, EncryptedKey encryptedKey) {
            if (writer is null)
                throw LogArgumentNullException(nameof(writer));

            if (encryptedKey is null)
                throw LogArgumentNullException(nameof(encryptedKey));

            // <EncryptedKey>
            writer.WriteStartElement(XmlEncryptionConstants.Prefix, XmlEncryptionConstants.ElementNames.EncryptedKey, XmlEncryptionConstants.Namespace);

            // @Recipient - optional
            if (encryptedKey.Recipient != null) {
                writer.WriteAttributeString(XmlEncryptionConstants.AttributeNames.Recipient, null, encryptedKey.Recipient);
            }

            // Write out encrypted type
            WriteEncryptedType(writer, encryptedKey);

            // Write out reference list
            if (encryptedKey.ReferenceList.KeyReferences.Count > 0 || encryptedKey.ReferenceList.DataReferences.Count > 0) {
                WriteReferenceList(writer, encryptedKey.ReferenceList);
            }

            // Write out carrier key name if there is one
            if (encryptedKey.CarriedKeyName != null) {
                writer.WriteElementString(XmlEncryptionConstants.Prefix, XmlEncryptionConstants.ElementNames.CarriedKeyName, XmlEncryptionConstants.Namespace, encryptedKey.CarriedKeyName);
            }

            // </EncryptedKey>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Read the &lt;xenc:ReferenceList> element.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="ReferenceList"/> element.</param>
        /// <returns>A <see cref="ReferenceList"/> instance.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        protected ReferenceList ReadReferenceList(XmlReader reader) {
            if (reader is null)
                throw LogArgumentNullException(nameof(reader));

            XmlUtil.CheckReaderOnEntry(reader, XmlEncryptionConstants.ElementNames.ReferenceList, XmlEncryptionConstants.Namespace);

            var referenceList = new ReferenceList();

            reader.ReadStartElement(XmlEncryptionConstants.ElementNames.ReferenceList, XmlEncryptionConstants.Namespace);

            if (reader.IsStartElement(XmlEncryptionConstants.ElementNames.DataReference, XmlEncryptionConstants.Namespace)) {
                while (reader.IsStartElement()) {
                    if (reader.IsStartElement(XmlEncryptionConstants.ElementNames.DataReference, XmlEncryptionConstants.Namespace)) {
                        string attribute = reader.GetAttribute(XmlEncryptionConstants.AttributeNames.Uri);
                        if (!string.IsNullOrEmpty(attribute)) {
                            var dataRefernce = new Uri(attribute);
                            if (referenceList.DataReferences.Contains(dataRefernce)) {
                                // throw ex
                            }

                            referenceList.DataReferences.Add(dataRefernce);
                        }

                        reader.Skip();
                    }
                    else {
                        if (reader.IsStartElement(XmlEncryptionConstants.ElementNames.KeyReference, XmlEncryptionConstants.Namespace)) {
                            //throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, "ID4189");
                        }

                        string str2 = reader.ReadOuterXml();
                    }
                }

                if (referenceList.DataReferences.Count == 0) {
                    // throw ex;
                }
            }
            else {
                if (!reader.IsStartElement(XmlEncryptionConstants.ElementNames.KeyReference, XmlEncryptionConstants.Namespace)) {
                    //throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, "ID4191");
                }

                while (reader.IsStartElement()) {
                    if (reader.IsStartElement(XmlEncryptionConstants.ElementNames.KeyReference, XmlEncryptionConstants.Namespace)) {
                        string attribute = reader.GetAttribute(XmlEncryptionConstants.AttributeNames.Uri);
                        var keyReference = new Uri(attribute);
                        if (referenceList.KeyReferences.Contains(keyReference)) {
                            // throw ex
                        }

                        referenceList.KeyReferences.Add(keyReference);

                        reader.Skip();
                    }
                    else {
                        if (reader.IsStartElement(XmlEncryptionConstants.ElementNames.DataReference, XmlEncryptionConstants.Namespace)) {
                            //throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, "ID4190");
                        }

                        string str4 = reader.ReadOuterXml();
                    }
                }

                if (referenceList.KeyReferences.Count == 0) {
                    // throw ex;
                }
            }

            reader.ReadEndElement();

            return referenceList;
        }

        /// <summary>
        /// Writes the &lt;xenc:ReferenceList> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="ReferenceList"/>.</param>
        /// <param name="referenceList">The <see cref="ReferenceList"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="referenceList"/> is null.</exception>
        protected void WriteReferenceList(XmlWriter writer, ReferenceList referenceList) {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (referenceList == null)
                throw LogArgumentNullException(nameof(referenceList));

            // <ReferenceList>
            writer.WriteStartElement(XmlEncryptionConstants.Prefix, XmlEncryptionConstants.ElementNames.ReferenceList, XmlEncryptionConstants.Namespace);

            // <DataReference> 0-oo
            foreach (var item in referenceList.DataReferences) {
                if (item is null) {
                    continue;
                }

                writer.WriteStartElement(XmlEncryptionConstants.Prefix, XmlEncryptionConstants.ElementNames.DataReference, XmlEncryptionConstants.Namespace);
                writer.WriteAttributeString(XmlEncryptionConstants.AttributeNames.Uri, item.AbsoluteUri);
                writer.WriteEndElement();
            }

            // <KeyReference> 0-oo
            foreach (var item in referenceList.KeyReferences) {
                if (item is null) {
                    continue;
                }

                writer.WriteStartElement(XmlEncryptionConstants.Prefix, XmlEncryptionConstants.ElementNames.KeyReference, XmlEncryptionConstants.Namespace);
                writer.WriteAttributeString(XmlEncryptionConstants.AttributeNames.Uri, item.AbsoluteUri);
                writer.WriteEndElement();
            }

            // </ReferenceList>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Read the &lt;xenc:EncryptedType> element.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="EncryptedType"/> element.</param>
        /// <param name="encryptedType">A <see cref="EncryptedType"/> instance.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptedType"/> is null.</exception>
        protected void ReadEncryptedType(XmlReader reader, EncryptedType encryptedType) {
            if (reader is null)
                throw LogArgumentNullException(nameof(reader));

            if (encryptedType is null)
                throw LogArgumentNullException(nameof(encryptedType));

            // @Id
            encryptedType.Id = reader.GetAttribute(XmlEncryptionConstants.AttributeNames.Id);

            // @Type
            var str = reader.GetAttribute(XmlEncryptionConstants.AttributeNames.Type);
            encryptedType.Type = str != null ? new Uri(str) : null;

            // @MimeType
            encryptedType.MimeType = reader.GetAttribute(XmlEncryptionConstants.AttributeNames.MimeType);

            // @Encoding
            str = reader.GetAttribute(XmlEncryptionConstants.AttributeNames.Encoding);
            encryptedType.Encoding = str != null ? new Uri(str) : null;

            reader.ReadStartElement();

            // EncryptionMethod
            reader.MoveToContent();
            if (reader.IsStartElement(XmlEncryptionConstants.ElementNames.EncryptionMethod, XmlEncryptionConstants.Namespace)) {
                encryptedType.EncryptionMethod = ReadEncryptionMethod(reader);
            }

            // KeyInfo
            reader.MoveToContent();
            if (reader.IsStartElement(XmlSignatureConstants.Elements.KeyInfo, XmlSignatureConstants.Namespace)) {
                encryptedType.KeyInfo = this.DSigSerializer.ReadKeyInfo(reader);
            }

            // CipherData
            reader.MoveToContent();
            encryptedType.CipherData = ReadChiperData(reader);
        }

        /// <summary>
        /// Read the &lt;xenc:EncryptionMethod> element.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="EncryptionMethod"/> element.</param>
        /// <returns>A <see cref="EncryptionMethod"/> instance.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        protected EncryptionMethod ReadEncryptionMethod(XmlReader reader) {
            if (reader is null)
                throw LogArgumentNullException(nameof(reader));

            XmlUtil.CheckReaderOnEntry(reader, XmlEncryptionConstants.ElementNames.EncryptionMethod, XmlEncryptionConstants.Namespace);

            bool isEmptyElement = reader.IsEmptyElement;

            // Algorithm
            var attribute = reader.GetAttribute(XmlEncryptionConstants.AttributeNames.Algorithm);
            if (attribute == null) {
                throw LogReadException(LogMessages.IDX51106, XmlEncryptionConstants.ElementNames.EncryptionMethod, XmlEncryptionConstants.AttributeNames.Algorithm);
            }

            if (!CanCreateValidUri(attribute, UriKind.Absolute)) {
                throw LogReadException(LogMessages.IDX51107, XmlEncryptionConstants.ElementNames.EncryptionMethod, XmlEncryptionConstants.AttributeNames.Algorithm, attribute);
            }

            var encryptionMethod = new EncryptionMethod(new Uri(attribute));

            reader.Read();
            if (!isEmptyElement) {
                // KeySize
                if (reader.IsStartElement(XmlEncryptionConstants.ElementNames.KeySize, XmlEncryptionConstants.Namespace)) {
                    encryptionMethod.KeySize = reader.ReadElementContentAsInt();
                }

                // OAEPparams
                if (reader.IsStartElement(XmlEncryptionConstants.ElementNames.OaepParams, XmlEncryptionConstants.Namespace)) {
                    encryptionMethod.OaepParams = reader.ReadElementContentAsBase64();
                }

                while (reader.IsStartElement()) {
                    // MFG
                    if (reader.IsStartElement(XmlEncryption11Constants.ElementNames.MaskGenerationFunction, XmlEncryption11Constants.Namespace)) {
                        reader.ReadStartElement(XmlEncryption11Constants.ElementNames.MaskGenerationFunction, XmlEncryption11Constants.Namespace);
                        var str = reader.GetAttribute(XmlEncryption11Constants.AttributeNames.Algorithm);
                        if (string.IsNullOrEmpty(str)) {
                            //throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2006Format(XmlEncryption11Constants.AttributeNames.Algorithm));
                        }

                        encryptionMethod.MaskGenerationFunction = new Uri(str);

                        reader.ReadEndElement();
                        continue;
                    }

                    reader.Skip();
                }

                reader.ReadEndElement();
            }

            return encryptionMethod;
        }

        /// <summary>
        /// Writes the &lt;xenc:EncryptedType> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="EncryptedType"/>.</param>
        /// <param name="encryptedType">The <see cref="EncryptedType"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptedType"/> is null.</exception>
        protected void WriteEncryptedType(XmlWriter writer, EncryptedType encryptedType) {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (encryptedType == null)
                throw LogArgumentNullException(nameof(encryptedType));

            // @Id - optional
            if (!string.IsNullOrEmpty(encryptedType.Id))
                writer.WriteAttributeString(XmlEncryptionConstants.AttributeNames.Id, encryptedType.Id);

            // @Type - optional
            if (encryptedType.Type != null)
                writer.WriteAttributeString(XmlEncryptionConstants.AttributeNames.Type, encryptedType.Type.AbsoluteUri);

            // @MimeType - optional
            if (encryptedType.MimeType != null)
                writer.WriteAttributeString(XmlEncryptionConstants.AttributeNames.MimeType, encryptedType.MimeType);

            // @Encoding - optional
            if (encryptedType.Encoding != null)
                writer.WriteAttributeString(XmlEncryptionConstants.AttributeNames.Encoding, encryptedType.Encoding.AbsoluteUri);

            // Write out encryption method
            if (encryptedType.EncryptionMethod != null)
                WriteEncryptionMethod(writer, encryptedType.EncryptionMethod);

            if (encryptedType.KeyInfo != null) {
                DSigSerializer.WriteKeyInfo(writer, encryptedType.KeyInfo);
            }

            WriteChiperData(writer, encryptedType.CipherData);
        }

        /// <summary>
        /// Writes the &lt;xenc:EncryptionMethod> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="EncryptionMethod"/>.</param>
        /// <param name="encryptionMethod">The <see cref="EncryptionMethod"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptionMethod"/> is null.</exception>
        protected void WriteEncryptionMethod(XmlWriter writer, EncryptionMethod encryptionMethod) {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (encryptionMethod == null)
                throw LogArgumentNullException(nameof(encryptionMethod));

            writer.WriteStartElement(XmlEncryptionConstants.Prefix, XmlEncryptionConstants.ElementNames.EncryptionMethod, XmlEncryptionConstants.Namespace);
            writer.WriteAttributeString(XmlEncryptionConstants.AttributeNames.Algorithm, null, encryptionMethod.Algorithm.AbsoluteUri);

            if (encryptionMethod.KeySize != null) {
                writer.WriteElementString(XmlEncryptionConstants.Prefix, XmlEncryptionConstants.ElementNames.KeySize, XmlEncryptionConstants.Namespace, XmlConvert.ToString(encryptionMethod.KeySize.Value));
            }

            if (encryptionMethod.OaepParams != null) {
                writer.WriteStartElement(XmlEncryptionConstants.Prefix, XmlEncryptionConstants.ElementNames.OaepParams, XmlEncryptionConstants.Namespace);
                writer.WriteBase64(encryptionMethod.OaepParams, 0, encryptionMethod.OaepParams.Length);
                writer.WriteEndElement();
            }

            if (encryptionMethod.Algorithm == new Uri(SecurityAlgorithms.RsaOaepKeyWrap)
                && encryptionMethod.MaskGenerationFunction != null) {
                writer.WriteStartElement(XmlEncryption11Constants.Prefix, XmlEncryption11Constants.ElementNames.MaskGenerationFunction, XmlEncryption11Constants.Namespace);
                writer.WriteAttributeString(XmlEncryption11Constants.AttributeNames.Algorithm, encryptionMethod.MaskGenerationFunction.AbsoluteUri);
                writer.WriteEndElement();
            }

            if (encryptionMethod.Algorithm == new Uri(SecurityAlgorithms.RsaOaepKeyWrap)
                //|| encryptionMethod.Algorithm == new Uri(SecurityAlgorithms.RsaOaepMgf1Sha1KeyWrap)
                || encryptionMethod.Algorithm == new Uri("http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p")
                ) {
                writer.WriteStartElement(XmlSignatureConstants.PreferredPrefix, XmlSignatureConstants.Elements.DigestMethod, XmlSignatureConstants.Namespace);
                writer.WriteAttributeString(XmlSignatureConstants.Attributes.Algorithm, "http://www.w3.org/2000/09/xmldsig#sha1");
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Read the &lt;xenc:CipherData> element.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="CipherData"/> element.</param>
        /// <returns>A <see cref="CipherData"/> instance.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> is null.</exception>
        protected virtual CipherData ReadChiperData(XmlReader reader) {
            if (reader is null)
                throw LogArgumentNullException(nameof(reader));

            reader.MoveToContent();

            XmlUtil.CheckReaderOnEntry(reader, XmlEncryptionConstants.ElementNames.CipherData, XmlEncryptionConstants.Namespace);

            byte[] chiperValue = null;

            reader.ReadStartElement(XmlEncryptionConstants.ElementNames.CipherData, XmlEncryptionConstants.Namespace);
            if (reader.IsStartElement(XmlEncryptionConstants.ElementNames.CipherValue, XmlEncryptionConstants.Namespace)) {
                chiperValue = reader.ReadElementContentAsBase64();
            }
            else if (reader.IsStartElement(XmlEncryptionConstants.ElementNames.CipherReference, XmlEncryptionConstants.Namespace)) {
                // do not support
                reader.Skip();
            }

            reader.MoveToContent();
            reader.ReadEndElement();

            return new CipherData(chiperValue);
        }

        /// <summary>
        /// Writes the &lt;xenc:CipherData> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="CipherData"/>.</param>
        /// <param name="cipherData">The <see cref="CipherData"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="cipherData"/> is null.</exception>
        protected virtual void WriteChiperData(XmlWriter writer, CipherData cipherData) {
            if (writer is null)
                throw LogArgumentNullException(nameof(writer));

            if (cipherData is null)
                throw LogArgumentNullException(nameof(cipherData));

            // <CipherData>
            writer.WriteStartElement(XmlEncryptionConstants.Prefix, XmlEncryptionConstants.ElementNames.CipherData, XmlEncryptionConstants.Namespace);

            // <CipherValue>
            writer.WriteStartElement(XmlEncryptionConstants.Prefix, XmlEncryptionConstants.ElementNames.CipherValue, XmlEncryptionConstants.Namespace);
            writer.WriteBase64(cipherData.CipherValue, 0, cipherData.CipherValue.Length);
            writer.WriteEndElement();

            // </CipherData>
            writer.WriteEndElement();
        }

        internal static Exception LogReadException(string format, params object[] args) {
            return LogExceptionMessage(new EncryptedDataReadException(FormatInvariant(format, args)));
        }
    }
}