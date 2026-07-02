using Pds.Core.Common.Identity.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Interfaces
{
    /// <summary>
    /// The user role authorization service interface.
    /// </summary>
    public interface IUserRoleAuthorizationService
    {
        /// <summary>
        /// Encrypts the list of required user roles names into an encrypted string to be used in url parameter.
        /// </summary>
        /// <param name="requiredUserRoles">The list of required user roles to encrypt.</param>
        /// <returns>The encrypted string to be used in url parameter.</returns>
        public string EncryptRequiredUserRolesToUrlParameter(List<UserRole> requiredUserRoles);

        /// <summary>
        /// Decrypts the encrypted url parameter string into a list of required user roles and filters the valid ones to display based on user type.
        /// </summary>
        /// <param name="urlParameter">The encrypted url parameter.</param>
        /// <param name="isExternalUser">True = user is external type; False = user is internal type.</param>
        /// <returns>The list of valid user role names to display.</returns>
        public List<string> DecryptUrlParameterToRequiredUserRoleNames(string urlParameter, bool isExternalUser);

        /// <summary>
        /// Redirects to the access denied page passing through the list of required user roles to be encrypted.
        /// </summary>
        /// <param name="requiredUserRoles">The list of required user roles to be encrypted.</param>
        /// <returns>An awaitable task.</returns>
        public Task RedirectToAccessDenied(List<UserRole> requiredUserRoles);
    }
}
