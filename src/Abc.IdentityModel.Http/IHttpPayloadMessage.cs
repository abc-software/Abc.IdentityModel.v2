namespace Abc.IdentityModel.Http {
    /// <summary>
    /// The message with payload.
    /// </summary>
    /// <typeparam name="T">The message payload type.</typeparam>
    public interface IHttpPayloadMessage<T> {
        /// <summary>
        /// Gets or sets the message payload.
        /// </summary>
        /// <value>
        /// The payload.
        /// </value>
        T Payload { get; set; }

        /// <summary>
        /// Gets or sets the message data.
        /// </summary>
        /// <value>
        /// The message data.
        /// </value>
        string Data { get; set; }
    }
}
