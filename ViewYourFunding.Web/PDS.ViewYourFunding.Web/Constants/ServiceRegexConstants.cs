using System.Text.RegularExpressions;

namespace PDS.ViewYourFunding.Web.Constants
{
    /// <summary>
    /// The service regular expressions constants.
    /// </summary>
    public static class ServiceRegexConstants
    {
        /// <summary>
        /// The json minify regex.
        /// </summary>
        public static readonly Regex JsonMinifyRegex = new Regex(@"\s(?=([^""]*""[^""]*"")*[^""]*$)", RegexOptions.Compiled);
    }
}