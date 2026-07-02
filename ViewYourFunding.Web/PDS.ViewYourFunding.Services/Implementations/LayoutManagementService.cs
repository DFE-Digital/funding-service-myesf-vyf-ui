using Microsoft.Azure.Cosmos.Linq;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.CosmosMigrations;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// The layout management service class.
    /// </summary>
    /// <seealso cref="ILayoutManagementService" />
    public class LayoutManagementService : ILayoutManagementService
    {
        // We only want to run migrations once per application lifecycle.
        private static bool migrationsBeenRan = false;

        private readonly ICosmosDbService<LayoutModel> _cosmosDatabaseService;
        private readonly ILoggerAdapter<LayoutManagementService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutManagementService"/> class.
        /// </summary>
        /// <param name="cosmosDatabaseService">The cosmos database service.</param>
        /// <param name="logger">The logger to use.</param>
        public LayoutManagementService(
           ICosmosDbService<LayoutModel> cosmosDatabaseService,
           ILoggerAdapter<LayoutManagementService> logger)
        {
            _cosmosDatabaseService = cosmosDatabaseService;
            _logger = logger;

            // Run any migrations
            if (!migrationsBeenRan)
            {
                Task.Run(() => RunMigrations()).GetAwaiter().GetResult();
                migrationsBeenRan = true;
            }
        }

        /// <inheritdoc />
        public async Task<LayoutModel> GetLayoutAsync(string id)
        {
            return await _cosmosDatabaseService.GetAsync(id);
        }

        /// <inheritdoc />
        public async Task<Guid> AddLayoutAsync(LayoutModel layoutModel)
        {
            var newLayoutId = Guid.NewGuid();
            layoutModel.CreatedDate = DateTime.UtcNow;
            layoutModel.LastModifiedDateTime = DateTime.UtcNow;
            layoutModel.Id = newLayoutId.ToString();

            var result = await _cosmosDatabaseService.AddAsync(layoutModel);

            return result ? newLayoutId : Guid.Empty;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateLayoutAsync(LayoutModel layoutModel)
        {
            layoutModel.LastModifiedDateTime = DateTime.UtcNow;

            return await _cosmosDatabaseService.UpdateAsync(layoutModel.Id, layoutModel);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<LayoutModel>> GetAllLayoutsAsync(
            List<Expression<Func<LayoutModel, bool>>> filters = null)
        {
            var result = await _cosmosDatabaseService.GetAllAsync(filters);

            return result.ToList();
        }

        /// <inheritdoc />
        public async Task<PaginationResult> GetPaginationResult(
            int pageNumber,
            int pageSize,
            IEnumerable<int> fundingStreams,
            IEnumerable<string> fundingViewTypes,
            IEnumerable<string> fundingViewScopes)
        {
            var totalCount = 0;
            int skip = (pageNumber - 1) * pageSize;

            var expressions = new List<Expression<Func<LayoutModel, bool>>>
            {
                layoutModel => layoutModel.DeletedDateTime == null || !layoutModel.DeletedDateTime.IsDefined()
            };

            if (fundingStreams.Any())
            {
                expressions.Add(layoutModel => fundingStreams.Contains(layoutModel.FundingStreamId));
            }

            if (fundingViewTypes.Any())
            {
                expressions.Add(layoutModel => fundingViewTypes.Contains(layoutModel.FundingViewType));
            }

            if (fundingViewScopes.Any())
            {
                expressions.Add(layoutModel => fundingViewScopes.Contains(layoutModel.FundingViewScope));
            }

            totalCount = await _cosmosDatabaseService.GetCountAsync(expressions);

            return new PaginationResult
            {
                TotalCount = totalCount,
                LayoutModels = await _cosmosDatabaseService.GetPagination(skip, pageSize, expressions),
                PaginationDetail = GetPaginationDetails(totalCount, pageSize, pageNumber),
            };
        }

        /// <inheritdoc />
        public async Task<PaginationResult> GetPaginationResultWithFilters(
            int pageNumber,
            int pageSize,
            IEnumerable<int> fundingStreams,
            IEnumerable<string> fundingViewTypes,
            IEnumerable<string> fundingViewScopes)
        {
            Expression<Func<LayoutModel, bool>> expression = layoutModel => layoutModel.DeletedDateTime == null
                || !layoutModel.DeletedDateTime.IsDefined();

            var expressions = new List<Expression<Func<LayoutModel, bool>>>
            {
                expression
            };

            var layouts = await _cosmosDatabaseService.GetAllAsync(expressions);

            var fundingViewTypeFilters = layouts
                .GroupBy(layout => layout.FundingViewType)
                .Select(g => g.Key)
                .ToList();

            var fundingViewScopeFilters = layouts
                .GroupBy(layout => layout.FundingViewScope)
                .Select(g => g.Key)
                .ToList();

            var fundingStreamIds = layouts
                .GroupBy(layout => layout.FundingStreamId)
                .Select(g => g.Key)
                .ToList();

            var paginationResult = await GetPaginationResult(pageNumber, pageSize, fundingStreams, fundingViewTypes, fundingViewScopes);

            paginationResult.FilterOptions = new FilterOptions
            {
                FundingStreamsIds = fundingStreamIds,
                FundingViewTypes = fundingViewTypeFilters,
                FundingViewScopes = fundingViewScopeFilters
            };

            return paginationResult;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteLayoutAsync(LayoutModel layoutModel)
        {
            layoutModel.DeletedDateTime = DateTime.UtcNow;
            return await _cosmosDatabaseService.DeleteAsync(layoutModel.Id, layoutModel);
        }

        private Pagination GetPaginationDetails(int totalRecords, int pageSize, int? pageNumber)
        {
            var pageNo = pageNumber ?? 1;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            (int, int) pageRange = PageRange(pageNo, totalPages);

            return new Pagination
            {
                TotalPages = totalPages,
                PageNumber = pageNo,
                ResultCount = totalRecords,
                PageIndex = pageNo - 1,
                FirstRecordNo = (totalRecords == 0) ? 0 : ((pageNo - 1) * pageSize) + 1,
                LastRecordNo = ((pageNo * pageSize) < totalRecords) ? (pageNo * pageSize) : totalRecords,
                FirstPage = pageRange.Item1,
                LastPage = pageRange.Item2,
                PageSize = pageSize
            };
        }

        private (int startPage, int endPage) PageRange(int pageNumber, int totalPages)
        {
            int start = pageNumber - 2;
            int end = pageNumber + 2;
            if (start <= 0)
            {
                end += (start - 1) * (-1);
                start = 1;
            }

            if (end > totalPages)
            {
                start -= end - totalPages;
                end = totalPages;
            }

            start = start <= 0 ? 1 : start;
            end = end > totalPages ? totalPages : end;
            return (start, end);
        }

        private async Task RunMigrations()
        {
            await Migration_2020_06_24_MoveToFundingViewTypeAndScope.Run(_cosmosDatabaseService, _logger);
        }
    }
}