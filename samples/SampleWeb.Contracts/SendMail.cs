namespace SampleWeb.Contracts
{
    public class SendMail
    {
        public string? Subject { get; set; }

        public string? To { get; set; }
        public string[]? Cc { get; set; }
        public string[]? Bcc { get; set; }
        public string? AdditionalMessage { get; set; }
    }
}
