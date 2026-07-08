using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.Admin
{
    /// <summary>
    /// The Admin homepage class.
    /// </summary>
    public class AdminHomePage : ViewYourFundingBasePage
    {
        /// <summary>
        /// The guidance links.
        /// </summary>
        private static readonly IReadOnlyCollection<string> TileHeaders = new ReadOnlyCollection<string>(new List<string>
        {
            "General settings",
            "Funding streams Settings",
            "Layout management settings",
            "Document generation actions"
        });

        #region Actions

        /// <summary>
        /// Navigates to page.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void NavigateToPageViaLogin(string username, string password)
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();
            LoginPage.Open();
            LoginPage.Login(username, password);

            GotoWithoutMainTitle("view-latest-funding/admin/home");
        }

        /// <summary>
        /// Navigates to page.
        /// </summary>
        public static void NavigateToPage()
        {
            GotoWithoutMainTitle("view-latest-funding/admin/home");
        }

        /// <summary>
        /// Clicks the on tile.
        /// </summary>
        /// <param name="tileHeader">The tile header.</param>
        public static void ClickOnTile(string tileHeader)
        {
            var targetTile = AdminTiles.First(tile =>
                tile.FindElement(By.TagName("h2")).Text.Contains(tileHeader, StringComparison.InvariantCultureIgnoreCase));

            targetTile.FindElement(By.ClassName("button-get-started"))
                .MoveAndClick();
        }

        #endregion


        #region Assertions

        public static void EnsureAdminTiles()
        {
            AdminTiles.Count.Should().BeGreaterOrEqualTo(3);
            foreach (var header in TileHeaders)
            {
                AdminTiles.Any(tile =>
                    tile.FindElement(By.TagName("h2")).Text.Contains(header, StringComparison.InvariantCultureIgnoreCase)).Should().BeTrue();
            }
        }

        #endregion


        #region Page Elements

        /// <summary>
        /// Gets the section1 div.
        /// </summary>
        /// <value>
        /// The section1 div.
        /// </value>
        protected static IReadOnlyList<IWebElement> AdminTiles => Driver.Instance.FindElements(By.ClassName("transaction"));

        #endregion

    }
}