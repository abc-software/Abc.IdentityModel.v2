namespace Abc.IdentityModel.Http {
    using System;

    /// <summary>
    ///  Implemented by messages that have explicit recipients.
    /// </summary>
    public interface IHttpProtocolMessage : IHttpMessage {
        /// <summary>
        /// Gets the URL of the intended receiver of this message.
        /// </summary>
        Uri BaseUri { get; }

        /// <summary>
        /// Gets the preferred method of transport for the message.
        /// </summary>
        /// <remarks>
        /// For indirect messages this will likely be GET+POST, which both can be simulated in the user agent:
        /// the GET with a simple 301 Redirect, and the POST with an HTML form in the response with javascript
        /// to automate submission.
        /// </remarks>
        HttpDeliveryMethods HttpMethods { get; }
    }
}
