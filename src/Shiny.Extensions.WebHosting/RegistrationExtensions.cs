using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Shiny;


public static class RegistrationExtensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        var assemblyFileNames = Directory
            .GetFiles(
                AppDomain.CurrentDomain.BaseDirectory,
                "BuildingOps.*.dll"
            )
            .Where(x => !Path
                .GetFileName(x)
                .Equals(
                    "BuildingOps.WebInfrastructure.dll",
                    StringComparison.InvariantCultureIgnoreCase
                )
            )
            .ToList();

        foreach (var assemblyFileName in assemblyFileNames)
        {
            var assembly = Assembly.LoadFrom(assemblyFileName);
            AppDomain.CurrentDomain.Load(assembly.GetName());

            var moduleTypes = assembly
                .GetTypes()
                .Where(x =>
                    x.IsPublic &&
                    x.IsClass &&
                    !x.IsAbstract &&
                    x.GetInterfaces().Any(y => y == typeof(IInfrastructureModule))
                )
                .ToList();

            foreach (var moduleType in moduleTypes)
            {
                Console.WriteLine("Registering Infrastructure Module: " + moduleType.FullName);
                var module = (IInfrastructureModule)Activator.CreateInstance(moduleType)!;
                builder.AddInfrastructureModule(module);
                Console.WriteLine("Successfully Registered Infrastructure Module: " + moduleType.FullName);
            }
        }
        return builder;
    }


    public static WebApplicationBuilder AddInfrastructureModule(this WebApplicationBuilder builder, IInfrastructureModule module)
    {
        module.Add(builder);
        builder.Services.AddTransient<IInfrastructureModule>(_ => module);
        return builder;
    }


    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        var maps = (IEnumerable<IInfrastructureModule>)app.Services.GetServices(typeof(IInfrastructureModule));
        foreach (var map in maps)
        {
            Console.WriteLine("Writing Infrastructure Module: " + map.GetType().FullName);
            map.Use(app);
            Console.WriteLine("Successfully Wired Infrastructure Module: " + map.GetType().FullName);
        }
        return app;
    }
}