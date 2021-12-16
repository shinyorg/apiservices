using Shiny.Extensions.Push.Providers;
using System.Threading.Tasks;


namespace Shiny.Extensions.Push
{
    public interface INotificationDecorator<T>
    {
        Task Decorate(PushRegistration registration, Notification notification, T nativeNotification);
    }


    public interface IAppleNotificationDecorator : INotificationDecorator<AppleNotification> {}
    public interface IGoogleNotificationDecorator : INotificationDecorator<GoogleNotification> { }
}
