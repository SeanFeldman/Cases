
using System;
using System.Configuration;
using System.IO;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Features;
using NServiceBus.Logging;

namespace SagaConcurrency.Host
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.EnableFeature<SecondLevelRetries>();
            configuration.EnableFeature<TimeoutManager>();

            configuration.UsePersistence<AzureStoragePersistence>();
            configuration.UseTransport<AzureServiceBusTransport>();
            configuration.EndpointName("GH-issue-248-AzureSagaPersister");

            var defaultFactory = LogManager.Use<DefaultFactory>();
            defaultFactory.Level(LogLevel.Warn);

            configuration.CustomConfigurationSource(new CustomConfigurationSource());
        }
    }

    public class CustomConfigurationSource : IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            if (typeof(T) == typeof(TransportConfig))
            {
                return new TransportConfig()
                {
                    MaxRetries = 5,
                    MaximumConcurrencyLevel = 15,
                } as T;
            }

            return ConfigurationManager.GetSection(typeof (T).Name) as T;
        }
    }

    public class ConfigureTransport : IProvideConfiguration<TransportConfig>
    {
        public TransportConfig GetConfiguration()
        {
            return new TransportConfig()
            {
                MaxRetries = 5,
                MaximumConcurrencyLevel = 15,
            };
        }
    }

    public class ConfigureErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
    {
        public MessageForwardingInCaseOfFaultConfig GetConfiguration()
        {
            return new MessageForwardingInCaseOfFaultConfig()
            {
                ErrorQueue = "error"
            };
        }
    }

    public class ConfigureAuditQueue : IProvideConfiguration<AuditConfig>
    {
        public AuditConfig GetConfiguration()
        {
            return new AuditConfig()
            {
                QueueName = "audit"
            };
        }
    }

    public class ConfigureSLR : IProvideConfiguration<SecondLevelRetriesConfig>
    {
        public SecondLevelRetriesConfig GetConfiguration()
        {
            return new SecondLevelRetriesConfig()
            {
                Enabled = true,
                TimeIncrease = TimeSpan.FromSeconds(5),
                NumberOfRetries = 1
            };
        }
    }

    public class ConfigureASB : IProvideConfiguration<AzureServiceBusQueueConfig>
    {
        public AzureServiceBusQueueConfig GetConfiguration()
        {
            return new AzureServiceBusQueueConfig()
            {
                ConnectionString = ConnectionStringReader.Get().Transport,
                LockDuration = 180000,
                MaxDeliveryCount = 480
            };
        }
    }

    public class ConfigureSagaPersistence : IProvideConfiguration<AzureSagaPersisterConfig>
    {
        public AzureSagaPersisterConfig GetConfiguration()
        {
            return new AzureSagaPersisterConfig()
            {
                ConnectionString = ConnectionStringReader.Get().Persistence
            };
        }
    }

    public class ConfigureTimeoutManagerPersistence : IProvideConfiguration<AzureTimeoutPersisterConfig>
    {
        public AzureTimeoutPersisterConfig GetConfiguration()
        {
            return new AzureTimeoutPersisterConfig()
            {
                ConnectionString = ConnectionStringReader.Get().Persistence
            };
        }
    }

    public class ConfigureSubscriptionPersistence : IProvideConfiguration<AzureSubscriptionStorageConfig>
    {
        public AzureSubscriptionStorageConfig GetConfiguration()
        {
            return new AzureSubscriptionStorageConfig()
            {
                ConnectionString = ConnectionStringReader.Get().Persistence
            };
        }
    }

    public class ConnectionStringReader
    {
        public static ConnectionStrings Get()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "connectionstrings.json");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ConnectionStrings>(File.ReadAllText(filePath));
        }
    }

    /*
     Required: connectionstring.json file in the following format
        {
            "Transport": "Endpoint=sb://[namespace].servicebus.windows.net/;Shared...",
            "Persistence": "DefaultEndpointsProtocol=https;AccountName=[account];AccountKey=..."
        }
     */

    public class ConnectionStrings
    {
        public string Transport { get; set; }
        public string Persistence { get; set; }
    }
}
