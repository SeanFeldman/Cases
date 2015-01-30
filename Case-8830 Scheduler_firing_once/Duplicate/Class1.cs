using System;
using NServiceBus;
using NServiceBus.Saga;

namespace Case_8830_Scheduler_firing_once
{
    public class SchedulingSaga : Saga<ScheduleSagaData>, IAmStartedByMessages<StartSchedulingSaga>, IHandleTimeouts<TimeToRunSchedule>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ScheduleSagaData> mapper)
        {
        }

        public void Handle(StartSchedulingSaga message)
        {
            RequestTimeout<TimeToRunSchedule>(TimeSpan.FromSeconds(10));
        }

        public void Timeout(TimeToRunSchedule state)
        {
            // we don't want to run a lengthy process from within saga
            Bus.Send(new CommandToRunScheduledTask());
            RequestTimeout<TimeToRunSchedule>(TimeSpan.FromSeconds(10));
        }
    }

    public class CommandToRunScheduledTask : ICommand
    {
    }

    public class TimeToRunSchedule
    {
    }

    public class StartSchedulingSaga : ICommand
    {
    }

    public class ScheduleSagaData : ContainSagaData
    {
    }
}