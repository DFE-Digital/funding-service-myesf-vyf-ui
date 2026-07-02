using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using System;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Binders
{
    /// <summary>
    /// The Data Value Editor Binder Provider.
    /// </summary>
    /// <seealso cref="IModelBinderProvider" />
    public class FilterTypeBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc/>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(LayoutFilter))
            {
                return new BinderTypeModelBinder(typeof(FilterTypeBinder));
            }

            return null;
        }
    }
}
