using System.Reflection;


namespace Shiny.Extensions.Localization.Providers
{
    public class ResxLocalizationProvider : ILocalizationProvider
    {
        readonly string baseName;
        readonly Assembly assembly;
        readonly string? alias;
        readonly bool ignoreCase;


        public ResxLocalizationProvider(string baseName, Assembly assembly, string? alias, bool ignoreCase)
        {
            this.baseName = baseName;
            this.assembly = assembly;
            this.alias = alias;
            this.ignoreCase = ignoreCase;
        }


        public ILocalizationSource[] Load() => new []
        {
            new ResxLocalizationSource(
                this.baseName,
                this.assembly,
                this.alias,
                this.ignoreCase
            )
        };
    }
}
