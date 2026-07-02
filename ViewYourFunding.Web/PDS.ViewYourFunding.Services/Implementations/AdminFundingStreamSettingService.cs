using AutoMapper;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// The Admin funding stream setting service class.
    /// </summary>
    /// <seealso cref="IAdminFundingStreamSettingService" />
    public class AdminFundingStreamSettingService : IAdminFundingStreamSettingService
    {
        private readonly ISettingValueRepository _settingValueRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminFundingStreamSettingService"/> class.
        /// </summary>
        /// <param name="settingValueRepository">The setting value repository.</param>
        /// <param name="mapper">The mapper.</param>
        public AdminFundingStreamSettingService(
            ISettingValueRepository settingValueRepository,
            IMapper mapper)
        {
            _settingValueRepository = settingValueRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(SettingValue setting)
        {
            var settingModel = _mapper.Map<Repositories.DataModels.SettingValue>(setting);
            return await _settingValueRepository.UpdateSettingValue(settingModel);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(SettingValue settingValue)
        {
            var dbSettingValue = _mapper.Map<Repositories.DataModels.SettingValue>(settingValue);
            return await _settingValueRepository.DeleteSettingValue(dbSettingValue);
        }

        /// <inheritdoc/>
        public async Task<SettingValue> AddAsync(SettingValue settingValue)
        {
            var dbSettingValue = _mapper.Map<Repositories.DataModels.SettingValue>(settingValue);
            var result = await _settingValueRepository.AddAsync(dbSettingValue);

            return _mapper.Map<SettingValue>(result);
        }

        /// <inheritdoc/>
        public async Task<SettingValue> GetFirstOrDefaultAsync(int settingValueId, int fundingStreamId)
        {
            var result = await _settingValueRepository.GetFirstOrDefaultAsync(
                settingValue =>
                    settingValue.Id == settingValueId && settingValue.FundingStreamId == fundingStreamId,
                $"{nameof(SettingValue.FundingStream)}, {nameof(SettingValue.Setting)}");

            return _mapper.Map<SettingValue>(result);
        }
    }
}
