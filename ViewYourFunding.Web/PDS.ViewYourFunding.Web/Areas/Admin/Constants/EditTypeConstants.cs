using System.Globalization;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Constants
{
    /// <summary>
    /// The Date Constants class.
    /// </summary>
    public static class EditTypeConstants
    {
        /// <summary>
        /// The hour regex.
        /// </summary>
        public const string HourRegex = @"^([01]?[0-9]|2[0-3])$";

        /// <summary>
        /// The minutes regex.
        /// </summary>
        public const string MinutesRegex = @"^([0-5]?[0-9])$";

        /// <summary>
        /// The day regex.
        /// </summary>
        public const string DayRegex = @"^(0[1-9]|[1-9]|[1-2][0-9]|3[0-1])$";

        /// <summary>
        /// The month regex.
        /// </summary>
        public const string MonthRegex = @"^(0[1-9]|[1-9]|1[0-2])$";

        /// <summary>
        /// The year regex.
        /// </summary>
        public const string YearRegex = @"[0-9]{4}";

        /// <summary>
        /// The day regex error message.
        /// </summary>
        public const string DayRegexErrorMessage = "Day must be a number between 1 and 31";

        /// <summary>
        /// The month regex error message.
        /// </summary>
        public const string MonthRegexErrorMessage = "Month must be a number between 1 and 12";

        /// <summary>
        /// The year regex error message.
        /// </summary>
        public const string YearRegexErrorMessage = "Year must be a 4 digit number";

        /// <summary>
        /// The hour regex error message.
        /// </summary>
        public const string HourRegexErrorMessage = "Hour must be a number between 0 and 23";

        /// <summary>
        /// The minutes regex error message.
        /// </summary>
        public const string MinutesRegexErrorMessage = "Minutes must be a number between 0 and 59";

        /// <summary>
        /// The day required error message.
        /// </summary>
        public const string DayRequiredErrorMessage = "Date Day is required";

        /// <summary>
        /// The month required error message.
        /// </summary>
        public const string MonthRequiredErrorMessage = "Month is required";

        /// <summary>
        /// The year required error message.
        /// </summary>
        public const string YearRequiredErrorMessage = "Year is required";

        /// <summary>
        /// The hour required error message.
        /// </summary>
        public const string HourRequiredErrorMessage = "Hour is required";

        /// <summary>
        /// The minutes required error message.
        /// </summary>
        public const string MinutesRequiredErrorMessage = "Minute is required";

        /// <summary>
        /// The date format.
        /// </summary>
        public const string DateFormat = "dd/MM/yyyy";

        /// <summary>
        /// The date time format.
        /// </summary>
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";

        /// <summary>
        /// The time format.
        /// </summary>
        public const string TimeFormat = "HH:mm";

        /// <summary>
        /// The en-GB culture name.
        /// </summary>
        public const string EnGbCultureName = "en-GB";

        /// <summary>
        /// The en-GB culture information.
        /// </summary>
        public static CultureInfo EnGbCultureInfo = new CultureInfo(EnGbCultureName);

        /// <summary>
        /// The new value is required error message.
        /// </summary>
        public const string NewValueRequiredErrorMessage = "New value required.";

        /// <summary>
        /// The range validation message for new value of type int.
        /// </summary>
        public const string RangeRequiredErrorMessage = "Range is between 1 and 2000000000.";

        /// <summary>
        /// The non date minimum error message.
        /// </summary>
        public const string NonDateMinErrorMessage = "The date is not valid.";
    }
}