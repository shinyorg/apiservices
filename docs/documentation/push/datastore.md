# Data Storage

You will rarely ever work directly with the repository object, but Shiny uses it quite a bit under the hood to manage registration, query who should receive notifications, etc. 

## Entity Framework

Out of the box, Shiny offers only one data store mechanism based on Microsoft's Entity Framework.  Just install the following package [!NugetShield(Shiny.Extensions.Push.Ef)]

To setup the Entity Framework storage you must do the following, first - add the following to your EF Data Context.

```csharp
using System;
using Microsoft.EntityFrameworkCore;
using Shiny.Extensions.Push.Ef;
using Shiny.Extensions.Push.Ef.Infrastructure;

namespace YourNamespace
{
    public class YouDbContext : DbContext, IPushDbContext
    {
        // NOTE: this is also required because Shiny's repository uses DB Factories under the hood
        public YouDbContext(DbContextOptions<SampleDbContext> options) : base(options) { }

        // observe these classes to see how they will map
        public DbSet<DbPushRegistration> Registrations => this.Set<DbPushRegistration>();
        public DbSet<DbPushTag> Tags => this.Set<DbPushTag>();
    }
}
```

Second, we need to register your DB context as a factory.  In your .NET6 Program.cs, add the followin

```csharp
builder.Services.AddDbContextFactory<YouDbContext>(x =>
{
    var connString = builder.Configuration.GetConnectionString("Main");
    x.UseSqlServer(connString);
});
```

Lastly, we need to register the EF repository with Shiny Push Extensions like so

```csharp
builder.Services.AddPushManagement(x => x
    .AddApplePush(...) // left for example
    .UseEfRepository<YourDbContext>()
);
```

That's it.  You may need to customize the EF table objects that Shiny enforces to your liking.  

## Custom Data Store

There will be times you need some form of advanced data retrieval for any number of reasons.  Shiny does have a Shiny.Extensions.Push.Infrastructure.IRepository located in the Shiny.Extensions.Push.Abstractions nuget package

```csharp

builder.Services.AddPushManagement(x => x
    .AddApplePush(...)
    .AddGooglePush(...)
    .UseRepository<YourCustomRepository>() // NOTE: this will be registered as a singleton using this method
);

```