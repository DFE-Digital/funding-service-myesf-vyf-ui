using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <inheritdoc cref="IBasePathService"  />
    public class BasePathService : IBasePathService
    {
        private static string _applicationBasePath = null;
        private static string _urlForLoggedInProviderPath = null;

        private readonly IGlobalSettingService _globalSettingService;

        private const string NewBaseAddress = "/view-latest-funding";
        private const string OldBaseAddress = "/single-funding-statement/latest";

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePathService"/> class.
        /// </summary>
        /// <param name="globalSettingService">The global setting service.</param>
        public BasePathService(IGlobalSettingService globalSettingService)
        {
            _globalSettingService = globalSettingService;
        }

        /// <inheritdoc />
        public string GetApplicationBasePath()
        {
            if (_applicationBasePath != null)
            {
                return _applicationBasePath;
            }

            var setting = Task.Run(() => _globalSettingService.GetFirstOrDefault(GlobalSettingTypeConstants.UrlForExternalViewTypeId))
                .GetAwaiter().GetResult();

            _applicationBasePath = setting.Value.Equals(NewBaseAddress, StringComparison.OrdinalIgnoreCase) ? NewBaseAddress : OldBaseAddress;
            return _applicationBasePath;
        }

        /// <inheritdoc />
        public string GetUrlForLoggedInProviderPath()
        {
            if (_urlForLoggedInProviderPath != null)
            {
                return _urlForLoggedInProviderPath;
            }

            var setting = Task.Run(() => _globalSettingService.GetFirstOrDefault(GlobalSettingTypeConstants.UrlForLoggedInProviderViewTypeId))
                .GetAwaiter().GetResult();

            _urlForLoggedInProviderPath = setting.Value;
            return _urlForLoggedInProviderPath;
        }
    }
}
