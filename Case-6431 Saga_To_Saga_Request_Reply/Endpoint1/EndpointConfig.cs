
using System;
using Contracts.Commands;
using Contracts.Messages;
using NServiceBus.Features;
using NServiceBus.Logging;
using NServiceBus.Unicast;

namespace Endpoint1
{
    using NServiceBus;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public EndpointConfig()
        {
            Configure.Features.Disable<SecondLevelRetries>();
            Configure.Features.Enable<Sagas>();
            Configure.Features.Disable<TimeoutManager>();
            Configure.Features.Disable<AutoSubscribe>();

            Configure.With()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.Contains("Contracts.Commands"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.Contains("Contracts.Messages"))
                .DefaultBuilder()
                .UseTransport<Msmq>()
                //                .MsmqSubscriptionStorage()
                .UnicastBus();
        }
    }

    public class Go : IWantToRunWhenBusStartsAndStops
    {
        private static ILog logger = LogManager.GetLogger("Endpoint1");
        public IBus Bus { get; set; }

        public void Start()
        {
            logger.Info("------------ Bus.Send(Req1) ------------");
            Bus.Send(new Req1 { Identifier = Guid.NewGuid() });
        }

        public void Stop()
        {
        }
    }

    public class ResponseHandler : IHandleMessages<Res2>
    {
        private static ILog logger = LogManager.GetLogger("Endpoint1");
        public IBus Bus { get; set; }


        public void Handle(Res2 message)
        {
            logger.Info("------------ Handle(Res2) ------------");
            logger.Info(message.Data);
        }
    }
}
