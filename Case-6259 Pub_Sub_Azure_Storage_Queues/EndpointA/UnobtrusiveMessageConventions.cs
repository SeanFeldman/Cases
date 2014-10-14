using NServiceBus;

// ReSharper disable once CheckNamespace
namespace Case6259.Shared
{
    public class UnobtrusiveMessageConventions : IWantToRunBeforeConfiguration
    {
        public void Init()
        {
            Configure.Instance
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.Contains(".Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.Contains(".Events"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.Contains(".Messages"));
        }
    }
}