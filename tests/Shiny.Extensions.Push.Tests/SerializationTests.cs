using FluentAssertions;
using Shiny.Extensions.Push.Apple;
using Shiny.Extensions.Push.GoogleFirebase;
using Shiny.Extensions.Push.Infrastructure;
using System.Text.Json;
using Xunit;
using AppleInf = Shiny.Extensions.Push.Apple.Infrastructure;
using GoogleInf = Shiny.Extensions.Push.GoogleFirebase.Infrastructure;

namespace Shiny.Extensions.Push.Tests;


public class SerializationTests
{
    [Fact]
    public void AppleNotificationTests()
    {
        var apple = new AppleNotification
        {
            Aps = new()
            {
                Alert = new()
                {
                    Title = "Test Title",
                    Body = "Test Body"
                },
                Badge = 99
            },
            CustomData = new()
            {
                { "Test1", "one" },
                { "Test2", "two" }
            }
        };

        var json = AppleInf.Serializer.Serialize(apple);
        json.Should().Be("{\"aps\":{\"alert\":{\"title\":\"Test Title\",\"body\":\"Test Body\"},\"badge\":99},\"Test1\":\"one\",\"Test2\":\"two\"}");
    }


    [Fact]
    public void GoogleNotificationTests()
    {
        var google = new GoogleNotification
        {
            Android = new()
            {
                Notification = new()
                {
                    Title = "Test Title",
                    Body = "Test Body"
                }
            },
            Data = new()
            {
                { "Test1", "one" }
            }
        };
        var json = GoogleInf.Serializer.Serialize(google);
        json.Should().Be("{\"data\":{\"Test1\":\"one\"},\"android\":{\"notification\":{\"title\":\"Test Title\",\"body\":\"Test Body\"}}}");
    }
}
