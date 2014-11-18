using NServiceBus;

namespace Case_209_V5
{
    static class ProgramV5
    {
        static void Main()
        {
            const string connectionString = "Endpoint=sb://";

            var config = new BusConfiguration();
            config.EndpointName("Case_209_V5");
            config.UseTransport<AzureServiceBusTransport>().ConnectionString(() => connectionString);
            config.UsePersistence<InMemoryPersistence>();
            var bus = Bus.CreateSendOnly(config);
        }
    }
}
