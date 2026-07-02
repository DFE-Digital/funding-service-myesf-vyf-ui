using Microsoft.Azure.Cosmos.Linq;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using PDS.ViewYourFunding.Web.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.Publications
{
    /// <summary>
    /// The publication action base class.
    /// </summary>
    public abstract class PublicationActionBase
    {
        /// <summary>
        /// The view your funding publication service.
        /// </summary>
        private readonly IAdminPublicationService _viewYourFundingPublicationService;

        /// <summary>
        /// The funding UI model details service.
        /// </summary>
        private readonly IFundingUiModelDetailsService _fundingUiModelDetailsService;

        /// <summary>
        /// The layout management service.
        /// </summary>
        private readonly ILayoutManagementService _layoutManagementService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationActionBase"/> class.
        /// </summary>
        /// <param name="viewYourFundingPublicationService">The view your funding publication service.</param>
        /// <param name="fundingUiModelDetailsService">The funding UI Model details service.</param>
        /// <param name="layoutManagementService">The layout management service.</param>
        protected PublicationActionBase(
            IAdminPublicationService viewYourFundingPublicationService,
            IFundingUiModelDetailsService fundingUiModelDetailsService,
            ILayoutManagementService layoutManagementService)
        {
            _viewYourFundingPublicationService = viewYourFundingPublicationService;
            _fundingUiModelDetailsService = fundingUiModelDetailsService;
            _layoutManagementService = layoutManagementService;
        }

        /// <summary>
        /// Gets the view your funding publication view model.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="publicationId">The publication identifier.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="getMaxUiAndSpreadsheetVersionNumbers">Get the Max Model versions.</param>
        /// <returns>
        /// The ViewYourFundingPublicationViewModel.
        /// </returns>
        public async Task<PublicationActionViewModel> GetViewYourFundingPublicationViewModel(
            int fundingStreamId,
            int publicationId,
            ActionMode actionMode,
            Publication publication,
            bool getMaxUiAndSpreadsheetVersionNumbers = true)
        {
            if (publication == null)
            {
                return new PublicationActionViewModel();
            }

            var layouts = await GetFundingStreamLayouts(fundingStreamId);

            var viewModel = new PublicationActionViewModel
            {
                FundingPublication = new PublicationViewModel
                {
                    Id = publicationId,
                    FundingStreamId = fundingStreamId,
                    PublishedDateDay = publication.PublishedDate.Day.ToString(),
                    PublishedDateMonth = publication.PublishedDate.Month.ToString(),
                    PublishedDateYear = publication.PublishedDate.Year.ToString(),
                    CutOffDateDay = publication.CutOffDate?.Day.ToString(),
                    CutOffDateMonth = publication.CutOffDate?.Month.ToString(),
                    CutOffDateYear = publication.CutOffDate?.Year.ToString(),
                    FundingPeriodCode = publication.FundingPeriodCode,
                    Description = publication.Description,
                    SpreadsheetModelVersion = publication.SpreadsheetModelVersion,
                    UIModelVersion = publication.UIModelVersion,
                    Status = publication.Status,
                    LayoutUiModels = layouts.Select(MapLayoutUiModel).ToList()
                },
                ActionMode = actionMode,
                FundingStreamId = fundingStreamId,
                FundingStreamName = publication.FundingStream.FundingStreamName
            };

            FlattenRowsToViewModelProperties(viewModel.FundingPublication, publication.PublicationLayouts);

            if (getMaxUiAndSpreadsheetVersionNumbers)
            {
                var maxUiAndSpreadsheetVersionNumbers = await GetMaximumUiAndSpreadsheetVersionNumbers(
                    publication.FundingStream.FundingStreamCode,
                    publication.FundingPeriodCode);

                viewModel.FundingPublication.UIModelMaxVersion = maxUiAndSpreadsheetVersionNumbers.MaximumUiVersion;
                viewModel.FundingPublication.SpreadsheetModelMaxVersion = maxUiAndSpreadsheetVersionNumbers.MaximumSpreadsheetVersion;
            }

            return viewModel;
        }

        /// <summary>
        /// Gets the publication by identifier.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="publicationId">The publication identifier.</param>
        /// <returns>
        /// The Publication.
        /// </returns>
        public async Task<Publication> GetPublicationById(int fundingStreamId, int publicationId)
        {
            var publications = await _viewYourFundingPublicationService.GetPublications(fundingStreamId);
            return publications.FirstOrDefault(publication => publication.Id == publicationId);
        }

        /// <summary>
        /// Gets the maximum UI and spreadsheet version numbers.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <returns>The MaximumUiSpreadsheetVersion.</returns>
        public async Task<MaximumUiSpreadsheetVersion> GetMaximumUiAndSpreadsheetVersionNumbers(string fundingStreamCode, string fundingPeriodCode)
        {
            var getMaximumSpreadSheetVersionEndPoint = $"{FundingConstants.Route_GetMaxUIAndSpreadsheetVersionNumbers}?fundingStreamCode={fundingStreamCode}&fundingPeriodCode={fundingPeriodCode}";
            return await _fundingUiModelDetailsService.GetMaximumUiAndSpreadsheetVersionNumbersByUrl(getMaximumSpreadSheetVersionEndPoint);
        }

        /// <summary>
        /// Gets the funding view type.
        /// </summary>
        /// <param name="fundingViewType">The funding view type.</param>
        /// <returns>The funding view type (e.g. ViewData).</returns>
        public FundingViewType GetFundingViewType(string fundingViewType)
        {
            Enum.TryParse<FundingViewType>(fundingViewType, out var result);

            return result;
        }

        /// <summary>
        /// Gets the funding view scope.
        /// </summary>
        /// <param name="fundingViewScope">The funding view scope.</param>
        /// <returns>The funding view scope (e.g. National).</returns>
        public FundingViewScope GetFundingViewScope(string fundingViewScope)
        {
            Enum.TryParse<FundingViewScope>(fundingViewScope, out var result);

            return result;
        }

        /// <summary>
        /// Maps the layout UI model.
        /// </summary>
        /// <param name="layoutModel">The layout model.</param>
        /// <returns>The mapped layout model.</returns>
        public LayoutUiModel MapLayoutUiModel(LayoutModel layoutModel)
        {
            return new LayoutUiModel
            {
                LayoutName = layoutModel.LayoutName,
                LayoutId = layoutModel.Id,
                LastModifiedDateTime = layoutModel.LastModifiedDateTime.ConvertUtcDateTimeToGmtDateTime(),
                FundingViewType = GetFundingViewType(layoutModel.FundingViewType),
                FundingViewScope = GetFundingViewScope(layoutModel.FundingViewScope)
            };
        }

        /// <summary>
        /// Gets the funding stream layouts.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <returns>A list of layout models.</returns>
        public async Task<IReadOnlyList<LayoutModel>> GetFundingStreamLayouts(int fundingStreamId)
        {
            var filters = new List<Expression<Func<LayoutModel, bool>>>
            {
                layout => layout.FundingStreamId == fundingStreamId && (layout.DeletedDateTime == null || !layout.DeletedDateTime.IsDefined())
            };

            var layouts = await _layoutManagementService.GetAllLayoutsAsync(filters);
            return layouts;
        }

        /// <summary>
        /// Flatten rows to view model propertis.
        /// </summary>
        /// <param name="flattenedModel">The view model.</param>
        /// <param name="publicationLayouts">The publication layouts to flatten.</param>
        protected void FlattenRowsToViewModelProperties(PublicationViewModel flattenedModel, List<PublicationLayout> publicationLayouts)
        {
            if (publicationLayouts == null)
            {
                return;
            }

            var vdLayouts = publicationLayouts.Where(pl => pl.FundingViewType == FundingViewType.ViewData);
            var ssLayouts = publicationLayouts.Where(pl => pl.FundingViewType == FundingViewType.Spreadsheet);

            flattenedModel.LocalAuthorityHistoryLayoutId = vdLayouts.FirstOrDefault(pl => pl.FundingViewScope == FundingViewScope.OrganisationHistory)?.LayoutId;
            flattenedModel.LocalAuthorityHistorySingleYearLayoutId = vdLayouts.FirstOrDefault(pl => pl.FundingViewScope == FundingViewScope.OrganisationHistorySingleYear)?.LayoutId;
            flattenedModel.LocalAuthoritySummaryLayoutId = vdLayouts.FirstOrDefault(pl => pl.FundingViewScope == FundingViewScope.OrganisationSummary)?.LayoutId;
            flattenedModel.LocalAuthorityFundingBreakdownLayoutId = vdLayouts.FirstOrDefault(pl => pl.FundingViewScope == FundingViewScope.Organisation)?.LayoutId;
            flattenedModel.ProviderFundingBreakdownLayoutId = vdLayouts.FirstOrDefault(pl => pl.FundingViewScope == FundingViewScope.Provider)?.LayoutId;
            flattenedModel.ProviderSummaryLayoutId = vdLayouts.FirstOrDefault(pl => pl.FundingViewScope == FundingViewScope.ProviderSummary)?.LayoutId;
            flattenedModel.ProviderHistoryLayoutId = vdLayouts.FirstOrDefault(pl => pl.FundingViewScope == FundingViewScope.ProviderHistory)?.LayoutId;
            flattenedModel.ProviderHistorySingleYearLayoutId = vdLayouts.FirstOrDefault(pl => pl.FundingViewScope == FundingViewScope.ProviderHistorySingleYear)?.LayoutId;
            flattenedModel.LocalAuthoritySpreadsheetLayoutId = ssLayouts.FirstOrDefault(pl => pl.FundingViewScope == FundingViewScope.Organisation)?.LayoutId;
            flattenedModel.ProviderSpreadsheetLayoutId = ssLayouts.FirstOrDefault(pl => pl.FundingViewScope == FundingViewScope.Provider)?.LayoutId;
        }

        /// <summary>
        /// Expand a view model back to publication layouts.
        /// </summary>
        /// <param name="flattenedModel">The flattened view model.</param>
        /// <param name="publication">The publication to update.</param>
        protected void ExpandViewModelToRows(PublicationViewModel flattenedModel, Publication publication)
        {
            ExpandViewModelToRow(
                flattenedModel.LocalAuthorityFundingBreakdownLayoutId,
                publication,
                flattenedModel.Id,
                FundingViewType.ViewData,
                FundingViewScope.Organisation);

            ExpandViewModelToRow(
                flattenedModel.LocalAuthorityHistoryLayoutId,
                publication,
                flattenedModel.Id,
                FundingViewType.ViewData,
                FundingViewScope.OrganisationHistory);

            ExpandViewModelToRow(
                flattenedModel.LocalAuthorityHistorySingleYearLayoutId,
                publication,
                flattenedModel.Id,
                FundingViewType.ViewData,
                FundingViewScope.OrganisationHistorySingleYear);

            ExpandViewModelToRow(
                flattenedModel.LocalAuthoritySpreadsheetLayoutId,
                publication,
                flattenedModel.Id,
                FundingViewType.Spreadsheet,
                FundingViewScope.Organisation);

            ExpandViewModelToRow(
                flattenedModel.LocalAuthoritySummaryLayoutId,
                publication,
                flattenedModel.Id,
                FundingViewType.ViewData,
                FundingViewScope.OrganisationSummary);

            ExpandViewModelToRow(
                flattenedModel.ProviderFundingBreakdownLayoutId,
                publication,
                flattenedModel.Id,
                FundingViewType.ViewData,
                FundingViewScope.Provider);

            ExpandViewModelToRow(
                flattenedModel.ProviderHistoryLayoutId,
                publication,
                flattenedModel.Id,
                FundingViewType.ViewData,
                FundingViewScope.ProviderHistory);

            ExpandViewModelToRow(
                flattenedModel.ProviderHistorySingleYearLayoutId,
                publication,
                flattenedModel.Id,
                FundingViewType.ViewData,
                FundingViewScope.ProviderHistorySingleYear);

            ExpandViewModelToRow(
                flattenedModel.ProviderSpreadsheetLayoutId,
                publication,
                flattenedModel.Id,
                FundingViewType.Spreadsheet,
                FundingViewScope.Provider);

            ExpandViewModelToRow(
                flattenedModel.ProviderSummaryLayoutId,
                publication,
                flattenedModel.Id,
                FundingViewType.ViewData,
                FundingViewScope.ProviderSummary);
        }

        private void ExpandViewModelToRow(string layoutId, Publication publication, int publicationId, FundingViewType fundingViewType, FundingViewScope fundingViewScope)
        {
            if (string.IsNullOrEmpty(layoutId))
            {
                return;
            }

            var publicationLayout = publication.PublicationLayouts?
                .FirstOrDefault(pl => pl.FundingViewType == fundingViewType && pl.FundingViewScope == fundingViewScope);

            if (publicationLayout == null)
            {
                publicationLayout = new PublicationLayout
                {
                    FundingViewType = fundingViewType,
                    FundingViewScope = fundingViewScope,
                    LayoutId = layoutId,
                    PublicationId = publicationId
                };

                if (publication.PublicationLayouts == null)
                {
                    publication.PublicationLayouts = new List<PublicationLayout>();
                }

                publication.PublicationLayouts.Add(publicationLayout);
            }
            else
            {
                publicationLayout.LayoutId = layoutId;
            }
        }
    }
}