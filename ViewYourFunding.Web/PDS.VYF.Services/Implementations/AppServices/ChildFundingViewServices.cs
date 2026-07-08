namespace PDS.VYF.Services.Implementations.AppServices
{
    using Ardalis.GuardClauses;
    using MoreLinq;
    using Pds.Core.Logging;
    using PDS.ViewYourFunding.Services.Constants;
    using PDS.ViewYourFunding.Services.DTOs;
    using PDS.ViewYourFunding.Services.Enums;
    using PDS.ViewYourFunding.Services.Helper;
    using PDS.ViewYourFunding.Services.Implementations.FundingView;
    using PDS.ViewYourFunding.Services.Interfaces;
    using PDS.VYF.Services.Abstracts.AppServices;
    using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
    using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
    using PDS.VYF.Services.Enums;
    using PDS.VYF.Services.Extensions.AdminSettings;
    using PDS.VYF.Services.Extensions.Core;
    using PDS.VYF.Services.Extensions.ModelMapping;
    using PDS.VYF.Services.Helpers;
    using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
    using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;
    using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;
    using PDS.VYF.Services.Models.ResponseModels.ViewDataResponseModels;
    using PDS.VYF.Services.Models.ViewDataModels;
    using System.Globalization;

    /// <summary>
    /// The Child Funding View Services.
    /// </summary>
    /// <seealso cref="PDS.VYF.Services.Abstracts.AppServices.IChildFundingViewServices" />
    public class ChildFundingViewServices : IChildFundingViewServices
    {
        private readonly ISharedFundingViewServices sharedFundingViewServices;
        private readonly ILoggerAdapter<ModelFundingViewService> loggerService;
        private readonly ICacheService cacheService;
        private readonly IChildApiClientServices childApiClientServices;
        private readonly IParentApiClientServices parentApiClientServices;
        private readonly IFundingStreamSettingsServices fundingStreamSettingsServices;
        private readonly IUserCountApiClientServices userCountApiClientServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildFundingViewServices"/> class.
        /// </summary>
        /// <param name="sharedFundingViewServices">The shared funding view services.</param>
        /// <param name="loggerService">The logger service.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="childApiClientServices">The child API client services.</param>
        /// <param name="parentApiClientServices">The parent API client services.</param>
        /// <param name="fundingStreamSettingsServices">The funding stream settings services.</param>
        /// <param name="userCountApiClientServices">The user count API client services.</param>
        public ChildFundingViewServices(
            ISharedFundingViewServices sharedFundingViewServices,
            ILoggerAdapter<ModelFundingViewService> loggerService,
            ICacheService cacheService,
            IChildApiClientServices childApiClientServices,
            IParentApiClientServices parentApiClientServices,
            IFundingStreamSettingsServices fundingStreamSettingsServices,
            IUserCountApiClientServices userCountApiClientServices)
        {
            this.sharedFundingViewServices = sharedFundingViewServices;
            this.loggerService = loggerService;
            this.cacheService = cacheService;
            this.childApiClientServices = childApiClientServices;
            this.parentApiClientServices = parentApiClientServices;
            this.fundingStreamSettingsServices = fundingStreamSettingsServices;
            this.userCountApiClientServices = userCountApiClientServices;
        }

        // Public Methods

        /// <summary>
        /// Retrieves the summary view data for a child.
        /// </summary>
        /// <param name="request">The request model containing the necessary parameters.</param>
        /// <returns>
        /// The response model containing the summary view data.
        /// </returns>
        public async Task<ChildSummaryViewDataResponseModel> GetChildSummaryViewData(ChildSummaryViewDataRequestModel request)
        {
            var response = this.ValidateChildSummaryViewDataRequestModel(request);

            if (response.IsValidUrl && response.HasUserHaveRightAccess)
            {
                var cachedResponse = await this.cacheService.AddOrGetExistingResultAsync(
                                            request.CacheKey,
                                            () => this.GetChildSummaryViewDataInternal(request),
                                            ViewYourFunding.Services.Cache.CacheExpirationPolicy.Sliding,
                                            TimeSpan.FromMinutes(5));

                cachedResponse.HasUserHaveRightAccess = true;
                cachedResponse.IsValidUrl = true;
                return cachedResponse;
            }

            return response;
        }

        /// <summary>
        /// Retrieves the detailed view data for a child.
        /// </summary>
        /// <param name="request">The request model containing the necessary parameters.</param>
        /// <returns>
        /// The response model containing the detailed view data.
        /// </returns>
        public async Task<ChildDetailedViewDataResponseModel> GetChildDetailedViewData(ChildDetailedViewDataRequestModel request)
        {
            var response = await this.ValidateAndEnrichChildDetailedViewDataRequestModel(request);

            if (response.IsValidUrl && response.HasUserHaveRightAccess)
            {
                await this.EnrichChildComparisonDetails(request);

                var cachedResponse = await this.cacheService.AddOrGetExistingResultAsync(
                                                                        request.CacheKey,
                                                                        () => this.GetChildDetailedViewDataInternal(request),
                                                                        ViewYourFunding.Services.Cache.CacheExpirationPolicy.Sliding,
                                                                        TimeSpan.FromMinutes(5));

                if (!cachedResponse.HasFundingDataExists)
                {
                    this.cacheService.RemoveCacheItem(request.CacheKey);
                }

                var fundingStream = await this.fundingStreamSettingsServices.GetFundingStream(request.FundingStreamCode);

                cachedResponse.FundingStreamCode = fundingStream!.FundingStreamCode;
                cachedResponse.FundingStreamName = fundingStream.FundingStreamName;
                cachedResponse.IsValidUrl = true;
                cachedResponse.HasUserHaveRightAccess = true;

                var pageData = cachedResponse.FundingViewData?.Components.FirstOrDefault()?.PageData;

                if (pageData != null && pageData.ContainsKey("ComparisonOptions") == false)
                {
                    pageData.TryAdd("ComparisonOptions", request.ComparisonOptions ?? new Dictionary<VarianceSelectionOption, DateTime>());
                }

                if (pageData != null && pageData.ContainsKey("SelectedVarianceOption") == false)
                {
                    pageData.TryAdd("SelectedVarianceOption", request.SelectedVarianceOption);
                }

                return cachedResponse;
            }

            return response;
        }

        /// <summary>
        /// Retrieves the history view data for a child.
        /// </summary>
        /// <param name="request">The request model containing the necessary parameters.</param>
        /// <returns>
        /// The response model containing the history view data.
        /// </returns>
        public async Task<ChildHistoryViewDataResponseModel> GetChildHistoryViewData(ChildHistoryViewDataRequestModel request)
        {
            var response = await this.ValidateChildHistoryViewDataRequest(request);

            if (response.IsValidUrl && response.HasUserHaveRightAccess)
            {
                var cachedResponse = await this.cacheService.AddOrGetExistingResultAsync(
                                                                        request.CacheKey,
                                                                        () => this.GetChildHistoryViewDataInternal(request, response),
                                                                        ViewYourFunding.Services.Cache.CacheExpirationPolicy.Sliding,
                                                                        TimeSpan.FromMinutes(5));

                if (!cachedResponse.HasFundingDataExists)
                {
                    this.cacheService.RemoveCacheItem(request.CacheKey);
                }

                return cachedResponse;
            }

            return response;
        }

        // Internal Methods

        /// <summary>
        /// Gets the child summary view data internal.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The Child Summary View Data Response.</returns>
        internal async Task<ChildSummaryViewDataResponseModel> GetChildSummaryViewDataInternal(ChildSummaryViewDataRequestModel request)
        {
            var response = new ChildSummaryViewDataResponseModel() { ViaChoicePage = request.ViaChoicePage };

            // Pull data from Data Store for the Given Request.
            var childSearchApiRequest = new ChildSearchApiRequestModel(true, request.UkprnFromLoggedInUser!);
            childSearchApiRequest.SetSelectFieldsExcept(a => new { a.FundingLines, a.Calculations });

            childSearchApiRequest.FundingStreamPeriods = await this.GetCurrentAndHistoricFundingStreamPeriods(childSearchApiRequest);

            var childFundingData = await this.childApiClientServices.SearchChild(childSearchApiRequest, false);

            // Generate FundingView Data
            var result = new Dictionary<string, FundingViewData>();

            request.IsLatestOrFinalFundingForYear = true;
            request.IsCurrentYear = true;
            request.SearchTerm = null;
            request.SelectedTab = string.Empty;

            foreach (var childFundingDatum in childFundingData)
            {
                var fundingStream = await this.fundingStreamSettingsServices.GetFundingStream(childFundingDatum.FundingStreamCode!);
                var publication = fundingStream?.GetLatestPublication(childFundingDatum.FundingPeriodCode);
                var statementVisitInfo = await this.GetStatementVisitInfo(request.UserId!, childFundingDatum, fundingStream);

                if (publication != null && fundingStream != null)
                {
                    request.FundingStreamCode = childFundingDatum.FundingStreamCode!;
                    request.FundingPeriodCode = childFundingDatum.FundingPeriodCode!;
                    request.ProviderFundingData = LoggedInChildModelExtensions.GetProviderFundingApiSearchResponse(childFundingDatum);
                    request.PublicationDate = childFundingDatum.StatusChangedDateOnly;
                    request.FundingStreamConfig = fundingStream;
                    request.PublicationUiModelVersion = publication?.UIModelVersion;
                    request.SchemaVersion = childFundingDatum.SchemaVersion;
                    request.TemplateVersion = childFundingDatum.TemplateVersion;
                    request.FundingViewScope = childFundingDatum.IsIndicative == true ? FundingViewScope.LoggedInIndicativeProviderSummary : FundingViewScope.LoggedInProviderSummary;

                    result.Add(childFundingDatum.Id!, await this.sharedFundingViewServices.GetFundingViewData(request));

                    response.StatementVisitInfo.Add(childFundingDatum.Id!, statementVisitInfo);
                }
            }

            // Store the Funding View data in Response Object.
            if (result.Count > 0)
            {
                response.HasFundingDataExists = true;
                var fundingViewData = new Dictionary<string, FundingViewData>();

                bool secondYearIYO = result.Values.Where(v => v.InYearOpener == true && v.IsIndicativeFunding == false).Count() == 2;

                if (secondYearIYO)
                {
                    result.OrderByDescending(a => a.Key).ForEach(a => fundingViewData.Add(a.Key, a.Value));
                    response.FundingViewData = fundingViewData;
                }
                else
                {
                    var latestFundingviewData = result.GroupBy(a => a.Value.FundingStreamCode)
                        .Select(a => a.OrderByDescending(a => a.Key).FirstOrDefault()).ToList();

                    latestFundingviewData.ForEach(a => fundingViewData.Add(a.Key, a.Value));

                    response.FundingViewData = fundingViewData;
                }
            }

            this.SetOrganizationNameAndUrn(childFundingData.OrderByDescending(a => a.StatusChangedDate).FirstOrDefault(), response);

            return response;
        }

        /// <summary>
        /// Gets the child detailed view data internal.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The child Detailed view Data Response.</returns>
        internal async Task<ChildDetailedViewDataResponseModel> GetChildDetailedViewDataInternal(ChildDetailedViewDataRequestModel request)
        {
            var response = new ChildDetailedViewDataResponseModel();

            var fundingStreamAndPeriods = new List<string> { $"{request.FundingStreamCode}-{request.FundingPeriodCode}" };

            ChildSearchApiRequestModel childRequest = new(true, request.UkprnFromRoute!)
            {
                StatusChangedDateOnly = request.PublicationDate,
            };

            var childFundingData = await this.childApiClientServices.SearchChild(childRequest, false, fundingStreamAndPeriods);

            var childFundingDatum = childFundingData.Where(a => request.UkprnFromLoggedInUser == request.UkprnFromRoute || a.ParentInfo?.Any(b => b.ParentUKPRN == request.UkprnFromLoggedInUser) == true).FirstOrDefault();

            if (childFundingDatum != null)
            {
                response.HasFundingDataExists = true;
                response.IsIndicative = childFundingDatum.IsIndicative ?? false;
                request.IsALoggedInView = true;
                request.FundingViewType = FundingViewType.ViewData;
                request.IsCurrentYear = true;
                request.SchemaVersion = childFundingDatum.SchemaVersion;
                request.TemplateVersion = childFundingDatum.TemplateVersion;
                request.ProviderFundingData = LoggedInChildModelExtensions.GetProviderFundingApiSearchResponse(childFundingDatum);
                request.FundingViewScope = childFundingDatum.IsIndicative == true ? FundingViewScope.LoggedInIndicativeProvider : FundingViewScope.LoggedInProvider;


                ChildSearchApiRequestModel latestFundingPeriodRequest = new(true, request.UkprnFromRoute!)
                {
                    FundingStreamPeriods = await this.fundingStreamSettingsServices.GetEmailEnabledFundingStreamPeriod(),
                };
                ChildSearchApiRequestModel latestStatementRequest = new(false, request.UkprnFromRoute!)
                {
                    FundingStreamPeriods = await this.childApiClientServices.LatestFundingPeriod(latestFundingPeriodRequest),
                };
                request.IsLatestOrFinalFundingForYear = await this.childApiClientServices.IsLatestStatement(childFundingDatum.Id!, latestStatementRequest, false);

                // ToDo: CSV logics needs to be revisited.
                request.FundingDocument = null;

                response.FundingViewData = await this.sharedFundingViewServices.GetFundingViewData(request);
                this.SetOrganizationNameAndUrn(childFundingDatum, response);

                await this.AddUserVisitedInfo(request.UserId!, childFundingDatum.Id!, childFundingDatum.StatusChangedDate ?? DateTime.Today, request.FundingStreamConfig);
            }

            return response;
        }

        /// <summary>
        /// Gets the child history view data internal.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <returns>The Child History View Data Response Model.</returns>
        internal async Task<ChildHistoryViewDataResponseModel> GetChildHistoryViewDataInternal(ChildHistoryViewDataRequestModel request, ChildHistoryViewDataResponseModel response)
        {
            List<string> validAllocationHistoryFundingStreamPeriodCodes = new List<string>();
            var fundingPeriod = request.FundingPeriodCode.Substring(request.FundingPeriodCode.Length - 4);
            var fundingYearTypeCode = request.FundingPeriodCode.Substring(0, 2);
            var fundingPeriodFromYearShort = int.Parse(fundingPeriod.Substring(0, 2));
            var fundingPeriodToYearShort = int.Parse(fundingPeriod.Substring(2, 2));

            string goLiveFromYearString = request.DigitalGoLiveDate?.Year.ToString().Substring(2, 2);
            int.TryParse(goLiveFromYearString, out int goLiveFromYearShort);

            for (int i = fundingPeriodFromYearShort; i >= goLiveFromYearShort && validAllocationHistoryFundingStreamPeriodCodes.Count() < 3; i--)
            {
                validAllocationHistoryFundingStreamPeriodCodes.Add($"{request.FundingStreamCode}-{fundingYearTypeCode}-{i}{i + 1}");
            }

            var childRequest = new ChildSearchApiRequestModel(false, request.UkprnFromRoute!)
            {
                HasIYOToBeRemoved = false,
                DoFindIsLatest = true,
                FundingStreamPeriods = validAllocationHistoryFundingStreamPeriodCodes,
            };
            childRequest.SetSelectFieldsExcept(a => new { a.FundingLines, a.Calculations, });

            var childFundingData = await this.childApiClientServices.SearchChild(childRequest, false);

            childFundingData = childFundingData.Where(a => request.UkprnFromLoggedInUser == request.UkprnFromRoute || a.ParentInfo?.Any(b => b.ParentUKPRN == request.UkprnFromLoggedInUser) == true)
            .OrderByDescending(a => a.FundingStreamPeriod)
            .ThenByDescending(a => a.StatusChangedDate).ToList();

            if (childFundingData.Count > 0)
            {
                var providerFundingData = LoggedInChildModelExtensions.GetProviderFundingApiSearchResponse(childFundingData.ToArray());

                request.IsALoggedInView = true;
                request.FundingViewType = FundingViewType.ViewData;
                request.IsCurrentYear = true;
                request.SchemaVersion = childFundingData.First().SchemaVersion;
                request.TemplateVersion = childFundingData.First().TemplateVersion;
                request.ProviderFundingData = providerFundingData;
                request.FundingViewScope = FundingViewScope.LoggedInProviderHistory;
                request.IsLatestOrFinalFundingForYear = true;
                request.SelectedVarianceOption = VarianceSelectionOption.NoComparison;
                request.PreviousPublicationDate = null;
                response.HasFundingDataExists = true;
                response.FundingViewData = await this.sharedFundingViewServices.GetFundingViewData(request);
                this.EnrichChildHistoryViewData(request, response, childFundingData);
                this.SetOrganizationNameAndUrn(childFundingData.First(), response);
            }

            return response;
        }

        // Get the current and historicfunding stream periods
        internal async Task<List<string>>? GetCurrentAndHistoricFundingStreamPeriods(ChildSearchApiRequestModel childRequest)
        {
            childRequest.FundingStreamPeriods = await this.fundingStreamSettingsServices.GetEmailEnabledFundingStreamPeriod();

            var getLatestChildFundingPeriod = await this.childApiClientServices.LatestFundingPeriod(childRequest);

            var fundingPeriod = string.Empty;
            var fundingStreamCode = string.Empty;
            var fundingStreamAndPeriods = new List<string>();

            foreach (var fundingStreamPeriod in getLatestChildFundingPeriod)
            {
                int index = fundingStreamPeriod.IndexOf('-');
                if (index > 0)
                {
                    fundingPeriod = fundingStreamPeriod.Length > 4 ? fundingStreamPeriod.Substring(index + 1) : null;
                    fundingStreamCode = fundingStreamPeriod.Substring(0, index);
                }

                var fundingStream = await this.fundingStreamSettingsServices.GetFundingStream(fundingStreamCode!);

                var digitalStatementsGoLiveDateStr = fundingStream?.SettingValues.FirstOrDefault(setting => setting.Setting.SettingName == SettingName.DigitalStatementsGoLiveDate)?.Value;

                DateTime? digitalStatementsGoLiveDate = null;

                if (digitalStatementsGoLiveDateStr != null && DateTime.TryParseExact(
                    digitalStatementsGoLiveDateStr,
                    DateConstants.SettingDateFormat,
                    DateConstants.EnGbCultureInfo,
                    DateTimeStyles.AdjustToUniversal,
                    out var date))
                {
                    digitalStatementsGoLiveDate = date;
                }

                fundingStreamAndPeriods = CurrentAndHistoricFundingStreamPeriodsHelper.GetCurrentAndHistoricFundingStreamPeriods(fundingPeriod, fundingStreamCode, digitalStatementsGoLiveDate, fundingStreamPeriod);
            }

            return fundingStreamAndPeriods;
        }

        // Comparison Methods

        /// <summary>
        /// Enriches the child comparison details in Child Detailed Request object.
        /// </summary>
        /// <param name="childDetailedViewDataRequestModel">The child detailed view data request model.</param>
        private async Task EnrichChildComparisonDetails(ChildDetailedViewDataRequestModel childDetailedViewDataRequestModel)
        {
            var childComparisonResponses = await this.childApiClientServices.GetChildComparison(new ChildComparisonRequest
            {
                ChildUKPRN = childDetailedViewDataRequestModel.UkprnFromRoute!,
                CurrentFundingStreamPeriodCode = childDetailedViewDataRequestModel.FundingStreamCode + "-" + childDetailedViewDataRequestModel.FundingPeriodCode,
                StatusChangedDateOnly = childDetailedViewDataRequestModel.PublishedDate!,
                FundingStreamCode = childDetailedViewDataRequestModel.FundingStreamCode,
                ParentUKPRN = childDetailedViewDataRequestModel.UkprnFromLoggedInUser != childDetailedViewDataRequestModel.UkprnFromRoute ? childDetailedViewDataRequestModel.UkprnFromLoggedInUser : null,
            });

            ChildComparisonResponse? selectedChildComparisonResponse;

            if (childDetailedViewDataRequestModel.SelectedVarianceOption == VarianceSelectionOption.NoComparison)
            {
                selectedChildComparisonResponse = null;
            }
            else if (childDetailedViewDataRequestModel.SelectedVarianceOption == VarianceSelectionOption.FinalStatementPreviousYear)
            {
                selectedChildComparisonResponse = childComparisonResponses.ContainsKey(ComparisonTypeEnum.FinalStatementPreviousYear)
                                            ? childComparisonResponses[ComparisonTypeEnum.FinalStatementPreviousYear]
                                            : childComparisonResponses.ContainsKey(ComparisonTypeEnum.PreviousStatementCurrentYear)
                                            ? childComparisonResponses[ComparisonTypeEnum.PreviousStatementCurrentYear]
                                            : null;
            }
            else if (childDetailedViewDataRequestModel.SelectedVarianceOption == VarianceSelectionOption.PreviousStatementCurrentYear)
            {
                selectedChildComparisonResponse = childComparisonResponses.ContainsKey(ComparisonTypeEnum.PreviousStatementCurrentYear)
                                            ? childComparisonResponses[ComparisonTypeEnum.PreviousStatementCurrentYear]
                                            : childComparisonResponses.ContainsKey(ComparisonTypeEnum.FinalStatementPreviousYear)
                                            ? childComparisonResponses[ComparisonTypeEnum.FinalStatementPreviousYear]
                                            : null;
            }
            else
            {
                selectedChildComparisonResponse = null;
            }

            if (selectedChildComparisonResponse != null && selectedChildComparisonResponse.LoggedInChildAzSearchModel != null)
            {
                selectedChildComparisonResponse.LoggedInChildAzSearchModel.FundingVersion = "0_0";
                selectedChildComparisonResponse.LoggedInChildAzSearchModel.FundingVersionInt = "0";
            }

            childDetailedViewDataRequestModel.SelectedVarianceOption = selectedChildComparisonResponse?.ComparisonType == ComparisonTypeEnum.PreviousStatementCurrentYear
                                                                            ? VarianceSelectionOption.PreviousStatementCurrentYear
                                                                            : selectedChildComparisonResponse?.ComparisonType == ComparisonTypeEnum.FinalStatementPreviousYear
                                                                            ? VarianceSelectionOption.FinalStatementPreviousYear
                                                                            : VarianceSelectionOption.NoComparison;

            childDetailedViewDataRequestModel.PreviousPublicationDate = selectedChildComparisonResponse?.StatusChangedDateOnly.Date;
            childDetailedViewDataRequestModel.PreviousProviderFundingData = selectedChildComparisonResponse?.LoggedInChildAzSearchModel != null ? LoggedInChildModelExtensions.GetProviderFundingApiSearchResponse(selectedChildComparisonResponse.LoggedInChildAzSearchModel) : null;

            childDetailedViewDataRequestModel.ComparisonOptions = childComparisonResponses.ToDictionary(a => a.Key.ConvertToVarianceSelectionOption(), a => a.Value.StatusChangedDateOnly);
        }

        // Shared Methods
        private async Task AddUserVisitedInfo(string userId, string id, DateTime statusChangeDate, ViewYourFunding.Services.Models.FundingStream? fundingStream)
        {
            var digitalStatementsGoLiveDateStr = fundingStream?.SettingValues.FirstOrDefault(setting => setting.Setting.SettingName == SettingName.DigitalStatementsGoLiveDate)?.Value;

            DateTime? digitalStatementsGoLiveDate = null;

            if (digitalStatementsGoLiveDateStr != null && DateTime.TryParseExact(
                            digitalStatementsGoLiveDateStr,
                            DateConstants.SettingDateFormat,
                            DateConstants.EnGbCultureInfo,
                            DateTimeStyles.AdjustToUniversal,
                            out var date))
            {
                digitalStatementsGoLiveDate = date;
            }

            if (statusChangeDate > digitalStatementsGoLiveDate)
            {
                await this.userCountApiClientServices.AddUserVisitedInfo(userId, id);
            }
        }

        private async Task<StatementVisitInfoEnum> GetStatementVisitInfo(string userId, LoggedInChildModel childFundingDatum, ViewYourFunding.Services.Models.FundingStream? fundingStream)
        {
            var digitalStatementsGoLiveDateStr = fundingStream?.SettingValues.FirstOrDefault(setting => setting.Setting.SettingName == SettingName.DigitalStatementsGoLiveDate)?.Value;

            DateTime? digitalStatementsGoLiveDate = null;

            if (digitalStatementsGoLiveDateStr != null && DateTime.TryParseExact(
                            digitalStatementsGoLiveDateStr,
                            DateConstants.SettingDateFormat,
                            DateConstants.EnGbCultureInfo,
                            DateTimeStyles.AdjustToUniversal,
                            out var date))
            {
                digitalStatementsGoLiveDate = date;
            }

            if (childFundingDatum.StatusChangedDate < digitalStatementsGoLiveDate)
            {
                return StatementVisitInfoEnum.PreGoLive;
            }

            var hasUserVisited = await this.userCountApiClientServices.HasUserVisited(userId, childFundingDatum.Id!);

            if (!hasUserVisited)
            {
                if (childFundingDatum.StatementType == "New")
                {
                    return StatementVisitInfoEnum.NewUnread;
                }

                return StatementVisitInfoEnum.UpdatedUnread;
            }

            return StatementVisitInfoEnum.Read;
        }

        private async Task<bool> HasUserHavePermissionToViewStatement(string currentUserUkprn, string ukprnFromRoute)
        {
            if (currentUserUkprn == ukprnFromRoute)
            {
                return true;
            }
            else
            {
                var fundingperiodcodes = await this.fundingStreamSettingsServices.GetEmailEnabledFundingStreamPeriod();
                var isParent = await this.parentApiClientServices.IsParent(currentUserUkprn, false, fundingperiodcodes);
                if (isParent)
                {
                    var isMyChild = await this.parentApiClientServices.IsMyChild(currentUserUkprn, ukprnFromRoute, false, fundingperiodcodes);

                    if (isMyChild)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void SetOrganizationNameAndUrn(LoggedInChildModel? childModel, ViewDataResponseModelBase response)
        {
            if (childModel != null)
            {
                response.OrganizationUrn = childModel.ProviderUrn;
                response.OrganizationName = childModel.OrganisationName;
            }
        }

        // Validation Methods
        private ChildSummaryViewDataResponseModel ValidateChildSummaryViewDataRequestModel(ChildSummaryViewDataRequestModel request)
        {
            var response = new ChildSummaryViewDataResponseModel();

            try
            {
                Guard.Against.ValidUkrpn(request.UkprnFromLoggedInUser);
                Guard.Against.NullOrWhiteSpace(request.UserId);

                response.IsValidUrl = true;
                response.HasUserHaveRightAccess = true;
            }
            catch (Exception ex)
            {
                this.loggerService.LogError($"Invalid data provided in Child Summary Page Request. Error Message: {ex.Message}");
            }

            return response;
        }

        private async Task<ChildDetailedViewDataResponseModel> ValidateAndEnrichChildDetailedViewDataRequestModel(ChildDetailedViewDataRequestModel request)
        {
            ChildDetailedViewDataResponseModel response = new ChildDetailedViewDataResponseModel();

            try
            {
                Guard.Against.ValidUkrpn(request.UkprnFromRoute, nameof(request.UkprnFromRoute));
                Guard.Against.ValidUkrpn(request.UkprnFromLoggedInUser, nameof(request.UkprnFromLoggedInUser));
                Guard.Against.NullOrWhiteSpace(request.FundingStreamNamePathPart, nameof(request.FundingStreamNamePathPart));
                Guard.Against.NullOrWhiteSpace(request.PublishedDate, nameof(request.PublishedDate));
                Guard.Against.NullOrOutOfRange(request.YearFrom, nameof(request.YearFrom), 2010, 3000);

                var yearFrom = request.YearFrom;

                Guard.Against.NullOrOutOfRange(request.YearTo, nameof(request.YearTo), yearFrom + 1, yearFrom + 1);

                var fundingStreamCodeAndNames = await this.fundingStreamSettingsServices.GetFundingStreamCodeAndName(false);

                var fundingStreamCodeAndName = fundingStreamCodeAndNames.FirstOrDefault(a => a.Value.ToUIPathComponent().Equals(request.FundingStreamNamePathPart));

                if (fundingStreamCodeAndName.Key == null)
                {
                    throw new ArgumentException("Invalid Funding Stream name provided.");
                }

                var fundingStream = await this.fundingStreamSettingsServices.GetFundingStream(fundingStreamCodeAndName.Key);

                var yearTypeCode = FundingPeriodHelper.GetYearSettingCode(fundingStream?.SettingValues);
                var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(request.YearFrom, request.YearTo, yearTypeCode);
                var publication = fundingStream?.GetLatestPublication(fundingPeriodCode);

                if (publication is null)
                {
                    throw new ArgumentException($"No publication found for the given Funding Stream: {fundingStreamCodeAndName.Key} and Funding Period: {fundingPeriodCode}.");
                }

                request.FundingStreamCode = fundingStreamCodeAndName.Key;
                request.FundingPeriodCode = fundingPeriodCode;
                request.PublicationDate = request.PublishedDate.ToRouteParameterDate();
                request.FundingStreamConfig = await this.fundingStreamSettingsServices.GetFundingStream(fundingStreamCodeAndName.Key);

                request.PublicationUiModelVersion = publication!.UIModelVersion;
            }
            catch (Exception ex)
            {
                this.loggerService.LogError($"Validation error in Child Detailed Page Request: {ex.Message}");
                return response;
            }

            response.IsValidUrl = true;
            response.HasUserHaveRightAccess = await this.HasUserHavePermissionToViewStatement(request.UkprnFromLoggedInUser, request.UkprnFromRoute);

            return response;
        }

        private async Task<ChildHistoryViewDataResponseModel> ValidateChildHistoryViewDataRequest(ChildHistoryViewDataRequestModel request)
        {
            var response = new ChildHistoryViewDataResponseModel();

            try
            {
                Guard.Against.ValidUkrpn(request.UkprnFromRoute);
                Guard.Against.ValidUkrpn(request.UkprnFromLoggedInUser);
                Guard.Against.NullOrWhiteSpace(request.FundingStreamNamePathPart);

                var fundingStreamCodeAndNames = await this.fundingStreamSettingsServices.GetFundingStreamCodeAndName(false);

                var fundingStreamCodeAndName = fundingStreamCodeAndNames.FirstOrDefault(a => a.Value.ToUIPathComponent().Equals(request.FundingStreamNamePathPart));

                if (fundingStreamCodeAndName.Key == null)
                {
                    throw new ArgumentException("Invalid Funding Stream name provided.");
                }

                var fundingStream = (await this.fundingStreamSettingsServices.GetFundingStream(fundingStreamCodeAndName.Key)) ?? throw new ArgumentException("Funding Stream not found.");
                var publication = fundingStream.GetLatestPublication() ?? throw new ArgumentException($"Publication not found for given Funding Stream Code {fundingStreamCodeAndName.Key}.");

                var digitalStatementsGoLiveDateStr = fundingStream?.SettingValues.FirstOrDefault(setting => setting.Setting.SettingName == SettingName.DigitalStatementsGoLiveDate)?.Value;

                DateTime? digitalStatementsGoLiveDate = null;

                if (digitalStatementsGoLiveDateStr != null && DateTime.TryParseExact(
                    digitalStatementsGoLiveDateStr,
                    DateConstants.SettingDateFormat,
                    DateConstants.EnGbCultureInfo,
                    DateTimeStyles.AdjustToUniversal,
                    out var date))
                {
                    digitalStatementsGoLiveDate = date;
                }

                ChildSearchApiRequestModel latestFundingPeriodRequest = new(true, request.UkprnFromRoute!)
                {
                    FundingStreamPeriods = await this.fundingStreamSettingsServices.GetEmailEnabledFundingStreamPeriod(),
                };
                List<string> latestFundingStreamPeriod = await this.childApiClientServices.LatestFundingPeriod(latestFundingPeriodRequest);

                request.FundingStreamCode = fundingStreamCodeAndName.Key;
                request.FundingPeriodCode = latestFundingStreamPeriod.FirstOrDefault().Substring(latestFundingStreamPeriod.FirstOrDefault().Length - 7);
                request.DigitalGoLiveDate = digitalStatementsGoLiveDate;
                request.FundingStreamConfig = await this.fundingStreamSettingsServices.GetFundingStream(fundingStreamCodeAndName.Key);

                request.PublicationUiModelVersion = publication.UIModelVersion;
            }
            catch (Exception ex)
            {
                this.loggerService.LogError($"Validation error in Child History Page Request: {ex.Message}");
                return response;
            }

            response.IsValidUrl = true;
            response.HasUserHaveRightAccess = await this.HasUserHavePermissionToViewStatement(request.UkprnFromLoggedInUser, request.UkprnFromRoute);

            return response;
        }

        // Child History Related Methods
        private void EnrichChildHistoryViewData(
                                ChildHistoryViewDataRequestModel request,
                                ChildHistoryViewDataResponseModel response,
                                List<LoggedInChildModel> childFundingData)
        {
            var fundingPeriodPublications = new List<KeyValuePair<(int yearFrom, int yearTo), List<ViewYourFunding.Services.Models.Publication>>>();
            var fundingPeriodProviderFundings = new List<KeyValuePair<(int yearFrom, int yearTo), List<ChildHistoryPageRow>>>();

            if (request.FundingStreamConfig!.HistoryIndependentOfPublications)
            {
                fundingPeriodProviderFundings = this.GetHistoryPageRows(childFundingData);
            }
            else
            {
                // ToDo: Logics needs to be revisited for Funding Streams which has (HistoryIndependentOfPublications = False) conditions.
            }

            var pageData = response.FundingViewData?.Components?.FirstOrDefault()?.PageData;

            if (pageData?.ContainsKey("FundingPeriodPublications") == false)
            {
                pageData?.Add("FundingPeriodPublications", fundingPeriodPublications);
            }

            if (pageData?.ContainsKey("FundingPeriodProviderFundings") == false)
            {
                pageData?.Add("FundingPeriodProviderFundings", fundingPeriodProviderFundings);
            }
        }

        private List<KeyValuePair<(int yearFrom, int yearTo), List<ChildHistoryPageRow>>> GetHistoryPageRows(List<LoggedInChildModel> childFundingData)
        {
            var result = new List<KeyValuePair<(int yearFrom, int yearTo), List<ChildHistoryPageRow>>>();

            var maxYearFrom = childFundingData.Max(a => a.YearFrom ?? 0);

            ChildHistoryHelper.UpdateStatementTypeByGroupingScenarios(childFundingData);

            var groupedData = childFundingData.GroupBy(a => (a.YearFrom ?? 0, a.YearTo ?? 0))
                                              .Select(a => new KeyValuePair<(int YearFrom, int YearTo), List<LoggedInChildModel>>(a.Key, a.ToList()));

            foreach (var allFundingsInaPeriod in groupedData)
            {
                var isLatestFundingPeriodCode = allFundingsInaPeriod.Key.YearFrom == maxYearFrom;
                var childHistoryModelInaYear = new List<ChildHistoryPageRow>();
                var model = new ChildHistoryPageRow();

                foreach (var aFunding in allFundingsInaPeriod.Value.OrderByDescending(a => a.FundingVersionInt).ThenBy(a => a.StatusChangedDate))
                {
                    model = ChildHistoryHelper.GetChildHistoryModelInaYear(aFunding, model, isLatestFundingPeriodCode);
                    childHistoryModelInaYear.Add(model);
                }

                result.Add(new KeyValuePair<(int YearFrom, int YearTo), List<ChildHistoryPageRow>>(allFundingsInaPeriod.Key, childHistoryModelInaYear));
            }

            return result;
        }
    }
}
