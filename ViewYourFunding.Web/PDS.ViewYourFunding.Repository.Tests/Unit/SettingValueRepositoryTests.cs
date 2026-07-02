using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Repositories.Implementations;
using System.Linq;
using System.Threading.Tasks;
using SettingValue = PDS.ViewYourFunding.Repositories.DataModels.SettingValue;

namespace PDS.ViewYourFunding.Repositories.Tests.Unit
{
    [TestClass]
    public class SettingValueRepositoryTests : DataContextTestBase
    {
        [TestMethod, TestCategory("Unit")]
        public async Task GetSettingValueById_GetExpectedResult()
        {
            // Arrange
            var settingValue = GetSettingValue();
            var fundingStreams = GetFundingStreamData();

            var databaseName = GetAsyncMethodName();
            SettingValue settingValueToGet;

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStreams);
                await context.AddRangeAsync(settingValue);
                await context.SaveChangesAsync();

                settingValueToGet = context.SettingValues.First();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var settingsRepository = new SettingValueRepository(context, null);

                // Act
                var actual = await settingsRepository.GetSettingValueById(settingValueToGet.FundingStreamId, settingValueToGet.SettingId);

                // Assert
                actual.Should().BeEquivalentTo(settingValueToGet, options =>
                    options.Excluding(x => x.FundingStream)
                        .Excluding(x => x.Setting));
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateSettingValue_GetExpectedResult()
        {
            // Arrange
            var settingValue = GetSettingValue();
            var fundingStreams = GetFundingStreamData();

            var databaseName = GetAsyncMethodName();
            SettingValue settingValueToUpdate;

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStreams);
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
                var settingsRepository = new SettingValueRepository(context, null);

                // Act
                var actual = await settingsRepository.UpdateSettingValue(settingValueToUpdate);

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
            var fundingStreams = GetFundingStreamData();

            var databaseName = GetAsyncMethodName();
            var settingValueToUpdate = new SettingValue();

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStreams);
                await context.AddRangeAsync(settingValue);
                await context.SaveChangesAsync();
            }

            const string updatedValue = "updated test";

            settingValueToUpdate.Value = updatedValue;

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var settingsRepository = new SettingValueRepository(context, null);

                // Act
                var actual = await settingsRepository.UpdateSettingValue(settingValueToUpdate);

                // Assert
                actual.Should().BeFalse();

                var updatedSettingValue = context.SettingValues.FirstOrDefault(x => x.Id == settingValueToUpdate.Id);
                updatedSettingValue.Should().BeNull();
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteSettingValue_GetExpectedResult()
        {
            // Arrange
            var settingValue = GetSettingValue();
            var fundingStreams = GetFundingStreamData();

            var databaseName = GetAsyncMethodName();
            SettingValue settingValueToDelete;

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStreams);
                await context.AddRangeAsync(settingValue);
                await context.SaveChangesAsync();

                settingValueToDelete = context.SettingValues.First();
            }


            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var settingsRepository = new SettingValueRepository(context, null);

                // Act
                var actual = await settingsRepository.DeleteSettingValue(settingValueToDelete);

                // Assert
                actual.Should().BeTrue();

                var deletedSettingValue = context.SettingValues.FirstOrDefault(x => x.Id == settingValueToDelete.Id);
                deletedSettingValue.Should().BeNull();
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteSettingValue_Fail_GetExpectedResult()
        {
            // Arrange
            var settingValue = GetSettingValue();
            var fundingStreams = GetFundingStreamData();

            var databaseName = GetAsyncMethodName();
            var settingValueToDelete = new SettingValue();

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
                var settingsRepository = new SettingValueRepository(context, null);

                // Act
                var actual = await settingsRepository.DeleteSettingValue(settingValueToDelete);

                // Assert
                actual.Should().BeFalse();

                var deletedSettingValue = context.SettingValues.FirstOrDefault(x => x.Id == settingValueToDelete.Id);
                deletedSettingValue.Should().BeNull();
            }
        }


        private static SettingValue GetSettingValue()
        {
            return new SettingValue
            {
                FundingStreamId = 1,
                Value = "test"
            };
        }
    }
}