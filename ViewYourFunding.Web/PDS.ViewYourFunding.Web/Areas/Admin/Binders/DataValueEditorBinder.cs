using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.DataValueTypes;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Binders
{
    /// <summary>
    /// The Query Filter Model binder.
    /// </summary>
    /// <seealso cref="IModelBinder" />
    public class DataValueEditorBinder : IModelBinder
    {
        private const string PostAction = WebRequestMethods.Http.Post;

        /// <inheritdoc/>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var request = bindingContext.HttpContext.Request;

            if (!request.Method.Equals(PostAction, StringComparison.InvariantCultureIgnoreCase))
            {
                return Task.CompletedTask;
            }

            var requestValues = request.Form;

            var dataTypeBaseEdit = GetDataTypeBaseEdit(requestValues);

            foreach (var (key, value) in requestValues.Where(x => !x.Key.Equals(nameof(SettingEditType))))
            {
                var property = dataTypeBaseEdit.GetType().GetProperties().
                    FirstOrDefault(
                        p => p.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));

                if (property != null)
                {
                    if (property.PropertyType == typeof(int))
                    {
                        int.TryParse(value.First(), out int iValue);
                        var val = Convert.ChangeType(iValue, property.PropertyType);
                        property.SetValue(dataTypeBaseEdit, val);
                    }
                    else
                    {
                        var val = Convert.ChangeType(value.First(), property.PropertyType);
                        property.SetValue(dataTypeBaseEdit, val);
                    }
                }
            }

            var modelMetadata = bindingContext.ModelMetadata.GetMetadataForType(dataTypeBaseEdit.GetType());
            bindingContext.Result = ModelBindingResult.Success(dataTypeBaseEdit);
            bindingContext.ValidationState[bindingContext.Result] = new ValidationStateEntry
            {
                Metadata = modelMetadata,
            };
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the data type base edit.
        /// </summary>
        /// <param name="requestValues">The request values.</param>
        /// <returns>The DataTypeBase edit.</returns>
        private static DataTypeBaseEdit GetDataTypeBaseEdit(IFormCollection requestValues)
        {
            var settingEditTypeValue = requestValues.First(x => x.Key.Equals(nameof(SettingEditType)));
            var editValue = Enum.Parse<SettingEditType>(settingEditTypeValue.Value.First());

            var currentValue = requestValues.First(x => x.Key.Equals(nameof(DataTypeBaseEdit.CurrentValue)));
            var currentValueString = currentValue.Value.First();

            var description = requestValues.First(x => x.Key.Equals(nameof(DataTypeBaseEdit.Description)));
            var descriptionValue = description.Value.First();

            var dataTypeId = requestValues.First(x => x.Key.Equals(nameof(DataTypeBaseEdit.DataTypeId)));
            var dataTypeIdValue = Convert.ToInt32(dataTypeId.Value.First());

            var dataTypeBaseEdit = DataTypeEditStrategyFactory.GetDataTypeEditStrategy().DataTypeEdits
                .First(x => x.AppliesTo(editValue)).Edit(dataTypeIdValue, currentValueString, descriptionValue);

            return dataTypeBaseEdit;
        }
    }
}