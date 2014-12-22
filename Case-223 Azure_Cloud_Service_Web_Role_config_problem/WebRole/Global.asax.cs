using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Case_223.Shared;
using NServiceBus;

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
            //configuration.AzureConfigurationSource();
            configuration.UseTransport<AzureServiceBusTransport>();
            configuration.UsePersistence<AzureStoragePersistence>();
            configuration.ApplyMessageConventions();
            startableBus = Bus.Create(configuration);
            startableBus.Start();
        }

        protected void Application_End()
        {
            startableBus.Dispose();
        }
    }
}
