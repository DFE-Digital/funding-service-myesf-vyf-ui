namespace PDS.VYF.Services.Implementations.InfraServices.FilesServices
{
    using Pds.Core.Logging;
    using PDS.ViewYourFunding.Services.Enums;
    using PDS.ViewYourFunding.Services.Implementations.FundingView;
    using PDS.ViewYourFunding.Services.Interfaces;
    using PDS.ViewYourFunding.Services.Models;
    using PDS.VYF.Services.Abstracts.InfraServices.FilesServices;
    using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;
    using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

    /// <summary>
    /// The UI Model Files Services.
    /// </summary>
    /// <seealso cref="PDS.VYF.Services.Abstracts.InfraServices.FilesServices.IUIModelFilesServices" />
    public class UIModelFilesServices : IUIModelFilesServices
    {
        private readonly ILoggerAdapter<ModelFundingViewService> loggerService;
        private readonly IModelFileStoreService modelFileStoreService;
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIModelFilesServices" /> class.
        /// </summary>
        /// <param name="loggerService">The logger service.</param>
        /// <param name="modelFileStoreService">The model file store service.</param>
        /// <param name="cacheService">The cache service.</param>
        public UIModelFilesServices(
            ILoggerAdapter<ModelFundingViewService> loggerService,
            IModelFileStoreService modelFileStoreService,
            ICacheService cacheService)
        {
            this.loggerService = loggerService;
            this.modelFileStoreService = modelFileStoreService;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// Retrieves the UI model for the given view data request.
        /// </summary>
        /// <param name="viewDataRequest">The view data request.</param>
        /// <returns>
        /// The UI model file response.
        /// </returns>
        public async Task<UiModelFileResponse> GetUiModel(ViewDataRequestBase viewDataRequest)
        {
            var cacheKey = string.Join(
                                    "-",
                                    nameof(UIModelFilesServices),
                                    nameof(this.GetUiModel),
                                    viewDataRequest.FundingStreamCode,
                                    viewDataRequest.FundingPeriodCode,
                                    viewDataRequest.SchemaVersion,
                                    viewDataRequest.TemplateVersion,
                                    viewDataRequest.PublicationUiModelVersion,
                                    viewDataRequest.FundingViewType,
                                    viewDataRequest.FundingViewScope);

            return await this.cacheService.AddOrGetExistingResultAsync(
                                                cacheKey,
                                                () => this.GetUiModelInternal(viewDataRequest),
                                                ViewYourFunding.Services.Cache.CacheExpirationPolicy.Sliding,
                                                TimeSpan.FromMinutes(20));
        }

        /// <summary>
        /// Gets the UI model internal.
        /// </summary>
        /// <param name="viewDataRequest">The view data request.</param>
        /// <returns>The Ui Model File Response.</returns>
        /// <exception cref="System.IO.FileNotFoundException">
        /// Model not found for fundingStreamCode: {viewDataRequest.FundingStreamCode} schemaVersion: {viewDataRequest.SchemaVersion} templateVersion: {viewDataRequest.TemplateVersion}"
        /// + $" modelVersion: {viewDataRequest.PublicationUiModelVersion} fundingViewType: {viewDataRequest.FundingViewType.ToString()} fundingViewScope: {viewDataRequest.FundingViewScope}.
        /// </exception>
        internal async Task<UiModelFileResponse> GetUiModelInternal(ViewDataRequestBase viewDataRequest)
        {
            var uiModelResponse = this.GetRelevantTemplateFilePath(
                viewDataRequest.FundingStreamCode,
                viewDataRequest.FundingPeriodCode,
                this.Parse(viewDataRequest.SchemaVersion),
                this.Parse(viewDataRequest.TemplateVersion),
                viewDataRequest.PublicationUiModelVersion,
                viewDataRequest.FundingViewType,
                viewDataRequest.FundingViewScope);

            if (string.IsNullOrWhiteSpace(uiModelResponse?.TemplateFileDetails?.FilePath))
            {
                throw new FileNotFoundException($"{viewDataRequest.FundingViewType} - No template found for fundingStreamCode: {viewDataRequest.FundingStreamCode} SchemaVersion: {viewDataRequest.SchemaVersion}"
                    + $" templateVersion: {viewDataRequest.TemplateVersion} modelVersion {viewDataRequest.PublicationUiModelVersion} fundingViewScope: {viewDataRequest.FundingViewScope}");
            }

            if (!File.Exists(uiModelResponse.TemplateFileDetails.FilePath))
            {
                throw new FileNotFoundException($"Model not found for fundingStreamCode: {viewDataRequest.FundingStreamCode} schemaVersion: {viewDataRequest.SchemaVersion} templateVersion: {viewDataRequest.TemplateVersion}"
                    + $" modelVersion: {viewDataRequest.PublicationUiModelVersion} fundingViewType: {viewDataRequest.FundingViewType} fundingViewScope: {viewDataRequest.FundingViewScope}");
            }

            var response = await File.ReadAllTextAsync(uiModelResponse.TemplateFileDetails.FilePath);
            uiModelResponse.Json = response;

            return uiModelResponse;
        }

        private double? Parse(string? value)
        {
            if (value != null && double.TryParse(value, out double result) && result > 0)
            {
                return result;
            }

            return null;
        }

        private string[] GetModelFilenames(string fundingViewTypeString, string fundingPeriodCode)
        {
            var allFilePaths = this.modelFileStoreService.GetModelFilenames($"{fundingViewTypeString}/{fundingPeriodCode}");
            return allFilePaths ?? this.modelFileStoreService.GetModelFilenames(fundingViewTypeString);
        }

        private UiModelFileResponse GetRelevantTemplateFilePath(
            string fundingStreamCode,
            string fundingPeriodCode,
            double? schemaVersion,
            double? templateVersion,
            int? modelVersion,
            FundingViewType fundingViewType,
            FundingViewScope fundingViewScope)
        {
            var allFilePaths = this.GetModelFilenames(fundingViewType.ToString(), fundingPeriodCode);
            var matchingFilePaths = new List<TemplateFileDetails>();

            const string FILE_PART_SEPERATOR_SCHEMA_MIN = "schemamin";
            const string FILE_PART_SEPERATOR_TEMPLATE_MIN = "templatemin";
            const string FILE_PART_SEPERATOR_MAX = "max";
            const string FILE_PART_SEPERATOR_MODEL_VERSION = "ModelVersion";
            const int FILE_PARTS_LENGTH_WTIH_MODEL_VERSION = 5;
            const int FILE_PARTS_LENGTH_WTIHOUT_MODEL_VERSION = 4;
            const int FILE_PART_SCHEMA = 1;
            const int FILE_PART_TEMPLATE = 2;
            const int FILE_PART_MODEL = 3;

            foreach (var filePath in allFilePaths)
            {
                try
                {
                    var fileName = Path.GetFileName(filePath);
                    var fileNameParts = fileName.Split('_');
                    var fileNameContainsModelVersion = fileName.Contains(FILE_PART_SEPERATOR_MODEL_VERSION);
                    var fileNamePartsExpectedLength = fileNameContainsModelVersion ? FILE_PARTS_LENGTH_WTIH_MODEL_VERSION : FILE_PARTS_LENGTH_WTIHOUT_MODEL_VERSION;
                    var lastFilePart = fileNamePartsExpectedLength - 1;

                    if (fileNameParts.Length < (fileNamePartsExpectedLength - 1))
                    {
                        throw new Exception($"Filename format is incorrect. Invalid FileName: {filePath}");
                    }

                    var fundingStreamCodeFileNamePart = fileNameParts.First();

                    // Filter by funding stream code matching
                    if (!fundingStreamCodeFileNamePart.Equals(fundingStreamCode, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    var scopeFileNamePart = fileNameParts.Length >= fileNamePartsExpectedLength
                        ? fileNameParts[lastFilePart].Split('.').First() : "National";

                    // Filter by document scope (e.g. National)
                    if (!fundingViewScope.ToString().Equals(scopeFileNamePart, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    var schemaFileNamePart = fileNameParts[FILE_PART_SCHEMA].ToLower();

                    var minSchema = double.Parse(this.ReplaceHyphenWithPeriod(schemaFileNamePart.Substring(
                        FILE_PART_SEPERATOR_SCHEMA_MIN.Length,
                        schemaFileNamePart.IndexOf(FILE_PART_SEPERATOR_MAX) - FILE_PART_SEPERATOR_SCHEMA_MIN.Length)));

                    var maxSchema = double.Parse(this.ReplaceHyphenWithPeriod(schemaFileNamePart.Substring(schemaFileNamePart.IndexOf(FILE_PART_SEPERATOR_MAX) + FILE_PART_SEPERATOR_MAX.Length)));

                    // Check if the schema version is between the min and max allowed
                    if (schemaVersion.HasValue && (minSchema > schemaVersion || schemaVersion > maxSchema))
                    {
                        continue;
                    }

                    var templateFileNamePart = fileNameParts[FILE_PART_TEMPLATE].Split('.')[0].ToLower();

                    var minTemplate = double.Parse(this.ReplaceHyphenWithPeriod(templateFileNamePart.Substring(
                        FILE_PART_SEPERATOR_TEMPLATE_MIN.Length,
                        templateFileNamePart.IndexOf(FILE_PART_SEPERATOR_MAX) - FILE_PART_SEPERATOR_TEMPLATE_MIN.Length)));

                    var maxTemplate = double.Parse(this.ReplaceHyphenWithPeriod(
                        templateFileNamePart.Substring(templateFileNamePart.IndexOf(FILE_PART_SEPERATOR_MAX) + FILE_PART_SEPERATOR_MAX.Length)));

                    // Check if the template version is between the min and max allowed
                    if (templateVersion.HasValue && (minTemplate > templateVersion || templateVersion > maxTemplate))
                    {
                        continue;
                    }

                    int? fileModelVersion = null;

                    if (fileName.Contains(FILE_PART_SEPERATOR_MODEL_VERSION))
                    {
                        var modelFileNamePart = fileNameParts[FILE_PART_MODEL].Split('.')[0].ToLower();
                        fileModelVersion = int.Parse(modelFileNamePart.Substring(
                            FILE_PART_SEPERATOR_MODEL_VERSION.Length,
                            modelFileNamePart.Length - FILE_PART_SEPERATOR_MODEL_VERSION.Length));

                        // If the files model version doesnt match the requested one, continue through the loop
                        // If no specific version of a model was requested, treat it as if it was version 1 to not break
                        // existing compatibility
                        if (fileModelVersion != (modelVersion ?? 1))
                        {
                            continue;
                        }
                    }

                    matchingFilePaths.Add(new TemplateFileDetails
                    {
                        FilePath = filePath,
                        MinSchemaVersion = minSchema,
                        MaxSchemaVersion = maxSchema,
                        MinTemplateVersion = minTemplate,
                        MaxTemplateVersion = maxTemplate,
                        ModelVersion = fileModelVersion,
                        Scope = scopeFileNamePart,
                    });
                }
                catch (Exception ex)
                {
                    this.loggerService?.LogError(ex, ex.Message);
                }
            }

            var match = matchingFilePaths
                .OrderByDescending(file => file.MaxTemplateVersion)
                .ThenByDescending(file => file.MaxSchemaVersion)
                .ThenByDescending(file => file.ModelVersion ?? int.MinValue)
                .FirstOrDefault();

            return new UiModelFileResponse
            {
                TemplateFileDetails = match ?? new TemplateFileDetails(),
            };
        }

        private string ReplaceHyphenWithPeriod(string numberWithHyphen)
        {
            return numberWithHyphen.Replace("-", ".");
        }
    }
}
