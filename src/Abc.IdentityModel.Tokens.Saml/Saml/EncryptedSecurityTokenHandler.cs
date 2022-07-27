namespace Abc.IdentityModel.Tokens.Saml {
    using Abc.IdentityModel.Xml;
    using Microsoft.IdentityModel.JsonWebTokens;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Xml;
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml;
    using static Microsoft.IdentityModel.Logging.LogHelper;

    internal class EncryptedSecurityTokenHandler : SecurityTokenHandler {
        private EncryptionSerializer encryptionSerializer = EncryptionSerializer.Default;

        public EncryptedSecurityTokenHandler(SecurityTokenHandler securityTokenHandler) {
            this.WrappedSecurityTokenHandler = securityTokenHandler ?? throw new ArgumentNullException(nameof(securityTokenHandler));
        }

        public override Type TokenType => typeof(EncryptedSecurityToken);

        public SecurityTokenHandler WrappedSecurityTokenHandler { get; }

        public override bool CanReadToken(string tokenString) {
            if (string.IsNullOrWhiteSpace(tokenString) || tokenString.Length > MaximumTokenSizeInBytes) {
                return false;
            }

            try {
                using var input = new StringReader(tokenString);
                var settings = new XmlReaderSettings {
                    DtdProcessing = DtdProcessing.Prohibit
                };

                using var reader = XmlDictionaryReader.CreateDictionaryReader(XmlReader.Create(input, settings));
                return CanReadToken(reader);
            }
            catch (Exception) {
                return false;
            }
        }

        public override bool CanReadToken(XmlReader reader) {
            return reader?.IsStartElement(XmlEncryptionConstants.ElementNames.EncryptedData, XmlEncryptionConstants.Namespace) ?? false;
        }

        public override SecurityToken ReadToken(XmlReader reader, TokenValidationParameters validationParameters) {
            var buffer = ReadEncryptedToken(reader, validationParameters);
            using var dictionaryReader = XmlDictionaryReader.CreateTextReader(buffer, XmlDictionaryReaderQuotas.Max);
            return this.WrappedSecurityTokenHandler.ReadToken(dictionaryReader);
        }

        public override void WriteToken(XmlWriter writer, SecurityToken token) {
            if (writer is null) {
                throw LogArgumentNullException(nameof(writer));
            }

            if (token is null) {
                throw LogArgumentNullException(nameof(token));
            }

            var encryptedToken = token as EncryptedSecurityToken;
            if (encryptedToken == null) {
                throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX50400, MarkAsNonPII(nameof(EncryptedSecurityTokenHandler)), MarkAsNonPII(typeof(EncryptedSecurityToken)), MarkAsNonPII(token.GetType()))));
            }

            if (encryptedToken.Token is null) {
                throw LogArgumentNullException(nameof(encryptedToken.Token));
            }

            var encryptingCredentials = encryptedToken.EncryptingCredentials;
            if (encryptingCredentials == null) {
                throw LogArgumentNullException(nameof(encryptingCredentials));
            }

            //EncryptionExtension.Encrypt(encryptingCredentials, w => this.WrappedSecurityTokenHandler.WriteToken(w, encryptedToken.Token));

            var cryptoProviderFactory = encryptingCredentials.CryptoProviderFactory ?? encryptingCredentials.Key.CryptoProviderFactory;
            if (cryptoProviderFactory == null) {
                throw LogExceptionMessage(new ArgumentException(LogMessages.IDX50600));
            }

            byte[] wrappedKey = null;
            SecurityKey securityKey = GetSecurityKey(encryptingCredentials, cryptoProviderFactory, out wrappedKey);

            // Change keyWrapAlgorithm to URI
            string keyWrapAlgorithm = null;
            switch (encryptingCredentials.Alg) {
                case SecurityAlgorithms.RsaOAEP:
                case SecurityAlgorithms.RsaOaepKeyWrap:
                    keyWrapAlgorithm = "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p";
                    break;
            }

            var encryptedKey = new EncryptedKey {
                EncryptionMethod = new EncryptionMethod(new Uri(keyWrapAlgorithm)),
                CipherData = new CipherData(wrappedKey),
                KeyInfo = new KeyInfo(encryptingCredentials.Key),
            };

            byte[] plaintText;
            using (var memoryStream = new MemoryStream()) {
                using (var dictionaryWriter = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, ownsStream: false)) {
                    this.WrappedSecurityTokenHandler.WriteToken(dictionaryWriter, encryptedToken.Token);
                }

                plaintText = memoryStream.ToArray();
            }

            ////using (var encryptionProvider = cryptoProviderFactory.CreateAuthenticatedEncryptionProvider(securityKey, encryptingCredentials.Enc)) {
            ////if (encryptionProvider == null) {
            if (!SecurityAlgorithms.Aes128Encryption.Equals(encryptingCredentials.Enc, StringComparison.Ordinal)
                && !SecurityAlgorithms.Aes192Encryption.Equals(encryptingCredentials.Enc, StringComparison.Ordinal)
                && !SecurityAlgorithms.Aes256Encryption.Equals(encryptingCredentials.Enc, StringComparison.Ordinal)) {
                throw LogExceptionMessage(new SecurityTokenEncryptionFailedException(FormatInvariant(LogMessages.IDX50601, encryptingCredentials.Enc, securityKey)));
            }

            //// var encryptionResult = encryptionProvider.Encrypt(plaintText, null);
            var encryptionResult = this.EncryptWithAesCbc(securityKey, plaintText, null);
            var encryptedData = new EncryptedData() {
                Type = XmlEncryptionConstants.EncryptedDataTypes.Element,
                CipherData = new CipherData(ConcatArrays(encryptionResult.IV, encryptionResult.Ciphertext)),
                EncryptionMethod = new EncryptionMethod(new Uri(encryptingCredentials.Enc)),
                KeyInfo = new EncryptedKeyKeyInfo(encryptedKey),
            };

            this.encryptionSerializer.WriteEncryptedData(writer, encryptedData);
            ////} // end encryptionProvider
        }

        public override ClaimsPrincipal ValidateToken(XmlReader reader, TokenValidationParameters validationParameters, out SecurityToken validatedToken) {
            if (reader is null) {
                throw LogArgumentNullException(nameof(reader));
            }

            if (validationParameters is null) {
                throw LogArgumentNullException(nameof(validationParameters));
            }

            var buffer = ReadEncryptedToken(reader, validationParameters);
            using var dictionaryReader = XmlDictionaryReader.CreateTextReader(buffer, XmlDictionaryReaderQuotas.Max);

            return this.WrappedSecurityTokenHandler.ValidateToken(dictionaryReader, validationParameters, out validatedToken);
        }

        public override ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken) {
            if (string.IsNullOrWhiteSpace(securityToken)) {
                throw LogArgumentNullException(nameof(securityToken));
            }

            if (validationParameters is null) {
                throw LogArgumentNullException(nameof(validationParameters));
            }

            if (securityToken.Length > MaximumTokenSizeInBytes) {
                throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX50209, securityToken.Length, MaximumTokenSizeInBytes)));
            }

            using var reader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(securityToken), XmlDictionaryReaderQuotas.Max);
            var buffer = ReadEncryptedToken(reader, validationParameters);

            return this.WrappedSecurityTokenHandler.ValidateToken(Encoding.UTF8.GetString(buffer), validationParameters, out validatedToken);
        }

        internal byte[] ReadEncryptedToken(XmlReader reader, TokenValidationParameters validationParameters) {
            if (reader is null) {
                throw LogArgumentNullException(nameof(reader));
            }

            if (validationParameters is null) {
                throw LogArgumentNullException(nameof(validationParameters));
            }

            var encryptedData = this.encryptionSerializer.ReadEncryptedData(reader);

            SecurityKey key = null;
            // Support only for a single key for now
            if (validationParameters.TokenDecryptionKeyResolver != null) {
                key = validationParameters.TokenDecryptionKeyResolver(null, null, encryptedData.KeyInfo.Id, validationParameters).FirstOrDefault();
            }
            else {
                key = validationParameters.TokenDecryptionKey;
            }

            var encryptedKey = (encryptedData.KeyInfo as EncryptedKeyKeyInfo)?.EncryptedKey;
            if (encryptedKey == null) {
                throw LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogMessages.IDX50618));
            }

            var cryptoProviderFactory = validationParameters.CryptoProviderFactory ?? key.CryptoProviderFactory;
            if (cryptoProviderFactory == null) {
                throw LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogMessages.IDX50621));
            }

            // Change keyWrapAlgorithm to URI
            string keyWrapAlgorithm = encryptedKey.EncryptionMethod.Algorithm.AbsoluteUri;
            switch (encryptedKey.EncryptionMethod.Algorithm.AbsoluteUri) {
                case "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p":
                    keyWrapAlgorithm = SecurityAlgorithms.RsaOaepKeyWrap;
                    break;
            }

            if (!cryptoProviderFactory.IsSupportedAlgorithm(keyWrapAlgorithm, key)) {
                throw LogExceptionMessage(new SecurityTokenEncryptionFailedException(FormatInvariant(LogMessages.IDX50602, encryptedKey.EncryptionMethod.Algorithm, key)));
            }

            var keyWrapProvider = cryptoProviderFactory.CreateKeyWrapProviderForUnwrap(key, keyWrapAlgorithm);
            var unwrappedKey = keyWrapProvider.UnwrapKey(encryptedKey.CipherData.CipherValue);
            var sessionKey = new SymmetricSecurityKey(unwrappedKey);

            //var decryptionProvider = cryptoProviderFactory.CreateAuthenticatedEncryptionProvider(sessionKey, encryptedData.EncryptionMethod.Algorithm.AbsoluteUri);
            //return decryptionProvider.Decrypt(encryptedData.CipherData.CipherValue, null, null, null);
            // TODO: validate encryption algorithm

            return DecryptWithAesCbc(sessionKey, encryptedData.CipherData.CipherValue);
        }

        internal static byte[] ConcatArrays(params byte[][] list) {
            int outputLength = 0;
            for (int i = 0; i < list.Length; i++) {
                outputLength += list[i].Length;
            }

            byte[] outputBytes = new byte[outputLength];

            int offset = 0;
            for (int i = 0; i < list.Length; i++) {
                Buffer.BlockCopy(list[i], 0, outputBytes, offset, list[i].Length);
                offset += list[i].Length;
            }

            return outputBytes;
        }

        internal static SecurityKey GetSecurityKey(EncryptingCredentials encryptingCredentials, CryptoProviderFactory cryptoProviderFactory, out byte[] wrappedKey) {
            wrappedKey = null;

            if (!cryptoProviderFactory.IsSupportedAlgorithm(encryptingCredentials.Alg, encryptingCredentials.Key)) {
                throw LogExceptionMessage(new SecurityTokenEncryptionFailedException(FormatInvariant(LogMessages.IDX50601, MarkAsNonPII(encryptingCredentials.Alg), encryptingCredentials.Key)));
            }

            byte[] keyBytes;
            // only 128, 384 and 512 AesCbc
            if (SecurityAlgorithms.Aes128Encryption.Equals(encryptingCredentials.Enc, StringComparison.Ordinal)) {
                keyBytes = JwtTokenUtilities.GenerateKeyBytes(128);
            }
            else if (SecurityAlgorithms.Aes192Encryption.Equals(encryptingCredentials.Enc, StringComparison.Ordinal)) {
                keyBytes = JwtTokenUtilities.GenerateKeyBytes(192);
            }
            else if (SecurityAlgorithms.Aes256Encryption.Equals(encryptingCredentials.Enc, StringComparison.Ordinal)) {
                keyBytes = JwtTokenUtilities.GenerateKeyBytes(256);
            }
            else {
                throw LogExceptionMessage(
                    new SecurityTokenEncryptionFailedException(FormatInvariant(LogMessages.IDX50617, MarkAsNonPII(SecurityAlgorithms.Aes128CbcHmacSha256), MarkAsNonPII(SecurityAlgorithms.Aes192CbcHmacSha384), MarkAsNonPII(SecurityAlgorithms.Aes256CbcHmacSha512), MarkAsNonPII(encryptingCredentials.Enc))));
            }

            var kwProvider = cryptoProviderFactory.CreateKeyWrapProvider(encryptingCredentials.Key, encryptingCredentials.Alg);
            wrappedKey = kwProvider.WrapKey(keyBytes);

            return new SymmetricSecurityKey(keyBytes);
        }

        private AuthenticatedEncryptionResult EncryptWithAesCbc(SecurityKey key, byte[] plainText, byte[] iv) {
            using Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = (key as SymmetricSecurityKey).Key;

            if (iv != null) {
                aes.IV = iv;
            }

            using ICryptoTransform transform = aes.CreateEncryptor();
            var cipherText = transform.TransformFinalBlock(plainText, 0, plainText.Length);

            return new AuthenticatedEncryptionResult(key, cipherText, aes.IV, null);
        }

        private byte[] DecryptWithAesCbc(SecurityKey key, byte[] cipherValue) {
            using Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.ISO10126;
            aes.Key = (key as SymmetricSecurityKey).Key;

            var iv = new byte[aes.BlockSize / 8];
            Buffer.BlockCopy(cipherValue, 0, iv, 0, iv.Length);

            aes.IV = iv;

            using ICryptoTransform transform = aes.CreateDecryptor();
            return transform.TransformFinalBlock(cipherValue, iv.Length, cipherValue.Length - iv.Length);
        }
    }
}
