# Manage Your Education and Skills Funding User Interface

The Manage Your Education and Skills Funding (MYESF) UI allows the following:

- View current and previous allocations by organisation or funding type
- Allows to view funding for organisations: academies and free schools,city technology colleges,general hospitals,local authorities,local authority maintained schools,non-maintained special schools and pupil referral units.
- Allows to view funding type figures for: Dedicated schools grant and PE and sport premium.
- Allows to download the latest allocation data for Dedicated schools grant and PE and sport premium.
- Allows to see the allocation history of Dedicated schools grant and PE and sport premium.

## Provider

[The Department for Education](https://www.gov.uk/government/organisations/department-for-education)

## About this project

This project is using .NET 8.0 framework.

This project consists of `Pds.ViewYourFunding.Web`,`Pds.ViewYourFunding.Core`,`Pds.ViewYourFunding.Repositories`,`Pds.ViewYourFunding.Services`, `Pds.VYF.Services` and corresponding test projects.

The UI runs on an Azure App service on Azure.

The application is responsible for BAU work for MYESF such as updating and rolling over funding stream spreadsheets.

**Note:** The project is currently being updated to be containerised via Docker where the deployment method and target will change, this document will be updated when these changes have been finalised.

# Local Configuration Guide

In order to run the application locally a valid `appsettings.json` file will need to be created in the `Pds.ViewYourFunding.Web` projects Below, and included in the repo, there is `appsettings.example.json` which can be used as a base and populated with the required values, which can be retrieved from the Azure Portal.

**Note:** Additional appsettings might be required to run different environments. There are different appsettings files for each environment but follow the same structure. It is recommended to consult the existing developers on this.

## Application Settings (`appsettings.json`)

```json
{
  "APPINSIGHTS_INSTRUMENTATIONKEY": "",
  "ASPNETCORE_FORWARDEDHEADERS_ENABLED": "true",

  "Authentication": {
    "AppIdUrl": "",
    "ClientId": "",
    "ClientSecret": "",
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": ""
  },
  "BlobStorage": {
    "ContainerName": "",
    "Key": "",
    "ServiceName": ""
  },
  "CookieName": "",
  "CosmosDbConfiguration": {
    "AuditCollection": "",
    "ConnectionString": "",
    "ProviderFundingCollection": "",
    "CosmosConnectionMode": ""
  },
  "DfeSignIn": {
    "Cookie:Name": "",
    "DfeLegacyCodeId": "",
    "OpenIDConnect": {
      "Authority": "",
      "Clientid": "",
      "ClientSecret": ""
    },
    "PublicApi": {
      "clientid": "",
      "ClientSecret": "",
      "Tokenissuer": "",
      "url": ""
    }
  },
  "DfeSignInUrl": "",

  "DocumentGeneratorFundingReportsUrl": "",
  "DocumentGeneratorPdfComparerUrl": "",
  "DocumentGeneratorRerunUrl": "",
  "DocumentGeneratorUrl": "",

  "Environment": "",
  "FeedReaderUrl": "",
  "FundingDataApiEndPoint": "",
  "GlobalCacheTimeToLive": "5",
  "IdamsMetadataAddress": "",
  "IdamsRealm": "",
  "LoggedInProviderHomeLink": "",
  "MyesfLogoutUrl": "",
  "oidc": {
    "Authority": "",
    "ClientId": "",
    "ClientSecret": "",
    "PostLogOutUrl": "",
    "RedirectUrl": ""
  },
  "PdsApplicationInsights": {
    "Environment": "",
    "InstrumentationKey": ""
  },
  "RecentlyOpenedLocalAuthorities": {
    "FundingPeriodCode": "",
    "LocalAuthorityCodeList": ""
  },
  "RequestAuthorisationKey": "",
  "roleApi": {
    "ClientId": "",
    "ClientSecret": "",
    "TokenIssuer": "",
    "Url": ""
  },
  "Services": {
    "AdminApiClient": {
      "ApiBaseAddress": "",
      "AppUri": "",
      "Authority": "https://login.microsoftonline.com/",
      "ClientId": "",
      "ClientSecret": "",
      "TenantId": ""
    },
    "OrganisationApiClient": {
      "ApiBaseAddress": "",
      "AppUri": "",
      "Authority": "https://login.microsoftonline.com/",
      "ClientId": "",
      "ClientSecret": "",
      "TenantId": ""
    }
  },
  "TerminatedLocalAuthority": {
    "FinalPublicationDate": "",
    "FundingPeriodCode": "",
    "LocalAuthorityCode": ""
  },
  "ViewYourFundingApiBaseAddress": "",
  "WEBSITE_HEALTHCHECK_MAXPINGFAILURES": "5",
  "ConnectionStrings:vyf": ""
}
```

### Setting Details

- **`APPINSIGHTS_INSTRUMENTATIONKEY`**  
  App insights secret key

- **`Authentication:AppIdUrl`**  
  The intended recipient of the microsoft azure authentication token.
 
- **`Authentication:ClientId`**  
  The application (client) ID registered in microsoft azure.

- **`Authentication:ClientSecret`**  
  The application (client) ID registered in microsoft azure.

- **`Authentication:Instance`**  
  The URL of the microsoft azure service used to authenticate. (https://login.microsoftonline.com/)

- **`Authentication:TenantId`**  
  The unique identifier for your microsoft azure tenant.

- **`BlobStorage:ServiceName`**  
  The connection string for the UI related azure storage blob containers. use (pdsatsharedstr)

- **`BlobStorage:Key`**  
  The connection string key for the UI blobcontainers.

- **`BlobStorage:ContainerName`**  
  The connection string for the document exchange related azure storage blob containers. use (spreadsheets).

- **`CookieName`**  
  Cookie name value

- **`CosmosDbConfiguration:AuditCollection`**  
  The secret value for document exchange cosmos db resource. (Use 'audit')
  
- **`CosmosDbConfiguration:ConnectionString`**  
  The secret connection string calue for cosmosdb
  
- **`CosmosDbConfiguration:ProviderFundingCollection`**  
  The value for provider funding collections. (Use 'providerfunding')

- **`CosmosDbConfiguration:CosmosConnectionMode`**  
  The connection mode used for cosmosdb Use ('Gateway').
  
- **`CosmosDbConfiguration:AuditCollection`**  
  The secret value for document exchange cosmos db resource. (Use 'audit').

- **`AzureCosmosDb:ServiceEndpoint`**  
  The uri for the document exchange cosmos db resource.

- **`DfeSignIn:Cookie:Name`**  
  Dfe sign in cookie name.

- **`DfeSignIn:DfeLegacyCodeId`**  
  Dfe legacy code id values (2 number strings).
  
- **`DfeSignIn:OpenIDConnect:Authority`**  
  Dfe sign in open id adress link.
  
- **`DfeSignIn:OpenIDConnect:Clientid`**  
  The application (client) ID for DfE sign in Open ID Connect service.
  
- **`DfeSignIn:OpenIDConnect:ClientSecret`**  
  The application (client) secret for DfE sign in Open ID Connect service.
  
- **`DfeSignIn:PublicApi:Clientid`**  
  The application (client) ID for DfE sign in public api service.

- **`DfeSignIn:PublicApi:ClientSecret`**  
  The application (client) secret for DfE sign in public api service.
  
- **`DfeSignIn:PublicApi:Tokenissuer`**  
  The document exchange token identifier for DfE sign in public api service.

- **`DfeSignIn:PublicApi:url`**  
  The url used to access DfE sign in public api service.

- **`DfeSignInUrl`**  
  The url used to access DfE sign in service.

- **`DocumentGeneratorFundingReportsUrl`**  
  Url link for document generator funding reports.
  
- **`DocumentGeneratorPdfComparerUrl`**  
  Url link for document generator pdf file comparer.

- **`DocumentGeneratorRerunUrl`**  
  Url link for document generator rerun.
  
- **`Environment`**  
  The target environment string.
  
- **`FeedReaderUrl`**  
  Url link for the feed reader.
  
- **`FundingDataApiEndPoint`**  
  Url link for API endpoint.

- **`GlobalCacheTimeToLive`**  
  Numeric value for cache time.
  
- **`IdamsMetadataAddress`**  
  Url link idams metadata.

- **`IdamsRealm`**  
  Unique idams realm connection string.
  
- **`LoggedInProviderHomeLink`**  
  Url link for provider home.

- **`MyesfLogoutUrl`**  
  Url link for logging out of myesf service.
  
- **`oidc:Authority`**  
  oidc adress link.

- **`oidc:ClientId`**  
  The oidc ID for DfE sign in Open ID Connect service.

- **`oidc:ClientSecret`**  
  The oidc secret for DfE sign in public service.

- **`oidc:PostLogOutUrl`**  
  String path value for postlogout.

- **`oidc:RedirectUrl`**  
  String path to redirect to.
  
- **`PdsApplicationInsights:Environment`**  
  String of the target environment
  
- **`PdsApplicationInsights:InstrumentationKey`**  
  Unique string key value for application insights.

- **`RecentlyOpenedLocalAuthorities:FundingPeriodCode`**  
  Internal local authority code.
  
- **`RecentlyOpenedLocalAuthorities:LocalAuthorityCodeList`**  
  List of internal local authority codes (seperated by comma).

- **`RequestAuthorisationKey`**  
  Unique autherisation key value.
  
- **`roleApi:ClientId`**  
  The role API client id value.

- **`roleApi:ClientSecret`**  
  The role API secret value.
  
- **`roleApi:TokenIssuer`**  
  The role API user token value.

- **`roleApi:Url`**  
  Url Link for api testing.

- **`Services:AdminApiClient:ApiBaseAddress`**  
  Url link to the API admin client.

- **`Services:AdminApiClient:AppUri`**  
  Unique string for admin api client uri.

- **`Services:AdminApiClient:Authority`**  
  Microsoft authentication.

- **`Services:AdminApiClient:ClientId`**  
  Unique Id string for API admin client.

- **`Services:AdminApiClient:ClientSecret`**  
  Unique secret string for API admin client.

- **`Services:AdminApiClient:TenantId`**  
  Unique tenant Id string for API admin client.
  
- **`Services:OrganisationApiClient:ApiBaseAddress`**  
  Url link to the API Organisation client.

- **`Services:OrganisationApiClient:AppUri`**  
  Unique string for Organisation API client uri.

- **`Services:OrganisationApiClient:Authority`**  
  Microsoft authentication.

- **`Services:OrganisationApiClient:ClientId`**  
  Unique Id string for API Organisation client.

- **`Services:OrganisationApiClient:ClientSecret`**  
  Unique secret string for API Organisation client.

- **`Services:OrganisationApiClient:TenantId`**  
  Unique tenant Id string for API Organisation client.
  
- **`TerminatedLocalAuthority:FinalPublicationDate`**  
  Internal use datetime format final publication date value.
  
- **`TerminatedLocalAuthority:FundingPeriodCode`**  
  Internal use funding period code.

- **`TerminatedLocalAuthority:LocalAuthorityCode`**  
  Internal use number local authority code.
  
- **`ViewYourFundingApiBaseAddress`**  
  URL link for base of funding API.

- **`WEBSITE_HEALTHCHECK_MAXPINGFAILURES`**  
  Number value of times checked for failures
  
- **`ConnectionStrings:vyf`**  
  Unique vyf connection string



## docker-compose

This project depends on a redis distributed cache resource for storing api requests/responses. We are unable to connect to deployed cloud resources and so a local redis container must be created via Docker in order to test full functionality local.

The docker-compose.yml file includeds the orchestration for starting both the api and redis containers.

You must select docker-compose as the startup project to ensure that all dependent resources are running in Docker to run this solution locally.

## Test execution

In order to run the application locally a valid `appsettings.json` file will need to be created in the `Pds.ViewYourFunding.Automation.Tests` project. `appsettings.example.json`, in `Pds.DocumentExchange.Data.Api.Tests` can be used as a base and populated with appropriate values which can be found in Azure Portal. The local environment resources should be utilised.

## Test Application Settings (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "vyf": ""
  },
  "BlobStorage": {
    "ServiceName": "",
    "Key": "",
    "ContainerName": ""
  },
  "CosmosDbConfiguration": {
    "ConnectionString": "",
    "LayoutCollection": "",
    "Database": ""
  },
  "baseSiteUrl": "",
  "TestLoginUsername": "",
  "TestLoginPassword": "",
  "FundingApiSecretKey": "",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "CanWriteExpectedHtml": "true",
  "AllowedHosts": "*"
}
```

### Setting Details

- **`ConnectionStrings`**  
  Use local connection for vyf

- **`BlobStorage:ServiceName`**  
  The connection string for the UI related azure storage blob containers. use (fundingbloblocal)

- **`BlobStorage:Key`**  
  The connection string key for the UI

- **`BlobStorage:ContainerName`**  
  The connection string for the document exchange related azure storage blob containers. use (spreadsheets)

- **`CosmosDbConfiguration:ConnectionString`**  
  The secret value for document exchange cosmos db resource.
  
- **`CosmosDbConfiguration:LayoutCollection`**  
  The secret value for layout collection (Use `layout`)
  
- **`CosmosDbConfiguration:Database`**  
  The name of the cosmos database (Use `funding`)

- **`baseSiteUrl`**  
  The base site url ('use localhost')

- **`TestLoginUsername`**  
  The internal testing username

- **`TestLoginPassword`**  
  The internal testing password

- **`FundingApiSecretKey`**  
  The secret connection key for funding api

  
