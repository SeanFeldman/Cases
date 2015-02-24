using System;
using NServiceBus;

namespace SagaConcurrency.Host
{
    public class KickoffSaga : ICommand
    {
        public DateTime When { get; set; }
    }
}