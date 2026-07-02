using PDS.ViewYourFunding.Services.Helper;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// A class to store constants for the View Your Funding area.
    /// </summary>
    public static class ViewYourFundingConstants
    {
        /// <summary>
        /// The MVC controller name.
        /// </summary>
        public const string ControllerName = "ViewYourFunding";

        /// <summary>
        /// The admin page signout link.
        /// </summary>
        public const string AdminSignOutLink = "logout";

        #region Start page

        /// <summary>
        /// The MVC route for the start page.
        /// </summary>
        public const string Route_Start = "start";

        /// <summary>
        /// The MVC route for the start page.
        /// </summary>
        public const string Route_Start_New = "";

        /// <summary>
        /// The MVC route name for the start page.
        /// </summary>
        public const string RouteName_Start = "VYF_Start";

        /// <summary>
        /// The MVC route name for the start page.
        /// </summary>
        public const string RouteName_Start_New = "VYF_Start_New";

        /// <summary>
        /// The page title for the start page.
        /// </summary>
        public const string PageTitle_Start = "View latest funding";

        #endregion


        #region DFE sign in pages

        /// <summary>
        /// The MVC route for the login page.
        /// </summary>
        public const string Route_Login = "/login";

        /// <summary>
        /// The MVC route name for the login page.
        /// </summary>
        public const string RouteName_Login = "VYF_Login";

        /// <summary>
        /// The MVC route for the logout page.
        /// </summary>
        public const string Route_Logout = "/logout";

        /// <summary>
        /// The MVC route name for the logout page.
        /// </summary>
        public const string RouteName_Logout = "VYF_Logout";

        /// <summary>
        /// The MVC route for the logout page.
        /// </summary>
        public const string Route_PostLogout = "/account/postlogout";

        /// <summary>
        /// The MVC route name for the logout page.
        /// </summary>
        public const string RouteName_PostLogout = "VYF_PostLogout";

        /// <summary>
        /// The MVC route for the logout page.
        /// </summary>
        public const string Route_PostLogoutRedirect = "/account/postlogout/redirect";

        /// <summary>
        /// The MVC route name for the logout page.
        /// </summary>
        public const string RouteName_PostLogoutRedirect = "VYF_PostLogoutRedirect";

        #endregion


        #region Error pages

        /// <summary>
        /// The MVC route for the error page.
        /// </summary>
        public const string Route_ErrorPage = "/error";

        /// <summary>
        /// The MVC route name for the error page.
        /// </summary>
        public const string RouteName_ErrorPage = "VYF_ErrorPage";

        /// <summary>
        /// The MVC route for the status code 403 DSI custom page.
        /// </summary>
        public const string Route_StatusCode403DSIRequiredUserRoles = "/error/403/{code}";

        /// <summary>
        /// The MVC route name for the access denied page.
        /// </summary>
        public const string RouteName_StatusCode403DSIRequiredUserRoles = "VYF_StatusCode403DSIRequiredUserRoles";

        /// <summary>
        /// The MVC route for the access denied page.
        /// </summary>
        public const string Route_AccessDenied = "/accessdenied";

        /// <summary>
        /// The MVC route name for the access denied page.
        /// </summary>
        public const string RouteName_AccessDenied = "VYF_AccessDenied";

        #endregion


        #region Layout Management pages

        /// <summary>
        /// The MVC route for the admin layout home page.
        /// </summary>
        public const string Route_AdminLayoutManagementHome = "admin/layout/home";

        /// <summary>
        /// The MVC route name for the layout home page.
        /// </summary>
        public const string RouteName_AdminLayoutManagementHome = "Admin_AdminLayoutManagementHome";

        /// <summary>
        /// The MVC route for the admin layout get layouts.
        /// </summary>
        public const string Route_AdminLayoutManagementGetLayouts = "admin/layout/getlayouts/{pageNumber}/{fundingStreamIds}/{fundingViewTypeIds}/{fundingViewScopeIds}";

        /// <summary>
        /// The MVC route name for the layout get layouts.
        /// </summary>
        public const string RouteName_AdminLayoutManagementGetLayouts = "Admin_AdminLayoutManagementGetLayouts";

        /// <summary>
        /// The MVC route for the admin layout import page.
        /// </summary>
        public const string Route_AdminLayoutImport = "admin/layout/import";

        /// <summary>
        /// The MVC route name for the admin layout import page.
        /// </summary>
        public const string RouteName_AdminLayoutImport = "Admin_AdminLayoutImport";

        /// <summary>
        /// The MVC route for the admin layout import file page.
        /// </summary>
        public const string Route_AdminLayoutFileImport = "admin/layout/importfile";

        /// <summary>
        /// The MVC route name for the admin layout import file page.
        /// </summary>
        public const string RouteName_AdminLayoutFileImport = "Admin_AdminLayoutImportFile";

        /// <summary>
        /// The MVC route for the admin layout file import upload page.
        /// </summary>
        public const string Route_AdminLayoutFileImportUpload = "admin/layout/importfileupload";

        /// <summary>
        /// The MVC route name for the admin layout file import upload page.
        /// </summary>
        public const string RouteName_AdminLayoutFileImportUpload = "Admin_AdminLayoutImportFileUpload";

        /// <summary>
        /// The MVC route name for layout import Confirmation.
        /// </summary>
        public const string Route_AdminLayoutImportConfirmation = "admin/layout/confirmation/{layoutId}/{layoutAction}";

        /// <summary>
        /// The MVC route name for layout import Confirmation.
        /// </summary>
        public const string RouteName_AdminLayoutImportConfirmation = "Admin_AdminLayoutImportConfirmation";

        /// <summary>
        /// The MVC route for the download layout file action.
        /// </summary>
        public const string Route_AdminLayoutDownloadFile = "admin/layout/download/{layoutId}";

        /// <summary>
        /// The MVC route name for the download layout file action.
        /// </summary>
        public const string RouteName_AdminLayoutDownloadFile = "Admin_AdminLayoutDownLoadFile";

        /// <summary>
        /// The MVC route to are you sure view.
        /// </summary>
        public const string Route_AdminLayoutAreYouSure = "admin/layout/areyousure/{layoutId}";

        /// <summary>
        /// The MVC route name are you sure view.
        /// </summary>
        public const string RouteName_AdminLayoutAreYouSure = "Admin_AdminLayoutAreYouSure";

        /// <summary>
        /// The MVC route to delete a layout.
        /// </summary>
        public const string Route_AdminDeleteLayout = "admin/layout/deletelayout/{layoutId}";

        /// <summary>
        /// The MVC route name to delete a layout.
        /// </summary>
        public const string RouteName_AdminDeleteLayout = "Admin_AdminDeleteLayout";

        /// <summary>
        /// The MVC route to preview a layout.
        /// </summary>
        public const string Route_AdminPreviewLayout = "admin/layout/preview/{layoutId}/{fundingViewScope}/{fundingStreamId}/{fundingViewtype}";

        /// <summary>
        /// The MVC route name to preview a layout.
        /// </summary>
        public const string RouteName_AdminPreviewLayout = "Admin_PreviewLayout";

        #endregion


        #region Admin Funding stream settings pages

        /// <summary>
        /// The MVC route for the admin home page.
        /// </summary>
        public const string Route_MYESFHome = "/";

        /// <summary>
        /// The MVC route name for the admin home page.
        /// </summary>
        public const string RouteName_MYESFHome = "MYESF_Home";

        /// <summary>
        /// The MVC route for the admin home page.
        /// </summary>
        public const string Route_AdminHome = "admin/home";

        /// <summary>
        /// The MVC route name for the admin home page.
        /// </summary>
        public const string RouteName_AdminHome = "Admin_Home";

        /// <summary>
        /// The MVC route for the admin home page.
        /// </summary>
        public const string Route_AdminSettingsHome = "admin/settings";

        /// <summary>
        /// The MVC route name for the admin home page.
        /// </summary>
        public const string RouteName_AdminSettingsHome = "Admin_SettingsHome";

        /// <summary>
        /// The MVC route for the admin funding stream page.
        /// </summary>
        public const string Route_AdminSettingsFundingStream = "admin/settings/fundingstream/{fundingStreamId}";

        /// <summary>
        /// The MVC route name for the admin funding stream page.
        /// </summary>
        public const string RouteName_AdminSettingsFundingStream = "Admin_SettingsFundingStream";

        /// <summary>
        /// The MVC route for the admin funding stream setting edit.
        /// </summary>
        public const string Route_AdminFundingStreamSettingAction = "admin/fundingstream-setting/action/{fundingStreamId}/{settingValueId}/{actionMode}";

        /// <summary>
        /// The MVC route name for the admin funding stream setting edit.
        /// </summary>
        public const string RouteName_AdminFundingStreamSettingAction = "Admin_FundingStreamSettingAction";

        /// <summary>
        /// The MVC route name for the admin funding stream setting add.
        /// </summary>
        public const string RouteName_AdminFundingStreamSettingAdd = "Admin_FundingStreamSettingAdd";

        /// <summary>
        /// The MVC route for the admin funding stream setting add.
        /// </summary>
        public const string Route_AdminFundingStreamSettingAdd = "admin/fundingstream-setting/add";

        /// <summary>
        /// The MVC route name for the admin funding stream setting set type.
        /// </summary>
        public const string RouteName_AdminFundingStreamSettingSetType = "Admin_FundingStreamSettingSetType";

        /// <summary>
        /// The MVC route for the admin funding stream setting set type.
        /// </summary>
        public const string Route_AdminFundingStreamSettingSetType = "admin/fundingstream-setting/set-type/{fundingStreamId}";

        /// <summary>
        /// The MVC route for the admin funding stream setting are you sure.
        /// </summary>
        public const string Route_AdminFundingStreamSettingAreYouSure = "admin/fundingstream-setting/are-you-sure";

        /// <summary>
        /// The MVC route name for the admin funding stream setting are you sure page.
        /// </summary>
        public const string RouteName_AdminFundingStreamSettingAreYouSure = "Admin_FundingStreamSettingAreYouSure";

        /// <summary>
        /// The MVC route for the admin funding stream setting save changes.
        /// </summary>
        public const string Route_AdminFundingStreamSettingSaveChanges = "admin/fundingstream-setting/saveChanges";

        /// <summary>
        /// The MVC route name for the admin setting save changes.
        /// </summary>
        public const string RouteName_AdminSettingSaveChanges = "Admin_SettingSaveChanges";

        /// <summary>
        /// The MVC route for the admin setting confirmation.
        /// </summary>
        public const string Route_AdminFundingStreamSettingConfirmation = "admin/fundingstream-setting/confirmation/{fundingStreamId}/{success}/{actionMode}";

        /// <summary>
        /// The MVC route name for the admin  setting Confirmation.
        /// </summary>
        public const string RouteName_AdminFundingStreamSettingConfirmation = "Admin_SettingConfirmation";

        /// <summary>
        /// The MVC route for the admin home page.
        /// </summary>
        public const string Route_AdminManageAllocationHome = "/";

        /// <summary>
        /// The MVC route name for the admin home page.
        /// </summary>
        public const string RouteName_AdminManageAllocationHome = "Admin_ManageAllocationHome";

        #endregion


        #region Publication pages

        /// <summary>
        /// The MVC route for the admin publication are you sure page.
        /// </summary>
        public const string Route_AdminPublicationAreYouSure = "admin/publication/are-you-sure/{fundingStreamId}/{publicationId}/{action}";

        /// <summary>
        /// The MVC route name for the admin publication are you sure page.
        /// </summary>
        public const string RouteName_AdminPublicationAreYouSure = "Admin_PublicationAreYouSure";

        /// <summary>
        /// The MVC route for the admin publication confirmation page.
        /// </summary>
        public const string Route_AdminPublicationConfirmation = "admin/publication/confirmation/{fundingStreamId}/{publicationId}/{actionMode}/{changesSaved}";

        /// <summary>
        /// The MVC route name for the admin publication confirmation page.
        /// </summary>
        public const string RouteName_AdminPublicationConfirmation = "Admin_PublicationConfirmation";

        /// <summary>
        /// The MVC route for the admin publication save changes page.
        /// </summary>
        public const string Route_AdminPublicationSaveChanges = "admin/publication/save-changes";

        /// <summary>
        /// The MVC route name for the publication save changes page.
        /// </summary>
        public const string RouteName_AdminPublicationSaveChanges = "Admin_PublicationSaveChanges";

        /// <summary>
        /// The MVC route for the admin publication action page.
        /// </summary>
        public const string Route_AdminPublicationAction = "admin/publication/action/{fundingStreamId}/{publicationId}/{actionMode}";

        /// <summary>
        /// The MVC route name for the admin publication action page.
        /// </summary>
        public const string RouteName_AdminPublicationAction = "Admin_PublicationAction";

        /// <summary>
        /// The MVC route for the admin publication generate spreadsheet page.
        /// </summary>
        public const string Route_AdminPublicationGenerateSpreadsheet = "admin/publication/generatespreadsheet/{fundingStreamId}/{publicationId}";

        /// <summary>
        /// The MVC route name for the admin publication generate spreadsheet page.
        /// </summary>
        public const string RouteName_AdminPublicationGenerateSpreadsheet = "Admin_PublicationGenerateSpreadsheet";

        /// <summary>
        /// The MVC route for the admin publication get spreadsheet created page.
        /// </summary>
        public const string Route_AdminPublicationGetSpreadsheetCreateDate = "admin/publication/getspreadsheetcreateddate/{fundingStreamCode}/{fundingPeriodCode}/{publishedDate}/{startDatetime}";

        /// <summary>
        /// The MVC route name for the admin publication get spreadsheet created date page.
        /// </summary>
        public const string RouteName_AdminPublicationGetSpreadsheetCreateDate = "Admin_PublicationGetSpreadsheetCreateDate";

        #endregion


        #region Next Payment Type Pages

        /// <summary>
        /// The MVC route for the admin NextPaymentType index page.
        /// </summary>
        public const string Route_AdminNextPaymentTypeIndex = "admin/nextpaymenttype/index/{fundingStreamId}";

        /// <summary>
        /// The MVC route namee for  NextPaymentType index page.
        /// </summary>
        public const string RouteName_AdminNextPaymentTypeIndex = "Admin_NextPaymentTypeIndex";

        /// <summary>
        /// The MVC route for the admin NextPaymentType index page.
        /// </summary>
        public const string Route_AdminNextPaymentTypeAction = "admin/nextpaymenttype/action/{nextPaymentTypeId}/{fundingStreamId}/{actionMode}";

        /// <summary>
        /// The MVC route namee for  NextPaymentType Action page.
        /// </summary>
        public const string RouteName_AdminNextPaymentTypeAction = "Admin_NextPaymentTypeAction";

        /// <summary>
        /// The MVC route namee for  NextPaymentType Are u sure  page.
        /// </summary>
        public const string Route_AdminNextPaymentTypeAreYouSure = "admin/nextpaymenttype/are-you-sure/{nextPaymentTypeId}/{fundingStreamId}/{actionMode}";

        /// <summary>
        /// The MVC route namee for  NextPaymentType Are u sure  page.
        /// </summary>
        public const string RouteName_AdminNextPaymentTypeAreYouSure = "Admin_NextPaymentTypeAreYouSure";


        /// <summary>
        /// The MVC route namee for  NextPaymentType Save Changes  page.
        /// </summary>
        public const string Route_AdminNextPaymentTypeSaveChanges = "admin/nextpaymenttype/savechanges";

        /// <summary>
        /// The MVC route namee for  NextPaymentType Save Changes  page.
        /// </summary>
        public const string RouteName_AdminNextPaymentTypeSaveChanges = "Admin_NextPaymentTypeSaveChanges";

        /// <summary>
        /// The MVC route namee for  NextPaymentType Confirmation.
        /// </summary>
        public const string Route_AdminNextPaymentTypeConfirmation = "admin/nextpaymenttype/confirmation/{nextPaymentTypeId}/{fundingStreamId}/{actionMode}";

        /// <summary>
        /// The MVC route namee for  NextPaymentType Confirmation.
        /// </summary>
        public const string RouteName_AdminNextPaymentTypeConfirmation = "Admin_NextPaymentTypeConfirmation";

        #endregion


        #region Next Payment Pages

        /// <summary>
        /// The MVC route for the admin NextPayment index page.
        /// </summary>
        public const string Route_AdminNextPaymentIndex = "admin/nextpayment/index/{fundingStreamId}";

        /// <summary>
        /// The MVC route namee for  NextPayment index page.
        /// </summary>
        public const string RouteName_AdminNextPaymentIndex = "Admin_NextPaymentIndex";

        /// <summary>
        /// The MVC route for the admin NextPayment index page.
        /// </summary>
        public const string Route_AdminNextPaymentAction = "admin/nextpayment/action/{nextPaymentId}/{fundingStreamId}/{actionMode}";

        /// <summary>
        /// The MVC route namee for  NextPayment Action page.
        /// </summary>
        public const string RouteName_AdminNextPaymentAction = "Admin_NextPaymentAction";

        /// <summary>
        /// The MVC route namee for  NextPayment Are u sure  page.
        /// </summary>
        public const string Route_AdminNextPaymentAreYouSure = "admin/nextpayment/are-you-sure/{nextPaymentId}/{fundingStreamId}/{actionMode}";

        /// <summary>
        /// The MVC route namee for  NextPayment Are u sure  page.
        /// </summary>
        public const string RouteName_AdminNextPaymentAreYouSure = "Admin_NextPaymentAreYouSure";

        /// <summary>
        /// The MVC route name for  NextPayment Save Changes  page.
        /// </summary>
        public const string Route_AdminNextPaymentSaveChanges = "admin/nextpayment/savechanges";

        /// <summary>
        /// The MVC route namee for  NextPayment  Save Changes  page.
        /// </summary>
        public const string RouteName_AdminNextPaymentSaveChanges = "Admin_NextPaymentSaveChanges";

        /// <summary>
        /// The MVC route namee for  NextPayment  Confirmation.
        /// </summary>
        public const string Route_AdminNextPaymentConfirmation = "admin/nextpayment/confirmation/{nextPaymentId}/{fundingStreamId}/{actionMode}";

        /// <summary>
        /// The MVC route namee for  NextPayment Confirmation.
        /// </summary>
        public const string RouteName_AdminNextPaymentConfirmation = "Admin_NextPaymentConfirmation";


        #endregion


        #region Funding Stream Pages

        /// <summary>
        /// The MVC route name for admin funding stream action page.
        /// </summary>
        public const string RouteName_AdminFundingStreamAction = "Admin_FundingStreamAction";

        /// <summary>
        /// The MVC route for admin funding stream action page.
        /// </summary>
        public const string Route_AdminFundingStreamAction = "admin/fundingstream/action/{fundingStreamId}/{actionMode}";

        /// <summary>
        /// The MVC route for admin funding stream are you sure page.
        /// </summary>
        public const string Route_AdminFundingStreamAreYouSure = "admin/fundingstream/are-you-sure/{fundingStreamId}/{actionMode}";

        /// <summary>
        /// The MVC route name for admin funding stream are you sure page.
        /// </summary>
        public const string RouteName_AdminFundingStreamAreYouSure = "Admin_FundingStreamAreYouSure";

        /// <summary>
        /// The MVC route for admin funding stream save changes page.
        /// </summary>
        public const string Route_AdminFundingStreamSaveChanges = "admin/fundingstream/savechanges";

        /// <summary>
        /// The MVC route name for admin funding stream save changes page.
        /// </summary>
        public const string RouteName_AdminFundingStreamSaveChanges = "Admin_FundingStreamSaveChanges";

        /// <summary>
        /// The MVC route for admin funding stream confirmation page.
        /// </summary>
        public const string Route_AdminFundingStreamConfirmation = "admin/fundingstream/confirmation/{fundingStreamId}/{actionMode}";

        /// <summary>
        /// The MVC route name for admin funding stream confirmation page.
        /// </summary>
        public const string RouteName_AdminFundingStreamConfirmation = "Admin_FundingStreamConfirmation";

        #endregion


        #region Clear Cache

        /// <summary>
        /// The MVC route for the clear cache admin page.
        /// </summary>
        public const string Route_ClearCache = "api/funding/vyf-clear-cache";

        /// <summary>
        /// The MVC route name for the clear cache admin page.
        /// </summary>
        public const string RouteName_ClearCache = "Admin_ClearCache";

        #endregion


        #region General Settings pages

        /// <summary>
        /// The MVC route for the start page.
        /// </summary>
        public const string Route_AdminGeneralSettingHome = "admin/generalsettings/home";

        /// <summary>
        /// The MVC route name for the start page.
        /// </summary>
        public const string RouteName_AdminGeneralSettingHome = "Admin_GeneralSettingHome";

        /// <summary>
        /// The MVC route for the start page.
        /// </summary>
        public const string Route_AdminGeneralSettingEdit = "admin/generalsettings/edit/{globalSettingId}";

        /// <summary>
        /// The MVC route name for the start page.
        /// </summary>
        public const string RouteName_AdminGeneralSettingEdit = "Admin_GlobalSettingEdit";

        /// <summary>
        /// The MVC route for the start page.
        /// </summary>
        public const string Route_AdminGeneralSettingSaveChanges = "admin/generalsettings/savechanges";

        /// <summary>
        /// The MVC route name for the start page.
        /// </summary>
        public const string RouteName_AdminGeneralSettingSaveChanges = "Admin_GeneralSettingSaveChanges";

        /// <summary>
        /// The MVC route for the start page.
        /// </summary>
        public const string Route_AdminGeneralSettingConfirmation = "admin/generalsettings/confirmation/{globalSettingId}/{changesSaved}";

        /// <summary>
        /// The MVC route name for the start page.
        /// </summary>
        public const string RouteName_AdminGeneralSettingConfirmation = "Admin_GeneralSettingConfirmation";

        #endregion


        #region PDF Generation Actions pages

        /// <summary>
        /// The MVC route for the start page.
        /// </summary>
        public const string Route_PdfGenerationActionsHome = "admin/pdfGenerationActions/home";

        /// <summary>
        /// The MVC route name for the start page.
        /// </summary>
        public const string RouteName_PdfGenerationActionsHome = "Admin_PdfGenerationActionsHome";

        /// <summary>
        /// The MVC route for the run feed reader page.
        /// </summary>
        public const string Route_PdfGenerationActionsRunFeedReader = "admin/pdfGenerationActions/runfeedreader";

        /// <summary>
        /// The MVC route name for the run feed reader page.
        /// </summary>
        public const string RouteName_PdfGenerationActionsRunFeedReader = "Admin_PdfGenerationActionsRunFeedReader";

        /// <summary>
        /// The MVC route for the confirm run feed reader page.
        /// </summary>
        public const string Route_PdfGenerationActionsConfirmRunFeedReader = "admin/pdfGenerationActions/confirmRunfeedreader";

        /// <summary>
        /// The MVC route name for the confirm run feed reader page.
        /// </summary>
        public const string RouteName_PdfGenerationActionsConfirmRunFeedReader = "Admin_PdfGenerationActionsConfirmRunFeedReader";

        /// <summary>
        /// The MVC route for the generate funding reports page.
        /// </summary>
        public const string Route_PdfGenerationActionsGenerateFundingReports = "admin/pdfGenerationActions/generateFundingReports";

        /// <summary>
        /// The MVC route name for the generate funding reports page.
        /// </summary>
        public const string RouteName_PdfGenerationActionsGenerateFundingReports = "Admin_PdfGenerationActionsGenerateFundingReports";

        /// <summary>
        /// The MVC route for the run pdf comparison page.
        /// </summary>
        public const string Route_PdfGenerationActionsRunPdfComparison = "admin/pdfGenerationActions/runPdfComparison";

        /// <summary>
        /// The MVC route name for the run pdf comparison page.
        /// </summary>
        public const string RouteName_PdfGenerationActionsRunPdfComparison = "Admin_PdfGenerationActionsRunPdfComparison";

        /// <summary>
        /// The MVC route for the confirm run pdf comparison page.
        /// </summary>
        public const string Route_PdfGenerationActionsConfirmRunPdfComparison = "admin/pdfGenerationActions/confirmRunPdfComparison";

        /// <summary>
        /// The MVC route name for the generate funding report page.
        /// </summary>
        public const string RouteName_PdfGenerationActionsGenerateFundingReport = "Admin_PdfGenerationActionsConfirmGenerateFundingReport";

        /// <summary>
        /// The MVC route for the generate funding report page.
        /// </summary>
        public const string Route_PdfGenerationActionsGenerateFundingReport = "admin/pdfGenerationActions/confirmGenerateFundingReport";

        /// <summary>
        /// The MVC route name for the confirm run pdf comparison page.
        /// </summary>
        public const string RouteName_PdfGenerationActionsConfirmRunPdfComparison = "Admin_PdfGenerationActionsConfirmRunPdfComparison";

        /// <summary>
        /// The MVC route for the start page.
        /// </summary>
        public const string Route_PdfGenerationActionsConfirmation = "admin/pdfGenerationActions/confirmation/{success}/{pdfGenerationAction}";

        /// <summary>
        /// The MVC route name for the 'run feed reader confirmation' page.
        /// </summary>
        public const string RouteName_PdfGenerationActionsConfirmation = "Admin_PdfGenerationActionsConfirmation";

        /// <summary>
        /// The MVC route for the rerun pdf generation page.
        /// </summary>
        public const string Route_PdfGenerationActionsRerunPdfGeneration = "admin/pdfGenerationActions/rerunPdfGeneration";

        /// <summary>
        /// The MVC route name for rerun pdf generation page.
        /// </summary>
        public const string RouteName_PdfGenerationActionsRerunPdfGeneration = "Admin_PdfGenerationActionsRerunPdfGeneration";

        /// <summary>
        /// The MVC route for the confirm rerun pdf generation page.
        /// </summary>
        public const string Route_PdfGenerationActionsConfirmRerunPdfGeneration = "admin/pdfGenerationActions/confirmRerunPdfGeneration";

        /// <summary>
        /// The MVC route name for the confirm rerun pdf generation page.
        /// </summary>
        public const string RouteName_PdfGenerationActionsConfirmRerunPdfGeneration = "Admin_PdfGenerationActionsConfirmRerunPdfGeneration";

        /// <summary>
        /// The MVC route for generate single pdf page.
        /// </summary>
        public const string Route_PdfGenerationActionsGenerateSinglePdf = "admin/pdfGenerationActions/generatesinglepdf";

        /// <summary>
        /// The MVC route name for the generate single pdf page.
        /// </summary>
        public const string RouteName_PdfGenerationActionsGenerateSinglePdf = "Admin_PdfGenerationActionsGenerateSinglePdf";

        /// <summary>
        /// The MVC route for confirm generate single pdf page.
        /// </summary>
        public const string Route_PdfGenerationActionsConfirmGenerateSinglePdf = "admin/pdfGenerationActions/confirmgeneratesinglepdf";

        /// <summary>
        /// The MVC route name for confirm generate single pdf page.
        /// </summary>
        public const string RouteName_PdfGenerationActionsConfirmGenerateSinglePdf = "Admin_PdfGenerationActionsConfirmGenerateSinglePdf";

        /// <summary>
        /// The MVC route for the 'feed reader last run' page.
        /// </summary>
        public const string Route_PdfGenerationActionsFeedReaderLastRun = "admin/pdfGenerationActions/feedreaderlastrun";

        /// <summary>
        /// The MVC route name for the 'feed reader last run' page.
        /// </summary>
        public const string RouteName_PdfGenerationActionsFeedReaderLastRun = "Admin_FeedReaderLastRun";

        #endregion


        #region Business Allocations Data Admin pages

        /// <summary>
        /// The MVC route for the business allocations admin home page.
        /// </summary>
        public const string Route_BusinessAllocationsAdminHome = "admin/allocations-management/home";

        /// <summary>
        /// The MVC route name the business allocations admin home page.
        /// </summary>
        public const string RouteName_BusinessAllocationsAdminHome = "Admin_BusinessAllocationsAdminHome";

        /// <summary>
        /// The MVC route for the business allocations run feed reader page.
        /// </summary>
        public const string Route_BusinessAllocationsAdminRunFeedReader = "admin/allocations-management/runfeedreader";

        /// <summary>
        /// The MVC route name the business allocations run feed reader page.
        /// </summary>
        public const string RouteName_BusinessAllocationsAdminRunFeedReader = "Admin_BusinessAllocationsAdminRunFeedReader";

        /// <summary>
        /// The MVC route for the business allocations 'feed reader last run' page.
        /// </summary>
        public const string Route_BusinessAllocationsFeedReaderLastRun = "admin/allocations-management/feedreaderlastrun";

        /// <summary>
        /// The MVC route name for the business allocations 'feed reader last run' page.
        /// </summary>
        public const string RouteName_BusinessAllocationsFeedReaderLastRun = "Admin_BusinessAllocationsFeedReaderLastRun";

        /// <summary>
        /// The MVC route for the business allocations 'feed reader last run' page.
        /// </summary>
        public const string Route_BusinessAllocationsFeedReaderLastRunConfirm = "admin/allocations-management/feedreaderlastrun/{FundingStreamCodes}";

        /// <summary>
        /// The MVC route name for the business allocations 'feed reader last run' page.
        /// </summary>
        public const string RouteName_BusinessAllocationsFeedReaderLastRunConfirm = "Admin_BusinessAllocationsFeedReaderLastRunConfirm";

        /// <summary>
        /// The MVC route for the business allocations 'feed reader last run' page.
        /// </summary>
        public const string Route_BusinessAllocationsSubmitRequest = "admin/allocations-management/submit";

        /// <summary>
        /// The MVC route name for the business allocations 'feed reader last run' page.
        /// </summary>
        public const string RouteName_BusinessAllocationsSubmitRequest = "Admin_BusinessAllocationsFeedReaderSubmit";

        /// <summary>
        /// The MVC route for the confirm business allocations run feed reader page.
        /// </summary>
        public const string Route_BusinessAllocationsConfirmRunFeedReader = "admin/allocations-management/confirmRunfeedreader";

        /// <summary>
        /// The MVC route name for the business allocations confirm run feed reader page.
        /// </summary>
        public const string RouteName_BusinessAllocationsConfirmRunFeedReader = "Admin_BusinessAllocationsConfirmRunFeedReader";

        /// <summary>
        /// The MVC route for business allocations confirmation page.
        /// </summary>
        public const string Route_BusinessAllocationsActionsConfirmation = "admin/allocations-management/confirmation/{success}/{businessAllocationsAction}/{FundingStreamCodeAndPeriodCode}/{SourceFolder?}/{TargetFolder?}";

        /// <summary>
        /// The MVC route name for the business allocations 'run feed reader confirmation' page.
        /// </summary>
        public const string RouteName_BusinessAllocationsActionsConfirmation = "Admin_BusinessAllocationsConfirmation";

        /// <summary>
        /// The MVC route for the business allocations run pdf compare page.
        /// </summary>
        public const string Route_BusinessAllocationsRunPdfComparison = "admin/allocations-management/runPdfComparison";

        /// <summary>
        /// The MVC route name the business allocations run pdf compare page.
        /// </summary>
        public const string RouteName_BusinessAllocationsRunPdfComparison = "Admin_BusinessAllocationsRunPdfComparison";

        /// <summary>
        /// The MVC route for the business allocations select funding stream page.
        /// </summary>
        public const string Route_BusinessAllocationsSelectFundingStream = "admin/allocations-management/selectFundingStream";

        /// <summary>
        /// The MVC route name the business allocations select funding stream page.
        /// </summary>
        public const string RouteName_BusinessAllocationsSelectFundingStream = "Admin_BusinessAllocationsSelectFundingStream";

        /// <summary>
        /// The MVC route for the business allocations search by provider and year page.
        /// </summary>
        public const string Route_BusinessAllocationsSearchByProviderAndYear = "admin/allocations-management/searchByProviderAndYear/{fundingStreamCode}";

        /// <summary>
        /// The MVC route name the business allocations search by provider and year page.
        /// </summary>
        public const string RouteName_BusinessAllocationsSearchByProviderAndYear = "Admin_BusinessAllocationsSearchByProviderAndYear";

        /// <summary>
        /// The MVC route for the business allocations search by provider and year details page.
        /// </summary>
        public const string Route_BusinessAllocationsSearchByProviderAndYearDetails = "admin/allocations-management/searchByProviderAndYearDetails/{fundingStreamCode}/{ukprn}/{fundingPeriodCode}";

        /// <summary>
        /// The MVC route name the business allocations search by provider and year details page.
        /// </summary>
        public const string RouteName_BusinessAllocationsSearchByProviderAndYearDetails = "Admin_BusinessAllocationsSearchByProviderAndYearDetails";

        /// <summary>
        /// The MVC route for the business allocations search by provider data result page.
        /// </summary>
        public const string Route_BusinessAllocationsSearchByProviderDataResult = "admin/allocations-management/searchByProviderDataResult";

        /// <summary>
        /// The MVC route name the business allocations search by provider data result page.
        /// </summary>
        public const string RouteName_BusinessAllocationsSearchByProviderDataResult = "Admin_BusinessAllocationsSearchByProviderDataResult";

        /// <summary>
        /// The value of the radio input that should map to the 'Selecting 14 to 16 Funding' action.
        /// </summary>
        public const string OptionValue_SelectingChoice_14To16Funding = "14-16Funding";

        /// <summary>
        /// The value of the radio input that should map to the 'Select 16 to 19 Funding' action.
        /// </summary>
        public const string OptionValue_SelectingChoice_16To19Funding = "16-19Funding";

        /// <summary>
        /// The value of the radio input that should map to the 'Select >Non-maintained special schools Funding' action.
        /// </summary>
        public const string OptionValue_SelectingChoice_NonMaintained = "Non-maintained";

        /// <summary>
        /// The MVC route for the business allocations run pdf compare page.
        /// </summary>
        public const string Route_BusinessAllocationsRunPdfComparisonConfirm = "admin/allocations-management/runPdfComparison/{FundingStreamCodeAndPeriodCode}/{SourceFolder}/{TargetFolder}";

        /// <summary>
        /// The MVC route name the business allocations run pdf compare page.
        /// </summary>
        public const string RouteName_BusinessAllocationsRunPdfComparisonConfirm = "Admin_BusinessAllocationsRunPdfComparisonConfirm";

        /// <summary>
        /// The MVC route for the  business allocations confirm run pdf comparison page.
        /// </summary>
        public const string Route_BusinessAllocationsConfirmRunPdfComparison = "admin/allocations-management/confirmRunPdfComparison";

        /// <summary>
        /// The MVC route name for the  business allocations confirm run pdf comparison page.
        /// </summary>
        public const string RouteName_BusinessAllocationsConfirmRunPdfComparison = "Admin_BusinessAllocationsConfirmRunPdfComparison";



        #endregion


        #region Setting type page

        /// <summary>
        /// The MVC route for the setting types page.
        /// </summary>
        public const string Route_AdminSettingTypesHome = "admin/settingTypes/home";

        /// <summary>
        /// The MVC route name for the setting types page.
        /// </summary>
        public const string RouteName_AdminSettingTypesHome = "Admin_GeneralSettingTypesHome";

        /// <summary>
        /// The MVC route for the setting types edit page.
        /// </summary>
        public const string Route_AdminSettingTypeAction = "admin/settingTypes/action/{settingTypeId}/{IsSettingTypeInUse}/{actionMode}";

        /// <summary>
        /// The MVC route name for the setting types edit page.
        /// </summary>
        public const string RouteName_AdminSettingTypeAction = "Admin_SettingTypeAction";

        /// <summary>
        /// The MVC route name for setting type confirmation.
        /// </summary>
        public const string Route_AdminSettingTypeConfirmation = "admin/settingTypes/confirmation/{settingTypeId}/{IsSettingTypeInUse}/{actionMode}";

        /// <summary>
        /// The MVC route name for the setting type confirmation page.
        /// </summary>
        public const string RouteName_AdminSettingTypeConfirmation = "Admin_SettingTypeConfirmation";

        /// <summary>
        /// The MVC route name for Setting Type Save Changes page.
        /// </summary>
        public const string Route_AdminSettingTypeSaveChanges = "admin/settingTypes/savechanges";

        /// <summary>
        /// The MVC route name for Setting Type Save Changes page.
        /// </summary>
        public const string RouteName_AdminSettingTypeSaveChanges = "Admin_SettingTypeSaveChanges";

        /// <summary>
        /// The MVC route name for Setting Type Are you sure  page.
        /// </summary>
        public const string Route_AdminSettingTypeAreYouSure = "admin/settingTypes/are-you-sure/{settingTypeId}/{actionMode}";

        /// <summary>
        /// The MVC route namee for Setting Type Are you sure  page.
        /// </summary>
        public const string RouteName_AdminSettingTypeAreYouSure = "Admin_SettingTypeAreYouSure";

        #endregion


        #region ViewingChoice page

        /// <summary>
        /// The MVC route for the 'Choose how to view funding' page.
        /// </summary>
        public const string Route_ViewingChoice = "viewing-choice";

        /// <summary>
        /// The MVC route name for the 'Choose how to view funding' page.
        /// </summary>
        public const string RouteName_ViewingChoice = "VYF_ViewingChoice";

        /// <summary>
        /// The page title for the 'Choose how to view funding' page.
        /// </summary>
        public const string PageTitle_ViewingChoice = "Choose how to view funding";

        /// <summary>
        /// The MVC route for the re-routing the result of the 'Choose how to view funding' page.
        /// </summary>
        public const string Route_ViewingChoiceChosen = "viewing-choice-chosen";

        /// <summary>
        /// The MVC route name for the re-routing the result of the 'Choose how to view funding' page.
        /// </summary>
        public const string RouteName_ViewingChoiceChosen = "VYF_ViewingChoiceChosen";

        /// <summary>
        /// The value of the radio input that should map to the 'View funding at organisation level' action.
        /// </summary>
        public const string OptionValue_ViewingChoice_FindAnOrganisation = "Organisation";

        /// <summary>
        /// The value of the radio input that should map to the 'Select a funding type' action.
        /// </summary>
        public const string OptionValue_ViewingChoice_WhichAllocation = "National";

        /// <summary>
        /// Maps the options on the 'Choose how to view funding' page to their respective target MVC actions.
        /// </summary>
        public static Dictionary<string, string> OptionMap_ViewingChoice = new Dictionary<string, string>
        {
            { OptionValue_ViewingChoice_FindAnOrganisation, RouteName_FindAnOrganisation },
            { OptionValue_ViewingChoice_WhichAllocation, RouteName_WhichAllocation }
        };

        #endregion


        #region FindAnOrganisation page

        /// <summary>
        /// The MVC route for the 'View funding at organisation level' page.
        /// </summary>
        public const string Route_FindAnOrganisation = "find-an-organisation";

        /// <summary>
        /// The MVC route name for the 'View funding at organisation level' page.
        /// </summary>
        public const string RouteName_FindAnOrganisation = "VYF_FindAnOrganisation";

        /// <summary>
        /// The page title for the 'View funding at organisation level' page.
        /// </summary>
        public const string PageTitle_FindAnOrganisation = "View funding at organisation level";

        /// <summary>
        /// The value of the radio input that should map to the Provider 'school or academy' results action.
        /// </summary>
        public const string OptionValue_FindAnOrganisation_Provider = "Provider";

        /// <summary>
        /// The value of the radio input that should map to the 'local authority name or code' action.
        /// </summary>
        public const string OptionValue_FindAnOrganisation_LocalAuthority = "LaCode";

        /// <summary>
        /// The display text for links back to the 'View funding at organisation level' page.
        /// </summary>
        public const string LinkText_FindAnOrganisation = "Find an organisation";

        #endregion


        #region Shared search constants

        /// <summary>
        /// The page title to use for search results pages.
        /// </summary>
        public const string PageTitle_SearchResults = "Search results";

        /// <summary>
        /// The page title to use for allocation history pages.
        /// </summary>
        public const string PageTitle_AllocationHistory = "Allocation history";

        #endregion


        #region Provider (School or Academy) results page

        /// <summary>
        /// The MVC route for the 'Provider search' page.
        /// </summary>
        public const string Route_ProviderSearch = "provider-search";

        /// <summary>
        /// The MVC route name for the 'Provider search' page.
        /// </summary>
        public const string RouteName_ProviderSearch = "VYF_ProviderSearch";

        /// <summary>
        /// The MVC route for the 'Provider did you mean' page.
        /// </summary>
        public const string Route_ProviderDidYouMean = "provider-did-you-mean";

        /// <summary>
        /// The MVC route name for the 'Provider did you mean' page.
        /// </summary>
        public const string RouteName_ProviderDidYouMean = "VYF_ProviderDidYouMean";

        /// <summary>
        /// The MVC route for the 'Provider no search results' page.
        /// </summary>
        public const string Route_ProviderNoResults = "provider-no-results";

        /// <summary>
        /// The MVC route name for the 'Provider no search results' page.
        /// </summary>
        public const string RouteName_ProviderNoResults = "VYF_ProviderNoResults";

        /// <summary>
        /// The MVC route for the 'Provider statement' page.
        /// </summary>
        public const string Route_ProviderStatement = "provider-statement/{organisationUkprn}";

        /// <summary>
        /// The MVC route name for the 'Provider statement' page.
        /// </summary>
        public const string RouteName_ProviderStatement = "VYF_ProviderStatement";

        /// <summary>
        /// The display text for resetting the search filters.
        /// </summary>
        public const string LinkText_ResetSearchFilter = "Reset filters";

        #endregion


        #region Provider history Page

        /// <summary>
        /// The MVC route for the 'provider history' page.
        /// </summary>
        public const string Route_ProviderHistory = "{fundingStreamName}/provider-allocation-history/{organisationUkprn}";

        /// <summary>
        /// The MVC route name for the 'Provider History' page.
        /// </summary>
        public const string RouteName_ProviderHistory = "VYF_ProviderAllocationHistory";

        /// <summary>
        /// The number of years of historic allocations to show.
        /// </summary>
        public const short NumberOfYearsOfHistoricAllocationsToShow = 3;

        #endregion


        #region Provider Funding Breakdown Page

        /// <summary>
        /// The MVC route for the 'Provider Funding Breakdown' page.
        /// </summary>
        public const string Route_ProviderFundingBreakdown = "{fundingStreamName}/provider-funding-breakdown/{publishDate}/{organisationUkprn}-{yearFrom}-to-{yearTo}";

        /// <summary>
        /// The MVC route name for the 'Provider Funding Breakdown' page.
        /// </summary>
        public const string RouteName_ProviderFundingBreakdown = "VYF_ProviderFundingBreakdown";

        #endregion


        #region Provider Funding Breakdown 201819 Page

        /// <summary>
        /// The MVC route for the 'Provider Funding Breakdown Single Year' page.
        /// </summary>
        public const string Route_ProviderHistorySingleYear = "{fundingStreamName}/provider-funding-breakdown-{year1}-to-{year2}/{organisationUkprn}";

        /// <summary>
        /// The MVC route name for the 'Provider Funding Breakdown Single Year' page.
        /// </summary>
        public const string RouteName_ProviderHistorySingleYear = "VYF_ProviderHistorySingleYear";

        #endregion


        #region Local Authority search

        /// <summary>
        /// The MVC route for the Local Authority search.
        /// </summary>
        public const string Route_LocalAuthoritySearch = "local-authority";

        /// <summary>
        /// The MVC route name for the Local Authority search.
        /// </summary>
        public const string RouteName_LocalAuthoritySearch = "VYF_LocalAuthoritySearch";

        /// <summary>
        /// The MVC route for the Local Authority 'did you mean' page.
        /// </summary>
        public const string Route_LocalAuthorityDidYouMean = Route_LocalAuthoritySearch + "/did-you-mean";

        /// <summary>
        /// The MVC route name for the Local Authority 'did you mean' page.
        /// </summary>
        public const string RouteName_LocalAuthorityDidYouMean = "VYF_LocalAuthorityDidYouMean";

        /// <summary>
        /// The MVC route for the Local Authority 'statement' page for {localAuthorityCode}.
        /// </summary>
        public const string Route_LocalAuthorityStatement = Route_LocalAuthoritySearch + "/statement/{localAuthorityCode}";

        /// <summary>
        /// The MVC route name for the Local Authority 'did you mean' page.
        /// </summary>
        public const string RouteName_LocalAuthorityStatement = "VYF_LocalAuthorityStatement";

        /// <summary>
        /// The MVC route for the Local Authority 'no results' page.
        /// </summary>
        public const string Route_LocalAuthorityNoResults = Route_LocalAuthoritySearch + "/no-results";

        /// <summary>
        /// The MVC route name for the Local Authority 'no results' page.
        /// </summary>
        public const string RouteName_LocalAuthorityNoResults = "VYF_LocalAuthorityNoResults";

        #endregion



        #region Local Authority History (single year) page

        /// <summary>
        /// The MVC route for the 'Local authority history' single year.
        /// </summary>
        public const string Route_LocalAuthorityHistorySingleYear =
            Route_LocalAuthoritySearch + "/{fundingStreamName}/funding-breakdown-{year1}-to-{year2}/{localAuthorityCode}";

        /// <summary>
        /// The MVC route name for the 'Local authority history' single page.
        /// </summary>
        public const string RouteName_LocalAuthorityHistorySingleYear = "VYF_LocalAuthorityHistorySingleYear";

        #endregion


        #region Local Authority History page

        /// <summary>
        /// The MVC route for the Local Authority 'allocation history' page.
        /// </summary>
        public const string Route_LocalAuthorityHistory =
            Route_LocalAuthoritySearch + "/allocation-history/{fundingStreamName}/{localAuthorityCode}";

        /// <summary>
        /// The MVC route name for the Local Authority 'allocation history' page.
        /// </summary>
        public const string RouteName_LocalAuthorityHistory = "VYF_LocalAuthorityHistory";

        #endregion


        #region WhichAllocation page

        /// <summary>
        /// The MVC route for the 'Select a funding type' page.
        /// </summary>
        public const string Route_WhichAllocation = "which-allocation";

        /// <summary>
        /// The MVC route name for the 'Select a funding type' page.
        /// </summary>
        public const string RouteName_WhichAllocation = "VYF_WhichAllocation";

        /// <summary>
        /// The page title for the 'Select a funding type' page.
        /// </summary>
        public const string PageTitle_WhichAllocation = "Select a funding type";

        /// <summary>
        /// The MVC route for the re-routing the result of the 'Select a funding type' page.
        /// </summary>
        public const string Route_WhichAllocationChosen = "which-allocation-chosen";

        /// <summary>
        /// The MVC route for the re-routing the result of the 'Select a funding type' page.
        /// </summary>
        public const string RouteName_WhichAllocationChosen = "VYF_WhichAllocationChosen";

        /// <summary>
        /// Maps the options on the 'Select a funding type' page to their respective behaviours.
        /// </summary>
        /// <param name="currentYearForFundingStream">The list containing latest/current year for funding streams.</param>
        /// <returns>A dictionary mapping the options to their respective behaviours.</returns>
        public static Dictionary<string, WhichAllocationOptionAction> OptionMap_WhichAllocation(
           List<FundingStreamCurrentYearViewModel> currentYearForFundingStream)
        {
            var allocationOptions = new Dictionary<string, WhichAllocationOptionAction>();

            foreach (var fundingStream in currentYearForFundingStream)
            {
                allocationOptions.Add(
                        fundingStream.FundingStreamCode,
                        new WhichAllocationOptionAction
                        {
                            RouteName = RouteName_NationalFundingAllocation,
                            RouteValues = new
                            {
                                fundingStreamCode = fundingStream.FundingStreamCode,
                                yearFrom = fundingStream.YearStart,
                                yearTo = fundingStream.YearEnd
                            },
                            UseRouteValuesWhenScriptsEnabled = true
                        });
            }

            return allocationOptions;
        }

        #endregion


        #region National Funding Allocations Download page

        /// <summary>
        /// The MVC route for the 'National Funding Allocations Download' page.
        /// </summary>
        public const string Route_NationalFundingAllocation = "national-funding-allocations/{fundingStreamCode}/{yearFrom}-to-{yearTo}";

        /// <summary>
        /// The MVC route name for the 'National Funding Allocations Download' page.
        /// </summary>
        public const string RouteName_NationalFundingAllocation = "VYF_NationalFundingAllocation";

        /// <summary>
        /// The breadcrumb text format string for the 'National Funding Allocations Download' for funding streams that display the funding stream code (e.g. DSG).
        /// </summary>
        public const string BreadCrumbTextFormat_FundingStreamCode = "<abbr title=\"{0}\">{3}</abbr> {1} to {2}";

        /// <summary>
        /// The breadcrumb text format string for the 'National Funding Allocations Download' for funding streams that display the funding stream name (e.g. PE and sport premium).
        /// </summary>
        public const string BreadCrumbTextFormat_FundingStreamName = "{0} {1} to {2}";

        #endregion National Funding Allocations Download page


        #region Common constants used across multiple pages

        /// <summary>
        /// The browser title format string.
        /// </summary>
        public const string BrowserTitleShortFormat_Common = "{2} {0} to {1}";

        /// <summary>
        /// The browser title format string for DSG pages.
        /// </summary>
        public const string BrowserTitleFormat_Common = "{2} ({3}) {0} to {1}";

        /// <summary>
        /// The common title format string for all 'Funding stream name {yearFrom} to {yearTo}' pages.
        /// </summary>
        /// <param name="fundingStreamName">The funding stream name (e.g. PE and sport premium).</param>
        /// <returns>A page title format.</returns>
        public static string PageTitleFormat_Common(string fundingStreamName)
        {
            return $"{fundingStreamName} {{0}} to {{1}}";
        }

        /// <summary>
        /// The content title format string for pages including the years.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingStreamName">The funding stream name (e.g. Dedicated schools grant).</param>
        /// <param name="canUseShortcode">Can the shortcode (e.g. DSG) be used.</param>
        /// <returns>A content title format string for.</returns>
        public static string ContentTitleFormat_Common(string fundingStreamCode, string fundingStreamName, bool canUseShortcode)
        {
            return ContentTitle_Common_NoYears(fundingStreamCode, fundingStreamName, canUseShortcode) + " {0} to {1}";
        }

        /// <summary>
        /// The shortest appropriate content title format string (e.g. DSG for DSG and 'PE and sport premium' for PSG).
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingStreamName">The funding stream name (e.g. Dedicated schools grant).</param>
        /// <param name="canUseShortcode">Can the shortcode (e.g. DSG) be used.</param>
        /// <returns>DSG for DSG and 'PE and sport premium' for PSG.</returns>
        public static string ContentTitle_ShortestAppropriate(string fundingStreamCode, string fundingStreamName, bool canUseShortcode)
        {
            return canUseShortcode ?
                fundingStreamCode :
                ContentTitle_Common_NoYears(fundingStreamCode, fundingStreamName, canUseShortcode);
        }

        /// <summary>
        /// The content title format string for pages including the years.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingStreamName">The funding stream name (e.g. Dedicated schools grant).</param>
        /// <param name="canUseShortcode">Can the shortcode (e.g. DSG) be used.</param>
        /// <returns>A content title format string for.</returns>
        public static string ContentTitle_Common_NoYears(string fundingStreamCode, string fundingStreamName, bool canUseShortcode)
        {
            return FundingStreamHelper.GetFundingStreamNameHtml(fundingStreamName, fundingStreamCode, canUseShortcode);
        }

        #endregion


        #region UnderConstruction page

        /// <summary>
        /// The MVC route for the 'Under construction' page.
        /// </summary>
        public const string Route_UnderConstruction = "under-construction";

        /// <summary>
        /// The MVC route name for the 'Under construction' page.
        /// </summary>
        public const string RouteName_UnderConstruction = "VYF_UnderConstruction";

        /// <summary>
        /// The page title for the 'Under construction' page.
        /// </summary>
        public const string PageTitle_UnderConstruction = "Not Implemented";

        #endregion


        #region Funding Breakdown page

        /// <summary>
        /// The MVC route for the 'Funding breakdown' page.
        /// </summary>
        public const string Route_LocalAuthorityFundingBreakdown = "{fundingStreamName}/funding-breakdown/{yearFrom}-to-{yearTo}/{localAuthorityCode}/{publishedDate}";

        /// <summary>
        /// The MVC route name for the 'Funding breakdown' page.
        /// </summary>
        public const string RouteName_LocalAuthorityFundingBreakdown = "VYF_FundingBreakdown";

        /// <summary>
        /// The content title format string for the 'Funding breakdown' page.
        /// E.g. DSG 2019 to 2020.
        /// </summary>
        /// <param name="canShowAbbreviation">Can the funding stream code be shown.</param>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingStreamName">The funding stream name (e.g. PE and sport).</param>
        /// <returns>The breadcrumb format.</returns>
        public static string BreadcrumbFormat_FundingBreakdown(bool canShowAbbreviation, string fundingStreamCode, string fundingStreamName)
        {
            return (canShowAbbreviation ? fundingStreamCode : fundingStreamName) + " {0} to {1}";
        }

        /// <summary>
        /// The page title for the 'DSG Funding breakdown' page.
        /// Download DSG allocation for Camden 2019 to 2020.
        /// </summary>
        public const string DocumentTitle_LAFundingBreakdown = "Download {3} allocation for {0} {1} to {2}";

        /// <summary>
        /// The page title for the 'Recoupment detail' page.
        /// </summary>
        public const string DocumentTitle_LARECFundingRecoupment = "Download {3} recoupment for {0} {1} to {2}";

        #endregion


        #region Provider spreadsheet download

        /// <summary>
        /// The MVC route for a provider spreadsheet download.
        /// </summary>
        public const string Route_ProviderSpreadsheetDownload = "{FundingStreamCode}/download-funding/provider/{Ukprn}/{YearTypeCode}/{YearFrom}-to-{YearTo}/{PublishedDate}/{Format}";

        /// <summary>
        /// The MVC route name for a provider spreadsheet download.
        /// </summary>
        public const string RouteName_ProviderSpreadsheetDownload = "VYF_ProviderSpreadsheetDownload";

        #endregion


        #region Organisation spreadsheet download

        /// <summary>
        /// The MVC route for an organisation spreadsheet download.
        /// </summary>
        public const string Route_LocalAuthoritySpreadsheetDownload = "{FundingStreamCode}/download-funding/organisation/{LocalAuthorityCode}/{YearTypeCode}/{YearFrom}-to-{YearTo}/{PublishedDate}/{Format}";

        /// <summary>
        /// The MVC route name for an organisation spreadsheet download.
        /// </summary>
        public const string RouteName_LocalAuthoritySpreadsheetDownload = "VYF_OrganisationSpreadsheetDownload";

        #endregion


        #region National spreadsheet download

        /// <summary>
        /// The MVC route for a preview national spreadsheet download.
        /// </summary>
        public const string Route_PreviewNationalSpreadsheetDownload = "{FundingStreamCode}/download-funding/national/{YearTypeCode}/{YearFrom}-to-{YearTo}/{PublishedDate}/{Format}";

        /// <summary>
        /// The MVC route name for a preview national spreadsheet download.
        /// </summary>
        public const string RouteName_PreviewNationalSpreadsheetDownload = "VYF_PreviewNationalSpreadsheetDownload";

        #endregion

        #region Banner related content for DSG

        public const string DsgNotificationBannerHeadingText = "The dedicated schools grant (DSG) allocations for the 2026 to 2027 financial year are not yet available in this service.";

        public const string DsgNotificationBannerLinkName = "https://www.gov.uk/government/publications/dedicated-schools-grant-dsg-2026-to-2027";

        public const string DsgNotificationBannerLinkText = "view the 2026 to 2027 DSG allocations";

        public const string DsgNotificationBannerBodyText = $@"You can <a class=""notification-banner-blue__link"" href=""{DsgNotificationBannerLinkName}"" rel=""external noopener noreferrer"" target=""_blank"">{DsgNotificationBannerLinkText}</a> on GOV.UK. DSG allocations for previous years are still available in this service.";

        #endregion
    }
}