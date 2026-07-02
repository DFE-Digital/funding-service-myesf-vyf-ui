using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Represents a funding stream in the View Your Funding area.
    /// </summary>
    public class FundingStream
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code that can be used to identify this funding stream, e.g. 'PSG'.
        /// </summary>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the full name of this funding stream, e.g. 'PE and Sport'.
        /// </summary>
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets the full name of this funding stream as used in a sentence (which may mean it it lowercased), e.g. 'PE and Sport or dedicated schools grant'.
        /// </summary>
        public string FundingStreamNameWithinSentence { get; set; }

        /// <summary>
        /// Gets or sets funding stream name for manage allocation data screen.
        /// </summary>
        public string FundingStreamBusinessAllocationName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the code is publically known (e.g. DSG is known by the pulic, but PSG isnt).
        /// </summary>
        public bool FundingStreamCodePubliclyKnown { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the funding stream is Active or not.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets when the funding stream was created.
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the funding stream was last updated.
        /// </summary>
        [Required]
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the username of the last person to update the funding stream.
        /// </summary>
        [Required, MaxLength(128)]
        public string LastUpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the collection of setting values for this funding stream.
        /// </summary>
        public ICollection<SettingValue> SettingValues { get; set; } = new List<SettingValue>();

        /// <summary>
        /// Gets or sets the collection of publications for this funding stream.
        /// </summary>
        public ICollection<Publication> Publications { get; set; }

        /// <summary>
        /// Gets or sets the collection of nextPaymenTypes for this funding stream.
        /// </summary>
        public ICollection<NextPaymentType> NextPaymentTypes { get; set; }

        /// <summary>
        /// Gets or sets the collection of nextPaymenTypes for this funding stream.
        /// </summary>
        public ICollection<NextPayment> NextPayments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether its relevant to show to logged in providers.
        /// </summary>
        public bool RelevantForProviders_LoggedIn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether its relevant to show to providers on the public view.
        /// </summary>
        public bool RelevantForProviders_Public { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether its relevant to show to logged in organisations.
        /// </summary>
        public bool RelevantForOrganisations_LoggedIn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether its relevant to show to organisations on the public view.
        /// </summary>
        public bool RelevantForOrganisations_Public { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether its relevant to show on the national journey.
        /// </summary>
        public bool RelevantForNational { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the allocation statement history is based on publication dates.
        /// </summary>
        public bool HistoryIndependentOfPublications { get; set; }
    }
}