using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Represents a setting value for a given setting type and funding stream in the View Your Funding area.
    /// </summary>
    public class Publication
    {
        /// <summary>
        /// Gets or sets the distinct id for this instance of a publication.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        public virtual int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the funding stream to which this publication relates.
        /// </summary>
        public FundingStream FundingStream { get; set; }

        /// <summary>
        /// Gets or sets the date of this publication.
        /// </summary>
        public DateTime PublishedDate { get; set; }

        /// <summary>
        /// Gets the date of this publication as a UI formatted string.
        /// </summary>
        public string PublishedDateUiFormatted
        {
            get
            {
                return DateTimeExtensions.ToDateDisplay(PublishedDate);
            }
        }

        /// <summary>
        /// Gets the date of this publication as a path formatted (e.g. dd-M-yyyy) string.
        /// </summary>
        public string PublishedDatePathFormatted
        {
            get
            {
                return PublishedDate.ToString("dd-M-yyyy");
            }
        }

        /// <summary>
        /// Gets or sets the funding period code, e.g. FY-2021.
        /// </summary>
        public virtual string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets the first year.
        /// </summary>
        public int Year1
        {
            get
            {
                return !string.IsNullOrEmpty(FundingPeriodCode) ? FundingPeriodHelper.GetYearsFromCode(FundingPeriodCode).yearFrom : -1;
            }
        }

        /// <summary>
        /// Gets the second year.
        /// </summary>
        public int Year2
        {
            get
            {
                return !string.IsNullOrEmpty(FundingPeriodCode) ?
                    FundingPeriodHelper.GetYearsFromCode(FundingPeriodCode).Item2 : -1;
            }
        }

        /// <summary>
        /// Gets or sets a value indicting whether its the latest publication.
        /// </summary>
        public bool? IsLatest { get; set; }

        /// <summary>
        /// Gets or sets a value indicting whether its the final publication.
        /// </summary>
        public bool? IsFinal { get; set; }

        /// <summary>
        /// Gets or sets the cut-off date for funding to be included in this publication.
        /// </summary>
        public DateTime? CutOffDate { get; set; }

        /// <summary>
        /// Gets or sets the description of this publication, e.g. "New allocations published for the academic year 2019 to 2020".
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the publication status.
        /// </summary>
        public PublicationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets an optional override to specify which version of the UI funding view model should be used.
        /// </summary>
        public int? UIModelVersion { get; set; }

        /// <summary>
        /// Gets or sets an optional override to specify which version of the spreadsheet funding view model should be used.
        /// </summary>
        public int? SpreadsheetModelVersion { get; set; }

        /// <summary>
        /// Gets or sets when the publication information was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the publication information was last updated.
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the username of the last person to update the publication information.
        /// </summary>
        public string LastUpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the publication layouts.
        /// </summary>
        public List<PublicationLayout> PublicationLayouts { get; set; }
    }
}