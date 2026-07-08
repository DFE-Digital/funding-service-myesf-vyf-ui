using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Testing;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The WhichAllocationPageTests class.
    /// </summary>
    /// <seealso cref="BaseRegressionTest" />
    [TestClass]
    [Ignore]
    public class WhichAllocationPageTests : BaseRegressionTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhichAllocationPageTests"/> class.
        /// </summary>
        public WhichAllocationPageTests()
        {
            DisplayViewYourFunding = true;
        }

        /// <summary>
        /// WhichAllocationPage basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void WhichAllocationPage_BasicLayout()
        {
            // Arrange Act
            WhichAllocationPage.NavigateToPage();

            // Assert
            WhichAllocationPage.EnsureCurrentPage();
            WhichAllocationPage.EnsureBreadcrumbs();
        }

        /// <summary>
        /// WhichAllocationPage has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void WhichAllocationPage_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            WhichAllocationPage.NavigateToPage();

            // Assert
            WhichAllocationPage.EnsureAndClickBreadcrumb("Choose how to view funding");
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// WhichAllocationPage errors hidden on first load.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void WhichAllocationPage_ErrorsHiddenOnFirstLoad()
        {
            // Arrange Act
            WhichAllocationPage.NavigateToPage();

            // Assert
            WhichAllocationPage.EnsureCurrentPage();
            WhichAllocationPage.EnsureErrorComponents(false);
        }

        /// <summary>
        /// WhichAllocationPage errors shown after clicking continue without selecting an option.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void WhichAllocationPage_ErrorsShownAfterClickingContinueWithoutSelectingAnOption()
        {
            // Arrange
            WhichAllocationPage.NavigateToPage();
            WhichAllocationPage.EnsureCurrentPage();

            // Act
            WhichAllocationPage.ClickContinueButton();

            // Assert
            WhichAllocationPage.EnsureErrorComponents(true);
        }

        /// <summary>
        /// WhichAllocationPage progressive disclosure hidden on load.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void WhichAllocationPage_ProgressiveDisclosureHiddenOnLoad()
        {
            // Arrange Act
            WhichAllocationPage.NavigateToPage();

            // Assert
            WhichAllocationPage.EnsureProgressiveDisclosureContent(false);
        }

        /// <summary>
        /// WhichAllocationPage progressive disclosure shown on button click.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void WhichAllocationPage_ProgressiveDisclosureShownOnButtonClick()
        {
            // Arrange
            WhichAllocationPage.NavigateToPage();

            // Act
            WhichAllocationPage.ClickProgressiveDisclosureButton();

            // Assert
            WhichAllocationPage.EnsureProgressiveDisclosureContent(true);
        }

        /// <summary>
        /// WhichAllocationPage progressive disclosure hidden on second button click.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void WhichAllocationPage_ProgressiveDisclosureHiddenOnSecondButtonClick()
        {
            // Arrange
            WhichAllocationPage.NavigateToPage();

            // Act
            WhichAllocationPage.ClickProgressiveDisclosureButton();
            WhichAllocationPage.ClickProgressiveDisclosureButton();

            // Assert
            WhichAllocationPage.EnsureProgressiveDisclosureContent(false);
        }
    }
}
