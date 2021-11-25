namespace SampleWeb.Contracts
{
    public class Registration
    {
        public string DeviceToken { get; set; }
        public string? UserId { get; set; }
        public string Platform { get; set; }
        public string[]? Tags { get; set; }
    }
}
