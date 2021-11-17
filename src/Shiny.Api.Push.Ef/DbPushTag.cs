namespace Shiny.Api.Push.Ef
{
    public class DbPushTag
    {
        public Guid Id { get; set; }
        public string Value { get; set; }

        public Guid NotificationRegistrationId { get; set; }
        public DbPushRegistration Registration { get; set; }
    }
}
