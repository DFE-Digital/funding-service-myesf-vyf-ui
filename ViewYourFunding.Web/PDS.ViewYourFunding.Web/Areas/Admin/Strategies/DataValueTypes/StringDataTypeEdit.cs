using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.DataValueTypes
{
    /// <summary>
    /// The string DataType Edit.
    /// </summary>
    /// <seealso cref="DataTypeEditBase" />
    /// <seealso cref="IDataTypeEdit" />
    public class StringDataTypeEdit : DataTypeEditBase, IDataTypeEdit
    {
        /// <inheritdoc/>
        public DataTypeBaseEdit Edit(int typeDataId, string currentValue, string description)
        {
            var model = GetDataTypeEditBase<StringTypeEdit>(typeDataId, currentValue, description, SettingEditType);
            model.NewValue = currentValue;

            return model;
        }

        /// <inheritdoc/>
        public string GetNewValue<T>(T dataTypeEdit)
        where T : DataTypeBaseEdit
        {
            var item = dataTypeEdit as StringTypeEdit;
            return item?.NewValue;
        }

        /// <inheritdoc/>
        public bool AppliesTo(SettingEditType settingEditType)
        {
            return settingEditType == SettingEditType;
        }

        /// <inheritdoc/>
        public SettingEditType SettingEditType => SettingEditType.String;
    }
}