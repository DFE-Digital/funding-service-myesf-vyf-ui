using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// The Cosmos Database service interface.
    /// </summary>
    /// <typeparam name="T">The model type.</typeparam>
    public interface ICosmosDbService<T>
        where T : CosmosDocument
    {
        /// <summary>
        /// Get All items.
        /// </summary>
        /// <param name="filters">Filter clauses.</param>
        /// <returns>A list of items.</returns>
        Task<IEnumerable<T>> GetAllAsync(List<Expression<Func<T, bool>>> filters = null);

        /// <summary>
        /// Gets the item by Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The item.</returns>
        Task<T> GetAsync(string id);

        /// <summary>
        /// Run a query and return the result as a list of dictionarys.
        /// </summary>
        /// <typeparam name="TModel">The type to return.</typeparam>
        /// <param name="query">The query to run.</param>
        /// <returns>A list of dictionarys.</returns>
        Task<IEnumerable<TModel>> RunQueryAsync<TModel>(string query);

        /// <summary>
        /// Run a query and return the result as a list of dictionarys.
        /// </summary>
        /// <typeparam name="Tdynamic">The type to return.</typeparam>
        /// <param name="query">The query to run.</param>
        /// <returns>A list of dictionarys.</returns>
        Task<string> RunQueryAllAsync<Tdynamic>(string query);

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>True if successfully added.</returns>
        Task<bool> AddAsync(T item);

        /// <summary>
        /// Updates the item by Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="item">The item.</param>
        /// <returns>True if updated successfully.</returns>
        Task<bool> UpdateAsync(string id, T item);

        /// <summary>
        /// Deletes an item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="item">The item.</param>
        /// <returns>True if successfully deleted.</returns>
        Task<bool> DeleteAsync(string id, T item);

        /// <summary>
        /// Get Count for specified filter.
        /// </summary>
        /// <param name="filters">Filter clauses.</param>
        /// <returns>Returns the total no. records.</returns>
        Task<int> GetCountAsync(List<Expression<Func<T, bool>>> filters = null);

        /// <summary>
        /// Get list of items for specified page no. and filter.
        /// </summary>
        /// <param name="skip">no. of records to skip.</param>
        /// <param name="take">no. of records to take.</param>
        /// <param name="filters">Filter clauses.</param>
        /// <returns>A list of items.</returns>
        Task<IEnumerable<T>> GetPagination(int skip, int take, List<Expression<Func<T, bool>>> filters = null);
    }
}