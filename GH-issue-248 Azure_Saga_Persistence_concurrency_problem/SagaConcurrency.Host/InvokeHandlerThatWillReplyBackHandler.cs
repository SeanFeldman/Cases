using System;
using System.Threading;
using NServiceBus;

namespace SagaConcurrency.Host
{
    public class InvokeHandlerThatWillReplyBackHandler : IHandleMessages<InvokeHandlerThatWillReplyBack>
    {
        public IBus Bus { get; set; }

        public void Handle(InvokeHandlerThatWillReplyBack message)
        {
            //Task.Delay(TimeSpan.FromSeconds(1));
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Bus.Reply(new InvokeHandlerThatWillReplyBackReply { When = DateTime.Now });    
        }
    }
}