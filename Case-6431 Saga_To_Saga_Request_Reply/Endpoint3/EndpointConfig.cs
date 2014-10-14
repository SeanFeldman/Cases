using System;
using Contracts.Commands;
using Contracts.Messages;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;
using NServiceBus.Saga;

namespace Endpoint3
{
    /*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public EndpointConfig()
        {
            Configure.Features.Disable<SecondLevelRetries>();
            Configure.Features.Disable<TimeoutManager>();
            Configure.Features.Disable<AutoSubscribe>();

            Configure.With()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.Contains("Contracts.Commands"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.Contains("Contracts.Messages"))
                .DefaultBuilder()
                .UseTransport<Msmq>()
                .InMemorySagaPersister()
                .UnicastBus();
        }
    }


    public class Handler : IHandleMessages<Req2>
    {
        static ILog logger = LogManager.GetLogger("Endpoint3");
        public IBus Bus { get; set; }


        public void Handle(Req2 message)
        {
            logger.Info("------------ Handle(Req2) ------------");
            Bus.Reply(new Res1());
            logger.Info("------------ Reply(Res1) ------------");
        }
    }
    public class ResponseHandler : IHandleMessages<Res2>
    {
        static ILog logger = LogManager.GetLogger("Endpoint3");
        public IBus Bus { get; set; }


        public void Handle(Res2 message)
        {
            logger.Info("------------ Handle(Res2) ------------");
            logger.Info(message.Data);
        }
    }
}
