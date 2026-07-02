using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Constants
{
    /// <summary>
    /// Local Authority Reports constants class.
    /// </summary>
    public static class LocalAuthorityReports
    {
        /// <summary>
        /// The student numbers text.
        /// </summary>
        public const string StudentNumbersText = "Student Numbers";

        /// <summary>
        /// The student numbers value.
        /// </summary>
        public const string StudentNumbersValue = "StudentNumbers";

        /// <summary>
        /// The sixth form text.
        /// </summary>
        public const string SixthFormText = "Sixth Form";

        /// <summary>
        /// The sixth form value.
        /// </summary>
        public const string SixthFormValue = "SixthForm";

        /// <summary>
        /// The sixth form MSS text.
        /// </summary>
        public const string SixthFormMssText = "Sixth Form MSS Only";

        /// <summary>
        /// The sixth form MSS value.
        /// </summary>
        public const string SixthFormMssValue = "SixthFormMss";

        /// <summary>
        /// The LAREC report text.
        /// </summary>
        public const string LARECText = "LA Recoupment";

        /// <summary>
        /// The LA Recoupment value.
        /// </summary>
        public const string LARECValue = "LARecoupment";

        /// <summary>
        /// The group type code parameter.
        /// </summary>
        public const string GroupTypeCodeParameter = "groupTypeCode";

        /// <summary>
        /// The group type reason parameter.
        /// </summary>
        public const string GroupTypeReasonParameter = "groupTypeReason";

        /// <summary>
        /// The excluded group type code parameter.
        /// </summary>
        public const string ExcludedGroupTypeCodeParameter = "excludedGroupTypeCode";

        /// <summary>
        /// The funding period identifier parameter.
        /// </summary>
        public const string FundingPeriodIdParameter = "fundingPeriodId";

        /// <summary>
        /// Gets the report types.
        /// </summary>
        /// <returns>The report types.</returns>
        public static IEnumerable<SelectListItem> GetReportTypes()
        {
            var result = new List<SelectListItem>
            {
                new SelectListItem(
                    StudentNumbersText,
                    StudentNumbersValue),
                new SelectListItem(
                    SixthFormText,
                    SixthFormValue),
                new SelectListItem(
                    SixthFormMssText,
                    SixthFormMssValue),
                new SelectListItem(
                    LARECText,
                    LARECValue)
            };

            return result;
        }
    }
}
