namespace Shiny.Extensions.Webhooks.Tests;

using Microsoft.Extensions.DependencyInjection;
using Xunit;


public class RegistrationTests
{
    [Fact]
    public void Defaults()
    {
        var services = new ServiceCollection();
        services.AddWebHookManagement();
        var sp = services.BuildServiceProvider();

        sp.GetRequiredService<IWebHookManager>();
    }
}
