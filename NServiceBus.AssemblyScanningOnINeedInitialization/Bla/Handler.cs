using System;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using Shared;

namespace Blaa
{
    public class Handler : IHandleMessages<TestCommand>
    {
        public void Handle(TestCommand message)
        {
            Console.WriteLine("got it");
        }
    }
}

public class CreateErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
{
    public MessageForwardingInCaseOfFaultConfig GetConfiguration()
    {
        return new MessageForwardingInCaseOfFaultConfig
        {
            ErrorQueue = "error"
        };
    }
}