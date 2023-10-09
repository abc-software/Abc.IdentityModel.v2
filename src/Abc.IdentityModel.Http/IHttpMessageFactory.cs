namespace Abc.IdentityModel.Http {
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A tool to analyze an incoming message to figure out what concrete class
    /// is designed to deserialize it and instantiates that class.
    /// </summary>
    public interface IHttpMessageFactory {
        /// <summary>
        /// Creates the message.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="method">The HTTPS method(s).</param>
        /// <param name="fields">The fields.</param>
        /// <returns>
        /// The HTTP message.
        /// </returns>
        IHttpMessage CreateMessage(Uri baseUrl, HttpDeliveryMethods method, IDictionary<string, string> fields);
    }
}
