using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A collection of request objects and there related api services for a dataset.
    /// </summary>
    public class FundingApiRequestObjectCollectionForDataset
    {
        /// <summary>
        /// Gets or sets dssdsd.
        /// </summary>
        public List<IFundingApiService> ApiServices { get; set; }

        /// <summary>
        /// Gets or sets ssdsd.
        /// </summary>
        public List<FundingApiSearchRequestObject> RequestObjects { get; set; }

        /// <summary>
        /// Gets or sets sddssd.
        /// </summary>
        public int DatasetPosition { get; set; }
    }
}