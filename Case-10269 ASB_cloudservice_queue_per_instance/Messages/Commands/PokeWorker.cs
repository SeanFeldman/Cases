using System;
using NServiceBus;

namespace Messages.Commands
{
    public class PokeWorker : IMessage
    {
        public DateTime LocalDateTime { get; set; }
        public string Data { get; set; }
    }
}
