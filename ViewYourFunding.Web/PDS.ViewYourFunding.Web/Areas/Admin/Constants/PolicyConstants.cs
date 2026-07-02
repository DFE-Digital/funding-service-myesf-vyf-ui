using Pds.Core.Common.Identity.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Policy;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Constants
{
    /// <summary>
    /// Security policy constants class.
    /// </summary>
    public static class PolicyConstants
    {
        /// <summary>
        /// The AllocationsAdministrator Role.
        /// </summary>
        public const string AllocationsAdministrator = nameof(AllocationsAdministrator);

        /// <summary>
        /// Gets the list of policy information used to create authorization policies and determine required roles for associated urls.
        /// </summary>
        public static List<PolicyInfo> PolicyInformation => new List<PolicyInfo>
        {
            new PolicyInfo
            {
                Name = nameof(UserRole.SfsAdmin),
                UserRoles = new List<UserRole> { UserRole.SfsAdmin },
                UrlEntryPoints = new List<string> { "/admin/home", "/admin/fundingstream", "/admin/settings", "/admin/nextpayment", "/admin/nextpaymenttype", "/admin/pdfGenerationActions", "/admin/publication", "/admin/settingTypes", $"/{ViewYourFundingConstants.Route_ClearCache}" }
            },
            new PolicyInfo
            {
                Name = nameof(UserRole.ViewAllocationStatements),
                UserRoles = new List<UserRole> { UserRole.ViewAsProvider, UserRole.ViewAllocationStatements },
                UrlEntryPoints = new List<string> { "/pre-16-16-19-statements" }
            },
            new PolicyInfo
            {
                Name = nameof(UserRole.ViewRecoupmentReports),
                UserRoles = new List<UserRole> { UserRole.ViewAsProvider, UserRole.ViewRecoupmentReports },
                UrlEntryPoints = new List<string> { "/recoupment-reports" }
            },
            new PolicyInfo
            {
                Name = nameof(AllocationsAdministrator),
                UserRoles = new List<UserRole> { UserRole.SfsAdmin, UserRole.AllocationsAdministrator_1416, UserRole.AllocationsAdministrator_1619, UserRole.AllocationsAdministrator_DSG, UserRole.AllocationsAdministrator_GAG, UserRole.AllocationsAdministrator_NMSS, UserRole.AllocationsAdministrator_PPG, UserRole.AllocationsAdministrator_PSG },
                UrlEntryPoints = new List<string> { "/admin/allocations-management" }
            }
        };
    }
}
