using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Helper method to get the current and historic funding stream periods.
    /// </summary>
    public static class CurrentAndHistoricFundingStreamPeriodsHelper
    {
        /// <summary>
        /// Gets the current and historic funding stream periods.
        /// </summary>
        /// <param name="fundingPeriod">fundingPeriod ex: AC-2425.</param>
        /// <param name="fundingStreamCode">fundingStreamCode ex: GAG.</param>
        /// <param name="digitalStatementsGoLiveDate">digital statements go live date.</param>
        /// <param name="fundingStreamPeriod">fundingStreamPeriod. ex: GAG-AC-2425.</param>
        /// <returns>The current and historic funding stream periods.</returns>
        public static List<string> GetCurrentAndHistoricFundingStreamPeriods(string fundingPeriod, string fundingStreamCode, DateTime? digitalStatementsGoLiveDate, string fundingStreamPeriod)
        {
            var periodStartYear = -1;
            var periodEndYear = -1;
            var fundingStreamAndPeriods = new List<string>();

            (periodStartYear, periodEndYear) = FundingPeriodHelper.GetYearsFromCode(fundingPeriod);
            var year1Short = periodStartYear.ToString().Length > 2 ? periodStartYear.ToString().Substring(2) : null;
            var year2Short = periodEndYear.ToString().Length > 2 ? periodEndYear.ToString().Substring(2) : null;
            var year0 = periodStartYear - 1;
            var year0Short = year0.ToString().Length > 2 ? year0.ToString().Substring(2) : null;
            var fundingCode = FundingPeriodHelper.GetYearTypeCodeFromFundingPeriodCode(fundingPeriod);

            // Since Gag Digital IYO only available from 2526 year onwards
            if (periodStartYear == digitalStatementsGoLiveDate?.Year)
            {
                fundingStreamAndPeriods.Add(fundingStreamCode + "-" + fundingCode + "-" + year1Short + year2Short);
            }
            else if (periodStartYear > digitalStatementsGoLiveDate?.Year && digitalStatementsGoLiveDate != DateTime.MinValue)
            {
                fundingStreamAndPeriods.Add(fundingStreamCode + "-" + fundingCode + "-" + year1Short + year2Short);
                fundingStreamAndPeriods.Add(fundingStreamCode + "-" + fundingCode + "-" + year0Short + year1Short);
            }
            else
            {
                fundingStreamAndPeriods.Add(fundingStreamPeriod);
            }

            return fundingStreamAndPeriods;
        }
    }
}