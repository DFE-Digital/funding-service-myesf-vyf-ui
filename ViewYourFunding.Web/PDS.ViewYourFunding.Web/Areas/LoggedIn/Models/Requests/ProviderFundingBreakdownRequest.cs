using PDS.ViewYourFunding.Services.Enums;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.Requests
{
    /// <summary>
    /// Request object for a provider funding breakdown request.
    /// </summary>
    public class ProviderFundingBreakdownRequest
    {
        /// <summary>
        /// Gets or sets the funding stream name.
        /// </summary>
        public string FundingStreamNamePathPart { get; set; }

        /// <summary>
        /// Gets or sets ukprn.
        /// </summary>
        public string Ukprn { get; set; }

        /// <summary>
        /// Gets or sets year from (e.g. 2020).
        /// </summary>
        public int YearFrom { get; set; }

        /// <summary>
        /// Gets or sets year to (e.g. 2020).
        /// </summary>
        public int YearTo { get; set; }

        /// <summary>
        /// Gets or sets published date .e.g. 2020-01-01.
        /// </summary>
        public string PublishedDate { get; set; }

        /// <summary>
        /// Gets or sets tab that was selected to come through to funding breakdown page.
        /// </summary>
        public string Tab { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user entered via the choose allocation type (pre or post 16 page).
        /// </summary>
        public bool ViaChoicePage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user entered via the variance page.
        /// </summary>
        public bool ViaVariancePage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether include history allocation page.
        /// </summary>
        public bool IncludeHistory { get; set; }

        /// <summary>
        /// Gets or sets the selected option.
        /// </summary>
        public VarianceSelectionOption? SelectedVarianceOption { get; set; } = null;
    }
}