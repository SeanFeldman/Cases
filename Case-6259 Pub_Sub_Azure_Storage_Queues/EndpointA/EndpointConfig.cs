using System;
using Case6259.Commands;
using Case6259.Events;
using NServiceBus.Features;

namespace EndpointA
{
    using NServiceBus;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, UsingTransport<AzureStorageQueue>
    {
        public EndpointConfig()
        {
            Feature.Disable<Gateway>();
            Feature.Disable<SecondLevelRetries>();
        }
    }

    public class ConfigureAzureStorage : INeedInitialization
    {
        public void Init()
        {
            Configure.Instance
                .AzureConfigurationSource()
                .AzureSubscriptionStorage()
                .AzureSagaPersister()
                .UseAzureTimeoutPersister();
        }
    }

    public class Go : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            Bus.Publish<DisasterHappened>(x => x.Description = "testing + " + DateTime.Now.ToString("HH:mm:ss"));
        }

        public void Stop()
        {
        }
    }
}
