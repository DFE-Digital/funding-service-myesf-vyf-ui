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
    public class SettingTypeRepositoryTests : DataContextTestBase
    {
        private IList<Setting> _testSettingTypes;

        public SettingTypeRepositoryTests()
        {
            // create test data for tests
            const string lastUpdatedBy = "System";
            DateTime currentDateTime = DateTime.Now;

            _testSettingTypes = CreateTestSettingTypes(lastUpdatedBy, currentDateTime);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetAllSettingTypes_GetExpectedResult()
        {
            // Arrange
            // Store the SettingTypes test data in an in-memory database.
            var testDatabaseName = GetAsyncMethodName();
            using (var context = GetContext(testDatabaseName))
            {
                // Add the test data to the database context
                await context.AddRangeAsync(_testSettingTypes);

                // Save the data in the db context to the db
                await context.SaveChangesAsync();
            }

            // Act
            // Retrieve all of the saved SettingTypes data from the in-memory database
            IEnumerable<Setting> dbSettingTypes;
            using (var context = GetContext(testDatabaseName))
            {
                var settingTypeRepository = new SettingTypeRepository(context, null);
                dbSettingTypes = await settingTypeRepository.GetAllSettingTypes();
            }

            // Assert
            // Confirm the retrieved number of SettingType matches the original test data number of SettingTypes
            dbSettingTypes?.Count().Equals(_testSettingTypes.Count());

            // Iterate over the retrieved SettingTypes confirming the values match the original test data SettingType
            var testSettingTypeIndex = 0;
            foreach (var dbSettingTypeValue in dbSettingTypes)
            {
                dbSettingTypeValue.Should().BeEquivalentTo(
                    _testSettingTypes[testSettingTypeIndex++],
                    options => options.Excluding(x => x.SettingValues));
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GeSettingTypeById_GetExpectedResult()
        {
            // Arrange
            // Store the SettingTypes test data in an in-memory database.
            var testDatabaseName = GetAsyncMethodName();
            using (var context = GetContext(testDatabaseName))
            {
                // Add the test data to the database context
                await context.AddRangeAsync(_testSettingTypes);

                // Save the data in the db context to the db
                await context.SaveChangesAsync();
            }

            var testSettingTypeIdToGet = _testSettingTypes[1].Id;

            // Act
            // Get the testSettingTypeToGet from the database
            Setting dbSettingType;
            using (var context = GetContext(testDatabaseName))
            {
                var settingTypeRepository = new SettingTypeRepository(context, null);
                dbSettingType = await settingTypeRepository.GetSettingTypeById(testSettingTypeIdToGet);
                dbSettingType.Should().NotBeNull();
            }

            // Assert
            // Confirm the database SettingType is correct
            dbSettingType.Should().BeEquivalentTo(_testSettingTypes[1]);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetSettingTypeById_SettingTypeNotFound()
        {
            // Arrange
            // Store the SettingTypes test data in an in-memory database.
            var testDatabaseName = GetAsyncMethodName();
            var testSettingTypeIdToGet = -1;
            using (var context = GetContext(testDatabaseName))
            {
                // Add the test data to the database context
                await context.AddRangeAsync(_testSettingTypes);

                // Save the data in the db context to the db
                await context.SaveChangesAsync();
            }

            // Act
            // Get the testSettingTypeToGet from the database
            Setting dbSettingType;
            using (var context = GetContext(testDatabaseName))
            {
                var settingTypeRepository = new SettingTypeRepository(context, null);
                dbSettingType = await settingTypeRepository.GetSettingTypeById(testSettingTypeIdToGet);
            }

            // Assert
            dbSettingType.Should().BeNull();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task CreateSettingType_GetExpectedResult()
        {
            // Arrange
            var testDatabaseName = GetAsyncMethodName();
            var testSettingType = _testSettingTypes[1];

            // Act
            // Add a test SettingType to the db via the reposity API
            using (var context = GetContext(testDatabaseName))
            {
                var settingTypeRepository = new SettingTypeRepository(context, null);

                var result = await settingTypeRepository.CreateSettingType(testSettingType);
                result.Should().NotBeNull();
            }

            // Assert
            // Retrieve the saved SettingTypeValue items from the in-memory database
            // and confirm the SettingTypeValue matches the original test SettingTypeValue
            using (var context = GetContext(testDatabaseName))
            {
                var settingTypeRepository = new SettingTypeRepository(context, null);
                var dbSettingType = await settingTypeRepository.GetSettingTypeById(_testSettingTypes[1].Id);
                dbSettingType.Should().BeEquivalentTo(_testSettingTypes[1], options => options.Excluding(x => x.SettingValues));
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task CreateSettingType_FailstoCreate()
        {
            // Arrange
            Setting result;
            Setting testSettingType = null;
            var testDatabaseName = GetAsyncMethodName();

            // Act
            using (var context = GetContext(testDatabaseName))
            {
                var settingTypeRepository = new SettingTypeRepository(context, null);

                try
                {
                    result = await settingTypeRepository.CreateSettingType(testSettingType);
                }
                catch
                {
                    result = null;
                }
            }

            // Assert
            result.Should().BeNull();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateSettingType_GetExpectedResult()
        {
            // Arrange
            // Store the SettingTypes test data in an in-memory database.
            var testDatabaseName = GetAsyncMethodName();
            var testSettingType = _testSettingTypes[1];
            testSettingType.SettingName += " [UPDATED-NAME]";
            using (var context = GetContext(testDatabaseName))
            {
                // Add the test data to the database context
                await context.AddRangeAsync(_testSettingTypes);

                // Save the data in the db context to the db
                await context.SaveChangesAsync();
            }

            // Act
            // Update the test setting type
            bool result;
            using (var context = GetContext(testDatabaseName))
            {
                var settingTypeRepository = new SettingTypeRepository(context, null);

                result = await settingTypeRepository.UpdateSettingType(testSettingType);
            }

            // Assert
            // Retrieve the saved SettingTypeValue items from the in-memory database
            // and confirm the SettingTypeValue matches the original test SettingTypeValue
            result.Should().BeTrue();
            using (var context = GetContext(testDatabaseName))
            {
                var settingTypeRepository = new SettingTypeRepository(context, null);
                var dbSettingTypes = settingTypeRepository.GetAllSettingTypes();
                var dbTestSettingType = dbSettingTypes.GetAwaiter().GetResult().Single(settingType => settingType.SettingName == testSettingType.SettingName);
                dbTestSettingType.Should().BeEquivalentTo(_testSettingTypes[1], options => options.Excluding(x => x.SettingValues).Excluding(x => x.LastUpdatedAt));
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateSettingType_FailsToUpdate()
        {
            // Arrange
            var testDatabaseName = GetAsyncMethodName();
            Setting testSettingType = null;

            // Store the SettingTypes test data in an in-memory database.
            using (var context = GetContext(testDatabaseName))
            {
                // Add the test data to the database context
                await context.AddRangeAsync(_testSettingTypes);

                // Save the data in the db context to the db
                await context.SaveChangesAsync();
            }

            // Act
            bool result;
            using (var context = GetContext(testDatabaseName))
            {
                var settingTypeRepository = new SettingTypeRepository(context, null);

                try
                {
                    result = await settingTypeRepository.UpdateSettingType(testSettingType);
                }
                catch
                {
                    result = false;
                }
            }

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteSettingType_GetExpectedResult()
        {
            // Arrange
            // Store all of the test data in an in-memory database.
            var testDatabaseName = GetAsyncMethodName();
            using (var context = GetContext(testDatabaseName))
            {
                // add the test data to the db context
                await context.AddRangeAsync(_testSettingTypes);

                // save the data in the db context to the db
                await context.SaveChangesAsync();
            }

            var testSettingTypeToDelete = _testSettingTypes[1];

            // Act
            // Delete the database SettingTypeValue
            using (var context = GetContext(testDatabaseName))
            {
                var settingTypeRepository = new SettingTypeRepository(context, null);

                var result = await settingTypeRepository.DeleteSettingType(testSettingTypeToDelete);
                result.Should().BeTrue();
            }

            // Assert
            // Confirm the database SettingTypeValue has been deleted
            using (var context = GetContext(testDatabaseName))
            {
                var settingTypeRepository = new SettingTypeRepository(context, null);
                var dbSettingType = await settingTypeRepository.GetSettingTypeById(_testSettingTypes[1].Id);
                dbSettingType.Should().BeNull();
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteSettingType_FailsToDelete()
        {
            // Arrange
            Setting testSettingType = null;
            var testDatabaseName = GetAsyncMethodName();

            // Store all of the test data in an in-memory database.
            using (var context = GetContext(testDatabaseName))
            {
                // add the test data to the db context
                await context.AddRangeAsync(_testSettingTypes);

                // save the data in the db context to the db
                await context.SaveChangesAsync();
            }

            // Act
            bool result;
            using (var context = GetContext(testDatabaseName))
            {
                var settingTypeRepository = new SettingTypeRepository(context, null);

                try
                {
                    result = await settingTypeRepository.DeleteSettingType(testSettingType);
                }
                catch
                {
                    result = false;
                }
            }

            // Assert
            result.Should().BeFalse();
        }

        #region Test Data

        /// <summary>
        /// Create a list of Settings that will be added to the in-memory database for testing.
        /// </summary>
        /// <param name="lastUpdatedBy"> LastUpdateBy value.</param>
        /// <param name="currentDateTime">Date time now.</param>
        /// <returns>A list of SettingTypes.</returns>
        private static IList<Setting> CreateTestSettingTypes(string lastUpdatedBy, DateTime currentDateTime)
        {
            return new List<Setting>
            {
                new Setting
                {
                    SettingName = "StringSettingType",
                    SettingDescription = "String SettingType description",
                    ValueDataType = Enums.SettingValueDataType.String,
                    ValuesAreEditable = true,
                    CreatedAt = currentDateTime,
                    LastUpdatedAt = currentDateTime,
                    LastUpdatedBy = lastUpdatedBy
                },
                new Setting
                {
                    SettingName = "IntSettingType",
                    SettingDescription = "Int SettingType description",
                    ValueDataType = Enums.SettingValueDataType.Int,
                    ValuesAreEditable = true,
                    CreatedAt = currentDateTime,
                    LastUpdatedAt = currentDateTime,
                    LastUpdatedBy = lastUpdatedBy
                },
                new Setting
                {
                    SettingName = "DateTimeSettingType",
                    SettingDescription = "DateTime SettingType description",
                    ValueDataType = Enums.SettingValueDataType.DateTime,
                    ValuesAreEditable = true,
                    CreatedAt = currentDateTime,
                    LastUpdatedAt = currentDateTime,
                    LastUpdatedBy = lastUpdatedBy
                },
                new Setting
                {
                    SettingName = "BoolSettingType",
                    SettingDescription = "Bool SettingType description",
                    ValueDataType = Enums.SettingValueDataType.Bool,
                    ValuesAreEditable = true,
                    CreatedAt = currentDateTime,
                    LastUpdatedAt = currentDateTime,
                    LastUpdatedBy = lastUpdatedBy
                },
                new Setting
                {
                    SettingName = "TimeSettingType",
                    SettingDescription = "TimeSettingType description",
                    ValueDataType = Enums.SettingValueDataType.Time,
                    ValuesAreEditable = true,
                    CreatedAt = currentDateTime,
                    LastUpdatedAt = currentDateTime,
                    LastUpdatedBy = lastUpdatedBy
                },
                new Setting
                {
                    SettingName = "DateSettingType",
                    SettingDescription = "Date SettingType description",
                    ValueDataType = Enums.SettingValueDataType.Date,
                    ValuesAreEditable = true,
                    CreatedAt = currentDateTime,
                    LastUpdatedAt = currentDateTime,
                    LastUpdatedBy = lastUpdatedBy
                }
            };
        }

        #endregion Test Data
    }
}