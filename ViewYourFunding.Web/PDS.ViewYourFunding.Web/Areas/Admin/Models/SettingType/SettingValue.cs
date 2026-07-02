using System;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType
{
    /// <summary>
    /// Represents a setting value for a given setting type and funding stream in the View Your Funding area.
    /// </summary>
    public class SettingValue
    {
        /// <summary>
        /// Gets or sets the distinct id for this instance of a setting value.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the funding stream id to which this value relates.
        /// </summary>
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the setting to which this value relates.
        /// </summary>
        public SettingType Setting { get; set; }

        /// <summary>
        /// Gets or sets the Setting Id for the SettingType.
        /// </summary>
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