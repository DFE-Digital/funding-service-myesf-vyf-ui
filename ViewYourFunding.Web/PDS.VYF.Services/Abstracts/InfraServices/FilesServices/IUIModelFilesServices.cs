namespace PDS.VYF.Services.Abstracts.InfraServices.FilesServices
{
    using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;
    using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

    /// <summary>
    /// Represents the interface for UI model files services.
    /// </summary>
    public interface IUIModelFilesServices
    {
        /// <summary>
        /// Retrieves the UI model for the given view data request.
        /// </summary>
        /// <param name="viewDataRequest">The view data request.</param>
        /// <returns>The UI model file response.</returns>
        Task<UiModelFileResponse> GetUiModel(ViewDataRequestBase viewDataRequest);
    }
}
