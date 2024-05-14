using System.Reflection;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;

namespace Shiny.Extensions.WebHosting.Tests;


public class RegistrationTests
{
    public RegistrationTests()
    {
        Module1.Reset();
        Module2.Reset();
    }
    
    
    [Fact]
    public void DidRegisterAll_AppDomain()
    {
        var builder = WebApplication.CreateBuilder();
        builder.AddInfrastructureForAppDomain(x => x.StartsWith("Shiny."));
        this.FireAll(builder);
    }


    [Fact]
    public void DidRegisterAll_SingleAssembly()
    {
        var builder = WebApplication.CreateBuilder();
        builder.AddInfrastructure(Assembly.GetExecutingAssembly());
        this.FireAll(builder);
    }

    
    [Fact]
    public void DidRegister_OnlyOneModule()
    {
        var builder = WebApplication.CreateBuilder();
        builder.AddInfrastructure(new Module1());
        
        Module1.AddCalled.Should().BeTrue("Module1.Add should be true");
        Module1.UseCalled.Should().BeFalse("Module1.Use should not have been fired");
        Module2.AddCalled.Should().BeFalse("Module2.Add should be false");
        Module2.UseCalled.Should().BeFalse("Module2.Use should not have been fired");
        var app = builder.Build();
        app.UseInfrastructure();
        Module1.UseCalled.Should().BeTrue("Module1.Use should have been fired");
        Module2.UseCalled.Should().BeFalse("Module2.Use should nopt have been fired");
    }
    

    void FireAll(WebApplicationBuilder builder)
    {
        Module1.AddCalled.Should().BeTrue("Module1.Add should be true");
        Module1.UseCalled.Should().BeFalse("Module1.Use should not have been fired");
        Module2.AddCalled.Should().BeTrue("Module2.Add should be true");
        Module2.UseCalled.Should().BeFalse("Module2.Use should not have been fired");     
        var app = builder.Build();

        app.UseInfrastructure();
        Module1.UseCalled.Should().BeTrue("Module1.Use should have been fired");
        Module2.UseCalled.Should().BeTrue("Module2.Use should have been fired");
    }
}


public class Module1 : IInfrastructureModule
{
    public static bool AddCalled { get; private set; }
    public static bool UseCalled { get; private set; }

    public static void Reset()
    {
        AddCalled = false;
        UseCalled = false;
    }
    
    public void Add(WebApplicationBuilder builder)
    {
        AddCalled = true;
    }

    public void Use(WebApplication app)
    {
        UseCalled = true;
    }
}


public class Module2 : IInfrastructureModule
{
    public static bool AddCalled { get; private set; }
    public static bool UseCalled { get; private set; }

    public static void Reset()
    {
        AddCalled = false;
        UseCalled = false;
    }
    
    public void Add(WebApplicationBuilder builder)
    {
        AddCalled = true;
    }

    public void Use(WebApplication app)
    {
        UseCalled = true;
    }
}