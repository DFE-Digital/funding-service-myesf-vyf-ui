using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pds.Core.Identity.Claims.Interfaces;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Controllers;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Extensions;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Helpers;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.ViewModels;
using PDS.VYF.Services.Abstracts.AppServices;
using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
using PDS.VYF.Services.Implementations.InfraServices.DataApiClientServices;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Filters
{
    public class LoggedInExceptionFilterAttribute : Attribute, IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var modelMetadataProvider = context.HttpContext.RequestServices.GetService<IModelMetadataProvider>();

            var userFromClaims = await UserHelpers.GetUserFromClaims(context.HttpContext);
            var errorModel = userFromClaims.GetBasePageViewModel<LoggedInErrorPageViewModel>(context.HttpContext, false);
            errorModel.ErrorTitle = "Internal Server Error";

            context.Result = new ViewResult()
            {
                ViewName = "../Error/UnhandledExceptions",
                ViewData = new ViewDataDictionary(modelMetadataProvider, new ModelStateDictionary())
                {
                    Model = errorModel
                },
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }
    }
}
