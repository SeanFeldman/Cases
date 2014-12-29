using System.Diagnostics;
using Case_223.Contracts.Messages;
using NServiceBus;

namespace WebRole
{
    public class PongHandler : IHandleMessages<Pong>
    {
        public void Handle(Pong message)
        {
            Trace.WriteLine(string.Format("PongHandler received: '{0}'.", message.OriginalMessage));
        }
    }
}