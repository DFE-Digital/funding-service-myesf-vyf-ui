using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit
{
    /// <summary>
    /// The time type edit class.
    /// </summary>
    /// <seealso cref="DataTypeBaseEdit" />
    public class TimeTypeEdit : DataTypeBaseEdit
    {
        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [Display(Name = "Time")]
        public DateTime TimeValue
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Hour) && !string.IsNullOrWhiteSpace(Minute))
                {
                    var currentDate = $"{DateTime.Now.ToString(EditTypeConstants.DateFormat)} {Hour.PadLeft(2, '0')}:{Minute.PadLeft(2, '0')}";

                    if (DateTime.TryParseExact(
                        currentDate,
                        EditTypeConstants.DateTimeFormat,
                        EditTypeConstants.EnGbCultureInfo,
                        DateTimeStyles.AdjustToUniversal,
                        out var date))
                    {
                        return date;
                    }
                }

                return DateTime.Now;
            }
        }

        /// <summary>
        /// Gets or sets the hour.
        /// </summary>
        /// <value>
        /// The hour.
        /// </value>
        [Required(ErrorMessage = EditTypeConstants.HourRequiredErrorMessage)]
        [RegularExpression(EditTypeConstants.HourRegex, ErrorMessage = EditTypeConstants.HourRegexErrorMessage)]
        public string Hour { get; set; }

        /// <summary>
        /// Gets or sets the minutes.
        /// </summary>
        /// <value>
        /// The minutes.
        /// </value>
        [Required(ErrorMessage = EditTypeConstants.MinutesRequiredErrorMessage)]
        [RegularExpression(EditTypeConstants.MinutesRegex, ErrorMessage = EditTypeConstants.MinutesRegexErrorMessage)]
        public string Minute { get; set; }
    }
}