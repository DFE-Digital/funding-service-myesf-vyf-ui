using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.DataValueTypes
{
    /// <summary>
    /// The data type edit action base class.
    /// </summary>
    public abstract class DataTypeEditBase
    {
        /// <summary>
        /// Gets the data type edit base.
        /// </summary>
        /// <typeparam name="T">The type passed in.</typeparam>
        /// <param name="dataTypeId">The data type identifier.</param>
        /// <param name="currentValue">The current value.</param>
        /// <param name="description">The item description.</param>
        /// <param name="settingEditType">The setting edit type.</param>
        /// <returns>The DataTypeBaseEdit type.</returns>
        protected T GetDataTypeEditBase<T>(int dataTypeId, string currentValue, string description, SettingEditType settingEditType)
        where T : DataTypeBaseEdit, new()
        {
            return new T
            {
                DataTypeId = dataTypeId,
                CurrentValue = currentValue,
                EditTemplateName = typeof(T).Name,
                Description = description,
                SettingEditType = settingEditType
            };
        }
    }
}