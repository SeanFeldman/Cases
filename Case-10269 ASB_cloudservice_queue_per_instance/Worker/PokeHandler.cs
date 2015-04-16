using System.Diagnostics;
using Messages;
using Messages.Commands;
using NServiceBus;

namespace Worker
{
    public class PokeHandler : IHandleMessages<PokeWorker>
    {
        public IBus Bus { get; set; }
        public void Handle(PokeWorker message)
        {
            Trace.WriteLine(string.Format("-+-+-+-+-+-+-+-+ Replying to WebApp: '{0}'  +-+-+-+-+-+-+-+-", Bus.CurrentMessageContext.ReplyToAddress));
            
            Bus.Reply(new PokeResponse
            {
                LocalDateTime = message.LocalDateTime,
                Data = message.Data
            });
        }
    }
}
