using PDS.ViewYourFunding.Services.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// ExpressionBuilder.
    /// </summary>
    public static class ExpressionBuilder
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains");
        private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

        /// <summary>
        /// GetExpression.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="filters">The filter.</param>
        /// <param name="orClause">The or Clase.</param>
        /// <returns>returns filter data.</returns>
        public static Expression<Func<T, bool>> GetExpression<T>(IList<UiModelDataSetFilter> filters, bool orClause)
        {
            if (filters.Count == 0)
            {
                return null;
            }

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
            {
                exp = GetExpression<T>(param, filters[0]);
            }
            else if (filters.Count == 2)
            {
                exp = GetExpression<T>(param, filters[0], filters[1], orClause);
            }
            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];

                    if (exp == null)
                    {
                        exp = GetExpression<T>(param, filters[0], filters[1], orClause);
                    }
                    else
                    {
                        exp = orClause ? Expression.OrElse(exp, GetExpression<T>(param, filters[0], filters[1], orClause)) : Expression.AndAlso(exp, GetExpression<T>(param, filters[0], filters[1], orClause));
                    }

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        exp = orClause ? Expression.OrElse(exp, GetExpression<T>(param, filters[0])) : Expression.AndAlso(exp, GetExpression<T>(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        /// <summary>
        /// GetExpression.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="param">The param.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>returns filter data.</returns>
        private static Expression GetExpression<T>(ParameterExpression param, UiModelDataSetFilter filter)
        {
            var member = Expression.Property(param, filter.PropertyName);
            var propertyType = ((PropertyInfo)member.Member).PropertyType;
            var converter = TypeDescriptor.GetConverter(propertyType); // 1

            if (!converter.CanConvertFrom(typeof(string)))
            {
                throw new NotSupportedException();
            }

            var propertyValue = converter.ConvertFromInvariantString((string)filter.Value);
            var constant = Expression.Constant(propertyValue);
            var valueExpression = Expression.Convert(constant, propertyType);

            switch (filter.Operation)
            {
                case Operator.Equals:
                    return Expression.Equal(member, valueExpression);

                case Operator.NotEquals:
                    return Expression.NotEqual(member, valueExpression);

                case Operator.GreaterThan:
                    return Expression.GreaterThan(member, valueExpression);

                case Operator.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, valueExpression);

                case Operator.LessThan:
                    return Expression.LessThan(member, valueExpression);

                case Operator.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, valueExpression);

                case Operator.Contains:
                    return Expression.Call(member, containsMethod, constant);

                case Operator.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);

                case Operator.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
            }

            return null;
        }

        private static BinaryExpression GetExpression<T>(ParameterExpression param, UiModelDataSetFilter filter1, UiModelDataSetFilter filter2, bool orClause)
        {
            Expression bin1 = GetExpression<T>(param, filter1);
            Expression bin2 = GetExpression<T>(param, filter2);

            if (orClause)
            {
                return Expression.OrElse(bin1, bin2);
            }
            else
            {
                return Expression.AndAlso(bin1, bin2);
            }
        }

        private static bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
