using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Testing;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The FindAnOrganisationPageTests class.
    /// </summary>
    /// <seealso cref="BaseRegressionTest" />
    [TestClass]
    [Ignore]
    public class FindAnOrganisationPageTests : BaseRegressionTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindAnOrganisationPageTests"/> class.
        /// </summary>
        public FindAnOrganisationPageTests()
        {
            DisplayViewYourFunding = true;
        }

        /// <summary>
        /// Basics the layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void BasicLayout()
        {
            // Arrange
            FindAnOrganisationPage.NavigateToPage();

            // Assert
            FindAnOrganisationPage.EnsureCurrentPage();
            FindAnOrganisationPage.EnsureBreadcrumbs();
            FindAnOrganisationPage.EnsureOrganisationsOptionsAvailable();
            FindAnOrganisationPage.EnsureErrorLinkIsHidden();
            FindAnOrganisationPage.EnsureProviderOptionInputFieldIsDisplayed(false);
            FindAnOrganisationPage.EnsureLaCodeOptionInputFieldIsDisplayed(false);
        }

        /// <summary>
        /// FindAnOrganisationPage click provider choice.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void FindAnOrganisationPage_ClickProviderChoice()
        {
            // Arrange Act
            FindAnOrganisationPage.NavigateToPage();
            FindAnOrganisationPage.ClickProviderOptionButton();

            // Assert
            FindAnOrganisationPage.EnsureCurrentPage();
            FindAnOrganisationPage.EnsureProviderOptionInputFieldIsDisplayed(true);
            FindAnOrganisationPage.EnsureLaCodeOptionInputFieldIsDisplayed(false);
        }

        /// <summary>
        /// FindAnOrganisationPage click la code choice.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void FindAnOrganisationPage_ClickLaCodeChoice()
        {
            // Arrange Act
            FindAnOrganisationPage.NavigateToPage();
            FindAnOrganisationPage.ClickLaCodeOptionButton();

            // Assert
            FindAnOrganisationPage.EnsureCurrentPage();
            FindAnOrganisationPage.EnsureLaCodeOptionInputFieldIsDisplayed(true);
            FindAnOrganisationPage.EnsureProviderOptionInputFieldIsDisplayed(false);
        }

        /// <summary>
        /// FindAnOrganisationPage has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void FindAnOrganisationPage_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            FindAnOrganisationPage.NavigateToPage();

            // Assert
            FindAnOrganisationPage.EnsureCurrentPage();
            FindAnOrganisationPage.EnsureAndClickBreadcrumb("Choose how to view funding");
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// FindAnOrganisationPage displays error message where no provider is entered.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void FindAnOrganisationPage_DisplaysErrorMessage_WhereNoProviderIsEntered()
        {
            // Arrange Act
            FindAnOrganisationPage.NavigateToPage();
            FindAnOrganisationPage.ClickProviderOptionButton();
            FindAnOrganisationPage.SubmitProviderSearch();

            // Assert
            FindAnOrganisationPage.EnsureCurrentPage();
            FindAnOrganisationPage.EnsureErrorLinkisCorrect("Provider");
            FindAnOrganisationPage.EnsureLaCodeOptionInputFieldIsDisplayed(false);
            FindAnOrganisationPage.EnsureProviderOptionInputFieldIsDisplayed(true);
        }

        /// <summary>
        /// FindAnOrganisationPage displays error message where no la is entered.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void FindAnOrganisationPage_DisplaysErrorMessage_WhereNoLaIsEntered()
        {
            // Arrange Act
            FindAnOrganisationPage.NavigateToPage();
            FindAnOrganisationPage.ClickLaCodeOptionButton();
            FindAnOrganisationPage.SubmitLocalAuthoritySearch();

            // Assert
            FindAnOrganisationPage.EnsureCurrentPage();
            FindAnOrganisationPage.EnsureErrorLinkisCorrect("LaCode");
            FindAnOrganisationPage.EnsureLaCodeOptionInputFieldIsDisplayed(true);
            FindAnOrganisationPage.EnsureProviderOptionInputFieldIsDisplayed(false);
        }

        /// <summary>
        /// FindAnOrganisationPage error message cleared when option changed after error message.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void FindAnOrganisationPage_ErrorMessageCleared_WhenOptionChangedAfterErrorMessage()
        {
            // Arrange Act
            FindAnOrganisationPage.NavigateToPage();
            FindAnOrganisationPage.ClickLaCodeOptionButton();
            FindAnOrganisationPage.SubmitLocalAuthoritySearch();

            // Assert
            FindAnOrganisationPage.EnsureCurrentPage();
            FindAnOrganisationPage.EnsureErrorLinkisCorrect("LaCode");
            FindAnOrganisationPage.ClickProviderOptionButton();
            FindAnOrganisationPage.EnsureErrorLinkIsHidden();
            FindAnOrganisationPage.EnsureLaCodeOptionInputFieldIsDisplayed(false);
            FindAnOrganisationPage.EnsureProviderOptionInputFieldIsDisplayed(true);
        }
    }
}
