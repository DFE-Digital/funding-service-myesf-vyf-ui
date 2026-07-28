using MapsterMapper;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataModel = PDS.ViewYourFunding.Repositories.DataModels;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// The view your funding next payment type service.
    /// </summary>
    /// <seealso cref="INextPaymentTypeService" />
    public class NextPaymentTypeService : INextPaymentTypeService
    {
        /// <summary>
        /// The view your funding next payment type repository.
        /// </summary>
        private readonly INextPaymentTypeRepository _viewYourFundingNextPaymentTypeRepository;

        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPaymentTypeService"/> class.
        /// </summary>
        /// <param name="viewYourFundingNextPaymentTypeRepository">The view your funding next payment type repository.</param>
        /// <param name="mapper"> The mapper .</param>
        public NextPaymentTypeService(INextPaymentTypeRepository viewYourFundingNextPaymentTypeRepository, IMapper mapper)
        {
            _viewYourFundingNextPaymentTypeRepository = viewYourFundingNextPaymentTypeRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<NextPaymentType> CreateNextPaymentType(NextPaymentType nextPaymentType)
        {
            var dbNextPaymentType = _mapper.Map<DataModel.NextPaymentType>(nextPaymentType);
            var newNextPaymentType = await _viewYourFundingNextPaymentTypeRepository.CreateNextPaymentType(dbNextPaymentType);
            return _mapper.Map<NextPaymentType>(newNextPaymentType);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteNextPaymentType(NextPaymentType nextPaymentType)
        {
            var dbNextPaymentType = _mapper.Map<DataModel.NextPaymentType>(nextPaymentType);
            return await _viewYourFundingNextPaymentTypeRepository.DeleteNextPaymentType(dbNextPaymentType);
        }

        /// <inheritdoc/>
        public async Task<List<NextPaymentType>> GetAllNextPaymentTypes()
        {
            var nextPaymentTypes = await _viewYourFundingNextPaymentTypeRepository.GetAllNextPaymentTypes();
            return _mapper.Map<List<NextPaymentType>>(nextPaymentTypes);
        }

        /// <inheritdoc/>
        public async Task<IList<NextPaymentType>> GetNextPaymentTypes(int fundingStreamId)
        {
            var nextPaymentTypes = await _viewYourFundingNextPaymentTypeRepository.GetNextPaymentTypes(fundingStreamId);
            return _mapper.Map<IList<NextPaymentType>>(nextPaymentTypes);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateNextPaymentType(NextPaymentType nextPaymentType)
        {
            var dbNextPaymentType = _mapper.Map<DataModel.NextPaymentType>(nextPaymentType);
            return await _viewYourFundingNextPaymentTypeRepository.UpdateNextPaymentType(dbNextPaymentType);
        }
    }
}
