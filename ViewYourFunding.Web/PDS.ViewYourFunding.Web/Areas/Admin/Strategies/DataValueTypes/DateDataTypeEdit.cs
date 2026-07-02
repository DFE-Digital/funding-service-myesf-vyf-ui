using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit;
using System;
using System.Globalization;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.DataValueTypes
{
    /// <summary>
    /// The Date DataType Edit.
    /// </summary>
    /// <seealso cref="DataTypeEditBase" />
    /// <seealso cref="IDataTypeEdit" />
    public class DateDataTypeEdit : DataTypeEditBase, IDataTypeEdit
    {
        /// <inheritdoc/>
        public DataTypeBaseEdit Edit(int typeDataId, string currentValue, string description)
        {
            var model = GetDataTypeEditBase<DateTypeEdit>(typeDataId, currentValue, description, SettingEditType);

            DateTime.TryParseExact(
                currentValue,
                EditTypeConstants.DateFormat,
                EditTypeConstants.EnGbCultureInfo,
                DateTimeStyles.AdjustToUniversal,
                out var currentDate);

            model.Year = currentDate.Year.ToString();
            model.Month = currentDate.Month.ToString();
            model.Day = currentDate.Day.ToString();

            return model;
        }

        /// <inheritdoc/>
        public string GetNewValue<T>(T dataTypeEdit)
            where T : DataTypeBaseEdit
        {
            var item = dataTypeEdit as DateTypeEdit;
            return item?.Date.ToString(EditTypeConstants.DateFormat);
        }

        /// <inheritdoc/>
        public bool AppliesTo(SettingEditType settingEditType)
        {
            return settingEditType == SettingEditType;
        }

        /// <inheritdoc/>
        public SettingEditType SettingEditType => SettingEditType.Date;
    }
}