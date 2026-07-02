using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Shared;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.PdfGenerationActions
{
    /// <summary>
    /// Base class for view your funding settings pages view models.
    /// </summary>
    public abstract class PdfGenerationActionsPageViewModel : AdminPageBaseViewModel
    {
        /// <summary>
        /// The view your funding settings bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for the view your funding settings page.</returns>
        protected BreadCrumbViewModel PdfGenerationActionsListBreadCrumb(bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Document Generation Actions",
                        RouteName = ViewYourFundingConstants.RouteName_PdfGenerationActionsHome,
                    }
                };

        /// <summary>
        /// The run feed reader page bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for run feed reader page.</returns>
        protected BreadCrumbViewModel RunFeedReaderBreadCrumb(bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Run Feed reader",
                        RouteName = ViewYourFundingConstants.RouteName_PdfGenerationActionsRunFeedReader,
                    }
                };

        /// <summary>
        /// The run Pdf Comparison page bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for run Pdf Comparison page page.</returns>
        protected BreadCrumbViewModel RunPdComparisonBreadCrumb(bool isCurrentPage = false) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = "Run Pdf Comparison",
                    RouteName = ViewYourFundingConstants.RouteName_PdfGenerationActionsRunPdfComparison,
                }
            };

        protected BreadCrumbViewModel GenerateFundingReportsBreadCrumb(bool isCurrentPage = false) =>
         new BreadCrumbViewModel
         {
             IsCurrentPage = isCurrentPage,
             Link = new MvcRouteLinkViewModel
             {
                 LinkText = "Generate Funding Report",
                 RouteName = ViewYourFundingConstants.RouteName_PdfGenerationActionsGenerateFundingReports,
             }
         };

        /// <summary>
        /// The run feed reader confirmation page bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for run feed reader confirmation page.</returns>
        protected BreadCrumbViewModel RunFeedReaderConfirmationBreadCrumb(bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Confirmation",
                        RouteName = ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmation,
                    }
                };

        /// <summary>
        /// The feed reader last run status page bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for feed reader last run status page.</returns>
        protected BreadCrumbViewModel FeedReaderLastRunBreadCrumb(bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Last Run Status",
                        RouteName = ViewYourFundingConstants.RouteName_PdfGenerationActionsFeedReaderLastRun,
                    }
                };

        protected BreadCrumbViewModel GetConfirmationBreadCrumb(PdfGenerationAction pdfGenerationAction)
        {
            return pdfGenerationAction switch
            {
                PdfGenerationAction.RunFeedReader => RunFeedReaderBreadCrumb(),
                PdfGenerationAction.PdfComparison => RunPdComparisonBreadCrumb(),
                PdfGenerationAction.GenerateSinglePdf => GenerateSinglePdfCrumb(),
                PdfGenerationAction.PdfGenerateFundingReport => GenerateFundingReportsBreadCrumb(),
                PdfGenerationAction.RerunPdfGeneration => RerunPdfGenerationBreadCrumb(),
                _ => throw new ArgumentOutOfRangeException(nameof(pdfGenerationAction), pdfGenerationAction, null)
            };
        }

        /// <summary>
        /// The Generate Single Pdf page bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for run feed reader confirmation page.</returns>
        protected BreadCrumbViewModel GenerateSinglePdfCrumb(bool isCurrentPage = false) =>
        new BreadCrumbViewModel
        {
            IsCurrentPage = isCurrentPage,
            Link = new MvcRouteLinkViewModel
            {
                LinkText = "Generate Single Document",
                RouteName = ViewYourFundingConstants.RouteName_PdfGenerationActionsGenerateSinglePdf,
            }
        };

        /// <summary>
        /// The rerun feed reader confirmation page bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for re-run feed reader confirmation page.</returns>
        protected BreadCrumbViewModel RerunPdfGenerationBreadCrumb(bool isCurrentPage = false) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = "Rerun Document Generation",
                    RouteName = ViewYourFundingConstants.RouteName_PdfGenerationActionsRerunPdfGeneration,
                }
            };
    }
}