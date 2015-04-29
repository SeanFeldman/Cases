using System;
using Messages;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Features;

namespace Sender
{
    class SenderProgram
    {
        static void Main(string[] args)
        {
            var configuration = new BusConfiguration();
            configuration.EndpointName("case-10108-sender");
            configuration.UseTransport<MsmqTransport>();
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.DisableFeature<SecondLevelRetries>();
            configuration.DisableFeature<TimeoutManager>();
            configuration.DisableFeature<AutoSubscribe>();
            var bus = Bus.Create(configuration);
            bus.Start();
            bus.Send(new CommandWithArrayOfNullables { Values = new int?[] {1, 2, 3}});
        }
    }

    class ConfigureRouting : IProvideConfiguration<UnicastBusConfig>
    {
        public UnicastBusConfig GetConfiguration()
        {
            return new UnicastBusConfig
            {
                MessageEndpointMappings = new MessageEndpointMappingCollection
                {
                    new MessageEndpointMapping
                    {
                        AssemblyName = "Messages",
                        Namespace = "Messages",
                        Endpoint = "case-10108-receiver"
                    }
                }
            };
        }
    }

    class ConfigureErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
    {
        public MessageForwardingInCaseOfFaultConfig GetConfiguration()
        {
            return new MessageForwardingInCaseOfFaultConfig
            {
                ErrorQueue = "error",
            };
        }
    }

    class ConfigureMaxRetries : IProvideConfiguration<TransportConfig>
    {
        public TransportConfig GetConfiguration()
        {
            return new TransportConfig
            {
                MaxRetries = 1,
                MaximumConcurrencyLevel = 1
            };
        }
    }
}
