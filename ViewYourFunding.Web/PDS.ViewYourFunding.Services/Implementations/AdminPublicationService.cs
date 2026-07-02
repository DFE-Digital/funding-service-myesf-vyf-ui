using AutoMapper;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// A class exposing get/set operations for publications in the View Your Funding area that are stored in the SQL database.
    /// </summary>
    public class AdminPublicationService : IAdminPublicationService
    {
        private readonly IPublicationRepository _publicationRepository;
        private readonly IPublicationLayoutRepository _publicationLayoutRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminPublicationService"/> class.
        /// </summary>
        /// <param name="publicationRepository">The publication repository.</param>
        /// <param name="publicationLayoutRepository">The publication layout repository.</param>
        /// <param name="mapper">The mapper.</param>
        public AdminPublicationService(
            IPublicationRepository publicationRepository,
            IPublicationLayoutRepository publicationLayoutRepository,
            IMapper mapper)
        {
            _publicationRepository = publicationRepository;
            _publicationLayoutRepository = publicationLayoutRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<IList<Publication>> GetPublications(int fundingStreamId)
        {
            var publicationLayouts = await _publicationLayoutRepository.GetAllAsync();
            var publications = await _publicationRepository.GetPublications(fundingStreamId);

            foreach (var publication in publications)
            {
                publication.PublicationLayouts = publicationLayouts
                    .Where(publicationLayout => publicationLayout.PublicationId == publication.Id)
                    .ToList();
            }

            var result = _mapper.Map<IList<Publication>>(publications);

            return result;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<Publication>> GetAll()
        {
            var publicationLayouts = await _publicationLayoutRepository.GetAllAsync();
            var publications = await _publicationRepository.GetAllAsync();

            foreach (var publication in publications)
            {
                publication.PublicationLayouts = publicationLayouts
                    .Where(publicationLayout => publicationLayout.PublicationId == publication.Id)
                    .ToList();
            }

            var result = _mapper.Map<IList<Publication>>(publications);
            return result.ToList();
        }

        /// <inheritdoc/>
        public async Task<bool> UpdatePublication(Publication publication)
        {
            var dbPublication = ConvertToRepositoryPublication(publication);
            var originalPublicationLayouts = await _publicationLayoutRepository.GetPublicationLayouts(publication.Id);

            var deletedLayouts = originalPublicationLayouts?.Where(opl =>
                !publication.PublicationLayouts?.Any(npl => opl.FundingViewScope == (int)npl.FundingViewScope && opl.FundingViewType == (int)npl.FundingViewType) != false)
                .ToList();

            var addedLayouts = publication.PublicationLayouts?.Where(npl =>
                !originalPublicationLayouts?
                    .Any(opl => (int)npl.FundingViewScope == opl.FundingViewScope && (int)npl.FundingViewType == opl.FundingViewType) != false)
                .ToList();

            var changedLayouts = originalPublicationLayouts?.Where(opl =>
                publication.PublicationLayouts?
                    .Any(npl => opl.FundingViewScope == (int)npl.FundingViewScope && opl.FundingViewType == (int)npl.FundingViewType && opl.LayoutId != npl.LayoutId) == true)
                .ToList();

            if (deletedLayouts != null)
            {
                foreach (var deletedLayout in deletedLayouts)
                {
                    await _publicationLayoutRepository.DeletePublicationLayout(deletedLayout);
                }
            }

            if (addedLayouts != null)
            {
                foreach (var addedLayout in addedLayouts)
                {
                    var dbPublicationLayout = ConvertToRepositoryPublicationLayout(addedLayout);
                    await _publicationLayoutRepository.CreatePublicationLayout(dbPublicationLayout);
                }
            }

            if (changedLayouts != null)
            {
                foreach (var changedLayout in changedLayouts)
                {
                    var newChangedLayout = publication.PublicationLayouts.First(pl => (int)pl.FundingViewScope == changedLayout.FundingViewScope
                        && (int)pl.FundingViewType == changedLayout.FundingViewType);

                    changedLayout.LayoutId = newChangedLayout.LayoutId;
                    await _publicationLayoutRepository.UpdatePublicationLayout(changedLayout);
                }
            }

            return await _publicationRepository.UpdatePublication(dbPublication);
        }

        /// <inheritdoc/>
        public async Task<bool> DeletePublication(Publication publication)
        {
            var dbPublication = ConvertToRepositoryPublication(publication);

            if (publication.PublicationLayouts != null)
            {
                foreach (var publicationLayout in publication.PublicationLayouts)
                {
                    var dbPublicationLayout = ConvertToRepositoryPublicationLayout(publicationLayout);
                    await _publicationLayoutRepository.DeletePublicationLayout(dbPublicationLayout);
                }
            }

            return await _publicationRepository.DeletePublication(dbPublication);
        }

        /// <inheritdoc/>
        public async Task<Publication> CreatePublication(Publication publication)
        {
            var dbPublication = ConvertToRepositoryPublication(publication);
            var result = await _publicationRepository.CreatePublication(dbPublication);

            if (publication.PublicationLayouts != null)
            {
                foreach (var publicationLayout in publication.PublicationLayouts)
                {
                    var dbPublicationLayout = ConvertToRepositoryPublicationLayout(publicationLayout);
                    await _publicationLayoutRepository.CreatePublicationLayout(dbPublicationLayout);
                }
            }

            var updatedPublication = ConvertToServicePublication(result);

            return updatedPublication;
        }

        #region Private Helpers

        private Publication ConvertToServicePublication(Repositories.DataModels.Publication result)
        {
            return _mapper.Map<Repositories.DataModels.Publication, Publication>(result);
        }

        private Repositories.DataModels.Publication ConvertToRepositoryPublication(Publication publication)
        {
            return _mapper.Map<Publication, Repositories.DataModels.Publication>(publication);
        }

        private Repositories.DataModels.PublicationLayout ConvertToRepositoryPublicationLayout(PublicationLayout publicationLayout)
        {
            return _mapper.Map<PublicationLayout, Repositories.DataModels.PublicationLayout>(publicationLayout);
        }

        #endregion
    }
}