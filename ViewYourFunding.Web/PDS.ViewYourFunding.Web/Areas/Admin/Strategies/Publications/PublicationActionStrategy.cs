using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.Publications
{
    /// <summary>
    /// The publication strategy for handling related publication actions.
    /// </summary>
    public class PublicationActionStrategy
    {
        /// <summary>
        /// Gets or sets the publication actions.
        /// </summary>
        /// <value>
        /// The publication actions.
        /// </value>
        public IList<IPublicationAction> PublicationActions { get; set; }
    }
}