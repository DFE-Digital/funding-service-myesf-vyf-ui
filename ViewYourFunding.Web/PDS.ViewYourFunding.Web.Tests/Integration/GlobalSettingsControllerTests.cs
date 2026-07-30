using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Repositories.Implementations;
using PDS.ViewYourFunding.Repositories.Migrations;
using PDS.ViewYourFunding.Services.Config;
using PDS.ViewYourFunding.Services.Extensions;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Tests.Integration
{
    /// <summary>
    /// The VYF controller integration tests.
    /// </summary>
    [TestClass]
    public class GlobalSettingsControllerTests
    {
        private readonly ICacheService _cacheService = new MemoryCacheService(null, 0);

        [TestMethod, TestCategory("Integration")]
        public async Task GetExternalViewUrlSetting_SettingExists_ReturnsOkResultAndUrlValue()
        {
            // Arrange
            var databaseName = GetMethodName();

            CreateInMemoryDatabaseWithSeedData(databaseName);

            using (var context = GetContext(databaseName))
            {
                var controller = GetGlobalSettingsController(context);

                // Act
                var response = await controller.GetExternalViewUrlSetting();

                //Assert
                response.Should().Be("/view-latest-funding");
            }
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetExternalViewYourFundingSetting_SettingExists_ReturnsOkResultAndUrlValue()
        {
            // Arrange
            var databaseName = GetMethodName();

            CreateInMemoryDatabaseWithSeedData(databaseName);

            using (var context = GetContext(databaseName))
            {
                // Arrange
                var controller = GetGlobalSettingsController(context);

                // Act
                var response = await controller.GetExternalViewYourFundingSetting();

                //Assert
                response.Should().Be("FALSE");
            }
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetLoggedInViewUrlSetting_SettingExists_ReturnsOkResultAndUrlValue()
        {
            // Arrange
            var databaseName = GetMethodName();

            CreateInMemoryDatabaseWithSeedData(databaseName);

            using (var context = GetContext(databaseName))
            {
                var controller = GetGlobalSettingsController(context);

                // Act
                var response = await controller.GetLoggedInAdminViewUrlSetting();

                //Assert
                response.Should().BeOfType(typeof(string));
                response.Should().Be("/view-latest-funding/admin/home");
            }
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetLoggedInViewYourFundingSetting_SettingExists_ReturnsOkResultAndUrlValue()
        {
            // Arrange
            var databaseName = GetMethodName();

            CreateInMemoryDatabaseWithSeedData(databaseName);

            using (var context = GetContext(databaseName))
            {
                var controller = GetGlobalSettingsController(context);

                // Act
                var response = await controller.GetLoggedInAdminViewYourFundingSetting();

                //Assert
                response.Should().Be("FALSE");
            }
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetExternalViewUrlSetting_SettingDoesNotExists_ReturnsNotFoundAndErrorMessage()
        {
            // Arrange
            var databaseName = GetMethodName();

            CreateInMemoryDatabaseWithOneSetting(databaseName);

            using (var context = GetContext(databaseName))
            {
                var controller = GetGlobalSettingsController(context);

                // Act
                var response = await controller.GetExternalViewUrlSetting();

                // Assert
                response.Should().Be(null);
            }
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetExternalViewYourFundingSetting_SettingDoesNotExists_ReturnsNotFoundAndErrorMessage()
        {
            // Arrange
            var databaseName = GetMethodName();

            CreateInMemoryDatabaseWithOneSetting(databaseName);

            using (var context = GetContext(databaseName))
            {
                // Arrange
                var controller = GetGlobalSettingsController(context);

                // Act
                var response = await controller.GetExternalViewYourFundingSetting();

                // Assert
                response.Should().Be("Setting type 6 not found.");
            }
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetLoggedInViewUrlSetting_SettingDoesNotExists_ReturnsNotFoundAndErrorMessage()
        {
            // Arrange
            var databaseName = GetMethodName();

            CreateInMemoryDatabaseWithOneSetting(databaseName);

            using (var context = GetContext(databaseName))
            {
                var controller = GetGlobalSettingsController(context);

                // Act
                var response = await controller.GetLoggedInAdminViewUrlSetting();

                // Assert
                response.Should().Be(null);
            }
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetLoggedInViewYourFundingSetting_SettingDoesNotExists_ReturnsNotFoundAndErrorMessage()
        {
            // Arrange
            var databaseName = GetMethodName();

            CreateInMemoryDatabaseWithOneSetting(databaseName);

            using (var context = GetContext(databaseName))
            {
                var controller = GetGlobalSettingsController(context);

                // Act
                var response = await controller.GetLoggedInAdminViewYourFundingSetting();

                // Assert
                response.Should().Be("Setting type 2 not found.");
            }
        }

        /// <summary>
        /// Allows you to obtain the method or property name of the caller to the method.
        /// Using reflection, MethodBase.GetCurrentMethod().Name returned MoveNext.
        /// </summary>
        /// <param name="name">Caller member name.</param>
        /// <returns>Name of property or method.</returns>
        private static string GetMethodName([CallerMemberName] string name = null)
        {
            return name;
        }

        /// <summary>
        /// Create DbContext for in-memory database.
        /// </summary>
        /// <param name="name">Database name.</param>
        /// <returns>DbContext.</returns>
        private static Context GetContext(string name)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(name).Options;
            return new Context(options);
        }

        /// <summary>
        /// Configure web auto-mapper.
        /// </summary>
        /// <returns>Auto-mapper settings.</returns>
        private static IMapper GetWebMapper()
        {
            var config = new TypeAdapterConfig();
            config.ConfigureWebMappings();

            return new Mapper(config);
        }

        /// <summary>
        /// Configure service auto-mapper.
        /// </summary>
        /// <returns>Auto-mapper settings.</returns>
        private static IMapper GetServiceMapper()
        {
            var config = new TypeAdapterConfig();
            config.ConfigureServicesMappings();

            return new Mapper(config);
        }


        /// <summary>
        /// Get instance of GlobalSettingsController.
        /// </summary>
        /// <param name="context">DbContext.</param>
        /// <returns>GlobalSettingsController.</returns>
        private static GlobalSettingsController GetGlobalSettingsController(Context context)
        {
            var globalSettingRepository = new GlobalSettingRepository(context, null);
            var service = new GlobalSettingService(globalSettingRepository, GetServiceMapper(), new MemoryCacheService(null, 0));
            return new GlobalSettingsController(service, GetWebMapper(), null, null, null, null, new MemoryCacheService(null, 0), null, null, null);
        }

        /// <summary>
        /// Create an in-memory database with one setting.
        /// </summary>
        private static void CreateInMemoryDatabaseWithOneSetting(string databaseName)
        {
            using (var context = GetContext(databaseName))
            {
                context.GlobalSettings.Add(new Repositories.DataModels.GlobalSetting()
                {
                    Id = 100,
                    Type = 100,
                    Description = "Dummy setting description",
                    EditType = Repositories.DataModels.GlobalSetting.SettingEditType.Bool,
                    ReadOnly = false,
                    Value = "DUMMY",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Create an in-memory database using the seed method.
        /// </summary>
        private static void CreateInMemoryDatabaseWithSeedData(string databaseName)
        {
            using (var context = GetContext(databaseName))
            {
                DatabaseConfiguration.SeedGlobalSettings(context, DateTime.Now);
                context.SaveChanges();
            }
        }
    }
}