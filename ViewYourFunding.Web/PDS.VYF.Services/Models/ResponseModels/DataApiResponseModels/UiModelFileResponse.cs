namespace PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels
{
    using Newtonsoft.Json;
    using PDS.ViewYourFunding.Services.Models;

    /// <summary>
    /// The UI model file response.
    /// </summary>
    public class UiModelFileResponse
    {
        private UiModel? uiModel;

        /// <summary>
        /// Gets or sets the template file details.
        /// </summary>
        /// <value>
        /// The template file details.
        /// </value>
        public TemplateFileDetails? TemplateFileDetails { get; set; }

        /// <summary>
        /// Gets or sets the json.
        /// </summary>
        /// <value>
        /// The json.
        /// </value>
        public string? Json { get; set; }

        /// <summary>
        /// Gets the UI model.
        /// </summary>
        /// <value>
        /// The UI model.
        /// </value>
        public UiModel? UiModel
        {
            get
            {
                if (this.uiModel != null)
                {
                    return this.uiModel;
                }
                else if (!string.IsNullOrWhiteSpace(this.Json))
                {
                    this.uiModel = JsonConvert.DeserializeObject<UiModel>(this.Json);
                    return this.uiModel;
                }

                return null;
            }
        }
    }
}
