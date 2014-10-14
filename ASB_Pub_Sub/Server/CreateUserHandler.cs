using NSBAzure.Messages.Commands;
using NSBAzure.Messages.Events;
using NServiceBus;

namespace Server
{
    public class CreateUserHandler : IHandleMessages<CreateUser>
    {
        public IBus Bus { get; set; }

        public void Handle(CreateUser message)
        {
            Bus.Publish<NotifyUserReceived>(x => x.UserId = message.UserId);
        }
    }
}
