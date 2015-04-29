using System;
using System.Linq;
using System.Threading;
using Messages;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Features;

namespace Receiver
{
    class ReceiverProgram
    {
        static void Main(string[] args)
        {
            Thread.Sleep(500);
            var configuration = new BusConfiguration();
            configuration.EndpointName("case-10108-receiver");
            configuration.UseTransport<MsmqTransport>();
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.DisableFeature<SecondLevelRetries>();
            configuration.DisableFeature<TimeoutManager>();
            configuration.DisableFeature<AutoSubscribe>();
            var bus = Bus.Create(configuration);
            bus.Start();
            Console.WriteLine("Receiver is running...");
            keepRunning = true;
            while (keepRunning)
            {
            }
        }

        public static bool keepRunning { get; set; }
    }

    public class CommandWithArrayOfNullablesHandler : IHandleMessages<CommandWithArrayOfNullables>
    {
        public void Handle(CommandWithArrayOfNullables message)
        {
            Console.WriteLine("Received a message");
            foreach (var value in message.Values)
            {
                Console.WriteLine("item: '{0}'", value);
            }
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
