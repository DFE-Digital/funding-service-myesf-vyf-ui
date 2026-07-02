namespace PDS.VYF.Services.Extensions.Core
{
    /// <summary>
    /// The string extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// The Delegate used to parse string to any type.
        /// </summary>
        /// <typeparam name="T">Any Type.</typeparam>
        /// <param name="str">The string.</param>
        /// <param name="value">The value.</param>
        /// <returns>Bool.</returns>
        public delegate bool TryParseDelegate<T>(string str, out T value);

        /// <summary>
        /// Equalses the ic.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>True if it matches.</returns>
        public static bool EqualsIC(this string a, string b) => a.Equals(b, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Parses the specified try parse.
        /// </summary>
        /// <typeparam name="T">Any Type.</typeparam>
        /// <param name="str">The string.</param>
        /// <param name="tryParse">The try parse.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// Parsed value fom string.
        /// </returns>
        public static T? Parse<T>(this string str, TryParseDelegate<T> tryParse, T defaultValue)
        {
            if (tryParse(str, out T value))
            {
                return value;
            }

            return defaultValue;
        }
    }
}
