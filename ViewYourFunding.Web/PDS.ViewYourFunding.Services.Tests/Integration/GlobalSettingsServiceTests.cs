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
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Integration
{
    /// <summary>
    /// The VYF controller integration tests.
    /// </summary>
    [TestClass]
    public class GlobalSettingsServiceTests
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalSettingsServiceTests"/> class.
        /// </summary>
        public GlobalSettingsServiceTests()
        {
            _mapper = GetMapper();
        }

        [TestMethod, TestCategory("Integration")]
        [DataRow(0)]
        [DataRow(-1)]
        public void GetFirstOrDefault_TypeLessThanOne_NullGlobalSettingReturned(int typeId)
        {
            // Arrange
            var databaseName = GetAsyncMethodName();

            // Create in-memory database and populate with seed data.
            using (var context = GetContext(databaseName))
            {
                DatabaseConfiguration.SeedGlobalSettings(context, DateTime.Now);
                context.SaveChanges();
            }

            using (var context = GetContext(databaseName))
            {
                var globalSettingRepository = new GlobalSettingRepository(context, null);
                var service = new GlobalSettingService(globalSettingRepository, _mapper, null);

                // Act
                Func<Task> act = async () => await service.GetFirstOrDefault(typeId);

                // Assert
                act.Should().ThrowAsync<ArgumentException>().WithMessage("Setting Id must be greater than 0.");
            }
        }

        [TestMethod, TestCategory("Integration")]
        [DataRow(0)]
        [DataRow(-1)]
        public async Task Get_LessThanOne_NullGlobalSettingReturned(int globalSettingId)
        {
            // Arrange
            var databaseName = GetAsyncMethodName();

            // Create in-memory database and populate with seed data.
            using (var context = GetContext(databaseName))
            {
                DatabaseConfiguration.SeedGlobalSettings(context, DateTime.Now);
                await context.SaveChangesAsync();
            }

            using (var context = GetContext(databaseName))
            {
                var globalSettingRepository = new GlobalSettingRepository(context, null);
                var service = new GlobalSettingService(globalSettingRepository, _mapper, null);

                // Act
                Func<Task> act = async () => await service.Get(globalSettingId);

                // Assert
                await act.Should().ThrowAsync<ArgumentException>().WithMessage("Global Setting Id must be greater than 0.");
            }
        }

        [TestMethod, TestCategory("Integration")]
        [DataRow(1)]
        [DataRow(2)]
        public async Task Get_GlobalSettingReturned(int globalSettingId)
        {
            // Arrange
            var databaseName = GetAsyncMethodName();

            // Create in-memory database and populate with seed data.
            using (var context = GetContext(databaseName))
            {
                DatabaseConfiguration.SeedGlobalSettings(context, DateTime.Now);
                await context.SaveChangesAsync();
            }

            using (var context = GetContext(databaseName))
            {
                var globalSettingRepository = new GlobalSettingRepository(context, null);
                var service = new GlobalSettingService(globalSettingRepository, _mapper, null);

                // Act
                var result = await service.Get(globalSettingId);

                // Assert
                result.Should().NotBeNull();
                result.Id.Should().Be(globalSettingId);
            }
        }

        [TestMethod, TestCategory("Integration")]
        [DataRow(999)]
        public async Task GetFirstOrDefault_TypeDoesNotExist_NullGlobalSettingReturned(int typeId)
        {
            // Arrange
            var databaseName = GetAsyncMethodName();

            // Create in-memory database and populate with seed data.
            using (var context = GetContext(databaseName))
            {
                DatabaseConfiguration.SeedGlobalSettings(context, DateTime.Now);
                context.SaveChanges();
            }

            using (var context = GetContext(databaseName))
            {
                var globalSettingRepository = new GlobalSettingRepository(context, null);
                var service = new GlobalSettingService(globalSettingRepository, _mapper, new MemoryCacheService(null, 0));

                // Act
                var actual = await service.GetFirstOrDefault(typeId);

                // Assert
                actual.Should().BeNull();
            }
        }

        [TestMethod, TestCategory("Integration")]
        [DataRow(6, "Is view your funding public view available?")]
        [DataRow(1, "URL for internal admin view")]
        public async Task GetFirstOrDefault_TypeExists_EnsureCorrectTypeIdAndDescriptionsReturned(int typeId, string description)
        {
            // Arrange
            var databaseName = GetAsyncMethodName();

            // Create in-memory database and populate with seed data.
            using (var context = GetContext(databaseName))
            {
                DatabaseConfiguration.SeedGlobalSettings(context, DateTime.Now);
                context.SaveChanges();
            }

            using (var context = GetContext(databaseName))
            {
                var globalSettingRepository = new GlobalSettingRepository(context, null);
                var service = new GlobalSettingService(globalSettingRepository, _mapper, new MemoryCacheService(null, 0));

                // Act
                var actual = await service.GetFirstOrDefault(typeId);

                // Assert
                actual.Type.Should().Be(typeId);
                actual.Description.Should().Be(description);
            }
        }


        [TestMethod, TestCategory("Integration")]
        public async Task GetAllGlobalSettings_EnsureExpectedReturned()
        {
            // Arrange
            var databaseName = GetAsyncMethodName();

            // Create in-memory database and populate with seed data.
            using (var context = GetContext(databaseName))
            {
                DatabaseConfiguration.SeedGlobalSettings(context, DateTime.Now);
                context.SaveChanges();
            }

            using (var context = GetContext(databaseName))
            {
                var globalSettingRepository = new GlobalSettingRepository(context, null);
                var service = new GlobalSettingService(globalSettingRepository, _mapper, null);

                // Act
                var actual = await service.GetAllGlobalSettings();

                // Assert
                actual.Should().OnlyContain(x => !string.IsNullOrWhiteSpace(x.Description));
                actual.Should().HaveCount(11);
            }
        }

        /// <summary>
        /// Allows you to obtain the method or property name of the caller to the method.
        /// Using reflection, MethodBase.GetCurrentMethod().Name returned MoveNext.
        /// </summary>
        /// <param name="name">Caller member name.</param>
        /// <returns>Name of property or method.</returns>
        private static string GetAsyncMethodName([CallerMemberName] string name = null)
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
        /// Configure auto-mapper.
        /// </summary>
        /// <returns>Auto-mapper settings.</returns>
        private static IMapper GetMapper()
        {
            var config = new TypeAdapterConfig();
            config.ConfigureServicesMappings();
            return new Mapper(config);
        }
    }
}