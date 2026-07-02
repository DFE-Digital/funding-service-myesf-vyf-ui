using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The view model to use for the 'View funding breakdown' page.
    /// </summary>
    public class FundingBreakdownViewModel : FilterableSearchResultsViewModel
    {
        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle
        {
            get
            {
                return string.Format(
                    ViewYourFundingConstants.BrowserTitleFormat_Common,
                    YearFrom,
                    YearTo,
                    FundingStreamConfiguration.FundingStreamName,
                    FundingStreamConfiguration.FundingStreamCode);
            }
        }

        /// <summary>
        /// Gets a value indicating whether whether or not to show the title in the page's main content section.
        /// </summary>
        public override bool ShowContentTitle => false;

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string ContentTitle
        {
            get
            {
                return string.Format(
                    ViewYourFundingConstants.ContentTitleFormat_Common(
                        FundingStreamConfiguration.FundingStreamCode,
                        FundingStreamConfiguration.FundingStreamName,
                        FundingStreamConfiguration.FundingStreamCodePubliclyKnown),
                    YearFrom,
                    YearTo);
            }
        }

        /// <summary>
        /// Gets a value indicating whether use full column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
        {
            get
            {
                var result = new List<BreadCrumbViewModel>
                {
                    ViewingChoicePageBreadCrumb(false),
                    FindAnOrganisationPageBreadCrumb(false),
                };

                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    result.Add(LocalAuthorityDidYouMeanPageBreadCrumb(false, SearchTerm));
                }

                result.Add(LocalAuthorityStatementPageBreadCrumb(false, LocalAuthorityCode, LocalAuthorityName, SearchTerm));

                if (IncludeHistory)
                {
                    result.Add(LocalAuthorityAllocationHistoryPageBreadCrumb(
                        false,
                        ViewYourFundingConstants.RouteName_LocalAuthorityHistory,
                        LocalAuthorityCode,
                        FundingStreamConfiguration.FundingStreamName,
                        SearchTerm));
                }

                result.Add(FundingBreakdownPageBreadCrumb(
                    true,
                    YearFrom,
                    YearTo,
                    FundingStreamConfiguration.FundingStreamCodePubliclyKnown,
                    FundingStreamConfiguration.FundingStreamCode,
                    FundingStreamConfiguration.FundingStreamName));

                return result;
            }
        }

        #region Domain specific properties

        /// <summary>
        /// Gets or sets the 1st year for import/export adjustments.
        /// </summary>
        public int ImportExportAdjustmentYear1 { get; set; }

        /// <summary>
        /// Gets or sets the 2nd year for import/export adjustments.
        /// </summary>
        public int ImportExportAdjustmentYear2 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the funding total after recoupment.
        /// </summary>
        public bool IsAfterRecoupment { get; set; }

        /// <summary>
        /// Gets or sets the string 'after recoupment' or string.Empty.
        /// </summary>
        public string AfterRecoupmentOrEmpty;

        /// <summary>
        /// Gets or sets the string 'initial', 'indicitve' or string.Empty.
        /// </summary>
        public string InitialIndicativeOrEmpty;

        /// <summary>
        /// Gets or sets local authority name e.g. Camden.
        /// </summary>
        public string LocalAuthorityName { get; set; }

        /// <summary>
        /// Gets or sets local authority code e.g. 202.
        /// </summary>
        public string LocalAuthorityCode { get; set; }

        /// <summary>
        /// Gets or sets the start year of the funding period being displayed.
        /// </summary>
        public int YearFrom { get; set; }

        /// <summary>
        /// Gets or sets the end year of the funding period being displayed.
        /// </summary>
        public int YearTo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is this the latest (or final) funding for the year being shown?.
        /// </summary>
        public bool IsLatestOrFinalFundingForYear { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is the current year's funding being shown?.
        /// </summary>
        public bool IsCurrentYear { get; set; }

        /// <summary>
        /// Gets or sets the downloadable document for this funding breakdown.
        /// </summary>
        public FundingDocument Document { get; set; }

        /// <summary>
        /// Gets get the downloadable document's title.
        /// </summary>
        public string DocumentTitle
        {
            get
            {
                return string.Format(ViewYourFundingConstants.DocumentTitle_LAFundingBreakdown, LocalAuthorityName, YearFrom, YearTo, FundingStreamConfiguration.FundingStreamCode);
            }
        }

        /// <summary>
        /// Gets or sets the configuration for the funding stream.
        /// </summary>
        public FundingStream.FundingStream FundingStreamConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the funding period code (e.g. AY-1920).
        /// </summary>
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the funding view data for this funding breakdown.
        /// </summary>
        public FundingViewData FundingViewData { get; set; }

        /// <summary>
        /// Gets or sets tab that was selected to come through to funding breakdown page, if it has tabs.
        /// </summary>
        public string Tab { get; set; }

        /// <summary>
        /// Gets or sets the 'as of' month of the provider data, if it is a breakdown of provider funding.
        /// </summary>
        public string AsOfMonth { get; set; }

        /// <summary>
        /// Gets or sets the 'as of' year of the provider data, if it is a breakdown of provider funding.
        /// </summary>
        public string AsOfYear { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether include history allocation page.
        /// </summary>
        public bool IncludeHistory { get; set; }

        #endregion
    }
}