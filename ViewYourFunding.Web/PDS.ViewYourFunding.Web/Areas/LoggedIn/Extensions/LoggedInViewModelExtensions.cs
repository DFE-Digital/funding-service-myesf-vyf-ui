using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.Requests;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.ViewModels;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.ViewModels.Child;
using PDS.VYF.Services.Models.ResponseModels.ViewDataResponseModels;
using System;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Extensions
{
    public static class LoggedInViewModelExtensions
    {
        public static LoggedInBasePageViewModel AddLinks(
                                                this LoggedInBasePageViewModel viewModel,
                                                ApplicationConfiguration applicationConfiguration,
                                                bool viaChoicePage)
        {
            string choicePageLink;
            string homeLink = applicationConfiguration.LoggedInProviderHomeLink;

            var homeLinkIsAbsolute = homeLink.Contains("http", StringComparison.InvariantCultureIgnoreCase);

            if (homeLinkIsAbsolute)
            {
                var homeLinkUri = new Uri(homeLink, UriKind.Absolute);
                choicePageLink = new UriBuilder(homeLinkUri.Scheme, homeLinkUri.Host, homeLinkUri.Port, "choose-a-statement-type")
                    .ToString().Replace(":80/", "/").Replace(":443/", "/");
            }
            else
            {
                choicePageLink = "/choose-a-statement-type";
            }

            viewModel.ContactUsLink = applicationConfiguration.ContactUsLink;
            viewModel.FeedbackLink = applicationConfiguration.FeedbackLinkForLoggedInView;
            viewModel.HomeLink = homeLink;
            viewModel.ViaChoicePage = viaChoicePage;
            viewModel.ChoicePageLink = choicePageLink;

            return viewModel;
        }

        public static LoggedInBasePageViewModel AddNameAndUrn(this LoggedInBasePageViewModel viewModel, ViewDataResponseModelBase response)
        {
            viewModel.OrganisationName = response.OrganizationName;
            viewModel.ProviderUrn = response.OrganizationUrn;

            return viewModel;
        }

        public static bool AddValidationResult(this LoggedInBasePageViewModel viewModel, ViewDataResponseModelBase response)
        {
            viewModel.IsValidUrl = response.IsValidUrl;
            viewModel.HasUserHaveRightAccess = response.HasUserHaveRightAccess;
            viewModel.HasFundingDataExists = response.HasFundingDataExists;

            return response.IsValidUrl && response.HasUserHaveRightAccess && response.HasFundingDataExists;
        }

        public static ChildDetailedPageViewModel AddBreakDownRequestToModel(this ChildDetailedPageViewModel viewModel, ProviderFundingBreakdownRequest providerFundingBreakdownRequest)
        {
            viewModel.FundingStreamNamePathPart = providerFundingBreakdownRequest.FundingStreamNamePathPart;
            viewModel.OrganisationUkPrn = providerFundingBreakdownRequest.Ukprn;
            viewModel.YearFrom = providerFundingBreakdownRequest.YearFrom;
            viewModel.YearTo = providerFundingBreakdownRequest.YearTo;
            viewModel.PublishedDate = providerFundingBreakdownRequest.PublishedDate;
            viewModel.Tab = providerFundingBreakdownRequest.Tab;
            viewModel.ViaChoicePage = providerFundingBreakdownRequest.ViaChoicePage;
            viewModel.ViaVariancePage = providerFundingBreakdownRequest.ViaVariancePage;
            viewModel.IncludeHistory = providerFundingBreakdownRequest.IncludeHistory;

            return viewModel;
        }

        public static ChildSummaryPageViewModel AddViewDataResponse(this ChildSummaryPageViewModel viewModel, ChildSummaryViewDataResponseModel viewDataResponse)
        {
            viewModel.AddNameAndUrn(viewDataResponse);

            if (viewModel.AddValidationResult(viewDataResponse))
            {
                viewModel.ProviderFundingViewData = viewDataResponse.FundingViewData.Values.ToDictionary(a => a.FundingStreamCode + "-" + a.FundingPeriodCode, a => a);
            }

            return viewModel;
        }

        public static ChildDetailedPageViewModel AddViewDataResponse(this ChildDetailedPageViewModel viewModel, ChildDetailedViewDataResponseModel viewDataResponse)
        {
            viewModel.AddNameAndUrn(viewDataResponse);

            if (viewModel.AddValidationResult(viewDataResponse))
            {
                viewModel.FundingViewData = viewDataResponse.FundingViewData;
            }

            viewModel.FromMatStatementsPage = viewDataResponse.HasParentAccessChildUrl;
            viewModel.FundingStreamCode = viewDataResponse.FundingStreamCode;
            viewModel.FundingStreamName = viewDataResponse.FundingStreamName;
            viewModel.IsIndicative = viewDataResponse.IsIndicative;

            return viewModel;
        }

        public static ChildHistoryPageViewModel AddViewDataResponse(this ChildHistoryPageViewModel viewModel, ChildHistoryViewDataResponseModel viewDataResponse, string ukprn, string fundingStreamNamePathPart, bool viaChoicePage, bool isParent)
        {
            viewModel.AddNameAndUrn(viewDataResponse);

            viewModel.FundingStreamNamePathPart = fundingStreamNamePathPart;
            viewModel.OrganisationUkprn = ukprn;
            viewModel.ViaChoicePage = viaChoicePage;
            viewModel.FromMatStatementsPage = isParent;

            if (viewModel.AddValidationResult(viewDataResponse))
            {
                viewModel.FundingViewData = viewDataResponse.FundingViewData;
            }

            return viewModel;
        }
    }
}
