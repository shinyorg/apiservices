using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SampleMobile
{
    public class AppSettings : ReactiveObject
    {
        [Reactive] public string ApiBaseUrl { get; set; } = "https://acrmonster:44372";
    }
}
