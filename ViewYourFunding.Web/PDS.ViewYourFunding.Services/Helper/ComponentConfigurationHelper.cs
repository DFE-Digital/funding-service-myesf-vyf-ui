using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Models;
using System;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// The component configuration helper class.
    /// </summary>
    public static class ComponentConfigurationHelper
    {
        private const string AcademyConverterOpeningReason = "Academy Converter";
        private const string NewProvisionOpeningReason = "New Provision";
        private const string ReBrokerageOpeningReason = "Fresh Start";

        /// <summary>
        /// Determines whether the specified date opened is an academic year (academy converter) in year opener.
        /// </summary>
        /// <param name="dateOpened">The date opened.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="openReason">The open reason.</param>
        /// <returns>
        ///  True if the specified date opened is an academic year (academy converter) in year opener.
        /// </returns>
        public static bool IsAcademyConverterOrNewProvisionInYearOpener(this DateTime? dateOpened, int yearFrom, int yearTo, string openReason)
        {
            if (dateOpened == null)
            {
                return default;
            }

            var iyoStartDate = new DateTime(yearFrom, 8, 31);
            var iyoEndDate = new DateTime(yearTo, 7, 31);

            return dateOpened >= iyoStartDate && dateOpened <= iyoEndDate &&
                   (AcademyConverterOpeningReason.Equals(openReason, StringComparison.InvariantCultureIgnoreCase)
                    || NewProvisionOpeningReason.Equals(openReason, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Determines whether the specified date opened is an academic year in year opener.
        /// </summary>
        /// <param name="dateOpened">The date opened.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <returns>
        ///  True if the specified date opened is an academic year in year opener.
        /// </returns>
        public static bool IsAcademicYearInYearOpener(this DateTime? dateOpened, int yearFrom, int yearTo)
        {
            if (dateOpened == null)
            {
                return default;
            }

            var iyoStartDate = new DateTime(yearFrom, 8, 31);
            var iyoEndDate = new DateTime(yearTo, 7, 31);

            return dateOpened >= iyoStartDate && dateOpened <= iyoEndDate;
        }

        /// <summary>
        /// Determines whether this institution is an in year opener.
        /// </summary>
        /// <param name="inYearOpenerCalculatorSource">The in year opener calculator source.</param>
        /// <returns>
        ///  True if this institution is an in year opener.
        /// </returns>
        public static bool IsInYearOpener(InYearOpenerCalculatorSource inYearOpenerCalculatorSource)
        {
            return
                (inYearOpenerCalculatorSource.IsOpenDaysEqualToFullYearDays ||
                 inYearOpenerCalculatorSource.IsPreviousYearPostAprilOpener ||
                 inYearOpenerCalculatorSource.IsAcademicYearInYearOpener) &&
                !(ReBrokerageOpeningReason.Equals(inYearOpenerCalculatorSource.OpeningReason, StringComparison.InvariantCultureIgnoreCase)
                    || inYearOpenerCalculatorSource.ReBrokerageCloseReason);
        }

        /// <summary>
        /// Builds the variance display message.
        /// </summary>
        /// <param name="previousPublicationDate">The previous publication date.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="option">The oprtion selected.</param>
        /// <returns>The variance message built.</returns>
        public static string GetVarianceMessage(DateTime previousPublicationDate, int yearFrom, int yearTo, VarianceSelectionOption option)
        {
            var publicationDate = previousPublicationDate.ToString("dd MMMM yyyy");
            switch (option)
            {
                case VarianceSelectionOption.FinalStatementPreviousYear:
                    return $"This statement is being compared to the {yearFrom - 1} to {yearFrom} statement published on {publicationDate}";
                case VarianceSelectionOption.PreviousStatementCurrentYear:
                    return $"This statement is being compared to the {yearFrom} to {yearTo} statement published on {publicationDate}";
                default:
                    return "The figures in this statement are not being compared to another statement";
            }
        }
    }
}