using System;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Funding publication date helper.
    /// </summary>
    public static class FundingPublicationDateHelper
    {
        private const string SeptemberFullName = "September", AprilFullName = "April";
        private const string SeptemberShortName = "Sept", AprilShortName = "Apr";

        /// <summary>
        /// Gets the As of Month and Year values for an academic year.
        /// </summary>
        /// <param name="publicationDate">The publication date.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="useShortName">Whether to use a shortened name (e.g. Apr rather than April).</param>
        /// <returns>The As of Month and Year.</returns>
        public static (string month, string year) GetAcademicAsOfData(
            DateTime publicationDate,
            int yearFrom,
            int yearTo,
            bool useShortName = false)
        {
            if (publicationDate.Equals(DateTime.MinValue))
            {
                return (null, null);
            }

            string month;
            string year;

            var septemberCutOffDate = new DateTime(yearTo, 4, 1);

            if (publicationDate < septemberCutOffDate)
            {
                month = useShortName ? SeptemberShortName : SeptemberFullName;
                year = yearFrom.ToString();
            }
            else
            {
                month = useShortName ? AprilShortName : AprilFullName;
                year = yearTo.ToString();
            }

            return (month, year);
        }
    }
}