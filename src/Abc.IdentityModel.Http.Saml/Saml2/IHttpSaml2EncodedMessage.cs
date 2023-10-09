namespace Abc.IdentityModel.Protocols.Saml2 {
    using Abc.IdentityModel.Http;

    /// <summary>
    /// The SAML2 Encoded message;
    /// </summary>
    internal interface IHttpSaml2EncodedMessage : IHttpEncodedMessage {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IHttpSaml2EncodedMessage"/> is decoded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if decoded; otherwise, <c>false</c>.
        /// </value>
        bool Decoded { get; set; }
    }
}