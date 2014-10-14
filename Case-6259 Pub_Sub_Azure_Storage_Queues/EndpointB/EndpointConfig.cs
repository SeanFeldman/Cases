
using NServiceBus.Features;

namespace EndpointB
{
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, UsingTransport<AzureStorageQueue>
	{
        public EndpointConfig()
        {
            Feature.Disable<Gateway>();
            Feature.Disable<SecondLevelRetries>();
            //test this
            Feature.Enable<AutoSubscribe>();
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

}
