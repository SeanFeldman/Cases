using System;
using Contracts.Commands;
using Contracts.Messages;
using Endpoint1;
using Endpoint2;
using NServiceBus.Testing;
using Xunit;

namespace UnitTests
{
    public class TestIt
    {
        public TestIt()
        {
            Test.Initialize(typeof(Endpoint1.EndpointConfig).Assembly);
        }

        [Fact]
        public void Try()
        {
            Test.Saga<MySaga>()
                .ExpectReply<Res2>()
                .ExpectReplyToOriginator<Res2>()
                .When(s => s.Handle(new Res1()))
                .AssertSagaCompletionIs(true);
        }
    }
}
