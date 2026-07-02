using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The LocalAuthorityDSGFundingBreakdownPage class.
    /// </summary>
    public class LocalAuthorityDSGFundingBreakdownPage : ViewYourFundingBasePage
    {
        #region Actions

        /// <summary>
        /// Opens the specified year from.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="localAuthorityName">Name of the local authority.</param>
        /// <param name="blockName">Name of the block.</param>
        /// <param name="multipleSearchResults">if set to <c>true</c> [multiple search results].</param>
        public static void Open(int yearFrom, int yearTo, string searchTerm, string localAuthorityName, string blockName, bool multipleSearchResults = true)
        {
            StartPage.Open();
            StartPage.ClickStartButton();
            ViewingChoicePage.SelectOrganisationOption();
            ViewingChoicePage.ClickContinueButton();
            FindAnOrganisationPage.ClickLaCodeOptionButton();
            FindAnOrganisationPage.InputLocalAuthoritySearchText(searchTerm);
            FindAnOrganisationPage.SubmitLocalAuthoritySearch();

            if (multipleSearchResults)
            {
                LocalAuthorityDidYouMeanPage.ClickLocalAuthorityLink(localAuthorityName); // click through from results page
            }

            LocalAuthorityStatementPage.ClickFundingBlock(blockName, yearFrom, yearTo);
        }

        /// <summary>
        /// Highes the needs disability access fund shows.
        /// </summary>
        public static void HighNeedsDisabilityAccessFundShows()
        {
            DisabilityAccessFund.Text.Should().Be("Disability access fund");
        }

        /// <summary>
        /// Highes the needs hospital does not show.
        /// </summary>
        public static void HighNeedsHospitalDoesNotShow()
        {
            HighNeedsHospital.Should().BeNull();
        }

        /// <summary>
        /// Clicks the history link.
        /// </summary>
        public static void ClickHistoryLink()
        {
            HistoryLink.FindElement(By.TagName("a")).MoveAndClick();
        }

        /// <summary>
        /// Clicks the schools tab.
        /// </summary>
        public static void ClickSchoolsTab()
        {
            SchoolsTab.MoveAndClick();
        }

        /// <summary>
        /// Clicks the central school services tab.
        /// </summary>
        public static void ClickCentralSchoolServicesTab()
        {
            CentralSchoolServicesTab.MoveAndClick();
        }

        /// <summary>
        /// Clicks the high needs tab.
        /// </summary>
        public static void ClickHighNeedsTab()
        {
            HighNeedsTab.MoveAndClick();
        }

        /// <summary>
        /// Clicks the early years tab.
        /// </summary>
        public static void ClickEarlyYearsTab()
        {
            EarlyYearsTab.MoveAndClick();
        }

        /// <summary>
        /// Clicks the format request link.
        /// </summary>
        public static void ClickFormatRequestLink()
        {
            FormatRequestLink.MoveAndClick();
        }

        /// <summary>
        /// Clicks the how we calculate this link.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        public static void ClickHowWeCalculateThisLink(string elementId)
        {
            SchoolTabHowWeCalculateThisLink(elementId).Click();
            Thread.Sleep(100);
        }

        #endregion


        #region Assertions

        /// <summary>
        /// Ensures the guidance link.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void EnsureGuidanceLink(int yearFrom, int yearTo)
        {
            var listItem = GuidanceLinkListItem;
            listItem.Displayed.Should().BeTrue();
            listItem.Text.Should().Be($"Get more general guidance on DSG: {yearFrom} to {yearTo}.");

            var link = listItem.FindElement(By.TagName("a"));
            link.GetAttribute("href").Should()
                .Contain($"www.gov.uk/government/publications/dedicated-schools-grant-dsg-{yearFrom}-to-{yearTo}");
        }

        /// <summary>
        /// Ensures the open document link.
        /// </summary>
        public static void EnsureOpenDocumentLink()
        {
            var listItem = OpenDocumentFormatLink;
            listItem.Displayed.Should().BeTrue();
            listItem.GetAttribute("href").Should().Contain("www.gov.uk/guidance/using-open-document-formats-odf-in-your-organisation");
        }

        /// <summary>
        /// Ensures the spreadsheet link.
        /// </summary>
        /// <param name="organisation">The organisation.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void EnsureSpreadsheetLink(string organisation, int yearFrom, int yearTo)
        {
            var linkText = $"Download DSG allocation for {organisation} {yearFrom} to {yearTo}";

            var documentLink = DocumentDownloadContainer.FindElement(By.LinkText(linkText));
            documentLink.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Ensures the early years universal entitlement.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureEarlyYearsUniversalEntitlement(string amount)
        {
            EarlyYearsUniversalEntitlement.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the early years additional15.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureEarlyYearsAdditional15(string amount)
        {
            EarlyYearsAdditional15.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the allocation amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureAllocationAmount(string amount)
        {
            AllocationAmount.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the early years2 year entitlement.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureEarlyYears2YearEntitlement(string amount)
        {
            EarlyYears2YearEntitlement.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the total amount.
        /// </summary>
        /// <param name="area">The area.</param>
        public static void EnsureTotalAmount(string area)
        {
            switch (area)
            {
                case "High needs":
                    HighNeedsTotalAmount.Text.Should().Be("£34,372,825");
                    break;
                case "Early years":
                    EarlyYearsTotalAmount.Text.Should().Be("£17,734,009");
                    break;
            }
        }

        /// <summary>
        /// Ensures the early years disability access.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureEarlyYearsDisabilityAccess(string amount)
        {
            EarlyYearsDisabilityAccess.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the early years pupil premium.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureEarlyYearsPupilPremium(string amount)
        {
            EarlyYearsPupilPremium.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the high needs NFF amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureHighNeedsNFFAmount(string amount)
        {
            HighNeedsNFFTotal.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the early years maintained nursery.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureEarlyYearsMaintainedNursery(string amount)
        {
            EarlyYearsMaintainedNursery.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the high needs basic entitlement amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureHighNeedsBasicEntitlementAmount(string amount)
        {
            HighNeedsBasicEntitlementTotal.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the high need deductions direct funding amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureHighNeedDeductionsDirectFundingAmount(string amount)
        {
            HighNeedDeductionsDirectFundingAmount.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the high need maintained mainstream.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureHighNeedMaintainedMainstream(string amount)
        {
            HighNeedMaintainedMainstream.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the high need feilp amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureHighNeedFEILPAmount(string amount)
        {
            HighNeedFEILPAmount.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the history link.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void EnsureHistoryLink(int yearFrom, int yearTo)
        {
            var listItem = HistoryLink;
            listItem.Displayed.Should().BeTrue();
            listItem.Text.Should().Be($"You can view all versions of this allocation for the financial year {yearFrom} to {yearTo} and previous years on the allocation history.");

            var link = listItem.FindElement(By.TagName("a"));
            link.GetAttribute("href").Should()
                .Contain($"local-authority/allocation-history/dedicated-schools-grant/202?searchTerm=ca");
        }

        /// <summary>
        /// Ensures the high needs additional funding special amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureHighNeedsAdditionalFundingSpecialAmount(string amount)
        {
            HighNeedsAdditionalFundingSpecialTotal.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the high needs hospital education and teachers pay/pension amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureHighNeedsHospitalEducationTeachersPayPensionAmount(string amount)
        {
            HighNeedsHospitalEducationTeachersPayPensionTotal.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the high needs import export amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureHighNeedsImportExportAmount(string amount)
        {
            HighNeedsImportExportTotal.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the high needs additional funding amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureHighNeedsAdditionalFundingAmount(string amount)
        {
            HighNeedsAdditionalFundingTotal.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the format request email link shown.
        /// </summary>
        /// <param name="organisation">The organisation.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void EnsureFormatRequestEmailLinkShown(string organisation, int yearFrom, int yearTo)
        {
            EnsureFormatRequestEmailLinkNotShownWhenSectionHasBeenCollapsed(1);
            var requestSubject = $"Request for 'DSG allocation for {organisation} {yearFrom} to {yearTo}' in an alternative format";
            var target = FormatRequestEmailLinks[0].GetAttribute("href");

            target.Should().Contain("mailto:");
            target.Should().Contain("subject=" + Uri.EscapeDataString(requestSubject));
            target.Should().Contain(Uri.EscapeDataString("Original format: ods"));
        }

        /// <summary>
        /// Ensures all post16 special educational needs places match.
        /// </summary>
        /// <param name="amounts">The amounts.</param>
        public static void EnsureAllPost16SpecialEducationalNeedsPlacesMatch(params string[] amounts)
        {
            Post16SpecialEducationalNeedsPlaces.Count.Should().Be(amounts.Length);

            for (int i = 0; i < amounts.Length; i++)
            {
                Post16SpecialEducationalNeedsPlaces[i].Text.Should().Be(amounts[i]);
            }
        }

        /// <summary>
        /// Ensures the high need total before deductions.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureHighNeedTotalBeforeDeductions(string amount)
        {
            HighNeedsTotalBeforeDeduction.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the format request email link not shown when section has been collapsed.
        /// </summary>
        /// <param name="numberOfLinks">The number of links.</param>
        public static void EnsureFormatRequestEmailLinkNotShownWhenSectionHasBeenCollapsed(int numberOfLinks)
        {
            FormatRequestEmailLinks.Should().HaveCount(numberOfLinks);  // zero collapsed
        }

        /// <summary>
        /// Ensures the high need ap amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureHighNeedAPAmount(string amount)
        {
            HighNeedsAPAmountTotal.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the correct tab has been selected.
        /// </summary>
        /// <param name="expectedTab">The expected tab.</param>
        public static void EnsureCorrectTabHasBeenSelected(string expectedTab)
        {
            var selectedTab = Driver.Instance.WaitToFindElements(By.ClassName("govuk-tabs__tab--selected"));
            selectedTab.Should().HaveCount(1);
            selectedTab[0].Text.Should().Be(expectedTab);
        }

        /// <summary>
        /// Ensures the high need maintained special amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void EnsureHighNeedMaintainedSpecialAmount(string amount)
        {
            HighNeedMaintainedSpecialAmounTotal.Text.Should().Be(amount);
        }

        /// <summary>
        /// Ensures the find out why partial pupil numbers are used link.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="linkShouldBeDisplayed">if set to <c>true</c> [link should be displayed].</param>
        public static void EnsureFindOutWhyPartialPupilNumbersAreUsedLink(int yearFrom, int yearTo, bool linkShouldBeDisplayed)
        {
            var listItem = FindOutWhyPartialPupilNumbersAreUsedListItem;

            if (linkShouldBeDisplayed)
            {
                listItem.Displayed.Should().BeTrue();
                listItem.Text.Should().Be("Find out why partial pupil numbers are used");

                var link = listItem.FindElement(By.TagName("a"));
                link.GetAttribute("href").Should()
                    .Contain(
                        $"https://www.gov.uk/government/publications/dedicated-schools-grant-dsg-{yearFrom}-to-{yearTo}/dsg-technical-note-{yearFrom}-to-{yearTo}#pupil-number-information");
            }
            else
            {
                listItem.Displayed.Should().BeFalse();
            }
        }

        /// <summary>
        /// Ensures the DSG technical notes link is correct.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void EnsureDSGTechnicalNotesLinkIsCorrect(int yearFrom, int yearTo)
        {
            var listItem = DSGTechnicalNotesLinks(yearFrom, yearTo)[0];

            listItem.Displayed.Should().BeTrue();
            listItem.Text.Should().Contain($"DSG: technical note {yearFrom} to {yearTo}");

            listItem.GetAttribute("href").Should()
                .Contain($"www.gov.uk/government/publications/dedicated-schools-grant-dsg-{yearFrom}-to-{yearTo}/dsg-technical-note-{yearFrom}-to-{yearTo}");
        }

        /// <summary>
        /// Ensure that guidance links are correct for the selected tab.
        /// </summary>
        /// <param name="tabNumber">0 is Schools tab and 1 is CSS tab.</param>
        /// <param name="yearFrom">Year from.</param>
        /// <param name="yearTo">Year to.</param>
        public static void EnsureGuidanceListItems(int tabNumber, int yearFrom, int yearTo)
        {
            var unOrderedLists = GuidanceListItems;
            unOrderedLists.Count.Should().Be(2);

            var listItems = unOrderedLists[tabNumber].FindElements(By.TagName("li"));

            listItems.Count.Should().Be(2);

            for (int itemNumber = 0; itemNumber < listItems.Count; itemNumber++)
            {
                string text, url;
                if (itemNumber == 0)
                {
                    text = $"National funding formula tables for schools and high needs: {yearFrom} to {yearTo}";
                    url = $"www.gov.uk/government/publications/national-funding-formula-tables-for-schools-and-high-needs-{yearFrom}-to-{yearTo}";
                }
                else
                {
                    text = $"Pre-16 schools funding: guidance for {yearFrom} to {yearTo}";
                    url = $"www.gov.uk/government/publications/pre-16-schools-funding-local-authority-guidance-for-{yearFrom}-to-{yearTo}";
                }

                listItems[itemNumber].Text.Should().Be(text);

                var link = listItems[itemNumber].FindElement(By.TagName("a"));
                link.GetAttribute("href").Should()
                    .Contain(url);
            }
        }

        /// <summary>
        /// Ensures the print this page items.
        /// </summary>
        /// <param name="tabNumber">The tab number.</param>
        public static void EnsurePrintThisPageItems(int tabNumber)
        {
            PrintThisPageItems.Count.Should().Be(4);
            PrintThisPageItems[tabNumber].Text.Should().Be("Print this page");
            PrintThisPageItems[tabNumber].GetAttribute("href").Should().EndWith("#");
        }

        /// <summary>
        /// Ensures the school tab total.
        /// </summary>
        public static void EnsureSchoolTabTotal()
        {
            SchoolBlockTotal.Text.Should().Be("£116,935,309");
        }

        /// <summary>
        /// Ensures funding teacher pay protected amount.
        /// </summary>
        public static void EnsureFundingTeacherPayProtected()
        {
            FundingTeacherPayProtectedAmount.Text.Should().Be("£1,107");
        }

        /// <summary>
        /// Ensures total funding transferable amount.
        /// </summary>
        public static void EnsureTotalFundingTransferable()
        {
            TotalFundingTransferableAmount.Text.Should().Be("-£1");
        }

        /// <summary>
        /// Ensures the central school services tab total.
        /// </summary>
        public static void EnsureCentralSchoolServicesTabTotal()
        {
            CentralSchoolServicesBlockTotal.Text.Should().Be("£1,429,568");
        }

        /// <summary>
        /// Ensures the how we calculate this link toggles.
        /// </summary>
        /// <param name="numberOfLinks">The number of links.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void EnsureHowWeCalculateThisLinkToggles(int numberOfLinks, int yearFrom, int yearTo)
        {
            DSGTechnicalNotesLinks(yearFrom, yearTo).Should().HaveCount(numberOfLinks);
        }

        /// <summary>
        /// Ensures the table data.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <param name="expectedTableData">The expected table data.</param>
        /// <param name="alternativeExpectedTableData">Alternative expected table data (optional).</param>
        public static void EnsureTableData(
            IWebElement tableElement,
            KeyValuePair<string, string>[] expectedTableData,
            KeyValuePair<string, string>[] alternativeExpectedTableData = null)
        {
            for (var rowNumber = 0; rowNumber < expectedTableData.Length; rowNumber++)
            {
                var row = expectedTableData[rowNumber];

                try
                {
                    EnsureTableRow(
                        tableElement,
                        row.Key,
                        row.Value,
                        rowNumber);
                }
                catch
                {
                    var alternativeRow = alternativeExpectedTableData != null
                        && alternativeExpectedTableData.Length > rowNumber
                        ? alternativeExpectedTableData[rowNumber] : (KeyValuePair<string, string>?)null;

                    if (alternativeRow == null)
                    {
                        throw;
                    }

                    EnsureTableRow(
                       tableElement,
                       alternativeRow.Value.Key,
                       alternativeRow.Value.Value,
                       rowNumber);
                }
            }
        }

        /// <summary>
        /// Ensures the breadcrumbs and final text.
        /// </summary>
        /// <param name="finalText">The final text.</param>
        /// <param name="breadcrumbLinks">The breadcrumb links.</param>
        public static void EnsureBreadcrumbsAndFinalText(string finalText, string[] breadcrumbLinks)
        {
            EnsureFinalBreadcrumbText(finalText);
            EnsureBreadcrumbLinksText(breadcrumbLinks);
        }

        #endregion


        #region Helper Methods

        /// <summary>
        /// Ensures the table row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <param name="expectedKey">The expected key.</param>
        /// <param name="expectedText">The expected text.</param>
        /// <param name="rowNumber">The row number.</param>
        private static void EnsureTableRow(IWebElement tableElement, string expectedKey, string expectedText, int rowNumber = 0)
        {
            var key = GetColumnKeyNameFromTableRow(rowNumber, tableElement);
            key.Text.Should().Contain(expectedKey);
            GetColumnKeyValueFromTableRow(rowNumber, tableElement).Text.Replace(" ", string.Empty).Should().Contain(expectedText);
        }

        /// <summary>
        /// Gets the column key name from table row.
        /// </summary>
        /// <param name="rowNumber">The row number.</param>
        /// <param name="tableElement">The table element.</param>
        /// <returns>The web element.</returns>
        private static IWebElement GetColumnKeyNameFromTableRow(int rowNumber, IWebElement tableElement)
        {
            return tableElement.GetRow(rowNumber).GetCellByElementType(0, "th");
        }

        /// <summary>
        /// Gets the column key value from table row.
        /// </summary>
        /// <param name="rowNumber">The row number.</param>
        /// <param name="tableElement">The table element.</param>
        /// <returns>The web element.</returns>
        private static IWebElement GetColumnKeyValueFromTableRow(int rowNumber, IWebElement tableElement)
        {
            return tableElement.GetRow(rowNumber).GetCellByElementType(0, "td");
        }

        #endregion


        #region Page Elements

        /// <summary>
        /// Gets the early years universal entitlement.
        /// </summary>
        /// <value>
        /// The early years universal entitlement.
        /// </value>
        public static IWebElement EarlyYearsUniversalEntitlement
            => Driver.Instance.WaitToFindElement(By.XPath("//th[contains(text(),'funding allocation for universal entitlement for 3 and 4 year olds')]/following-sibling::td"));

        /// <summary>
        /// Gets the early years additional15.
        /// </summary>
        /// <value>
        /// The early years additional15.
        /// </value>
        public static IWebElement EarlyYearsAdditional15
            => Driver.Instance.WaitToFindElement(By.XPath("//th[contains(text(),'funding allocation for additional 15 hours entitlement for eligible working parents of 3 and 4 year olds')]/following-sibling::td"));

        /// <summary>
        /// Gets the early years2 year entitlement.
        /// </summary>
        /// <value>
        /// The early years2 year entitlement.
        /// </value>
        public static IWebElement EarlyYears2YearEntitlement
            => Driver.Instance.WaitToFindElement(By.XPath("//th[contains(text(),'funding allocation for 2 year old entitlement')]/following-sibling::td"));

        /// <summary>
        /// Gets the early years disability access.
        /// </summary>
        /// <value>
        /// The early years disability access.
        /// </value>
        public static IWebElement EarlyYearsDisabilityAccess
            => Driver.Instance.WaitToFindElement(By.XPath("//th[contains(text(),'Total funding allocation for disability access fund')]/following-sibling::td"));

        /// <summary>
        /// Gets the early years maintained nursery.
        /// </summary>
        /// <value>
        /// The early years maintained nursery.
        /// </value>
        public static IWebElement EarlyYearsMaintainedNursery
            => Driver.Instance.WaitToFindElement(By.XPath("//th[contains(text(),'supplementary funding allocation for maintained nursery schools')]/following-sibling::td"));

        /// <summary>
        /// Gets the early years pupil premium.
        /// </summary>
        /// <value>
        /// The early years pupil premium.
        /// </value>
        public static IWebElement EarlyYearsPupilPremium
            => Driver.Instance.WaitToFindElement(By.XPath("//th[contains(text(),'funding allocation for early years pupil premium')]/following-sibling::td"));

        /// <summary>
        /// Gets the high needs NFF total.
        /// </summary>
        /// <value>
        /// The high needs NFF total.
        /// </value>
        public static IWebElement HighNeedsNFFTotal
            => Driver.Instance.WaitToFindElement(By.XPath("//th[contains(text(),'National funding formula')]/parent::tr/parent::thead/following-sibling::tbody/descendant::td"));

        /// <summary>
        /// Gets the high needs basic entitlement total.
        /// </summary>
        /// <value>
        /// The high needs basic entitlement total.
        /// </value>
        public static IWebElement HighNeedsBasicEntitlementTotal
            => Driver.Instance.WaitToFindElement(By.XPath("//th[contains(text(),'Total basic entitlement')]/following-sibling::td"));

        /// <summary>
        /// Gets the high needs additional funding special total.
        /// </summary>
        /// <value>
        /// The high needs additional funding special total.
        /// </value>
        public static IWebElement HighNeedsAdditionalFundingSpecialTotal
            => Driver.Instance.WaitToFindElement(By.XPath("//th[contains(text(),'Total additional funding for special free schools')]/following-sibling::td"));

        /// <summary>
        /// Gets the high needs hospital education and teachers pay/pension total.
        /// </summary>
        /// <value>
        /// The high needs hospital education and teachers pay/pension total.
        /// </value>
        public static IWebElement HighNeedsHospitalEducationTeachersPayPensionTotal
            => Driver.Instance.WaitToFindElement(By.XPath("//th[contains(text(),\"Total hospital education and teachers' pay and pension\")]/following-sibling::td"));

        /// <summary>
        /// Gets the high needs import export total.
        /// </summary>
        /// <value>
        /// The high needs import export total.
        /// </value>
        public static IWebElement HighNeedsImportExportTotal
            => Driver.Instance.WaitToFindElement(By.XPath("//th[contains(text(),'Total import/export adjustment')]/following-sibling::td"));

        /// <summary>
        /// Gets the high needs additional funding total.
        /// </summary>
        /// <value>
        /// The high needs additional funding total.
        /// </value>
        public static IWebElement HighNeedsAdditionalFundingTotal
            => Driver.Instance.WaitToFindElement(By.XPath("//th[contains(text(),'Total additional high needs funding')]/following-sibling::td"));

        /// <summary>
        /// Gets the high needs total before deduction.
        /// </summary>
        /// <value>
        /// The high needs total before deduction.
        /// </value>
        public static IWebElement HighNeedsTotalBeforeDeduction
            => Driver.Instance.WaitToFindElement(By.Id("total-before-deductions"));

        /// <summary>
        /// Gets the high needs ap amount total.
        /// </summary>
        /// <value>
        /// The high needs ap amount total.
        /// </value>
        public static IWebElement HighNeedsAPAmountTotal
            => Driver.Instance.WaitToFindElement(By.XPath("(//th[contains(text(),'Pre-16 alternative provision places')]/following-sibling::td[3])"));

        /// <summary>
        /// Gets the high need maintained special amoun total.
        /// </summary>
        /// <value>
        /// The high need maintained special amoun total.
        /// </value>
        public static IWebElement HighNeedMaintainedSpecialAmounTotal
           => Driver.Instance.WaitToFindElement(By.XPath("//*[@id=\"high-needs\"]/table[9]/tbody/tr/td[3]"));

        /// <summary>
        /// Gets the high need deductions direct funding amount.
        /// </summary>
        /// <value>
        /// The high need deductions direct funding amount.
        /// </value>
        public static IWebElement HighNeedDeductionsDirectFundingAmount
            => Driver.Instance.WaitToFindElement(By.Id("direct-funding-total"));

        /// <summary>
        /// Gets the high need maintained mainstream.
        /// </summary>
        /// <value>
        /// The high need maintained mainstream.
        /// </value>
        public static IWebElement HighNeedMaintainedMainstream
           => Driver.Instance.WaitToFindElement(By.XPath("(//th[contains(text(),'Post-16 special educational needs places')]/following-sibling::td)[6]"));

        /// <summary>
        /// Gets the high need feilp amount.
        /// </summary>
        /// <value>
        /// The high need feilp amount.
        /// </value>
        public static IWebElement HighNeedFEILPAmount
           => Driver.Instance.WaitToFindElement(By.XPath("//th[contains(text(),'Further education (')]//parent::tr/parent::thead/following-sibling::tbody/descendant::td[3]"));


        /// <summary>
        /// Gets the primary school table.
        /// </summary>
        /// <value>
        /// The primary school table.
        /// </value>
        public static IWebElement PrimarySchoolTable => Driver.Instance.WaitToFindElement(By.Id("primary-school-table"));

        /// <summary>
        /// Gets the secondary school table.
        /// </summary>
        /// <value>
        /// The secondary school table.
        /// </value>
        public static IWebElement SecondarySchoolTable => Driver.Instance.WaitToFindElement(By.Id("secondary-school-table"));

        /// <summary>
        /// Gets the local authority protection table.
        /// </summary>
        /// <value>
        /// The local authority protection table.
        /// </value>
        public static IWebElement LocalAuthorityProtectionTable => Driver.Instance.WaitToFindElement(By.Id("local-authority-protection-table"));

        /// <summary>
        /// Gets the premises mobility table.
        /// </summary>
        /// <value>
        /// The premises mobility table.
        /// </value>
        public static IWebElement PremisesMobilityTable => Driver.Instance.WaitToFindElement(By.Id("premises-mobility-table"));

        /// <summary>
        /// Gets the historic commitments table.
        /// </summary>
        /// <value>
        /// The historic commitments table.
        /// </value>
        public static IWebElement HistoricCommitmentsTable => Driver.Instance.WaitToFindElement(By.Id("historic-commitments"));

        /// <summary>
        /// Gets the total schools block table.
        /// </summary>
        /// <value>
        /// The total schools block table.
        /// </value>
        public static IWebElement TotalSchoolsBlockTable => Driver.Instance.WaitToFindElement(By.Id("total-schools-block"));

        /// <summary>
        /// Gets the central school services table.
        /// </summary>
        /// <value>
        /// The central school services table.
        /// </value>
        public static IWebElement CentralSchoolServicesTable => Driver.Instance.WaitToFindElement(By.Id("central-school-services-table"));

        /// <summary>
        /// Gets the growth table.
        /// </summary>
        /// <value>
        /// The growth table.
        /// </value>
        public static IWebElement GrowthTable => Driver.Instance.WaitToFindElement(By.Id("growth-table"));

        /// <summary>
        /// Gets the totals table.
        /// </summary>
        /// <value>
        /// The totals table.
        /// </value>
        public static IWebElement TotalsTable => Driver.Instance.WaitToFindElement(By.Id("totals-table"));

        /// <summary>
        /// Gets the school block total.
        /// </summary>
        /// <value>
        /// The school block total.
        /// </value>
        public static IWebElement SchoolBlockTotal => Driver.Instance.WaitToFindElement(By.Id("school-block-total"));

        /// <summary>
        /// Gets the central school services block total.
        /// </summary>
        /// <value>
        /// The central school services block total.
        /// </value>
        public static IWebElement CentralSchoolServicesBlockTotal => Driver.Instance.WaitToFindElement(By.Id("css-block-total"));

        /// <summary>
        /// Gets the find out why partial pupil numbers are used list item.
        /// </summary>
        /// <value>
        /// The find out why partial pupil numbers are used list item.
        /// </value>
        protected static IWebElement FindOutWhyPartialPupilNumbersAreUsedListItem
            => Driver.Instance.WaitToFindElement(By.Id("why-pupil-nos"));

        /// <summary>
        /// Schools the tab how we calculate this link.
        /// </summary>
        /// <param name="elementIdName">Name of the element identifier.</param>
        /// <returns>The web element.</returns>
        protected static IWebElement SchoolTabHowWeCalculateThisLink(string elementIdName)
            => Driver.Instance.WaitToFindElement(By.Id(elementIdName));

        /// <summary>
        /// DSGs the technical notes links.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <returns>The web element list.</returns>
        protected static ReadOnlyCollection<IWebElement> DSGTechnicalNotesLinks(int yearFrom, int yearTo)
            => Driver.Instance.WaitToFindElements(By.PartialLinkText($"technical note {yearFrom} to {yearTo}"));

        /// <summary>
        /// Gets the post16 special educational needs places.
        /// </summary>
        /// <value>
        /// The post16 special educational needs places.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> Post16SpecialEducationalNeedsPlaces => Driver.Instance.WaitToFindElements(
            By.XPath("(//th[contains(text(),'Post-16 special educational needs places')]/following-sibling::td[3])"));

        /// <summary>
        /// Gets the guidance list items.
        /// </summary>
        /// <value>
        /// The guidance list items.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> GuidanceListItems => Driver.Instance.WaitToFindElements(By.ClassName("guidance-items"));

        /// <summary>
        /// Gets the print this page items.
        /// </summary>
        /// <value>
        /// The print this page items.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> PrintThisPageItems => Driver.Instance.WaitToFindElements(By.ClassName("print-page"));


        /// <summary>
        /// Gets the guidance link list item.
        /// </summary>
        /// <value>
        /// The guidance link list item.
        /// </value>
        protected static IWebElement GuidanceLinkListItem
            => Driver.Instance.WaitToFindElement(By.Id("guidance-link"));

        /// <summary>
        /// Gets the history link.
        /// </summary>
        /// <value>
        /// The history link.
        /// </value>
        protected static IWebElement HistoryLink =>
            Driver.Instance.WaitToFindElement(By.Id("allocation-history-link-dsg"));

        /// <summary>
        /// Gets the disability access fund.
        /// </summary>
        /// <value>
        /// The disability access fund.
        /// </value>
        protected static IWebElement DisabilityAccessFund =>
           Driver.Instance.WaitToFindElement(By.XPath("//caption[contains(text(),'Disability access fund')]"));

        /// <summary>
        /// Gets the high needs hospital.
        /// </summary>
        /// <value>
        /// The high needs hospital.
        /// </value>
        protected static IWebElement HighNeedsHospital
        {
            get
            {
                var elements = Driver.Instance.FindElements(By.XPath("//caption[contains(text(),'Hospital academies')]"));
                return elements.Count > 0 ? elements[0] : null;
            }
        }

        /// <summary>
        /// Gets the schools tab.
        /// </summary>
        /// <value>
        /// The schools tab.
        /// </value>
        protected static IWebElement SchoolsTab =>
            Driver.Instance.WaitToFindElement(By.Id("tab_schools"));

        /// <summary>
        /// Gets the central school services tab.
        /// </summary>
        /// <value>
        /// The central school services tab.
        /// </value>
        protected static IWebElement CentralSchoolServicesTab =>
            Driver.Instance.WaitToFindElement(By.Id("tab_css"));

        /// <summary>
        /// Gets the high needs tab.
        /// </summary>
        /// <value>
        /// The high needs tab.
        /// </value>
        protected static IWebElement HighNeedsTab =>
            Driver.Instance.WaitToFindElement(By.Id("tab_high-needs"));

        /// <summary>
        /// Gets the early years tab.
        /// </summary>
        /// <value>
        /// The early years tab.
        /// </value>
        protected static IWebElement EarlyYearsTab =>
            Driver.Instance.WaitToFindElement(By.Id("tab_early-years"));

        /// <summary>
        /// Gets the allocation amount.
        /// </summary>
        /// <value>
        /// The allocation amount.
        /// </value>
        protected static IWebElement AllocationAmount =>
            Driver.Instance.WaitToFindElement(By.Id("allocation-amount"));

        /// <summary>
        /// Gets the early years total amount.
        /// </summary>
        /// <value>
        /// The early years total amount.
        /// </value>
        protected static IWebElement EarlyYearsTotalAmount =>
            Driver.Instance.WaitToFindElement(By.CssSelector("#early-years .heading-large.heading-funding-summary"));

        /// <summary>
        /// Gets the high needs total amount.
        /// </summary>
        /// <value>
        /// The high needs total amount.
        /// </value>
        protected static IWebElement HighNeedsTotalAmount =>
            Driver.Instance.WaitToFindElement(By.CssSelector("#high-needs .heading-large.heading-funding-summary"));

        /// <summary>
        /// Gets the document download container.
        /// </summary>
        /// <value>
        /// The document download container.
        /// </value>
        protected static IWebElement DocumentDownloadContainer
            => Driver.Instance.WaitToFindElement(By.ClassName("download-a-document-container"));

        /// <summary>
        /// Gets the format request link.
        /// </summary>
        /// <value>
        /// The format request link.
        /// </value>
        protected static IWebElement FormatRequestLink
            => Driver.Instance.WaitToFindElement(By.LinkText("Request an accessible format"));

        /// <summary>
        /// Gets the format request email links.
        /// </summary>
        /// <value>
        /// The format request email links.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> FormatRequestEmailLinks
            => Driver.Instance.WaitToFindElements(By.LinkText("ESFA.GOVUK-ENQUIRIES@education.gov.uk"));

        /// <summary>
        /// Gets the open document format link.
        /// </summary>
        /// <value>
        /// The open document format link.
        /// </value>
        protected static IWebElement OpenDocumentFormatLink
            => Driver.Instance.WaitToFindElement(By.LinkText("OpenDocument"));

        /// <summary>
        /// Gets the amount of teacher pay/pension that is protected.
        /// </summary>
        protected static IWebElement FundingTeacherPayProtectedAmount
            => Driver.Instance.WaitToFindElement(By.XPath("//h2[contains(text(),\"Funding protected from being transferred to other blocks through teachers' pay and pension grants\")]/parent::div/following-sibling::div"));

        /// <summary>
        /// Gets the amount of total funding that is transferable.
        /// </summary>
        protected static IWebElement TotalFundingTransferableAmount
            => Driver.Instance.WaitToFindElement(By.XPath("//h2[contains(text(),\"Total schools block of which a percentage can be transferred to other blocks\")]/parent::div/following-sibling::div"));

        #endregion
    }
}