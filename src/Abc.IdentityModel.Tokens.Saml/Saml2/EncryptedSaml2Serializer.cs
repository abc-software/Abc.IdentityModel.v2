// ----------------------------------------------------------------------------
// <copyright file="EncryptedSaml2Serializer.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Tokens.Saml2 {
    using Abc.IdentityModel.Tokens.Saml;
    using Abc.IdentityModel.Xml;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Tokens.Saml2;
    using System.Linq;
    using System.Xml;
    using static Microsoft.IdentityModel.Logging.LogHelper;

    public class EncryptedSaml2Serializer : Saml2Serializer {
        private EncryptionSerializer encryptionSerializer = EncryptionSerializer.Default;

        /// <summary>
        /// Gets or sets the <see cref="P:EncryptionSerializer" /> to use for reading/writing the <see cref="T:Abc.IdentityModel.Xml.EncryptedData" />
        /// </summary>
        /// <exception cref="T:ArgumentNullException">if value is null.</exception>
        public EncryptionSerializer EncryptionSerializer {
            get {
                return encryptionSerializer;
            }
            set {
                encryptionSerializer = value ?? throw LogArgumentNullException(nameof(value));
            }
        }

        /// <inheritdoc/>
        public override void WriteAssertion(XmlWriter writer, Saml2Assertion assertion) {
            if (assertion is EncryptedSaml2Assertion encryptedAssertion) {
                this.WriteEncryptedAssertion(writer, encryptedAssertion);
                return;
            }

            base.WriteAssertion(writer, assertion);
        }

        public Saml2Assertion ReadAssertion(XmlReader reader, TokenValidationParameters validationParameters) {
            if (reader.IsStartElement(Saml2Constants.Elements.EncryptedAssertion, Saml2Constants.Namespace)) {
                return this.ReadEncryptedAssertion(reader, validationParameters);
            }

            return base.ReadAssertion(reader);
        }
        protected EncryptedSaml2Assertion ReadEncryptedAssertion(XmlReader reader, TokenValidationParameters validationParameters) {
            reader.ReadStartElement(Saml2Constants.Elements.EncryptedAssertion, Saml2Constants.Namespace);
            var encryptedData = this.encryptionSerializer.ReadEncryptedData(reader);
            reader.ReadEndElement();

            SecurityKey key = null;
            // Support only for a single key for now
            if (validationParameters.TokenDecryptionKeyResolver != null) {
                key = validationParameters.TokenDecryptionKeyResolver(null, null, encryptedData.KeyInfo.Id, validationParameters).FirstOrDefault();
            }
            else {
                key = validationParameters.TokenDecryptionKey;
            }

            if (validationParameters.CryptoProviderFactory != null) {
                key.CryptoProviderFactory = validationParameters.CryptoProviderFactory;
            }

            var saml2Assertion = EncryptionExtension.Decrypt(encryptedData, key, r => base.ReadAssertion(r));

            return new EncryptedSaml2Assertion(saml2Assertion) {
                EncryptingCredentials = new EncryptingCredentials(
                    key, 
                    encryptedData.EncryptionMethod.Algorithm?.ToString(), 
                    (encryptedData.KeyInfo as EncryptedKeyKeyInfo)?.EncryptedKey?.EncryptionMethod?.Algorithm.ToString() ?? "http://www.w3.org/2001/04/xmlenc#rsa-oaep"
                ),
                EncryptedData = encryptedData,
            };
        }

        protected void WriteEncryptedAssertion(XmlWriter writer, EncryptedSaml2Assertion assertion) {
            if (writer is null) {
                throw LogArgumentNullException(nameof(writer));
            }

            if (assertion is null) {
                throw LogArgumentNullException(nameof(assertion));
            }

            var encryptingCredentials = assertion.EncryptingCredentials;
            if (encryptingCredentials == null) {
                throw LogArgumentNullException(nameof(encryptingCredentials));
            }

            var encryptedData = EncryptionExtension.Encrypt(encryptingCredentials, w => base.WriteAssertion(w, assertion));

            writer.WriteStartElement(Saml2Constants.Elements.EncryptedAssertion, Saml2Constants.Namespace);
            this.encryptionSerializer.WriteEncryptedData(writer, encryptedData);
            writer.WriteEndElement();
        }
    }
}
