using AutoMapper;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Cache;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// Application global setting service.
    /// </summary>
    public class GlobalSettingService : IGlobalSettingService
    {
        private readonly IGlobalSettingRepository _globalSettingRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cachingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalSettingService"/> class.
        /// </summary>
        /// <param name="globalSettingRepository">The global setting repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="cachingService">Cache service.</param>
        public GlobalSettingService(
            IGlobalSettingRepository globalSettingRepository,
            IMapper mapper,
            ICacheService cachingService)
        {
            _globalSettingRepository = globalSettingRepository;
            _mapper = mapper;
            _cachingService = cachingService;
        }

        /// <inheritdoc/>
        public async Task<GlobalSetting> GetFirstOrDefault(int settingTypeId)
        {
            if (settingTypeId <= 0)
            {
                throw new ArgumentException("Setting Id must be greater than 0.");
            }

            var cacheKey = $"{nameof(GlobalSetting)}-{settingTypeId}";

            return await _cachingService.AddOrGetExistingResultAsync(
                cacheKey,
                () => GetSettingFromDB(settingTypeId),
                CacheExpirationPolicy.Sliding);
        }

        /// <inheritdoc/>
        public async Task<GlobalSetting> Get(int globalSettingId)
        {
            if (globalSettingId > 0)
            {
                var setting = await _globalSettingRepository.GetAsync(globalSettingId);
                var result = _mapper.Map<GlobalSetting>(setting);
                return result;
            }

            throw new ArgumentException("Global Setting Id must be greater than 0.");
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveSettingByTypeAsync(int settingTypeId)
        {
            if (settingTypeId > 0)
            {
                return await _globalSettingRepository.RemoveSettingByTypeAsync(settingTypeId);
            }

            throw new ArgumentException("Setting Id must be greater than 0.");
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveAsync(int id)
        {
            if (id > 0)
            {
                return await _globalSettingRepository.RemoveAsync(id) > 0;
            }

            throw new ArgumentException("Id must be greater than 0.");
        }

        /// <inheritdoc/>
        public async Task<GlobalSetting> AddAsync(GlobalSetting globalSetting)
        {
            var dbGlobalSetting = _mapper.Map<Repositories.DataModels.GlobalSetting>(globalSetting);
            return _mapper.Map<GlobalSetting>(await _globalSettingRepository.AddAsync(dbGlobalSetting));
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(GlobalSetting globalSetting)
        {
            var result = _mapper.Map<Repositories.DataModels.GlobalSetting>(globalSetting);
            return await _globalSettingRepository.UpdateAsync(result);
        }

        /// <inheritdoc/>
        public async Task<IList<GlobalSetting>> GetAllGlobalSettings()
        {
            var result = await _globalSettingRepository.GetAllAsync();

            return _mapper.Map<IList<GlobalSetting>>(result
                ?.OrderBy(globalSetting => globalSetting.Type));
        }

        private async Task<GlobalSetting> GetSettingFromDB(int settingTypeId)
        {
            var setting = await _globalSettingRepository.GetFirstOrDefaultAsync(s => s.Type == settingTypeId);
            var result = _mapper.Map<GlobalSetting>(setting);
            return result;
        }
    }
}