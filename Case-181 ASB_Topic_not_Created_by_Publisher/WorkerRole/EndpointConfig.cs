using NServiceBus;

namespace WorkerRole
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UseTransport<AzureServiceBusTransport>();
            configuration.UsePersistence<AzureStoragePersistence>();

            configuration.Conventions()
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Case181.Messages.Events"));

            configuration.EndpointName("Azure.CloudServiceExample.Server");
        }
    }

    public class Test : IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
