using PDS.ViewYourFunding.Repositories.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Repositories.DataModels
{
    /// <summary>
    /// Represents a setting value for a given setting type and funding stream in the View Your Funding area.
    /// </summary>
    public class SettingValue : TableWithIntegerId
    {
        /// <summary>
        /// Gets or sets the distinct id for this instance of a setting value.
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets the funding stream to which this setting value relates.
        /// </summary>
        public FundingStream FundingStream { get; set; }

        /// <summary>
        /// Gets or sets the funding stream id to which this value relates.
        /// </summary>
        [Required]
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the setting to which this value relates.
        /// </summary>
        public Setting Setting { get; set; }

        /// <summary>
        /// Gets or sets the setting Id.
        /// </summary>
        [Required]
        public int SettingId { get; set; }

        /// <summary>
        /// Gets or sets the current value of this setting.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets when the setting value was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the setting value was last updated.
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user name of the last person to update the setting value.
        /// </summary>
        public string LastUpdatedBy { get; set; }
    }
}