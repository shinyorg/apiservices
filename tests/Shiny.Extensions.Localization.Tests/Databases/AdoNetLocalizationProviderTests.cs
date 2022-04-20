namespace Shiny.Extensions.Localization.Tests.Databases;

using Xunit;
using Shiny.Extensions.Localization;
using FluentAssertions;
using Microsoft.Data.Sqlite;

public class AdoNetLocalizationProviderTests : AdoNetProviderTests<SqliteConnection>
{
    public AdoNetLocalizationProviderTests()
    {
        this.ConnectionString = "";
        this.ExecuteNonQuery(@"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Localizations' and xtype='U')

	CREATE TABLE [dbo].[Localizations](
		[LocalizationId][uniqueidentifier] NOT NULL,
		[AppIdentifier][varchar](50) NOT NULL,
		[Section][varchar](50) NOT NULL,
		[ResourceKey][varchar](50) NOT NULL,
		[CultureCode][varchar](5) NULL,
		[Value][nvarchar](4000) NOT NULL,
	 CONSTRAINT[PK_Localizations] PRIMARY KEY CLUSTERED
	(
		[LocalizationId] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON[PRIMARY],
	 CONSTRAINT[UK_Localizations] UNIQUE NONCLUSTERED
	(
	   [AppIdentifier] ASC,
	   [Section] ASC,
	   [ResourceKey] ASC,
	   [CultureCode] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON[PRIMARY]
	) ON[PRIMARY]
	GO
GO"
);
		this.ExecuteNonQuery("DELETE FROM Localizations");
    }


	[Fact]
	public void EndToEnd()
    {
		this.ExecuteNonQuery("INSERT INTO Localizations(AppIdentifier, Section, ResourceKey, CultureCode, Value) VALUES ('UnitTests', 'EndToEnd', 'HelloWorld', NULL, 'One')");

		var cfg = new LocalizationBuilder()
			.AddAdoNet<SqliteConnection>(this.ConnectionString!, "UnitTests")
			.Build();

		// testing case insensitivity as well
		var value = cfg.GetSection("unittests")?.GetString("helloWorld");
		value.Should().Be("One");
    }
}
