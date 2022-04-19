namespace Shiny.Extensions.Webhooks.Tests;

using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Shiny.Extensions.Webhooks.Infrastructure;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;


public class WebHookTests
{
    //https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?msclkid=d874f631c00f11ecbb9e5f7d6d06e853&view=aspnetcore-6.0
    //https://andrewlock.net/introduction-to-integration-testing-with-xunit-and-testserver-in-asp-net-core/

    [Fact]
    public async Task EndToEnd_Request_Test()
    {
        using (var server = GetTestServer<TestRequest, TestResponse>(obj =>
        {
            return Task.FromResult(new TestResponse
            {
                Value = obj!.Arg == "hello" ? "world" : "NO"
            });
        }))
        {
            var manager = GetManager();
            var registration = new WebHookRegistration(
                "Test",
                "http://localhost/test",
                "",
                30
            );
            await manager.Subscribe(registration);

            var response = await manager.Request<TestResponse>(registration, new TestRequest { Arg = "hello" });
            response.Should().NotBeNull();
            response!.Value.Should().Be("world");
        }
    }


    [Fact]
    public async Task Timeout_Test()
    {
        using (var server = GetTestServer<TestRequest, TestResponse>(async obj =>
        {
            await Task.Delay(3000).ConfigureAwait(false);
            return null;
        }))
        {
            // I don't need a registration for this request
            var manager = GetManager();
            var registration = new WebHookRegistration(
                "Test",
                "http://localhost/test",
                "",
                2
            );
            try
            {
                await manager.Request<TestResponse>(registration, new TestRequest { Arg = "hello" });
                throw new Exception("Failed - should not have made it here");
            }
            catch (OperationCanceledException)
            {
                // yay
            }
            catch (Exception ex)
            {
                throw new Exception("Failed", ex);
            }
        }
    }


    static IWebHookManager GetManager()
    {
        var services = new ServiceCollection();
        services.AddWebHookManagement(x => x.UseRepository<InMemoryRepository>());
        var sp = services.BuildServiceProvider();

        return sp.GetRequiredService<IWebHookManager>();
    }


    //[Fact]
    //public async Task BadHash()
    //{
    //}


    static TestServer GetTestServer<TArgIn, TArgOut>(Func<TArgIn, Task<TArgOut>> execute)
    {
        var hostBuilder = new WebHostBuilder()
            .Configure(app =>
            {
                app.Run(async context =>
                {
                    using var reader = new StreamReader(context.Request.Body);
                    var jsonString = await reader.ReadToEndAsync().ConfigureAwait(false);
                    var obj = JsonSerializer.Deserialize<TArgIn>(jsonString);
                    var result = await execute(obj).ConfigureAwait(false);

                    var text = JsonSerializer.Serialize(result);
                    await context.Response.WriteAsync(text);
                });
            });

        var server = new TestServer(hostBuilder);
        Runner.TestHttpClient = server.CreateClient();
        return server;
    }
}

public class TestRequest
{
    public string? Arg { get; set; }
}

public class TestResponse
{
    public string? Value { get; set; }
}