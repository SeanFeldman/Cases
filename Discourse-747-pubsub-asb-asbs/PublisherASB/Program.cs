namespace PublisherASB
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    class Program
    {
        static async Task Main(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration("publisher");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.EnableInstallers();

            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            transport.UseForwardingTopology();
            transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString"));

            var startableEndpoint = await Endpoint.Create(endpointConfiguration);
            var instance = await startableEndpoint.Start();

            ConsoleKeyInfo key;
            do
            {
                await instance.Publish(new CustomEvent { Data = DateTime.Now.ToString("O") });

                Console.WriteLine("Event published");
                Console.WriteLine("Press ESC to quit or any other key to send an event");

                key = Console.ReadKey();
            } while (ConsoleKey.Escape != key.Key);

            await instance.Stop();
        }
    }
}
