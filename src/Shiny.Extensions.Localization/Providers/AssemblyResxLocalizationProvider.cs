using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Shiny.Extensions.Localization.Providers
{
    public class AssemblyResxLocalizationProvider : ILocalizationProvider
    {
        readonly Assembly assembly;
        readonly bool trimAssemblyNames;
        readonly bool ignoreCase;


        public AssemblyResxLocalizationProvider(Assembly assembly, bool trimAssemblyNames, bool ignoreCase)
        {
            this.assembly = assembly;
            this.trimAssemblyNames = trimAssemblyNames;
            this.ignoreCase = ignoreCase;
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

                var source = new ResxLocalizationSource(
                    name,
                    this.assembly,
                    alias,
                    this.ignoreCase
                );
                sources.Add(source);
            }
            return sources.ToArray();
        }
    }
}
