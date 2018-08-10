namespace TestEndpoint
{
    using NServiceBus;
    using NServiceBus.Faults;
    using NServiceBus.Logging;

    public class ErrorHandler
    {
        private static ILog logger = LogManager.GetLogger<ErrorHandler>();

        private readonly Notifications notifications;

        public ErrorHandler(Notifications notifications)
        {
            this.notifications = notifications;

            notifications.Errors.MessageSentToErrorQueue += ErrorsOnMessageSentToErrorQueue;
        }

        private void ErrorsOnMessageSentToErrorQueue(object sender, FailedMessage message)
        {
            logger.Info($"mapping failed message to a custom message to be sent. Original exception: {message.Exception.Message}");
            // Just to show it's working
            MessageSession.Send("temp", new ConvertedMessage());
        }

        public IMessageSession MessageSession { get; set; }

        public void Unsubscribe()
        {
            notifications.Errors.MessageSentToErrorQueue -= ErrorsOnMessageSentToErrorQueue;
        }
    }
}