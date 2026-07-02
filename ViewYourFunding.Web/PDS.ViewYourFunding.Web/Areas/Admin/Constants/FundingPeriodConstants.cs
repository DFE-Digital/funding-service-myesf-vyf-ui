using System.Text.RegularExpressions;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Constants
{
    /// <summary>
    /// The Funding Period constants.
    /// </summary>
    public static class FundingPeriodConstants
    {
        /// <summary>
        /// The funding period code regex pattern.
        /// </summary>
        public const string FundingPeriodCodeRegexPattern = @"^[A-Za-z]{2}(-)[0-Z]{4}$";

        /// <summary>
        /// The funding period code error message.
        /// </summary>
        public const string FundingPeriodCodeErrorMessage = "Funding period code must consist of 2 letters and 4 digits seperated by a dash. e.g FY-2021";

        /// <summary>
        /// The funding period code regex.
        /// </summary>
        public static readonly Regex FundingPeriodCodeRegex = new Regex(FundingPeriodCodeRegexPattern, RegexOptions.Compiled);
    }
}