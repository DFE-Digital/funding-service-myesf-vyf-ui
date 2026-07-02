using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Tests.Unit
{
    [TestClass]
    public class GlobalSettingRepositoryTests : DataContextTestBase
    {
        [TestMethod, TestCategory("Unit")]
        public async Task GetFirstOrDefault_TypeExists_GlobalSettingReturned()
        {
            // Arrange
            const int typeId = 1;
            const string typeValue = "TRUE";
            var now = DateTime.Now;

            var databaseName = GetAsyncMethodName();
            var databaseSettings = GetGlobalSettings(typeId, typeValue, now);

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(databaseSettings);
                await context.SaveChangesAsync();
            }

            var expectedResult = databaseSettings.Where(s => s.Type == typeId).ToList();

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var sqlContext = new GlobalSettingRepository(context, null);

                // Act
                var actual = await sqlContext.GetFirstOrDefaultAsync(s => s.Type == typeId);

                // Assert
                actual.Should().BeEquivalentTo(expectedResult[0]);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFirstOrDefault_TypeDoesNotExist_NullGlobalSettingReturned()
        {
            // Arrange
            const int typeId = 2;
            const string typeValue = "FALSE";
            const int typeIdToFind = 100;
            var now = DateTime.Now;

            var databaseName = GetAsyncMethodName();
            var databaseSettings = GetGlobalSettings(typeId, typeValue, now);

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(databaseSettings);
                await context.SaveChangesAsync();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var sqlContext = new GlobalSettingRepository(context, null);

                // Act
                var actual = await sqlContext.GetFirstOrDefaultAsync(s => s.Type == typeIdToFind);

                // Assert
                actual.Should().BeNull();
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFirstOrDefault_MultipleEditTypesExist_OnlyOneGlobalSettingReturned()
        {
            // Arrange
            const int typeId = 2;
            const string typeValue = "FALSE";
            var now = DateTime.Now;

            var databaseName = GetAsyncMethodName();
            var databaseSettings = GetGlobalSettings(typeId, typeValue, now);

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(databaseSettings);
                await context.SaveChangesAsync();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var sqlContext = new GlobalSettingRepository(context, null);

                // Act
                var actual = await sqlContext.GetFirstOrDefaultAsync(s => s.EditType == GlobalSetting.SettingEditType.String);

                // Assert
                actual.EditType.Should().Be(GlobalSetting.SettingEditType.String);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetAllAsync_MultipleEditTypesExist_TwoGlobalSettingsReturned()
        {
            // Arrange
            const int typeId = 2;
            const string typeValue = "FALSE";
            var now = DateTime.Now;

            var databaseName = GetAsyncMethodName();
            var databaseSettings = GetGlobalSettings(typeId, typeValue, now);

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(databaseSettings);
                await context.SaveChangesAsync();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var sqlContext = new GlobalSettingRepository(context, null);

                // Act
                var actual = await sqlContext.GetAllAsync(s => s.EditType == GlobalSetting.SettingEditType.String);

                // Assert
                actual.Count().Should().Be(2);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetAllAsync_TypesDoesNotExist_NoGlobalSettingsReturned()
        {
            // Arrange
            const int typeId = 2;
            const string typeValue = "FALSE";
            var now = DateTime.Now;

            var databaseName = GetAsyncMethodName();
            var databaseSettings = GetGlobalSettings(typeId, typeValue, now);

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(databaseSettings);
                await context.SaveChangesAsync();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var sqlContext = new GlobalSettingRepository(context, null);

                // Act
                var actual = await sqlContext.GetAllAsync(s => s.EditType == GlobalSetting.SettingEditType.Time);

                // Assert
                actual.Count().Should().Be(0);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateAsync_ReturnsExpected()
        {
            // Arrange
            const int typeId = 12;
            const string typeValue = "FALSE";
            var now = DateTime.Now;
            var databaseSettings = GetGlobalSettings(typeId, typeValue, now);

            var databaseName = GetAsyncMethodName();

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(databaseSettings);
                await context.SaveChangesAsync();
            }

            var globalSettingToUpdate = databaseSettings.First(x => x.Type == typeId);

            globalSettingToUpdate.Value = "TRUE";

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var globalSettingRepository = new GlobalSettingRepository(context, null);

                // Act
                var actual = await globalSettingRepository.UpdateAsync(globalSettingToUpdate);

                // Assert
                actual.Should().BeTrue();

                var updatedGlobalSetting = context.GlobalSettings.First(x => x.Type == typeId);
                updatedGlobalSetting.Value.Should().Be("TRUE");
            }
        }

        /// <summary>
        /// Create a list of global settings that will be added to in-memory database for testing.
        /// </summary>
        /// <param name="typeId">Type Id.</param>
        /// <param name="settingValue">Setting value.</param>
        /// <param name="now">Date time now.</param>
        /// <returns>A list of global settings.</returns>
        private static IList<GlobalSetting> GetGlobalSettings(int typeId, string settingValue, DateTime now)
        {
            return new List<GlobalSetting>()
            {
                new GlobalSetting()
                {
                    Id = 1,
                    Type = typeId,
                    Description = "Is View Your Funding External area available",
                    EditType = GlobalSetting.SettingEditType.Bool,
                    ReadOnly = false,
                    Value = settingValue,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new GlobalSetting()
                {
                    Id = 2,
                    Type = 10,
                    Description = "Description for Type 10",
                    EditType = GlobalSetting.SettingEditType.String,
                    ReadOnly = false,
                    Value = settingValue,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new GlobalSetting()
                {
                    Id = 3,
                    Type = 11,
                    Description = "Description for Type 11",
                    EditType = GlobalSetting.SettingEditType.String,
                    ReadOnly = false,
                    Value = settingValue,
                    CreatedAt = now,
                    UpdatedAt = now
                }
            };
        }
    }
}