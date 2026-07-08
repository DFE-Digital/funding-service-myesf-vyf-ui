using System;
using System.Globalization;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Extension class for DateTime related modifications.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// The format provider.
        /// </summary>
        public static IFormatProvider FormatProvider = CultureInfo.GetCultureInfo("en-GB");

        /// <summary>
        /// The route parameter format.
        /// </summary>
        public const string RouteParameterFormat = "d-M-yyyy";

        /// <summary>
        /// The UI display format.
        /// </summary>
        public const string UiDisplayFormat = "d MMMM yyyy";

        /// <summary>
        /// The filename format.
        /// </summary>
        public const string FilenameFormat = "dd-MM-yyyy";

        /// <summary>
        /// Get a date time in GB culture format.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>A datetime like '24/04/2020 00:00:00'.</returns>
        public static string ToGBFormat(this DateTime dateTime)
        {
            return dateTime.ToString(FormatProvider);
        }

        /// <summary>
        /// Extension method to convert a date into a UTC string with the 'Z' to specify no offset.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>A string in UTC Z format.</returns>
        public static string ToUTCZFormat(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("s") + "Z";
        }

        /// <summary>
        /// Extension method to convert a date time into a formatted date string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// The formatted date e.g. 4 October 2019.
        /// </returns>
        public static string ToDateDisplay(this DateTime dateTime)
        {
            return dateTime.ToString(UiDisplayFormat);
        }

        /// <summary>
        /// Extension method to convert a date time into a filename formatted date string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// The formatted date e.g. 04-10-2019.
        /// </returns>
        public static string ToFilenameString(this DateTime dateTime)
        {
            return dateTime.ToString(FilenameFormat);
        }

        /// <summary>
        /// Extension method to convert a DateTime to the format used in route parameters (d-M-yyyy).
        /// </summary>
        /// <param name="dateTime">The DateTime.</param>
        /// <returns>
        /// The formatted date e.g. 31-1-2020.
        /// </returns>
        public static string ToRouteParameterString(this DateTime dateTime)
        {
            return dateTime.ToString(RouteParameterFormat);
        }

        /// <summary>
        /// Extension method to convert a string into a date, using the format used in route parameters (d-M-yyyy).
        /// </summary>
        /// <param name="routeParameterString">The string in the route parameter date format.</param>
        /// <returns>
        /// The date that was contained in the string.
        /// </returns>
        /// <exception cref="System.Exception">route parameter not as expected.</exception>
        public static DateTime ToRouteParameterDate(this string routeParameterString)
        {
            if (DateTime.TryParseExact(
                routeParameterString,
                RouteParameterFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var routeParameterDate))
            {
                return routeParameterDate;
            }

            throw new Exception($"{routeParameterString} is not in the expected format ({RouteParameterFormat})");
        }

        /// <summary>
        /// A date format that can be compared with other dates to find which is the earliest/latest.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The Date time in yyyyMMdd_HHmmss format.</returns>
        /// <param name="includeDateTimeSeperator">Whether to include the date time underscore seperator (defaults to false).</param>
        /// <returns>A date formatted as yyyyMMdd_HHmmss or yyyyMMddHHmmss.</returns>
        public static string ToIncrementalDateFormat(this DateTime dateTime, bool includeDateTimeSeperator = false)
        {
            return dateTime.ToString(includeDateTimeSeperator ? "yyyyMMdd_HHmmss" : "yyyyMMddHHmmss");
        }

        /// <summary>
        /// Extension method to convert a date into a datetime string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The census date display.</returns>
        public static string ToCensusDateDisplay(this DateTime dateTime)
        {
            return dateTime.ToString("h:mmtt").ToLower() + dateTime.ToString(" dd MMMM yyyy");
        }

        /// <summary>
        /// Extension method to convert a date time into a timestamp string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>Date time formatted as a time stamp.</returns>
        public static string ToTimestampDisplay(this DateTime dateTime)
        {
            return dateTime.ToString("dd:mmmm:yyyy h:mmtt").ToLower();
        }

        /// <summary>
        /// Extension method to convert a nullable date time object into a formatted date string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToDateDisplay(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToDateDisplay();
            }

            return string.Empty;
        }

        /// <summary>
        /// Extension method to convert a date time into a formatted date string with at string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToDateTimeDisplayWithOn(this DateTime dateTime)
        {
            return dateTime.ToString("h:mmtt").ToLower() + " on " + dateTime.ToString("dd MMMM yyyy");
        }

        /// <summary>
        /// Extension method to convert a date time into a formatted date string with at string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToDateTimeDisplayWithAt(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMMM yyyy") + " at " + dateTime.ToString("h:mmtt").ToLower();
        }

        /// <summary>
        /// Extension method to convert a nullable date time into a formatted date string with on string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToDateTimeDisplayWithOn(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToDateTimeDisplayWithOn();
            }

            return string.Empty;
        }

        /// <summary>
        /// Extension method to convert a nullable date time into a formatted date string with at string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToDateTimeDisplayWithAt(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToDateTimeDisplayWithAt();
            }

            return string.Empty;
        }

        /// <summary>
        /// Extension method to convert a date time into a formatted date string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToDateTimeDisplay(this DateTime dateTime)
        {
            return dateTime.ToString("h:mmtt ").ToLower() + " " + dateTime.ToString("dd MMMM yyyy");
        }

        /// <summary>
        /// Extension method to convert a date time into a formatted date string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToUIDateFormat(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMMM yyyy");
        }

        /// <summary>
        /// Extension method to convert a nullable date time object into a formatted date string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToDateTimeDisplay(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToDateTimeDisplay();
            }

            return string.Empty;
        }

        /// <summary>
        /// Extension method to convert a date time into a formatted date string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToMonthYearDisplay(this DateTime dateTime)
        {
            return dateTime.ToString("MMMM-yy");
        }

        /// <summary>
        /// Extension method to convert a date time into a formatted date string.
        /// Format will be MMMM yyyy e.g. March 2016.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToFullMonthAndFullYearDisplay(this DateTime dateTime)
        {
            return dateTime.ToString("MMMM yyyy");
        }

        /// <summary>
        /// Extension method to convert a date time into a formatted date string.
        /// Format will be MMM yyyy e.g. Mar 2016.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToAbbreviatedMonthAndFullYearDisplay(this DateTime dateTime)
        {
            return dateTime.ToString("MMM yyyy");
        }

        /// <summary>
        /// Extension method to convert a date time into a formatted date string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToPerformanceData(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MMMM-dd");
        }

        /// <summary>
        /// Extension method to convert a date time into a formatted time string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted time.</returns>
        public static string ToTimeDisplay(this DateTime dateTime)
        {
            return dateTime.ToString("h:mmtt").ToLower();
        }

        /// <summary>
        /// Extension method to convert a date time into a formatted time string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted time.</returns>
        public static string ToTimeDisplay(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToTimeDisplay();
            }

            return string.Empty;
        }

        /// <summary>
        /// Extension method to convert a date time into a formatted date string with hyphens.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToDateFormattedDisplay(this DateTime dateTime)
        {
            return $"{dateTime:dd/MMMM/yy}";
        }

        /// <summary>
        /// Extension method to convert a date time into a formatted date timestamp.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToDateFormattedDatetimeStamp(this DateTime dateTime)
        {
            return $"{dateTime:dd/MMMM/yyyy HH:mm:ss}";
        }

        /// <summary>
        /// Extension method to convert a date time into a formatted date.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The formatted date.</returns>
        public static string ToDateFormattedDate(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MMMM/yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Extension method to convert a date time in to the format of dd/mm/yyyy hh:mm am/pm.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The am or pm formatted date.</returns>
        public static string ToDateFormattedWithAmOrPm(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MMMM/yyyy h:mmtt").ToLower();
        }

        /// <summary>
        /// Extension method to convert a date time to be in GMT standard time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The GMT standard time.</returns>
        public static DateTime ToGmtStandardTime(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
        }

        /// <summary>
        /// Extension method to convert a GMT datetime to a UTC datetime.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The UTC time.</returns>
        public static DateTime ToUtcTime(this DateTime dateTime)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZone);
        }

        /// <summary>
        /// Extension method to convert a date time to the format of d MMMM yyyy.
        /// i.e. 1 August 2017.
        /// </summary>
        /// <param name="dateTimeToFormat">The date time.</param>
        /// <returns>The Date display with no leading zero.</returns>
        public static string ToDateDisplayWithNoLeadingZero(this DateTime dateTimeToFormat)
        {
            return dateTimeToFormat.ToString("d MMMM yyyy");
        }

        /// <summary>
        /// Converts the UTC date time to GB date time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The GB Date time.</returns>
        public static DateTime ConvertUtcDateTimeToGmtDateTime(this DateTime dateTime)
        {
            DateTime britishLocalTime;
            var inputDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            try
            {
                britishLocalTime = inputDateTime.ToGmtStandardTime();
            }
            catch
            {
                var localTime = DateTime.Now;
                var localTimeUtc = DateTime.Now.ToUniversalTime();
                var offSet = localTime - localTimeUtc;
                britishLocalTime = inputDateTime + offSet;
            }

            return britishLocalTime;
        }

        /// <summary>
        /// Extension method to convert a date time to be in GMT standard time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The GMT standard time.</returns>
        public static DateTime? ToGmtStandardTime(this DateTime? dateTime)
        {
            return dateTime != null ? ToGmtStandardTime(dateTime.Value) : (DateTime?)null;
        }

        /// <summary>
        /// Extension method to get the number of days in the year.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>Number of days in the year.</returns>
        public static int NumberOfDaysInYear(this DateTime dateTime)
        {
            return DateTime.IsLeapYear(dateTime.Year) ? 366 : 365;
        }
    }
}