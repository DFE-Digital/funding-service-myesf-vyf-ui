using Pds.Core.Common.Identity.Attributes;
using Pds.Core.Common.Identity.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace PDS.ViewYourFunding.Web.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets an attribute of an enum value.
        /// </summary>
        /// <typeparam name="T">The type of attribute to retrieve.</typeparam>
        /// <param name="enumValue">The enum value to retrieve from.</param>
        /// <returns>The attribute of specified type or null.</returns>
        public static T GetAttributeOfType<T>(this Enum enumValue)
            where T : Attribute
        {
            return enumValue.GetType().GetMember(enumValue.ToString()).First()
                .GetCustomAttributes<T>(inherit: false)
                .FirstOrDefault();
        }

        /// <summary>
        /// Get the <see cref="DisplayAttribute.Name"/> friendly name for the <see cref="Enum"/> value.
        /// </summary>
        /// <param name="enumValue">The enum to get the display name for.</param>
        /// <returns>The friendly name.</returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            DisplayAttribute attributeOfType = enumValue.GetAttributeOfType<DisplayAttribute>();
            if (attributeOfType != null)
            {
                return attributeOfType.Name;
            }

            return enumValue.ToString();
        }

        /// <summary>
        /// Get the <see cref="UserTypeDescriptorAttribute.UserType"/> user type for the <see cref="Enum"/> value.
        /// </summary>
        /// <param name="enumValue">The enum to get the user type for.</param>
        /// <returns>The user type.</returns>
        public static UserType GetUserType(this Enum enumValue)
        {
            UserTypeDescriptorAttribute attributeOfType = enumValue.GetAttributeOfType<UserTypeDescriptorAttribute>();
            if (attributeOfType != null)
            {
                return attributeOfType.UserType;
            }

            return UserType.Unknown;
        }
    }
}
