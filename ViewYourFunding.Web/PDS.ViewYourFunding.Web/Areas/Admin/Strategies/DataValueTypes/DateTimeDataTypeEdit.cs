using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit;
using System;
using System.Globalization;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.DataValueTypes
{
    /// <summary>
    /// The Date time DataType Edit.
    /// </summary>
    /// <seealso cref="DataTypeEditBase" />
    /// <seealso cref="IDataTypeEdit" />
    public class DateTimeDataTypeEdit : DataTypeEditBase, IDataTypeEdit
    {
        /// <inheritdoc/>
        public DataTypeBaseEdit Edit(int typeDataId, string currentValue, string description)
        {
            var model = GetDataTypeEditBase<DateTimeTypeEdit>(typeDataId, currentValue, description, SettingEditType);

            DateTime.TryParseExact(
                currentValue,
                EditTypeConstants.DateTimeFormat,
                EditTypeConstants.EnGbCultureInfo,
                DateTimeStyles.AdjustToUniversal,
                out var currentDate);

            model.Year = currentDate.Year.ToString();
            model.Month = currentDate.Month.ToString();
            model.Day = currentDate.Day.ToString();
            model.Hour = currentDate.Hour.ToString();
            model.Minute = currentDate.Minute.ToString();

            return model;
        }

        /// <inheritdoc/>
        public string GetNewValue<T>(T dataTypeEdit)
            where T : DataTypeBaseEdit
        {
            var item = dataTypeEdit as DateTimeTypeEdit;
            return item?.DateTime.ToString(EditTypeConstants.DateTimeFormat);
        }

        /// <inheritdoc/>
        public bool AppliesTo(SettingEditType settingEditType)
        {
            return settingEditType == SettingEditType;
        }

        /// <inheritdoc/>
        public SettingEditType SettingEditType => SettingEditType.DateTime;
    }
}