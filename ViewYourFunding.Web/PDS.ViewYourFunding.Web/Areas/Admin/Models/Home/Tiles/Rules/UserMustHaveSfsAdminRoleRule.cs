using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Constants;
using System;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Rules
{
    /// <summary>
    /// User must have Sfs Admin Rule.
    /// </summary>
    /// <seealso cref="IRule" />
    public class UserMustHaveSfsAdminRoleRule : IRule
    {
        /// <inheritdoc/>
        public bool IsSatisfied(IUserContext userContext = null)
        {
            if (userContext == null)
            {
                throw new Exception("Missing user context");
            }

            return userContext.User.Roles.Any(x => x.Equals(TileBuilderConstants.SfsAdminRole, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}