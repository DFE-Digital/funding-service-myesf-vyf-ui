using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit;
using System;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Binders
{
    /// <summary>
    /// The Data Value Editor Binder Provider.
    /// </summary>
    /// <seealso cref="IModelBinderProvider" />
    public class DataValueEditorBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc />
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(DataTypeBaseEdit))
            {
                return new BinderTypeModelBinder(typeof(DataValueEditorBinder));
            }

            return null;
        }
    }
}