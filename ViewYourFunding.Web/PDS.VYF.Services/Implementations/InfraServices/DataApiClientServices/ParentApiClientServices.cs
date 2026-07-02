namespace PDS.VYF.Services.Implementations.InfraServices.DataApiClientServices
{
    using Newtonsoft.Json;
    using PDS.ViewYourFunding.Services.Interfaces;
    using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
    using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
    using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
    using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

    /// <summary>
    /// The Class for Parent Api Client Services.
    /// </summary>
    /// <seealso cref="IParentApiClientServices" />
    public class ParentApiClientServices : IParentApiClientServices
    {
        private readonly IHttpApiService httpApiService;
        private readonly IFundingStreamSettingsServices fundingStreamSettings;
        private const string JsonMediaType = "application/json";

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentApiClientServices"/> class.
        /// </summary>
        /// <param name="httpApiService">httpapi service.</param>
        /// <param name="fundingStreamSettingsServices">Funding stream setting.</param>
        public ParentApiClientServices(
                                    IHttpApiService httpApiService,
                                    IFundingStreamSettingsServices fundingStreamSettingsServices)
        {
            this.httpApiService = httpApiService;
            this.fundingStreamSettings = fundingStreamSettingsServices;
        }

        /// <summary>
        /// Searches the parent.
        /// </summary>
        /// <param name="parentRequest">The parent request.</param>
        /// <param name="setLoggedInFundingStreamPeriods">if set to <c>true</c> [set logged in funding stream periods].</param>
        /// <param name="fundingStreamPeriods">The funding stream periods.</param>
        /// <returns>return parent details.</returns>
        public async Task<List<LoggedInParentModel>> SearchParent(ParentSearchApiRequestModel parentRequest, bool setLoggedInFundingStreamPeriods, List<string>? fundingStreamPeriods = null)
        {
            parentRequest.FundingStreamPeriods = setLoggedInFundingStreamPeriods switch
            {
                true => await this.fundingStreamSettings.GetLoggedInFundingStreamPeriod(true),
                _ when fundingStreamPeriods != null => fundingStreamPeriods,
                _ => parentRequest.FundingStreamPeriods
            };

            return await this.httpApiService.PostRequest<List<LoggedInParentModel>>($"Parent/SearchParent", JsonConvert.SerializeObject(parentRequest), JsonMediaType);
        }

        /// <summary>
        /// Determines whether the specified string ukprn is parent.
        /// </summary>
        /// <param name="strUKPRN">The string ukprn.</param>
        /// <param name="setLoggedInFundingStreamPeriods">if set to <c>true</c> [set logged in funding stream periods].</param>
        /// <param name="fundingStreamPeriod">The funding stream period.</param>
        /// <returns>
        ///   <c>true</c> if the specified string ukprn is parent; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsParent(string strUKPRN, bool setLoggedInFundingStreamPeriods, List<string>? fundingStreamPeriod = null)
        {
            var requestFundingStreamPeriods = setLoggedInFundingStreamPeriods switch
            {
                true => await this.fundingStreamSettings.GetLoggedInFundingStreamPeriod(true),
                _ when fundingStreamPeriod != null => fundingStreamPeriod,
                _ => null
            };

            return await this.httpApiService.PostRequest<bool>($"Parent/IsParent/{strUKPRN}", JsonConvert.SerializeObject(requestFundingStreamPeriods), JsonMediaType);
        }

        /// <summary>
        /// Determines whether [is my child] [the specified parent ukprn].
        /// </summary>
        /// <param name="parentUKPRN">The parent ukprn.</param>
        /// <param name="childUKPRN">The child ukprn.</param>
        /// <param name="setLoggedInFundingStreamPeriods">if set to <c>true</c> [set logged in funding stream periods].</param>
        /// <param name="fundingStreamPeriods">The funding stream periods.</param>
        /// <returns>
        ///   <c>true</c> if [is my child] [the specified parent ukprn]; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsMyChild(string parentUKPRN, string childUKPRN, bool setLoggedInFundingStreamPeriods, List<string>? fundingStreamPeriods = null)
        {
            var requestFundingStreamPeriods = setLoggedInFundingStreamPeriods switch
            {
                true => await this.fundingStreamSettings.GetLoggedInFundingStreamPeriod(true),
                _ when fundingStreamPeriods != null => fundingStreamPeriods,
                _ => null
            };

            return await this.httpApiService.PostRequest<bool>($"Parent/IsMyChild/{parentUKPRN}/{childUKPRN}", JsonConvert.SerializeObject(requestFundingStreamPeriods), JsonMediaType);
        }
    }
}
