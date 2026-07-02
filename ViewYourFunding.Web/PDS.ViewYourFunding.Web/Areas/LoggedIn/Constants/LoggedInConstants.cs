namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants
{
    /// <summary>
    /// The logged in constants class.
    /// </summary>
    public static class LoggedInConstants
    {
        /// <summary>
        /// The route for the provider statement.
        /// </summary>
        public const string Route_ProviderStatement = "pre-16-16-19-statements";

        /// <summary>
        /// The route name for the provider statement.
        /// </summary>
        public const string RouteName_ProviderStatement = "LoggedIn_ProviderStatement";

        /// <summary>
        /// The route for the local authority funding breakdown statement.
        /// </summary>
        public const string Route_LocalAuthorityFundingBreakdown = "pre-16-16-19-statements/{ukprn}/{fundingStreamNamePathPart}/{publishedDate}/{yearFrom}-to-{yearTo}/la";

        /// <summary>
        /// The route name for the provider statement.
        /// </summary>
        public const string RouteName_LocalAuthorityFundingBreakdown = "LoggedIn_LocalAuthorityFundingBreakdown";

        /// <summary>
        /// The MVC route for the logged in local authority allocation history page.
        /// </summary>
        public const string Route_LocalAuthorityHistory = "pre-16-16-19-statements/{ukprn}/{fundingStreamNamePathPart}/allocation-history/la";

        /// <summary>
        /// The MVC route for the logged in la recoupment history page.
        /// </summary>
        public const string Route_LARecoupmentHistory = "recoupment-reports/recoupment-history";

        /// <summary>
        /// The MVC route name for the logged in local authority allocation history page.
        /// </summary>
        public const string RouteName_LocalAuthorityHistory = "LoggedIn_LocalAuthority-allocation-history";

        /// <summary>
        /// The MVC route name for the logged in la recoupment history page.
        /// </summary>
        public const string RouteName_LARecoupmentHistory = "LoggedIn_LARecoupment-allocation-history";

        /// <summary>
        /// The route for the multiple academy trust statement.
        /// </summary>
        public const string Route_MultipleAcademyTrustStatement = "pre-16-16-19-statements/parent";

        /// <summary>
        /// The route name for the multiple academy trust statement.
        /// </summary>
        public const string RouteName_MultipleAcademyTrustStatement = "LoggedIn_MultipleAcademyTrustStatement";

        /// <summary>
        /// The browser title for the allocation statements.
        /// </summary>
        public const string BrowserTitle_AllocationStatements = "Allocation statements - manage your education and skills...";

        /// <summary>
        /// The browser title for the allocation history.
        /// </summary>
        public const string BrowserTitle_AllocationHistory = "Allocation history - manage your education and skills...";

        /// <summary>
        /// The page title for the allocation statements.
        /// </summary>
        public const string PageTitle_AllocationStatements = "Allocation statements";

        /// <summary>
        /// The standard MYESF header (a variation on the usual one).
        /// </summary>
        public const string HeaderTitle_StandardMYESFHeader = "Manage your education and skills funding";

        /// <summary>
        /// The MVC route for the provider home page.
        /// </summary>
        public const string Route_ProviderHome = "/";

        /// <summary>
        /// The Provider funding breakdown route.
        /// </summary>
        public const string Route_ProviderFundingBreakdown =
            "pre-16-16-19-statements/{ukprn}/{fundingStreamNamePathPart}/{publishedDate}/{yearFrom}-to-{yearTo}";

        /// <summary>
        /// The Provider funding breakdown route name.
        /// </summary>
        public const string RouteName_ProviderFundingBreakdown = "LoggedIn_ProviderFundingBreakdown";

        /// <summary>
        /// The MVC route for the logged in provider allocation history page.
        /// </summary>
        public const string Route_ProviderHistory = "pre-16-16-19-statements/{ukprn}/{fundingStreamNamePathPart}/allocation-history";

        /// <summary>
        /// The MVC route name for the logged in provider allocation history page.
        /// </summary>
        public const string RouteName_ProviderHistory = "LoggedIn_provider-allocation-history";

        /// <summary>
        /// The MVC route for the logged in provider spreadsheet download.
        /// </summary>
        public const string Route_ProviderSpreadsheetDownload = "pre-16-16-19-statements/{Id}/{FundingStreamCode}/download-funding/provider/{Ukprn}/{YearTypeCode}/{YearFrom}-to-{YearTo}/{PublishedDate}/{Format}";

        /// <summary>
        /// The MVC route name for the logged in provider spreadsheet download.
        /// </summary>
        public const string RouteName_ProviderSpreadsheetDownload = "LoggedIn_provider-spreadsheet-download";

        /// <summary>
        /// The MVC route for the logged in organisation spreadsheet download.
        /// </summary>
        public const string Route_OrganisationSpreadsheetDownload = "pre-16-16-19-statements/{Id}/{FundingStreamCode}/download-funding/local-authority/{Ukprn}/{YearTypeCode}/{YearFrom}-to-{YearTo}/{PublishedDate}/{Format}";

        /// <summary>
        /// The MVC route name for the logged in organisation spreadsheet download.
        /// </summary>
        public const string RouteName_OrganisationSpreadsheetDownload = "LoggedIn_organisation-spreadsheet-download";

        /// <summary>
        /// The MVC route for the local authority recoupment summary.
        /// </summary>
        public const string Route_LocalAuthorityRecoupmentSummary = "recoupment-reports";

        /// <summary>
        /// The MVC route name for the local authority recoupment summary.
        /// </summary>
        public const string RouteName_LocalAuthorityRecoupmentSummary = "LoggedIn_LocalAuthority-recoupment-summary";

        /// <summary>
        /// The MVC route for the local authority recoupment detail.
        /// </summary>
        public const string Route_LocalAuthorityRecoupmentDetail = "recoupment-reports/{Ukprn}/{FundingStreamCode}/{PublishedDate}/{YearFrom}-to-{YearTo}/{Format}";

        /// <summary>
        /// The MVC route name for the local authority recoupment detail.
        /// </summary>
        public const string RouteName_LocalAuthorityRecoupmentDetail = "LoggedIn_LocalAuthority-recoupment-detail";

        /// <summary>
        /// The browser title for the recoupment reports.
        /// </summary>
        public const string BrowserTitle_RecoupmentReports = "Recoupment Reports";

        /// <summary>
        /// The page title for the recoupment reports.
        /// </summary>
        public const string PageTitle_RecoupmentReports = "Recoupment Reports";

        /// <summary>
        /// The page title for the logged in provider allocation history page.
        /// </summary>
        public const string PageTitle_ProviderHistory = "Allocation history";

        /// <summary>
        /// The page title for the recoupment history.
        /// </summary>
        public const string PageTitle_RecoupmentHistory = "Recoupment history";

        /// <summary>
        /// The MVC route for the variance selection page.
        /// </summary>
        public const string Route_VarianceSelection = "pre-16-16-19-statements/variance-selection/{ukprn}/{fundingStreamNamePathPart}/{publishedDate}/{yearFrom}-to-{yearTo}";

        /// <summary>
        /// The MVC route name for the variance selection page.
        /// </summary>
        public const string RouteName_VarianceSelection = "Variance_Selection";

        /// <summary>
        /// The page title for the variance selection page.
        /// </summary>
        public const string PageTitle_VarianceSelection = "Select a previous statement to compare your figures";

        /// <summary>
        /// The browser title for the variance selection.
        /// </summary>
        public const string BrowserTitle_VarianceSelection = "Choose how to view funding";

        /// <summary>
        /// The browser title for the unauthorized access.
        /// </summary>
        public const string BrowserTitle_UnauthorizedAccess = "You do not have permission";
    }
}