namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Details about a VYF template file (inferred from the filename).
    /// </summary>
    public class TemplateFileDetails
    {
        /// <summary>
        /// Gets or sets the minimum schema version supported by this template.
        /// </summary>
        public double MinSchemaVersion { get; set; }

        /// <summary>
        /// Gets or sets the maximum schema version supported by this template.
        /// </summary>
        public double MaxSchemaVersion { get; set; }

        /// <summary>
        /// Gets or sets the minimum template version supported by this template.
        /// </summary>
        public double MinTemplateVersion { get; set; }

        /// <summary>
        /// Gets or sets the maximum template version supported by this template.
        /// </summary>
        public double MaxTemplateVersion { get; set; }

        /// <summary>
        /// Gets or sets the model version number.
        /// </summary>
        public int? ModelVersion { get; set; }

        /// <summary>
        /// Gets or sets the original filename.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the funding view scope.
        /// </summary>
        public string Scope { get; set; }
    }
}