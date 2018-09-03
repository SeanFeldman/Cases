namespace SubscriberASBS
{
    using System.Threading.Tasks;
    using Messages;
    using NServiceBus;
    using NServiceBus.Logging;

    public class CustomEventHandler : IHandleMessages<CustomEvent>
    {
        private static ILog logger = LogManager.GetLogger<CustomEventHandler>();

        public Task Handle(CustomEvent message, IMessageHandlerContext context)
        {
            logger.Info($"Received event with data: {message.Data}");

            return Task.CompletedTask;
        }
    }
}