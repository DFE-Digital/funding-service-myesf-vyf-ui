using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Rules
{
    /// <summary>
    /// Interface for implementing a tile rule.
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// Determines whether the rule conditions are satisfied.
        /// </summary>
        /// <param name="providerContext">The provider context.</param>
        /// <returns>
        ///   <c>true</c> if the specified provider context is satisfied; otherwise, <c>false</c>.
        /// </returns>
        bool IsSatisfied(IUserContext providerContext = null);
    }
}