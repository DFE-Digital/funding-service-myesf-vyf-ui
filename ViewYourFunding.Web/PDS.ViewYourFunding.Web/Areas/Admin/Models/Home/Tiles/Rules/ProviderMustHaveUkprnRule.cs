using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using System;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Rules
{
    /// <summary>
    /// Provider must have Ukprn Rule.
    /// </summary>
    /// <seealso cref="IRule" />
    public class ProviderMustHaveUkprnRule : IRule
    {
        /// <inheritdoc/>
        public bool IsSatisfied(IUserContext providerContext = null)
        {
            if (providerContext == null)
            {
                throw new Exception("Missing provider context");
            }

            return providerContext.User.Ukprn > 0;
        }
    }
}