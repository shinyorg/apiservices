SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- WIHTOUT APP IDENTIFIER
CREATE TABLE [dbo].[Localizations](
	[LocalizationId] [uniqueidentifier] NOT NULL,
	[Section] [varchar](50) NOT NULL,
	[ResourceKey] [varchar](50) NOT NULL,
	[CultureCode] [varchar](5) NULL,
	[Value] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_Localizations] PRIMARY KEY CLUSTERED
(
	[LocalizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_Localizations] UNIQUE NONCLUSTERED
(
	[Section] ASC,
	[ResourceKey] ASC,
	[CultureCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



-- WITH APP IDENTIFIER

CREATE TABLE [dbo].[Localizations](
	[LocalizationId] [uniqueidentifier] NOT NULL,
	[AppIdentifier] [varchar](50) NOT NULL,
	[Section] [varchar](50) NOT NULL,
	[ResourceKey] [varchar](50) NOT NULL,
	[CultureCode] [varchar](5) NULL,
	[Value] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_Localizations] PRIMARY KEY CLUSTERED
(
	[LocalizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_Localizations] UNIQUE NONCLUSTERED
(
	[AppIdentifier] ASC,
	[Section] ASC,
	[ResourceKey] ASC,
	[CultureCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO