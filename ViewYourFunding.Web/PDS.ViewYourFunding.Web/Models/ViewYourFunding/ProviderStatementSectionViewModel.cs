using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// A section of the provider statement page.
    /// </summary>
    public class ProviderStatementSectionViewModel
    {
        /// <summary>
        /// Gets or sets the provider result.
        /// </summary>
        /// <value>
        /// The provider result.
        /// </value>
        public IFundingApiSearchProviderFunding ProviderResult { get; set; }

        /// <summary>
        /// Gets or sets the current year start.
        /// </summary>
        /// <value>
        /// The current year start.
        /// </value>
        public int CurrentYearStart { get; set; }

        /// <summary>
        /// Gets or sets the current year end.
        /// </summary>
        /// <value>
        /// The current year end.
        /// </value>
        public int CurrentYearEnd { get; set; }

        /// <summary>
        /// Gets or sets the funding stream configuration.
        /// </summary>
        public FundingStream.FundingStream FundingStreamConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the current 'as of' month of funding stream allocations that should be shown.
        /// </summary>
        /// <value>
        /// The current as of month.
        /// </value>
        public string CurrentAsOfMonth { get; set; }

        /// <summary>
        /// Gets or sets the current 'as of' year of funding stream allocations that should be shown.
        /// </summary>
        /// <value>
        /// The current as of year.
        /// </value>
        public string CurrentAsOfYear { get; set; }

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        public FundingDocument Document { get; set; }

        /// <summary>
        /// Gets or sets the search term.
        /// </summary>
        /// <value>
        /// The search term.
        /// </value>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets the document title.
        /// </summary>
        /// <value>
        /// The document title.
        /// </value>
        public string DocumentTitle => $"Download {FundingStreamConfiguration.FundingStreamName} allocation for {ProviderResult.OrganisationName}" + " {0} to {1}";

        /// <summary>
        /// Gets or sets the next available payment date.
        /// </summary>
        public DateTime? NextPaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the no next payment for the year text.
        /// </summary>
        /// <value>
        /// The no next payment for the year text.
        /// </value>
        public string NoNextPaymentForTheYearText { get; set; }

        /// <summary>
        /// Gets or sets the no next payment for organisation text.
        /// </summary>
        /// <value>
        /// The no next payment for organisation text.
        /// </value>
        public string NoNextPaymentForOrganisationText { get; set; } =
            "There are no more scheduled payments for this organisation.";

        /// <summary>
        /// Gets a value indicating whether the organisation is closed.
        /// </summary>
        public bool OrganisationClosed =>
            "Closed".Equals(ProviderResult.ProviderStatus, StringComparison.InvariantCultureIgnoreCase);
    }
}