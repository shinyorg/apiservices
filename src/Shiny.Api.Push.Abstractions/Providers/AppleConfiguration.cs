namespace Shiny.Api.Push.Providers
{
    public class AppleConfiguration
    {
        public string P8PrivateKey { get; set; }

        /// <summary>
        /// 10 digit p8 certificate id. Usually a part of a downloadable certificate filename
        /// </summary>
        public string P8PrivateKeyId { get; set; }


        public string TeamId { get; set; }
        public string AppBundleIdentifier { get; set; }
        public bool IsProduction { get; set;}
    }
}
