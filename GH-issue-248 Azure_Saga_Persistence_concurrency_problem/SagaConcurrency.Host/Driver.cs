using System;
using NServiceBus;

namespace SagaConcurrency.Host
{
    class Driver : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            Bus.SendLocal(new KickoffSaga {When = DateTime.Now});
        }

        public void Stop()
        {
        }
    }
}