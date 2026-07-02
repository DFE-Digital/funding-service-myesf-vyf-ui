using PDS.ViewYourFunding.Web.Areas.Admin.Attributes;
using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit
{
    /// <summary>
    /// The Date type edit class.
    /// </summary>
    /// <seealso cref="DataTypeBaseEdit" />
    public class DateTypeEdit : DataTypeBaseEdit
    {
        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        [Display(Name = "Date")]
        [NonMinValueDate(ErrorMessage = EditTypeConstants.NonDateMinErrorMessage)]
        public DateTime Date
        {
            get
            {
                var currentDate = string.Empty;
                if (!string.IsNullOrEmpty(Day) && !string.IsNullOrEmpty(Month) && !string.IsNullOrEmpty(Year))
                {
                    currentDate =
                        $"{Day.PadLeft(2, '0')}/{Month.PadLeft(2, '0')}/{Year.PadLeft(4, '0')}";
                }

                if (DateTime.TryParseExact(
                    currentDate,
                    EditTypeConstants.DateFormat,
                    EditTypeConstants.EnGbCultureInfo,
                    DateTimeStyles.AdjustToUniversal,
                    out var date))
                {
                    return date;
                }

                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Gets or sets the date day.
        /// </summary>
        /// <value>
        /// The date day.
        /// </value>
        [Required(ErrorMessage = EditTypeConstants.DayRequiredErrorMessage)]
        [RegularExpression(EditTypeConstants.DayRegex, ErrorMessage = EditTypeConstants.DayRegexErrorMessage)]
        public string Day { get; set; }

        /// <summary>
        /// Gets or sets the date month.
        /// </summary>
        /// <value>
        /// The date month.
        /// </value>
        [Required(ErrorMessage = EditTypeConstants.MonthRequiredErrorMessage)]
        [RegularExpression(EditTypeConstants.MonthRegex, ErrorMessage = EditTypeConstants.MonthRegexErrorMessage)]
        public string Month { get; set; }

        /// <summary>
        /// Gets or sets the date year.
        /// </summary>
        /// <value>
        /// The date year.
        /// </value>
        [Required(ErrorMessage = EditTypeConstants.YearRequiredErrorMessage)]
        [RegularExpression(EditTypeConstants.YearRegex, ErrorMessage = EditTypeConstants.YearRegexErrorMessage)]
        public string Year { get; set; }
    }
}