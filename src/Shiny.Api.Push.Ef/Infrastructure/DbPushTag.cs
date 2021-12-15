namespace Shiny.Api.Push.Ef.Infrastructure
{
    public class DbPushTag
    {
        public Guid Id { get; set; }
        public string Value { get; set; }

        public Guid PushRegistrationId { get; set; }
        public DbPushRegistration PushRegistration { get; set; }
    }
}
