using System;
using Contracts.Commands;
using Contracts.Messages;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;
using NServiceBus.Saga;

namespace Endpoint2
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


    public class MySaga : Saga<MySagaData>, IAmStartedByMessages<Req1>, IHandleMessages<Res1>
    {
        static ILog logger = LogManager.GetLogger("Endpoint2");
        
        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<Req1>(m => m.Identifier).ToSaga(s => s.Id);
        }

        public void Handle(Req1 message)
        {
            logger.Info("------------ Handle(Req1) ------------");
            Bus.Send(new Req2 {Identifier = Guid.NewGuid()});
        }

        public void Handle(Res1 message)
        {
            logger.Info("------------ Handle(Res1) ------------");

            logger.Info("------------ Reply(Res2) ------------");
            Bus.Reply(new Res2 { Data = "bus.reply"});

            logger.Info("------------ ReplyToOriginator(Res2) ------------");
            ReplyToOriginator(new Res2 { Data = "ReplyToOriginator" });

            MarkAsComplete();
        }
    }

    public class MySagaData : ContainSagaData
    {
    }
}
