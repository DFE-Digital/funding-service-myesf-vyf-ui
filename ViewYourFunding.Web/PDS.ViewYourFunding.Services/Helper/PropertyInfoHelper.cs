using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Property Info Helper.
    /// </summary>
    public static class PropertyInfoHelper
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertiesCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The property info.</returns>
        public static PropertyInfo GetProperty(Type type, string propertyName)
        {
            return GetAllProperties(type).FirstOrDefault(t => t.Name == propertyName);
        }

        private static IEnumerable<PropertyInfo> GetAllProperties(Type type)
        {
            if (PropertiesCache.ContainsKey(type))
            {
                return PropertiesCache[type];
            }

            if (!type.IsInterface)
            {
                PropertiesCache.TryAdd(type, type.GetProperties());
            }
            else
            {
                PropertiesCache.TryAdd(type, new Type[] { type }
                    .Concat(type.GetInterfaces())
                    .SelectMany(i => i.GetProperties())
                    .ToArray());
            }

            return PropertiesCache[type];
        }
    }
}
