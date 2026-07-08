#define vyfv2

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Attributes;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Extensions;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Filters;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.Requests;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.ViewModels.Child;
using PDS.VYF.Services.Abstracts.AppServices;
using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Controllers
{
    [Area("LoggedIn")]
    [Authorize(nameof(UserRole.ViewAllocationStatements))]
    [ServiceFilter(typeof(ProviderViewToggledCheckAttribute))]
    [LoggedInExceptionFilterAttribute]
    public class ChildController : LoggedInBaseController
    {
        private readonly IChildFundingViewServices childFundingViewServices;
        private readonly IMapper mapper;

        public ChildController(
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            IChildFundingViewServices childFundingViewServices,
            IMapper mapper)
        : base(securityService, applicationConfigurationOptions)
        {
            this.childFundingViewServices = childFundingViewServices;
            this.mapper = mapper;
        }

#if vyfv2
        [Route(LoggedInConstants.Route_ProviderStatement, Name = LoggedInConstants.RouteName_ProviderStatement)]
        [UserFilter(Enumerations.RedirectOptions.ToParentSummaryPage)]
#endif
        public async Task<IActionResult> ChildSummaryPage(bool viaChoicePage = false)
        {
            var viewModel = GetViewModel<ChildSummaryPageViewModel>(viaChoicePage);

            var viewDataRequest = new ChildSummaryViewDataRequestModel(CurrentUserUKPRN, UserFromClaims.Principal, viaChoicePage);

            var viewDataResponse = await childFundingViewServices.GetChildSummaryViewData(viewDataRequest);

            viewModel.AddViewDataResponse(viewDataResponse);

            return BaseView(viewModel);
        }

#if vyfv2
        [Route(LoggedInConstants.Route_ProviderFundingBreakdown, Name = LoggedInConstants.RouteName_ProviderFundingBreakdown)]
        [UserFilter]
#endif
        public async Task<IActionResult> ChildDetailedPage(ProviderFundingBreakdownRequest providerFundingBreakdownRequest)
        {
            var viewModel = GetViewModel<ChildDetailedPageViewModel>(providerFundingBreakdownRequest.ViaChoicePage);

            var childDetailedViewDataRequestModel = this.mapper.Map<ProviderFundingBreakdownRequest, ChildDetailedViewDataRequestModel>(providerFundingBreakdownRequest);
            childDetailedViewDataRequestModel.UserId = UserFromClaims.Principal;
            childDetailedViewDataRequestModel.UkprnFromLoggedInUser = CurrentUserUKPRN;

            var viewDataResponse = await childFundingViewServices.GetChildDetailedViewData(childDetailedViewDataRequestModel);

            viewModel
                .AddViewDataResponse(viewDataResponse)
                .AddBreakDownRequestToModel(providerFundingBreakdownRequest);

            viewModel.FromMatStatementsPage = providerFundingBreakdownRequest.Ukprn != CurrentUserUKPRN;

            return BaseView(viewModel);
        }

#if vyfv2
        [Route(LoggedInConstants.Route_ProviderHistory, Name = LoggedInConstants.RouteName_ProviderHistory)]
        [UserFilter]
#endif
        public async Task<IActionResult> ChildHistoryPage(string ukprn, string fundingStreamNamePathPart, bool viaChoicePage = false)
        {
            var viewModel = GetViewModel<ChildHistoryPageViewModel>(viaChoicePage);

            var viewDataRequest = new ChildHistoryViewDataRequestModel
            {
                FundingStreamNamePathPart = fundingStreamNamePathPart,
                UkprnFromLoggedInUser = CurrentUserUKPRN,
                UkprnFromRoute = ukprn,
                ViaChoicePage = viaChoicePage,
                IsPreviewModeEnabled = await PreviewModeEnabled(),
            };

            var viewDataResponse = await childFundingViewServices.GetChildHistoryViewData(viewDataRequest);

            viewModel
                .AddViewDataResponse(viewDataResponse, ukprn, fundingStreamNamePathPart, viaChoicePage, HasUserLoggedInAsParent);

            return BaseView(viewModel);
        }
    }
}
