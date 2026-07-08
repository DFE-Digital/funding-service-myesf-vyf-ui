# Manage Your Education and Skills Funding User Interface

The Manage Your Education and Skills Funding (MYESF) UI allows the following:

- View current and previous allocations by organisation or funding type
- Allows users to view funding for organisations: academies and free schools,city technology colleges,general hospitals,local authorities,local authority maintained schools,non-maintained special schools and pupil referral units.
- Allows users to view funding type figures for: Dedicated schools grant and PE and sport premium.
- Allows users to download the latest allocation data for Dedicated schools grant and PE and sport premium.
- Allows users to see the allocation history of Dedicated schools grant and PE and sport premium.
- Allows authorised users to view digital versions of General Annual Grant (GAG) statements.

## Provider

[The Department for Education](https://www.gov.uk/government/organisations/department-for-education)

## About this project

This project is an ASP.NET Core 8 web api utilising Azure App Service for deployment.

The web api runs on an Azure App service on Azure.

**Note:** The project is currently being updated to be containerised via Docker where the deployment method and target will change, this document will be updated when these changes have been finalised.

# Local Configuration Guide

In order to run the application locally a valid `appsettings.json` file will need to be created in the `Pds.ViewYourFunding.Web` projects Below, and included in the repo, there is `appsettings.example.json` which can be used as a base and populated with the required values, which can be retrieved from the Azure Portal.

## Application Settings (`appsettings.json`)

```json
{
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
  "Logging": {
    "ApplicationInsights": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft": "Error"
      }
    },
    "LogLevel": {
      "Default": "Information"
    }
  },
  "MSClarityId": "",
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
    }
  },
  "TerminatedLocalAuthority": {
    "FinalPublicationDate": "",
    "FundingPeriodCode": "",
    "LocalAuthorityCode": ""
  },
  "ViewYourFundingApiBaseAddress": "",
  "WEBSITE_HEALTHCHECK_MAXPINGFAILURES": "5",
  "ConnectionStrings": {
    "vyf": ""
  }
}
```

### Setting Details

- **`Authentication:AppIdUrl`**  
  The intended recipient of the microsoft azure authentication token for the VYF data api.
 
- **`Authentication:ClientId`**  
  The application (client) ID registered in azure ad for the VYF data api.

- **`Authentication:ClientSecret`**  
  The application (client) ID registered in azure ad for the VYF data api.

- **`Authentication:Instance`**  
  The url of the azure ad service used to authenticate the admin api.

- **`Authentication:TenantId`**  
  The unique identifier for the admin api azure ad tenant.

- **`BlobStorage:ServiceName`**  
  The name of the azure blob storage account for the UI related azure storage blob containers.

- **`BlobStorage:Key`**  
  The access policy key for the azure blob storage account for the UI related blob containers.

- **`BlobStorage:ContainerName`**  
  The name of the blob storage container used for storage purposes.

- **`CookieName`**  
  The name given to the cookie used for cross service data access.

- **`CosmosDbConfiguration:AuditCollection`**  
  The name of the cosmos db collection used for audit purposes.
  
- **`CosmosDbConfiguration:ConnectionString`**  
  The connection string value used for accessing the VYF cosmos db service.
  
- **`CosmosDbConfiguration:ProviderFundingCollection`**  
  The name of the cosmos db collection used for provider funding data.

- **`CosmosDbConfiguration:CosmosConnectionMode`**  
  The connection mode used for accessing the VYF cosmos db service.

- **`DfeSignIn:Cookie:Name`**  
  The name given to the cookie used for Dfe Sign In authentication.

- **`DfeSignIn:DfeLegacyCodeId`**  
  The identifier for the Dfe legacy code used for Dfe Sign In authentication.
  
- **`DfeSignIn:OpenIDConnect:Authority`**  
  The authority URL for DfE sign in Open ID Connect service.
  
- **`DfeSignIn:OpenIDConnect:Clientid`**  
  The application (client) ID for DfE sign in Open ID Connect service.
  
- **`DfeSignIn:OpenIDConnect:ClientSecret`**  
  The application (client) secret for DfE sign in Open ID Connect service.
  
- **`DfeSignIn:PublicApi:Clientid`**  
  The application (client) ID for DfE sign in public api service.

- **`DfeSignIn:PublicApi:ClientSecret`**  
  The application (client) secret for DfE sign in public api service.
  
- **`DfeSignIn:PublicApi:Tokenissuer`**  
  The identifier for the token issuer for DfE sign in public api service.

- **`DfeSignIn:PublicApi:url`**  
  The url used to access DfE sign in public api service.

- **`DfeSignInUrl`**  
  The url used to access DfE sign in service.

- **`DocumentGeneratorFundingReportsUrl`**  
  The url for the document generator funding reports function app http trigger.
  
- **`DocumentGeneratorPdfComparerUrl`**  
  The url for document generator pdf file comparer function app http trigger.

- **`DocumentGeneratorRerunUrl`**  
  The url for document generator rerun function app http trigger.

- **`DocumentGeneratorRerunUrl`**  
  The url for document generator base function app http trigger.
  
- **`Environment`**  
  The target environment string.
  
- **`FeedReaderUrl`**  
  The url for the funding feed reader base function app http trigger
  
- **`FundingDataApiEndPoint`**  
  The url for the VYF data api.

- **`GlobalCacheTimeToLive`**  
  The default value for cache time to live.
  
- **`LoggedInProviderHomeLink`**  
  The url for the VYF logged in home page.

- **`Logging:ApplicationInsights:LogLevel:Default`**
  The default logging level for the service when logging to Application Insights; refer to the [Microsoft Documentation](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel?view=net-9.0-pp) for an explanation of the different levels.

- **`Logging:ApplicationInsights:LogLevel:Microsoft`**
  The default logging level for Microsoft specific information when logging to Application Insights; refer to the [Microsoft Documentation](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel?view=net-9.0-pp) for an explanation of the different levels.

- **`Logging:LogLevel:Default`**
  The default logging level for the service; refer to the [Microsoft Documentation](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel?view=net-9.0-pp) for an explanation of the different levels.

- **`MyesfLogoutUrl`**  
  The url for logging out of the MYESF service.
  
- **`oidc:Authority`**  
  The authority URL for DfE sign in Open ID Connect service.

- **`oidc:ClientId`**  
  The application (client) ID for DfE sign in Open ID Connect service.

- **`oidc:ClientSecret`**  
  The application (client) secret for DfE sign in Open ID Connect service.

- **`oidc:PostLogOutUrl`**  
  The url for logging out of the DfE sign in Open ID Connect service.

- **`oidc:RedirectUrl`**  
  The redirect url for the DfE sign in Open ID Connect service.
  
- **`PdsApplicationInsights:InstrumentationKey`**  
  The key value for Application Insights resource for logging purposes.

- **`PdsApplicationInsights:Environment`**  
  The environment which the app is running on for Application Insights for logging purposes.

- **`RecentlyOpenedLocalAuthorities:FundingPeriodCode`**  
  The funding period code stored for recently opened local authorities used for specific logic.
  
- **`RecentlyOpenedLocalAuthorities:LocalAuthorityCodeList`**  
  The local authority codes stored for recently opened local authorities used for specific logic.

- **`RequestAuthorisationKey`**  
  The secret key used for authorising requests to the external funding api.
  
- **`roleApi:ClientId`**  
  The application (client) ID for DfE sign in public api service.

- **`roleApi:ClientSecret`**  
  The application (client) secret for DfE sign in public api service.
  
- **`roleApi:TokenIssuer`**  
  The identifier for the token issuer for DfE sign in public api service.

- **`roleApi:Url`**  
  The url used to access DfE sign in public api service.

- **`Services:AdminApiClient:ApiBaseAddress`**  
  The url for the admin api.

- **`Services:AdminApiClient:AppUri`**  
  The intended recipient of the microsoft azure authentication token for the admin api.

- **`Services:AdminApiClient:Authority`**  
  The url of the azure ad service used to authenticate the admin api.

- **`Services:AdminApiClient:ClientId`**  
  The application (client) ID registered in azure ad for the admin api.

- **`Services:AdminApiClient:ClientSecret`**  
  The application (client) ID registered in azure ad for the admin api.

- **`Services:AdminApiClient:TenantId`**  
  The unique identifier for the admin api azure ad tenant.
  
- **`TerminatedLocalAuthority:FinalPublicationDate`**  
  The final publication dates of terminated local authorities used for specific logic.
  
- **`TerminatedLocalAuthority:FundingPeriodCode`**  
  The funding period codes of terminated local authorities used for specific logic.

- **`TerminatedLocalAuthority:LocalAuthorityCode`**  
  The local authority codes of terminated local authorities used for specific logic.
  
- **`ViewYourFundingApiBaseAddress`**  
  The url of the VYF external api.
  
- **`ConnectionStrings:vyf`**  
  The connection string to the VYF database.

## Test execution

### Pds.ViewYourFunding.Web.Tests

In order to test the project locally a valid `appsettings.json` file will need to be created in the `Pds.ViewYourFunding.Web.Tests` project. `appsettings.example.json`, in `Pds.ViewYourFunding.Web.Tests` can be used as a base and populated with appropriate values which can be found in Azure Portal. The local environment resources should be utilised.

## Test Application Settings (`appsettings.json`)

```json
{
  "BlobStorage:ContainerName": "",
  "BlobStorage:Key": "",
  "BlobStorage:ServiceName": ""
}
```

### Setting Details

- **`BlobStorage:ContainerName`**  
  The name of the blob storage container used for storage purposes. (Use `pdsdevsharedstr`)

- **`BlobStorage:Key`**  
  The access policy key for the azure blob storage account for the UI related blob containers.

- **`BlobStorage:ServiceName`**  
  The name of the azure blob storage account for the UI related azure storage blob containers. (Use `spreadsheets1`)

### Pds.ViewYourFunding.Services.Tests

In order to test the project locally a valid `appsettings.json` file will need to be created in the `Pds.ViewYourFunding.Services.Tests` project. `appsettings.example.json`, in `Pds.ViewYourFunding.Services.Tests` can be used as a base and populated with appropriate values which can be found in Azure Portal. The local environment resources should be utilised.

## Test Application Settings (`appsettings.json`)

```json
{
  "CosmosDbConfiguration:ConnectionString": "",
  "CosmosDbConfiguration:DatabaseName": "",
  "CosmosDbConfiguration:CosmosConnectionMode": ""
}
```

### Setting Details
  
- **`CosmosDbConfiguration:ConnectionString`**  
  The connection string value used for accessing the VYF cosmos db service. (Use `pds-dev-shared-cdb`)
  
- **`CosmosDbConfiguration:DatabaseName`**  
  The name of the cosmos db database used for VYF. (Use `funding`)

- **`CosmosDbConfiguration:CosmosConnectionMode`**  
  The connection mode used for accessing the VYF cosmos db service. (Use `Gateway`)

### Pds.ViewYourFunding.Automation.Tests

In order to test the project locally a valid `appsettings.json` file will need to be created in the `Pds.ViewYourFunding.Services.Tests` project. `appsettings.example.json`, in `Pds.ViewYourFunding.Services.Tests` can be used as a base and populated with appropriate values which can be found in Azure Portal. The local environment resources should be utilised.

The web application must also be running in order for the automation tests to run successfully. Start the web application **without** debugging (Ctrl + F5) using the `ViewYourFunding.Web` profile.

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
    "Database": "",
    "CosmosConnectionMode": ""
  },
  "baseSiteUrl": "",
  "FundingApiSecretKey": "",
  "CanWriteExpectedHtml": "true"
}
```

### Setting Details
  
- **`ConnectionStrings:vyf`**  
  The connection string to the VYF database. (Use local db instance)

- **`BlobStorage:ContainerName`**  
  The name of the blob storage container used for storage purposes. (Use `pdsdevsharedstr`)

- **`BlobStorage:Key`**  
  The access policy key for the azure blob storage account for the UI related blob containers.

- **`BlobStorage:ServiceName`**  
  The name of the azure blob storage account for the UI related azure storage blob containers. (Use `spreadsheets1`)

- **`CosmosDbConfiguration:ConnectionString`**  
  The connection string value used for accessing the VYF cosmos db service. (Use `pds-dev-shared-cdb`)

- **`CosmosDbConfiguration:LayoutCollection`**  
  The name of the cosmos db collection used for layout data. (Use `layout`)
  
- **`CosmosDbConfiguration:Database`**  
  The name of the cosmos db database used for VYF. (Use `funding`)

- **`CosmosDbConfiguration:CosmosConnectionMode`**  
  The connection mode used for accessing the VYF cosmos db service. (Use `Gateway`)

- **`baseSiteUrl`**  
  The url for the VYF root. (Use local host url)

- **`FundingApiSecretKey`**  
  The secret key used for authorising requests to the external funding api.

- **`CanWriteExpectedHtml`**  
  Sets whether expected html files used for automation test purposes will be re-written.

  
