using System.Net.Http;

namespace Shiny
{
    public class PushConfiguration
    {
        public PushConfiguration(string registerUri, string unregisterUri)
        {
            this.RegisterUri = registerUri;
            this.UnRegisterUri = unregisterUri;
        }


        /// <summary>
        /// The value should have {token} in it somewhere and should be a GET
        /// </summary>
        public string RegisterUri { get; }

        /// <summary>
        /// The value should have {token} in it somewhere and should be a GET
        /// </summary>
        public string UnRegisterUri { get; }

        /// <summary>
        /// This is useful if you want to have a custom HTTP client where you can use HttpClientHandlers to alter the push requests to your server to add headers like auth tokens
        /// </summary>
        public HttpClient? Http { get; set; }
    }
}
