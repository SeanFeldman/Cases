using System.Diagnostics;
using Case_223.Contracts.Commands;
using Case_223.Contracts.Messages;
using NServiceBus;

namespace WorkerRole
{
    public class PingHandler : IHandleMessages<Ping>
    {
        private readonly IBus bus;

        public PingHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(Ping message)
        {
            Trace.WriteLine(string.Format("PingHandler received: '{0}'.", message.Message));
            bus.Reply<Pong>(pong => pong.OriginalMessage = message.Message);
        }
    }
}