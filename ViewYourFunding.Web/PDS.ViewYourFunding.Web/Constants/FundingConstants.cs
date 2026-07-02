namespace PDS.ViewYourFunding.Web.Constants
{
    /// <summary>
    /// Constants that are used for funding documents etc..
    /// </summary>
    public static class FundingConstants
    {
        /// <summary>
        /// The MVC route prefix for the controller.
        /// </summary>
        public const string RoutePrefix = "api/funding";

        /// <summary>
        /// The route for the generic 'render html' method.
        /// </summary>
        public const string Route_RenderHtml = "render";

        /// <summary>
        /// The route name for the generic 'render html' method.
        /// </summary>
        public const string RouteName_RenderHtml = "RenderHtml";

        /// <summary>
        /// The route for the generic 'get file' method.
        /// </summary>
        public const string Route_GetFile = "getFile";

        /// <summary>
        /// The route name for the generic 'get file' method.
        /// </summary>
        public const string RouteName_GetFile = "GetFile";

        /// <summary>
        /// Route for generating a funding document (e.g. a spreadsheet).
        /// </summary>
        public const string Route_GenerateFundingDocument = "api/funding/GenerateFundingDocument";

        /// <summary>
        /// Route name for generating a funding document (e.g. a spreadsheet).
        /// </summary>
        public const string RouteName_GenerateFundingDocument = "VYF_GenerateFundingDocument";

        /// <summary>
        /// Route for getting the maximum UI and spreadsheet schema template version numbers.
        /// </summary>
        public const string Route_GetMaxUIAndSpreadsheetVersionNumbers = "api/funding/GetMaxUIAndSpreadsheetVersionNumbers";

        /// <summary>
        /// Route for generating a funding document to a byte array (not storing it).
        /// </summary>
        public const string Route_GenerateFundingDocumentToByteArray = "GenerateFundingDocumentToByteArray";

        /// <summary>
        /// Route for getting the path to a funding document (e.g. a spreadsheet).
        /// </summary>
        public const string Route_GetFundingDocumentMetadata = "GetFundingDocumentMetadata";

        /// <summary>
        /// Route name for getting the path to a funding document (e.g. a spreadsheet).
        /// </summary>
        public const string RouteName_GetFundingDocumentMetadata = "VYF_GetFundingDocumentMetadata";

        /// <summary>
        /// Route for downloading a spreadsheet.
        /// </summary>
        public const string Route_DownloadSpreadsheet = "DownloadSpreadsheet/{fileName}/{publishedDate}";

        /// <summary>
        /// Route name for downloading a spreadsheet.
        /// </summary>
        public const string RouteName_DownloadSpreadsheet = "VYF_DownloadSpreadsheet";

        /// <summary>
        /// Route for getting the latest publication date.
        /// </summary>
        public const string Route_GetLatestFundingStreamPublishedDate = "LatestFundingStreamPublishedDate/{fundingStreamCode}/{fundingPeriodCode}";

        /// <summary>
        /// Route name for getting the latest publication date.
        /// </summary>
        public const string RouteName_GetLatestFundingStreamPublishedDate = "VYF_GetLatestFundingStreamPublishedDate";

        /// <summary>
        /// Route for getting all funding streams configured for auto pull.
        /// </summary>
        public const string Route_GetAutoPullConfiguredFundingStreams = "getAutoPullConfiguredFundingStreams";

        /// <summary>
        /// Route name for getting all funding streams configured for auto pull.
        /// </summary>
        public const string RouteName_GetAutoPullConfiguredFundingStreams = "GetAutoPullConfiguredFundingStreams";

        /// <summary>
        /// The route get email enabled funding stream and periods.
        /// </summary>
        public const string Route_GetEmailEnabledFundingStreamAndPeriods = "getEmailEnabledFundingStreamAndPeriods";

        /// <summary>
        /// The route name get email enabled funding stream and periods.
        /// </summary>
        public const string RouteName_GetEmailEnabledFundingStreamAndPeriods = "GetEmailEnabledFundingStreamAndPeriods";
    }
}