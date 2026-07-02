using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Implementations;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    [TestClass]
    public class AutomationTestingSettingTypeServiceUnitTests : DataContextTestBase
    {
        #region Tests

        [TestMethod, TestCategory("Unit")]
        public async Task AddRegressionSettingValueTestSettingTypeAsync_GetExpectedResult()
        {
            // Arrange
            var databaseName = GetAsyncMethodName();

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var automationTestingSettingTypeService = new AutomationTestingSettingTypeService(context, null);

                // Act
                var actual = await automationTestingSettingTypeService.AddRegressionSettingValueTestSettingTypeAsync();

                // Assert
                actual.Should().BeTrue();

                var addedSetting = context.Settings.FirstOrDefault(setting => setting.SettingName == DoNotUseSettingNameAndDescription);
                addedSetting.Should().NotBeNull();
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteRegressionSettingValueTestSettingTypeAsync_GetExpectedResult()
        {
            // Arrange
            var settingType = GetRegressionSettingValueTestSettingType();
            var fundingStreams = GetFundingStreamData();

            var databaseName = GetAsyncMethodName();

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStreams);
                await context.AddRangeAsync(settingType);
                await context.SaveChangesAsync();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var automationTestingSettingTypeService = new AutomationTestingSettingTypeService(context, null);

                // Act
                var actual = await automationTestingSettingTypeService.DeleteRegressionSettingValueTestSettingTypeAsync();

                // Assert
                actual.Should().BeTrue();
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteRegressionTestSettingTypeAsync_GetExpectedResult()
        {
            // Arrange
            var settingType = GetRegressionSettingType();
            var fundingStreams = GetFundingStreamData();

            var databaseName = GetAsyncMethodName();

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStreams);
                await context.AddRangeAsync(settingType);
                await context.SaveChangesAsync();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var automationTestingSettingTypeService = new AutomationTestingSettingTypeService(context, null);

                // Act
                var actual = await automationTestingSettingTypeService.DeleteRegressionTestSettingTypeAsync();

                // Assert
                actual.Should().BeTrue();
            }
        }

        #endregion
    }
}