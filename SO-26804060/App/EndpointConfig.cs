
using System;
using Autofac;
using NServiceBus.Persistence;

namespace App
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(BusConfiguration configuration)
        {
            // https://stackoverflow.com/questions/26804060/nservicebus-5-0-object-reference-not-set-to-an-instance-of-an-object-at-nservice
            var container = BuildContainer();
            configuration.EndpointName("busbus");
            configuration.UseTransport<AzureStorageQueueTransport>();
            configuration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));
            configuration.UsePersistence</*RavenDBPersistence*/InMemoryPersistence>();
        }

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAssemblyModules(domainAssemblies);
            return builder.Build();
        }
    }

    public class Test : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }
        public void Start()
        {
            Bus.Send<TestCommand>(command => command.When = DateTime.Now );
        }

        public void Stop()
        {
        }
    }

    public class TestCommand : ICommand
    {
        public DateTime When { get; set; }
    }
}
