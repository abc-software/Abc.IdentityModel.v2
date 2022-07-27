namespace Abc.IdentityModel.Tokens {
    /// <summary>
    /// Log messages and codes
    /// </summary>
    /// <remarks>
    /// Range: 50000 - 50999
    /// </remarks>
    internal static class LogMessages {
#pragma warning disable 1591
        // token validation
        public const string IDX50400 = "IDX50400: The '{0}', can only process SecurityTokens of type: '{1}'. The SecurityToken received is of type: '{2}'.";
        public const string IDX50209 = "IDX50209: Token has length: '{0}' which is larger than the MaximumTokenSizeInBytes: '{1}'.";
        public const string IDX50254 = "IDX50254: '{0}.{1}' failed. The virtual method '{2}.{3}' returned null. If this method was overridden, ensure a valid '{4}' is returned.";

        // encryption/decryption
        public const string IDX50600 = "IDX50600: Unable to obtain a CryptoProviderFactory, both EncryptingCredentials.CryptoProviderFactory and EncryptingCredentials.Key.CrypoProviderFactory are null.";
        public const string IDX50601 = "IDX50601: Saml2Assertion encryption failed. No support for algorithm: '{0}', SecurityKey: '{1}'.";
        public const string IDX50602 = "IDX50602: Saml2Assertion decryption failed. No support for algorithm: '{0}', SecurityKey: '{1}'.";
        public const string IDX50617 = "IDX50617: Encryption failed. Keywrap is only supported for: '{0}', '{1}' and '{2}'. The content encryption specified is: '{3}'.";
        public const string IDX50618 = "IDX50618: EncryptedData->KeyInfo->EncryptedKey is missing.";
        public const string IDX50621 = "IDX50621: Unable to obtain a CryptoProviderFactory, both key.CryptoProviderFactory and ValidationParameters.CrypoProviderFactory are null.";

#pragma warning restore 1591
    }
}
