using System;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

namespace SagaConcurrency.Host
{
    public class ConcurrencyTestSaga : Saga<ConcurrencyTestSagaData>,
        IAmStartedByMessages<KickoffSaga>,
        IHandleMessages<InvokeHandlerThatWillReplyBackReply>,
        IHandleTimeouts<FirstTimeout>,
        IHandleTimeouts<SeconfAndFinalTimeout>
    {
        private ILog logger = LogManager.GetLogger<ConcurrencyTestSaga>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ConcurrencyTestSagaData> mapper)
        {
            
        }

        public void Handle(KickoffSaga message)
         {
            Data.Counter = 0;

            for (var i = 0; i < 10; i++)
            {
                Bus.SendLocal(new InvokeHandlerThatWillReplyBack { When = DateTime.Now });
                Data.Counter++;
            }

            logger.WarnFormat("Sent {0} '{1}' commands", Data.Counter, typeof(InvokeHandlerThatWillReplyBack).Name);

            RequestTimeout<FirstTimeout>(TimeSpan.FromSeconds(20));
            RequestTimeout<SeconfAndFinalTimeout>(TimeSpan.FromSeconds(30));
        }

        public void Handle(InvokeHandlerThatWillReplyBackReply message)
        {
            Data.Counter--;
            logger.WarnFormat("Received '{0}' reply, Counter={1}", typeof(InvokeHandlerThatWillReplyBack).Name, Data.Counter);

            if (Data.Counter <= 0)
            {
                logger.Warn("!!!!! Counter is 0, Success !!!!!");
                MarkAsComplete();
            }
        }

        public void Timeout(FirstTimeout state)
        {
           logger.WarnFormat("Counter at first timeout: ", Data.Counter);
        }

        public void Timeout(SeconfAndFinalTimeout state)
        {
            logger.WarnFormat("Counter at second timeout: ", Data.Counter);
            if (Data.Counter >= 0)
            {
                logger.Warn("!!!!!! Failure !!!!!!");
            }
            this.MarkAsComplete();
        }
    }

    public class ConcurrencyTestSagaData : ContainSagaData
    {
        public int Counter { get; set; }
    }

    public class FirstTimeout
    {
    }

    public class SeconfAndFinalTimeout
    {
    }

    public class InvokeHandlerThatWillReplyBack : ICommand
    {
        public DateTime When { get; set; }
    }

    public class InvokeHandlerThatWillReplyBackReply : IMessage
    {
        public DateTime When { get; set; }
    }
}