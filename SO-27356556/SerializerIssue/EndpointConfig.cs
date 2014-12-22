using NServiceBus.Features;

namespace SerializerIssue
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UseTransport<MsmqTransport>();
            configuration.UsePersistence<InMemoryPersistence>();

            configuration.AssembliesToScan(AllAssemblies.Except("ABCpdf9-32.dll"));
            configuration.UseSerialization<JsonSerializer>();
            configuration.EndpointName("testqueue");

            configuration.DisableFeature<AutoSubscribe>();
            configuration.DisableFeature<SecondLevelRetries>();

            var conventionsBuilder = configuration.Conventions();

            conventionsBuilder.DefiningCommandsAs(t => t.Namespace != null &&
                                                       !t.IsInterface &&
                                                       t.Name.EndsWith("Command"));
            conventionsBuilder.DefiningMessagesAs(t => t.Namespace != null &&
                                                       !t.IsInterface &&
                                                       t.Name.EndsWith("Notification"));

            Bus.Create(configuration);
        }
    }

    public interface IDeepCopy<T>
    {
        T DeepCopy();
    }

    public class TestCommand
    {
        public IUserContext UserContext { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
    }

    public interface IUserContext : IDeepCopy<IUserContext>
    {
        int Id { get; set; }
        string Description { get; set; }
    }

    public class UserContext : IUserContext
    {
        public IUserContext DeepCopy()
        {
            return new UserContext
            {
                Id = Id,
                Description = Description
            };
        }

        public int Id { get; set; }
        public string Description { get; set; }
    }
}