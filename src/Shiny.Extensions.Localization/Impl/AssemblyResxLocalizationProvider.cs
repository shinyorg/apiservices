using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Shiny.Extensions.Localization.Impl
{
    public class AssemblyResxLocalizationProvider : ILocalizationProvider
    {
        readonly Assembly assembly;
        readonly bool trimAssemblyNames;


        public AssemblyResxLocalizationProvider(Assembly assembly, bool trimAssemblyNames) 
        {
            this.assembly = assembly;
            this.trimAssemblyNames = trimAssemblyNames;
        }


        public ILocalizationSource[] Load()
        {
            var baseNames = this.assembly.GetManifestResourceNames();
            var sources = new List<ILocalizationSource>(baseNames.Length);

            foreach (var baseName in baseNames)
            {
                var name = baseName.Replace(".resources", "");
                var alias = name;
                if (this.trimAssemblyNames)
                    alias = alias.Split('.').Last();

                var source = new ResxLocalizationSource(name, this.assembly, alias);
                sources.Add(source);
            }
            return sources.ToArray();
        }
    }
}
