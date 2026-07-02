using AutoMapper;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// SettingType service.
    /// </summary>
    public class SettingTypeService : ISettingTypeService
    {
        private readonly ISettingTypeRepository _settingTypeRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingTypeService"/> class.
        /// </summary>
        /// <param name="settingTypeRepository">The setting type repository.</param>
        /// <param name="mapper">The mapper.</param>
        public SettingTypeService(
            ISettingTypeRepository settingTypeRepository,
            IMapper mapper)
        {
            _settingTypeRepository = settingTypeRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<IList<SettingType>> GetAllSettingTypes()
        {
            var result = await _settingTypeRepository.GetAllSettingTypes();

            return _mapper.Map<IList<SettingType>>(result);
        }

        /// <inheritdoc/>
        public async Task<SettingType> GetSettingTypeById(int id)
        {
            var result = await _settingTypeRepository.GetSettingTypeById(id);

            return _mapper.Map<SettingType>(result);
        }

        /// <inheritdoc/>
        public async Task<SettingType> CreateSettingType(SettingType settingType)
        {
            var setting = _mapper.Map<Repositories.DataModels.Setting>(settingType);
            var newSettingType = await _settingTypeRepository.CreateSettingType(setting);
            return _mapper.Map<SettingType>(newSettingType);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteSettingType(SettingType settingType)
        {
            var dbSettingType = _mapper.Map<Repositories.DataModels.Setting>(settingType);
            return await _settingTypeRepository.DeleteSettingType(dbSettingType);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateSettingType(SettingType settingType)
        {
            var dbSettingType = _mapper.Map<Repositories.DataModels.Setting>(settingType);
            return await _settingTypeRepository.UpdateSettingType(dbSettingType);
        }

        /// <inheritdoc/>
        public async Task<IList<SettingType>> GetAvailableSettingTypes(IEnumerable<int> excludedSettingTypeIds)
        {
            var result = await _settingTypeRepository
                .GetAllAsync(settingType => settingType.ValuesAreEditable &&
                                            !excludedSettingTypeIds.Contains(settingType.Id));

            return _mapper.Map<IList<SettingType>>(result);
        }
    }
}