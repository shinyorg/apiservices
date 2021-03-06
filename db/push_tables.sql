/****** Object:  Table [dbo].[PushRegistrations]    Script Date: 12/16/2021 3:27:13 PM ******/
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
