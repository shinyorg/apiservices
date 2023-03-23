// SQL SERVER
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'MailTemplates' and xtype = 'U')
     CREATE TABLE [dbo].[MailTemplates](
	    [TemplateName] [nvarchar](255) NOT NULL PRIMARY KEY,
	    [CultureCode] [varchar](5) NULL PRIMARY KEY,
	    [Content] [nvarchar](max) NOT NULL
    )
GO

// POSTGRES
CREATE TABLE IF NOT EXISTS MailTemplates (
    TemplateName varchar(255) NOT NULL,
    CultureCode varchar(5) NULL,
    Content varchar(2000) NULL,
    PRIMARY KEY(TemplateName, CultureCode)
);


// SQLITE - use for testing
CREATE TABLE IF NOT EXISTS MailTemplates (
    TemplateName    TEXT NOT NULL,
    CultureCode TEXT NOT NULL,
    Content      TEXT,
    PRIMARY KEY (TemplateName, CultureCode)
);