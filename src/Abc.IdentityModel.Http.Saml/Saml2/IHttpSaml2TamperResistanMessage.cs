namespace Abc.IdentityModel.Protocols.Saml2 {
    using System;

    internal interface IHttpSaml2TamperResistanMessage {
        /// <summary>
        /// Gets or sets the message signature.
        /// </summary>
        byte[] Signature { get; set; }

        /// <summary>
        /// Gets or sets the signature algorithm used to sign the message.
        /// </summary>
        string SignatureAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        string Data { get; set; }

        /// <summary>
        /// Gets or sets the signature object.
        /// </summary>
        object SignatureObject { get; set; }
    }
}