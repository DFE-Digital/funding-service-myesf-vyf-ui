namespace PDS.VYF.Services.Models.ViewDataModels
{
    using PDS.ViewYourFunding.Services.Helper;

    /// <summary>
    /// The class for Child History Page Row.
    /// </summary>
    public class ChildHistoryPageRow
    {
        /// <summary>
        /// Gets or sets the date the allocation was published.
        /// </summary>
        public DateTime StatusChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the description associated with the published allocation.
        /// </summary>
        public string VariationReason { get; set; } = default!;

        /// <summary>
        /// Gets or sets the grouping reason associated with the published allocation.
        /// </summary>
        public string GroupingReason { get; set; } = default!;

        /// <summary>
        /// Gets the date the allocation was published, as a UI formatted string.
        /// </summary>
        public string StatusChangedDateUiFormatted => DateTimeExtensions.ToDateDisplay(this.StatusChangedDate);

        /// <summary>
        /// Gets the date the allocation was published, as a path formatted (e.g. dd-M-yyyy) string.
        /// </summary>
        public string StatusChangedDatePathFormatted => this.StatusChangedDate.ToString("dd-M-yyyy");

        /// <summary>
        /// Gets or sets the funding period code, e.g. FY-2021.
        /// </summary>
        public virtual string FundingPeriodCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets the first year.
        /// </summary>
        public int Year1 => !string.IsNullOrEmpty(this.FundingPeriodCode) ? FundingPeriodHelper.GetYearsFromCode(this.FundingPeriodCode).yearFrom : -1;

        /// <summary>
        /// Gets the second year.
        /// </summary>
        public int Year2 => !string.IsNullOrEmpty(this.FundingPeriodCode) ? FundingPeriodHelper.GetYearsFromCode(this.FundingPeriodCode).yearTo : -1;

        /// <summary>
        /// Gets or sets a value indicting whether its the latest publication.
        /// </summary>
        public bool? IsLatest { get; set; }

        /// <summary>
        /// Gets or sets a value indicting whether its the final publication.
        /// </summary>
        public bool? IsFinal { get; set; }
    }
}
