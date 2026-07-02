using Microsoft.AspNetCore.Mvc.Filters;
using PDS.ViewYourFunding.Web.Exceptions;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Attributes
{
    /// <summary>
    /// Filter to Check that mat view is toggled on and logged in user MAT status.
    /// </summary>
    public class MultipleAcademyTrustViewCheckAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether the provider view is toggled on.
        /// </summary>
        public static bool MultipleAcademyTrustViewToggledOn { get; set; }


        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!MultipleAcademyTrustViewToggledOn)
            {
                throw new RequestException("MAT view not toggled on");
            }
        }
    }
}