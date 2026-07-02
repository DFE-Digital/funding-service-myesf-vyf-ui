using PDS.ViewYourFunding.Services.Helper;
using System;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Models
{
    public class LocalAuthorityFundingViewModel
    {
        /// <summary>
        /// Gets or sets the date the allocation was published.
        /// </summary>
        public DateTime StatusChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the description associated with the published allocation.
        /// </summary>
        public string VariationReason { get; set; }

        /// <summary>
        /// Gets the date the allocation was published, as a UI formatted string.
        /// </summary>
        public string StatusChangedDateUiFormatted
        {
            get
            {
                return DateTimeExtensions.ToDateDisplay(StatusChangedDate);
            }
        }

        /// <summary>
        /// Gets the date the allocation was published, as a path formatted (e.g. dd-M-yyyy) string.
        /// </summary>
        public string StatusChangedDatePathFormatted
        {
            get
            {
                return StatusChangedDate.ToString("dd-M-yyyy");
            }
        }

        /// <summary>
        /// Gets or sets the funding period code, e.g. FY-2021.
        /// </summary>
        public virtual string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets the first year.
        /// </summary>
        public int Year1
        {
            get
            {
                return !string.IsNullOrEmpty(FundingPeriodCode) ? FundingPeriodHelper.GetYearsFromCode(FundingPeriodCode).yearFrom : -1;
            }
        }

        /// <summary>
        /// Gets the second year.
        /// </summary>
        public int Year2
        {
            get
            {
                return !string.IsNullOrEmpty(FundingPeriodCode) ?
                    FundingPeriodHelper.GetYearsFromCode(FundingPeriodCode).Item2 : -1;
            }
        }

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