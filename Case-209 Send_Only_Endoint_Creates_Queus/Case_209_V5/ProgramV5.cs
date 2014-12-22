using System;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Persistence;
using NServiceBus.Saga;
using NServiceBus.SagaPersisters;

namespace Case_209_V5
{
    class Config : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>, IProvideConfiguration<TransportConfig>, IProvideConfiguration<AzureServiceBusQueueConfig>
    {
        public MessageForwardingInCaseOfFaultConfig GetConfiguration()
        {
            return new MessageForwardingInCaseOfFaultConfig
            {
                ErrorQueue = "error"
            };
        }

        TransportConfig IProvideConfiguration<TransportConfig>.GetConfiguration()
        {
            return new TransportConfig
            {
                 MaximumConcurrencyLevel = 1,
                 MaxRetries = 1
            };
        }

        AzureServiceBusQueueConfig IProvideConfiguration<AzureServiceBusQueueConfig>.GetConfiguration()
        {
            return new AzureServiceBusQueueConfig
            {
                MaxSizeInMegabytes = 1024 * 3,
                RequiresDuplicateDetection = true,
                DuplicateDetectionHistoryTimeWindow = 86400000
            };
        }
    }

    static class ProgramV5
    {
        static void Main()
        {
            const string seviceBusConnectionString = "Endpoint=sb://seanfeldman-test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=so1NHoEEMR2MMYO5G28jobUIKn5e5eunySk0Znom43g=";
            const string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=seanfeldman;AccountKey=n8XalBvh0iW7gOyq4VAYr9TZY0C76ag2iDvN9k/DH1pioJuc1yKrZefNclEYLxhdchlTU0pm3JNVEw8CsOEJfg==";

            var config = new BusConfiguration();
            config.EndpointName("Case209V5");
            config.UseTransport<AzureServiceBusTransport>().ConnectionString(() => seviceBusConnectionString);
//            config.UseTransport<AzureStorageQueueTransport>().ConnectionString(() => storageConnectionString);
            config.UsePersistence<AzureStoragePersistence>();

            #region both

            config.UsePersistence<AzureStoragePersistence, StorageType.Sagas>()
                .ConnectionString(storageConnectionString)
                .CreateSchema(true);

            #endregion

            #region only for ASQ

            config.UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>()
                .ConnectionString(storageConnectionString)
                .TableName("Case209V5subs");

            config.UsePersistence<AzureStoragePersistence, StorageType.Timeouts>()
                .ConnectionString(storageConnectionString)
                .CreateSchema(true)
                .TimeoutManagerDataTableName("TimeoutManager")
                .TimeoutDataTableName("TimeoutData")
                .CatchUpInterval(3600)
                .PartitionKeyScope("yyyy-MM-dd-HH-dd");

            #endregion

            var bus = Bus.Create(config);
            bus.Start();
            bus.SendLocal<TestCommand>(x =>
            {
                x.Data = "123";
                x.Id = "1122334455";
            });
            Console.ReadLine();
        }
    }

    public class TestSaga : Saga<TestSagaData>, IAmStartedByMessages<TestCommand>, IHandleTimeouts<TestTimeout>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TestSagaData> mapper)
        {
            mapper.ConfigureMapping<TestCommand>(m => m.Id).ToSaga(s => s.IdFromMessage);
        }

        public void Handle(TestCommand message)
        
        {
            Data.DataFromMessage = message.Data;
            var timeout = new TestTimeout {Reason = "Just like that " + DateTime.Now.Ticks};
            RequestTimeout(TimeSpan.FromSeconds(60), timeout);
        }

        public void Timeout(TestTimeout state)
        {
            Console.WriteLine("received timeout: " + state.Reason);
            MarkAsComplete();
        }
    }

/*
    public class TimeoutPersisterReceiverConfigurator : IWantToRunWhenBusStartsAndStops
    {
        public TimeoutPersisterReceiver TimeoutPersisterReceiver { get; set; }
        public void Start()
        {
            TimeoutPersisterReceiver.SecondsToSleepBetweenPolls = 5;
        }

        public void Stop()
        {
        }
    }
*/

    public class TestTimeout
    {
        public string Reason { get; set; }
    }

    public class TestSagaData : ContainSagaData
    {
        public string IdFromMessage { get; set; }
        public string DataFromMessage { get; set; }
    }

    public class TestCommand : ICommand
    {
        public string Id { get; set; }
        public string Data { get; set; }
    }
}
