namespace SampleWeb.Contracts
{
    public class Registration
    {
        public string? DeviceToken { get; set; }
        public string? UserId { get; set; }
        public bool UseApple { get; set; }
        public bool UseAndroid { get; set; }
        public string[]? Tags { get; set; }
    }
}
