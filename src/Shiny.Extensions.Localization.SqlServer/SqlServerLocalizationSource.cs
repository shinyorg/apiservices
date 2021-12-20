using System.Globalization;


namespace Shiny.Extensions.Localization.SqlServer
{
    public class SqlServerLocalizationSource : ILocalizationSource
    {
        public string? this[string key] => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string? GetString(string key, CultureInfo? culture = null)
        {
            throw new NotImplementedException();
        }
    }
}
