using PDS.ViewYourFunding.Services.Interfaces.Models;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.DTOs
{
    /// <summary>
    /// A class representing the data for a funding view.
    /// </summary>
    public class FundingViewData
    {
        /// <summary>
        /// Gets or sets the funding stream code.
        /// </summary>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the entity to which the funding data relates (e.g. 'Camden' or 'Lickhill Primary School').
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the primary identifier of the entity to which the funding data relates (e.g. a UKPRN or LA code).
        /// </summary>
        public string EntityPrimaryIdentifier { get; set; }

        /// <summary>
        /// Gets or sets an alternative identifier of the entity to which the funding data relates (e.g. DfE number).
        /// </summary>
        public string EntityAlternativeIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity to which the funding data relates, e.g. Academy.
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the sub-type of the entity to which the funding data relates, e.g. Non-maintained special school.
        /// </summary>
        public string EntitySubType { get; set; }

        /// <summary>
        /// Gets or sets the publication date for the funding data.
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the publication UIModelVersion (optional).
        /// </summary>
        public int? PublicationUiModelVersion { get; set; }

        /// <summary>
        /// Gets or sets the total amount of funding.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets a dictionary containing named funding values, where the key is the name and the value is the funding data value.
        /// </summary>
        public IDictionary<string, object> FundingValues { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the components for use with the UI renderer.
        /// </summary>
        public List<Component> Components { get; set; } = new List<Component>();

        /// <summary>
        /// Gets or sets a collection of sub-data to this funding data.
        /// </summary>
        public IReadOnlyCollection<IFundingApiSearchProviderFunding> FundingSubData { get; set; }

        /// <summary>
        /// Gets or sets the name of the Local Authority.
        /// </summary>
        public string LocalAuthorityName { get; set; }

        /// <summary>
        /// Gets or sets the funding period code.
        /// </summary>
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is an in-year opener.
        /// </summary>
        public bool? InYearOpener { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is an indicative.
        /// </summary>
        public bool? IsIndicativeFunding { get; set; }
    }
}