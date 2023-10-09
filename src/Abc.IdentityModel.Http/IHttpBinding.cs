namespace Abc.IdentityModel.Http {
    /// <summary>
    /// An interface that must be implemented by message transforms/validators in order
    /// to be included in the channel stack.
    /// </summary>
    public interface IHttpBinding {
        /// <summary>
        /// Performs any transformation on an incoming message that may be necessary and/or
        /// validates an incoming message based on the rules of this channel binding element.
        /// </summary>
        /// <param name="message">The incoming message to process.</param>
        void ProcessIncomingMessage(IHttpProtocolMessage message);
        
        /// <summary>
        /// Prepares a message for sending based on the rules of this binding .
        /// </summary>
        /// <param name="message">The message to prepare for sending.</param>
        void ProcessOutgoingMessage(IHttpProtocolMessage message);
    }
}
