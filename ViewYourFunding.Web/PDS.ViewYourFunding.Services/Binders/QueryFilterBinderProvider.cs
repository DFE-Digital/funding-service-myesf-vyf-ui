using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using PDS.ViewYourFunding.Services.Models;
using System;

namespace PDS.ViewYourFunding.Services.Binders
{
    /// <summary>
    /// The Query Filter Binder Provider.
    /// </summary>
    /// <seealso cref="IModelBinderProvider" />
    public class QueryFilterBinderProvider : IModelBinderProvider
    {
        /// <summary>
        /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder" /> based on <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderProviderContext" />.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderProviderContext" />.</param>
        /// <returns>
        /// An <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">context.</exception>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(QueryFilter))
            {
                return new BinderTypeModelBinder(typeof(QueryFilterBinder));
            }

            return null;
        }
    }
}