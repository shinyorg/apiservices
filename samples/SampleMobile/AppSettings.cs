using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Refit;
using System;
using System.Reactive.Linq;


namespace SampleMobile
{
    public class AppSettings : ReactiveObject
    {
        public AppSettings()
        {
            this.WhenAnyValue(x => x.ApiBaseUrl)
                .Skip(1)
                .Subscribe(_ => this.apiClient = null);
        }


        [Reactive] public string ApiBaseUrl { get; set; } = "https://acrmonster:44372";


        ISampleApi? apiClient = null;
        public ISampleApi ApiClient
        {
            get
            {
                this.apiClient ??= RestService.For<ISampleApi>(this.ApiBaseUrl);
                return apiClient;
            }
        }
    }
}
