using Microsoft.EntityFrameworkCore;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Repositories.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Implementations
{
    /// <summary>
    /// Generic database repository.
    /// </summary>
    /// <typeparam name="T">Class of type T.</typeparam>
    public class Repository<T> : IRepository<T>
        where T : TableWithIntegerId
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        private readonly ILoggerAdapter<Repository<T>> _loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">DbContext.</param>
        /// <param name="loggerService">The logger service.</param>
        public Repository(DbContext context, ILoggerAdapter<Repository<T>> loggerService)
        {
            _dbContext = context;
            _dbSet = GetDbSet<T>();
            _loggerService = loggerService;
        }

        /// <inheritdoc />
        public async Task<T> GetAsync(int id, string includeProperties = null)
        {
            var dbSet = _dbSet;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                return await _dbSet.Include(includeProperties).FirstOrDefaultAsync(d => d.Id == id);
            }

            return await dbSet.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            return await GetAllAsyncType<T>(filter, orderBy, includeProperties);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TModel>> GetAllAsyncType<TModel>(
            Expression<Func<TModel, bool>> filter = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            string includeProperties = null)
                where TModel : class
        {
            var query = GetDbSet<TModel>().AsNoTracking();
            query = AddFilterAndPropertyIncludes(filter, includeProperties, query);

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            var result = await query.ToListAsync();

            LogIEnumerableQueryResult(result, GetAsyncMethodName());

            return result;
        }

        /// <inheritdoc />
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            var query = _dbSet.AsNoTracking();
            query = AddFilterAndPropertyIncludes(filter, includeProperties, query);

            var result = await query.FirstOrDefaultAsync();

            LogQueryResponse(result, GetAsyncMethodName());

            return result;
        }

        /// <inheritdoc />
        public async Task<T> AddAsync(T entity)
        {
            var result = await _dbContext.AddAsync(entity);
            var saveResult = await SaveChangesAsync();

            return saveResult > 0 ? result.Entity : null;
        }

        /// <inheritdoc />
        public async Task<int> RemoveAsync(int id)
        {
            T entityToRemove = await _dbSet.FindAsync(id);
            if (entityToRemove == null)
            {
                return 0;
            }

            return await Remove(entityToRemove);
        }

        /// <inheritdoc />
        public async Task<int> Remove(T entity)
        {
            _dbSet.Remove(entity);
            var result = await _dbContext.SaveChangesAsync();
            return result;
        }

        /// <inheritdoc />
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Get method or property name of the caller to the method.
        /// </summary>
        /// <param name="name">Caller member name.</param>
        /// <returns>Name of property or method.</returns>
        private static string GetAsyncMethodName([CallerMemberName] string name = null)
        {
            return name;
        }

        private static IQueryable<TModel> AddFilterAndPropertyIncludes<TModel>(Expression<Func<TModel, bool>> filter, string includeProperties, IQueryable<TModel> query)
            where TModel : class
        {
            if (filter != null)
            {
                query = query.Where(filter);
            }

            //include properties will be comma separated
            if (includeProperties != null)
            {
                var propertiesToInclude =
                    includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var includeProperty in propertiesToInclude)
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            return query;
        }

        private DbSet<TModel> GetDbSet<TModel>()
            where TModel : class
        {
            return _dbContext.Set<TModel>();
        }

        /// <summary>
        /// Log response from query.
        /// </summary>
        /// <param name="result">Query response.</param>
        /// <param name="methodName">Calling method name.</param>
        private void LogQueryResponse(T result, string methodName)
        {
            int? id = -1;
            var propertyFound = false;

            if (result != null)
            {
                var type = result.GetType();

                var idProperty = type.GetProperty("Id");

                if (idProperty != null)
                {
                    id = (int)idProperty.GetValue(result);
                    propertyFound = true;
                }
            }

            string resultValue;

            if (result == null)
            {
                resultValue = ": null";
            }
            else if (propertyFound)
            {
                resultValue = $"Id: {id}";
            }
            else
            {
                resultValue = $"not null and Id: not member of class";
            }

            var logMessage = $"{methodName}(): return type: {typeof(T).FullName} return value {resultValue}";

            _loggerService?.LogTrace(logMessage);
        }

        /// <summary>
        /// Log response from query.
        /// </summary>
        /// <param name="result">Query response.</param>
        /// <param name="methodName">Calling method name.</param>
        private void LogIEnumerableQueryResult<TModel>(IEnumerable<TModel> result, string methodName)
            where TModel : class
        {
            var logMessage = $"{methodName}(): return IEnumerable of type: {typeof(TModel).FullName}. Record count: {result.Count()}";
            _loggerService?.LogTrace(logMessage);
        }
    }
}