using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Controllers;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Enumerations;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Helpers;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.ViewModels;
using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Filters
{
    public class UserFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly RedirectOptions redirectOption;

        public UserFilterAttribute(RedirectOptions redirectOption = RedirectOptions.None)
        {
            this.redirectOption = redirectOption;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as LoggedInBaseController;

            controller.UserFromClaims = await UserHelpers.GetUserFromClaims(context.HttpContext);
            controller.CurrentUserUKPRN = controller.UserFromClaims.Ukprn?.ToString() ?? string.Empty;

            if (controller.UserFromClaims.Ukprn > 10000000 && controller.UserFromClaims.Ukprn < 99999999)
            {
                var parentApiClientServices = context.HttpContext.RequestServices.GetRequiredService<IParentApiClientServices>();
                var fundingStreamSettingsServices = context.HttpContext.RequestServices.GetRequiredService<IFundingStreamSettingsServices>();

                var fundingperiodcodes = await fundingStreamSettingsServices.GetEmailEnabledFundingStreamPeriod();

                controller.HasUserLoggedInAsParent = await parentApiClientServices.IsParent(controller.CurrentUserUKPRN, false, fundingperiodcodes);

                if (redirectOption == RedirectOptions.ToChildSummaryPage && !controller.HasUserLoggedInAsParent)
                {
                    context.ActionArguments.TryGetValue("viaChoicePage", out object viaChoicePage);
                    context.Result = controller.RedirectToRoute(LoggedInConstants.RouteName_ProviderStatement, new { viaChoicePage });
                }
                else if (redirectOption == RedirectOptions.ToParentSummaryPage && controller.HasUserLoggedInAsParent)
                {
                    context.ActionArguments.TryGetValue("viaChoicePage", out object viaChoicePage);
                    context.Result = controller.RedirectToRoute(LoggedInConstants.RouteName_MultipleAcademyTrustStatement, new { viaChoicePage });
                }
                else
                {
                    await next();
                }
            }
            else
            {
                var modelMetadataProvider = context.HttpContext.RequestServices.GetService<IModelMetadataProvider>();

                var errorModel = controller.UserFromClaims.GetBasePageViewModel<LoggedInErrorPageViewModel>(context.HttpContext, false);
                errorModel.ErrorTitle = "Unauthorised Access";

                context.Result = new ViewResult()
                {
                    ViewName = "../Error/UnauthorisedAccess",
                    ViewData = new ViewDataDictionary(modelMetadataProvider, new ModelStateDictionary())
                    {
                        Model = errorModel
                    },
                    StatusCode = (int)HttpStatusCode.NotFound,
                };
            }
        }
    }
}
