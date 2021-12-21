# SHINY SERVICE EXTENSIONS FOR .NET
<img src="https://github.com/shinyorg/shiny/raw/master/art/logo.png" width="100" /> 

## COMING SOON

## FEATURES
* Push Notification Management without a 3rd Party (think Azure Notification Hubs for your on-prem servers)
* Mail Templating On Steroids!
* Async File Providers
	* Local File System
	* Azure Blob Storage
	* FTP

## LINKS
* [Documentation](https://shinylib.net/apiservices)
* [Samples](https://github.com/shinyorg/apservices/tree/master/samples)
* [Community Support](https://github.com/shinyorg/shiny/discussions)

## SUPPORT SHINY

While Shiny is free and will continue to be so, maintenance and support takes a heavy toll on sustainability. If you or your company have the resources, please consider becoming a GitHub Sponsor. GitHub Sponsorships help to make Open Source Development more sustainable.

Depending on your Sponsorship Tier, you may also get access to some great benefits on Sponsor Connect (https://sponsorconnect.dev) including:
- The Sponsor Only Discord server
- Training available ONLY to sponsors on Sponsor Connect
- Special sponsor-only packages

[https://sponsor.shinylib.net](https://sponsor.shinylib.net)

How about some [Shiny Gear](https://www.redbubble.com/shop/ap/45038461)

## BUILDS

Branch|Status
------|------
Master|![Build](https://img.shields.io/github/workflow/status/shinyorg/shiny/Build/master?style=for-the-badge)|
Dev|![Build](https://img.shields.io/github/workflow/status/shinyorg/shiny/Build/dev?style=for-the-badge)|
Preview|![Build](https://img.shields.io/github/workflow/status/shinyorg/shiny/Build/preview?style=for-the-badge)|

## NUGETS

Name|Stable|Preview
----|------|-------
Shiny.Extensions.Push|![Nuget](https://img.shields.io/nuget/v/shiny.extensions.push?style=for-the-badge)|![Nuget (Preview)](https://img.shields.io/nuget/vpre/shiny.extensions.push?style=for-the-badge)
Shiny.Extensions.Push.Ef|![Nuget](https://img.shields.io/nuget/v/shiny.extensions.push.ef?style=for-the-badge)|![Nuget (Preview)](https://img.shields.io/nuget/vpre/shiny.extensions.push.ef?style=for-the-badge)
Shiny.Extensions.Mail|![Nuget](https://img.shields.io/nuget/v/shiny.extensions.mail?style=for-the-badge)|![Nuget (Preview)](https://img.shields.io/nuget/vpre/shiny.extensions.mail?style=for-the-badge)
Shiny.Extensions.Localization|![Nuget](https://img.shields.io/nuget/v/shiny.extensions.localization?style=for-the-badge)|![Nuget (Preview)](https://img.shields.io/nuget/vpre/shiny.extensions.localization?style=for-the-badge)
Shiny.Extensions.Localization.SqlServer|![Nuget](https://img.shields.io/nuget/v/shiny.extensions.localization.sqlserver?style=for-the-badge)|![Nuget (Preview)](https://img.shields.io/nuget/vpre/shiny.extensions.localization.sqlserver?style=for-the-badge)
Shiny.Storage|![Nuget](https://img.shields.io/nuget/v/shiny.storage?style=for-the-badge)|![Nuget (Preview)](https://img.shields.io/nuget/vpre/shiny.storage?style=for-the-badge)

### DOCS TODO
* Push Notification Management
	* General Setup
	* Global Decorators
	* Contextual Decorators
	* Notification Reporters
	* Repository
	* Vendor Documentation
		* [Firebase Cloud Messaging Documentation](https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages)
		* [Apple Notification Documentation](https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/generating_a_remote_notification)
* Localization
	* General Setup
	* Resource Files
	* Database
* Mail Templates
	* General Setup
	* Processor
	* Template Loader
	* Template Parser
	* Mail Template Converter
	* Sender
* Async File Providers
	* General Setup
	* File System
	* Azure Blob Storage
	* FTP Client