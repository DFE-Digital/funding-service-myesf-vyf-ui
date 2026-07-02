namespace PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels
{
    using PDS.ViewYourFunding.Services.Enums;
    using PDS.ViewYourFunding.Services.Interfaces.Models;
    using PDS.ViewYourFunding.Services.Models;
    using PDS.ViewYourFunding.Services.ResponseObjects;

    /// <summary>
    /// Base class for ViewDataRequest models.
    /// </summary>
    public abstract class ViewDataRequestBase
    {
        /// <summary>
        /// Gets the cache key for the request.
        /// </summary>
        public abstract string CacheKey { get; }

        /// <summary>
        /// Gets or sets the funding stream code.
        /// </summary>
        public string FundingStreamCode { get; set; } = default!;

        /// <summary>
        /// Gets or sets the funding period code.
        /// </summary>
        public string FundingPeriodCode { get; set; } = default!;

        /// <summary>
        /// Gets or sets the funding data.
        /// </summary>
        public IFundingApiSearchResponseFunding? FundingData { get; set; }

        /// <summary>
        /// Gets or sets the provider funding data.
        /// </summary>
        public IFundingApiSearchResponseProviderFunding? ProviderFundingData { get; set; }

        /// <summary>
        /// Gets or sets the previous provider funding data.
        /// </summary>
        /// <value>
        /// The previous provider funding data.
        /// </value>
        public IFundingApiSearchResponseProviderFunding? PreviousProviderFundingData { get; set; }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        public DateTime? PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the previous publication date.
        /// </summary>
        public DateTime? PreviousPublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the funding stream configuration.
        /// </summary>
        public FundingStream? FundingStreamConfig { get; set; }

        /// <summary>
        /// Gets or sets the funding document.
        /// </summary>
        public FundingDocument? FundingDocument { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is the latest or final funding for the year.
        /// </summary>
        public bool IsLatestOrFinalFundingForYear { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is the current year.
        /// </summary>
        public bool IsCurrentYear { get; set; }

        /// <summary>
        /// Gets or sets the publication UI model version.
        /// </summary>
        public int? PublicationUiModelVersion { get; set; }

        /// <summary>
        /// Gets or sets the search term.
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the selected tab.
        /// </summary>
        public string? SelectedTab { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a logged-in view.
        /// </summary>
        public bool IsALoggedInView { get; set; } = true;

        /// <summary>
        /// Gets or sets the selected variance option.
        /// </summary>
        public VarianceSelectionOption? SelectedVarianceOption { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether it is via choice page.
        /// </summary>
        public bool ViaChoicePage { get; set; }

        /// <summary>
        /// Gets or sets the schema version.
        /// </summary>
        public string? SchemaVersion { get; set; }

        /// <summary>
        /// Gets or sets the template version.
        /// </summary>
        public string? TemplateVersion { get; set; }

        /// <summary>
        /// Gets or sets the funding view type.
        /// </summary>
        public FundingViewType FundingViewType { get; set; } = FundingViewType.ViewData;

        /// <summary>
        /// Gets or sets the funding view scope.
        /// </summary>
        public FundingViewScope FundingViewScope { get; set; }
    }
}
