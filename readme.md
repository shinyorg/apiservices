# SHINY SERVICE EXTENSIONS FOR .NET
<img src="https://github.com/shinyorg/shiny/raw/master/art/logo.png" width="100" /> 


## FEATURES
* Push Notification Management without a 3rd Party (think Azure Notification Hubs for your on-prem servers)
* Mail Templating On Steroids!  Loaders, parsers, converters, & senders!

## LINKS
* [Documentation](https://shinylib.net/server)
* [Push Sample](https://github.com/shinyorg/pushtester)
* [Community Support](https://github.com/shinyorg/shiny/discussions)


## SUPPORT SHINY

While Shiny is free and will continue to be so, maintenance and support takes a heavy toll on sustainability. If you or your company have the resources, please consider becoming a GitHub Sponsor. GitHub Sponsorships help to make Open Source Development more sustainable.

Depending on your Sponsorship Tier, you may also get access to some great benefits on Sponsor Connect (https://sponsorconnect.dev) including:
- The Sponsor Only Discord server
- Training available ONLY to sponsors on Sponsor Connect
- Special sponsor-only packages

[https://sponsor.shinylib.net](https://sponsor.shinylib.net)

How about some [Shiny Gear](https://www.redbubble.com/shop/ap/45038461)

## SQL Server Table Scripts

### Mail

```sql
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MailTemplates](
	[MailTemplateId] [uniqueidentifier] NOT NULL,
	[TemplateName] [nvarchar](255) NOT NULL,
	[CultureCode] [varchar](5) NULL,
	[Content] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_MailTemplates] PRIMARY KEY CLUSTERED
(
	[MailTemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_MailTemplates] UNIQUE NONCLUSTERED
(
	[TemplateName] ASC,
	[CultureCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```

### Push

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
