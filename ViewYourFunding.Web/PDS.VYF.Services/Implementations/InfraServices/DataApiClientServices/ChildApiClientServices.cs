namespace PDS.VYF.Services.Implementations.InfraServices.DataApiClientServices
{
    using Newtonsoft.Json;
    using PDS.ViewYourFunding.Services.Interfaces;
    using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
    using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
    using PDS.VYF.Services.Enums;
    using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
    using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

    /// <summary>
    /// The Class which implementing the Child API Client Services.
    /// </summary>
    /// <seealso cref="PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices.IChildApiClientServices" />
    public class ChildApiClientServices : IChildApiClientServices
    {
        private readonly IHttpApiService httpApiService;
        private readonly IFundingStreamSettingsServices fundingStreamSettingsServices;
        private const string JsonMediaType = "application/json";

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildApiClientServices"/> class.
        /// </summary>
        /// <param name="httpApiService">The HTTP API service.</param>
        /// <param name="fundingStreamSettingsServices">The funding stream settings services.</param>
        public ChildApiClientServices(
                                    IHttpApiService httpApiService,
                                    IFundingStreamSettingsServices fundingStreamSettingsServices)
        {
            this.httpApiService = httpApiService;
            this.fundingStreamSettingsServices = fundingStreamSettingsServices;
        }

        /// <summary>
        /// Searches the child.
        /// </summary>
        /// <param name="childRequest">The child request.</param>
        /// <param name="setLoggedInFundingStreamPeriods">if set to <c>true</c> [set logged in funding stream period].</param>
        /// <param name="fundingStreamPeriod">The funding stream period.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<List<LoggedInChildModel>> SearchChild(
                                    ChildSearchApiRequestModel childRequest,
                                    bool setLoggedInFundingStreamPeriods,
                                    List<string>? fundingStreamPeriod = null)
        {
            childRequest.FundingStreamPeriods = setLoggedInFundingStreamPeriods switch
            {
                true => await this.fundingStreamSettingsServices.GetLoggedInFundingStreamPeriod(false),
                _ when fundingStreamPeriod != null => fundingStreamPeriod,
                _ => childRequest.FundingStreamPeriods
            };

            return await this.httpApiService.PostRequest<List<LoggedInChildModel>>($"Child/SearchChild", JsonConvert.SerializeObject(childRequest), JsonMediaType);
        }

        /// <summary>
        /// Searches the Latest Funding period code.
        /// </summary>
        /// <param name="childRequest">The child request.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<List<string>> LatestFundingPeriod(
                                    ChildSearchApiRequestModel childRequest)
        {
            return await this.httpApiService.PostRequest<List<string>>($"Child/LatestFundingPeriod", JsonConvert.SerializeObject(childRequest), JsonMediaType);
        }

        /// <summary>
        /// Searches the children of a parent.
        /// </summary>
        /// <param name="parentUKPRN">The parent ukprn.</param>
        /// <param name="childRequest">The child request.</param>
        /// <param name="setLoggedInFundingStreamPeriods">if set to <c>true</c> [set logged in funding stream period].</param>
        /// <param name="fundingStreamPeriod">The funding stream period.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<List<LoggedInChildModel>> SearchChildrenOfAParent(
                                    string parentUKPRN,
                                    ChildSearchApiRequestModel childRequest,
                                    bool setLoggedInFundingStreamPeriods,
                                    List<string>? fundingStreamPeriod = null)
        {
            childRequest.FundingStreamPeriods = setLoggedInFundingStreamPeriods switch
            {
                true => await this.fundingStreamSettingsServices.GetLoggedInFundingStreamPeriod(false),
                _ when fundingStreamPeriod != null => fundingStreamPeriod,
                _ => childRequest.FundingStreamPeriods
            };

            return await this.httpApiService.PostRequest<List<LoggedInChildModel>>($"Child/SearchChildrenOfAParent/{parentUKPRN}", JsonConvert.SerializeObject(childRequest), JsonMediaType);
        }

        public async Task<bool> IsLatestStatement(
                                    string id,
                                    ChildSearchApiRequestModel childRequest,
                                    bool setLoggedInFundingStreamPeriods,
                                    List<string>? fundingStreamPeriod = null)
        {
            childRequest.FundingStreamPeriods = setLoggedInFundingStreamPeriods switch
            {
                true => await this.fundingStreamSettingsServices.GetLoggedInFundingStreamPeriod(false),
                _ when fundingStreamPeriod != null => fundingStreamPeriod,
                _ => childRequest.FundingStreamPeriods
            };

            return await this.httpApiService.PostRequest<bool>($"Child/IsLatestStatement/{id}", JsonConvert.SerializeObject(childRequest), JsonMediaType);
        }

        public async Task<List<string>> GetCurrentChildUkprnsForParent(
                                   string parentUkprn,
                                   List<string>? childIUkprns = null)
        {
            return await this.httpApiService.PostRequest<List<string>>($"Child/GetCurrentChildUkprnsForParent/{parentUkprn}", JsonConvert.SerializeObject(childIUkprns), JsonMediaType);
        }

        /// <summary>
        /// Gets the child comparison.
        /// </summary>
        /// <param name="childComparisonRequest">The child comparison request.</param>
        /// <returns>List of ChildComparisonResponse.</returns>
        public async Task<Dictionary<ComparisonTypeEnum, ChildComparisonResponse>> GetChildComparison(ChildComparisonRequest childComparisonRequest)
            => await this.httpApiService.PostRequest<Dictionary<ComparisonTypeEnum, ChildComparisonResponse>>($"Child/GetChildComparison", JsonConvert.SerializeObject(childComparisonRequest), JsonMediaType);
    }
}
