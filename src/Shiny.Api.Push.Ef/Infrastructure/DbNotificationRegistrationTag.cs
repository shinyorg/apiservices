namespace Shiny.Api.Push.Ef.Infrastructure
{
    public class DbNotificationRegistrationTag
    {
        public Guid Id { get; set; }
        public string Value { get; set; }

        public Guid NotificationRegistrationId { get; set; }
        public DbNotificationRegistration Registration { get; set; }
    }
}
