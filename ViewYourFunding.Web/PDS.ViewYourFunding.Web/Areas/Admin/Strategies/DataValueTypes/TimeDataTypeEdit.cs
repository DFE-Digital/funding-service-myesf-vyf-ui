using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.DataValueTypes
{
    /// <summary>
    /// The Time DataType Edit.
    /// </summary>
    /// <seealso cref="DataTypeEditBase" />
    /// <seealso cref="IDataTypeEdit" />
    public class TimeDataTypeEdit : DataTypeEditBase, IDataTypeEdit
    {
        /// <inheritdoc/>
        public DataTypeBaseEdit Edit(int typeDataId, string currentValue, string description)
        {
            var model = GetDataTypeEditBase<TimeTypeEdit>(typeDataId, currentValue, description, SettingEditType);
            var timeParts = currentValue.Split(":");
            if (timeParts.Length == 2)
            {
                model.Hour = timeParts[0];
                model.Minute = timeParts[1];
            }

            return model;
        }

        /// <inheritdoc/>
        public string GetNewValue<T>(T dataTypeEdit)
            where T : DataTypeBaseEdit
        {
            var item = dataTypeEdit as TimeTypeEdit;
            return item?.TimeValue.ToString(EditTypeConstants.TimeFormat);
        }

        /// <inheritdoc/>
        public bool AppliesTo(SettingEditType settingEditType)
        {
            return settingEditType == SettingEditType;
        }

        /// <inheritdoc/>
        public SettingEditType SettingEditType => SettingEditType.Time;
    }
}