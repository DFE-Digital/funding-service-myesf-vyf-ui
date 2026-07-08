namespace PDS.VYF.Services.Implementations.InfraServices.SettingsServices
{
    using Ardalis.GuardClauses;
    using AutoMapper;
    using Pds.Core.Logging;
    using PDS.ViewYourFunding.Repositories.Interfaces;
    using PDS.ViewYourFunding.Services.Cache;
    using PDS.ViewYourFunding.Services.Interfaces;
    using PDS.ViewYourFunding.Services.Models;
    using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The Global Settings Service.
    /// </summary>
    /// <seealso cref="PDS.VYF.Services.Abstracts.InfraServices.SettingsServices.IGlobalSettingsService" />
    public class GlobalSettingsService : IGlobalSettingsService
    {
        private readonly ICacheService cachingService;
        private readonly IMapper mapper;
        private readonly ILoggerAdapter<IUserJourneyService> logger;
        private readonly IGlobalSettingRepository globalSettingRepository;

        private SemaphoreSlim semaphoreSlim = new(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalSettingsService"/> class.
        /// </summary>
        /// <param name="cachingService">The caching service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="globalSettingRepository">The global setting repository.</param>
        public GlobalSettingsService(
                ICacheService cachingService,
                IMapper mapper,
                ILoggerAdapter<IUserJourneyService> logger,
                IGlobalSettingRepository globalSettingRepository)
        {
            this.cachingService = cachingService;
            this.mapper = mapper;
            this.logger = logger;
            this.globalSettingRepository = globalSettingRepository;
        }

        /// <summary>
        /// Gets the value of the global setting as a boolean.
        /// </summary>
        /// <param name="settingTypeId">The ID of the setting type.</param>
        /// <returns>
        /// The value of the global setting as a boolean.
        /// </returns>
        public async Task<bool> GetValueAsBool(int settingTypeId)
        {
            var setting = await this.GetFirstOrDefault(settingTypeId);

            if (bool.TryParse(setting?.Value ?? "false", out var value))
            {
                return value;
            }

            return false;
        }

        /// <summary>
        /// Gets the first global setting of the specified type.
        /// </summary>
        /// <param name="settingTypeId">The ID of the setting type.</param>
        /// <returns>
        /// The first global setting of the specified type, or null if not found.
        /// </returns>
        public async Task<GlobalSetting?> GetFirstOrDefault(int settingTypeId)
        {
            Guard.Against.OutOfRange(settingTypeId, nameof(settingTypeId), 1, 10000);

            var allSettings = await this.GetAllGlobalSettings();

            return allSettings?.FirstOrDefault(a => a.Type == settingTypeId);
        }

        private async Task<IList<GlobalSetting>> GetAllGlobalSettings()
        {
            const string cacheKey = $"{nameof(FundingStreamSettingsServices)}-{nameof(this.GetAllGlobalSettings)}";

            return await this.cachingService.AddOrGetExistingResultAsync(
                cacheKey,
                this.GetAllGlobalSettingsFromCacheOrDb,
                CacheExpirationPolicy.Absolute,
                TimeSpan.FromMinutes(60));
        }

        private async Task<IList<GlobalSetting>> GetAllGlobalSettingsFromCacheOrDb()
        {
            this.semaphoreSlim.Wait();
            try
            {
                var cacheKey = $"{nameof(FundingStreamSettingsServices)}-{nameof(this.GetAllGlobalSettingsFromCacheOrDb)}";

                this.logger?.LogInformation("GetAllGlobalSettingsFromCacheOrDb");

                return await this.cachingService.AddOrGetExistingResultAsync(
                    cacheKey,
                    this.GetAllGlobalSettingsFromDb,
                    CacheExpirationPolicy.Absolute,
                    TimeSpan.FromSeconds(60));
            }
            finally
            {
                this.semaphoreSlim.Release();
            }
        }

        private async Task<IList<GlobalSetting>> GetAllGlobalSettingsFromDb()
        {
            this.logger?.LogInformation("GetFundingStreamsFromDb");

            var globalSettings = await this.globalSettingRepository.GetAllAsync();
            var result = this.mapper.Map<List<GlobalSetting>>(globalSettings);

            return result;
        }
    }
}
