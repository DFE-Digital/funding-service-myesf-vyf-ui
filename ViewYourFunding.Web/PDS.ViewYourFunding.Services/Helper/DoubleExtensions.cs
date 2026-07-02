using System;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Extension class for double related modifications.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Extension method to convert a double into a GB currency formatted string.
        /// </summary>
        /// <param name="doubleValue">The double value.</param>
        /// <returns>The GB currency string value.</returns>
        public static string ToGBCurrency(this double doubleValue)
        {
            IFormatProvider formatProvider = System.Globalization.CultureInfo.GetCultureInfo("en-GB");
            return doubleValue.ToString("C", formatProvider);
        }

        /// <summary>
        /// Extension method to convert a double into a GB currency formatted string without a decimal place.
        /// </summary>
        /// <param name="doubleValue">The double value.</param>
        /// <returns>A double as a GB currency formatted string without decimal places.</returns>
        public static string ToGBCurrencyWithoutDecimalPlace(this double doubleValue)
        {
            var formatProvider = System.Globalization.CultureInfo.GetCultureInfo("en-GB");
            return doubleValue.ToString("C0", formatProvider);
        }

        /// <summary>
        /// Extension method to convert a decimal into a GB currency formatted string, that doesn't have trailing zeroes.
        /// E.g. 10.00 will return £10 and 123456.99 will return £123,456.99.
        /// </summary>
        /// <param name="doubleValue">The decimal value.</param>
        /// <returns>A decimal as a GB currency formatted string without trailing zeroes.</returns>
        public static string ToGBCurrencyWithoutTrailingZeroes(this double doubleValue)
        {
            var convertedToCurrency = doubleValue.ToGBCurrency();
            return convertedToCurrency.EndsWith(".00") ? convertedToCurrency.Split('.')[0] : convertedToCurrency;
        }

        /// <summary>
        /// Extension method to convert a double into a 6 DP weighting formatted string, that doesn't have trailing zeroes.
        /// E.g. 10.00 will return 10 and 1.2343211 will return 1.234321.
        /// </summary>
        /// <param name="doubleValue">The double value.</param>
        /// <returns>A double as a 6DP weighting formatted string without trailing zeroes.</returns>
        public static string ToWeightingWithoutTrailingZeroes(this double doubleValue)
        {
            var weightingValue = doubleValue.ToString("0.000000");
            return weightingValue.Replace(".000000", string.Empty);
        }

        /// <summary>
        /// Extension method to convert a number, add thousands seperators, but have no trailing zeroes (e.g. 1345.78 becomes 1,345.78 and 1444 becomes 1,444).
        /// </summary>
        /// <param name="doubleValue">The decimal value.</param>
        /// <returns>A number, with thousands seperators, but no trailing zeroes (e.g. 1345.78 becomes 1,345.78 and 1444 becomes 1,444).</returns>
        public static string ToThousandsSeperatedNoTrailingZeroes(this double doubleValue)
        {
            var convertedToN = doubleValue.ToString("N2");
            return convertedToN.EndsWith(".00") ? convertedToN.Split('.')[0] : convertedToN;
        }

        /// <summary>
        /// Extension method to convert a number, add thousands seperators, with 2 dp (e.g. 1345.78 becomes 1,345.78 and 1444.00 becomes 1,444).
        /// </summary>
        /// <param name="doubleValue">The decimal value.</param>
        /// <returns>A number, with thousands seperators, but no trailing zeroes (e.g. 1345.78 becomes 1,345.78 and 1444.00 becomes 1,444).</returns>
        public static string ToThousandsSeperated2DP(this double doubleValue)
        {
            var convertedToN = doubleValue.ToString("N2");
            return convertedToN;
        }

        /// <summary>
        /// Extension method to convert a decimal into a 2 DP formatted string, that doesn't have trailing zeroes.
        /// e.g 12.00 will return 12, 123456.01 will return 123,456.01.
        /// </summary>
        /// <param name="doubleValue">The decimal value.</param>
        /// <returns>A decimal as a 2 DP formatted string without trailing zeroes.</returns>
        public static string To2DPWithoutTrailingZeroes(this double doubleValue)
        {
            var convertedToCurrency = doubleValue.ToGBCurrencyWithoutTrailingZeroes();
            return convertedToCurrency.Replace("£", string.Empty);
        }

        /// <summary>
        /// Extension method to convert a decimal into a 2 DP formatted string, that will have trailing zeroes.
        /// e.g 12.00 will return 12.00, 123456.01 will return 123,456.01.
        /// </summary>
        /// <param name="doubleValue">The decimal value.</param>
        /// <returns>A decimal as a 2 DP formatted string with trailing zeroes.</returns>
        public static string To2DPWithTrailingZeroes(this double doubleValue)
        {
            return string.Format("{0:#,0.00}", doubleValue);
        }

        /// <summary>
        /// Extension method to convert a decimal into a 1 DP formatted string, that doesn't have trailing zeroes.
        /// e.g 12.00 will return 12, 123456.01 will return 123,456.0.
        /// </summary>
        /// <param name="doubleValue">The decimal value.</param>
        /// <returns>A decimal as a 1 DP formatted string without trailing zeroes.</returns>
        public static string To1DPWithoutTrailingZeroes(this double doubleValue)
        {
            return doubleValue.ToString("N1");
        }

        /// <summary>
        /// Extension method to convert a decimal into a 2 DP formatted percentage string, that doesn't have trailing zeroes.
        /// </summary>
        /// <param name="doubleValue">The decimal value.</param>
        /// <returns>A decimal as a 2 DP formatted percentage string.</returns>
        public static string ToPercentageWith2DecimalPlaces(this double doubleValue)
        {
            return $"{doubleValue.To2DPWithTrailingZeroes()}%";
        }

        /// <summary>
        /// Extension method to convert a decimal into a 1 DP formatted percentage string, that doesn't have trailing zeroes.
        /// </summary>
        /// <param name="doubleValue">The decimal value.</param>
        /// <returns>A decimal as a 1 DP formatted percentage string.</returns>
        public static string ToPercentageWith1DecimalPlace(this double doubleValue)
        {
            return $"{doubleValue.To1DPWithoutTrailingZeroes()}%";
        }

        /// <summary>
        /// Extension method to convert a decimal into a formatted percentage string displaying only the significant digits.
        /// </summary>
        /// <param name="doubleValue">The decimal value.</param>
        /// <returns>A decimal as a formatted percentage string with only the significant digits.</returns>
        public static string ToPercentageWithSignificantDigits(this double doubleValue)
        {
            return $"{doubleValue:0.##}%";
        }

        /// <summary>
        /// Extension method to convert a double into a GB currency formatted string without a decimal place and without the pound symbol.
        ///  /// e.g 22.5 will return 22, 123455.56 will return 123,456.
        /// </summary>
        /// <param name="doubleValue">The double value.</param>
        /// <returns>A double as a GB currency formatted string without decimal places and without the pound symbol.</returns>
        public static string ToThousandsSeparatedNoDP(this double doubleValue)
        {
            var convertedToCurrency = doubleValue.ToGBCurrencyWithoutDecimalPlace();
            return convertedToCurrency.Replace("£", string.Empty);
        }

        /// <summary>
        /// Extension method to convert a double formatted string with the specified number of decimal places.
        ///  /// e.g 22.500289 with 3 places will return 22.500, 0.51236  with 1  decimal placewill return 0.51.
        /// </summary>
        /// <param name="doubleValue">The double value.</param>
        /// <param name="decimalPlaces">The number of decimal places needed.</param>
        /// <returns>A double as a formatted string with specified decimal places and commas if applicable.</returns>
        public static string ToSpecifiedDecimalPlaces(this double doubleValue, int decimalPlaces)
        {
            return doubleValue.ToString($"N{decimalPlaces}");
        }

        /// <summary>
        /// Extension method to convert a double formatted string with zero decimal places.
        ///  /// e.g 22.500289s will return 22.
        /// </summary>
        /// <param name="doubleValue">The double value.</param>
        /// <returns>A double as a formatted string with zero decimal places.</returns>
        public static string ZeroDecimalPlaces(this double doubleValue)
        {
            return doubleValue.ToString("F0");
        }

        /// <summary>
        /// Extension method to convert a double formatted string with 6 decimal places.
        ///  /// e.g 22.500289s will return 22.
        /// </summary>
        /// <param name="doubleValue">The double value.</param>
        /// <returns>A double as a formatted string with 6 decimal places.</returns>
        public static string To6DPWithoutTrailingZeroes(this double doubleValue)
        {
            return doubleValue.ToString("0.######");
        }
    }
}