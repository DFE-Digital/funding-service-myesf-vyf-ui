using AutoMapper;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataModel = PDS.ViewYourFunding.Repositories.DataModels;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// The view your funding next payment service.
    /// </summary>
    /// <seealso cref="INextPaymentService" />
    public class NextPaymentService : INextPaymentService
    {
        private readonly INextPaymentRepository _nextPaymentRepository;

        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPaymentService"/> class.
        /// </summary>
        /// <param name="nextPaymentRepository">The view your funding next payment repository.</param>
        /// <param name="mapper"> The mapper.</param>
        public NextPaymentService(INextPaymentRepository nextPaymentRepository, IMapper mapper)
        {
            _nextPaymentRepository = nextPaymentRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<NextPayment> CreateNextPayment(NextPayment nextPayment)
        {
            var dbNextPayment = _mapper.Map<DataModel.NextPayment>(nextPayment);
            var newNextPayment = await _nextPaymentRepository.CreateNextPayment(dbNextPayment);
            return _mapper.Map<NextPayment>(newNextPayment);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteNextPayment(NextPayment nextPayment)
        {
            var dbNextPayment = _mapper.Map<DataModel.NextPayment>(nextPayment);
            return await _nextPaymentRepository.DeleteNextPayment(dbNextPayment);
        }

        /// <inheritdoc/>
        public async Task<IList<NextPayment>> GetNextPayments(int fundingStreamId)
        {
            var nextPaymentTypes = await _nextPaymentRepository.GetNextPayments(fundingStreamId);
            return _mapper.Map<IList<NextPayment>>(nextPaymentTypes);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateNextPayment(NextPayment nextPaymentType)
        {
            var dbNextPayment = _mapper.Map<DataModel.NextPayment>(nextPaymentType);
            return await _nextPaymentRepository.UpdateNextPayment(dbNextPayment);
        }
    }
}