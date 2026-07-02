using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// The layout management service interface.
    /// </summary>
    public interface ILayoutManagementService
    {
        /// <summary>
        /// Gets all layout models.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>A list of Layout models.</returns>
        Task<IReadOnlyList<LayoutModel>> GetAllLayoutsAsync(List<Expression<Func<LayoutModel, bool>>> filters = null);

        /// <summary>
        /// Gets the layout by Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The layout model.</returns>
        Task<LayoutModel> GetLayoutAsync(string id);

        /// <summary>
        /// Adds the layout.
        /// </summary>
        /// <param name="layoutModel">The layout model.</param>
        /// <returns>The layout Id if the layout is added successfully.</returns>
        Task<Guid> AddLayoutAsync(LayoutModel layoutModel);

        /// <summary>
        /// Updates the layout.
        /// </summary>
        /// <param name="layoutModel">The layout model.</param>
        /// <returns>True if the layout model is updated successfully.</returns>
        Task<bool> UpdateLayoutAsync(LayoutModel layoutModel);

        /// <summary>
        /// Deletes the layout.
        /// </summary>
        /// <param name="layoutModel">The layout model.</param>
        /// <returns>True if the layout model is deleted successfully.</returns>
        Task<bool> DeleteLayoutAsync(LayoutModel layoutModel);

        /// <summary>
        /// Get the layout class holding the list of layouts and pagination detail.
        /// </summary>
        /// <param name="pageNumber">the page number.</param>
        /// <param name="pageSize">the page Size.</param>
        /// <param name="fundingStreams">list of funding stream ids.</param>
        /// <param name="fundingViewTypes">A list of funding view types.</param>
        /// <param name="fundingViewScopes">A list of funding view scopes.</param>
        /// <returns>A class holding list of layouts and pagination detail.</returns>
        Task<PaginationResult> GetPaginationResult(
            int pageNumber,
            int pageSize,
            IEnumerable<int> fundingStreams,
            IEnumerable<string> fundingViewTypes,
            IEnumerable<string> fundingViewScopes);

        /// <summary>
        /// Get the layout class holding the list of layouts and pagination detail and filters.
        /// </summary>
        /// <param name="pageNumber">the page number.</param>
        /// <param name="pageSize">the page Size.</param>
        /// <param name="fundingStreams">list of funding stream ids.</param>
        /// <param name="fundingViewTypes">A list of layout types.</param>
        /// <param name="fundingViewScopes">A list of layout scopes.</param>
        /// <returns>A class holding list of layouts and pagination detail and filters.</returns>
        Task<PaginationResult> GetPaginationResultWithFilters(
            int pageNumber,
            int pageSize,
            IEnumerable<int> fundingStreams,
            IEnumerable<string> fundingViewTypes,
            IEnumerable<string> fundingViewScopes);
    }
}