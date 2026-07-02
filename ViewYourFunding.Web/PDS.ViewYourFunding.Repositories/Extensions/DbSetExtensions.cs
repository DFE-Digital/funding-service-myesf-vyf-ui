using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PDS.ViewYourFunding.Repositories.Extensions
{
    /// <summary>
    /// Extensions for updating database data.
    /// </summary>
    public static class DbSetExtensions
    {
        /// <summary>
        /// Get the DbContext from the DbSet that is passed in.
        /// </summary>
        /// <typeparam name="TEntity">The type of DbSet (e.g. FundingStream).</typeparam>
        /// <param name="dbSet">An entity set that can be used for create, read, update, and delete operations.</param>
        /// <returns>The relevant DbContext.</returns>
        public static DbContext GetContext<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class
        {
            return (DbContext)dbSet
                .GetType().GetTypeInfo()
                .GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(dbSet);
        }

        /// <summary>
        /// Convert expression to a where clause.
        /// </summary>
        /// <typeparam name="T">The type of function.</typeparam>
        /// <param name="expression">An expression to be converted.</param>
        /// <param name="obj">The data.</param>
        /// <returns>The expression result.</returns>
        public static Expression<Func<T, bool>> ConvertToWhereClause<T>(this Expression<Func<T, object>> expression, T obj)
            where T : class, new()
        {
            var memberExpression = (MemberExpression)expression.Body;
            var objPropExpression = Expression.PropertyOrField(Expression.Constant(obj), memberExpression.Member.Name);
            var equalExp = Expression.Equal(expression.Body, objPropExpression);

            return Expression.Lambda<Func<T, bool>>(equalExp, expression.Parameters);
        }

        /// <summary>
        /// Add Or UpdateAsync a DbSet using the data passed in.
        /// </summary>
        /// <typeparam name="T">The type of DbSet (e.g. FundingStream).</typeparam>
        /// <param name="dbSet">An entity set that can be used for create, read, update, and delete operations.</param>
        /// <param name="data">The data to add or update.</param>
        /// <param name="whereExpressions">Where clauses.</param>
        public static void AddOrUpdate<T>(this DbSet<T> dbSet, T data, params Expression<Func<T, object>>[] whereExpressions)
            where T : class, new()
        {
            AddOrUpdate(dbSet, new List<T> { data }, whereExpressions);
        }

        /// <summary>
        /// Add Or UpdateAsync a DbSet using a list of data passed in.
        /// </summary>
        /// <typeparam name="T">The type of DbSet (e.g. FundingStream).</typeparam>
        /// <param name="dbSet">An entity set that can be used for create, read, update, and delete operations.</param>
        /// <param name="dataList">A list of data to add or update.</param>
        /// <param name="whereExpressions">Where clause expressions.</param>
        public static void AddOrUpdate<T>(this DbSet<T> dbSet, List<T> dataList, params Expression<Func<T, object>>[] whereExpressions)
            where T : class, new()
        {
            var context = dbSet.GetContext();

            foreach (var item in dataList)
            {
                var query = context.Set<T>().AsNoTracking();

                foreach (var where in whereExpressions)
                {
                    query = query.Where(where.ConvertToWhereClause(item));
                }

                var entity = query.FirstOrDefault();

                if (entity == null)
                {
                    dbSet.Add(item);
                }
                else
                {
                    var ids = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name);
                    var type = typeof(T);
                    var keyFields = type.GetProperties().Where(x => ids.Contains(x.Name)).ToList();

                    foreach (var keyField in keyFields)
                    {
                        keyField.SetValue(item, keyField.GetValue(entity));
                    }

                    dbSet.Update(item);
                }
            }
        }
    }
}