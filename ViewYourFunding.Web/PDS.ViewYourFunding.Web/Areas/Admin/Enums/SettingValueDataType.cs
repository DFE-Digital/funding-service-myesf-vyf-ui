namespace PDS.ViewYourFunding.Web.Areas.Admin.Enums
{
    /// <summary>
    /// Enumeration of the data types that setting values can have.
    /// </summary>
    public enum SettingValueDataType
    {
        /// <summary>
        /// A setting with the data type of string.
        /// </summary>
        String = 0,

        /// <summary>
        /// A setting with the data type of integer.
        /// </summary>
        Int = 1,

        /// <summary>
        /// A setting with the data type of date time.
        /// </summary>
        DateTime = 2,

        /// <summary>
        /// A setting with the data type of boolean.
        /// </summary>
        Bool = 3,

        /// <summary>
        /// A setting that is displayed as a time.
        /// </summary>
        Time = 4,

        /// <summary>
        /// A setting that is displayed as a date.
        /// </summary>
        Date = 5,

        /// <summary>
        /// A setting that is displayed as a national layout.
        /// </summary>
        NationalLayout = 6,

        /// <summary>
        /// A setting that is displayed as a national spreadsheet layout.
        /// </summary>
        NationalSpreadsheetLayout = 7
    }
}