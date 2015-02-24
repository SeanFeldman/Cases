
using System;
using System.Configuration;
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
            defaultFactory.Level(LogLevel.Info);
        }
    }

    class ConfigureErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
    {
        public MessageForwardingInCaseOfFaultConfig GetConfiguration()
        {
            return new MessageForwardingInCaseOfFaultConfig()
            {
                ErrorQueue = "error"
            };
        }
    }

    class ConfigureAuditQueue : IProvideConfiguration<AuditConfig>
    {
        public AuditConfig GetConfiguration()
        {
            return new AuditConfig()
            {
                QueueName = "audit"
            };
        }
    }

    class ConfigureTransport : IProvideConfiguration<TransportConfig>
    {
        public TransportConfig GetConfiguration()
        {
            return new TransportConfig()
            {
                MaxRetries = 5,
                MaximumConcurrencyLevel = 10,
            };
        }
    }

    class ConfigureSLR : IProvideConfiguration<SecondLevelRetriesConfig>
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

    class ConfigureASBTransport : IProvideConfiguration<AzureServiceBusQueueConfig>
    {
        public AzureServiceBusQueueConfig GetConfiguration()
        {
            return new AzureServiceBusQueueConfig()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["NServiceBus/Transport"].ConnectionString,
            };
        }
    }

    class ConfigureSagaPersistence : IProvideConfiguration<AzureSagaPersisterConfig>
    {
        public AzureSagaPersisterConfig GetConfiguration()
        {
            return new AzureSagaPersisterConfig()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["NServiceBus/Persistence"].ConnectionString,
            };
        }
    }

    class ConfigureTimeoutManagerPersistence : IProvideConfiguration<AzureTimeoutPersisterConfig>
    {
        public AzureTimeoutPersisterConfig GetConfiguration()
        {
            return new AzureTimeoutPersisterConfig()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["NServiceBus/Persistence"].ConnectionString,
            };
        }
    }

    class ConfigureSubscriptionPersistence : IProvideConfiguration<AzureSubscriptionStorageConfig>
    {
        public AzureSubscriptionStorageConfig GetConfiguration()
        {
            return new AzureSubscriptionStorageConfig()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["NServiceBus/Persistence"].ConnectionString,
            };
        }
    }

}
