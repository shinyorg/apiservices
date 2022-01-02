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

That's it.  You may need to customize the EF table objects that Shiny enforces to your liking.  In any case, here is the default SQL script to create tables

```sql
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PushRegistrations](
	[PushRegistrationId] [uniqueidentifier] NOT NULL,
	[Platform] [int] NOT NULL,
	[DeviceToken] [varchar](512) NOT NULL,
	[UserId] [varchar](50) NULL,
	[DateUpdated] [datetimeoffset](7) NOT NULL,
	[DateCreated] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_PushRegistrations] PRIMARY KEY CLUSTERED
(
	[PushRegistrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_PushRegistrations] UNIQUE NONCLUSTERED
(
	[DeviceToken] ASC,
	[Platform] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PushTags]    Script Date: 12/16/2021 3:27:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PushTags](
	[PushTagId] [uniqueidentifier] NOT NULL,
	[PushRegistrationId] [uniqueidentifier] NOT NULL,
	[Value] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PushTags] PRIMARY KEY CLUSTERED
(
	[PushTagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_PushTags] UNIQUE NONCLUSTERED
(
	[PushRegistrationId] ASC,
	[Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PushTags]  WITH CHECK ADD  CONSTRAINT [FK_PushTags_PushRegistrations] FOREIGN KEY([PushRegistrationId])
REFERENCES [dbo].[PushRegistrations] ([PushRegistrationId])
GO
ALTER TABLE [dbo].[PushTags] CHECK CONSTRAINT [FK_PushTags_PushRegistrations]
GO
```


## Custom Data Store

There will be times you need some form of advanced data retrieval for any number of reasons.  Shiny does have a Shiny.Extensions.Push.Infrastructure.IRepository located in the Shiny.Extensions.Push.Abstractions nuget package

```csharp

builder.Services.AddPushManagement(x => x
    .AddApplePush(...)
    .AddGooglePush(...)
    .UseRepository<YourCustomRepository>() // NOTE: this will be registered as a singleton using this method
);

```