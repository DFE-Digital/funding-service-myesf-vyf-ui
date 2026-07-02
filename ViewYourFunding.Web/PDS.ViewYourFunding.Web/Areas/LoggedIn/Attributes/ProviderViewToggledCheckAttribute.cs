using Microsoft.AspNetCore.Mvc.Filters;
using PDS.ViewYourFunding.Web.Exceptions;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Attributes
{
    /// <summary>
    /// Check that provider view is toggled on.
    /// </summary>
    public class ProviderViewToggledCheckAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether the provider view is toggled on.
        /// </summary>
        public static bool ProviderViewToggledOn { get; set; }

        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!ProviderViewToggledOn)
            {
                throw new RequestException("Provider view not toggled on");
            }
        }
    }
}