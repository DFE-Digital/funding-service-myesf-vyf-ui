using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Interfaces
{
    /// <summary>
    /// Generic database repository.
    /// </summary>
    /// <typeparam name="T">Class of type T.</typeparam>
    public interface IRepository<T>
        where T : class
    {
        /// <summary>
        /// Get type T by Id.
        /// </summary>
        /// <param name="id">Id to retrieve.</param>
        /// <param name="includeProperties">List of include properties to return - comma separated.</param>
        /// <returns>A Task of generic class T.</returns>
        Task<T> GetAsync(int id, string includeProperties = null);

        /// <summary>
        /// Get all items for specified filter.
        /// </summary>
        /// <param name="filter">Filter clause.</param>
        /// <param name="orderBy">Order by clause.</param>
        /// <param name="includeProperties">List of include properties to return - comma separated.</param>
        /// <returns>A task that contains an IEnumerable list of type T.</returns>
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null);

        /// <summary>
        /// Get all items for specified filter.
        /// </summary>
        /// <typeparam name="TModel">The type.</typeparam>
        /// <param name="filter">Filter clause.</param>
        /// <param name="orderBy">Order by clause.</param>
        /// <param name="includeProperties">List of include properties to return - comma separated.</param>
        /// <returns>A task that contains an IEnumerable list of type TModel.</returns>
        Task<IEnumerable<TModel>> GetAllAsyncType<TModel>(
            Expression<Func<TModel, bool>> filter = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            string includeProperties = null)
                where TModel : class;

        /// <summary>
        /// Get first item and if not found return default object.
        /// </summary>
        /// <param name="filter">Filter clause.</param>
        /// <param name="includeProperties">List of properties to return - comma separated.</param>
        /// <returns>A task of type T.</returns>
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null);

        /// <summary>
        /// Add new Item of type T.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        /// <returns>The item added.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Remove and Item of Type T by its Id.
        /// </summary>
        /// <param name="id">Id to remove.</param>
        /// <returns>A <see cref="Task"/>Number of items deleted.</returns>
        Task<int> RemoveAsync(int id);

        /// <summary>
        /// Remove and Item of Type T.
        /// </summary>
        /// <param name="entity">Entity to remove.</param>
        /// <returns>Number of items deleted.</returns>
        Task<int> Remove(T entity);

        /// <summary>
        /// Save the changes in the repository..
        /// </summary>
        /// <returns>The save changes result.</returns>
        Task<int> SaveChangesAsync();
    }
}