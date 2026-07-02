namespace PDS.ViewYourFunding.Repositories.Enums
{
    /// <summary>
    /// The setting edit Type.
    /// </summary>
    public enum SettingEditType
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
        Date = 5
    }
}