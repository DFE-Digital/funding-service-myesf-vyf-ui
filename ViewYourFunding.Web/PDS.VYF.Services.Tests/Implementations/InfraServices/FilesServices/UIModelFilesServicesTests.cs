using Moq;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.VYF.Services.Implementations.InfraServices.FilesServices;
using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;

namespace PDS.VYF.Services.Tests.Implementations.InfraServices.FilesServices
{
    [TestClass, TestCategory("Unit")]
    public class UIModelFilesServicesTests
    {
        private MockRepository mockRepository;

        private Mock<ILoggerAdapter<ModelFundingViewService>> mockLoggerAdapter;
        private Mock<IModelFileStoreService> mockModelFileStoreService;
        private Mock<ICacheService> mockCacheService;

        /// <summary>
        /// The test json property name.
        /// </summary>
        private const string TestJsonPropertyName = "fundingAmount";

        /// <summary>
        /// The test funding amount.
        /// </summary>
        private const decimal TestFundingAmount = 1234.56m;

        /// <summary>
        /// The test funding value.
        /// </summary>
        private static readonly string TestFundingValue =
            "{ \"" + TestJsonPropertyName + "\": " + TestFundingAmount.ToString() + " }";

        public UIModelFilesServicesTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockLoggerAdapter = this.mockRepository.Create<ILoggerAdapter<ModelFundingViewService>>();
            this.mockModelFileStoreService = this.mockRepository.Create<IModelFileStoreService>();
            this.mockCacheService = this.mockRepository.Create<ICacheService>();
        }

        [TestMethod]
        public async Task GetUiModelInternal_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var uIModelFilesServices = this.CreateUIModelFilesServices();

            var viewDataRequest = new ChildSummaryViewDataRequestModel()
            {
                FundingStreamCode = "GAG",
                FundingPeriodCode = "AC-2425",
                SchemaVersion = "1.2",
                TemplateVersion = "2.0",
                FundingViewType = FundingViewType.ViewData,
                FundingViewScope = FundingViewScope.LoggedInProviderSummary,
            };

            mockModelFileStoreService.Setup(mfss => mfss.GetModelFilenames(It.IsAny<string>()))
                .Returns(new[]
                {
                    Environment.CurrentDirectory + @"/Views/FundingUIModels/ViewData/GAG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProviderSummary.json",
                });

            // Act
            var result = await uIModelFilesServices.GetUiModelInternal(
                viewDataRequest);

            // Assert
            this.mockRepository.VerifyAll();
        }

        private UIModelFilesServices CreateUIModelFilesServices()
        {
            return new UIModelFilesServices(
                this.mockLoggerAdapter.Object,
                this.mockModelFileStoreService.Object,
                this.mockCacheService.Object);
        }
    }
}
