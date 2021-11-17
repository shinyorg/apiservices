using Shiny.Api.Push.Providers;

namespace Shiny.Api.Push
{
    public class PushConfigurator
    {
        public PushConfigurator AddDecorator(IAppleNotificationDecorator decorator)
        {
            return this;
        }

        public PushConfigurator AddDecorator(IGoogleNotificationDecorator decorator)
        {
            return this;
        }

        public PushConfigurator AddApple(AppleConfiguration configuration)
        {
            return this;
        }


        public PushConfigurator AddGoogle(GoogleConfiguration configuration)
        {
            return this;
        }


        public PushConfigurator AddApple<TImpl>(GoogleConfiguration configuration) where TImpl : IApplePushProvider
        {
            return this;
        }

        public PushConfigurator AddGoogle<TImpl>(GoogleConfiguration configuration) where TImpl : IGooglePushProvider
        {
            return this;
        }
    }
}
