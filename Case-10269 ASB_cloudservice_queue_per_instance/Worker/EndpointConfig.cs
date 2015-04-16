using System.Diagnostics;
using NServiceBus;
using NServiceBus.Features;

namespace Worker
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker, UsingTransport<AzureServiceBus>, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                .DefaultBuilder();

            Configure.ScaleOut(settings => settings.UseUniqueBrokerQueuePerMachine());

            Configure.Features.Disable<TimeoutManager>();
            Configure.Features.Disable<SecondLevelRetries>();
            Configure.Features.Disable<AutoSubscribe>();
        }
    }

    public class MyClass : IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
            Trace.WriteLine("The VideoStore.Sales endpoint is now started and ready to accept messages");
        }

        public void Stop()
        {

        }
    }
}
