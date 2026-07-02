namespace PDS.VYF.Services.Implementations.AppServices
{
    using Ardalis.GuardClauses;
    using Pds.Core.Logging;
    using PDS.ViewYourFunding.Services.DTOs;
    using PDS.ViewYourFunding.Services.Enums;
    using PDS.ViewYourFunding.Services.Interfaces;
    using PDS.ViewYourFunding.Services.Interfaces.Models;
    using PDS.ViewYourFunding.Services.Models;
    using PDS.VYF.Services.Abstracts.AppServices;
    using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
    using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
    using PDS.VYF.Services.Extensions.AdminSettings;
    using PDS.VYF.Services.Extensions.Core;
    using PDS.VYF.Services.Extensions.ModelMapping;
    using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
    using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;
    using PDS.VYF.Services.Models.ResponseModels.ViewDataResponseModels;

    /// <summary>
    /// The Parent Funding View Services.
    /// </summary>
    /// <seealso cref="PDS.VYF.Services.Abstracts.AppServices.IParentFundingViewServices" />
    public class ParentFundingViewServices : IParentFundingViewServices
    {
        private readonly ISharedFundingViewServices sharedFundingViewServices;
        private readonly ILoggerAdapter<ParentFundingViewServices> loggerService;
        private readonly ICacheService cacheService;
        private readonly IChildApiClientServices childApiClientServices;
        private readonly IParentApiClientServices parentApiClientServices;
        private readonly IFundingStreamSettingsServices fundingStreamSettingsServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentFundingViewServices"/> class.
        /// </summary>
        /// <param name="sharedFundingViewServices">The shared funding view services.</param>
        /// <param name="loggerService">The logger service.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="childApiClientServices">The child API client services.</param>
        /// <param name="parentApiClientServices">The parent API client services.</param>
        /// <param name="fundingStreamSettingsServices">The funding stream settings services.</param>
        /// <param name="userCountApiClientServices">The user count API client services.</param>
        public ParentFundingViewServices(
            ISharedFundingViewServices sharedFundingViewServices,
            ILoggerAdapter<ParentFundingViewServices> loggerService,
            ICacheService cacheService,
            IChildApiClientServices childApiClientServices,
            IParentApiClientServices parentApiClientServices,
            IFundingStreamSettingsServices fundingStreamSettingsServices)
        {
            this.sharedFundingViewServices = sharedFundingViewServices;
            this.loggerService = loggerService;
            this.cacheService = cacheService;
            this.childApiClientServices = childApiClientServices;
            this.parentApiClientServices = parentApiClientServices;
            this.fundingStreamSettingsServices = fundingStreamSettingsServices;
        }

        /// <summary>
        /// Gets the parent summary view data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The ParentSummaryViewDataResponseModel.</returns>
        public async Task<ParentSummaryViewDataResponseModel> GetParentSummaryViewData(ParentSummaryViewDataRequestModel request)
        {
            var response = this.ValidateParentSummaryViewDataRequestModel(request);

            if (response.IsValidUrl && response.HasUserHaveRightAccess)
            {
                var cachedResponse = await this.cacheService.AddOrGetExistingResult(
                                            request.CacheKey,
                                            () => this.GetParentSummaryViewDataInternal(request, response),
                                            ViewYourFunding.Services.Cache.CacheExpirationPolicy.Sliding,
                                            TimeSpan.FromMinutes(5));

                return cachedResponse;
            }

            return response;
        }

        // Internal methods

        /// <summary>
        /// Gets the parent summary view data internal.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <returns>The ParentSummaryViewDataResponseModel.</returns>
        internal async Task<ParentSummaryViewDataResponseModel> GetParentSummaryViewDataInternal(ParentSummaryViewDataRequestModel request, ParentSummaryViewDataResponseModel response)
        {
            response.HasUserHaveRightAccess = true;

            var parentDataApiRequest = new ParentSearchApiRequestModel(true, request.UkprnFromLoggedInUser ?? string.Empty);

            parentDataApiRequest.FundingStreamPeriods = await this.fundingStreamSettingsServices.GetEmailEnabledFundingStreamPeriod();

            parentDataApiRequest.SetSelectFieldsExcept(a => new { a.FundingLines, a.Calculations });

            var parentFundingData = await this.parentApiClientServices.SearchParent(parentDataApiRequest, false, parentDataApiRequest.FundingStreamPeriods);

            var childrenUKPRNs = parentFundingData?.SelectMany(a => a.ChildUKPRNs ?? new List<string>())?.ToList();

            childrenUKPRNs = await this.childApiClientServices.GetCurrentChildUkprnsForParent(request.UkprnFromLoggedInUser ?? string.Empty, childrenUKPRNs);

            if (childrenUKPRNs?.Count > 0)
            {
                var childDataApiRequest = new ChildSearchApiRequestModel()
                {
                    ListOfUKPRNs = childrenUKPRNs,
                    FundingStreamPeriods = parentDataApiRequest.FundingStreamPeriods,
                    HasToBeLatestFunding = true,
                    HasIYOToBeRemoved = false,
                };

                childDataApiRequest.SetSelectFieldsExcept(a => new { a.FundingLines, a.Calculations });

                var childFundingData = await this.childApiClientServices.SearchChild(childDataApiRequest, false, parentDataApiRequest.FundingStreamPeriods);

                childFundingData = childFundingData.GroupBy(x => x.OrganisationUkprn)
                .Select(g =>
                    g.OrderByDescending(x => x.FundingStreamPeriod)
                     .First()).OrderByDescending(y => y.StatusChangedDate).ToList();

                var fundingViewdataDic = new Dictionary<string, FundingViewData>();
                var providerFundingData = new List<IFundingApiSearchProviderFunding>();
                var relavantFundingStreams = new HashSet<FundingStream>();

                foreach (var childFundingDatum in childFundingData)
                {
                    var fundingStream = await this.fundingStreamSettingsServices.GetFundingStream(childFundingDatum.FundingStreamCode!);
                    var publication = fundingStream?.GetLatestPublication(childFundingDatum.FundingPeriodCode);
                    var providerFundingDatum = LoggedInChildModelExtensions.GetProviderFundingApiSearchResponse(childFundingDatum);

                    if (fundingStream != null && !relavantFundingStreams.Contains(fundingStream))
                    {
                        relavantFundingStreams.Add(fundingStream);
                    }

                    providerFundingData.AddRange(providerFundingDatum.ProviderFunding);

                    if (publication != null && fundingStream != null)
                    {
                        var childSummaryRequest = new ChildSummaryViewDataRequestModel()
                        {
                            FundingStreamCode = childFundingDatum.FundingStreamCode ?? string.Empty,
                            FundingPeriodCode = childFundingDatum.FundingPeriodCode ?? string.Empty,
                            FundingData = null,
                            ProviderFundingData = providerFundingDatum,
                            PublicationDate = childFundingDatum.StatusChangedDateOnly,
                            PreviousPublicationDate = null,
                            FundingStreamConfig = fundingStream,
                            FundingDocument = null,
                            IsLatestOrFinalFundingForYear = true,
                            IsCurrentYear = true,
                            PublicationUiModelVersion = publication?.UIModelVersion,
                            SearchTerm = null,
                            SelectedTab = string.Empty,
                            IsALoggedInView = true,
                            SelectedVarianceOption = VarianceSelectionOption.NoComparison,
                            ViaChoicePage = request.ViaChoicePage,
                            SchemaVersion = childFundingDatum.SchemaVersion,
                            TemplateVersion = childFundingDatum.TemplateVersion,
                            FundingViewType = FundingViewType.ViewData,
                            FundingViewScope = FundingViewScope.LoggedInMatProviderSummary,
                        };

                        var fundingData = await this.sharedFundingViewServices.GetFundingViewData(childSummaryRequest);

                        fundingViewdataDic.Add($"{fundingData.FundingStreamCode}-{fundingData.EntityPrimaryIdentifier}", fundingData);
                    }

                    if (fundingViewdataDic.Count > 0)
                    {
                        response.ProviderFundingData = providerFundingData;
                        response.FundingViewData = fundingViewdataDic;
                        response.HasFundingDataExists = true;
                        response.RelavantFundingStreams = relavantFundingStreams;
                    }
                }
            }

            return response;
        }

        // Private Methods
        private ParentSummaryViewDataResponseModel ValidateParentSummaryViewDataRequestModel(ParentSummaryViewDataRequestModel request)
        {
            var response = new ParentSummaryViewDataResponseModel();

            try
            {
                Guard.Against.ValidUkrpn(request.UkprnFromLoggedInUser);

                response.IsValidUrl = true;
                response.HasUserHaveRightAccess = true;
            }
            catch (Exception ex)
            {
                this.loggerService.LogError($"Invalid data provided in Child Summary Page Request. Error Message: {ex.Message}");
            }

            return response;
        }
    }
}
