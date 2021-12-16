namespace Shiny.Extensions.Mail
{
    public class SmtpConfig
    {
        public string? Host { get; set; }
        public int? Port { get; set; }
        public string? PickupDirectoryLocation { get; set; }
        public bool EnableSsl { get; set; } = true;
    }
}
