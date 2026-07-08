using FluentAssertions;
using Moq;
using PDS.VYF.Services.Helpers;
using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;
using PDS.VYF.Services.Models.ViewDataModels;
using System.Globalization;

namespace PDS.VYF.Services.Tests.Helpers.ChildHistoryHelperTests
{
    /// <summary>
    /// The Child History Helper Tests.
    /// </summary>
    [TestClass, TestCategory("Unit")]
    public class ChildHistoryHelperTests
    {
        private MockRepository mockRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildHistoryHelperTests"/> class.
        /// </summary>
        public ChildHistoryHelperTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }

        [TestMethod]
        [DataRow(false, false, "New", "Initial allocation.")]
        [DataRow(false, false, "Updated", "Revised allocation.")]

        //InYearOpener is false, which falls under scenarios 09/10/19/20.
        public void GetChildHistoryModel_ForMainAllocation_NewUpdated_ExpectedBehavior(bool inYearOpener, bool isIndicative, string statementType, string expectedVariationReason)
        {
            //Arrange
            var loggedInChildModel = new LoggedInChildModel()
            {
                FundingStreamCode = "GAG",
                FundingPeriodCode = "AC-2526",
                StatusChangedDateOnly = new DateTime(2025, 08, 28),
                InYearOpener = inYearOpener,
                IsIndicative = isIndicative,
                StatementType = statementType,
            };

            var actualModel = new ChildHistoryPageRow();

            //Act
            actualModel = ChildHistoryHelper.GetChildHistoryModelInaYear(loggedInChildModel, actualModel, true);

            // Assert
            actualModel.VariationReason.Equals(expectedVariationReason);
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        [DataRow(true, true, "New", "Indicative part year allocation statement")]
        [DataRow(true, true, "Updated", "Revised indicative part year allocation statement")]

        //It's indicative and opened between April and August, which falls under scenarios 01 and 11.
        public void GetChildHistoryModel_ForIndicative_OpenedAprilToAugust_NewUpdated_ExpectedBehavior(bool inYearOpener, bool isIndicative, string statementType, string expectedVariationReason)
        {
            //Arrange
            var loggedInChildModel = new LoggedInChildModel()
            {
                FundingStreamCode = "GAG",
                FundingPeriodCode = "AC-2526",
                StatusChangedDateOnly = new DateTime(2025, 08, 28),
                InYearOpener = inYearOpener,
                IsIndicative = isIndicative,
                StatementType = statementType,
                DateOpened = DateTime.ParseExact("01/04/2025", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            };

            var actualModel = new ChildHistoryPageRow();

            //Act
            actualModel = ChildHistoryHelper.GetChildHistoryModelInaYear(loggedInChildModel, actualModel, true);

            // Assert
            actualModel.VariationReason.Equals(expectedVariationReason);
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        [DataRow(true, true, "New", "Indicative part year allocation statement")]
        [DataRow(true, true, "Updated", "Revised indicative part year allocation statement")]

        //It's indicative and opened between Sept and Mar, which falls under scenarios 06 and 16.
        public void GetChildHistoryModel_ForIndicative_OpenedBetSeptAndMar_NewUpdated_ExpectedBehavior(bool inYearOpener, bool isIndicative, string statementType, string expectedVariationReason)
        {
            //Arrange
            var loggedInChildModel = new LoggedInChildModel()
            {
                FundingStreamCode = "GAG",
                FundingPeriodCode = "AC-2526",
                StatusChangedDateOnly = new DateTime(2025, 08, 28),
                InYearOpener = inYearOpener,
                IsIndicative = isIndicative,
                StatementType = statementType,
                DateOpened = DateTime.ParseExact("01/09/2025", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            };

            var actualModel = new ChildHistoryPageRow();

            //Act
            actualModel = ChildHistoryHelper.GetChildHistoryModelInaYear(loggedInChildModel, actualModel, true);

            // Assert
            actualModel.VariationReason.Equals(expectedVariationReason);
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        [DataRow(true, false, "New", "Initial allocation statement")]
        [DataRow(true, false, "Updated", "Revised allocation statement")]

        //It's In Year Opener, opened between April and August and IsSecondYearInYearOpener is true, which falls under scenarios 04/05/14/15.
        public void GetChildHistoryModel_ForInYearOpeners_Final_SecondYear_OpenedAprilToAugust_NewUpdated_ExpectedBehavior(bool inYearOpener, bool isIndicative, string statementType, string expectedVariationReason)
        {
            //Arrange
            var loggedInChildModel = new LoggedInChildModel()
            {
                FundingStreamCode = "GAG",
                FundingPeriodCode = "AC-2526",
                StatusChangedDateOnly = new DateTime(2025, 08, 28),
                InYearOpener = inYearOpener,
                IsIndicative = isIndicative,
                StatementType = statementType,
                YearFrom = 2025,
                YearTo = 2025,
                DateOpened = DateTime.ParseExact("01/04/2025", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            };

            var actualModel = new ChildHistoryPageRow();

            //Act
            actualModel = ChildHistoryHelper.GetChildHistoryModelInaYear(loggedInChildModel, actualModel, true);

            // Assert
            actualModel.VariationReason.Equals(expectedVariationReason);
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        [DataRow(true, false, "New", "Final part year allocation statement")]
        [DataRow(true, false, "Updated", "Revised final part year allocation statement")]

        //It's In Year Opener, opened between Sept and Mar, which falls under scenarios 07/08/17/18.
        public void GetChildHistoryModel_ForInYearOpeners_Final_OpenedBetSeptAndMar_NewUpdated_ExpectedBehavior(bool inYearOpener, bool isIndicative, string statementType, string expectedVariationReason)
        {
            //Arrange
            var loggedInChildModel = new LoggedInChildModel()
            {
                FundingStreamCode = "GAG",
                FundingPeriodCode = "AC-2526",
                StatusChangedDateOnly = new DateTime(2025, 08, 28),
                InYearOpener = inYearOpener,
                IsIndicative = isIndicative,
                StatementType = statementType,
                YearFrom = 2025,
                YearTo = 2026,
                DateOpened = DateTime.ParseExact("01/09/2025", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            };

            var actualModel = new ChildHistoryPageRow();

            //Act
            actualModel = ChildHistoryHelper.GetChildHistoryModelInaYear(loggedInChildModel, actualModel, true);

            // Assert
            actualModel.VariationReason.Equals(expectedVariationReason);
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        [DataRow(true, false, "New", "Final part year allocation statement")]
        [DataRow(true, false, "Updated", "Revised final part year allocation statement")]

        //It's In Year Opener, opened April to August and IsCurrentYearOpener is true, which falls under scenarios 02/03/12/13.
        public void GetChildHistoryModel_ForInYearOpeners_Final_OpenedAprilToAugust_NewUpdated_ExpectedBehavior(bool inYearOpener, bool isIndicative, string statementType, string expectedVariationReason)
        {
            //Arrange
            var loggedInChildModel = new LoggedInChildModel()
            {
                FundingStreamCode = "GAG",
                FundingPeriodCode = "AC-2526",
                StatusChangedDateOnly = new DateTime(2025, 05, 11),
                InYearOpener = inYearOpener,
                IsIndicative = isIndicative,
                StatementType = statementType,
                YearFrom = 2025,
                YearTo = 2026,
                DateOpened = DateTime.ParseExact("01/04/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            };

            var actualModel = new ChildHistoryPageRow();

            //Act
            actualModel = ChildHistoryHelper.GetChildHistoryModelInaYear(loggedInChildModel, actualModel, true);

            // Assert
            actualModel.VariationReason.Equals(expectedVariationReason);
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UpdateStatementTypeByGroupingScenariosAll_ExpectedBehaviour()
        {
            // Arrange
            List<LoggedInChildModel> loggedInChildModel = GetOriginalInputScenarioList();
            List<LoggedInChildModel> expectedLoggedInChildModel = GetOriginalExpectedList();

            // Act
            ChildHistoryHelper.UpdateStatementTypeByGroupingScenarios(loggedInChildModel);

            // Assert
            loggedInChildModel.Should().BeEquivalentTo(expectedLoggedInChildModel);
        }

        public List<LoggedInChildModel> GetOriginalInputScenarioList()
        {
            return new List<LoggedInChildModel>
            {
                CreateChildModel("GAG-AC-2526-12345678-1_0", new DateTime(2025, 09, 10), "New", "1", true, true, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-2_0", new DateTime(2025, 09, 20), "Updated", "2", true, true, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-3_0", new DateTime(2025, 10, 03), "Updated", "3", false, true, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-4_0", new DateTime(2025, 10, 17), "Updated", "4", false, true, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-5_0", new DateTime(2025, 11, 06), "Updated", "5", false, false, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-6_0", new DateTime(2025, 11, 28), "Updated", "6", false, false, 2025, 2026)
            };
        }

        public List<LoggedInChildModel> GetOriginalExpectedList()
        {
            return new List<LoggedInChildModel>
            {
                CreateChildModel("GAG-AC-2526-12345678-1_0", new DateTime(2025, 09, 10), "New", "1", true, true, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-2_0", new DateTime(2025, 09, 20), "Updated", "2", true, true, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-3_0", new DateTime(2025, 10, 03), "New", "3", false, true, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-4_0", new DateTime(2025, 10, 17), "Updated", "4", false, true, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-5_0", new DateTime(2025, 11, 06), "New", "5", false, false, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-6_0", new DateTime(2025, 11, 28), "Updated", "6", false, false, 2025, 2026)
            };
        }

        [TestMethod]
        public void UpdateStatementTypeByGroupingScenariosIndicativeAndFinalPartYear_ExpectedBehaviour()
        {
            // Arrange
            List<LoggedInChildModel> loggedInChildModel = GetOriginalInputScenarioList_IndicativeAndFinalPartYear();
            List<LoggedInChildModel> expectedLoggedInChildModel = GetOriginalExpectedList_IndicativeAndFinalPartYear();

            // Act
            ChildHistoryHelper.UpdateStatementTypeByGroupingScenarios(loggedInChildModel);

            // Assert
            loggedInChildModel.Should().BeEquivalentTo(expectedLoggedInChildModel);
        }

        public List<LoggedInChildModel> GetOriginalInputScenarioList_IndicativeAndFinalPartYear()
        {
            return new List<LoggedInChildModel>
            {
                CreateChildModel("GAG-AC-2526-12345678-1_0", new DateTime(2025, 09, 10), "New", "1", true, true, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-2_0", new DateTime(2025, 09, 20), "Updated", "2", true, true, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-3_0", new DateTime(2025, 10, 03), "Updated", "3", true, true, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-4_0", new DateTime(2025, 10, 17), "Updated", "4", false, true, 2025, 2026),
            };
        }

        public List<LoggedInChildModel> GetOriginalExpectedList_IndicativeAndFinalPartYear()
        {
            return new List<LoggedInChildModel>
            {
                CreateChildModel("GAG-AC-2526-12345678-1_0", new DateTime(2025, 09, 10), "New", "1", true, true, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-2_0", new DateTime(2025, 09, 20), "Updated", "2", true, true, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-3_0", new DateTime(2025, 10, 03), "Updated", "3", true, true, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-4_0", new DateTime(2025, 10, 17), "New", "4", false, true, 2025, 2026),
            };
        }

        [TestMethod]
        public void UpdateStatementTypeByGroupingScenarios_MainAllocations_ExpectedBehaviour()
        {
            // Arrange
            List<LoggedInChildModel> loggedInChildModel = GetOriginalInputScenarioList_MainAllocations();
            List<LoggedInChildModel> expectedLoggedInChildModel = GetOriginalExpectedList_MainAllocations();

            // Act
            ChildHistoryHelper.UpdateStatementTypeByGroupingScenarios(loggedInChildModel);

            // Assert
            loggedInChildModel.Should().BeEquivalentTo(expectedLoggedInChildModel);
        }

        public List<LoggedInChildModel> GetOriginalInputScenarioList_MainAllocations()
        {
            return new List<LoggedInChildModel>
            {
                CreateChildModel("GAG-AC-2425-12345678-1_0", new DateTime(2024, 02, 28), "New", "1", false, false, 2024, 2025),
                CreateChildModel("GAG-AC-2526-12345678-1_0", new DateTime(2025, 01, 28), "New", "1", false, false, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-2_0", new DateTime(2025, 02, 07), "Updated", "2", false, false, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-3_0", new DateTime(2025, 02, 19), "Updated", "3", false, false, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-4_0", new DateTime(2025, 11, 04), "Updated", "4", false, false, 2025, 2026),
                CreateChildModel("GAG-AC-2627-12345678-1_0", new DateTime(2025, 12, 05), "New", "1", false, false, 2026, 2027),
            };
        }

        public List<LoggedInChildModel> GetOriginalExpectedList_MainAllocations()
        {
            return new List<LoggedInChildModel>
            {
                CreateChildModel("GAG-AC-2425-12345678-1_0", new DateTime(2024, 02, 28), "New", "1", false, false, 2024, 2025),
                CreateChildModel("GAG-AC-2526-12345678-1_0", new DateTime(2025, 01, 28), "New", "1", false, false, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-2_0", new DateTime(2025, 02, 07), "Updated", "2", false, false, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-3_0", new DateTime(2025, 02, 19), "Updated", "3", false, false, 2025, 2026),
                CreateChildModel("GAG-AC-2526-12345678-4_0", new DateTime(2025, 11, 04), "Updated", "4", false, false, 2025, 2026),
                CreateChildModel("GAG-AC-2627-12345678-1_0", new DateTime(2025, 12, 05), "New", "1", false, false, 2026, 2027),
            };
        }

        public LoggedInChildModel CreateChildModel(
            string? fundingStreamId = null,
            DateTime? statusChangedDate = null,
            string? statementType = null,
            string? fundingVersionInt = null,
            bool isIndicative = false,
            bool isYearOpener = false,
            int? yearFrom = null,
            int? yearTo = null)
        {
            return new LoggedInChildModel
            {
                Id = fundingStreamId,
                StatusChangedDateOnly = statusChangedDate,
                StatementType = statementType,
                IsIndicative = isIndicative,
                InYearOpener = isYearOpener,
                YearFrom = yearFrom,
                YearTo = yearTo
            };
        }
    }
}
