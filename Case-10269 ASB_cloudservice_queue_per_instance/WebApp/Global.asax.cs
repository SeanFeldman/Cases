using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Installation.Environments;

namespace WebApp
{
    public class MvcApplication : HttpApplication
    {
        private static IBus bus;

        private IStartableBus startableBus;

        public static IBus Bus
        {
            get { return bus; }
        }

        protected void Application_Start()
        {
            Configure.Instance.DefineEndpointName("lhptlistener");
            Configure.ScaleOut(s => s.UseUniqueBrokerQueuePerMachine());

            Feature.Disable<TimeoutManager>();
            Feature.Disable<SecondLevelRetries>();
            Feature.Disable<AutoSubscribe>();

            startableBus = Configure.With()
                .DefaultBuilder()
                .TraceLogger()
                .UseTransport<AzureServiceBus>()
                .PurgeOnStartup(true)
                .UnicastBus()
                .CreateBus();

            Configure.Instance.ForInstallationOn<Windows>().Install();


            bus = startableBus.Start();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End()
        {
            startableBus.Dispose();
        }
    }
}
