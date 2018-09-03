using System;

namespace SubscriberASBS
{
    using System.Threading.Tasks;
    using Messages;
    using NServiceBus;

    class Program
    {
        static async Task Main(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration("subscriber");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.EnableInstallers();

            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString"));

            var startableEndpoint = await Endpoint.Create(endpointConfiguration);
            var instance = await startableEndpoint.Start();

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();

            await instance.Stop();
        }
    }
}
