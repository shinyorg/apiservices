namespace Shiny.Api.Push.Ef.Infrastructure
{
    public class DbPushRegistration
    {
        public Guid Id { get; set; }
        public PushPlatform Platform { get; set; }
        public string DeviceToken { get; set; }
        public string? UserId { get; set; }
        public DateTimeOffset? DateExpiry { get; set; }
        public DateTimeOffset DateCreated { get; set; }

        public ICollection<DbPushTag> Tags { get; set; }
    }
}
