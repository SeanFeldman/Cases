namespace TestEndpoint
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;

    public class ErrorHandlerFeature : Feature
    {
        private Notifications notifications;

        public ErrorHandlerFeature()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            notifications = context.Settings.Get<Notifications>();
            var errorHandler = new ErrorHandler(notifications);
            context.RegisterStartupTask(new ErrorHandlerStartupTask(errorHandler));
        }
    }

    public class ErrorHandlerStartupTask : FeatureStartupTask
    {
        private ErrorHandler errorHandler;

        public ErrorHandlerStartupTask(ErrorHandler errorHandler)
        {
            this.errorHandler = errorHandler;
        }

        protected override Task OnStart(IMessageSession session)
        {
            errorHandler.MessageSession = session;

            // needed to kickoff the sample
            var sendOptions = new SendOptions();
            sendOptions.RouteToThisEndpoint();
            return session.Send(new KickoffMessage(), sendOptions);
        }

        protected override Task OnStop(IMessageSession session)
        {
            errorHandler.Unsubscribe();
            return Task.CompletedTask;
        }
    }

}