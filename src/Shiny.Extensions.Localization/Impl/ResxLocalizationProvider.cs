using System.Reflection;


namespace Shiny.Extensions.Localization.Impl
{
    public class ResxLocalizationProvider : ILocalizationProvider
    {
        readonly string baseName;
        readonly Assembly assembly;
        readonly string? alias;


        public ResxLocalizationProvider(string baseName, Assembly assembly, string? alias)
        {
            this.baseName = baseName;
            this.assembly = assembly;
            this.alias = alias;
        }


        public ILocalizationSource[] Load() => new []
        {
            new ResxLocalizationSource(this.baseName, this.assembly, this.alias)
        };
    }
}
