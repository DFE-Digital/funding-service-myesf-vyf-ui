#define vyfv2

using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Web.Components.Areas.Lists.DTOs;
using Pds.Core.Web.Components.Areas.Lists.Models;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Attributes;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Controllers
{
    /// <summary>
    /// The UI controller for logged in multiple academy trusts.
    /// </summary>
    [Area("LoggedIn")]
    [Authorize(nameof(UserRole.ViewAllocationStatements))]
    [ServiceFilter(typeof(MultipleAcademyTrustViewCheckAttribute))]
    public class MultipleAcademyTrustController : BaseFundingController
    {
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

        /// <summary>
        /// The feedback link.
        /// </summary>
        private readonly string _feedbackLink;

        /// <summary>
        /// The contact us link.
        /// </summary>
        private readonly string _contactUsLink;

        /// <summary>
        /// If the number of list items is equal to, or more than, this number then display the back to top link.
        /// </summary>
        private readonly int _backToTopListCountTrigger;

        /// <summary>
        /// The funding view service.
        /// </summary>
        private readonly IFundingViewService _fundingViewService;

        /// <summary>
        /// The component service.
        /// </summary>
        private readonly IComponentService _componentService;

        /// <summary>
        /// The funding Api service.
        /// </summary>
        private readonly IFundingApiService _fundingApiService;

        /// <summary>
        /// The home link.
        /// </summary>
        private readonly string _homeLink;

        /// <summary>
        /// The choice page link.
        /// </summary>
        private readonly string _choicePageLink;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleAcademyTrustController"/> class.
        /// </summary>
        /// <param name="componentService">The component service to use.</param>
        /// <param name="securityService">The security service to use.</param>
        /// <param name="settingsService">The settings service to use.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="fundingApiService">The API service to use for searching for funding.</param>
        /// <param name="fundingViewService">The funding view service.</param>
        /// <param name="globalSettingService">The service to get global settings.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="applicationConfigurationOptions">The configuration service.</param>
        public MultipleAcademyTrustController(
            IComponentService componentService,
            IClaimsBasedIdentityService securityService,
            IUserJourneyService settingsService,
            IMapper mapper,
            IFundingApiService fundingApiService,
            IFundingViewService fundingViewService,
            IGlobalSettingService globalSettingService,
            ICacheService cacheService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions)
            : base(
                  securityService,
                  applicationConfigurationOptions,
                  settingsService,
                  mapper,
                  fundingApiService,
                  cacheService,
                  fundingViewService,
                  globalSettingService)
        {
            _componentService = componentService;
            _fundingViewService = fundingViewService;
            _contactUsLink = applicationConfigurationOptions.Value.ContactUsLink;
            _feedbackLink = applicationConfigurationOptions.Value.FeedbackLinkForLoggedInView;
            _fundingApiService = fundingApiService;
            _backToTopListCountTrigger = applicationConfigurationOptions.Value.BackToTopLinkMinimumCount;
            _homeLink = applicationConfigurationOptions.Value.LoggedInProviderHomeLink;

            var homeLinkIsAbsolute = _homeLink.Contains("http", StringComparison.InvariantCultureIgnoreCase);

            if (homeLinkIsAbsolute)
            {
                var homeLinkUri = new Uri(_homeLink, UriKind.Absolute);
                _choicePageLink = new UriBuilder(homeLinkUri.Scheme, homeLinkUri.Host, homeLinkUri.Port, "choose-a-statement-type")
                    .ToString().Replace(":80/", "/").Replace(":443/", "/");
            }
            else
            {
                _choicePageLink = "/choose-a-statement-type";
            }
        }

        /// <summary>
        /// The MVC action for the MAT statement page.
        /// </summary>
        /// <param name="request">List request for filtering and paging the statements.</param>
        /// <param name="viaChoicePage">if set to <c>true</c> [via choice page].</param>
        /// <returns>
        /// The start page view.
        /// </returns>
#if !vyfv2
        [Route(LoggedInConstants.Route_MultipleAcademyTrustStatement, Name = LoggedInConstants.RouteName_MultipleAcademyTrustStatement)]
#endif
        public async Task<IActionResult> MultipleAcademyTrustStatement(ListRequest request, bool viaChoicePage = false)
        {
            var userDetails = await GetUserAsync();

            if (!await CheckMatStatus(userDetails, Enums.FundingUIViewType.Providers_LoggedIn))
            {
                if (viaChoicePage)
                {
                    return RedirectToRoute(LoggedInConstants.RouteName_ProviderStatement, new { viaChoicePage = viaChoicePage });
                }

                return RedirectToRoute(LoggedInConstants.RouteName_ProviderStatement);
            }

            var viewModel = await GetBasePageViewModel<MultipleAcademyTrustStatementViewModel>(userDetails, true);
            viewModel.ContactUsLink = _contactUsLink;
            viewModel.FeedbackLink = _feedbackLink;
            viewModel.BackToTopLinkMinimumCount = _backToTopListCountTrigger;
            viewModel.HomeLink = _homeLink;
            viewModel.ViaChoicePage = viaChoicePage;
            viewModel.ChoicePageLink = _choicePageLink;

            var fundingStreams = await GetRelevantFundingStreams(
                Enums.FundingUIViewType.Organisations_LoggedIn,
                userDetails,
                true);

            var matUkprn = GetUkprn(userDetails);
            var dataRequirements = await GetDataRequirements(fundingStreams, userDetails, true);
            var fundingRequestObject = SquashDataRequirements(dataRequirements, "Funding");
            var fundingData = await _fundingApiService.SearchFunding(fundingRequestObject);
            var ukprnList = GetProviderFundingIds(fundingData.Funding.Select(funding => funding.ProviderFundings))?.ToList();

            if (ukprnList?.Any() != true)
            {
                throw new Exception($"No provider fundings found for academy trust with ukprn {matUkprn}");
            }

            var viewModelProviderResults = await DoProviderFundingSearch(
                null,
                fundingStreams,
                null,
                null,
                multipleUkPrnSearchList: string.Join("|", ukprnList),
                currentUser: userDetails,
                currentUserPassed: true,
                byPassGrouping: true);

            var viewDataList = await GenerateAllFundingViewData(fundingStreams, viewModelProviderResults, userDetails, viaChoicePage);
            AddFundingViewDataToViewModel(viewModel, viewDataList);

            var parentProviderTypes = GetProviderParentGroupingType(fundingStreams);
            var localAuths = viewModelProviderResults
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

            HydrateListComponentViewModelProperties(viewModel, request, fundingStreams, localAuths);

            return View(viewModel);
        }

        private static void AddFundingViewDataToViewModel(MultipleAcademyTrustStatementViewModel viewModel, FundingViewData[] viewDataList)
        {
            foreach (var fundingViewDataRequest in viewDataList)
            {
                var fundingViewDataResponse = fundingViewDataRequest;
                var dataKey =
                    $"{fundingViewDataResponse.FundingStreamCode}-{fundingViewDataResponse.EntityPrimaryIdentifier}";

                if (!viewModel.FundingViewData.ContainsKey(dataKey))
                {
                    viewModel.FundingViewData.Add(dataKey, fundingViewDataResponse);
                }
            }
        }

        private async Task<FundingViewData[]> GenerateAllFundingViewData(
            Dictionary<string, FundingStream> fundingStreams,
            List<IFundingApiSearchProviderFunding> viewModelProviderResults,
            Pds.Core.Common.Identity.Models.User userDetails,
            bool viaChoicePage)
        {
            var fundingViewDataRequests = new List<Task<FundingViewData>>();
            var previewModeEnabled = await PreviewModeEnabled(userDetails, true);
            var componentDefaults = GetComponentDefaults();
            var ukprn = GetUkprn(userDetails);

            var showSelectors = await GetShowSelectorsState();
            var asStatementSpecification = await GetStatementSpecificationState();
            var showData = await GetShowData();

            var uniqueUkPrnAndFundingStreamGroups = viewModelProviderResults
                .GroupBy(funding => new { funding.OrganisationUkprn, funding.FundingStreamCode })
                .Select(fundingGroup => fundingGroup.OrderByDescending(x => x.StatusChangedDate));

            foreach (var fundingGroup in uniqueUkPrnAndFundingStreamGroups)
            {
                var providerFunding = GetDistinctProviderFundings(fundingGroup).FirstOrDefault();
                var fundingStream = fundingStreams[providerFunding.FundingStreamCode];
                var publication = fundingStream.GetLatestPublication(previewModeEnabled);

                if (publication == null)
                {
                    continue;
                }

                var fundingDocumentFileType = fundingStream.SettingValues.FirstOrDefault(settingValue =>
                    settingValue.Setting.SettingName == "FundingDocumentFileType")?.Value;

                if (string.IsNullOrEmpty(fundingDocumentFileType))
                {
                    fundingDocumentFileType = FundingDocumentFileType.Spreadsheet_OpenFormat;
                }

                var fundingDocument = GetProviderFundingDocument(
                    providerFunding,
                    publication.PublishedDate,
                    fundingStream,
                    true,
                    fundingDocumentFileType);

                var filters = new[]
                {
                    new SearchFilter
                    {
                        PropertyName = SearchFilterPropertyName.Ukprn,
                        PropertyValue = providerFunding.OrganisationUkprn
                    }
                };

                fundingViewDataRequests.Add(_fundingViewService.GenerateFundingViewData(
                    _componentService,
                    publication.FundingPeriodCode,
                    fundingStream.FundingStreamCode,
                    fundingStreams.Values.ToArray(),
                    FundingPeriodHelper.GetCutOffDateForPublication(publication),
                    publication,
                    publication.UIModelVersion,
                    FundingViewScope.LoggedInMatProviderSummary,
                    componentDefaults,
                    fundingDocument,
                    true,
                    true,
                    null,
                    filters,
                    null,
                    bubbleUpException: false,
                    iFundingApiSearchProviderFunding: new IFundingApiSearchProviderFunding[] { providerFunding },
                    explicitProviderFundingPassed: true,
                    showSelectors: showSelectors,
                    asStatementSpecification: asStatementSpecification,
                    showData: showData,
                    viaChoicePage: viaChoicePage));
            }

            return await Task.WhenAll(fundingViewDataRequests);
        }

        private void HydrateListComponentViewModelProperties(
            MultipleAcademyTrustStatementViewModel viewModel,
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