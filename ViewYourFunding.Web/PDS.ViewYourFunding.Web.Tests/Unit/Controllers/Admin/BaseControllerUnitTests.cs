using Microsoft.Extensions.Options;
using Moq;
using Pds.Core.Common.Identity.Models;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Logging;
using Pds.Core.Web.Models;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Controllers;
using System.Net.Http;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers.Admin
{
    public abstract class BaseControllerUnitTests
    {
        protected IClaimsBasedIdentityService SecurityService { get; }
          = Mock.Of<IClaimsBasedIdentityService>(MockBehavior.Strict);

        protected IAdminSettingsService AdminSettingsService { get; }
            = Mock.Of<IAdminSettingsService>(MockBehavior.Strict);

        protected IBackgroundTaskQueue BackgroundTaskQueue { get; }
            = Mock.Of<IBackgroundTaskQueue>(MockBehavior.Strict);

        protected IHttpClientFactory HttpClientFactory { get; }
            = Mock.Of<IHttpClientFactory>(MockBehavior.Strict);

        protected ILoggerAdapter<AdminActionsBaseController> PdfGenerationActionsControllerLogger { get; }
            = Mock.Of<ILoggerAdapter<AdminActionsBaseController>>(MockBehavior.Strict);

        protected Mock<DelegatingHandler> mockHandler = new Mock<DelegatingHandler>(MockBehavior.Strict);


        protected IAuditService AuditService { get; }
            = Mock.Of<IAuditService>(MockBehavior.Strict);

        protected IProviderFundingService ProviderFundingService { get; }
            = Mock.Of<IProviderFundingService>(MockBehavior.Strict);

        protected IOptions<ApplicationConfiguration> ApplicationConfigurationOptions { get; }
           = Mock.Of<IOptions<ApplicationConfiguration>>(MockBehavior.Strict);

        protected DelegatingHandler Handler { get; }
          = Mock.Of<DelegatingHandler>(MockBehavior.Strict);

        protected static readonly User LoggedInUser = new User
        {
            Ukprn = 12345678,
            ProviderName = "Test Provider",
            Email = "test@test.com",
            IsAuthenticated = true,
            IsExternalUser = true,
            FirstName = nameof(User.FirstName),
            FullName = nameof(User.FullName),
            LastName = nameof(User.LastName)
        };

        protected static CurrentUserViewModel GetCurrentUserViewModel()
        {
            return new CurrentUserViewModel
            {
                Ukprn = LoggedInUser.Ukprn,
                ProviderName = LoggedInUser.ProviderName,
                IsExternalUser = LoggedInUser.IsExternalUser,
                FirstName = LoggedInUser.FirstName,
                LastName = LoggedInUser.LastName,
                FullName = LoggedInUser.FullName,
                IsLoggedIn = LoggedInUser.IsAuthenticated
            };
        }
    }
}