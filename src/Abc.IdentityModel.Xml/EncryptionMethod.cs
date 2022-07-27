namespace Abc.IdentityModel.Xml {
    using System;
    using static Microsoft.IdentityModel.Logging.LogHelper;

    /// <summary>
    /// Represents the <see cref="EncryptionMethod"/> element in XML encryption that describes the encryption algorithm applied to the cipher data. This class cannot be inherited.
    /// </summary>
    /// <remarks>http://www.w3.org/TR/xmlenc-core/#sec-EncryptionMethod</remarks>
    public class EncryptionMethod {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionMethod"/> class with logarithm.
        /// </summary>
        /// <param name="algorithm">The algorithm Uniform Resource Identifier(URI)</param>
        public EncryptionMethod(Uri algorithm) {
            this.Algorithm = algorithm ?? throw LogArgumentNullException(nameof(algorithm));
        }

        /// <summary>
        /// Gets or sets the algorithm Uniform Resource Identifier(URI).
        /// </summary>
        /// <value>
        /// The algorithm Uniform Resource Identifier(URI).
        /// </value>
        public Uri Algorithm { get; } 

        /// <summary>
        /// Gets or sets the size of the key.
        /// </summary>
        /// <value>
        /// The size of the key.
        /// </value>
        public int? KeySize { get; set; }

        /// <summary>
        /// Gets or sets the OAEP parameters.
        /// </summary>
        /// <value>
        /// The OAEP parameters.
        /// </value>
        public byte[] OaepParams { get; set; }

        /// <summary>
        /// Gets or sets the mask generation function.
        /// </summary>
        /// <value>
        /// The mask generation function.
        /// </value>
        public Uri MaskGenerationFunction { get; set; }
    }
}
