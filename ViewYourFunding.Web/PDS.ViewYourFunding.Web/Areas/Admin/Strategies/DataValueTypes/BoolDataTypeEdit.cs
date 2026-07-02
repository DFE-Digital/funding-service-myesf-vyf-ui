using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit;
using System;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.DataValueTypes
{
    /// <summary>
    /// The Bool Date DataType Edit.
    /// </summary>
    /// <seealso cref="DataTypeEditBase" />
    /// <seealso cref="IDataTypeEdit" />
    public class BoolDataTypeEdit : DataTypeEditBase, IDataTypeEdit
    {
        /// <inheritdoc/>
        public DataTypeBaseEdit Edit(int typeDataId, string currentValue, string description)
        {
            var model = GetDataTypeEditBase<BoolTypeEdit>(typeDataId, currentValue, description, SettingEditType);
            model.NewValue = Convert.ToBoolean(currentValue);

            return model;
        }

        /// <inheritdoc/>
        public string GetNewValue<T>(T dataTypeEdit)
            where T : DataTypeBaseEdit
        {
            var item = dataTypeEdit as BoolTypeEdit;
            return item?.NewValue.ToString().ToUpper();
        }

        /// <inheritdoc/>
        public bool AppliesTo(SettingEditType settingEditType)
        {
            return settingEditType == SettingEditType;
        }

        /// <inheritdoc/>
        public SettingEditType SettingEditType => SettingEditType.Bool;
    }
}