using Microsoft.AspNetCore.Mvc.Rendering;
using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.PdfGenerationActions
{
    public class GenerateSinglePdfViewModel : PdfGenerationActionsPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                PdfGenerationActionsListBreadCrumb(),
                GenerateSinglePdfCrumb(true)
            };

        #endregion

        /// <summary>
        /// The ukprn regex.
        /// </summary>
        private const string UkprnRegex = @"^[0-9]{8}$";

        /// <summary>
        /// The cut off date regex.
        /// </summary>
        private const string CutOffDateRegex = @"^\d{4}-((0[1-9])|(1[012]))-((0[1-9]|[12]\d)|3[01])$";

        /// <summary>
        /// Gets or sets the provider funding id.
        /// </summary>
        /// <value>
        /// The provider funding id.
        /// </value>
        [Required]
        [Display(Name = "Provider Funding Id")]
        public string ProviderFundingId { get; set; }

        /// <summary>
        /// Gets or sets the funding stream and period code.
        /// </summary>
        /// <value>
        /// The funding stream code.
        /// </value>
        [Display(Name = "Funding Stream Code")]
        public string FundingStreamCodeAndPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream and period codes.
        /// </summary>
        /// <value>
        /// The funding stream codes.
        /// </value>
        public IEnumerable<SelectListItem> FundingStreamCodeAndPeriodCodes { get; set; }

        /// <summary>
        /// Gets or sets the ukprn.
        /// </summary>
        /// <value>
        /// The ukprn.
        /// </value>
        [Required]
        [RegularExpression(UkprnRegex, ErrorMessage = "Please enter a valid Provider UKPRN")]
        [Display(Name = "UKPRN - 8 digits")]
        public string Ukprn { get; set; }

        /// <summary>
        /// Gets or sets the cut off date.
        /// </summary>
        /// <value>
        /// The cut off date.
        /// </value>
        [Required]
        [RegularExpression(CutOffDateRegex, ErrorMessage = "Please enter a valid date: yyyy-mm-dd")]
        [Display(Name = "Cut Off Date")]
        public string CutOffDate { get; set; }

        /// <summary>
        /// Gets or sets the provider type.
        /// </summary>
        /// <value>
        /// The provider type.
        /// </value>
        [Display(Name = "Provider Type")]
        public string ProviderType { get; set; }

        /// <summary>
        /// Gets or sets the provider types.
        /// </summary>
        /// <value>
        /// The provider types.
        /// </value>
        public IEnumerable<SelectListItem> ProviderTypes { get; set; }

        /// <summary>
        /// Gets or sets the provider sub type.
        /// </summary>
        /// <value>
        /// The provider sub type.
        /// </value>
        [Display(Name = "Provider Sub Type")]
        public string ProviderSubType { get; set; }

        /// <summary>
        /// Gets or sets the provider sub types.
        /// </summary>
        /// <value>
        /// The provider sub types.
        /// </value>
        public IEnumerable<SelectListItem> ProviderSubTypes { get; set; }
    }
}