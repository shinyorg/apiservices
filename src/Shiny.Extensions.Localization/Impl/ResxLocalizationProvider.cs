using System.Reflection;


namespace Shiny.Extensions.Localization.Impl
{
    public class ResxLocalizationProvider : ILocalizationProvider
    {
        readonly string baseName;
        readonly Assembly assembly;


        public ResxLocalizationProvider(string baseName, Assembly assembly)
        {
            this.baseName = baseName;
            this.assembly = assembly;
        }


        public ILocalizationSource[] Load() => new []
        {
            new ResxLocalizationSource(this.baseName, this.assembly)
        };
    }
}
