using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Constants
{
    /// <summary>
    /// The Payment type code class.
    /// </summary>
    public static class PaymentTypeCode
    {
        /// <summary>
        /// The type code for funding specific e.g. DSG.
        /// </summary>
        public const string FundingSpecific = "FS";

        /// <summary>
        /// The type code for maintained schools.
        /// </summary>
        public const string MaintainedSchool = "MS";

        /// <summary>
        /// The type code for academies.
        /// </summary>
        public const string Academy = "AD";

        /// <summary>
        /// The type code for Non maintained special schools.
        /// </summary>
        public const string NonMaintainedSpecialSchool = "NMSS";

        /// <summary>
        /// Gets the next payment date.
        /// </summary>
        /// <param name="nextPayments">The next payments.</param>
        /// <param name="nextPaymentDateTypeCode">The next payment date type code.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <returns>The next payment date.</returns>
        public static DateTime? GetNextPaymentDate(
            IEnumerable<NextPayment> nextPayments,
            string nextPaymentDateTypeCode,
            string fundingPeriodCode)
        {
            var nextPaymentsList = nextPayments?.ToList();
            if (nextPaymentsList?.Any() == true)
            {
                return nextPaymentsList
                    .Where(x => x.Active)
                    .OrderBy(x => x.NextPaymentDate)
                    .FirstOrDefault(x =>
                        fundingPeriodCode.Equals(x.FundingPeriodCode, StringComparison.InvariantCultureIgnoreCase)
                        && nextPaymentDateTypeCode.Equals(x.NextPaymentTypeCode, StringComparison.InvariantCultureIgnoreCase)
                        && x.NextPaymentDate.Date >= DateTime.Now.Date)?.NextPaymentDate;
            }

            return null;
        }

        /// <summary>
        /// Get the text to show if there is no next payment date for the year (inferred from funding period code).
        /// </summary>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <returns>The text to display.</returns>
        public static string GetNoNextPaymentForTheYearText(string fundingPeriodCode)
        {
            var years = GetYearsFromCode(fundingPeriodCode);
            var yearTypeName = GetYearTypeNameFromCode(fundingPeriodCode);

            return $"There are no more scheduled payments for {yearTypeName} {years.fundingPeriodStartYear} to {years.fundingPeriodEndYear}.";
        }

        /// <summary>
        /// Get the years from a funding period code (e.g. FY-1920).
        /// </summary>
        /// <param name="fundingPeriodCode">Funding period code (e.g. FY-1920).</param>
        /// <returns>A tuple, where the first value is the first (from) year, and the second is the second (to) year.</returns>
        private static (int fundingPeriodStartYear, int fundingPeriodEndYear) GetYearsFromCode(string fundingPeriodCode)
        {
            if (string.IsNullOrEmpty(fundingPeriodCode))
            {
                return (0, 0);
            }

            var fundingPeriod = fundingPeriodCode.Substring(fundingPeriodCode.Length - 4);

            var fundingPeriodStartYear = 2000 + int.Parse(fundingPeriod.Substring(0, 2));
            var fundingPeriodEndYear = 2000 + int.Parse(fundingPeriod.Substring(2, 2));

            return (fundingPeriodStartYear, fundingPeriodEndYear);
        }

        /// <summary>
        /// Get the year type from a funding period code (e.g. FY-1920).
        /// </summary>
        /// <param name="fundingPeriodCode">Funding period code (e.g. FY-1920).</param>
        /// <returns>The year type (e.g. 'financial year').</returns>
        private static string GetYearTypeNameFromCode(string fundingPeriodCode)
        {
            var yearTypeCode = fundingPeriodCode.Split('-')[0];

            switch (yearTypeCode)
            {
                case YearTypeCode.AcademicYear:
                case YearTypeCode.AcademyAcademicYear:
                case YearTypeCode.AcademyAndSchoolAcademicYear:
                    return "academic year";
                case YearTypeCode.FinancialYear:
                    return "financial year";
            }

            return null;
        }
    }
}