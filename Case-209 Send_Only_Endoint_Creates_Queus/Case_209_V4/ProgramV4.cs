using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureServiceBus;
using NServiceBus.Features;

namespace Case_209_V4
{
    static class ProgramV4
    {
        static void Main()
        {
            const string connectionString = "Endpoint=sb:";

//            Feature.Disable<QueueAutoCreation>();

            var bus = Configure.With()
                        .DefineEndpointName("Case_209_Sender_V4")
                        .DefaultBuilder()
                        .UseTransport<AzureServiceBus>(() => connectionString)
                        .UseInMemoryTimeoutPersister()
                        .UnicastBus()
                        .SendOnly();
        }
    }
}
