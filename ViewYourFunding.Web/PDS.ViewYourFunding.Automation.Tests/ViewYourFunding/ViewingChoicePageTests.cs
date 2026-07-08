using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Testing;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The ViewingChoicePageTests class.
    /// </summary>
    /// <seealso cref="BaseRegressionTest" />
    [TestClass]
    [Ignore]
    public class ViewingChoicePageTests : BaseRegressionTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewingChoicePageTests"/> class.
        /// </summary>
        public ViewingChoicePageTests()
        {
            DisplayViewYourFunding = true;
        }

        /// <summary>
        /// ViewingChoicePage correct header text.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ViewingChoicePage_CorrectHeaderText()
        {
            // Arrange
            StartPage.Open();

            // Act
            StartPage.ClickStartButton();

            // Assert
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// ViewingChoicePage errors hidden on first load.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ViewingChoicePage_ErrorsHiddenOnFirstLoad()
        {
            // Arrange
            StartPage.Open();

            // Act
            StartPage.ClickStartButton();
            ViewingChoicePage.EnsureCurrentPage();

            // Assert
            ViewingChoicePage.EnsureErrorComponents(false);
        }

        /// <summary>
        /// ViewingChoicePage errors shown after clicking continue without selecting an option.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ViewingChoicePage_ErrorsShownAfterClickingContinueWithoutSelectingAnOption()
        {
            // Arrange
            StartPage.Open();
            StartPage.ClickStartButton();
            ViewingChoicePage.EnsureCurrentPage();

            // Act
            ViewingChoicePage.ClickContinueButton();

            // Assert
            ViewingChoicePage.EnsureErrorComponents(true);
        }
    }
}
