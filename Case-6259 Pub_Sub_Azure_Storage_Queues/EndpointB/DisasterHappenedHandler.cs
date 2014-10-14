using System;
using Case6259.Events;
using NServiceBus;

namespace EndpointB
{
    public class DisasterHappenedHandler  : IHandleMessages<DisasterHappened>
    {
        public void Handle(DisasterHappened message)
        {
            Console.WriteLine("Received: " + message.Description);
        }
    }
}
