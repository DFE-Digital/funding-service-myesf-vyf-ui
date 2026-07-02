using AutoMapper;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Cache;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// A class exposing get operations for settings in the View Your Funding area that are stored in the SQL database.
    /// </summary>
    public class UserJourneyService : IUserJourneyService
    {
        private readonly ICacheService _cachingService;
        private readonly IMapper _mapper;
        private readonly IFundingStreamRepository _fundingStreamRepository;
        private readonly ILoggerAdapter<IUserJourneyService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserJourneyService"/> class.
        /// </summary>
        /// <param name="cachingService">Caching service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fundingStreamRepository">The funding stream repository.</param>
        public UserJourneyService(
            ICacheService cachingService,
            IMapper mapper,
            ILoggerAdapter<IUserJourneyService> logger,
            IFundingStreamRepository fundingStreamRepository)
        {
            _cachingService = cachingService;
            _mapper = mapper;
            _logger = logger;
            _fundingStreamRepository = fundingStreamRepository;
        }

        /// <inheritdoc/>
        public async Task<FundingStream> GetFundingStream(string fundingStreamCode)
        {
            var fundingStreams = await GetFundingStreams();
            var fundingStream = fundingStreams.FirstOrDefault(fs => fs.FundingStreamCode == fundingStreamCode);

            if (fundingStream == null)
            {
                throw new Exception($"No funding stream can be found with code '{fundingStreamCode}'");
            }

            return fundingStream;
        }

        /// <inheritdoc/>
        public async Task<IList<Publication>> GetPublications(string fundingStreamCode)
        {
            var fundingStreams = await GetFundingStreams();
            var fundingStream = fundingStreams.First(fs => fs.FundingStreamCode == fundingStreamCode);
            var publications = fundingStream.Publications.ToList();

            return publications;
        }

        /// <inheritdoc/>
        public async Task<IList<SettingValue>> GetSettings(string fundingStreamCode)
        {
            var fundingStreams = await GetFundingStreams();
            var fundingStream = fundingStreams.First(fs => fs.FundingStreamCode == fundingStreamCode);
            var settingValues = fundingStream.SettingValues.ToList();

            return settingValues;
        }

        /// <inheritdoc/>
        public async Task<IList<FundingStream>> GetFundingStreams()
        {
            const string cacheKey = nameof(GetFundingStreams);

            return await _cachingService.AddOrGetExistingResultAsync(
                cacheKey,
                GetFundingStreamsFromDb,
                CacheExpirationPolicy.Absolute);
        }

        private async Task<IList<FundingStream>> GetFundingStreamsFromDb()
        {
            _logger?.LogInformation("GetFundingStreamsFromDb");

            var fundingStreams = await _fundingStreamRepository.GetAllFundingStreams(true, FetchDataHelper.AllFetchData);
            var result = _mapper.Map<List<FundingStream>>(fundingStreams);

            return result;
        }
    }
}