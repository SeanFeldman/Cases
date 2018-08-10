//namespace TestEndpoint
//{
//    using System.Threading.Tasks;
//    using NServiceBus;
//    using NServiceBus.Faults;
//    using NServiceBus.Logging;
//
//    public class ErrorNotificationForwarder : IWantToRunWhenEndpointStartsAndStops
//    {
//        private readonly EndpointConfiguration endpointConfiguration;
//        private static ILog logger = LogManager.GetLogger<ErrorNotificationForwarder>();
//
//        public ErrorNotificationForwarder(EndpointConfiguration endpointConfiguration)
//        {
//            this.endpointConfiguration = endpointConfiguration;
//        }
//
//        public Task Start(IMessageSession session)
//        {
//            logger.Info("-- Start --");
//
//            this.endpointConfiguration.Notifications.Errors.MessageSentToErrorQueue += ErrorsOnMessageSentToErrorQueue;
//
//            var sendOptions = new SendOptions();
//            sendOptions.RouteToThisEndpoint();
//
//            return session.Send(new KickoffMessage(), sendOptions);
//        }
//
//        private void ErrorsOnMessageSentToErrorQueue(object sender, FailedMessage e)
//        {
//            logger.Info($"mapping failed message to a custom message to be sent. Original exception: {e.Exception.Message}");
//            // Just to show it's working
//            session.Send("temp", new ConvertedMessage());
//        }
//
//        public Task Stop(IMessageSession session)
//        {
//            logger.Info("-- Stop --");
//            return Task.CompletedTask;
//        }
//    }
//}