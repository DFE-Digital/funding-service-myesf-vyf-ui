using Pds.Core.Utils.Helpers;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using YearTypeCode = PDS.ViewYourFunding.Services.Constants.YearTypeCode;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Helper methods for working with funding periods.
    /// </summary>
    public static class FundingPeriodHelper
    {
        private static readonly Regex FundingPeriodCodeRegex = new Regex(@"^[A-Z]{2}-\d{4}$", RegexOptions.Compiled);

        /// <summary>
        /// Get the years from a funding period code (e.g. FY-1920).
        /// </summary>
        /// <param name="fundingPeriodCode">Funding period code (e.g. FY-1920).</param>
        /// <returns>A tuple, where the first value is the first (from) year, and the second is the second (to) year.</returns>
        public static (int yearFrom, int yearTo) GetYearsFromCode(string fundingPeriodCode)
        {
            if (string.IsNullOrEmpty(fundingPeriodCode))
            {
                throw new ArgumentNullException(nameof(fundingPeriodCode));
            }

            if (!FundingPeriodCodeRegex.IsMatch(fundingPeriodCode))
            {
                throw new FormatException(nameof(fundingPeriodCode));
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
        public static string GetYearTypeNameFromCode(string fundingPeriodCode)
        {
            var yearTypeCode = fundingPeriodCode?.Split('-')[0];

            switch (yearTypeCode)
            {
                case YearTypeCode.AcademicYear:
                case YearTypeCode.AcademyAcademicYear:
                case YearTypeCode.AcademyAndSchoolAcademicYear:
                    return YearTypeName.AcademicYear;
                case YearTypeCode.FinancialYear:
                    return YearTypeName.FinancialYear;
            }

            return null;
        }

        /// <summary>
        /// Gets the year type code (e.g. AY or FY).
        /// </summary>
        /// <param name="yearType">The year type name (e.g. academic year).</param>
        /// <returns>The year type code (e.g. AY or FY).</returns>
        public static string GetCodeFromYearType(string yearType)
        {
            switch (yearType)
            {
                case YearTypeName.AcademicYear:
                    return YearTypeCode.AcademicYear;
                case YearTypeName.FinancialYear:
                    return YearTypeCode.FinancialYear;
            }

            return null;
        }

        /// <summary>
        /// Gets the year type code (e.g. AY or FY).
        /// </summary>
        /// <param name="yearSettingName">The year setting name (e.g. AcademicYear).</param>
        /// <returns>The year type code (e.g. AY or FY).</returns>
        public static string GetYearTypeCodeFromName(string yearSettingName)
        {
            switch (yearSettingName)
            {
                case SettingName.AcademicYear:
                    return YearTypeCode.AcademicYear;
                case SettingName.AcademyAcademicYear:
                    return YearTypeCode.AcademyAcademicYear;
                case SettingName.AcademyAndSchoolAcademicYear:
                    return YearTypeCode.AcademyAndSchoolAcademicYear;
                case SettingName.FinancialYear:
                    return YearTypeCode.FinancialYear;
            }

            throw new Exception($"Year type code not found for {yearSettingName}.");
        }

        /// <summary>
        /// Get a funding period code from the years.
        /// </summary>
        /// <param name="yearFrom">The first year (e.g. 2019).</param>
        /// <param name="yearTo">The last year (e.g. 2020).</param>
        /// <param name="yearTypeCode">The year type code e.g. AY or FY.</param>
        /// <returns>A funding period code (e.g. AY-1920).</returns>
        public static string GetCodeFromYears(int yearFrom, int yearTo, string yearTypeCode)
        {
            return $"{yearTypeCode}-{yearFrom - 2000}{yearTo - 2000}";
        }

        /// <summary>
        /// Get the year type code from a funding period code (e.g. FY-1920).
        /// </summary>
        /// <param name="fundingPeriodCode">Funding period code (e.g. FY-1920).</param>
        /// <returns>The year type (e.g. 'FY').</returns>
        public static string GetYearTypeCodeFromFundingPeriodCode(string fundingPeriodCode)
        {
            return fundingPeriodCode.Split('-')[0];
        }

        /// <summary>
        /// Get the latest year from settings.
        /// </summary>
        /// <param name="settingValues">The setting values for the funding stream.</param>
        /// <param name="activeFundingPeriodCodes">The active funding period codes.</param>
        /// <returns>The years that make up the latest year (e.g. 2019 and 2020).</returns>
        public static IEnumerable<(int yearFrom, int yearTo)> GetLatestYears(ICollection<SettingValue> settingValues, IReadOnlyCollection<string> activeFundingPeriodCodes)
        {
            foreach (var fundingPeriodCode in GetLatestFundingPeriodCodes_FundingPeriodFormat(settingValues, activeFundingPeriodCodes))
            {
                yield return GetYearsFromCode(fundingPeriodCode);
            }
        }

        /// <summary>
        /// Get the latest funding period code list from settings.
        /// </summary>
        /// <param name="settingValues">The setting values for the funding stream.</param>
        /// <param name="activeFundingPeriodCodes">The active funding period codes.</param>
        /// <returns>The years that make up the latest year (e.g. AY-1920 and AY-2021).</returns>
        public static IEnumerable<string> GetLatestFundingPeriodCodes_FundingPeriodFormat(ICollection<SettingValue> settingValues, IReadOnlyCollection<string> activeFundingPeriodCodes)
        {
            var yearSettingName = GetYearSettingName(settingValues);
            var yearSettingCode = GetYearTypeCodeFromName(yearSettingName);

            var latestYearSet = GetLatestFundingPeriods_6NumberFormat(settingValues).Split(',');
            var fundingPeriodCodes = new List<string>();

            foreach (var fundingPeriodCode in latestYearSet)
            {
                var trimmedFundingPeriodCode = fundingPeriodCode.Trim();

                // 202122 or 201920 :- Only valid years to be returned
                if (trimmedFundingPeriodCode.Length == 6)
                {
                    var fundingPeriod = $"{yearSettingCode}-{trimmedFundingPeriodCode.Substring(2)}";

                    if (activeFundingPeriodCodes.Contains(fundingPeriod, StringComparer.CurrentCultureIgnoreCase))
                    {
                        fundingPeriodCodes.Add(fundingPeriod);
                    }
                }
            }

            return fundingPeriodCodes.OrderBy(fundingPeriod => fundingPeriod);
        }

        /// <summary>
        /// Get the year setting name (e.g. AcademicYear or FinancialYear).
        /// </summary>
        /// <param name="settingValues">The setting values for the funding stream.</param>
        /// <returns>The year setting name (e.g. AcademicYear or FinancialYear).</returns>
        public static string GetYearSettingName(ICollection<SettingValue> settingValues)
        {
            var matches = settingValues.Where(settingValue =>
                SettingName.FinancialYear.Equals(settingValue?.Setting?.SettingName, StringComparison.InvariantCultureIgnoreCase) ||
                SettingName.AcademicYear.Equals(settingValue?.Setting?.SettingName, StringComparison.InvariantCultureIgnoreCase) ||
                SettingName.AcademyAndSchoolAcademicYear.Equals(settingValue?.Setting?.SettingName, StringComparison.InvariantCultureIgnoreCase) ||
                SettingName.AcademyAcademicYear.Equals(settingValue?.Setting?.SettingName, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (matches.Count == 0)
            {
                throw new Exception($"No year type settings found - {settingValues.FirstOrDefault()?.FundingStream.FundingStreamCode}");
            }

            if (matches.Count > 1)
            {
                throw new Exception($"More then one year type settings found - {settingValues.FirstOrDefault()?.FundingStream.FundingStreamCode}");
            }

            return matches.First().Setting.SettingName;
        }

        /// <summary>
        /// Get the year setting code (e.g. AY or FY).
        /// </summary>
        /// <param name="settingValues">The setting values for the funding stream.</param>
        /// <returns>The year setting name (e.g. AcademicYear or FinancialYear).</returns>
        public static string GetYearSettingCode(ICollection<SettingValue> settingValues)
        {
            return GetYearTypeCodeFromName(GetYearSettingName(settingValues));
        }

        /// <summary>
        /// Get the latest funding periods (without year type) (e.g. 201920, 202021).
        /// </summary>
        /// <param name="settingValues">The setting values for the funding stream.</param>
        /// <returns>The latest funding period codes.</returns>
        public static string GetLatestFundingPeriods_6NumberFormat(ICollection<SettingValue> settingValues)
        {
            var settingName = GetYearSettingName(settingValues);
            var yearSetting = settingValues.SingleOrDefault(s
                => s.Setting.SettingName.Equals(settingName, StringComparison.OrdinalIgnoreCase));

            if (yearSetting == null)
            {
                throw new Exception($"Unable to find {settingName} setting.");
            }

            return yearSetting.Value;
        }

        /// <summary>
        /// Gets the cut-off date to use to retrieve funding for a given publication.
        /// Attempts to use the publication cut-off date if set, otherwise falls back to the publication date.
        /// Additionally, the return value is capped at the value of cut-off date property of this funding stream configuration.
        /// </summary>
        /// <param name="publication">The publication for which to get the cut-off date.</param>
        /// <returns>The cut-off date to use for the given publication.</returns>
        public static DateTime GetCutOffDateForPublication(Publication publication)
        {
            return publication?.CutOffDate ?? publication?.PublishedDate ?? DateTime.MinValue;
        }

        /// <summary>
        /// Gets the current and historic years.
        /// </summary>
        /// <param name="currentYearFrom">The current 'from' year.</param>
        /// <param name="currentYearTo">The current 'to' year.</param>
        /// <param name="yearsOfHistoricAllocationsToShow">The number of years of historic allocations to show.</param>
        /// <returns>The current and historic years.</returns>
        public static List<(int yearFrom, int yearTo)> GetCurrentAndHistoricYears(int currentYearFrom, int currentYearTo, int yearsOfHistoricAllocationsToShow)
        {
            var currentAndHistoricYears = new List<(int, int)>
            {
                (currentYearFrom, currentYearTo)
            };

            for (var numberOfYearsAgo = 1;
                numberOfYearsAgo <= yearsOfHistoricAllocationsToShow;
                numberOfYearsAgo++)
            {
                var historicYearFrom = currentYearFrom - numberOfYearsAgo;

                currentAndHistoricYears.Add((historicYearFrom, historicYearFrom + 1));
            }

            return currentAndHistoricYears;
        }

        /// <summary>
        /// Gets the active funding period codes.
        /// </summary>
        /// <param name="fundingStreamConfig">The funding stream configuration.</param>
        /// <param name="previewModeEnabled">if set to <c>true</c> [preview mode enabled].</param>
        /// <returns>The active funding periods.</returns>
        public static IReadOnlyCollection<string> GetActiveFundingPeriodCodes(FundingStream fundingStreamConfig, bool previewModeEnabled)
        {
            var activeFundingPeriodCodes = fundingStreamConfig.Publications
                .Where(pub => pub.Status == PublicationStatus.Published ||
                              (previewModeEnabled && pub.Status == PublicationStatus.Preview))
                .Select(pub => pub.FundingPeriodCode)
                .Distinct()
                .AsSafeReadOnlyList();

            return activeFundingPeriodCodes;
        }

        /// <summary>
        /// Get the years from a funding period code (e.g. FY-1920).
        /// </summary>
        /// <param name="fundingPeriodCode">Funding period code (e.g. FY-1920).</param>
        /// <returns>The year and type name (e.g. 2023/24 AcademicYear or 2023/24 FinancialYear).</returns>
        public static string GetYearAndTypeNameFromCode(string fundingPeriodCode)
        {
            if (string.IsNullOrEmpty(fundingPeriodCode))
            {
                throw new ArgumentNullException(nameof(fundingPeriodCode));
            }

            if (!FundingPeriodCodeRegex.IsMatch(fundingPeriodCode))
            {
                throw new FormatException(nameof(fundingPeriodCode));
            }

            var fundingPeriod = fundingPeriodCode[^4..];

            var fundingYearType = $"20{fundingPeriod[..2]}/{fundingPeriod.Substring(2, 2)} {GetYearTypeNameFromCode(fundingPeriodCode)}";

            return fundingYearType;
        }
    }
}