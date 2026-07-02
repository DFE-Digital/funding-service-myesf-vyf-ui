using PDS.ViewYourFunding.Services.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Services.Enums
{
    /// <summary>
    /// The scope (what the data represents) of the funding view.
    /// </summary>
    public enum FundingViewScope
    {
        /// <summary>
        /// Unknown data type.
        /// </summary>
        [ApplicableFor(FundingViewType.Other)]
        Unknown = 0,

        /// <summary>
        /// Data for a whole country.
        /// </summary>
        [ApplicableFor(FundingViewType.Spreadsheet)]
        [ApplicableFor(FundingViewType.ViewData)]
        National = 1,

        /// <summary>
        /// Data specific to a provider (e.g. a school).
        /// </summary>
        [ApplicableFor(FundingViewType.Spreadsheet)]
        [ApplicableFor(FundingViewType.Pdf)]
        [ApplicableFor(FundingViewType.ViewData, "Provider funding breakdown")]
        Provider = 2,

        /// <summary>
        /// Data specific to an organisation (e.g. a LA).
        /// </summary>
        [ApplicableFor(FundingViewType.Spreadsheet)]
        [ApplicableFor(FundingViewType.ViewData, "Local authority funding breakdown")]
        [Display(Name = "Local authority")]
        Organisation = 3,

        /// <summary>
        /// Data specific to an organisation (e.g. a LA) - limited to summary-level data.
        /// </summary>
        [ApplicableFor(FundingViewType.ViewData)]
        [Display(Name = "Local authority summary")]
        OrganisationSummary = 4,

        /// <summary>
        /// Data specific to an organisation (e.g. a LA) history list page.
        /// </summary>
        [ApplicableFor(FundingViewType.ViewData)]
        [Display(Name = "Local authority history")]
        OrganisationHistory = 5,

        /// <summary>
        /// Data specific to a provider (e.g. a school) - limited to summary-level data.
        /// </summary>
        [ApplicableFor(FundingViewType.ViewData)]
        [Display(Name = "Provider summary")]
        ProviderSummary = 6,

        /// <summary>
        /// History for a provider (e.g. a school).
        /// </summary>
        [ApplicableFor(FundingViewType.ViewData)]
        [Display(Name = "Provider history")]
        ProviderHistory = 7,

        /// <summary>
        /// History for a provider (e.g. a school) for a single year (e.g. 2018-2019).
        /// </summary>
        [ApplicableFor(FundingViewType.ViewData)]
        [Display(Name = "Provider history (single year)")]
        ProviderHistorySingleYear = 8,

        /// <summary>
        /// History for an organisation (e.g. an LA) for a single year (e.g. 2018-2019).
        /// </summary>
        [ApplicableFor(FundingViewType.ViewData)]
        [Display(Name = "Local authority history (single year)")]
        OrganisationHistorySingleYear = 9,

        /// <summary>
        /// A logged in provider summary.
        /// </summary>
        [LoggedInView]
        LoggedInProviderSummary = 10,

        /// <summary>
        /// A logged in provider funding breakdown.
        /// </summary>
        [LoggedInView]
        LoggedInProvider = 11,

        /// <summary>
        /// A logged in provider allocation history.
        /// </summary>
        [LoggedInView]
        LoggedInProviderHistory = 12,

        /// <summary>
        /// A logged in Mat provider summary.
        /// </summary>
        [LoggedInView]
        LoggedInMatProviderSummary = 13,

        /// <summary>
        /// Data specific to a local authority.
        /// </summary>
        [ApplicableFor(FundingViewType.Pdf)]
        LocalAuthority = 14,

        /// <summary>
        /// Data specific to an organisation (e.g. a LA) - limited to summary-level data.
        /// </summary>
        [LoggedInView]
        LoggedInOrganisationSummary = 15,

        /// <summary>
        /// Data specific to an organisation (e.g. a LA) - Detailed Maintained Special School -level data.
        /// </summary>
        [LoggedInView]
        LoggedInOrganisationMss = 16,

        /// <summary>
        /// Data specific to an organisation (e.g. a LA) - Detailed School Sixth Form-level data.
        /// </summary>
        [LoggedInView]
        LoggedInOrganisationSsf = 17,

        /// <summary>
        /// History for an organisation (e.g. a LA).
        /// </summary>
        [LoggedInView]
        LoggedInOrganisationHistory = 18,

        /// <summary>
        /// A logged in indicative provider summary.
        /// </summary>
        [LoggedInView]
        LoggedInIndicativeProviderSummary = 19,

        /// <summary>
        /// A logged in indicative provider funding breakdown.
        /// </summary>
        [LoggedInView]
        LoggedInIndicativeProvider = 20,

        /// <summary>
        /// Detail for an organisation (e.g. a LA).
        /// </summary>
        [LoggedInView]
        LoggedInOrganisation = 21
    }
}