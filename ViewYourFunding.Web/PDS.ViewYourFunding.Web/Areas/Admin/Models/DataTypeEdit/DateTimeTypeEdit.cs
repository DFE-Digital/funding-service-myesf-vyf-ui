using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit
{
    /// <summary>
    /// The Datetime type edit class.
    /// </summary>
    /// <seealso cref="DataTypeBaseEdit" />
    public class DateTimeTypeEdit : DateTypeEdit
    {
        /// <summary>
        /// Gets the Date time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [Display(Name = "Date time")]
        public DateTime DateTime
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Hour) && !string.IsNullOrWhiteSpace(Minute))
                {
                    var currentDate = $"{Date.ToString(EditTypeConstants.DateFormat)} {Hour.PadLeft(2, '0')}:{Minute.PadLeft(2, '0')}";

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

                return DateTime.MinValue;
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