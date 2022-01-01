# Mail Templating

[!NugetShield(Shiny.Extensions.Mail)]

There are tons of email engines out there, but we didn't see anything as comphrehensive as what we've made here.

## The Engine Layers

### Template Loaders
These guys are responsible for going out and loading up your templates.  Templates can come from anywhere - a database, blob storage in azure, or even your local file system.  Why should it matter?

Shiny.Extensions.Mail provides several out-of-the-box providers
* FileSystem
* SqlServer
* Shiny.Storage - access to FTP, filesystem, Azure Blobs, etc - read more about it here: [Shiny.Storage](storage.md)

### Template Parsers
Template parsers are responsible for providing logic & raw parsing capabilities before taking the content and creating a mail message.  Here, you can really customize your emails by customer such as looping through a list of order items.

Shiny providers one parser out of the box - Razor

### MailMessage Converters
Mail message parsers are the last line before sending your message.  Here, we take the raw string after the loader and template parser have taken their runs and convert the content into a mail message.  This is where formatting of your template becomes more important.  Out of the box, we have what we call a "front matter mail template converter".  We will show you what this format looks like in the next section.
* Front Matter

### MailMessage Senders
The last layer of the engine is responsible for sending your message.  The culmination of all the things that have ran to get you here.... all by a couple of lines of code

Out of the box, Shiny provides two mechanisms:
* SMTP
* SendGrid


## Front Matter Mail Templates

This is something we thought was truly magical as you can do so many customizations at this level.  Below is an example of a front matter mail template with Razor

```
to: @Modal.To
from: @Modal.From ?? Do Not Reply <donotreply@shinylib.net>
subject: Hello World
cc: @Modal.SomeoneElseWhoWantsThis
bcc: admin@customer.com; 
---
All of your beautiful HTML content here
<h1>Order #@Model.OrderId</h1>
<ul>
    @foreach (var item in Model.OrderItems) {
        {<li>@item.Description</li>}
</ul>

```

## Getting Started

Setting Shiny.Extensions.Mail up is very straight forward, but does require the use of Microsoft.Extensions.DependencyInjection 

> [!WARNING]
> The Shiny.Extensions.Mail engine can certainly run within your ASP.NET 5+ applications, however, it is recommended that as your site grows in traffic, that you offload this to an Azure Function or a Service Bus Queue.  This will allow you to scale your site without having to worry about the performance of your mail engine.


```csharp

protected override void ConfigureServices(IServiceCollection services) 
{
    services.AddMailProcessor(x => x
        .UseSmtpSender(Configuration.GetSection("Mail:Smtp").Get<SmtpConfig>())
        .UseFileTemplateLoader("mailtemplates", "mailtemplate")
        //.UseSendGridSender(Configuration["Mail:SendGridApiKey"])
        // mail processor, razor parser, and front matter parser loaded automatically
    );
}

// now within your controller
public MyController(IMailProcessor mailProcessor)
{
    _mailProcessor = mailProcessor;
}

public async Task Order() {
    await _mailProcessor.Send(
        "YourTemplateName", 
        new {
            OrderId = 123,
            OrderItems = new[] {
                new {
                    Description = "Item 1",
                    Price = 10.00m
                },
                new {
                    Description = "Item 2",
                    Price = 20.00m
                }
            }
        },
        CultureInfo.CurrentCulture // optional - but should either user the culture from your browser headers or a customer profile setting within your application
    });
}
```