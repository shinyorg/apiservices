namespace Shiny.Extensions.Localization
{
    public interface ILocalizationProvider
    {
        ILocalizationSource[] Load();
    }
}
