using Case_223.Shared;
using NServiceBus;

namespace WorkerRole
{
    class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
    {
        public void Customize(BusConfiguration configuration)
        {
            //configuration.AzureConfigurationSource();
            configuration.UseTransport<AzureServiceBusTransport>();
            configuration.UsePersistence<AzureStoragePersistence>();
            configuration.ApplyMessageConventions();
        }
    }
}
