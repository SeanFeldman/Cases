using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

namespace Case_8830_Scheduler_firing_once
{
    public class ConfigureErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
    {
        public MessageForwardingInCaseOfFaultConfig GetConfiguration()
        {
            return new MessageForwardingInCaseOfFaultConfig
            {
                ErrorQueue = "error." + this.GetType().Namespace
            };
        }
    }
}