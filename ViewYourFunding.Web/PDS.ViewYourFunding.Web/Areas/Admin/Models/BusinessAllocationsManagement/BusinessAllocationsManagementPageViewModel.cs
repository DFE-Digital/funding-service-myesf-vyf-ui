using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Shared;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.BusinessAllocationsManagement
{
    /// <summary>
    /// Base class for view your funding settings pages view models.
    /// </summary>
    public abstract class BusinessAllocationsManagementPageViewModel : AdminPageBaseViewModel
    {
        /// <summary>
        /// The business allocations data admin home page.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for the business allocations data admin home pagee.</returns>
        protected BreadCrumbViewModel BusinessAllocationsManagementHomeBreadCrumb(bool isCurrentPage = false) =>
                new ()
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Manage allocations data",
                        RouteName = ViewYourFundingConstants.RouteName_BusinessAllocationsAdminHome,
                    }
                };

        /// <summary>
        /// The business allocations compare pdf bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for the business allocations pdf comparison page.</returns>
        protected BreadCrumbViewModel RunPdfCompareBreadCrumb(bool isCurrentPage = false) =>
                new ()
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Compare statements",
                        RouteName = ViewYourFundingConstants.RouteName_BusinessAllocationsRunPdfComparison,
                    }
                };

        protected BreadCrumbViewModel RunPdfConfirmComparisonBreadCrumb(string fundingStreamCodeAndPeriodCode, string sourceFolder, string targetFolder, bool isCurrentPage = false) =>
                new ()
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Confirm comparison",
                        RouteName = ViewYourFundingConstants.RouteName_BusinessAllocationsRunPdfComparisonConfirm,
                        Parameters = new
                        {
                            FundingStreamCodeAndPeriodCode = fundingStreamCodeAndPeriodCode,
                            SourceFolder = sourceFolder,
                            TargetFolder = targetFolder
                        }
                    }
                };

        /// <summary>
        /// The run feed reader page bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for run feed reader page.</returns>
        protected BreadCrumbViewModel RunFeedReaderBreadCrumb(bool isCurrentPage = false) =>
                new ()
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Request allocations data",
                        RouteName = ViewYourFundingConstants.RouteName_BusinessAllocationsAdminRunFeedReader,
                    }
                };

        /// <summary>
        /// The feed reader last run status page bread crumb.
        /// </summary>
        /// <param name="fundingStreamCodes">the funding Stream Codes.</param>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for feed reader last run status page.</returns>
        protected BreadCrumbViewModel FeedReaderLastRunBreadCrumb(string fundingStreamCodes, bool isCurrentPage = false)
        {
            return new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = "Confirm data request",
                    RouteName = ViewYourFundingConstants.RouteName_BusinessAllocationsFeedReaderLastRunConfirm,
                    Parameters = new
                    {
                        FundingStreamCodes = fundingStreamCodes
                    }
                }
            };
        }

        /// <summary>
        /// The feed reader last run status page bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for feed reader last run status page.</returns>
        protected BreadCrumbViewModel CheckStatusBreadCrumb(bool isCurrentPage = false) =>
                new ()
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Check status",
                        RouteName = ViewYourFundingConstants.RouteName_BusinessAllocationsFeedReaderLastRun
                    }
                };

        /// <summary>
        /// The confirmation page bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for confirmation page.</returns>
        protected BreadCrumbViewModel ConfirmationBreadCrumb(bool isCurrentPage = false) =>
                new ()
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Confirmation",
                        RouteName = ViewYourFundingConstants.RouteName_BusinessAllocationsActionsConfirmation,
                    }
                };

        /// <summary>
        /// The business allocations search provider data bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for the business allocations search provider data page.</returns>
        protected BreadCrumbViewModel SearchProviderDataBreadCrumb(bool isCurrentPage = false) =>
                new ()
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Search provider data",
                        RouteName = ViewYourFundingConstants.RouteName_BusinessAllocationsAdminHome,
                    }
                };

        /// <summary>
        /// The business allocations select a funding stream bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for the business allocations select a funding stream page.</returns>
        protected BreadCrumbViewModel SelectAFundingStreamBreadCrumb(bool isCurrentPage = false) =>
                new ()
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Select a funding stream",
                        RouteName = ViewYourFundingConstants.RouteName_BusinessAllocationsSelectFundingStream,
                    }
                };

        /// <summary>
        /// The business allocations search by provider and year bread crumb.
        /// </summary>
        /// <param name="fundingStreamCode">the funding Stream Codes..</param>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for the business allocations search by provider and year page.</returns>
        protected BreadCrumbViewModel SearchByProviderAndYearBreadCrumb(string fundingStreamCode, bool isCurrentPage = false) =>
                new ()
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Search by provider and year",
                        RouteName = ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderAndYear,
                        Parameters = new
                        {
                            FundingStreamCode = fundingStreamCode
                        }
                    }
                };

        /// <summary>
        /// The business allocations search by provider and year details bread crumb.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code to be used in link.</param>
        /// <param name="fundingPeriodCode">The funding period code to be used in link.</param>
        /// <param name="providerName">The provider name to display in the breadcrumb.</param>
        /// <param name="ukprn">The Ukprn to display in the breadcrumb.</param>
        /// /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for the business allocations search by provider and year details page.</returns>
        protected BreadCrumbViewModel SearchByProviderAndYearDetailsBreadCrumb(string fundingStreamCode, string fundingPeriodCode, string providerName, string ukprn, bool isCurrentPage = false) =>
                new ()
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $"{providerName} (UKPRN: {ukprn})",
                        RouteName = ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderAndYearDetails,
                        Parameters = new
                        {
                            FundingStreamCode = fundingStreamCode,
                            Ukprn = ukprn,
                            FundingPeriodCode = fundingPeriodCode
                        }
                    }
                };

        /// <summary>
        /// The business allocations Search by provider data result bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for the business allocations search by provider data result page.</returns>
        protected BreadCrumbViewModel SearchByProviderDataResultBreadCrumb(bool isCurrentPage = false) =>
                new ()
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Search result",
                        RouteName = ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderDataResult,
                    }
                };
    }
}