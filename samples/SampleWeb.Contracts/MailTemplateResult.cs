namespace SampleWeb.Contracts
{
    public class Address
    {
        public string DisplayName { get; set; }
        public string Value { get; set; }
    }


    public class MailTemplateResult
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public Address From { get; set; }
        public Address[] To { get; set; }
        public Address[] Cc { get; set; }
        public Address[] Bcc { get; set; }
    }
}
