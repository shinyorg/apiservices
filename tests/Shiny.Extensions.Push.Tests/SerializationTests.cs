namespace Shiny.Extensions.Push.Tests;

using Shiny.Extensions.Push.Infrastructure;
using Shiny.Extensions.Push.Providers;
using System.Text.Json;
using Xunit;


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
                }
            }
        };
        var json = Serializer.Serialize(apple);
        var apple2 = JsonSerializer.Deserialize<AppleNotification>(json);

        apple.Aps.Alert.Title.Equals(apple2.Aps.Alert.Title);
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
            }
        };
        var json = Serializer.Serialize(google);
        var google2 = JsonSerializer.Deserialize<GoogleNotification>(json);

        google.Android.Notification.Title.Equals(google2.Android.Notification.Title);
    }
}
