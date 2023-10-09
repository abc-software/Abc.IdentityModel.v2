namespace Abc.IdentityModel.Http {
    using System;

    /// <summary>
    /// The methods available for the local party to send messages to a remote party.
    /// </summary>
    [Flags]
    public enum HttpDeliveryMethods {
        /// <summary>
        /// No HTTP methods are allowed.
        /// </summary>
        None = 0,
        
        /// <summary>
        /// As the HTTP POST request body with a content-type of application/x-www-form-urlencoded.
        /// </summary>
        PostRequest = 2,

        /// <summary>
        /// Added to the URLs in the query part (as defined by [RFC3986] (Berners-Lee, T., “Uniform Resource Identifiers (URI): Generic Syntax,” .) section 3).
        /// </summary>
        GetRequest = 4,

        /// <summary>
        /// The flags that control HTTP verbs.
        /// </summary>
        VerbMask = PostRequest | GetRequest,
    }
}
