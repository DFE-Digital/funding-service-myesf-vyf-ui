using PDS.ViewYourFunding.Web.Areas.Admin.Attributes;
using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication
{
    /// <summary>
    /// The PublicationDate model class.
    /// </summary>
    public class PublicationDates
    {
        /// <summary>
        /// The day regex.
        /// </summary>
        private const string DayRegex = @"^(0[1-9]|[1-9]|[1-2][0-9]|3[0-1])$";

        /// <summary>
        /// The month regex.
        /// </summary>
        private const string MonthRegex = @"^(0[1-9]|[1-9]|1[0-2])$";

        /// <summary>
        /// The year regex.
        /// </summary>
        private const string YearRegex = @"[0-9]{4}";

        /// <summary>
        /// The day regex error message.
        /// </summary>
        private const string DayRegexErrorMessage = "Day must be a number between 1 and 31";

        /// <summary>
        /// The month regex error message.
        /// </summary>
        private const string MonthRegexErrorMessage = "Month must be a number between 1 and 12";

        /// <summary>
        /// The year regex error message.
        /// </summary>
        private const string YearRegexErrorMessage = "Year must be a 4 digit number";

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the funding period code, e.g. FY-2021.
        /// </summary>
        [Display(Name = "Funding Period Code")]
        [Required, MaxLength(16)]
        [RegularExpression(FundingPeriodConstants.FundingPeriodCodeRegexPattern, ErrorMessage = FundingPeriodConstants.FundingPeriodCodeErrorMessage)]
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream name.
        /// </summary>
        /// <value>
        /// The funding stream name.
        /// </value>
        public int FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets the published date.
        /// </summary>
        /// <value>
        /// The published date.
        /// </value>
        [DistinctPublishedDate(nameof(FundingStreamId), nameof(FundingPeriodCode), nameof(Id), ErrorMessage = "Please use a distinct publish date for this funding stream.")]
        [Display(Name = "Published Date")]
        public DateTime PublishedDate
        {
            get
            {
                var enGb = new CultureInfo("en-GB");
                var publishDate = string.Empty;
                if (!string.IsNullOrEmpty(PublishedDateDay) && !string.IsNullOrEmpty(PublishedDateMonth) && !string.IsNullOrEmpty(PublishedDateYear))
                {
                    publishDate =
                        $"{PublishedDateDay.PadLeft(2, '0')}/{PublishedDateMonth.PadLeft(2, '0')}/{PublishedDateYear.PadLeft(4, '0')}";
                }

                if (DateTime.TryParseExact(publishDate, "dd/MM/yyyy", enGb, DateTimeStyles.AdjustToUniversal, out var publishedDate))
                {
                    return publishedDate;
                }

                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Gets or sets the published date day.
        /// </summary>
        /// <value>
        /// The published date day.
        /// </value>
        [Display(Name = "Day")]
        [Required(ErrorMessage = "Published Date Day is required")]
        [RegularExpression(DayRegex, ErrorMessage = DayRegexErrorMessage)]
        public string PublishedDateDay { get; set; }

        /// <summary>
        /// Gets or sets the published date month.
        /// </summary>
        /// <value>
        /// The published date month.
        /// </value>
        [Display(Name = "Month")]
        [Required(ErrorMessage = "Published Date Month is required")]
        [RegularExpression(MonthRegex, ErrorMessage = MonthRegexErrorMessage)]
        public string PublishedDateMonth { get; set; }

        /// <summary>
        /// Gets or sets the published date year.
        /// </summary>
        /// <value>
        /// The published date year.
        /// </value>
        [Display(Name = "Year")]
        [Required(ErrorMessage = "Published Date Year is required")]
        [RegularExpression(YearRegex, ErrorMessage = YearRegexErrorMessage)]
        public string PublishedDateYear { get; set; }

        /// <summary>
        /// Gets the cut off date.
        /// </summary>
        /// <value>
        /// The cut off date.
        /// </value>
        [Display(Name = "Cut Off Date")]
        public DateTime? CutOffDate
        {
            get
            {
                var enGb = new CultureInfo("en-GB");
                var cutoffDate = string.Empty;
                if (!string.IsNullOrEmpty(CutOffDateDay) && !string.IsNullOrEmpty(CutOffDateMonth) && !string.IsNullOrEmpty(CutOffDateYear))
                {
                    cutoffDate =
                        $"{CutOffDateDay.PadLeft(2, '0')}/{CutOffDateMonth.PadLeft(2, '0')}/{CutOffDateYear.PadLeft(4, '0')}";
                }

                if (string.IsNullOrEmpty(cutoffDate) && string.IsNullOrEmpty(CutOffDateDay) && string.IsNullOrEmpty(CutOffDateMonth) && string.IsNullOrEmpty(CutOffDateYear))
                {
                    // CutOffDate can be null
                    return null;
                }
                else
                {
                    if (DateTime.TryParseExact(cutoffDate, "dd/MM/yyyy", enGb, DateTimeStyles.AdjustToUniversal, out var cutOffDate))
                    {
                        return cutOffDate;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the cut off date day.
        /// </summary>
        /// <value>
        /// The cut off date day.
        /// </value>
        [Display(Name = "Day")]
        [RegularExpression(DayRegex, ErrorMessage = DayRegexErrorMessage)]
        public string CutOffDateDay { get; set; }

        /// <summary>
        /// Gets or sets the cut off date month.
        /// </summary>
        /// <value>
        /// The cut off date month.
        /// </value>
        [Display(Name = "Month")]
        [RegularExpression(MonthRegex, ErrorMessage = MonthRegexErrorMessage)]
        public string CutOffDateMonth { get; set; }

        /// <summary>
        /// Gets or sets the cut off date year.
        /// </summary>
        /// <value>
        /// The cut off date year.
        /// </value>
        [Display(Name = "Year")]
        [RegularExpression(YearRegex, ErrorMessage = YearRegexErrorMessage)]
        public string CutOffDateYear { get; set; }
    }
}