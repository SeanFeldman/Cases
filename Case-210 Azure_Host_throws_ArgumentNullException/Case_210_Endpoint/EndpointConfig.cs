using NServiceBus;

namespace Case_210_Endpoint_V5
{
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.EndpointName("Case_210_Endpoint");
            configuration.UsePersistence<InMemoryPersistence>();
            var sendOnlyBus = Bus.CreateSendOnly(configuration);
            (sendOnlyBus as IStartableBus).Start();
        }
    }
}
