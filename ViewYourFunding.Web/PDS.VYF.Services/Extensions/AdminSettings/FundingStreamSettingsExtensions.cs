namespace PDS.VYF.Services.Extensions.AdminSettings
{
    using PDS.ViewYourFunding.Services.Models;
    using PDS.VYF.Services.Extensions.Core;

    /// <summary>
    /// Extension methods for the FundingStreamSettings class.
    /// </summary>
    public static class FundingStreamSettingsExtensions
    {
        /// <summary>
        /// Delegate for parsing a string value to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to parse the value to.</typeparam>
        /// <param name="value">The string value to parse.</param>
        /// <param name="outValue">The parsed value.</param>
        /// <returns>True if the parsing was successful, otherwise false.</returns>
        public delegate bool TryParseHandler<T>(string value, out T outValue);

        /// <summary>
        /// Gets the value of a setting from the FundingStream object.
        /// </summary>
        /// <typeparam name="T">The type of the setting value.</typeparam>
        /// <param name="settings">The FundingStream object.</param>
        /// <param name="settingName">The name of the setting.</param>
        /// <param name="handler">The parsing handler for the setting value.</param>
        /// <returns>The value of the setting, or null if not found or parsing failed.</returns>
        public static T? GetSettingValue<T>(this FundingStream settings, string settingName, TryParseHandler<T> handler)
            where T : struct
        {
            return settings.SettingValues.FirstOrDefault(a => a.Setting.SettingName == settingName)?.Value?.TryParse(handler);
        }

        /// <summary>
        /// Gets the latest publication for the FundingStream object.
        /// </summary>
        /// <param name="fundingStream">The FundingStream object.</param>
        /// <param name="fundingPeriodCode">The funding period code (optional).</param>
        /// <returns>The latest publication, or null if not found.</returns>
        public static Publication? GetLatestPublication(this FundingStream fundingStream, string? fundingPeriodCode = null)
        {
            return fundingStream
                    .Publications
                    .Where(a => a.Status == ViewYourFunding.Services.Enums.PublicationStatus.Published
                                    && (fundingPeriodCode == null || a.FundingPeriodCode.EqualsIC(fundingPeriodCode)))
                    .OrderByDescending(a => a.PublishedDate)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Tries to parse a string value to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to parse the value to.</typeparam>
        /// <param name="value">The string value to parse.</param>
        /// <param name="handler">The parsing handler for the value.</param>
        /// <returns>The parsed value, or null if parsing failed.</returns>
        private static T? TryParse<T>(this string? value, TryParseHandler<T> handler)
            where T : struct
        {
            if (value != null && handler(value, out T outValue))
            {
                return outValue;
            }

            return null;
        }
    }
}
