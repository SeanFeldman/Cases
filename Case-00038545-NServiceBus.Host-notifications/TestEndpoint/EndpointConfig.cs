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
            configuration.RegisterComponents(components => components.RegisterSingleton(configuration));
        }
    }
}
