using Microsoft.AspNetCore.Mvc.Filters;
using Pds.Core.Common.Identity.Constants;
using Pds.Core.Common.Identity.Enums;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Attributes
{
    /// <summary>
    /// Action filter to be used on the Preview Layout functionality only available to SFS Admin users.
    /// </summary>
    /// <seealso cref="ActionFilterAttribute" />
    public class PreviewLayoutActionAttribute : ActionFilterAttribute
    {
        private readonly IReadOnlyList<string> _sfsAdminRoles = new List<string>
        {
            nameof(UserRole.SfsAdmin)
        };

        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var queryCollection = httpContext.Request.Query;
            if (queryCollection.ContainsKey(nameof(PreviewLayoutModel.LayoutId)) &&
                queryCollection.ContainsKey("preview"))
            {
                var user = httpContext.User;
                if (user.Claims.Any(claim =>
                    claim.Type == PdsClaimTypes.Role && _sfsAdminRoles.Contains(claim.Value)))
                {
                    context.ActionArguments.TryGetValue(nameof(PreviewLayoutModel), out var previewLayoutModelData);
                    if (previewLayoutModelData != null)
                    {
                        context.ActionArguments.Remove(nameof(PreviewLayoutModel));
                        var previewLayoutModel = (PreviewLayoutModel)previewLayoutModelData;
                        previewLayoutModel.IsPreview = true;
                        context.ActionArguments.Add(nameof(PreviewLayoutModel), previewLayoutModel);
                    }
                }
            }
        }
    }
}