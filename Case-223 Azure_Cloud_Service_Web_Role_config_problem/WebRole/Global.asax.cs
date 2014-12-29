using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Case_223.Contracts.Commands;
using Case_223.Shared;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

namespace WebRole
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IStartableBus startableBus;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var configuration = new BusConfiguration();
            configuration.AzureConfigurationSource();
            configuration.UseTransport<AzureStorageQueueTransport>();
            configuration.UsePersistence<AzureStoragePersistence>();
            configuration.ApplyMessageConventions();
            startableBus = Bus.Create(configuration);
            startableBus.Start();

            startableBus.Send<Ping>(ping => ping.Message = "ping from web");
            Trace.WriteLine("WebRole - sent a message");
        }

        protected void Application_End()
        {
            startableBus.Dispose();
        }
    }

    internal class MappingConfigurationSource : IProvideConfiguration<UnicastBusConfig>
    {
        public UnicastBusConfig GetConfiguration()
        {
            return new UnicastBusConfig
            {
                MessageEndpointMappings = new MessageEndpointMappingCollection
                {
                    new MessageEndpointMapping
                    {
                         Endpoint = "case223-worker",
                         AssemblyName = "Case-223.Contracts",
                    }
                }
            };
        }
    }
}
