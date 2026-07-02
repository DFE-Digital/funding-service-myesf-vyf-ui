using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Services.Implementations;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    [TestClass]
    public class AutomationTestingSettingValueServiceUnitTests : DataContextTestBase
    {
        #region Tests

        [TestMethod, TestCategory("Unit")]
        public async Task DeletePsgRegressionSettingValue_GetExpectedResult()
        {
            // Arrange
            var settingType = GetRegressionSettingValueTestSettingType();
            var settingValue = GetSettingValue();

            var databaseName = GetAsyncMethodName();
            SettingValue settingValueToDelete;

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(settingType);
                await context.SaveChangesAsync();

                var regressionSettingType = context.Settings.First();
                settingValue.SettingId = regressionSettingType.Id;
                await context.AddRangeAsync(settingValue);
                await context.SaveChangesAsync();

                settingValueToDelete = context.SettingValues.First();
            }


            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var automationTestingSettingValueService = new AutomationTestingSettingValueService(context, null);

                // Act
                var actual = await automationTestingSettingValueService.DeletePsgRegressionSettingValue(1);

                // Assert
                actual.Should().BeTrue();

                var deletedSettingValue = context.SettingValues.FirstOrDefault(x => x.Id == settingValueToDelete.Id);
                deletedSettingValue.Should().BeNull();
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeletePsgRegressionSettingValue_Fail_GetExpectedResult()
        {
            // Arrange
            var settingValue = GetSettingValue();
            var fundingStreams = GetFundingStreamData();

            var databaseName = GetAsyncMethodName();

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStreams);
                await context.AddRangeAsync(settingValue);
                await context.SaveChangesAsync();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var automationTestingSettingValueService = new AutomationTestingSettingValueService(context, null);

                // Act
                var actual = await automationTestingSettingValueService.DeletePsgRegressionSettingValue(12);

                // Assert
                actual.Should().BeFalse();
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateSettingValue_GetExpectedResult()
        {
            // Arrange
            var settingValue = GetSettingValue();
            var regressionSettingTypeItem = GetRegressionSettingValueTestSettingType();

            var databaseName = GetAsyncMethodName();
            SettingValue settingValueToUpdate;

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(regressionSettingTypeItem);
                await context.SaveChangesAsync();

                var regressionSettingType = context.Settings.First();
                settingValue.SettingId = regressionSettingType.Id;
                await context.AddRangeAsync(settingValue);
                await context.SaveChangesAsync();

                settingValueToUpdate = context.SettingValues.First();
            }

            const string updatedValue = "updated test";

            settingValueToUpdate.Value = updatedValue;

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var automationTestingSettingValueService = new AutomationTestingSettingValueService(context, null);

                // Act
                var actual = await automationTestingSettingValueService.UpdateSettingValueAsync(settingValueToUpdate.FundingStreamId, settingValue.Id, updatedValue);

                // Assert
                actual.Should().BeTrue();

                var updatedSettingValue = context.SettingValues.First(x => x.Id == settingValueToUpdate.Id);
                updatedSettingValue.Value.Should().Be(updatedValue);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateSettingValue_Fail_GetExpectedResult()
        {
            // Arrange
            var settingValue = GetSettingValue();

            var databaseName = GetAsyncMethodName();
            var settingValueToUpdate = new SettingValue();

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.SaveChangesAsync();
            }

            const string updatedValue = "updated test";

            settingValueToUpdate.Value = updatedValue;

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var automationTestingSettingValueService = new AutomationTestingSettingValueService(context, null);

                // Act
                var actual = await automationTestingSettingValueService.UpdateSettingValueAsync(settingValueToUpdate.FundingStreamId, settingValue.Id, updatedValue);

                // Assert
                actual.Should().BeFalse();

                var updatedSettingValue = context.SettingValues.FirstOrDefault(x => x.Id == settingValueToUpdate.Id);
                updatedSettingValue.Should().BeNull();
            }
        }

        #endregion
    }
}