using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A UI mapping model for funding (e.g. a DSG spreadsheet template).
    /// </summary>
    public class UiModel
    {
        /// <summary>
        /// Gets or sets the name of the template.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of template (e.g. spreadsheet).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the groups of data in the template. These can be thought of as a type of building block (e.g. groups may be sheets in a spreadsheet).
        /// </summary>
        public List<UiModelGroup> Groups { get; set; }

        /// <summary>
        /// Gets or sets definition of the classes available to use in the template.
        /// </summary>
        public Dictionary<string, UiModelClass> Classes { get; set; }

        /// <summary>
        /// Gets or sets dataset for the group data.
        /// </summary>
        public List<UiModelDataset> Dataset { get; set; }

        /// <summary>
        /// Gets or sets the variables.
        /// </summary>
        public List<UIModelVariable> Variables { get; set; }

        /// <summary>
        /// Gets or sets the bookmarks.
        /// </summary>
        public List<UIModelBookmark> Bookmarks { get; set; }

        /// <summary>
        /// Gets or sets the additional funding streams.
        /// </summary>
        public UIModelAdditionalFundingStream[] AdditionalFundingStreams { get; set; }

        /// <summary>
        /// Convert a UiModel to a UIModelGroup.
        /// </summary>
        /// <returns>A UIModelGroup.</returns>
        public UiModelGroup ToUiModelGroup()
        {
            return new UiModelGroup
            {
                Type = Type,
                Groups = Groups
            };
        }
    }
}