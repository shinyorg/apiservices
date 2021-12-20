using Shiny.Extensions.Localization.Impl;
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


        public LocalizationBuilder AddResource(string baseName, Assembly assembly, string? alias = null)
            => this.Add(new ResxLocalizationProvider(baseName, assembly, alias));


        public LocalizationBuilder AddAssemblyResources(Assembly assembly, bool trimAssemblyNames)
            => this.Add(new AssemblyResxLocalizationProvider(assembly, trimAssemblyNames));


        public ILocalizationManager Build()
        {
            var sources = new Dictionary<string, ILocalizationSource>();
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
