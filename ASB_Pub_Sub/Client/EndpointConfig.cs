using NSBAzure.Messages.Commands;
using NServiceBus;

namespace Client
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UseTransport<AzureServiceBusTransport>();
            configuration.UsePersistence<InMemoryPersistence>();

            configuration.Conventions().DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"));
            configuration.Conventions().DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"));
        }
    }

    public class KickOffCommand : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            Bus.Send<CreateUser>(x => x.UserId = "testId");
        }

        public void Stop()
        {
        }
    }
}
