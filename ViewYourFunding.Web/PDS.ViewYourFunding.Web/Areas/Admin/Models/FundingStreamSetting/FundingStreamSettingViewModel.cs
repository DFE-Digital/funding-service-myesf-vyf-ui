namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting
{
    /// <summary>
    /// The Funding stream setting view model.
    /// </summary>
    public class FundingStreamSettingViewModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the setting value identifier.
        /// </summary>
        /// <value>
        /// The setting value identifier.
        /// </value>
        public int SettingValueId { get; set; }
    }
}