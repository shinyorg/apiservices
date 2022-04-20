using Shiny.Extensions.Localization.Infrastructure;
using Shiny.Extensions.Localization.Providers;

using System;
using System.Collections.Generic;
using System.Reflection;


namespace Shiny.Extensions.Localization
{
    public class LocalizationBuilder
    {
        readonly IList<ILocalizationProvider> providers = new List<ILocalizationProvider>();


        public LocalizationBuilder Add(ILocalizationProvider provider)
        {
            this.providers.Add(provider);
            return this;
        }


        public LocalizationBuilder AddResource(string baseName, Assembly assembly, string? alias = null, bool ignoreCase = true)
            => this.Add(new ResxLocalizationProvider(baseName, assembly, alias, ignoreCase));


        public LocalizationBuilder AddAssemblyResources(Assembly assembly, bool trimAssemblyNames, bool ignoreCase = true)
            => this.Add(new AssemblyResxLocalizationProvider(assembly, trimAssemblyNames, ignoreCase));


        public ILocalizationManager Build()
        {
            var sources = new Dictionary<string, ILocalizationSource>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var provider in this.providers)
            {
                var loadedSources = provider.Load();
                foreach (var source in loadedSources)
                {
                    if (sources.ContainsKey(source.Name))
                        throw new ArgumentException("This localize source already exists");

                    sources.Add(source.Name, source);
                }
            }
            return new LocalizationManager(sources);
        }
    }
}
