# Localization

|Name|Nuget|
|----|-----|
|Main Library|[!NugetShield(Shiny.Extensions.Localization)]|
|SQL Server Plugin|[!NugetShield(Shiny.Extensions.Localization.SqlServer)]

Everyone has to do localization at some point in there career.  It can be tedious and time consuming.  Most of us .NET folk are accumstumed to using RESX resource files.  They're fast, efficient, and work well for general purpose localization efforts 

UNTIL THEY DON'T.. 

### Scenario 1:
Along comes that one customer who doesn't like the terminology your app uses, they think they own your app, and demand changes.  You also end up with that one product manager who is all to happy to answer the request.  This is where resource files are all of a sudden
in the way.  

### Scenario 2:
You want to add a new language, but no one in your organization knows the language.  Product reaches out to have translators come in.  The translators ask for a list of all keys.  They don't like XML or Visual Studio.

### Scenario 3:
You are a multi-tenanted app, you have a new multi-million $$$ customer that will come onboard if you can add their language.  Delivering builds is still painful because you know your devops pipeline will cause some sort of issue.  Why deliver a build just for a language?


## HOW DO WE SOLVE THIS?

Along comes Shiny.Extensions.Localization.  With the best abstraction around for localization.  You 

```csharp
using Shiny.Extensions.Localization;

var manager = new LocalizationBuilder()
    .AddAssemblyResources(this.GetType().Assembly)
    .AddSqlServer("Your Connection String")
    .AddResource("YourNamespace.YourResourceFile", this.GetType().Assembly) // useful for adding a specific resource file
    .Add(new YourOwnCustomLocalizationProvider())
    .Build();
```

Just like Microsoft.Extensions.Configuration, you can bring in resources from many different places and clump them all into one beautiful source.


## USE CASES

### Scenario 1: Strongly Typed Resources

Question: We use strongly typed resources everywhere?  It will hurt to remove them from every page.
Answer: Yes it will, so don't do it.  Our localization manager has a Bind option that will automatically (shown below).  Simply change all of your strongly typed file to be "public string YourKey { set; set; }"

```csharp
var manager = new LocalizationBuilder()
    .AddAssemblyResources(this.GetType().Assembly)
    .AddSqlServer("Your Connection String")
    .Build();

var obj = new YourStronglyTypedClass();
manager.Bind(obj);

var nowSet = obj.YourKey;

```

### Scenario 2: JSON Localization
Question: I have a React/Angular/New Popular JS library - I want to have all of my resources be serialized from the server
Answer: No problems!  Take a look:

```csharp
var manager = new LocalizationBuilder()
    .AddAssemblyResources(this.GetType().Assembly)
    .AddSqlServer("Your Connection String")
    .Build();

var allTranslations = manager.GetAllSectionsWithKeys(CultureInfo.CurrentCulture); // of whatever culture you support
var stringContent = JsonConvert.SerializeObject(allTranslations); // return it from your web api
```

### Scenario 3: XAML

Question: Can I use it in XAML?
Answer: Shiny started with Xamarin.  Most examples our there show how to use custom XAML binding.  Don't do this!  Do this instead via your viewmodel:

```csharp
// ideally, you will inject the Shiny.Extensions.Localization.ILocalizationManager with something like Prism
readonly ILocalizationManager manager;

this.manager = new LocalizationBuilder()
    .AddAssemblyResources(this.GetType().Assembly)
    .Build();

// you can also hone down to a specific resource using manager.GetSection
public string this[string key] => this.manager.GetString(key);
```

Now, in your XAML
```xml
<Label Text="{Binding [MySection.YourKey]}" /> 
```


## SQL Server Plugin

SQL Tables for mail templates

```sql
SET QUOTED_IDENTIFIER ON
GO

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
```