namespace Abc.IdentityModel.Protocols.Saml2 {
    using System;
#if AZUREAD
    using Microsoft.IdentityModel.Tokens;
#else
    using System.IdentityModel.Tokens;
#endif

    /// <summary>
    /// The <c>Saml2MessageSecurityKeyIdentifierClause</c> class represents a security key that is must be resolved by inspecting
    /// a <c>Saml2Message</c>.
    /// </summary>
    /// <remarks>
    /// This class is used when a <c>Saml2Message</c> is received without a KeyInfo inside the signature element.
    /// The KeyInfo describes the key required to check the signature.  When the key is needed this clause 
    /// will be presented to the current SecurityTokenResolver. It will contain the 
    /// <c>Saml2Message</c>c&gt; fully read which can be queried to determine the key required.
    /// </remarks>
    internal class Saml2MessageSecurityKeyIdentifierClause : SecurityKeyIdentifierClause {
        private readonly Saml2Message message;

#if AZUREAD
        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2MessageSecurityKeyIdentifierClause" /> class.
        /// </summary>
        /// <param name="message">The message can be queried to obtain information about 
        /// the issuer when resolving the key needed to check the signature. The message will
        /// be read completely when this clause is passed to the SecurityTokenResolver.</param>
        public Saml2MessageSecurityKeyIdentifierClause(Saml2Message message) {
            this.message = message;
        }
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2MessageSecurityKeyIdentifierClause" /> class.
        /// </summary>
        /// <param name="message">The message can be queried to obtain information about 
        /// the issuer when resolving the key needed to check the signature. The message will
        /// be read completely when this clause is passed to the SecurityTokenResolver.</param>
        public Saml2MessageSecurityKeyIdentifierClause(Saml2Message message)
            : base(typeof(Saml2MessageSecurityKeyIdentifierClause).ToString()) {
            this.message = message;
        }
#endif

        /// <summary>
        /// Gets the SAML message to be used when resolving the signing token.
        /// </summary>
        /// <remarks>When SAML messages are being processed and have signatures without KeyInfo, 
        /// this property will contain the message that is currently being processed.
        /// </remarks>
        /// <value>The SAML message to be used when resolving the signing token.</value>
        public Saml2Message Message {
            get {
                return this.message;
            }
        }
    }
}
