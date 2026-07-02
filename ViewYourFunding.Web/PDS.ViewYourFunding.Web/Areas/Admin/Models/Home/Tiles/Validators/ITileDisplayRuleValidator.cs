using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Validators
{
    /// <summary>
    /// Implement <see cref="ITileDisplayRuleValidator"/> to validate tile specific rules.
    /// </summary>
    public interface ITileDisplayRuleValidator
    {
        /// <summary>
        /// Checks if rules associated with the implemented validator apply to a <see cref="Tile"/>.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>True or false.</returns>
        bool Validate(IUserContext userContext);
    }
}