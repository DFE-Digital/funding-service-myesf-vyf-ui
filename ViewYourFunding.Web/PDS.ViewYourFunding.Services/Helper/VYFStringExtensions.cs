using System.Text.RegularExpressions;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// The string extensions class.
    /// </summary>
    public static class VyfStringExtensions
    {
        // Regular Expressions for processing the search text:
        private static readonly Regex SpacingCharacters = new Regex(@"[\s\u2212\u2013\u2014\u2010-]+", RegexOptions.Compiled);
        private static readonly Regex DisallowedCharacters = new Regex(@"[^\w]+", RegexOptions.Compiled);
        private static readonly Regex MultipleDashes = new Regex(@"_{2,}", RegexOptions.Compiled);

        /// <summary>
        /// Sanitise the name by removing unallowed characters, spaces etc...
        /// </summary>
        /// <param name="inputString">The unsanitised input string.</param>
        /// <param name="outputDelimiter">The word delimiter to use in the output.</param>
        /// <returns>A string in the format we require.</returns>
        public static string Sanitise(this string inputString, char outputDelimiter = '_')
        {
            const char workingOutputDelimiter = '_';
            var defaultOutput = string.Empty;

            if (string.IsNullOrEmpty(inputString))
            {
                return defaultOutput;
            }

            var sanitisedString = SpacingCharacters.Replace(inputString, "_");
            sanitisedString = DisallowedCharacters.Replace(sanitisedString, string.Empty);
            sanitisedString = MultipleDashes.Replace(sanitisedString, "_");
            sanitisedString = sanitisedString.Trim(new[] { '_' });

            if (sanitisedString == string.Empty)
            {
                return defaultOutput;
            }

            return sanitisedString.Replace(workingOutputDelimiter, outputDelimiter);
        }

        /// <summary>
        /// Removes all whitespace from a string.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>The input string with all whitespace removed.</returns>
        public static string RemoveWhitespace(this string inputString)
        {
            return Regex.Replace(inputString, @"\s+", string.Empty);
        }

        /// <summary>
        /// Replaces @ symbol in the inputString so that it's not treated as variable in renderer.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>The input string with @ replaced with [[ATSYMBOLVYF]].</returns>
        public static string ReplaceAtSymbolInName(this string inputString)
        {
            return inputString?.Replace("@", "[[ATSYMBOLVYF]]");
        }

        /// <summary>
        /// Replaces [[ATSYMBOLVYF]] symbol in the inputString.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>The input string with [[ATSYMBOLVYF]] replaced with @.</returns>
        public static string ReplaceVYFAtSymbolWithAt(this string inputString)
        {
            return inputString?.Replace("[[ATSYMBOLVYF]]", "@");
        }
    }
}