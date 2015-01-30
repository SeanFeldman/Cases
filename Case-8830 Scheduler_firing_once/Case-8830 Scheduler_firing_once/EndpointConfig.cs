using System;
using System.Configuration;
using System.Threading;
using NServiceBus;
using NServiceBus.Persistence;

namespace Case_8830_Scheduler_firing_once
{
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
//            var endpoingName = ConfigurationManager.AppSettings["endpoint.name"];
//            configuration.EndpointName(endpoingName);
            configuration.UsePersistence<RavenDBPersistence>();
            configuration.UseTransport<MsmqTransport>();
            configuration.UseSerialization<JsonSerializer>();
            configuration.EnableInstallers();
        }
    }

    public class Driver : IWantToRunWhenBusStartsAndStops
    {
        public Schedule _Schedule { get; set; }

        public void Start()
        {
            Console.WriteLine("endpoint started");

            _Schedule.Every(TimeSpan.FromSeconds(10), "mytask", () =>
            {
                Console.WriteLine("----- executing mytask from thread name '{0}' id {1}", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId);
            });
        }

        public void Stop()
        {
        }
    }
}
