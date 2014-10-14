using NSBAzure.Messages.Events;
using NServiceBus;

namespace Client
{
    public class NotifyUserReceivedHandler : IHandleMessages<NotifyUserReceived>
    {
        public void Handle(NotifyUserReceived message)
        {
            
        }
    }
}
