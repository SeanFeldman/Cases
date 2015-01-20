using System;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Legacy;

namespace Test_issue_2622
{
    class Program_develop
    {
        static void Main(string[] args)
        {
            var config = new BusConfiguration();
            config.UseTransport<MsmqTransport>();

            config.UsePersistence<InMemoryPersistence>();
            config.UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>();
            // test that .For() is possible
            //~config.UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>().For(Storage.Sagas, Storage.Subscriptions);

            var startableBus = Bus.Create(config);
            startableBus.Start();

            return;
            startableBus.SendLocal<TestIt>(x => { });
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }

    class ConfigureErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
    {

        public MessageForwardingInCaseOfFaultConfig GetConfiguration()
        {
            return new MessageForwardingInCaseOfFaultConfig
            {
                ErrorQueue = "Test_issue_2622_error"
            };
        }
    }

    class ConfigureTimeoutsConfig : IProvideConfiguration<AzureSubscriptionStorageConfig>
    {
        public AzureSubscriptionStorageConfig GetConfiguration()
        {
            return new AzureSubscriptionStorageConfig
            {
                ConnectionString = "UseDevelopmentStorage=true"
            };
        }
    }

    public class TestIt : ICommand
    {
    }

    public class Handler : IHandleMessages<TestIt>
    {
        public IBus Bus { get; set; }

        public void Handle(TestIt message)
        {
            Console.WriteLine("Received message with id " + Bus.CurrentMessageContext.Id);
        }
    }
}
