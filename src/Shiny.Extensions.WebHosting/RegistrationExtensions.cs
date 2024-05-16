using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Shiny;


public static class RegistrationExtensions
{
    public static WebApplicationBuilder AddInfrastructureForAppDomain(this WebApplicationBuilder builder, Func<string, bool>? predicate = null)
    {
        predicate ??= x => !x.StartsWith("System.");
        
        var assemblies = Directory
            .GetFiles(
                AppDomain.CurrentDomain.BaseDirectory,
                "*.dll"
            )
            .Where(x => predicate.Invoke(Path.GetFileName(x)))
            .Select(x =>
            {
                var assembly = Assembly.LoadFrom(x);
                AppDomain.CurrentDomain.Load(assembly.GetName());
                return assembly;
            })
            .ToArray();

        return builder.AddInfrastructure(assemblies);
    }
    
    
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
            throw new InvalidOperationException("No assemblies passed to scan");
        
        foreach (var assembly in assemblies)
        {
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
                builder.AddInfrastructureModules(module);
                Console.WriteLine("Successfully Registered Infrastructure Module: " + moduleType.FullName);
            }
        }
        return builder;
    }
    

    public static WebApplicationBuilder AddInfrastructureModules(this WebApplicationBuilder builder, params IInfrastructureModule[] modules)
    {
        foreach (var module in modules)
        {
            module.Add(builder);
            builder.Services.AddTransient<IInfrastructureModule>(_ => module);
        }
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