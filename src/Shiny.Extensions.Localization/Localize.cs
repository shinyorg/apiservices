using System.Collections.Generic;
using System.Globalization;


namespace Shiny.Extensions.Localization
{
    public interface ILocalize<out TCategoryName> : ILocalizationSource {}
    public interface ILocalize : ILocalize<object> { }

    public class Localize<T> : ILocalize<T>
    {
        static readonly Dictionary<string, string> empty = new Dictionary<string, string>(0);
        readonly ILocalizationSource? source;


        public Localize(ILocalizationManager manager) => this.source = manager.GetSection(typeof(T).Name);
        public string? this[string key] => this.source?[key];
        public string Name => typeof(T).Name;
        public string? GetString(string key, CultureInfo? culture = null) => this.source?.GetString(key, culture);
        public IReadOnlyDictionary<string, string> GetValues(CultureInfo culture) => this.source?.GetValues(culture) ?? empty;
    }
}
