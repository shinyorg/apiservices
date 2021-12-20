using System.Collections.Generic;
using System.Globalization;


namespace Shiny.Extensions.Localization.SqlServer
{
    public class SqlServerLocalizationSource : ILocalizationSource
    {
        readonly IList<string> rawKeys;
        readonly Dictionary<string, string> values;


        public SqlServerLocalizationSource(string section, IList<string> rawKeys, Dictionary<string, string> values)
        { 
            this.Name = section;
            this.rawKeys = rawKeys;
            this.values = values;
        }


        public string Name { get; }
        public string? this[string key] => this.GetString(key);
        public string? GetString(string key, CultureInfo? culture = null)
        {
            if (culture == null)
                return this.GetByKey(key, null);

            var value = this.GetByKey(key, culture.Name) ?? this.GetByKey(key, culture.TwoLetterISOLanguageName) ?? this.GetByKey(key, null);
            return value;
        }


        string? GetByKey(string key, string? code)
        {
            var fullKey = $"{key}_{code}";
            return this.values.ContainsKey(fullKey) ? this.values[fullKey] : null;
        }


        public IReadOnlyDictionary<string, string> GetValues(CultureInfo culture)
        {
            var dict = new Dictionary<string, string>();
            foreach (var key in this.rawKeys)
            {
                var value = this.GetString(key, culture);
                dict.Add(key, value);
            }
            return dict;
        }
    }
}
