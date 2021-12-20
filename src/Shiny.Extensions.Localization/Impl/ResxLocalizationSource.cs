using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;


namespace Shiny.Extensions.Localization.Impl
{
    public class ResxLocalizationSource : ILocalizationSource
    {
        readonly ResourceManager resources;


        public ResxLocalizationSource(string baseName, Assembly assembly, string? alias)
        {
            this.Name = alias ?? baseName;
            this.resources = new ResourceManager(baseName, assembly);
        }


        public string? GetString(string key, CultureInfo? culture = null)
        {
            var value = this.resources.GetString(key, culture);
            if (value == null)
                return $"KEY '{key}' not found";

            return value;
        }

        public IReadOnlyDictionary<string, string> GetValues(CultureInfo culture)
        {
            var dict = new Dictionary<string, string>();
            var resSet = this.resources.GetResourceSet(culture, true, true).GetEnumerator();
            while (resSet.MoveNext())
            {
                if (resSet.Value is string value)
                { 
                    var key = (string)resSet.Entry.Key;
                    dict.Add(key, value);
                }
            }
            return dict;
        }

        public string Name { get; }

        public string? this[string key] => this.GetString(key);
    }
}
