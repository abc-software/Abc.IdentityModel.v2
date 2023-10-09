namespace Abc.IdentityModel.Protocols.Saml2 {
    using System;

    /// <summary>
    /// Specifies the type of authentication context reference represented by a URI.
    /// </summary>
    /// <remarks>See saml:AuthnContextClassRef in [SamlCore, 2.7.2.2] for more details.</remarks>
    public enum Saml2AuthenticationContextReferenceType {
        Class,
        Declaration,
    }
}
