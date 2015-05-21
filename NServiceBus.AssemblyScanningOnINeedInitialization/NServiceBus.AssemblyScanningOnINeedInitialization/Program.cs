using System;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using Shared;

namespace NServiceBus.AssemblyScanningOnINeedInitialization
{
    class Program
    {
        private static void Main(string[] args)
        {
            var config = new BusConfiguration();
            config.UsePersistence<InMemoryPersistence>();
            config.EndpointName("nservicebus");
            config.EnableInstallers();
            //config.AssembliesToScan(AllAssemblies.Except("Blaa.dll"));
            var bus = Bus.Create(config);
            bus.Start();
            bus.SendLocal(new TestCommand());

            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                
            }
        }
    }
}

public class Bla : INeedInitialization
{
    public void Customize(BusConfiguration config)
    {
        config.AssembliesToScan(AllAssemblies.Except("Blaa.dll"));
        config.EndpointName("ops");
    }
}

public class CreateErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
{
    public MessageForwardingInCaseOfFaultConfig GetConfiguration()
    {
        return new MessageForwardingInCaseOfFaultConfig
        {
            ErrorQueue = "error"
        };
    }
}