﻿using System.Threading.Tasks;


namespace Shiny.Api.Push.Providers
{
    public interface INotificationDecorator<T>
    {
        Task Decorate(NotificationRegistration registration, Notification notification, T nativeNotification);
    }


    public interface IAppleNotificationDecorator : INotificationDecorator<AppleNotification> {}
    public interface IGoogleNotificationDecorator : INotificationDecorator<GoogleNotification> { }
}