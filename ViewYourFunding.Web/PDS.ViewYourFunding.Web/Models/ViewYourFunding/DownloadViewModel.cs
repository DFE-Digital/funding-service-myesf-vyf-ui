using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.ResponseObjects;
using PDS.ViewYourFunding.Web.Models.Shared;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The view model to use for the 'Funding Stream {year}' page.
    /// </summary>
    public class DownloadViewModel : BaseViewYourFundingPageViewModel
    {
        private const short NumberOfYearsOfHistoricAllocationsToShow = 3;
        private const short LastYearForWhichToShowExternalHistoricAllocationsLink = 2017;
        private IReadOnlyCollection<FundingDocument> _fundingDocuments = new List<FundingDocument>();

        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle => PageTitle;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the title in the page's main content section.
        /// </summary>
        public override bool ShowContentTitle => false;

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string ContentTitle => PageTitle;

        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems => new List<BreadCrumbViewModel>
        {
            ViewingChoicePageBreadCrumb(false),
            WhichAllocationPageBreadCrumb(false),
        };

        /// <summary>
        /// Gets a value indicating whether whether or not to show the 'related' sections.
        /// </summary>
        public override bool ShowRelatedSections => true;

        /// <summary>
        /// Gets or sets the start year of the funding period being displayed.
        /// </summary>
        public int YearFrom { get; set; }

        /// <summary>
        /// Gets or sets the end year of the funding period being displayed.
        /// </summary>
        public int YearTo { get; set; }

        /// <summary>
        /// Gets or sets the start year of the current/latest funding period.
        /// </summary>
        public int LatestYearFrom { get; set; }

        /// <summary>
        /// Gets or sets the end year of the current/latest funding period.
        /// </summary>
        public int LatestYearTo { get; set; }

        /// <summary>
        /// Gets or sets the name of the funding stream.
        /// </summary>
        public string FundingStreamName { get; set; }

        /// <summary>
        /// <code>Gets a value indicating whether true</code> if displaying the current year's funding,. <code>false</code> for historic funding.
        /// </summary>
        public bool IsCurrentYear => YearFrom == LatestYearFrom && YearTo == LatestYearTo;

        /// <summary>
        /// Gets or sets the collection of spreadsheet documents, each of which should have a link on the page.
        /// </summary>
        public IReadOnlyCollection<FundingDocument> Spreadsheets
        {
            get
            {
                return _fundingDocuments?
                    .Where(fundingDocument =>
                        fundingDocument.FileExtension == FundingDocumentFileType.Spreadsheet_OpenFormat)
                    .OrderByDescending(s => s.DocumentPublishedDate)
                    .ToList();
            }

            set
            {
                _fundingDocuments = value;
            }
        }

        private string PageTitle => string.Format(ViewYourFundingConstants.PageTitleFormat_Common(FundingStreamName), YearFrom, YearTo);
    }
}