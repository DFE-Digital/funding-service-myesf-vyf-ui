using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Rules;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Validators
{
    /// <summary>
    /// Validator class checks if the supplied rules are satisfied.
    /// </summary>
    /// <seealso cref="ITileDisplayRuleValidator" />
    public class TileDisplayRuleValidator : ITileDisplayRuleValidator
    {
        private readonly IEnumerable<IRule> _rules;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileDisplayRuleValidator"/> class.
        /// </summary>
        /// <param name="rules">The rules.</param>
        public TileDisplayRuleValidator(IEnumerable<IRule> rules)
        {
            _rules = rules;
        }

        /// <inheritdoc/>
        public virtual bool Validate(IUserContext userContext)
        {
            if (_rules == null || !_rules.Any())
            {
                return false;
            }

            foreach (var rule in _rules)
            {
                try
                {
                    if (!rule.IsSatisfied(userContext))
                    {
                        return false;
                    }
                }
                catch (System.Exception)
                {
                    return false;
                }
            }

            return true;
        }
    }
}