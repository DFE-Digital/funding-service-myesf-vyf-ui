#define vyfv2

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Web.Components.Areas.Lists.DTOs;
using Pds.Core.Web.Components.Areas.Lists.Models;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Attributes;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Extensions;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Filters;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.ViewModels.Parent;
using PDS.VYF.Services.Abstracts.AppServices;
using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Controllers
{
    [Area("LoggedIn")]
    [Authorize(nameof(UserRole.ViewAllocationStatements))]
    [ServiceFilter(typeof(MultipleAcademyTrustViewCheckAttribute))]
    [LoggedInExceptionFilter]
    public class ParentController : LoggedInBaseController
    {
        private const string PARENT_PROVIDER_TYPE_DEFAULT = "DEFAULT";
        private const string AcademyFilterKey = "academy";
        private const string FundingTypeFilterKey = "fundingType";
        private const string LocalAuthorityFilterKey = "localAuthority";

        private static readonly IDictionary<string, Func<IEnumerable<MatStatementListItem>, string[], IEnumerable<MatStatementListItem>>> FilterActions
            = new Dictionary<string, Func<IEnumerable<MatStatementListItem>, string[], IEnumerable<MatStatementListItem>>>
            {
                { AcademyFilterKey, ApplyAcademyFilter },
                { FundingTypeFilterKey, ApplyFundingTypesFilter },
                { LocalAuthorityFilterKey, ApplyLocalAuthorityFilter }
            };

        private readonly IClaimsBasedIdentityService securityService;
        private readonly IOptions<ApplicationConfiguration> applicationConfigurationOptions;
        private readonly IParentFundingViewServices parentFundingViewServices;
        private readonly IMapper mapper;
        private readonly IParentApiClientServices parentApiClientServices;

        public ParentController(
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            IParentFundingViewServices parentFundingViewServices,
            IMapper mapper,
            IParentApiClientServices parentApiClientServices)
            : base(securityService, applicationConfigurationOptions)
        {
            this.securityService = securityService;
            this.applicationConfigurationOptions = applicationConfigurationOptions;
            this.parentFundingViewServices = parentFundingViewServices;
            this.mapper = mapper;
            this.parentApiClientServices = parentApiClientServices;
        }

#if vyfv2
        [Route(LoggedInConstants.Route_MultipleAcademyTrustStatement, Name = LoggedInConstants.RouteName_MultipleAcademyTrustStatement)]
        [UserFilter(Enumerations.RedirectOptions.ToChildSummaryPage)]
#endif
        public async Task<IActionResult> ParentSummaryPage(ListRequest request, bool viaChoicePage = false)
        {
            var viewDataResponse = await this.parentFundingViewServices.GetParentSummaryViewData(new ParentSummaryViewDataRequestModel() { UkprnFromLoggedInUser = CurrentUserUKPRN, ViaChoicePage = viaChoicePage });

            var viewModel = GetViewModel<ParentSummaryPageViewModel>(viaChoicePage);

            viewModel.BackToTopLinkMinimumCount = applicationConfigurationOptions.Value.BackToTopLinkMinimumCount;

            viewModel.FundingViewData = viewDataResponse.FundingViewData;
            viewModel.AddValidationResult(viewDataResponse);

            if (!viewDataResponse.HasFundingDataExists)
            {
                return BaseView(viewModel);
            }

            var fundingStreamsDic = viewDataResponse.RelavantFundingStreams.DistinctBy(a => a.FundingStreamCode).ToDictionary(a => a.FundingStreamCode, a => a);
            var parentProviderTypes = GetProviderParentGroupingType(fundingStreamsDic);

            var localAuths = viewDataResponse.ProviderFundingData
                .Where(provider => !string.IsNullOrEmpty(provider.ParentName))
                .Where(provider =>
                {
                    var providerFundingStreamCode = provider.FundingStreamCode;
                    var parentProviderType = parentProviderTypes?.ContainsKey(providerFundingStreamCode) == true
                        ? parentProviderTypes[providerFundingStreamCode] : null;

                    if (parentProviderType == null)
                    {
                        parentProviderType = parentProviderTypes?.ContainsKey(PARENT_PROVIDER_TYPE_DEFAULT) == true
                            ? parentProviderTypes[PARENT_PROVIDER_TYPE_DEFAULT] : null;
                    }

                    return string.IsNullOrEmpty(parentProviderType) ||
                        parentProviderType.Equals(provider.ParentProviderType, StringComparison.InvariantCultureIgnoreCase);
                });

            HydrateListComponentViewModelProperties(viewModel, request, fundingStreamsDic, localAuths);

            return BaseView(viewModel);
        }

        private static Dictionary<string, string> GetProviderParentGroupingType(Dictionary<string, FundingStream> fundingStreams, string defaultGroupingType = GroupingType.LocalAuthority)
        {
            var returnList = new Dictionary<string, string>
            {
                { PARENT_PROVIDER_TYPE_DEFAULT, defaultGroupingType }
            };

            BuildFundingStreamDictionary(fundingStreams, returnList);

            return returnList;
        }

        private static void BuildFundingStreamDictionary(Dictionary<string, FundingStream> fundingStreams, Dictionary<string, string> returnList)
        {
            foreach (var fundingStream in fundingStreams)
            {
                var specificParentProviderType = fundingStream.Value.SettingValues
                    .FirstOrDefault(settingValue => settingValue.Setting.SettingName == "ParentProviderType")?
                    .Value;

                if (string.IsNullOrEmpty(specificParentProviderType))
                {
                    continue;
                }

                returnList.Add(fundingStream.Key, specificParentProviderType);
            }
        }

        private static IEnumerable<MatStatementListItem> ApplyAcademyFilter(
            IEnumerable<MatStatementListItem> listItems,
            string[] filterValues)
        {
            return listItems.Where(item => filterValues.Contains(item.FundingViewData.EntityName));
        }

        private static IEnumerable<MatStatementListItem> ApplyFundingTypesFilter(
            IEnumerable<MatStatementListItem> listItems,
            string[] filterValues)
        {
            return listItems.Where(item => filterValues.Contains(item.FundingViewData.FundingStreamCode));
        }

        private static IEnumerable<MatStatementListItem> ApplyLocalAuthorityFilter(
            IEnumerable<MatStatementListItem> listItems,
            string[] filterValues)
        {
            return listItems.Where(item => filterValues.Contains(item.FundingViewData.LocalAuthorityName));
        }

        private RedirectToRouteResult RedirectToChild(bool viaChoicePage)
        {
            object routeData = null;

            if (viaChoicePage)
            {
                routeData = new { viaChoicePage };
            }

            return RedirectToRoute(LoggedInConstants.RouteName_ProviderStatement, routeData);
        }

        private void HydrateListComponentViewModelProperties(
            ParentSummaryPageViewModel viewModel,
            ListRequest request,
            Dictionary<string, FundingStream> fundingStreams,
            IEnumerable<IFundingApiSearchProviderFunding> localAuths)
        {
            var listItems = viewModel.FundingViewData.Keys.Select((fundingDataKey, index) =>
            {
                var fundingStreamData = viewModel.FundingViewData[fundingDataKey];

                fundingStreamData.LocalAuthorityName = localAuths.FirstOrDefault(funding =>
                    funding.OrganisationName == fundingStreamData.EntityName)?.LocalAuthorityName;

                var sharedData = fundingStreamData.Components.FirstOrDefault()?.PageData;
                var data = sharedData != null ?
                    sharedData.ToDictionary(entry => entry.Key, entry => entry.Value)
                    : new Dictionary<string, object>();

                data["AccordionInstanceNumber"] = index + 1;

                return new MatStatementListItem
                {
                    Header = fundingStreamData.Components?.SingleOrDefault(component => component.Type == ComponentType.Accordion_Button),
                    Body = fundingStreamData.Components?.SingleOrDefault(component => component.Type == ComponentType.Accordion_Panel),
                    Data = data,
                    FundingViewData = fundingStreamData
                };
            });

            viewModel.ListItems = GetFilteredListItems(listItems, request);
            viewModel.FilterCategories = GetListFilters(listItems, request, fundingStreams);
        }

        private IEnumerable<MatStatementListItem> GetFilteredListItems(IEnumerable<MatStatementListItem> listItems, ListRequest request)
        {
            if (request == null)
            {
                return listItems;
            }

            if (request.FilterRequest?.Filters?.Any() == true)
            {
                var listFilters = request.FilterRequest.Filters.OfType<ListFilterCategory>();
                foreach (var filter in listFilters)
                {
                    listItems = FilterActions[filter.Key](listItems, filter.Values);
                }
            }

            return listItems;
        }

        private IEnumerable<IFilterCategoryViewModel> GetListFilters(
            IEnumerable<MatStatementListItem> listItems,
            ListRequest request,
            IDictionary<string, FundingStream> fundingStreams)
        {
            var academies = listItems.Select(item => item.FundingViewData.EntityName).Distinct().OrderBy(academyName => academyName);
            var fundingTypes = listItems.Select(item => item.FundingViewData.FundingStreamCode).Distinct().OrderBy(fundingType => fundingType);
            var localAuthorities = listItems.Select(item => item.FundingViewData.LocalAuthorityName).Distinct().OrderBy(localAuth => localAuth);
            var listFilters = request.FilterRequest?.Filters?.OfType<ListFilterCategory>();

            var academiesFilter = new ListFilterCategoryViewModel
            {
                Key = AcademyFilterKey,
                Title = "Filter by academy",
                Groups = Enumerable.Empty<ListGroupFilterViewModel>(),
                Values = academies.Select(academy => new FilterValueViewModel
                {
                    Title = academy,
                    Value = academy,
                    Count = listItems.Count(item => item.FundingViewData.EntityName == academy),
                    Selected = listFilters?.Any(
                        filter => filter.Key == AcademyFilterKey && filter.Values.Contains(academy)) == true
                })
            };

            var fundingTypesFilter = new ListFilterCategoryViewModel
            {
                Key = FundingTypeFilterKey,
                Title = "Filter by funding type",
                Groups = Enumerable.Empty<ListGroupFilterViewModel>(),
                Values = fundingTypes.Select(fundingType => new FilterValueViewModel
                {
                    Title = fundingStreams[fundingType].FundingStreamName,
                    Value = fundingType,
                    Count = listItems.Count(item => item.FundingViewData.FundingStreamCode == fundingType),
                    Selected = listFilters?.Any(
                        filter => filter.Key == FundingTypeFilterKey && filter.Values.Contains(fundingType)) == true
                })
            };

            var localAuthorityFilter = new ListFilterCategoryViewModel
            {
                Key = LocalAuthorityFilterKey,
                Title = "Filter by local authority",
                Groups = Enumerable.Empty<ListGroupFilterViewModel>(),
                Values = localAuthorities.Select(la => new FilterValueViewModel
                {
                    Title = la,
                    Value = la,
                    Count = listItems.Count(item => item.FundingViewData.LocalAuthorityName == la),
                    Selected = listFilters?.Any(
                        filter => filter.Key == LocalAuthorityFilterKey && filter.Values.Contains(la)) == true
                })
            };

            return new[]
            {
                academiesFilter,
                fundingTypesFilter,
                localAuthorityFilter
            };
        }
    }
}
