using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Identity.Claims.Interfaces;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using PDS.ViewYourFunding.Web.Attributes;
using PDS.ViewYourFunding.Web.Constants;
using PDS.ViewYourFunding.Web.Interfaces;
using PDS.ViewYourFunding.Web.Models;
using PDS.ViewYourFunding.Web.Models.Request;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Controllers
{
    /// <summary>
    /// External API methods for View Your Funding.
    /// </summary>
    [TokenAuthorize]
    [ApiController]
    [Route("api/external")]
    public partial class ExternalApiController : BaseFundingController
    {
        /// <summary>
        /// A service to render a razor view to a string.
        /// </summary>
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

        /// <summary>
        /// The funding view service.
        /// </summary>
        private readonly IFundingViewService _fundingViewService;

        /// <summary>
        /// The component service.
        /// </summary>
        private readonly IComponentService _componentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalApiController"/> class.
        /// </summary>
        /// <param name="componentService">The component service to use.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="applicationConfigurationOptions">The config.</param>
        /// <param name="settingsService">The settings service.</param>
        /// <param name="mapper">Auto-mapper.</param>
        /// <param name="fundingApiService">The funding api service.</param>
        /// <param name="fundingViewService">The funding view service.</param>
        /// <param name="razorViewToStringRenderer">Service to render a view as HTML.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="globalSettingService">The service to get global settings.</param>
        public ExternalApiController(
            IComponentService componentService,
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            IUserJourneyService settingsService,
            IMapper mapper,
            IFundingApiService fundingApiService,
            IFundingViewService fundingViewService,
            IRazorViewToStringRenderer razorViewToStringRenderer,
            ICacheService cacheService,
            IGlobalSettingService globalSettingService)
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
            _razorViewToStringRenderer = razorViewToStringRenderer;
            _fundingViewService = fundingViewService;
        }

        /// <summary>
        /// Render a webpage to an HTML string.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <returns>The start page view.</returns>
        [Route(FundingConstants.Route_RenderHtml, Name = FundingConstants.RouteName_RenderHtml)]
        public async Task<IActionResult> RenderHtml([FromQuery] RenderHtmlRequest request)
        {
            if (request == null)
            {
                throw new Exception("Request object should not be null");
            }

            var fundingStreams = await GetActiveFundingStreams();
            var fundingStream = fundingStreams.FirstOrDefault(fs =>
                fs.FundingStreamCode.Equals(request.FundingStreamCode, StringComparison.InvariantCultureIgnoreCase));

            if (fundingStream == null)
            {
                throw new Exception($"Cannot find an active funding stream with the code '{request.FundingStreamCode}'");
            }

            var isFunding = !string.IsNullOrEmpty(request.FundingId);
            var funding = isFunding ? new[] { await GetFunding(request.FundingId, request.FundingStreamCode, fundingStream.UseFakeApiService()) } : null;

            var isProviderFunding = !string.IsNullOrEmpty(request.ProviderFundingId);
            var providerFunding = isProviderFunding ? new[] { await GetProviderFunding(request.ProviderFundingId, request.FundingStreamCode, fundingStream.UseFakeApiService()) } : null;

            if ((funding == null || funding.First().Id == null) && (providerFunding == null || providerFunding.First().Id == null))
            {
                throw new Exception("Unable to find data from indices.");
            }

            var showSelectors = await GetShowSelectorsState();
            var asStatementSpecification = await GetStatementSpecificationState();
            var showData = await GetShowData();

            var fundingViewData = await _fundingViewService.GenerateFundingViewData(
                _componentService,
                request.FundingPeriodCode,
                fundingStream.FundingStreamCode,
                fundingStreams.ToArray(),
                request.CutOffDate,
                null,
                null,
                FundingViewScope.Unknown,
                GetComponentDefaults(),
                null,
                true,
                true,
                null,
                null,
                bubbleUpException: true,
                iFundingApiSearchFunding: funding,
                explicitFundingPassed: isFunding,
                iFundingApiSearchProviderFunding: providerFunding,
                explicitProviderFundingPassed: isProviderFunding,
                previewLayoutModel: GetLayoutModel(fundingStream, request.LayoutId),
                showSelectors: showSelectors,
                asStatementSpecification: asStatementSpecification,
                showData: showData);

            var model = new RenderHtmlViewModel
            {
                FundingViewData = fundingViewData
            };

            if (model.FundingViewData == null)
            {
                throw new Exception("A problem occured while generating the funding view data");
            }

            var html = await _razorViewToStringRenderer.RenderViewToStringAsync("Shared/RenderHtml", model);

            if (ContainsErrorComponent(model.FundingViewData.Components) || html.Contains("\"componentError\""))
            {
                throw new Exception(html);
            }

            return Content(html, "text/html");
        }

        /// <summary>
        /// Returns the file on basis of request object.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <returns>Returns the file data.</returns>
        [Route(FundingConstants.Route_GetFile, Name = FundingConstants.RouteName_GetFile)]
        public async Task<IActionResult> GetFile([FromQuery] RenderFileRequest request)
        {
            if (request == null)
            {
                throw new Exception("Request object should not be null");
            }

            var fundingStreams = await GetActiveFundingStreams();
            var fundingStream = fundingStreams.FirstOrDefault(fs =>
                fs.FundingStreamCode.Equals(request.FundingStreamCode, StringComparison.InvariantCultureIgnoreCase));

            if (fundingStream == null)
            {
                throw new Exception($"Cannot find an active funding stream with the code '{request.FundingStreamCode}'");
            }

            var openDocumentFormatStates = new FileFormat[] { FileFormat.ODS };

            var isFunding = !string.IsNullOrEmpty(request.FundingId);
            var funding = isFunding ? new[] { await GetFunding(request.FundingId, request.FundingStreamCode, fundingStream.UseFakeApiService()) } : null;

            var isProviderFunding = !string.IsNullOrEmpty(request.ProviderFundingId);
            var providerFunding = isProviderFunding ? new[] { await GetProviderFunding(request.ProviderFundingId, request.FundingStreamCode, fundingStream.UseFakeApiService()) } : null;

            if ((funding == null || funding.First()?.Id == null) && (providerFunding == null || providerFunding.First()?.Id == null))
            {
                throw new Exception("Unable to find data from indices.");
            }

            var outputData = (await _fundingViewService.GenerateFundingDocument(
              fundingStream,
              request.FundingPeriodCode,
              request.PublicationDate,
              null,
              FundingViewType.Other,
              FundingViewScope.Unknown,
              openDocumentFormatStates,
              previewLayoutModel: GetLayoutModel(fundingStream, request.LayoutId),
              iFundingApiSearchFunding: funding,
              explicitFundingPassed: isFunding,
              iFundingApiSearchProviderFunding: providerFunding,
              explicitProviderFundingPassed: isProviderFunding)).First();

            var fileName = string.IsNullOrEmpty(request.FileName) ? "Result.ods" : request.FileName;

            return File(outputData.Data, "application/octet-stream", fileName);
        }

        /// <summary>
        /// Returns a list of GetAutoPullFundingStreamResults containing the funding stream code and name for auto pull configured funding streams.
        /// </summary>
        /// <returns>Returns a list of GetAutoPullFundingStreamResults for auto pull configured funding streams.</returns>
        [Route(FundingConstants.Route_GetAutoPullConfiguredFundingStreams, Name = FundingConstants.RouteName_GetAutoPullConfiguredFundingStreams)]
        public async Task<List<GetAutoPullFundingStreamResult>> GetAutoPullConfiguredFundingStreams()
        {
            var autoPullFundingStreams = await GetAutoPullFundingStreams();
            var getAutoPullFundingStreamResults = autoPullFundingStreams.Select(fs => new GetAutoPullFundingStreamResult()
            {
                FundingStreamCode = fs.FundingStreamCode,
                FundingStreamName = fs.FundingStreamName
            })
            .ToList();
            return getAutoPullFundingStreamResults;
        }

        /// <summary>
        /// Gets the email enabled funding stream and periods.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Route(FundingConstants.Route_GetEmailEnabledFundingStreamAndPeriods, Name = FundingConstants.RouteName_GetEmailEnabledFundingStreamAndPeriods)]
        public async Task<List<EmailEnabledFundingStreamAndPeriodsResult>> GetEmailEnabledFundingStreamAndPeriods()
        {
            var activeFundingStreams = await GetActiveFundingStreams();

            return activeFundingStreams
                .Where(fs => fs.Active == true
                    && (fs.RelevantForProviders_LoggedIn == true || fs.RelevantForOrganisations_LoggedIn == true)
                    && !string.IsNullOrWhiteSpace(fs.SettingValues
                    .FirstOrDefault(setting => setting.Setting.SettingName == SettingName.EmailEnabledFundingPeriods)
                    ?.Value))
                .Select(fs =>
                {
                    var fundingPeriods = fs.SettingValues
                        .FirstOrDefault(setting => setting.Setting.SettingName == SettingName.EmailEnabledFundingPeriods)
                        ?.Value
                        .Split(",")
                        .ToList();

                    var digitalStatementsGoLiveDateStr = fs.SettingValues
                        .FirstOrDefault(setting => setting.Setting.SettingName == SettingName.DigitalStatementsGoLiveDate)
                        ?.Value;

                    DateTime? digitalStatementsGoLiveDate = null;

                    if (digitalStatementsGoLiveDateStr != null && DateTime.TryParseExact(
                                    digitalStatementsGoLiveDateStr,
                                    EditTypeConstants.DateFormat,
                                    EditTypeConstants.EnGbCultureInfo,
                                    DateTimeStyles.AdjustToUniversal,
                                    out var date))
                    {
                        digitalStatementsGoLiveDate = date;
                    }

                    var result = new EmailEnabledFundingStreamAndPeriodsResult
                    {
                        FundingStreamCode = fs.FundingStreamCode,
                        FundingStreamName = fs.FundingStreamName,
                        FundingPeriods = fundingPeriods,
                        DigitalStatementsGoLiveDate = digitalStatementsGoLiveDate,
                        HasChildViewEnabled = fs.RelevantForProviders_LoggedIn,
                        HasParentViewEnabled = fs.RelevantForOrganisations_LoggedIn,
                    };

                    return result;
                })
                .ToList();
        }

        private static bool ContainsErrorComponent(List<Services.Interfaces.Models.Component> components)
        {
            if (components == null)
            {
                return false;
            }

            foreach (var component in components)
            {
                if (ComponentHelper.IsErrorType(component?.Type) || ContainsErrorComponent(component?.Components))
                {
                    return true;
                }
            }

            return false;
        }

        private PreviewLayoutModel GetLayoutModel(FundingStream fundingStream, string layoutId)
        {
            return new PreviewLayoutModel
            {
                FundingViewScope = FundingViewScope.Unknown,
                FundingViewType = FundingViewType.Other,
                FundingStreamId = fundingStream.Id,
                IsPreview = true,
                LayoutId = layoutId
            };
        }
    }
}