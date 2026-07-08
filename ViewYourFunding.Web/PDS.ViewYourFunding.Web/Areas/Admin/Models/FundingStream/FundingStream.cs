using PDS.ViewYourFunding.Web.Areas.Admin.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStream
{
    /// <summary>
    /// The FundingStream.
    /// </summary>
    public class FundingStream
    {
        /// <summary>
        /// The funding stream regex.
        /// </summary>
        private const string FundingStreamRegex = @"^[a-zA-Z0-9\x20]+$";

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the funding stream.
        /// </summary>
        /// <value>
        /// The name of the funding stream.
        /// </value>
        [Required, Display(Name = "Funding Stream Name")]
        [DistinctFundingStreamName(nameof(Id), ErrorMessage = "Please use a distinct funding stream name.")]
        [RegularExpression(FundingStreamRegex, ErrorMessage = "Funding stream name should not contain special characters.")]
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets the full name of this funding stream as used in a sentence (which may mean it it lowercased), e.g. 'PE and Sport or dedicated schools grant'.
        /// </summary>
        [Display(Name = "Funding stream name when used within a sentence")]
        [RegularExpression(FundingStreamRegex, ErrorMessage = "Funding stream name should not contain special characters.")]
        public string FundingStreamNameWithinSentence { get; set; }

        /// <summary>
        /// Gets or sets the full name of this funding stream as used in a sentence (which may mean it it lowercased), e.g. 'PE and Sport or dedicated schools grant'.
        /// </summary>
        [Display(Name = "Funding stream name for manage allocation data screen")]
        public string FundingStreamBusinessAllocationName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the code is publically known (e.g. DSG is known by the pulic, but PSG isnt).
        /// </summary>
        [Display(Name = "Show funding stream code as abbreviation in the UI")]
        public bool FundingStreamCodePubliclyKnown { get; set; }

        /// <summary>
        /// Gets or sets the code of the funding stream.
        /// </summary>
        /// <value>
        /// The code of the funding stream.
        /// </value>
        [Required, Display(Name = "Funding Stream Code")]
        [DistinctFundingStreamCode(nameof(Id), ErrorMessage = "Please use a distinct funding stream code.")]
        [RegularExpression(FundingStreamRegex, ErrorMessage = "Funding stream code should not contain special characters.")]
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the funding stream is Active or not.
        /// </summary>
        [Display(Name = "Active")]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the user name of the last person to update the funding stream.
        /// </summary>
        public string LastUpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the national UI layout file id to use.
        /// </summary>
        public string NationalUiLayoutId { get; set; }

        /// <summary>
        /// Gets or sets the national spreadsheet layout file id to use.
        /// </summary>
        public string NationalSpreadsheetLayoutId { get; set; }

        /// <summary>
        /// Gets or sets the publications.
        /// </summary>
        public List<Services.Models.Publication> Publications { get; set; }

        /// <summary>
        /// Gets or sets the next payments.
        /// </summary>
        public List<Services.Models.NextPayment> NextPayments { get; set; }

        /// <summary>
        /// Gets or sets the setting values.
        /// </summary>
        public List<Services.Models.SettingValue> SettingValues { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether its relevant to show to logged in providers.
        /// </summary>
        [Display(Name = "Show on logged-in provider view")]
        public bool RelevantForProviders_LoggedIn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether its relevant to show to providers on the public view.
        /// </summary>
        [Display(Name = "Show on public provider view")]
        public bool RelevantForProviders_Public { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether its relevant to show to logged in organisations.
        /// </summary>
        [Display(Name = "Show on logged-in organisation view")]
        public bool RelevantForOrganisations_LoggedIn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether its relevant to show to organisations on the public view.
        /// </summary>
        [Display(Name = "Show on public organisation view")]
        public bool RelevantForOrganisations_Public { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether its relevant to show on the national journey.
        /// </summary>
        [Display(Name = "Show on public national view")]
        public bool RelevantForNational { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the statement history is based on publication dates.
        /// </summary>
        [Required, Display(Name = "Is allocation history based on publications")]
        public bool HistoryIndependentOfPublications { get; set; }
    }
}