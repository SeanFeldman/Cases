namespace TestEndpoint
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    public class KickoffMessageHandler : IHandleMessages<KickoffMessage>
    {
        private static ILog logger = LogManager.GetLogger<KickoffMessageHandler>();

        public Task Handle(KickoffMessage message, IMessageHandlerContext context)
        {
            logger.Info("failing message");

            throw new Exception("kaboom rico!");
        }
    }
}