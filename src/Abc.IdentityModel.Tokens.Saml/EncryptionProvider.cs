namespace Abc.IdentityModel.Tokens {
    using Microsoft.IdentityModel.Tokens;
    using System;

    public class EncryptionProvider : AuthenticatedEncryptionProvider, ICryptoProvider {
        public EncryptionProvider(SecurityKey key, string algorithm) 
            : base(key, SecurityAlogithms.Aes) {
        }

        public bool IsSupportedAlgorithm(string algorithm, params object[] args) {
            throw new NotImplementedException();
        }

        public object Create(string algorithm, params object[] args) {
            return new EncryptionProvider((SecurityKey)args[0], algorithm);
        }

        public void Release(object cryptoInstance) {
            throw new NotImplementedException();
        }

        public override AuthenticatedEncryptionResult Encrypt(byte[] plaintext, byte[] authenticatedData, byte[] iv) {
            if (plaintext == null || plaintext.Length == 0) {
                throw LogHelper.LogArgumentNullException(nameof(plaintext));
            }

            //if (_disposed)
            //    throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));

            return EncryptWithAesCbc(plaintext, iv);
        }

        public override byte[] Decrypt(byte[] ciphertext, byte[] authenticatedData, byte[] iv, byte[] authenticationTag) {
            if (ciphertext == null || ciphertext.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(ciphertext));

            if (iv == null || iv.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(iv));

            //if (_disposed)
            //    throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));

            return DecryptWithAesCbc(ciphertext, iv);
        }

        private AuthenticatedEncryptionResult EncryptWithAesCbc(byte[] plaintext, byte[] iv) {
            using Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _authenticatedkeys.Value.AesKey.Key;
            if (iv != null) {
                aes.IV = iv;
            }

            byte[] ciphertext;
            try {
                ciphertext = Transform(aes.CreateEncryptor(), plaintext, 0, plaintext.Length);
            }
            catch (Exception ex) {
                throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant(LogMessages.IDX10654, ex)));
            }

            byte[] al = Utility.ConvertToBigEndian(authenticatedData.Length * 8);
            byte[] macBytes = new byte[authenticatedData.Length + aes.IV.Length + ciphertext.Length + al.Length];
            Array.Copy(authenticatedData, 0, macBytes, 0, authenticatedData.Length);
            Array.Copy(aes.IV, 0, macBytes, authenticatedData.Length, aes.IV.Length);
            Array.Copy(ciphertext, 0, macBytes, authenticatedData.Length + aes.IV.Length, ciphertext.Length);
            Array.Copy(al, 0, macBytes, authenticatedData.Length + aes.IV.Length + ciphertext.Length, al.Length);
            byte[] macHash = _symmetricSignatureProvider.Value.Sign(macBytes);
            var authenticationTag = new byte[_authenticatedkeys.Value.HmacKey.Key.Length];
            Array.Copy(macHash, authenticationTag, authenticationTag.Length);

            return new AuthenticatedEncryptionResult(Key, ciphertext, aes.IV, authenticationTag);
        }

        private byte[] DecryptWithAesCbc(byte[] ciphertext, byte[] iv) {
            // Verify authentication Tag
            byte[] al = Utility.ConvertToBigEndian(authenticatedData.Length * 8);
            byte[] macBytes = new byte[authenticatedData.Length + iv.Length + ciphertext.Length + al.Length];
            Array.Copy(authenticatedData, 0, macBytes, 0, authenticatedData.Length);
            Array.Copy(iv, 0, macBytes, authenticatedData.Length, iv.Length);
            Array.Copy(ciphertext, 0, macBytes, authenticatedData.Length + iv.Length, ciphertext.Length);
            Array.Copy(al, 0, macBytes, authenticatedData.Length + iv.Length + ciphertext.Length, al.Length);
            if (!_symmetricSignatureProvider.Value.Verify(macBytes, authenticationTag, _authenticatedkeys.Value.HmacKey.Key.Length))
                throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant(LogMessages.IDX10650, Base64UrlEncoder.Encode(authenticatedData), Base64UrlEncoder.Encode(iv), Base64UrlEncoder.Encode(authenticationTag))));

            using Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _authenticatedkeys.Value.AesKey.Key;
            aes.IV = iv;
            try {
                return Transform(aes.CreateDecryptor(), ciphertext, 0, ciphertext.Length);
            }
            catch (Exception ex) {
                throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant(LogMessages.IDX10654, ex)));
            }
        }

        internal static byte[] Transform(ICryptoTransform transform, byte[] input, int inputOffset, int inputLength) {
            if (transform.CanTransformMultipleBlocks)
                return transform.TransformFinalBlock(input, inputOffset, inputLength);

            using (var messageStream = new MemoryStream()) {
                using (var cryptoStream = new CryptoStream(messageStream, transform, CryptoStreamMode.Write)) {
                    cryptoStream.Write(input, inputOffset, inputLength);
                    cryptoStream.FlushFinalBlock();
                    return messageStream.ToArray();
                }
            }
        }
    }
}
