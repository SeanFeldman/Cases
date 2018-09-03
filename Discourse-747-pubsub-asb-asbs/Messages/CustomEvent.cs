namespace Messages
{
    using NServiceBus;

    public interface CustomEvent : IEvent
    {
        string Data { get; set; }
    }
}
