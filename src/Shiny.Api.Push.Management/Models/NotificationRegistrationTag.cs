namespace Shiny.Api.Push.Management.Models
{
    public class NotificationRegistrationTag
    {
        public int Id { get; set; }
        public int NotificationRegistrationId { get; set; }
        public string Value { get; set; }

        public int RegistrationId { get; set; }
        public NotificationRegistrationModel Registration { get; set; }
    }
}
