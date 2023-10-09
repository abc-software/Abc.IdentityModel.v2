namespace Abc.IdentityModel.Http {
    /// <summary>
    /// An interface that messages wishing to perform custom serialization/deserialization
    /// may implement to be notified of <see cref="T:DotNetOpenAuth.Messaging.Channel" /> events.
    /// </summary>
    internal interface IHttpMessageWithEvents : IHttpMessage {
        /// <summary>
        /// Called when the message has been received, 
        /// after it passes through the channel binding elements.
        /// </summary>
        void OnReceiving();

        /// <summary>
        /// Called when the message is about to be transmitted,
        /// before it passes through the channel binding elements.
        /// </summary>
        void OnSending();
    }
}
