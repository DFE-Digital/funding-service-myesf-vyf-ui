using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Helper methods for funding streams.
    /// </summary>
    public static class FundingStreamHelper
    {
        private static readonly IDictionary<string, string> PreferredDownloadDocumentFormat = new Dictionary<string, string>
        {
            { "PSG", "ods" },
            { "GAG", "csv" },
            { "DSG", "ods" },
        };

        /// <summary>
        /// Gets the current funding period code.
        /// </summary>
        /// <param name="fundingStream">A funding stream.</param>
        /// <param name="previewModeEnabled">Is preview mode enabled.</param>
        /// <returns>The current funding period code.</returns>
        public static string CurrentFundingPeriodCode_FromPublications(this FundingStream fundingStream, bool previewModeEnabled)
        {
            return fundingStream.Publications?
                .Where(publication => publication.Status == PublicationStatus.Published
                    || (previewModeEnabled && publication.Status == PublicationStatus.Preview))
                .OrderByDescending(publication => publication.PublishedDate)
                .FirstOrDefault()?
                .FundingPeriodCode;
        }

        /// <summary>
        /// Uses the fake API service.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <returns>True if the funding stream should use a fake api service.</returns>
        public static bool UseFakeApiService(this FundingStream fundingStream)
        {
            var settingValue =
                fundingStream.SettingValues.FirstOrDefault(sv => sv.Setting?.SettingName == SettingName.UseStaticData);

            if (settingValue == null)
            {
                return false;
            }

            bool.TryParse(settingValue.Value, out var useStaticData);
            return useStaticData;
        }

        /// <summary>
        /// Uses the latest funding data.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <returns>True if we need only latest funding data for this funding stream.</returns>
        public static bool UseLatestFundingData(this FundingStream fundingStream)
        {
            var settingValue =
                fundingStream.SettingValues.FirstOrDefault(sv => sv.Setting?.SettingName == SettingName.UseLatestFundingData);

            if (settingValue == null)
            {
                return false;
            }

            bool.TryParse(settingValue.Value, out var useLatestFundingData);
            return useLatestFundingData;
        }

        /// <summary>
        /// Gets the parent group type filter setting.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <returns>Returns the parent group type filter setting.</returns>
        public static string ParentGroupTypeFilterSetting(this FundingStream fundingStream)
        {
            return fundingStream
                 .SettingValues
                 .FirstOrDefault(sv => sv.Setting?.SettingName == SettingName.ParentGroupTypeFilter)
                 ?.Value;
        }

        /// <summary>
        /// Gets the grouping reason filter setting.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <returns>Returns the grouping reason filter setting.</returns>
        public static string GroupingReasonFilterSetting(this FundingStream fundingStream)
        {
            return fundingStream
                 .SettingValues
                 .FirstOrDefault(sv => sv.Setting?.SettingName == SettingName.GroupingReasonFilter)
                 ?.Value;
        }


        /// <summary>
        /// Get the funding stream code, or the full name (if its not appropriate).
        /// </summary>
        /// <param name="name">The funding stream name (e.g. Dedicated schools grant).</param>
        /// <param name="code">The funding stream code (e.g. DSG).</param>
        /// <param name="canUseShortCode">Whether the short code is publically known (i.e. with DSG).</param>
        /// <returns>A short name or the full name.</returns>
        public static string GetFundingStreamCodeHtml(string name, string code, bool canUseShortCode)
        {
            return !canUseShortCode ? name : $"<abbr title=\"{name}\">{code}</abbr>";
        }

        /// <summary>
        /// Get the funding stream name (either with or without brackets on the end).
        /// </summary>
        /// <param name="name">The funding stream name (e.g. Dedicated schools grant).</param>
        /// <param name="code">The funding stream code (e.g. DSG).</param>
        /// <param name="canUseShortCode">Whether the short code is publically known (i.e. with DSG).</param>
        /// <returns>The full name with or without brackets.</returns>
        public static string GetFundingStreamNameHtml(string name, string code, bool canUseShortCode)
        {
            if (!canUseShortCode)
            {
                return name;
            }

            return $"{name} ({GetFundingStreamCodeHtml(name, code, canUseShortCode)})";
        }

        /// <summary>
        /// Funding stream name to path component (swap spaces for hyphens and lowercase).
        /// </summary>
        /// <param name="fundingStreamName">The funding stream name (e.g. PE and sport premium).</param>
        /// <returns>A path like 'pe-and-sports-premium'.</returns>
        public static string ToUIPathComponent(this string fundingStreamName)
        {
            return fundingStreamName?.Replace(" ", "-").ToLower();
        }

        /// <summary>
        /// Gets the preferred document format used for downloading publications from the web UI.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <returns>The preferred document format.</returns>
        public static string GetPreferredDocumentFormat(this FundingStream fundingStream)
        {
            var fundingStreamCode = fundingStream?.FundingStreamCode?.ToUpperInvariant();

            if (!string.IsNullOrEmpty(fundingStreamCode) && PreferredDownloadDocumentFormat.ContainsKey(fundingStreamCode))
            {
                return PreferredDownloadDocumentFormat[fundingStreamCode];
            }

            // default to ods.
            return "ods";
        }

        /// <summary>
        /// Uses the Auto Pull functionality.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <returns>True if we use the auto pull functionality for the funding stream.</returns>
        public static bool UseAutoPull(this FundingStream fundingStream)
        {
            var settingValue = fundingStream.SettingValues.FirstOrDefault(sv => sv.Setting?.SettingName == SettingName.UseAutoPull);

            if (settingValue == null)
            {
                return false;
            }

            bool.TryParse(settingValue.Value, out var useAutoPull);
            return useAutoPull;
        }
    }
}