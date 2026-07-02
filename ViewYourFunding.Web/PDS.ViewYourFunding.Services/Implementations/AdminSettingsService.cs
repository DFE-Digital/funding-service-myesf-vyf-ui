using AutoMapper;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Repositories.Enums;
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
    /// A class exposing get/set operations for settings in the View Your Funding area that are stored in the SQL database.
    /// </summary>
    public class AdminSettingsService : IAdminSettingsService
    {
        private readonly IMapper _mapper;
        private readonly ILoggerAdapter<AdminSettingsService> _logger;
        private readonly ISettingValueRepository _settingValueRepository;
        private readonly IFundingStreamRepository _fundingStreamRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminSettingsService"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="settingsRepository">The setting repository.</param>
        /// <param name="fundingStreamRepository">The funding stream repository.</param>
        public AdminSettingsService(
            IMapper mapper,
            ILoggerAdapter<AdminSettingsService> logger,
            ISettingValueRepository settingsRepository,
            IFundingStreamRepository fundingStreamRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _settingValueRepository = settingsRepository;
            _fundingStreamRepository = fundingStreamRepository;
        }

        /// <inheritdoc/>
        public async Task<IList<SettingValue>> GetSettingsById(int fundingStreamId)
        {
            var fundingStreams = await GetFundingStreamsFromDb(FetchData.SettingValues_Setting);

            var fundingStream = fundingStreams.First(fs => fs.Id == fundingStreamId);
            var settingValues = fundingStream.SettingValues.ToList();

            return settingValues;
        }

        /// <inheritdoc/>
        public async Task<FundingStream> GetFundingStream(string fundingStreamCode, params FetchData[] fetchData)
        {
            var fundingStreams = await GetFundingStreamsFromDb(fetchData);
            var fundingStream = fundingStreams.FirstOrDefault(fs => fs.FundingStreamCode == fundingStreamCode);

            if (fundingStream == null)
            {
                throw new Exception($"No funding stream can be found with code '{fundingStreamCode}'");
            }

            return fundingStream;
        }

        /// <inheritdoc/>
        public async Task<FundingStream> GetFundingStreamById(int fundingStreamId, params FetchData[] fetchData)
        {
            var fundingStreams = await GetFundingStreamsFromDb(fetchData);
            var fundingStream = fundingStreams.FirstOrDefault(fs => fs.Id == fundingStreamId);

            if (fundingStream == null)
            {
                throw new Exception($"No funding stream can be found with Id '{fundingStreamId}'");
            }

            return fundingStream;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<FundingStream>> GetAllFundingStreams(params FetchData[] fetchData)
        {
            var fundingStreams = await GetFundingStreamsFromDb(fetchData);

            return fundingStreams.ToList();
        }

        /// <inheritdoc/>
        public async Task<IList<Publication>> GetPublications(string fundingStreamCode)
        {
            var fundingStreams = await GetFundingStreamsFromDb(FetchData.Publications, FetchData.Publications_PublicationLayouts);
            var fundingStream = fundingStreams.First(fs => fs.FundingStreamCode == fundingStreamCode);
            var publications = fundingStream.Publications.ToList();

            return publications;
        }

        /// <inheritdoc/>
        public async Task<IList<SettingValue>> GetSettings(string fundingStreamCode)
        {
            var fundingStreams = await GetFundingStreamsFromDb(FetchData.SettingValues_Setting);
            var fundingStream = fundingStreams.First(fs => fs.FundingStreamCode == fundingStreamCode);
            var settingValues = fundingStream.SettingValues.ToList();

            return settingValues;
        }

        /// <inheritdoc/>
        public async Task<IList<NextPaymentType>> GetNextPaymentTypes(int fundingStreamId)
        {
            var fundingStream = await GetFundingStreamById(fundingStreamId, FetchData.NextPayments_NextPaymentType, FetchData.NextPaymentTypes_NextPayments);

            return fundingStream.NextPaymentTypes.ToList();
        }

        /// <inheritdoc/>
        public async Task<IList<NextPayment>> GetNextPayments(int fundingStreamId)
        {
            var fundingStream = await GetFundingStreamById(fundingStreamId, FetchData.NextPayments_NextPaymentType, FetchData.NextPaymentTypes_NextPayments);

            return fundingStream.NextPayments.ToList();
        }

        /// <inheritdoc/>
        public async Task<SettingValue> GetSettingById(int fundingStreamId, int settingId)
        {
            var setting = await _settingValueRepository.GetSettingValueById(fundingStreamId, settingId);
            return _mapper.Map<SettingValue>(setting);
        }

        private async Task<IList<FundingStream>> GetFundingStreamsFromDb(params FetchData[] fetchData)
        {
            var fetchDataKey = fetchData != null ? "_" + string.Join(",", fetchData.Select(fd => fd.ToString()).ToArray()) : string.Empty;
            _logger?.LogInformation($"GetFundingStreamsFromDb_fetchProperties{fetchDataKey}");

            var fundingStreams = await _fundingStreamRepository.GetAllFundingStreams(false, fetchData);
            var result = _mapper.Map<List<FundingStream>>(fundingStreams);

            return result;
        }
    }
}