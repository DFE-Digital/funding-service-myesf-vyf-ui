using AutoMapper;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System.Threading.Tasks;
using DataModel = PDS.ViewYourFunding.Repositories.DataModels;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// The funding stream service.
    /// </summary>
    /// <seealso cref="IFundingStreamService" />
    public class FundingStreamService : IFundingStreamService
    {
        /// <summary>
        /// The funding stream repository.
        /// </summary>
        private readonly IFundingStreamRepository _fundingStreamRepository;

        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingStreamService"/> class.
        /// </summary>
        /// <param name="fundingStreamRepository">The funding stream repository.</param>
        /// <param name="mapper"> The mapper.</param>
        public FundingStreamService(IFundingStreamRepository fundingStreamRepository, IMapper mapper)
        {
            _fundingStreamRepository = fundingStreamRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<FundingStream> CreateFundingStream(FundingStream fundingStream)
        {
            var dbFundingStream = _mapper.Map<DataModel.FundingStream>(fundingStream);
            var newFundingStream = await _fundingStreamRepository.CreateFundingStream(dbFundingStream);
            return _mapper.Map<FundingStream>(newFundingStream);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteFundingStream(FundingStream fundingStream)
        {
            var dbFundingStream = _mapper.Map<DataModel.FundingStream>(fundingStream);
            return await _fundingStreamRepository.DeleteFundingStream(dbFundingStream);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateFundingStream(FundingStream fundingStream)
        {
            var dbFundingStream = _mapper.Map<DataModel.FundingStream>(fundingStream);
            return await _fundingStreamRepository.UpdateFundingStream(dbFundingStream);
        }
    }
}