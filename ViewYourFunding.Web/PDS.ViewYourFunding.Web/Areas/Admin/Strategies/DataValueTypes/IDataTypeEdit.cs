using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.DataValueTypes
{
    /// <summary>
    /// The data type edit interface.
    /// </summary>
    public interface IDataTypeEdit
    {
        /// <summary>
        /// Gets the Edit type model for the specified type data.
        /// </summary>
        /// <param name="typeDataId">The type data identifier.</param>
        /// <param name="currentValue">The current value.</param>
        /// <param name="description">The description.</param>
        /// <returns>The DataTypeBaseEdit.</returns>
        DataTypeBaseEdit Edit(int typeDataId, string currentValue, string description);

        /// <summary>
        /// Get the new value for the data type..
        /// </summary>
        /// <param name="dataTypeEdit">The data edit model.</param>
        /// <typeparam name="T">The type passed in.</typeparam>
        /// <returns>The value to be persisted in the backing store.</returns>
        string GetNewValue<T>(T dataTypeEdit)
            where T : DataTypeBaseEdit;

        /// <summary>
        /// Checks if it the passed setting edit type Applies to this instance.
        /// </summary>
        /// <param name="settingEditType">The setting edit type.</param>
        /// <returns>true if it applies to the setting edit type mode.</returns>
        bool AppliesTo(SettingEditType settingEditType);

        /// <summary>
        /// Gets the action mode.
        /// </summary>
        /// <value>
        /// The action mode.
        /// </value>
        SettingEditType SettingEditType { get; }
    }
}