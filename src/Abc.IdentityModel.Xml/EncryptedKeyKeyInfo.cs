namespace Abc.IdentityModel.Xml {
    using Microsoft.IdentityModel.Xml;
    using static Microsoft.IdentityModel.Logging.LogHelper;

    public class EncryptedKeyKeyInfo : KeyInfo {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedKeyKeyInfo"/> class.
        /// </summary>
        /// <param name="encryptedKey">The encrypted key.</param>
        public EncryptedKeyKeyInfo(EncryptedKey encryptedKey) {
            this.EncryptedKey = encryptedKey ?? throw LogArgumentNullException(nameof(encryptedKey));
        }

        /// <summary>
        /// Gets the encrypted key.
        /// </summary>
        /// <value>
        /// The encrypted key.
        /// </value>
        public EncryptedKey EncryptedKey { get; }
    }
}
