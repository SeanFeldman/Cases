namespace TestEndpoint
{
    using NServiceBus;

    [EndpointName("Test")]
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration configuration)
        {
            configuration.SendFailedMessagesTo("error");
            configuration.EnableInstallers();
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.UseTransport<MsmqTransport>();
            configuration.Recoverability().Immediate(settings => settings.NumberOfRetries(0));
            configuration.Recoverability().Delayed(settings => settings.NumberOfRetries(0));
            // Required for approach 01, but not 02
            //configuration.RegisterComponents(components => components.RegisterSingleton(configuration));
        }
    }
}
