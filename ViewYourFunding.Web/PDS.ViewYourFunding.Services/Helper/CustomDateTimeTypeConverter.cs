using System;
using System.ComponentModel;
using System.Globalization;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// CustomDateTimeTypeConverter Class.
    /// </summary>
    public class CustomDateTimeTypeConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return DateTime.ParseExact(value.ToString(), "dd/MM/yyyy HH-mm-ss", culture);
        }
    }
}
