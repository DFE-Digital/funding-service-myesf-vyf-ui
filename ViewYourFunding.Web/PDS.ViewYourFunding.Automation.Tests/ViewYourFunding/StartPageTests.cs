using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Testing;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The StartPageTests class.
    /// </summary>
    /// <seealso cref="BaseRegressionTest" />
    [TestClass]
    [Ignore]
    public class StartPageTests : BaseRegressionTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartPageTests"/> class.
        /// </summary>
        public StartPageTests()
        {
            DisplayViewYourFunding = true;
        }

        /// <summary>
        /// StartPage correct header text.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void StartPage_CorrectHeaderText()
        {
            // Arrange Act
            StartPage.Open();

            // Assert
            StartPage.EnsureCurrentPage("View latest funding");
        }
    }
}
