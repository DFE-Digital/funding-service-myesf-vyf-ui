namespace PDS.VYF.Services.Implementations.AppServices
{
    using Pds.Core.Logging;
    using PDS.ViewYourFunding.Services.Implementations.FundingView;
    using PDS.ViewYourFunding.Services.Interfaces;
    using PDS.VYF.Services.Abstracts.AppServices;
    using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
    using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
    using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;

    /// <summary>
    /// The Service class for getting Child or Parent Name.
    /// </summary>
    /// <seealso cref="PDS.VYF.Services.Abstracts.AppServices.IChildOrParentNameServices" />
    public class ChildOrParentNameServices : IChildOrParentNameServices
    {
        private readonly ILoggerAdapter<ModelFundingViewService> loggerService;
        private readonly ICacheService cacheService;
        private readonly IChildApiClientServices childApiClientServices;
        private readonly IParentApiClientServices parentApiClientServices;
        private readonly IFundingStreamSettingsServices fundingStreamSettingsServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildOrParentNameServices"/> class.
        /// </summary>
        /// <param name="loggerService">logger services.</param>
        /// <param name="cacheService">cache services.</param>
        /// <param name="childApiClientServices">child api services.</param>
        /// <param name="parentApiClientServices">parent api services.</param>
        /// <param name="fundingStreamSettingsServices">The funding stream settings services.</param>
        public ChildOrParentNameServices(
            ILoggerAdapter<ModelFundingViewService> loggerService,
            ICacheService cacheService,
            IChildApiClientServices childApiClientServices,
            IParentApiClientServices parentApiClientServices,
            IFundingStreamSettingsServices fundingStreamSettingsServices)
        {
            this.loggerService = loggerService;
            this.cacheService = cacheService;
            this.childApiClientServices = childApiClientServices;
            this.parentApiClientServices = parentApiClientServices;
            this.fundingStreamSettingsServices = fundingStreamSettingsServices;
        }

        public async Task<string?> GetParentOrChildName(string ukprn)
        {
            var fundingperiodcodes = await this.fundingStreamSettingsServices.GetEmailEnabledFundingStreamPeriod();

            var isParent = await this.parentApiClientServices.IsParent(ukprn, false, fundingperiodcodes);

            return await this.cacheService.AddOrGetExistingResultAsync(
                                                            $"ChildOrParentName-{ukprn}",
                                                            () => this.GetParentOrChildNameInternal(ukprn, isParent),
                                                            ViewYourFunding.Services.Cache.CacheExpirationPolicy.Sliding,
                                                            TimeSpan.FromMinutes(60));
        }

        // Internal methods (Changed from Private to protected virtual to make Unit Testing easier).

        /// <summary>
        /// </summary>
        /// <param name="ukprn">UKRPN.</param>
        /// <param name="isParent">Isparent or child.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        internal async Task<string?> GetParentOrChildNameInternal(string ukprn, bool isParent)
        {
            string? name = isParent
                ? await this.GetNameFromParentIndex(ukprn)
                : await this.GetNameFromChildIndex(ukprn);

            if (string.IsNullOrWhiteSpace(name))
            {
                name = isParent
                    ? await this.GetNameFromChildIndex(ukprn)
                    : await this.GetNameFromParentIndex(ukprn);
            }

            return name ?? string.Empty;
        }

        private async Task<string?> GetNameFromParentIndex(string ukprn)
        {
            var parentRequest = new ParentSearchApiRequestModel(true, ukprn);
            parentRequest.SetSelectFields(a => new { a.GroupUkprn, a.GroupName, a.StatusChangedDate });
            var allParentData = await this.parentApiClientServices.SearchParent(parentRequest, false);
            return allParentData.OrderByDescending(a => a.StatusChangedDate).FirstOrDefault()?.GroupName;
        }

        private async Task<string?> GetNameFromChildIndex(string ukprn)
        {
            var childRequest = new ChildSearchApiRequestModel(true, ukprn);
            childRequest.HasIYOToBeRemoved = false;
            childRequest.SetSelectFields(a => new { a.OrganisationUkprn, a.OrganisationName, a.StatusChangedDate });
            var allChildData = await this.childApiClientServices.SearchChild(childRequest, false);
            return allChildData.OrderByDescending(a => a.StatusChangedDate).FirstOrDefault()?.OrganisationName;
        }
    }
}
