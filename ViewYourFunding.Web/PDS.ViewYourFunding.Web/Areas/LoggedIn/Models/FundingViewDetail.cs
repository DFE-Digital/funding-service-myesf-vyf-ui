using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Models
{
    /// <summary>
    /// Represents data used to build funding views.
    /// </summary>
    public class FundingViewDetail
    {
        public Dictionary<string, FundingStream> FundingStreams { get; set; }

        public bool ShowSelectors { get; set; }

        public bool StatementSpecificationState { get; set; }

        public bool ShowData { get; set; }
    }
}
