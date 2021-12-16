using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Refit;
using Shiny;
using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace SampleMobile
{
    public class AppSettings : ReactiveObject, IShinyStartupTask
    {
        readonly IPlatform platform;


        public AppSettings(IPlatform platform)
        {
            this.platform = platform;
            this.WhenAnyValue(x => x.ApiBaseUrl)
                .Skip(1)
                .Subscribe(_ => this.apiClient = null);
        }


        public void Start()
        {
            if (this.ApiBaseUrl != null)
                return;

            this.ApiBaseUrl = this.platform.IsAndroid()
                ? "https://10.0.2.2"
                : "https://localhost";

            //this.ApiBaseUrl = this.platform.IsAndroid()
            //    ? "https://10.0.2.2:44372"
            //    : "https://localhost:44372";
        }


        [Reactive] public string ApiBaseUrl { get; set; }


        ISampleApi? apiClient = null;
        public ISampleApi ApiClient
        {
            get
            {
                if (this.apiClient == null)
                {
                    var httpClient = new HttpClient(new MyHttpClientHandler())
                    {
                        BaseAddress = new(this.ApiBaseUrl)
                    };
                    this.apiClient = RestService.For<ISampleApi>(httpClient);
                }
                return apiClient;
            }
        }
    }


    public class MyHttpClientHandler : HttpClientHandler
    {
        public MyHttpClientHandler()
        {
            this.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{request.Method} {request.RequestUri}");
            var response = await base.SendAsync(request, cancellationToken);
            //response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);

            return response;
        }
    }
}
