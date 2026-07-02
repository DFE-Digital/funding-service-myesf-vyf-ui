using PDS.ViewYourFunding.Web.Areas.Admin.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment
{
    /// <summary>
    /// The next payment dates view model .
    /// </summary>
    public class NextPaymentDatesViewModel
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
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the next payment type identifier.
        /// </summary>
        /// <value>
        /// The next payment identifier.
        /// </value>
        public int NextPaymentTypeId { get; set; }

        /// <summary>
        /// Gets the NextPayment date.
        /// </summary>
        /// <value>
        /// The Next Payment date.
        /// </value>
        [Display(Name = "NextPayment Date")]
        [DistinctNextPaymentDate(nameof(FundingStreamId), nameof(NextPaymentTypeId), nameof(Id), ErrorMessage = "Please use a distinct publish date for this funding stream.")]
        public DateTime NextPaymentDate
        {
            get
            {
                var enGb = new CultureInfo("en-GB");
                var nextPaymentDate = string.Empty;
                if (!string.IsNullOrEmpty(NextPaymentDateDay) && !string.IsNullOrEmpty(NextPaymentDateMonth) && !string.IsNullOrEmpty(NextPaymentDateYear))
                {
                    nextPaymentDate =
                        $"{NextPaymentDateDay.PadLeft(2, '0')}/{NextPaymentDateMonth.PadLeft(2, '0')}/{NextPaymentDateYear.PadLeft(4, '0')}";
                }

                if (DateTime.TryParseExact(nextPaymentDate, "dd/MM/yyyy", enGb, DateTimeStyles.AdjustToUniversal, out var nextPaymentDateResult))
                {
                    return nextPaymentDateResult;
                }

                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Gets or sets the NextPayment date day.
        /// </summary>
        /// <value>
        /// The Next Payment date day.
        /// </value>
        [Display(Name = "Day")]
        [Required(ErrorMessage = "NextPayment Date Day is required")]
        [RegularExpression(DayRegex, ErrorMessage = DayRegexErrorMessage)]
        public string NextPaymentDateDay { get; set; }

        /// <summary>
        /// Gets or sets the Next Payment date month.
        /// </summary>
        /// <value>
        /// The Next Payment date month.
        /// </value>
        [Display(Name = "Month")]
        [Required(ErrorMessage = "NextPayment Date Month is required")]
        [RegularExpression(MonthRegex, ErrorMessage = MonthRegexErrorMessage)]
        public string NextPaymentDateMonth { get; set; }

        /// <summary>
        /// Gets or sets the Next Payment date year.
        /// </summary>
        /// <value>
        /// The Next Payment date year.
        /// </value>
        [Display(Name = "Year")]
        [Required(ErrorMessage = "NextPayment Date Year is required")]
        [RegularExpression(YearRegex, ErrorMessage = YearRegexErrorMessage)]
        public string NextPaymentDateYear { get; set; }
    }
}
