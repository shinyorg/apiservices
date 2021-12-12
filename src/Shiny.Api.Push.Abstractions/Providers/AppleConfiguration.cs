namespace Shiny.Api.Push.Providers
{
    public class AppleConfiguration
    {
        public string Key { get; set; }
        public string KeyId { get; set; }

        public string TeamId { get; set; }
        public string AppBundleIdentifier { get; set; }
        public bool IsProduction { get; set;}
    }
}
