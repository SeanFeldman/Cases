using NServiceBus;

namespace Messages
{
    public class CommandWithArrayOfNullables : ICommand
    {
        public int?[] Values { get; set; } 
    }
}
