using System;

namespace Abc.IdentityModel.Protocols.Saml2 {
    /// <summary>
    /// The comparison method used to evaluate the requested context classes or statements.
    /// </summary>
    /// <remarks>See [SamlCore, 3.3.2.2.1] for more details.</remarks>
    internal enum Saml2AuthenticationContextComparisonType {
        /// <summary>
        /// The resulting authentication context in the authentication
        /// statement MUST be the exact match of at least one of the authentication contexts specified
        /// </summary>
        Exact,

        /// <summary>
        /// The resulting authentication context in the authentication
        /// statement MUST be at least as strong (as deemed by the responder) as one of the authentication
        /// contexts specified.
        /// </summary>
        Minimum,

        /// <summary>
        /// the resulting authentication context in the authentication
        /// statement MUST be as strong as possible (as deemed by the responder) without exceeding the strength
        /// of at least one of the authentication contexts specified.
        /// </summary>
        Maximum,

        /// <summary>
        /// The resulting authentication context in the authentication
        /// statement MUST be stronger (as deemed by the responder) than any one of the authentication contexts
        /// specified.
        /// </summary>
        Better,
    }
}
