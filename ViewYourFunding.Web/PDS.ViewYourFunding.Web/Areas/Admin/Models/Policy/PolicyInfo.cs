using Pds.Core.Common.Identity.Enums;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Policy
{
    /// <summary>
    /// The PolicyInfo class.
    /// </summary>
    public class PolicyInfo
    {
        /// <summary>
        /// Gets or sets the name of the policy.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the user roles associated with the policy.
        /// </summary>
        public List<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the url entry points associated with the policy.
        /// </summary>
        public List<string> UrlEntryPoints { get; set; }
    }
}
