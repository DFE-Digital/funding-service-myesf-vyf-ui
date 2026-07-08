namespace PDS.VYF.Services.Implementations.InfraServices.SettingsServices
{
    using AutoMapper;
    using Pds.Core.Logging;
    using PDS.ViewYourFunding.Repositories.Interfaces;
    using PDS.ViewYourFunding.Services.Cache;
    using PDS.ViewYourFunding.Services.Constants;
    using PDS.ViewYourFunding.Services.Helper;
    using PDS.ViewYourFunding.Services.Interfaces;
    using PDS.ViewYourFunding.Services.Models;
    using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;

    /// <summary>
    /// The Funding Stream Settings Services.
    /// </summary>
    /// <seealso cref="PDS.VYF.Services.Abstracts.InfraServices.SettingsServices.IFundingStreamSettingsServices" />
    public class FundingStreamSettingsServices : IFundingStreamSettingsServices
    {
        private readonly ICacheService cachingService;
        private readonly IMapper mapper;
        private readonly ILoggerAdapter<IUserJourneyService> logger;
        private readonly IFundingStreamRepository fundingStreamRepository;
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingStreamSettingsServices" /> class.
        /// </summary>
        /// <param name="cachingService">The caching service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fundingStreamRepository">The funding stream repository.</param>
        public FundingStreamSettingsServices(
                ICacheService cachingService,
                IMapper mapper,
                ILoggerAdapter<IUserJourneyService> logger,
                IFundingStreamRepository fundingStreamRepository)
        {
            this.cachingService = cachingService;
            this.mapper = mapper;
            this.logger = logger;
            this.fundingStreamRepository = fundingStreamRepository;
        }

        /// <summary>
        /// Gets the active funding stream period.
        /// </summary>
        /// <param name="isForParent">if set to <c>true</c> [is for parent].</param>
        /// <returns>ActiveFundingStreamPeriod.</returns>
        public async Task<List<string>> GetLoggedInFundingStreamPeriod(bool isForParent)
        {
            var allFundingStreams = await this.GetFundingStreams();

            return allFundingStreams
                    .Where(a => isForParent ? a.RelevantForOrganisations_LoggedIn : a.RelevantForProviders_LoggedIn)
                    .Select(a =>
                    {
                        var fundingStreamCode = a.FundingStreamCode;
                        var fundingPeriodCode = a.Publications
                                            .OrderByDescending(b => b.PublishedDate)
                                            .FirstOrDefault()
                                            ?.FundingPeriodCode ?? string.Empty;

                        return $"{fundingStreamCode}-{fundingPeriodCode}";
                    })
                    .ToList();
        }

        public async Task<List<string>> GetEmailEnabledFundingStreamPeriod()
        {
            var activeFundingStreams = await this.GetFundingStreams();

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
                       .ToList() ?? new List<string>();

                   var result = new List<string>();

                   if (!string.IsNullOrWhiteSpace(fs.FundingStreamCode))
                   {
                       foreach (var fp in fundingPeriods)
                       {
                           result.Add(fs.FundingStreamCode + "-" + fp);
                       }
                   }

                   return string.Join(",", result);
               }).ToList();
        }

        public async Task<Dictionary<string, FundingStream>> GetLoggedInFundingStreams(bool isForParent)
        {
            var allFundingStreams = await this.GetFundingStreams();

            return allFundingStreams
                    .Where(a => isForParent ? a.RelevantForOrganisations_LoggedIn : a.RelevantForProviders_LoggedIn)
                    .ToDictionary(a => a.FundingStreamCode, a => a);
        }

        public async Task<Publication?> GetLatestPublication(string fundingStreamCode, string fundingPeriodCode)
        {
            var allFundingStreams = await this.GetFundingStreams();

            return allFundingStreams
                    .Where(a => a.FundingStreamCode.Equals(fundingStreamCode, StringComparison.InvariantCultureIgnoreCase))
                    .SelectMany(a => a.Publications)
                    .Where(a => a.FundingPeriodCode.Equals(fundingPeriodCode, StringComparison.InvariantCultureIgnoreCase))
                    .OrderByDescending(a => a.PublishedDate)
                    .FirstOrDefault();
        }

        public async Task<FundingStream?> GetFundingStream(string fundingStreamCode)
        {
            var allFundingStreams = await this.GetFundingStreams();

            return allFundingStreams
                    .Where(a => a.FundingStreamCode.Equals(fundingStreamCode, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
        }

        public async Task<Dictionary<string, string>> GetFundingStreamCodeAndName(bool isForParent)
        {
            var allFundingStreams = await this.GetFundingStreams();

            return allFundingStreams
                    .Where(a => isForParent ? a.RelevantForOrganisations_LoggedIn : a.RelevantForProviders_LoggedIn)
                    .ToDictionary(a => a.FundingStreamCode, a => a.FundingStreamName);
        }

        private async Task<IList<FundingStream>> GetFundingStreams()
        {
            const string cacheKey = $"{nameof(FundingStreamSettingsServices)}-{nameof(this.GetFundingStreams)}";

            return await this.cachingService.AddOrGetExistingResultAsync(
                cacheKey,
                this.GetFundingStreamsFromCacheOrDb,
                CacheExpirationPolicy.Absolute,
                TimeSpan.FromMinutes(60));
        }

        private async Task<IList<FundingStream>> GetFundingStreamsFromCacheOrDb()
        {
            this.semaphoreSlim.Wait();
            try
            {
                var cacheKey = $"{nameof(FundingStreamSettingsServices)}-{nameof(this.GetFundingStreamsFromCacheOrDb)}";

                this.logger?.LogInformation("GetFundingStreamsFromDb");

                return await this.cachingService.AddOrGetExistingResultAsync(
                    cacheKey,
                    this.GetFundingStreamsFromDb,
                    CacheExpirationPolicy.Absolute,
                    TimeSpan.FromSeconds(60));
            }
            finally
            {
                this.semaphoreSlim.Release();
            }
        }

        private async Task<IList<FundingStream>> GetFundingStreamsFromDb()
        {
            this.logger?.LogInformation("GetFundingStreamsFromDb");

            var fundingStreams = await this.fundingStreamRepository.GetAllFundingStreams(true, FetchDataHelper.AllFetchData);
            var result = this.mapper.Map<List<FundingStream>>(fundingStreams);

            return result;
        }
    }
}
