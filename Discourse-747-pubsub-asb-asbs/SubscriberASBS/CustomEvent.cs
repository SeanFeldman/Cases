namespace PublisherASB
{
    using NServiceBus;

    public class CustomEvent : IEvent
    {
        public string Data { get; set; }
    }
}