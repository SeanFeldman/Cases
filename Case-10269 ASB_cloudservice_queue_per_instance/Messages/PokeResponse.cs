using System;
using NServiceBus;

namespace Messages
{
    public class PokeResponse : IMessage
    {
        public string Data { get; set; }
        public DateTime LocalDateTime { get; set; }
    }
}
