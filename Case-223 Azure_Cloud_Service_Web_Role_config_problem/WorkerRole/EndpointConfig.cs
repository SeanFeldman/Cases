using Case_223.Shared;
using NServiceBus;

namespace WorkerRole
{
    class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UseTransport<AzureStorageQueueTransport>();
            configuration.UsePersistence<AzureStoragePersistence>();
            configuration.ApplyMessageConventions();
        }
    }
}
