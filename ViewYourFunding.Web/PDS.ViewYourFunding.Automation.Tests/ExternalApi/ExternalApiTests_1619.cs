using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PDS.ViewYourFunding.Automation.Tests.Config;
using PDS.ViewYourFunding.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass, TestCategory("Regression"), TestCategory("CoreRegression")]
    public class ExternalApiTests_1619
    {
        private static HttpClient _httpClient = null;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly string _baseUrl;
        private readonly string _secretKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalApiTests_1619"/> class.
        /// </summary>
        public ExternalApiTests_1619()
        {
            _applicationConfiguration = ConfigHelper.GetApplicationConfiguration();
            var config = ConfigHelper.GetIConfigurationRoot();

            if (_httpClient == null)
            {
                _httpClient = new HttpClient();

                _secretKey = config["FundingApiSecretKey"];
                _httpClient.DefaultRequestHeaders.Add("x-secret-key", _secretKey);
            }

            _baseUrl = config["baseSiteUrl"];
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_FE_With()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10007063-2_0",
                "46790b3f-699f-4e74-96ed-f0ba6277881f",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "1619 Provider Pdf Layout - sixth form",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_FE_with);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_FE_Without()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10000552-2_0",
                "46790b3f-699f-4e74-96ed-f0ba6277881f",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "1619 Provider Pdf Layout - sixth form",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_FE_without);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_Academies_With()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10034690-2_0",
                "46790b3f-699f-4e74-96ed-f0ba6277881f",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "1619 Provider Pdf Layout - sixth form",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_Academies_with);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_Academies_Without()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10030654-2_0",
                "46790b3f-699f-4e74-96ed-f0ba6277881f",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "1619 Provider Pdf Layout - sixth form",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_Academies_without);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_TuitionFunding_With_MathsTop()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10047244-2_0",
                "46790b3f-699f-4e74-96ed-f0ba6277881f",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "1619 Provider Pdf Layout - sixth form",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_TuitionFunding_With_MathsTop);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_TuitionFunding_Without_MathsTop()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10064744-2_0",
                "46790b3f-699f-4e74-96ed-f0ba6277881f",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "1619 Provider Pdf Layout - sixth form",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_TuitionFunding_Without_MathsTop);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_AcademyConverter_InYear_Opener()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10088096-2_0",
                "46790b3f-699f-4e74-96ed-f0ba6277881f",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "1619 Provider Pdf Layout - sixth form",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_AcademyConverterInYearOpener);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_SpecialPost16_Without()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10001929-2_0",
                "46790b3f-699f-4e74-96ed-f0ba6277881f",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "1619 Provider Pdf Layout - sixth form",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_SpecialPost16);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_SpecialAcademies_Without()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10004756-1_0",
                "46790b3f-699f-4e74-96ed-f0ba6277881f",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "1619 Provider Pdf Layout - sixth form",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_SpecialAcademies);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_SixthForm_Without()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10000866-2_0",
                "46790b3f-699f-4e74-96ed-f0ba6277881f",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "1619 Provider Pdf Layout - sixth form",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_SixthForm_without);
        }

        // Note that this test is just for checking if the free meals line shows right - the rest of the data isn't intended
        // to match last years Haringey pdf.
        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_SixthForm_WithFreeMealsLine()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10040630-1_0",
                "46790b3f-699f-4e74-96ed-f0ba6277881f",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "1619 Provider Pdf Layout - sixth form",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_SixthForm_withFreeMealsLine);
        }

        // Note that this test is just for checking if the 'high value courses for school leavers', SUP and POG shows right - the
        // rest of the data isn't intended to match last years Haringey pdf.
        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_SixthForm_WithHighValueCoursesForSchoolAndCollegeLeavers_And_SUP_AND_POG()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-FY-2021-10040631-1_0",
                "46790b3f-699f-4e74-96ed-f0ba6277881f",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "1619 Provider Pdf Layout - sixth form",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_SixthForm_withHighValueCoursesForSchoolAndCollegeLeavers_And_SUP_AND_POG);
        }


        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_SixthForm_With()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10006247-2_0",
                "46790b3f-699f-4e74-96ed-f0ba6277881f",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "1619 Provider Pdf Layout - sixth form",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_SixthForm_with);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_NMSS()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10030654-2_0",
                "13f5d97c-d85f-44e9-87f6-acdd3498141f",
                "NMSS_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "NMSS provider Pdf Layout",
                "NMSS",
                5,
                "Provider",
                "AY-2122");

            // Assert
            actualHtml.Should().Be(html_1619_NMSS);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_LA_StudentNumber()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                null,
                "963d6669-aa3a-4694-8adb-5661c67cbe01",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInLA_StudentNumber.json",
                "1619 Local Authority Student Numbers Pdf Layout",
                "1619",
                4,
                "LocalAuthority",
                "AS-2122",
                "1619-AS-2122-Information-LocalAuthority-341-1_0");

            // Assert
            actualHtml.Should().Be(html_1619_LA_StudentNumber);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_LA_SixthForm()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                null,
                "526a04e5-b01a-4a07-8947-7d9b28f2303e",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInLA_SixthForm.json",
                "1619 Local Authority Sixth Form Pdf Layout",
                "1619",
                4,
                "LocalAuthority",
                "AS-2122",
                "1619-AS-2122-Contracting-LocalAuthoritySsf-10001464-1_0");

            // Assert
            actualHtml.Should().Be(html_1619_LA_SixthForm);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_LA_SixthForm_MSS()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                null,
                "fed3e6b0-49d3-4f22-9c35-35a61a8fc997",
                "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInLA_SixthForm_MSS_Only.json",
                "1619 Local Authority Sixth Form MSS Only Pdf Layout",
                "1619",
                4,
                "LocalAuthority",
                "AS-2122",
                "1619-AS-2122-Contracting-LocalAuthority-10001464-1_0");

            // Assert
            actualHtml.Should().Be(html_1619_SixthForm_Mss);
        }


        #region indicative statements

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_Provider_Indicative()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10007063-2_0",
                "da7f1a86-d288-43e7-8bc4-82113830bf0b",
                "1619_Indicative_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "16-19 Logged In Provider Indicative Statement",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_IndicativeStatement);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_Provider_Indicative_WithoutHighValueCourses_2223()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10088092-2_0",
                "da7f1a86-d288-43e7-8bc4-82113830bf0b",
                "1619_Indicative_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "16-19 Logged In Provider Indicative Statement",
                "1619",
                4,
                "Provider",
                "AS-2223");

            // Assert
            actualHtml.Should().Be(html_Provider_Indicative_WithoutHighValueCourses_2223);
        }

        [TestMethod]
        public async Task ExternalApi_RenderHtml_1619_Provider_Indicative_WithHighValueCourses()
        {
            // Arrange Act
            var actualHtml = await GetHtml(
                "1619-AS-2122-10088092-2_0",
                "da7f1a86-d288-43e7-8bc4-82113830bf0b",
                "1619_Indicative_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json",
                "16-19 Logged In Provider Indicative Statement",
                "1619",
                4,
                "Provider");

            // Assert
            actualHtml.Should().Be(html_1619_Provider_Indicative_WithHighValueCourses);
        }


        #endregion

        private async Task<Dictionary<string, object>> GetItemFromCosmos(Container container, string id)
        {
            try
            {
                var item = await container.ReadItemAsync<Dictionary<string, object>>(id, new PartitionKey(id));
                return item;
            }
            catch
            {
                return null;
            }
        }

        private readonly string html_1619_SpecialAcademies =
         @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>16 to 19 academies revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your revenue funding allocation statement.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>Abbey Hill Academy</td></tr>
<tr>
<td>UKPRN</td><td>10004756</td></tr>
<tr>
<td>Local authority</td><td>Stockton-on-Tees</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021/22 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 50%;"">High needs student</td><td style=""width: 50%;"" class=""right"">&pound;960,000</td></tr>
<tr>
<td style=""width: 50%;"">Student financial support</td><td style=""width: 50%;"" class=""right"">&pound;7,724</td></tr>
<tr>
<td style=""width: 50%;"">Start-up and post-opening grant</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr>
<td style=""width: 50%;"">Maths top up</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total funding allocation</td><td class=""right"">&pound;967,724</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">High needs funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">16-19 students</td><td style=""width: 15%"">19-24 students</td><td style=""width: 15%"">Total students</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>High needs place funding</td><td>96</td><td>0</td><td>96</td><td>&pound;10,000</td><td style=""font-weight: bold"">&pound;960,000</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">Student financial support funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">Number of funded students</td><td style=""width: 15%"">Instances per student</td><td style=""width: 15%"">Number of instances</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 1: Financial disadvantage</td><td>96</td><td>0.05615</td><td>5.39</td><td>&pound;242</td><td>&pound;1,304</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 2a: Student costs - travel</td><td>96</td><td>0.03372</td><td>3.24</td><td>&pound;483</td><td>&pound;1,564</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund</td><td>&pound;2,868</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">2019 to 2020 discretionary bursary fund - baseline for transition</td><td>&pound;10,299</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition lower limit</td><td>&pound;7,724</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition upper limit</td><td>&pound;12,874</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Exceptional adjustment</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Student financial support funding total</td><td>&pound;7,724</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Start-up and post-opening grant</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part A</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part B</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - per pupil resources</td><td>-&pound;997</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - leadership disecononomies</td><td>-&pound;997</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td style=""font-weight: bold"">Total start-up and post-opening grant</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Maths top up</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Maths top up funding</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style='font-size: 10px; margin-top: 5px'>The values on your statement are shown rounded to various numbers of decimal places.<br> The calculation of your funding however is done using un-rounded values. This may result in some slight differences when you work through the calculation yourselves.</div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_AcademyConverterInYearOpener =
           @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>16 to 19 revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your revenue funding allocation statement.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>St Mary's Catholic School</td></tr>
<tr>
<td>UKPRN</td><td>10064744</td></tr>
<tr>
<td>Local authority</td><td>Hertfordshire</td></tr>
<tr>
<td>Open date</td><td>01 November 2021</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021/22 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 50%;"">Programme funding</td><td style=""width: 50%;"" class=""right"">&pound;803,165</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;62,400</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;19,110</td></tr>
<tr>
<td>Care standards</td><td class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">High needs student</td><td style=""width: 50%;"" class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">Student financial support</td><td style=""width: 50%;"" class=""right"">&pound;24,782</td></tr>
<tr>
<td style=""width: 50%;"">Teachers' pension scheme grant</td><td style=""width: 50%;"" class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">16 to 19 Tuition funding</td><td style=""width: 50%;"" class=""right"">&pound;2,000</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total funding allocation</td><td class=""right"">&pound;909,457</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Start-up and post-opening grant</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 50%;""></td><td style=""width: 50%;"" class=""right"">Funding</td></tr>
<tr>
<td>Start-up grant - part A</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Start-up grant - part B</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Post opening grant - per pupil resources</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Post opening grant - leadership diseconomies</td><td class=""right"">&pound;0</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total start-up and post-opening grant</td><td style=""font-weight: bold;"" class=""right"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_SpecialPost16 =
    @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD//gA7Q1JFQVRPUjogZ2QtanBlZyB2MS4wICh1c2luZyBJSkcgSlBFRyB2NjIpLCBxdWFsaXR5ID0gODIK/9sAQwAGBAQFBAQGBQUFBgYGBwkOCQkICAkSDQ0KDhUSFhYVEhQUFxohHBcYHxkUFB0nHR8iIyUlJRYcKSwoJCshJCUk/9sAQwEGBgYJCAkRCQkRJBgUGCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQk/8AAEQgAkQEsAwEiAAIRAQMRAf/EAB8AAAEFAQEBAQEBAAAAAAAAAAABAgMEBQYHCAkKC//EALUQAAIBAwMCBAMFBQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYnKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX29/j5+v/EAB8BAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKC//EALURAAIBAgQEAwQHBQQEAAECdwABAgMRBAUhMQYSQVEHYXETIjKBCBRCkaGxwQkjM1LwFWJy0QoWJDThJfEXGBkaJicoKSo1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoKDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uLj5OXm5+jp6vLz9PX29/j5+v/aAAwDAQACEQMRAD8A9B/Zcdm8L68WYn/ibP1P/TNK9orxX9lv/kV9e/7Cz/8AotK9qrfE/wARmGG/hRAnA9a8Wu/2gLXSPEj2mqXUEFn9pjR90LL9jXGHWRzglxjdgL/EBzg49nflSMke461+f91f/aPHGsW3iS7MOoYvTbarfXLq8cm1lCS4U7sFGQBQnzEnkYFYG591Q+K9LvNBttcsJzfWd2F+ymAZa4LHChQcck+uAOSSACa4D4s6hd3Y8PCXRdWgMOoxzDZImGYdFBWUZOf4Rlj/AAg848o/Zc8RavrngfXvD9tdRJc6Iwv9OeRVwjENlWJP3TyOnGTz0FLqXjrx9qUqXs+ialHbz6rDqtst79ntysYQAbFllJ7DAGAeTkE4oA+lYvELfabeK80u9sI7k7IpZzGVL84Q7WJUnHGeM8ZyQDR8Vave+FWXXQkt3pEa7b+BF3SW6Z/16AcsF/jX+7yOVIbzn4Uav4v8UavLpniO2v7G2tbiTVW+2QqDcb5cwrG+9gUUhiduAvyryOa0/wBoOTxavh+0Tw9rVnpVhJKI9RllwGWMsBksSMRgbt20ZPA6ZoA9I0LxHpHiazN7o2o22oWyuYzLbuHUMOoyPqPzFaNfnl4R8UXHwu+JNjJ4c1mS8SK6EF23kNHDOpfay+W2Djb6gEHpjGT+hgPHagBaKPyooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKPyoA8V/Zb/5FfXv+ws//otK9okkWGN5JGCogLMx4AA714v+y3/yK+vf9hZ//RaV7SwDKQwyCMEHvW+J/iMwwv8ACieBal4h+IXiLxhqV9Dd3uneHZozbeHbVB5Mt5d4ASTbgM0Y+Z2LjZt7HrXg37UFxov/AAtK9stGRAbcBr10OQ1y/wA0gHsCeR/eLfQfRHjD4l2vhnUPF8wt7xvE8EciWZmtXEVnYpGpMqsw27S+49cu+1egBHyV4ZtdU/eeNEsrTWTBqUNq9nfwfaReSzLI2Cp++fkOcc5YEVgbmx8O/E2r/DjQ9R8RaDqKte6mq6VFbxIHKO2WLSKQcEBRs/vFup2stdFpvwm0q6e8h8Tave3WtwwCWcW93EEhIKgxNkO5Kg4J2gAjABxmu58VeFNI8G/E/wAMavd2mleFtM1m3tpH04rI5W6WSN8FR8mEk2Z5X5Q3TIq5YkW0t8qyX8NyttNLKt4wIGydd5A37Qd0mAx6cggc0AcN4Xn1H4Z67Da2OtahfeDde8y2llt3aC5j8olmWNhysgOdu3AfJGA2QuX8S/iL4wsriyiuhdi4t5P3Gq3qoZby3ADRK8YynAbcT824kEngVt/E/WBJ4AbU7cxWk7azbyw+U0bM84jkdnBQ8FNwBJHJYd812/xA0bwf8N/hhp0/inQLrxHProt3hsGcQ/YZ/Ky4jlVd6oC2AnzY4HSgDlvhHolr43tj4/uNHsbaTw/dRIAkhCXNyTGI2fqyxrkOxJJOX5PG36C8A/EXWtX8b+IPBviPTIbS70vD21zCjRpexZ++EYtgcrj5jnJ9CK+Q/hFql7oXjibTyt/a6TPMDfaM7sZrmEE5Tbsw7IjMxBC7lDDqQK+zPBVxpt9qV3p1vfWut2ulLBcafel1mkgSZXHlmTkkqF+91KuucnJIB29FFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQB4r+y3/yK+vf9hZ//RaV7VXiv7Lf/Ir69/2Fn/8ARaV7VW+J/iMwwv8ACieEfGHV7/xt8Q7L4PxXEekaXrFqJrzUDBulmKbpPKjJIHIRc8d/wOhr37OOkQ+D9J03wXePpGraJcNfWd853NNcFQMysB3Kp0HAXGK3vjJ8KtN8f2VrqUsOqNqOlbpLf+y50huH4+6rv8o557ZIHNfPlncfEvxPat8Ovh54Y1jwvoiSsLu4vncTEk4YyzEAKMfwIMnH8WTWBudR450rX/i6bjwf4q8QeBE13TF36V/Z12TcTzjIdGQk4DhRlcAg7SM4IrDvvEt94SvLhfE/h3U453C6db6jeBoJZrYbGeSTAVCWMYw4Yv2IPBrm/iX8Dbn4Tah4QSw1iMX99Kzy6xdTCC3t50KlQM/dA65OSccAdK9/8e3cPiSHQJl1Wy1dIwkfmaVdKRHdMMNKSG4TphiSq87lbIwAeJ6Z4X1DxrcJ438TSQR+CNCZpbK1muXjF45fIj8y52ltzY3uSeBhR0A9tsdN1z446PY3Oraz4ZtNHguIbxIdEIu7mOZMMFaZiUQg8HapyO+DivNf2rr+61vSPDVra+IdC1CO2I+16ba3amaW5ICh1QHJX7wGMEbj+FfVvgN8QvhHJbeLvhrf3cz+Sj3emBg80Zxlkx92dAcjpu9AetAHVfFXTG8B+Nbrx9bXEVlc2jNPb6fPC09tqHmosU8nyn92+wDJ43bcY6lvTvhwtpaSS2/h7QYtO8Pyb3ikELIzEFQDvLHcCTIAoA2hB2Irzzwlr/iD4/HTLLxV4U1XSLHSp/OvwFCWd84HCMsg38EA7Rux1JBwR9AAADAGBQAtFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQB4r+y3/AMivr3/YWf8A9FpXtVeK/st/8ivr3/YWf/0Wle1Vvif4jMML/CiFFJuXPUUbl/vD86wNzlPiT8NNC+Kfh8aLrqzCNJBNDNA+2SGQAjcCQR0JGCCK8Nuf2HtMaQm28a3kceeFksVc/mHH8q+nQwPQiloA8G8DfsheFPCet2msX+rX+sT2cqzRROixRb1OVLKMk4IBxnHrmveaje4hidY3ljR3+6rMAW+gqSgAooooAKKKKACiiigAoopks8UCb5ZEjXOMu2B+tAD6KQMGUMpBB5BHeloAKKKKACiiigAooqKS6gidUkniR26KzAE0AS0UZzRQAUUVSvtc0rS2Vb/UrK0ZugnnVCfzNAF2iorW8tr2FZrW4iuIm6PE4ZT+IpZLiGJlSSWNGc4UMwBb6etAElFFMlljhQvI6og6sxwBQA+ionu7eONZXniWNvuuXAB+hpZLmGJkWSaNC/ChmA3fT1oAkooooAKKKKAPFf2W/wDkV9e/7Cz/APotK9qrxX9lv/kV9e/7Cz/+i0r2qt8T/EZhhf4UT5K8Y+C7L4jftYar4a1W8v7eyltY5SbSUI4K2sZGCQR19q7/AP4Y88C/9BvxX/4GR/8AxqvPvGNz4ttP2tNVl8EWNje62LWMRw3rbYin2SPcSdy84969B/4SD9pn/oUvB/8A3+/+31gbnefDD4QaH8J01FNFvdVuhqBjMv26ZZNuzdjbtVcfeOfwqfWvjJ8PvDuoNp2p+LdKt7tDteLzd5jPo23O0+xxXmnxL+IPxF8KfA/VL/xZa2GkeIry9XT7Y6c+VSJ1BLg7mw2FkHXjg1pfCT9njwTp/gfTbnXtDtNX1XULZLm5nu1L7WdQ2xQeABnGRyetAHNfGTVtP1z40/CK/wBLvba+tJrsFJ7eQOjjzo+hHFfQL+INHj1ZNGfVbBdTkTelkZ0E7LgncEzuIwDzjsa+U/Gfwxsfhp+0L4Di0Uyx6NqOoxXEFqzllt5BKokVc9j8h9e3YV22uf8AJ5Wgf9ghv/RU9AH0DdXVvY20t1dTxQW8KGSSWVgqRqBksSeAAO9Q6Zq2n61Zpe6XfWt/avkJPbSrJG2Dg4ZSQcEEVz/xX/5Jf4t/7A93/wCiWr50tfHF94I/Y/0p9Mne3vdTvJ7BJkOGjVppWcg9jtQjPbNAH0FrXxm+Hnh6/fT9T8XaVBdRna8Ql3lD6Ntzg+xrpNE1/SfEtguoaLqVpqNo5wJraUSLn0yO/tXlfw0/Zz8DaL4PsF1vQbTVtVuYElu7i7XefMYAlVB4UDOBjnjJqT4e/BG/+GPxI1TVvD2pwReE9QiIbSnd2eN8AggkYOGyASc7WxzQB3fij4k+D/BUqw+IfEWnadMw3CGWUeYR67Blse+KPC/xK8HeNZXh8PeItP1GZBuaGKX94B67Dg498V45ovwn8FeEdZ1nX/jFr3h3VdZ1G4M0X2y5wkcZ7CNyMnt0IAAxivP/ABHqPw7tfjv4FuPhfJDDuvoor82SssB3SKuFzxyrMDt4xjvmgD0z43/Fj+x/HPgKy8PeL7SG1fVGh1mO2u42CIJYQRNydgAMnXHf0rrvi1Y+Dfih8PXtbvxppmn6R9sjLalDcxSRLIuSE3btuTnpnNeRftA/DDwho/xG8Aix0dYR4j1iQap+/lP2ndNDnOWO3PmP93HX6V0P7SHgnw/4B+BM+leG9OGn2TarDOYhK8mXIIJy5J6KO/agD3jRY7TSvDtjFHdxy2lraRqtyWAV41QAPnpggZrlpPjr8M4r02TeNNH84Nt4mymf98fL+teNfGnV9V8QWXw1+F+l3b2kevWtq946n7yEKig+qjDsR3wK9Ttf2cPhjb6Aujt4Ytph5exruRm+0scfe8zOQe/GB7YoA9Htry2vbWO7tZ4p7eRQ6SxMGR19QRwRXPyfEzwVFo8utN4p0f8As6KUwPci6QoJAAdmQeWwQcDmvE/gFc6j8Pvih4u+E9zeS3Wm2sbXliZDkoPlPHpuSRSR0yvvXK/sv/CnRfHkOsav4mhOpWGnXzRWlhK58lZWUGSRlB5JAjHPHHOeMAH0p4X+J/gvxpcta+H/ABHp+oXKjcYI5MSY7kKcEj3ArqK+W/jv4G0H4YeNfAXiXwhYR6PdTamIZY7XKxuAyY+XoMhmBx1Br6koAp61cz2Oj311axedcQW8kkcf99gpIH4kV8nfCP4SaH8b/DGs+MvGPiXVJtaa6ljeRJ1UWgChgzBgeOc44AAwOlfTHxB8faN8N/DVxr+tyMIIyEjijGZJ5DnCKPU4P0AJ7V8b+JvDvi+ysr/x3/YupeGfA/iS7Q3unWFx+8+zswIZlI4ViW25AGWxgAjIB75+yX4k1rXvAuo2uq3k2oQaZfta2d3KSxePaDtyeSBnj0DAdq9xryrw948+HXw80fwV4d8PLM9j4iPl6abZA+9iyhnlJIIO5+e4IIxxivVaAPNP2hvH+ofDr4aXmqaS3l6hcSpZwTYz5LPkl8eoVWx74rg/h9+zF4V8R+GNP8Q+MrvU9d1fVreO8mme7ZVXzFDAAjk4BGSSc+1ewfEfwDpvxL8JXfhzU3eKOfDxzxjLQyKcq4Hf3HcEivCdO8EftC/CS3Fh4Y1HT/E2jQf6m2kKkovoFk2sv+6rkelAFLxd4E1f9m7xfoviLwJc6te6BfXHk32mNulAAwSDtHIK7tpIypHU5rpP2jG3fFX4PMO+rZ/8j21M0T9qTVdA1m30b4o+Dbrw9LMdovIkcRjnG4o3JX1Ks30qD9pnVbKy+Ifwl1a4uY0sYdQa5kuM5RYhNbMXyO2OaAPoi/v7TS7Oa+v7mG1tYEMks0zhEjUdSSeAK4H4gz+Dfin8M9Ysv+Ev0620aV4o7jVIZkeKBllRwCxO3JIUdf4hXjcnjF/2pviGfCUWqto3g+wU3TWwO241IKQM+ncHB+6OcE9PRv2hNB0zwz+zvrmk6PZxWVjbJapFDEMBR9pi/M9yTyaAOF/aZ02y0b9n/wAHabpt+mo2Vpd2kMF2hBFxGttKFcYJGCADxWp+0N/yUP4N/wDYTH/o22rl/jj/AMmt/Dr62H/pJJXUftD/APJRPg3/ANhMf+jbagD3TxF4r0HwjZC91/V7LTLcnCvcyhN59FB5Y+wrF8P/ABf8A+Kb5LDR/Fel3V25wkHm7Hc+ihsFj9K8C+O4sdM+Pmk6r8Q9Ovb7wYbVUgCBjEG2nIIBGcPyy9SMdRxWz4g8AfBn4s6fbRfD3XNB0HXo5UeCS3JikYZ5UwkqSe4IGQR1oA+lKKqaRBd2ulWcF9cLc3cUCJPOq7RLIFAZgO2Tk496t0AeK/st/wDIr69/2Fn/APRaV7VXiv7Lf/Ir69/2Fn/9FpXtVb4n+IzDC/wony3e6/pXhr9snUtR1rUbXTrNLNVae5kCICbSMAZPrXuH/C6Phv8A9Dx4e/8AA6P/ABp3iH4O+AvFmrTaxrnhmyvr+faJJ5C25tqhR0PYAD8Kzf8Ahnr4Wf8AQl6d+b//ABVYG5ynx3bSPjF8KNWg8HarZa5eaPLFftFYzLK2BuBGF7lS5A77avfCD48+Dde8D6ZHquvadpOp2NslvdW97OsJ3IoXcu4gMDjPHTOK7/wl8PPC3gT7V/wjWjW+mC72+eIS37zbnbnJPTcfzrB174B/DTxJqUmpaj4Ts3upTud4XkhDnuSI2AJ98UAeEePPibpfxF/aJ8Bx6FKbnS9K1CGBLoAhJpWlUuVz1UAIM9/piuj+Jes2fgX9qjwx4j12Q2ukzaaYftTA7EJEqc/QsufQHNez2/wm8D2culS23hqwgk0d/MsWjQr5D5BLDB5OQDk5PFaHi3wP4c8dWC2HiTSLbUoEbcglHzRn1VhhlP0IoA80+Nnxp8HwfD3WNL0jWrHWtU1WzltILawmE5AdSGdtmdoVSTz6V5PL4QvfFf7HmjS6fC88+k309+0aDLNGJplfA9g+76Ka+hvD/wAD/h34XFz/AGV4Xs4WuoXt5XdnlcxupVlDOxKggkHGK6fw94a0nwppMWj6JYxWOnwljHbx52ruJY9c9SSaAPPvhv8AHvwR4i8G2F3qHiLTNMv4bdEu7a8uFhdJFUBiAxG5TjIIz19eK5zw98ZvEvxD8e+JovCCQ3HhTR9OleKc2533FyIzsCsfV8kDHIX3rtNY/Z8+GGuX73974RsvPkO5jA8kKsfXajBf0rr/AA94Y0XwnpqaZoWmWunWaHIigQKCfU+p9zzQB8s/s7/8Kv1rTtV1j4h3uk3fiiS8dpTrsy/6sgEMokO1iTuyeSPYVX+KHjzwNqXxc8Aw+FRp9to2i6jG1ze20Sw2pZpYy2CAAQqqCW6c19A658Afhp4i1OXU9R8KWj3crb5HikkhDseSSqMASfXHNXNU+C/w/wBZ0O00O78L2H9nWbmWCGENFsYjBO5CGJOBnJ5wM9KAPJ/2m9XsLbxb8Jtbe6iOmRai1010h3R+V5ls28EdRtGeOoq5+0/4m0XxX8D5b/QtTtNStBqcEZmtpA6hhklcjvgj869g1f4f+Ftf0Gz0DVdEs7zTLJEjtoJl3CEKu1dp6jAGM5qkvwl8Dp4bfwyvh20GjPcfams8tsMuAN3XOcAd6APCfjRYaj4X/wCFX/E+ztJLq10a1tIbxUH3VAVlz6Bsuuexx617LbfHn4bXOgjWv+Et0yKDZvMMkoWdTj7vlffz7AV2n9k2J0waW1pDJYiIQfZ5FDIYwMBSD1GOOa4CT9nH4VS3pvG8H2nmFt21ZpRHn/cD7ce2MUAeafAgXnxF+L/jH4pm1lt9JliayszIMGT7gH4hIxn0LCtL9jP/AJEvxH/2GX/9FpXvOn6XZaTYRWGn2kFpaQrsjggQIiD0AHArO8LeC/D/AIKtZ7Tw9pcOnQXEpmlSLOHfAG45J5wBQB4r+1r/AMfPw+/7DH9Y6+haxPEngrw/4vaybXdLgvzYS+dbebn90/HzDB9h+VbdAHzx+2Ppt6/hzw3rKW73OnabqBa8iA4wwG0t6D5Suf8AaHrXfD4yfCvxN4RknvfEejf2bc25S4srqVVlCkco0R+YntgA+1eh3llbajay2l5bxXNvMpSSKVAyOp6gg8EV52f2cPhU179rPg+08zdu2iaUR5/3N+3HtjFAHyr4A1fQPCPxV0vxLcW2sv4Eg1G4h0u6uVOyFiOGPY7dysQOeAeoxX3hbzw3UEc8EqSxSKHSRGyrKRkEEdRisbVfAnhnWvD6eHb7Q7GXSI9pjsxEFjj29NoXG3Ht6n1q9oeh6f4b0uDStKtha2NuCsUIYsEGc4GSTjnpQB5b+03N4x0zwRba74P1G+tJNMuRLeraOVLwEYJOOoU4z7EntXQeAvjf4K8c6JbXkWu6fZXhjBuLK7nWKWF8cjDEbhnuODXfsiyKUdQysMEEZBFeZ67+zb8L/EN495c+GYreaQ7nNnNJApP+6jBR+AoA87/aq8d+E/EfhK18KaPd2mua/cX0TQR2LCdocZB5XOCc7dvU59qwvjL4VKXXwL8K68nm8RafeoHPzfNao65H4jIr3jwb8FvAXgG5F5oPh63gvAMC5lZppV/3Wcnb+GK3Nd8FeH/EupaZqWr6XBeXmkyedZSyZzA+VbIwfVVPPpQB4h+0F8M5vCX9lfEvwFax2F94dCLcQW0eFa3XhW2jqFHysO6n2q78WfHunfEn9l7V/EOnMAJltVnhzkwSi5i3IfoenqCD3r3m4t4rqCS3njWWKVSjo4yrKRggjuK5Oz+EPgaw0O/0G18O2sWl6iyPdWgZ/LlZCCpI3dQQOnoKAPn344/8mt/Dr62H/pJJXUftD/8AJRPg3/2Ex/6Ntq9m1b4c+FNd8P2Ph3UtFtrrSbDZ9mtHLbItilVxg54UkVPrfgjw94jvdLvtW0uC7udJk82ykfOYGypyuD6ovX0oA858WfGnTdA+J03gTxzo9la+H7m3WW21G6/eRTEgcOpXAXdvXPYgZ615x8fNG+B6+DrvU/Dt1odv4hUqbNNFuFPmNuGQ0aEqBjJzgY9ex+jfFfgbw344s1s/EejWmpxISU85PmjJ67WHzL+BrmdG/Z9+GOgXyX1l4Rs/PjO5DO8k4U+oWRmH6UAX/g1caxdfC7w3PrzTNqL2SmRps72GTsLZ5yU25zzXZ0AYGBwKWgDxX9lv/kV9e/7Cz/8AotK9qrxX9lv/AJFfXv8AsLP/AOi0r2qt8T/EZhhf4UThPE/xx+HvgzWp9E17xHHZahbhTJA1tM5UMoYcqhHIIPWsr/hpn4S/9DfD/wCAdx/8bry+bR9N139szUrLVdPtNQtGslZoLqFZYyRaIQdrAivef+FXeA/+hK8M/wDgsg/+JrA3LPg7x14d+IGnSal4a1JdQtIpTA8qxumHABIw4B6MPzrerz/xV4z8G/BODSrT+xPsUGsXZhii0q0iRPN+UbnAKjoRzyeK7+gBaK4fwn8XdC8Zt4mXTbbUEbw1I0V2J41Xew3/AHMMc/6tuuO1cR/w1h4VudGtLrStF1vUtTu3dYtKt4lacKpxvfaSAD26njpQB7fRXkXw9/aP0Pxr4mHhfUNG1Pw7rMmfJt79QBKQM7c8ENgE4IGfWum+J3xf8M/CnT4rjXJpZLm4z9msrdQ002OpAJACj1JoA7Y8CuQ8PfFjwp4p0LWNd0q+lmsNF8z7bI0DoY9ib2wCMthR2rzvTP2q9JN1br4k8IeIvDljcsFiv7qEmHJ6EnA4+ma5b9mW7063+GPxEu9TgN3pkdzcS3MScmaEQZdRyOq5HUdaAPoLwd4y0bx5oMOu6DcPc6fOzokjRtGSVYqeGAPUGtuvIvDPxO8E+E/goPGfh/Qb6x8OW8rKliir5oYzbCQC5HLHP3qztU/am0RWhi8N+Gdd8Szm3jnuFsYcrbb1DbGYZ+YZwcDAORmgD26ivO/hR8bfD/xYS7gsIbrT9TsgDcWN2AHVc43KR1GeD0IPUcioPih8efDXwyvYdJmhu9W1qcBk06xUM4B6Fiemew5PtQB6XRXhtp+1doEEd2niPw1r3h68hgaeG3u4gDchRnahbb83pkAH1zxXq/gvxVa+N/C+neIrGGaG2v4vNjjmADqMkc4JHb1oA26yPFnivSPBOg3Ou65c/ZdPttvmSbSxGWCgADJJyR0rXr56/aRvJfGvjHwb8KrGRv8AiY3S3l/sP3IgSBn6KJWx7CgD2PwP4+8PfEXR31fw3em7tElaBmaNo2VwASCrAHoQfxroq+bvhIU+E/x+8T/Dxv3OlayPtumofuqQC6qv/AC6/WMV9B61rWn+HNKutW1W6itLG1QyTTSHARR/P6dSaAL1FeBt+1tplzNNPpPgfxPqekwMRJqEUI2gDqccgevJH4V13gz9oHwp498W23hvQ472aW4szeC4ZFVEA6owzuDDpjGPfHNAHp1Fea/Ez48eG/htfxaO8F5rGuTAMmnaegeQA9Nx7Z7Dk+2Kw/CP7Tug634ig8PeIND1bwrf3JCwf2im1HY8AEnBUk9MjHvQB7NRXD/EP4t6J8NtV0Cw1mG4xrczQx3CbRHb7SgLSFiMKN4PGeAa891L9rPS4Gmu9L8F+JNT0WFiraokOyIgHlhkEY+pB+lAHvVFc54D8e6J8RvDcOv6FMz2shKOkg2vC46o47EZH4EHvXm3iP8Aam8P2WuTaN4Y0HWPFlxbkrLJp0eYwRwdp5LD3xj0JoA9soryv4e/tDeHPHuoXGjHT9S0nXoY2caZeRhZJtoyVjOQC2OxwfwzXQ/DP4raB8VdPvbzQ472D7DP5E8F5GqSKcZBwGPB5HXqpoA7OiuQ+JXxP0L4WaRbanriXcqXNwLaGG0jDyO5BPAJHAA9e4rqrWc3NtFOYpITIgfy5MBkyM4OMjIoAlooooAKKKKAPFf2W/8AkV9e/wCws/8A6LSvaq8e/Zp02+0zw3rkd/ZXNo76o7qs8TRll8tOQCORXsNbYj+IzDDfwkfI/jHwpe+M/wBrTVdH0/xBfeH7iS1jcXtkSJFC2kZIGGU4PTrXoP8Awzh4r/6LX4w/7+Sf/HaybHTr0ftm396bS4FqbIAT+WfLJ+yIPvdOtfRtYm58y/tHaNceHtB+GGk3WpXGqT2mpCJ724JMk5Gz5myScn6mvpqvEP2q/COt6/4V0fWdBs5L650K+F09vGhZjGRywUcnBVcgdiT2rNt/2r01+xXTvDngjX7vxPMmxLQxqYUlPGS4OdoPPKjjrigDJ+AH/Hz8af8Ar9k/nc1pfsY6BYW3gDUNbWBDf3d+8LzEfMI0VNqg+mST+NZP7N2ja1pWn/FSDWreYXzPtkcocTSBbgMVOPmBbuPUV137IlldWHwpeG7tpreT+0pzslQo2Nqc4NAGH+0NBFbfGX4T3sMax3MuoiJ5VGGZRPDgE+g3t+ZqjY2UPjn9sHVE1hBcW/h+yElpDKMqCqR449mlZ/ritn9oawu7r4q/CeW3tZ5o4dT3SPHGWEY86DliOnQ9fSqfxg8N+J/hz8VbX4ueFNLl1a1kiEOq2cKkvgKEJIAJ2lQvODhlyeKAPePEXh/TvE+iXmjapbR3FndxNFJG4zwR1HoR1B7GvmT4BW32L4I/FW1DB/JW9j3Dvi1YZ/Sun1D9qOTxbpz6N4C8IeILnxFdp5MfnwqI7Zm43kqTnb15wOOSK574BaRqVj8EfiZa3lncx3Lx3aqjxsGkP2UjjI5yaAM21/5MkuP+vk/+lor3r4EaBYaB8J/DUdjbpGbqxiu5mAwZJZFDMxPfk4+gA7V4fbaXfj9jCey+w3X2v7ST5HlN5n/H6D93GenNfQnwnikg+GPhSKVGjkTSbVWRhgqREuQRQB5D4egisf2yPEEdqiwpNpO+RUGAzGOFiT7k8/Wqv7NltB4u+JvxB8Z6mgn1KK98i3aQZMCO0mcZ6fKiL9AR3rV0mwu1/bB1m8NrOLVtIVROYzsJ8qHjd07Gucu/7e/Zs+Kmu6+uh3mreDPELmaR7Rcm3csWAPYFSzgA4BU9cjgA9H/ag8Oafrfwf1i6uoo/tGmhLq2lI+aNg6ggH3UkY+npWv8As+/8kZ8Kf9ef/szV4j8YvjNqXxf8EajpnhHw7qtpoVtH9r1TUr+MRrsQgrGuCRktt75OOmMmvb/2fgR8GfCgIIP2IH/x5qAO/mlSCJ5ZXVI0UszMcBQOpNfGHg743eHLX41eJPiF4jtdVu1mDW2mJZwLJ5ceQoJ3MuD5agcf32r6A/aR8T3vh34W6jBpkFxPqGq4sIlgjLMquD5jcdBsDDPqRWn8DPBI8B/DHRtLli8u8ki+1XYIwfOk+Yg+6jC/8BoA+ZvjT8a/D3izxZ4X8Y+E7LWLTVtFl/ete26xrKgcMgyrt33gj0avRv2qPFi+Ivh14Oi02crp/iO7jnZx3j2AqD+Lg49V9q9s+JPhGLx14F1rw84G69tmWIn+GUfNG34MFNfLvhfwjr/xQ+Bl14RazuYPEPhK++02EdwhjM8Lhsxgt3zvx9FHegD630HQdP8ADei2mj6ZbR29naRLFHGowAAP1J6k9zXzp4U8O2Hhj9sHVbPTYkhtpbGS5EUYwqNJEjMAO3zEnHvWjof7V39laRDpPijwd4iHii3QQvBDAAs8gGM/MQy5IyRtOO2a5b4Qy+JL/wDabn1XxVaGy1TUdOlvHtDnNtGyqI0YdiEC8Hn15oAw/hR8TrnQ/Gni3xZceBtb8U6rf3jKLmyiL/ZE3MSmQpxn5R9FAra+M/xEv/ix4TOlj4S+K7TUYJUmtL17R2MJBG4cJnBXIx64Pats3Gv/ALNHxC8QX58P3useCdfn+1ebZrua0fJOD2BG4jBxuG0g8EVa8RfH7xH8U47fw58JtB1y0vriZPO1S6hVFtkByem5QD3JPTgAk0Ac18Zo7rxdpvwTtvEMNxDdagRb3qTKUk3M1uj5B5BPJ/GvqyLTLK205dOitYEs0i8lbdUAjCYxt29MY4xXgHx60bU08VfCCFjdanNZ36rc3YjJLsJLfLtgYGSCa+iD0oA+PfhlrNx4Z+A3xVm092hMN6YYdh/1fmbYyR6HB6+1e1fsxeGdP0H4RaPd20EYutTVru5mx80jFiFBPoFAAH19a88+AfgeXxR8PPiP4b1KCezGp3zxxvLGVwdvyuAeoDAH8Kq/Dv4wav8AAXSm8DfELwvrHlWEjiyvLOMOsiFicAsQGXJJBB74IGKAPcvEfwl8N+JvGWk+MbpLqDWNKZTFLbSBBJtbIEgwdw6j6EivKPDsH/Cpv2ndQ0j/AFOjeM4DcW46Ks+S2PrvEgA/6aLUega14t+PXxU0bX7Sw1fw94L0M+bmZ2j+2uDuwQDhskKCBkBQecmum/aj8Nzz+DrHxlpnyar4VvI76Jx18vcoYfgQjfRTQBjeOIv+Fo/tJaB4Yx5uleE4P7RvR1UykqwU+uT5I/Fq+gq8T/Zj0i6v9H1z4h6rHt1LxXfSXAB/ggViFUZ7bi34Ba9soAKKKKACiiigAxiiiigAooooAKTaoOQBS0UAFFFFABRRRQAgVR0AFLgUUUAFFFFABivDPE3if4p/DL4galqE+j6j4y8HX3zW8VlGDLY8524Vc8cjngjHIORXudHWgD5p8e+MvHfxz0seC/C3gLWtD0+8kT7fqGrRGFVQMGx0xjIBOCScYAr6B8KeH7fwp4a0vQrUlodPtY7ZWPVtqgbj7nGfxrU6UtABRRRQAVxnxb0zxjqfg2dfAmpmw1yB1miwF/fqM7o8sCBnOQfUDkV2dFAHgth+0L4q07T0ste+E3it9ciQRv8AZrVmhmcDG4Nt4BPpu+pqz8EvA3iy98ca78UPHNiNN1LVY/s9pYH70EPy9R24RFAPPUkDNe4YHpS0ABAPUUgUDoAPpS0UAFFFFABSEBuoB+tLRQAAAdBXzp8VNS+JfxYvbn4caf4IvdH0l9Q8u51qct5M9vHJkOCVAwcBsAsTgAV9F0UAUNB0W08O6JY6PYJstbGBLeJfRVUAfjxV+iigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAAUUUUAFFFFABRRRQAgpaKKACkoooAWiiigAooooAO1IaKKAFNFFFAAKQ0UUAf/9k=' /><br /><br /></div><h2>16 to 19 special post -16 institutions revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your revenue funding allocation statement.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>Derwen College</td></tr>
<tr>
<td>UKPRN</td><td>10001929</td></tr>
<tr>
<td>Local authority</td><td>Shropshire</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021/22 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 50%;"">Core programme funding</td><td style=""width: 50%;"" class=""right"">&pound;508,152</td></tr>
<tr>
<td>Condition of funding adjustment</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Care standards</td><td class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">High needs student</td><td style=""width: 50%;"" class=""right"">&pound;558,000</td></tr>
<tr>
<td style=""width: 50%;"">Student financial support</td><td style=""width: 50%;"" class=""right"">&pound;13,943</td></tr>
<tr>
<td style=""width: 50%;"">Teachers' pension scheme grant</td><td style=""width: 50%;"" class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">Start-up and post-opening grant</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr>
<td style=""width: 50%;"">Maths top up</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total funding allocation</td><td class=""right"">&pound;1,080,095</td></tr></tbody></table><div style='font-weight: bold; font-size: 14px; margin-top: 5px'>Core programme funding</div>
<table id=""formulaTables"" class="""">


<tbody>

<tr>
<td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAADiSURBVGhD7dsxioQwGEDhmGJBEYLiDQIeYgsL72XvmfZyrsuEhYfjzJTj8L5Kfh6SvwuCYXvmr1iW5ftcWNc1pRQe6LquPJ35L35OlKKqqtu5jizoo4phGPYixlgGBxfaxQIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsKOSc67pOKZXBwYV2sQALsiALsiALsqDQ9/1FTmoBFmRBFmRBFmRBb1OM49i27X5PKYODC+1iARZkQRb0UUXOuWkav1oXFvRmxe7rRJimKcZ4i+7b3zPPc+nvee2/k8eeFdv2C7C2ttUtLoinAAAAAElFTkSuQmCC' /></td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Student&nbsp;numbers<br>for 2021/22</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;student&nbsp;numbers<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">93</td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;"">&nbsp;</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">National funding<br>rate per student<br>(dependent on<br>band)</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;breakdown&nbsp;of<br>funding&nbsp;by&nbsp;band&nbsp;table<br><br></td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;""></td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;389,484</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Retention&nbsp;factor</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">0.98300</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">-&pound;6,621</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;382,863</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Programme&nbsp;cost<br>weighting</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.05400</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;20,675</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;403,537</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Level 3<br>programme maths<br>and English<br>payment</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;Level&nbsp;3<br>programme&nbsp;maths&nbsp;and<br>English&nbsp;payment&nbsp;table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;0</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;403,537</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Disadvantage<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;distribution&nbsp;of<br>disadvantage&nbsp;funding<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;104,614</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;508,152</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Large<br>programme<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;large<br>programmes&nbsp;uplift<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;0</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;508,152</td></tr></tbody></table></td><td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAAEKSURBVGhD7dvNaoQwGEZhoyAoEongzYgrBS/MldfkpQliv6GhcJimsyjUMrzPIpiZg2Qgi8Efd11X9iM3DEM8fDLP87qucfKttm23bYuTlBCC2/c9zmhZFhsfRWqleZ7bV1bk8YM0FfQGhXPOxqIoksXnvjnP8+aVflFBKkgFqSAVpIJUkApSQSpIBakgFaSCVJAKUkEqSAWpoGThva+qqus63SsgFaSCVJAKUkEq6G0K59yLwv4+/IuVGhWkglSQClJBKkgF3VyEEJqm6fteVzdIBakgFaSCbi6893Vd66r1ExX0N4UryzIe0nEcNtoufDxwn2JbdRzHzM6RMk2TbebX7538/rdk2QcWtDy+yWdeMAAAAABJRU5ErkJggg==' /></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Area&nbsp;cost<br>allowance</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.00000</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;0</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;508,152</td></tr></tbody></table></td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""3"" scope=""col"">Student numbers</th>    </tr>
</thead>
<tbody>

<tr style=""width: 100%;"">
<td colspan=""2"" style=""width: 80%"">2020/21 ILR R06 students</td><td style=""width: 20%; text-align: right"">93</td></tr>
<tr>
<td colspan=""2"">Exceptional variations to lagged student number</td><td style=""width: 15%; text-align: right"">0</td></tr>
<tr class=""greyBold"">
<td colspan=""2"" class=""bold"">Total student numbers for 2021/22</td><td style=""width: 20%; text-align: right"">93</td></tr>
<tr style=""width: 100%;"">
<td style=""width: 65%;"">Student number methodology used</td><td colspan=""2"" style=""width: 35%;"">2020/21 R06 return</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Breakdown of funding by band</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 30%"">Funding band<br><br></td><td>Student&nbsp;numbers<br>2019/20</td><td>Proportions&nbsp;used&nbsp;in<br>2021/22 allocation</td><td>Number&nbsp;of&nbsp;students<br>allocated in 2021/22</td><td>National&nbsp;funding&nbsp;rate</td><td style=""width: 20%"">Student&nbsp;funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>111</td><td>100.00%</td><td>93</td><td>&pound;4,188</td><td>&pound;389,484</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4 (Sum of bands 4a and 4b)</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;3,455</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4a</td><td>0</td><td>0.00%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4b</td><td>0</td><td>0.00%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,827</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,234</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Band 1</td><td>Students</td><td>0</td><td>0.00%</td><td>0</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td>FTEs</td><td class=""right"">0.00</td><td class=""greyBold""></td><td>0.00</td><td>&pound;4,188</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total student funding</td><td>111</td><td>100%</td><td>93</td><td></td><td>&pound;389,484</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Level 3 programme maths and English payment</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 42%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Instances per student</th><th style=""width: 14%;"" scope=""col"">Number of instances</th><th style=""width: 10%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 1 year programme</td><td>0.00000</td><td>0.00</td><td>&pound;375</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 2 year programme</td><td>0.00000</td><td>0.00</td><td>&pound;750</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""4"" class=""bold"">Level 3 programme maths and English payment funding total</td><td>&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""5"" scope=""col"">Distribution of disadvantage funding</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 1</td></tr>
<tr style=""width: 100%;"">
<td colspan=""2"" rowspan=""2"">Economic deprivation funding</td><td>Block 1 factor</td><td>Funding including programme costs</td><td style=""width: 20%;"">Block 1 funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>1.03800</td><td>&pound;403,537</td><td>&pound;15,334</td></tr>
<tr>
<td colspan=""2"" rowspan=""2"">Care leavers</td><td>Number of qualifying students</td><td>Rate per qualifying student</td><td>Care leaver funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 1 funding</td><td>&pound;15,334</td></tr>
<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 2</td></tr>
<tr>
<td colspan=""4"" rowspan=""2"">Total 2021/22 instances attracting funding per student</td><td>Instances per Student</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>2.00000</td></tr>
<tr>
<td colspan=""3"" class=""right"">Number of instances in each band</td><td>Block 2 funding rates</td><td>Block 2 funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Total funded instances for 2021/22</td><td>186.00</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the higher rate (bands 4 and 5)</td><td>186.00</td><td>&pound;480</td><td>&pound;89,280</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the lower rate (bands 2 and 3)</td><td>0.00</td><td>&pound;292</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Students attracting the FTE rate<br>(Band 1)</td><td>Students</td><td>0.00</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>0.00</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 2 funding</td><td>&pound;89,280</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""4"">Minimum top up if applicable</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total disadvantage funding (sum of block 1, block 2 and minimum top up)</td><td>&pound;104,614</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Large programme uplift (based on 2018/19 students)</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 40%;""></td><td style=""width: 15%;"">Students meeting large programme uplift criteria</td><td style=""width: 25%;"">Funding uplift per student per year</td><td style=""width: 20%;"">Total large programme uplift (2 years)</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 20% of national rate</td><td>0</td><td>&pound;838</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 10% of national rate</td><td>0</td><td>&pound;419</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td class=""bold"">Total large programme funding</td><td>0</td><td></td><td>&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""3"" scope=""col"">2021/22 total programme funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns3OnwardsRightAlign"">
<td>2021/22 total programme funding per student</td><td style=""font-weight: bold;text-align: right;"">&pound;5,464</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Condition of funding (CoF)</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 25%"">Funding band<br><br></td><td>National funding rate<br>in 2019/20</td><td>Total students <br> (2019/20 R14)</td><td>National funding rate<br>applied to total students <br> (2019/20 R14)</td><td>Students not meeting<br>CoF (2019/20 R14)</td><td style=""width: 20%"">National funding rate applied to<br>CoF non-compliant students</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>&pound;4,000</td><td>111</td><td>&pound;444,000</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4</td><td>&pound;3,300</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>&pound;2,700</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>&pound;2,133</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"" style=""width: 20%"">Band 1</td><td>Students</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>&pound;4,000</td><td>0.00</td><td>&pound;0</td><td>0.00</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total</td><td>111</td><td>&pound;444,000</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">5% of national rate funding for total 2019/20 R14 students</td><td>&pound;22,200</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">Funding for non-compliant students less 5% of funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""6"" class=""bold"">Final condition of funding adjustment (at 50%)</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Advanced maths premium</caption>


<thead class=""whiteBackground top"">
    <tr>
<th style=""width: 24%;"" scope=""col""></th><th style=""width: 13%;"" scope=""col"">Baseline students</th><th style=""width: 14%;"" scope=""col"">Eligible students</th><th style=""width: 14%;"" scope=""col"">Eligible minus<br>baseline</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Advanced maths premium funding</td><td>0</td><td>0</td><td>0</td><td>&pound;600</td><td class=""bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">High value courses premium</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Qualifying students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>High value courses premium funding</td><td>0</td><td>&pound;400</td><td class=""bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Industry placements funding</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Number&nbsp;of&nbsp;students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Industry&nbsp;placements:&nbsp;Capacity&nbsp;and<br>delivery&nbsp;funding&nbsp;(CDF)</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Industry&nbsp;placements:&nbsp;Capacity&nbsp;and&nbsp;delivery&nbsp;funding&nbsp;(CDF)</td><td>0</td><td>&pound;250</td><td class=""bold"">&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Care standards funding</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td>Eligible students</td><td style=""width: 15%;"">Rate</td><td style=""width: 20%;"" class=""right"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Care standards student funding</td><td>0</td><td>&pound;817</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Care standards institution lump sum funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total care standards funding</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">High needs funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">16-19 students</td><td style=""width: 15%"">19-24 students</td><td style=""width: 15%"">Total students</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>2020/21 R06 total high needs students</td><td>26</td><td>67</td><td>93</td><td class=""greyBold""></td><td class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>2020/21 R06 high needs student proportions by age</td><td>27.96%</td><td>72.04%</td><td class=""greyBold""></td><td class=""greyBold""></td><td class=""greyBold""></td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td>Exceptional variations to lagged high needs student number</td><td class=""greyBold""></td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td><td class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>High needs element 2 for 2021/22</td><td>26</td><td>67</td><td>93</td><td>&pound;6,000</td><td style=""font-weight: bold"">&pound;558,000</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">Student financial support funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">Number of funded students</td><td style=""width: 15%"">Instances per student</td><td style=""width: 15%"">Number of instances</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 1: Financial disadvantage</td><td>93</td><td>0.26600</td><td>24.76</td><td>&pound;242</td><td>&pound;5,991</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 2a: Student costs - travel</td><td>93</td><td>0.14000</td><td>13.01</td><td>&pound;483</td><td>&pound;6,282</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Element 2b: Student costs - industry placements</td><td>0</td><td>&pound;49</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Bursary adjustment in respect of free meals</td><td>-&pound;1,488</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund</td><td>&pound;10,785</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">2019 to 2020 discretionary bursary fund - baseline for transition</td><td>&pound;10,635</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition lower limit</td><td>&pound;7,976</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition upper limit</td><td>&pound;13,293</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Exceptional adjustment</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund total</td><td>&pound;10,785</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Residential Bursary Fund</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Residential Support Scheme</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Residential funding total</td><td>&pound;0</td></tr>
<tr style=""height: 50px"">
<td colspan=""2"" style=""font-weight: bold""></td><td style=""vertical-align: bottom"">Total students</td><td style=""vertical-align: bottom"">Free meals<br>students</td><td style=""vertical-align: bottom"">Proportion of students<br>on free meals</td><td style=""vertical-align: bottom"">Total students in 2021/22 funded<br>for free meals</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals students</td><td>111</td><td>10</td><td>9.01%</td><td>8.38</td></tr>
<tr>
<td colspan=""3""></td><td>Free meals students</td><td style=""vertical-align: bottom"">Free meals rates</td><td style=""vertical-align: bottom"">Free meals funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals higher rate</td><td class=""greyBold""></td><td>8.38</td><td>&pound;358</td><td>&pound;2,999</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals lower rate</td><td class=""greyBold""></td><td>0.00</td><td>&pound;179</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals FTE rate </td><td class=""greyBold""></td><td>0.00</td><td>&pound;358</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<th colspan=""5"" style=""text-align: left;"" scope=""row"">Free meals sub-total</th><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Free meals administration</td><td colspan=""4"" class=""greyBold""></td><td>&pound;159</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Exceptional adjustment</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Total free meals funding</td><td>&pound;3,158</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Total student support funding</td><td>&pound;13,943</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Teachers' pension scheme grant</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 20%;"" scope=""col""></th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Payments made to Capita<br>for TPS, financial year<br>2019-2020 rebased at<br>16.4% for the full year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Annual payments<br>increased to 23.6%<br>equivalent for the full<br>year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">3.1% uplift for 2020-21</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">3% uplift for 2021-22</th><th style=""vertical-align: top; width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Revised annual cost</td><td>&pound;0</td><td>&pound;0</td><td>&pound;0</td><td>&pound;0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Teachers'&nbsp;pension&nbsp;scheme&nbsp;grant&nbsp;(difference&nbsp;between&nbsp;2019/2020&nbsp;payments&nbsp;at&nbsp;16.4%&nbsp;and revised&nbsp;annual&nbsp;cost)</td><td style=""font-weight: bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Start-up and post-opening grant</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part A</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part B</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - per pupil resources</td><td>-&pound;997</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - leadership disecononomies</td><td>-&pound;997</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td style=""font-weight: bold"">Total start-up and post-opening grant</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Maths top up</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Maths top up funding</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style='font-size: 10px; margin-top: 5px'>The values on your statement are shown rounded to various numbers of decimal places.<br> The calculation of your funding however is done using un-rounded values. This may result in some slight differences when you work through the calculation yourselves.</div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_FE_with =
      @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>16 to 19 revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your revenue funding allocation statement.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>Truro and Penwith College</td></tr>
<tr>
<td>UKPRN</td><td>10007063</td></tr>
<tr>
<td>Local authority</td><td>Cornwall</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021/22 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 50%;"">Core programme funding</td><td style=""width: 50%;"" class=""right"">&pound;22,508,598</td></tr>
<tr>
<td>Condition of funding adjustment</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;152,800</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;218,875</td></tr>
<tr>
<td>Care standards</td><td class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">High needs student</td><td style=""width: 50%;"" class=""right"">&pound;1,302,000</td></tr>
<tr>
<td style=""width: 50%;"">Student financial support</td><td style=""width: 50%;"" class=""right"">&pound;905,211</td></tr>
<tr>
<td style=""width: 50%;"">Teachers' pension scheme grant</td><td style=""width: 50%;"" class=""right"">&pound;682,402</td></tr>
<tr>
<td style=""width: 50%;"">Alternative completion</td><td style=""width: 50%;"" class=""right"">&pound;34,034</td></tr>
<tr>
<td style=""width: 50%;"">Start-up and post-opening grant</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr>
<td style=""width: 50%;"">Maths top up</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total funding allocation</td><td class=""right"">&pound;25,803,920</td></tr></tbody></table><div style='font-weight: bold; font-size: 14px; margin-top: 5px'>Core programme funding</div>
<table id=""formulaTables"" class="""">


<tbody>

<tr>
<td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAADiSURBVGhD7dsxioQwGEDhmGJBEYLiDQIeYgsL72XvmfZyrsuEhYfjzJTj8L5Kfh6SvwuCYXvmr1iW5ftcWNc1pRQe6LquPJ35L35OlKKqqtu5jizoo4phGPYixlgGBxfaxQIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsKOSc67pOKZXBwYV2sQALsiALsiALsqDQ9/1FTmoBFmRBFmRBFmRBb1OM49i27X5PKYODC+1iARZkQRb0UUXOuWkav1oXFvRmxe7rRJimKcZ4i+7b3zPPc+nvee2/k8eeFdv2C7C2ttUtLoinAAAAAElFTkSuQmCC' /></td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Student&nbsp;numbers<br>for 2021/22</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;student&nbsp;numbers<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">4,946</td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;"">&nbsp;</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">National funding<br>rate per student<br>(dependent on<br>band)</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;breakdown&nbsp;of<br>funding&nbsp;by&nbsp;band&nbsp;table<br><br></td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;""></td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;19,985,637</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Retention&nbsp;factor</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">0.96500</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">-&pound;693,701</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;19,291,935</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Programme&nbsp;cost<br>weighting</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.07100</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;1,367,027</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;20,658,962</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Level 3<br>programme maths<br>and English<br>payment</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;Level&nbsp;3<br>programme&nbsp;maths&nbsp;and<br>English&nbsp;payment&nbsp;table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;169,579</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;20,828,540</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Disadvantage<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;distribution&nbsp;of<br>disadvantage&nbsp;funding<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;1,588,716</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;22,417,256</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Large<br>programme<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;large<br>programmes&nbsp;uplift<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;91,342</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;22,508,598</td></tr></tbody></table></td><td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAAEKSURBVGhD7dvNaoQwGEZhoyAoEongzYgrBS/MldfkpQliv6GhcJimsyjUMrzPIpiZg2Qgi8Efd11X9iM3DEM8fDLP87qucfKttm23bYuTlBCC2/c9zmhZFhsfRWqleZ7bV1bk8YM0FfQGhXPOxqIoksXnvjnP8+aVflFBKkgFqSAVpIJUkApSQSpIBakgFaSCVJAKUkEqSAWpoGThva+qqus63SsgFaSCVJAKUkEq6G0K59yLwv4+/IuVGhWkglSQClJBKkgF3VyEEJqm6fteVzdIBakgFaSCbi6893Vd66r1ExX0N4UryzIe0nEcNtoufDxwn2JbdRzHzM6RMk2TbebX7538/rdk2QcWtDy+yWdeMAAAAABJRU5ErkJggg==' /></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Area&nbsp;cost<br>allowance</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.00000</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;0</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;22,508,598</td></tr></tbody></table></td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""3"" scope=""col"">Student numbers</th>    </tr>
</thead>
<tbody>

<tr style=""width: 100%;"">
<td colspan=""2"" style=""width: 80%"">2020/21 R04 students</td><td style=""width: 20%; text-align: right"">4,926</td></tr>
<tr>
<td colspan=""2"">2019/20 R04 to R14 ratio</td><td style=""width: 15%; text-align: right"">1.00400</td></tr>
<tr>
<td colspan=""2"">Total lagged student number</td><td style=""width: 20%; text-align: right"">4,946</td></tr>
<tr>
<td colspan=""2"">Exceptional variations to lagged student number</td><td style=""width: 15%; text-align: right"">0</td></tr>
<tr class=""greyBold"">
<td colspan=""2"" class=""bold"">Total student numbers for 2021/22</td><td style=""width: 20%; text-align: right"">4,946</td></tr>
<tr style=""width: 100%;"">
<td style=""width: 65%;"">Student number methodology used</td><td colspan=""2"" style=""width: 35%;"">2020/21 R04 x R04:R14 ratio</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Breakdown of funding by band</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 30%"">Funding band<br><br></td><td>Student&nbsp;numbers<br>2019/20</td><td>Proportions&nbsp;used&nbsp;in<br>2021/22 allocation</td><td>Number&nbsp;of&nbsp;students<br>allocated in 2021/22</td><td>National&nbsp;funding&nbsp;rate</td><td style=""width: 20%"">Student&nbsp;funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">T Level band 9</td><td class=""greyBold""></td><td class=""greyBold""></td><td>0</td><td>&pound;6,108</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">T Level band 8</td><td class=""greyBold""></td><td class=""greyBold""></td><td>0</td><td>&pound;5,584</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">T Level band 7</td><td class=""greyBold""></td><td class=""greyBold""></td><td>45</td><td>&pound;5,061</td><td>&pound;227,745</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">T Level band 6</td><td class=""greyBold""></td><td class=""greyBold""></td><td>0</td><td>&pound;4,363</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total T Level bands</td><td></td><td></td><td>45</td><td></td><td>&pound;227,745</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>4,170</td><td>84.16%</td><td>4,117</td><td>&pound;4,188</td><td>&pound;17,243,651</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4 (Sum of bands 4a and 4b)</td><td>670</td><td>13.52%</td><td>669</td><td>&pound;3,455</td><td>&pound;2,310,628</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4a</td><td>610</td><td>12.31%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4b</td><td>60</td><td>1.21%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>47</td><td>0.95%</td><td>47</td><td>&pound;2,827</td><td>&pound;132,627</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>8</td><td>0.16%</td><td>8</td><td>&pound;2,234</td><td>&pound;17,839</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Band 1</td><td>Students</td><td>60</td><td>1.21%</td><td>60</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td>FTEs</td><td class=""right"">12.71</td><td class=""greyBold""></td><td>12.69</td><td>&pound;4,188</td><td>&pound;53,146</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total mainstream bands</td><td>4,955</td><td>100%</td><td>4,901</td><td></td><td>&pound;19,757,892</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total student funding</td><td></td><td></td><td>4,946</td><td></td><td>&pound;19,985,637</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Level 3 programme maths and English payment</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 42%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Instances per student</th><th style=""width: 14%;"" scope=""col"">Number of instances</th><th style=""width: 10%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 1 year programme</td><td>0.08600</td><td>424.22</td><td>&pound;375</td><td>&pound;159,081</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 2 year programme</td><td>0.00300</td><td>14.00</td><td>&pound;750</td><td>&pound;10,498</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""4"" class=""bold"">Level 3 programme maths and English payment funding total</td><td>&pound;169,579</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""5"" scope=""col"">Distribution of disadvantage funding</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 1</td></tr>
<tr style=""width: 100%;"">
<td colspan=""2"" rowspan=""2"">Economic deprivation funding</td><td>Block 1 factor</td><td>Funding including programme costs</td><td style=""width: 20%;"">Block 1 funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>1.02900</td><td>&pound;20,828,540</td><td>&pound;599,237</td></tr>
<tr>
<td colspan=""2"" rowspan=""2"">Care leavers</td><td>Number of qualifying students</td><td>Rate per qualifying student</td><td>Care leaver funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>37</td><td>&pound;480</td><td>&pound;17,760</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 1 funding</td><td>&pound;616,997</td></tr>
<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 2</td></tr>
<tr>
<td colspan=""4"" rowspan=""2"">Total 2021/22 instances attracting funding per student</td><td>Instances per Student</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0.41400</td></tr>
<tr>
<td colspan=""3"" class=""right"">Number of instances in each band</td><td>Block 2 funding rates</td><td>Block 2 funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Total funded instances for 2021/22</td><td>2,046.24</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the higher rate (bands 4 and 5)</td><td>1,980.14</td><td>&pound;480</td><td>&pound;950,465</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the lower rate (bands 2 and 3)</td><td>22.71</td><td>&pound;292</td><td>&pound;6,632</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Students attracting the FTE rate<br>(Band 1)</td><td>Students</td><td>24.78</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>5.25</td><td>&pound;480</td><td>&pound;2,520</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the T level rate</td><td>18.62</td><td>&pound;650</td><td>&pound;12,101</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 2 funding including T Levels</td><td>&pound;971,719</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""4"">Minimum top up if applicable</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total disadvantage funding (sum of block 1, block 2 and minimum top up)</td><td>&pound;1,588,716</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Large programme uplift (based on 2018/19 students)</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 40%;""></td><td style=""width: 15%;"">Students meeting large programme uplift criteria</td><td style=""width: 25%;"">Funding uplift per student per year</td><td style=""width: 20%;"">Total large programme uplift (2 years)</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 20% of national rate</td><td>39</td><td>&pound;838</td><td>&pound;65,364</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 10% of national rate</td><td>31</td><td>&pound;419</td><td>&pound;25,978</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td class=""bold"">Total large programme funding</td><td>70</td><td></td><td>&pound;91,342</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Condition of funding (CoF)</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 25%"">Funding band<br><br></td><td>National funding rate<br>in 2019/20</td><td>Total students <br> (2019/20 R14)</td><td>National funding rate<br>applied to total students <br> (2019/20 R14)</td><td>Students not meeting<br>CoF (2019/20 R14)</td><td style=""width: 20%"">National funding rate applied to<br>CoF non-compliant students</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>&pound;4,000</td><td>4,170</td><td>&pound;16,680,000</td><td>48</td><td>&pound;192,000</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4</td><td>&pound;3,300</td><td>628</td><td>&pound;2,072,400</td><td>18</td><td>&pound;59,400</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>&pound;2,700</td><td>47</td><td>&pound;126,900</td><td>6</td><td>&pound;16,200</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>&pound;2,133</td><td>8</td><td>&pound;17,064</td><td>2</td><td>&pound;4,266</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"" style=""width: 20%"">Band 1</td><td>Students</td><td class=""greyBold""></td><td>60</td><td class=""greyBold""></td><td>5</td><td class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>&pound;4,000</td><td>12.71</td><td>&pound;50,853</td><td>1.86</td><td>&pound;7,427</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total</td><td>4,913</td><td>&pound;18,947,217</td><td>79</td><td>&pound;279,293</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">5% of national rate funding for total 2019/20 R14 students</td><td>&pound;947,361</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">Funding for non-compliant students less 5% of funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""6"" class=""bold"">Final condition of funding adjustment (at 50%)</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Advanced maths premium</caption>


<thead class=""whiteBackground top"">
    <tr>
<th style=""width: 24%;"" scope=""col""></th><th style=""width: 13%;"" scope=""col"">Baseline students</th><th style=""width: 14%;"" scope=""col"">Eligible students</th><th style=""width: 14%;"" scope=""col"">Eligible minus<br>baseline</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Advanced maths premium funding</td><td>837</td><td>576</td><td>0</td><td>&pound;600</td><td class=""bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">High value courses premium</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Qualifying students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>High value courses premium funding</td><td>382</td><td>&pound;400</td><td class=""bold"">&pound;152,800</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Industry placements funding</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Number&nbsp;of&nbsp;students&nbsp;minus&nbsp;final&nbsp;T<br>Level&nbsp;students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Industry&nbsp;placements:&nbsp;Capacity&nbsp;and<br>delivery&nbsp;funding&nbsp;(CDF)</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Industry&nbsp;placements:&nbsp;Capacity&nbsp;and&nbsp;delivery&nbsp;funding&nbsp;(CDF)</td><td>826</td><td>&pound;250</td><td class=""bold"">&pound;206,500</td></tr>
<tr>
<td></td><td>Number of estimated T<br>Level students</td><td>Rate</td><td>Industry placements: T Level<br>funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Industry placements: T Level funding</td><td>45</td><td>&pound;275</td><td>&pound;12,375</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Industry placements funding total</td><td>&pound;218,875</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Care standards funding</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td>Eligible students</td><td style=""width: 15%;"">Rate</td><td style=""width: 20%;"" class=""right"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Care standards student funding</td><td>0</td><td>&pound;817</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Care standards institution lump sum funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total care standards funding</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">High needs funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">16-19 students</td><td style=""width: 15%"">19-24 students</td><td style=""width: 15%"">Total students</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>High needs element 2 for 2021/22</td><td>143</td><td>74</td><td>217</td><td>&pound;6,000</td><td style=""font-weight: bold"">&pound;1,302,000</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">Student financial support funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">Number of funded students</td><td style=""width: 15%"">Instances per student</td><td style=""width: 15%"">Number of instances</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 1: Financial disadvantage</td><td>4,946</td><td>0.17400</td><td>862.43</td><td>&pound;242</td><td>&pound;208,707</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 2a: Student costs - travel</td><td>4,946</td><td>0.51600</td><td>2,554.54</td><td>&pound;483</td><td>&pound;1,233,843</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Element 2b: Student costs - industry placements</td><td>770</td><td>&pound;49</td><td>&pound;37,730</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Bursary adjustment in respect of free meals</td><td>-&pound;95,935</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund</td><td>&pound;1,384,346</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">2019 to 2020 discretionary bursary fund - baseline for transition</td><td>&pound;530,927</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition lower limit</td><td>&pound;398,196</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition upper limit</td><td>&pound;663,659</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Exceptional adjustment</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund total</td><td>&pound;663,659</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Residential Bursary Fund</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Residential Support Scheme</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Residential funding total</td><td>&pound;0</td></tr>
<tr style=""height: 50px"">
<td colspan=""2"" style=""font-weight: bold""></td><td style=""vertical-align: bottom"">Total students</td><td style=""vertical-align: bottom"">Free meals<br>students</td><td style=""vertical-align: bottom"">Proportion of students<br>on free meals</td><td style=""vertical-align: bottom"">Total students in 2021/22 funded<br>for free meals</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals students</td><td>4,995</td><td>652</td><td>13.16%</td><td>650.81</td></tr>
<tr>
<td colspan=""3""></td><td>Free meals students</td><td style=""vertical-align: bottom"">Free meals rates</td><td style=""vertical-align: bottom"">Free meals funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals higher rate</td><td class=""greyBold""></td><td>635.71</td><td>&pound;358</td><td>&pound;227,583</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals lower rate</td><td class=""greyBold""></td><td>7.22</td><td>&pound;179</td><td>&pound;1,293</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals FTE rate </td><td class=""greyBold""></td><td>1.67</td><td>&pound;358</td><td>&pound;598</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<th colspan=""5"" style=""text-align: left;"" scope=""row"">Free meals sub-total</th><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Free meals administration</td><td colspan=""4"" class=""greyBold""></td><td>&pound;12,078</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Exceptional adjustment</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Total free meals funding</td><td>&pound;241,552</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Total student support funding</td><td>&pound;905,211</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Teachers' pension scheme grant</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 20%;"" scope=""col""></th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Payments made to Capita<br>for TPS, financial year<br>2019-2020 rebased at<br>16.4% for the full year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Annual payments<br>increased to 23.6%<br>equivalent for the full<br>year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">3.1% uplift for 2020-21</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">3% uplift for 2021-22</th><th style=""vertical-align: top; width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Revised annual cost</td><td>&pound;1,938,115</td><td>&pound;2,788,995</td><td>&pound;86,459</td><td>&pound;86,264</td><td>&pound;2,961,718</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Teachers'&nbsp;pension&nbsp;scheme&nbsp;grant&nbsp;(difference&nbsp;between&nbsp;2019/2020&nbsp;payments&nbsp;at&nbsp;16.4%&nbsp;and revised&nbsp;annual&nbsp;cost)</td><td style=""font-weight: bold"">&pound;682,402</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead class=""greyBold"">
    <tr>
<th colspan=""2"" scope=""col"">Alternative completion</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td>Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Alternative completion funding</td><td style=""font-weight: bold"">&pound;34,034</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Start-up and post-opening grant</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part A</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part B</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - per pupil resources</td><td>-&pound;997</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - leadership disecononomies</td><td>-&pound;997</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td style=""font-weight: bold"">Total start-up and post-opening grant</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Maths top up</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Maths top up funding</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style='font-size: 10px; margin-top: 5px'>The values on your statement are shown rounded to various numbers of decimal places.<br> The calculation of your funding however is done using un-rounded values. This may result in some slight differences when you work through the calculation yourselves.</div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_FE_without =
      @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>16 to 19 revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your revenue funding allocation statement.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>Barton Peveril College</td></tr>
<tr>
<td>UKPRN</td><td>10000552</td></tr>
<tr>
<td>Local authority</td><td>Hampshire</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021/22 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 50%;"">Core programme funding</td><td style=""width: 50%;"" class=""right"">&pound;15,562,634</td></tr>
<tr>
<td>Condition of funding adjustment</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;115,800</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;321,600</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;70,250</td></tr>
<tr>
<td>Care standards</td><td class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">High needs student</td><td style=""width: 50%;"" class=""right"">&pound;120,000</td></tr>
<tr>
<td style=""width: 50%;"">Student financial support</td><td style=""width: 50%;"" class=""right"">&pound;221,954</td></tr>
<tr>
<td style=""width: 50%;"">Teachers' pension scheme grant</td><td style=""width: 50%;"" class=""right"">&pound;283,705</td></tr>
<tr>
<td style=""width: 50%;"">Start-up and post-opening grant</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr>
<td style=""width: 50%;"">Maths top up</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total funding allocation</td><td class=""right"">&pound;16,695,943</td></tr></tbody></table><div style='font-weight: bold; font-size: 14px; margin-top: 5px'>Core programme funding</div>
<table id=""formulaTables"" class="""">


<tbody>

<tr>
<td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAADiSURBVGhD7dsxioQwGEDhmGJBEYLiDQIeYgsL72XvmfZyrsuEhYfjzJTj8L5Kfh6SvwuCYXvmr1iW5ftcWNc1pRQe6LquPJ35L35OlKKqqtu5jizoo4phGPYixlgGBxfaxQIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsKOSc67pOKZXBwYV2sQALsiALsiALsqDQ9/1FTmoBFmRBFmRBFmRBb1OM49i27X5PKYODC+1iARZkQRb0UUXOuWkav1oXFvRmxe7rRJimKcZ4i+7b3zPPc+nvee2/k8eeFdv2C7C2ttUtLoinAAAAAElFTkSuQmCC' /></td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Student&nbsp;numbers<br>for 2021/22</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;student&nbsp;numbers<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">3,577</td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;"">&nbsp;</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">National funding<br>rate per student<br>(dependent on<br>band)</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;breakdown&nbsp;of<br>funding&nbsp;by&nbsp;band&nbsp;table<br><br></td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;""></td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;14,790,865</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Retention&nbsp;factor</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">0.96200</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">-&pound;562,053</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;14,228,812</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Programme&nbsp;cost<br>weighting</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.03800</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;539,983</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;14,768,796</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Level 3<br>programme maths<br>and English<br>payment</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;Level&nbsp;3<br>programme&nbsp;maths&nbsp;and<br>English&nbsp;payment&nbsp;table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;88,250</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;14,857,045</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Disadvantage<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;distribution&nbsp;of<br>disadvantage&nbsp;funding<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;373,623</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;14,857,045</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Large<br>programme<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;large<br>programmes&nbsp;uplift<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;26,816</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;15,257,485</td></tr></tbody></table></td><td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAAEKSURBVGhD7dvNaoQwGEZhoyAoEongzYgrBS/MldfkpQliv6GhcJimsyjUMrzPIpiZg2Qgi8Efd11X9iM3DEM8fDLP87qucfKttm23bYuTlBCC2/c9zmhZFhsfRWqleZ7bV1bk8YM0FfQGhXPOxqIoksXnvjnP8+aVflFBKkgFqSAVpIJUkApSQSpIBakgFaSCVJAKUkEqSAWpoGThva+qqus63SsgFaSCVJAKUkEq6G0K59yLwv4+/IuVGhWkglSQClJBKkgF3VyEEJqm6fteVzdIBakgFaSCbi6893Vd66r1ExX0N4UryzIe0nEcNtoufDxwn2JbdRzHzM6RMk2TbebX7538/rdk2QcWtDy+yWdeMAAAAABJRU5ErkJggg==' /></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Area&nbsp;cost<br>allowance</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.02000</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;305,150</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;15,562,634</td></tr></tbody></table></td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""3"" scope=""col"">Student numbers</th>    </tr>
</thead>
<tbody>

<tr style=""width: 100%;"">
<td colspan=""2"" style=""width: 80%"">2020/21 R04 students</td><td style=""width: 20%; text-align: right"">3,576</td></tr>
<tr>
<td colspan=""2"">2019/20 R04 to R14 ratio</td><td style=""width: 15%; text-align: right"">1.00000</td></tr>
<tr>
<td colspan=""2"">Total lagged student number</td><td style=""width: 20%; text-align: right"">3,577</td></tr>
<tr>
<td colspan=""2"">Exceptional variations to lagged student number</td><td style=""width: 15%; text-align: right"">0</td></tr>
<tr class=""greyBold"">
<td colspan=""2"" class=""bold"">Total student numbers for 2021/22</td><td style=""width: 20%; text-align: right"">3,577</td></tr>
<tr style=""width: 100%;"">
<td style=""width: 65%;"">Student number methodology used</td><td colspan=""2"" style=""width: 35%;"">2020/21 R04 x R04:R14 ratio</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Breakdown of funding by band</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 30%"">Funding band<br><br></td><td>Student&nbsp;numbers<br>2019/20</td><td>Proportions&nbsp;used&nbsp;in<br>2021/22 allocation</td><td>Number&nbsp;of&nbsp;students<br>allocated in 2021/22</td><td>National&nbsp;funding&nbsp;rate</td><td style=""width: 20%"">Student&nbsp;funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>3,245</td><td>93.62%</td><td>3,349</td><td>&pound;4,188</td><td>&pound;14,025,413</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4 (Sum of bands 4a and 4b)</td><td>190</td><td>5.48%</td><td>196</td><td>&pound;3,455</td><td>&pound;677,479</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4a</td><td>148</td><td>4.27%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4b</td><td>42</td><td>1.21%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>29</td><td>0.84%</td><td>30</td><td>&pound;2,827</td><td>&pound;84,609</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,234</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Band 1</td><td>Students</td><td>2</td><td>0.06%</td><td>2</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td>FTEs</td><td class=""right"">0.78</td><td class=""greyBold""></td><td>0.80</td><td>&pound;4,188</td><td>&pound;3,364</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total student funding</td><td>3,466</td><td>100%</td><td>3,577</td><td></td><td>&pound;14,790,865</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Level 3 programme maths and English payment</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 42%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Instances per student</th><th style=""width: 14%;"" scope=""col"">Number of instances</th><th style=""width: 10%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 1 year programme</td><td>0.00200</td><td>78.44</td><td>&pound;375</td><td>&pound;29,417</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 2 year programme</td><td>0.02200</td><td>78.44</td><td>&pound;750</td><td>&pound;58,833</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""4"" class=""bold"">Level 3 programme maths and English payment funding total</td><td>&pound;88,250</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""5"" scope=""col"">Distribution of disadvantage funding</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 1</td></tr>
<tr style=""width: 100%;"">
<td colspan=""2"" rowspan=""2"">Economic deprivation funding</td><td>Block 1 factor</td><td>Funding including programme costs</td><td style=""width: 20%;"">Block 1 funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>1.01200</td><td>&pound;14,857,045</td><td>&pound;178,730</td></tr>
<tr>
<td colspan=""2"" rowspan=""2"">Care leavers</td><td>Number of qualifying students</td><td>Rate per qualifying student</td><td>Care leaver funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>5</td><td>&pound;480</td><td>&pound;2,400</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 1 funding</td><td>&pound;181,130</td></tr>
<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 2</td></tr>
<tr>
<td colspan=""4"" rowspan=""2"">Total 2021/22 instances attracting funding per student</td><td>Instances per Student</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0.11300</td></tr>
<tr>
<td colspan=""3"" class=""right"">Number of instances in each band</td><td>Block 2 funding rates</td><td>Block 2 funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Total funded instances for 2021/22</td><td>402.49</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the higher rate (bands 4 and 5)</td><td>398.89</td><td>&pound;480</td><td>&pound;191,466</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the lower rate (bands 2 and 3)</td><td>3.37</td><td>&pound;292</td><td>&pound;983</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Students attracting the FTE rate<br>(Band 1)</td><td>Students</td><td>0.23</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>0.09</td><td>&pound;480</td><td>&pound;43</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 2 funding</td><td>&pound;192,493</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""4"">Minimum top up if applicable</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total disadvantage funding (sum of block 1, block 2 and minimum top up)</td><td>&pound;373,623</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Large programme uplift (based on 2018/19 students)</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 40%;""></td><td style=""width: 15%;"">Students meeting large programme uplift criteria</td><td style=""width: 25%;"">Funding uplift per student per year</td><td style=""width: 20%;"">Total large programme uplift (2 years)</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 20% of national rate</td><td>0</td><td>&pound;838</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 10% of national rate</td><td>32</td><td>&pound;419</td><td>&pound;26,816</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td class=""bold"">Total large programme funding</td><td>32</td><td></td><td>&pound;26,816</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Condition of funding (CoF)</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 25%"">Funding band<br><br></td><td>National funding rate<br>in 2019/20</td><td>Total students <br> (2019/20 R14)</td><td>National funding rate<br>applied to total students <br> (2019/20 R14)</td><td>Students not meeting<br>CoF (2019/20 R14)</td><td style=""width: 20%"">National funding rate applied to<br>CoF non-compliant students</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>&pound;4,000</td><td>3,245</td><td>&pound;12,980,000</td><td>34</td><td>&pound;136,000</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4</td><td>&pound;3,300</td><td>190</td><td>&pound;627,000</td><td>1</td><td>&pound;3,300</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>&pound;2,700</td><td>29</td><td>&pound;78,300</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>&pound;2,133</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"" style=""width: 20%"">Band 1</td><td>Students</td><td class=""greyBold""></td><td>2</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>&pound;4,000</td><td>0.78</td><td>&pound;3,113</td><td>0.00</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total</td><td>3,466</td><td>&pound;13,688,413</td><td>35</td><td>&pound;139,300</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">5% of national rate funding for total 2019/20 R14 students</td><td>&pound;684,421</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">Funding for non-compliant students less 5% of funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""6"" class=""bold"">Final condition of funding adjustment (at 50%)</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Advanced maths premium</caption>


<thead class=""whiteBackground top"">
    <tr>
<th style=""width: 24%;"" scope=""col""></th><th style=""width: 13%;"" scope=""col"">Baseline students</th><th style=""width: 14%;"" scope=""col"">Eligible students</th><th style=""width: 14%;"" scope=""col"">Eligible minus<br>baseline</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Advanced maths premium funding</td><td>849</td><td>1,042</td><td>193</td><td>&pound;600</td><td class=""bold"">&pound;115,800</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">High value courses premium</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Qualifying students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>High value courses premium funding</td><td>804</td><td>&pound;400</td><td class=""bold"">&pound;321,600</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Industry placements funding</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Number&nbsp;of&nbsp;students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Industry&nbsp;placements:&nbsp;Capacity&nbsp;and<br>delivery&nbsp;funding&nbsp;(CDF)</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Industry&nbsp;placements:&nbsp;Capacity&nbsp;and&nbsp;delivery&nbsp;funding&nbsp;(CDF)</td><td>281</td><td>&pound;250</td><td class=""bold"">&pound;70,250</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Care standards funding</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td>Eligible students</td><td style=""width: 15%;"">Rate</td><td style=""width: 20%;"" class=""right"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Care standards student funding</td><td>0</td><td>&pound;817</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Care standards institution lump sum funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total care standards funding</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">High needs funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">16-19 students</td><td style=""width: 15%"">19-24 students</td><td style=""width: 15%"">Total students</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>High needs element 2 for 2021/22</td><td>20</td><td>0</td><td>20</td><td>&pound;6,000</td><td style=""font-weight: bold"">&pound;120,000</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">Student financial support funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">Number of funded students</td><td style=""width: 15%"">Instances per student</td><td style=""width: 15%"">Number of instances</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 1: Financial disadvantage</td><td>3,577</td><td>0.01700</td><td>253.25</td><td>&pound;242</td><td>&pound;61,287</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 2a: Student costs - travel</td><td>3,577</td><td>0.05900</td><td>211.58</td><td>&pound;483</td><td>&pound;102,194</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Element 2b: Student costs - industry placements</td><td>90</td><td>&pound;49</td><td>&pound;4,410</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Bursary adjustment in respect of free meals</td><td>-&pound;13,393</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund</td><td>&pound;154,498</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">2019 to 2020 discretionary bursary fund - baseline for transition</td><td>&pound;228,832</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition lower limit</td><td>&pound;171,624</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition upper limit</td><td>&pound;286,040</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Exceptional adjustment</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund total</td><td>&pound;171,624</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Residential Bursary Fund</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Residential Support Scheme</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Residential funding total</td><td>&pound;0</td></tr>
<tr style=""height: 50px"">
<td colspan=""2"" style=""font-weight: bold""></td><td style=""vertical-align: bottom"">Total students</td><td style=""vertical-align: bottom"">Free meals<br>students</td><td style=""vertical-align: bottom"">Proportion of students<br>on free meals</td><td style=""vertical-align: bottom"">Total students in 2021/22 funded<br>for free meals</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals students</td><td>3,466</td><td>130</td><td>3.75%</td><td>134.16</td></tr>
<tr>
<td colspan=""3""></td><td>Free meals students</td><td style=""vertical-align: bottom"">Free meals rates</td><td style=""vertical-align: bottom"">Free meals funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals higher rate</td><td class=""greyBold""></td><td>132.96</td><td>&pound;358</td><td>&pound;47,601</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals lower rate</td><td class=""greyBold""></td><td>1.12</td><td>&pound;179</td><td>&pound;201</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals FTE rate </td><td class=""greyBold""></td><td>0.03</td><td>&pound;358</td><td>&pound;11</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<th colspan=""5"" style=""text-align: left;"" scope=""row"">Free meals sub-total</th><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Free meals administration</td><td colspan=""4"" class=""greyBold""></td><td>&pound;2,517</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Exceptional adjustment</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Total free meals funding</td><td>&pound;50,330</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Total student support funding</td><td>&pound;221,954</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Teachers' pension scheme grant</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 20%;"" scope=""col""></th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Payments made to Capita<br>for TPS, financial year<br>2019-2020 rebased at<br>16.4% for the full year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Annual payments<br>increased to 23.6%<br>equivalent for the full<br>year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">3.1% uplift for 2020-21</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">3% uplift for 2021-22</th><th style=""vertical-align: top; width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Revised annual cost</td><td>&pound;805,761</td><td>&pound;1,159,509</td><td>&pound;35,945</td><td>&pound;35,864</td><td>&pound;1,231,318</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Teachers'&nbsp;pension&nbsp;scheme&nbsp;grant&nbsp;(difference&nbsp;between&nbsp;2019/2020&nbsp;payments&nbsp;at&nbsp;16.4%&nbsp;and revised&nbsp;annual&nbsp;cost)</td><td style=""font-weight: bold"">&pound;283,705</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Start-up and post-opening grant</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part A</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part B</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - per pupil resources</td><td>-&pound;997</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - leadership disecononomies</td><td>-&pound;997</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td style=""font-weight: bold"">Total start-up and post-opening grant</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Maths top up</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Maths top up funding</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style='font-size: 10px; margin-top: 5px'>The values on your statement are shown rounded to various numbers of decimal places.<br> The calculation of your funding however is done using un-rounded values. This may result in some slight differences when you work through the calculation yourselves.</div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_NMSS =
         @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>Non Maintained Special School revenue funding allocation statement: 2021 to 2022</h2>
<table class=""styleTable bold left"" style=""width: 100%; text-align: center !important; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""width: 35%;"">Name</td><td>Birtenshaw</td></tr>
<tr>
<td>UKPRN</td><td>10015031</td></tr>
<tr>
<td>Local Authority</td><td>Bolton</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%; border: 1px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"" class=""greyBold"">Summary of 2021 to 2022 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 35%;"">High needs place funding</td><td class=""right"">&pound;600,000</td></tr>
<tr>
<td>Student financial support</td><td class=""right"">&pound;1,425</td></tr>
<tr class=""greyBold"">
<td>Total funding allocation</td><td class=""right"">&pound;601,425</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%; border: 1px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"" class=""greyBold"">Funded places</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 85%;"">Total pupil numbers recorded in the October 2019 Census</td><td style=""width: 15%;"" class=""right"">48</td></tr>
<tr>
<td>Total pupil numbers recorded in the January 2020 Census</td><td class=""right"">52</td></tr>
<tr>
<td>Difference between October 2019 and January 2020</td><td class=""right"">4</td></tr>
<tr>
<td>Total pupil numbers recorded in the October 2020 Census</td><td class=""right"">56</td></tr>
<tr class=""greyBold"">
<td>Total places funded for 2021 to 2022</td><td class=""right noBold"">60</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%; border: 1px solid #000;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"" class=""greyBold"">High needs place funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 25%;""></td><td style=""width: 15%;"">Pupil numbers <br> (October 2020 census)</td><td style=""width: 15%;"">Proportions used in 2021/22 allocation</td><td style=""width: 15%;"">Funded places</td><td style=""width: 15%;"">Rate</td><td style=""width: 15%;"">Place funding</td></tr>
<tr>
<td>Pre-16 pupil numbers</td><td class=""right"">39</td><td class=""right"">69.64%</td><td class=""right"">42</td><td class=""right"">&pound;10,000</td><td class=""right"">&pound;420,000</td></tr>
<tr>
<td>Post-16 pupil numbers</td><td class=""right"">17</td><td class=""right"">30.36%</td><td class=""right"">18</td><td class=""right"">&pound;10,000</td><td class=""right"">&pound;180,000</td></tr>
<tr class=""greyBold"">
<td>Total high needs place funding</td><td class=""right"">56</td><td class=""right"">100%</td><td class=""right"">60</td><td class=""right""></td><td class=""right"">&pound;600,000</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%; border: 1px solid #000;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"" class=""greyBold"">Student financial support funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 25%;""></td><td style=""width: 15%;"">Post-16 funded place number</td><td style=""width: 15%;"">Instances per student</td><td style=""width: 15%;"">Number of instances</td><td style=""width: 15%;"">Rate</td><td style=""width: 15%;"">Funding</td></tr>
<tr>
<td>Element&nbsp;1:&nbsp;Financial&nbsp;disadvantage</td><td class=""right"">18</td><td class=""right"">0.280</td><td class=""right"">5.04</td><td class=""right"">&pound;242</td><td class=""right"">&pound;1,220</td></tr>
<tr>
<td>Element&nbsp;2a:&nbsp;Student&nbsp;costs&nbsp;-&nbsp;travel</td><td class=""right"">18</td><td class=""right"">0.040</td><td class=""right"">0.72</td><td class=""right"">&pound;483</td><td class=""right"">&pound;350</td></tr>
<tr>
<td colspan=""3"">Element 2b: Student costs - Industry placement</td><td class=""right"">0</td><td class=""right"">&pound;49</td><td class=""right"">&pound;0</td></tr>
<tr class=""greyBold"">
<td colspan=""5"">Discretionary bursary fund</td><td class=""right"">&pound;1,570</td></tr>
<tr>
<td colspan=""5"">2019 to 2020 discretionary bursary fund - baseline for transition</td><td class=""right"">&pound;1,140</td></tr>
<tr>
<td colspan=""5"">Transition lower limit</td><td class=""right"">&pound;855</td></tr>
<tr>
<td colspan=""5"">Transition upper limit</td><td class=""right"">&pound;1,425</td></tr>
<tr>
<td colspan=""5"">Exceptional adjustment</td><td class=""right"">&pound;0</td></tr>
<tr class=""greyBold"">
<td colspan=""5"">Student financial support funding total</td><td class=""right"">&pound;1,425</td></tr></tbody></table><div style=""font-size: 8pt"">The values on your statement are shown rounded to various numbers of decimal places.<br />The calculation of your funding however is done using un-rounded values. This may result in some slight differences when you work through the calculation yourselves.<br/>For an explanation of the contents of each box on this statement please refer to the <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>explanatory note</a>.</div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, greyBold td { text-align: left; font-weight: bold; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_LA_StudentNumber =
          @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 120px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /></div><h2>Local Authority summary for student numbers:<br>2021 to 2022</h2>
<table id=""topTable"" class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr class=""bold"">
<td style=""width: 25%;"">Local Authority</td><td>Hertfordshire</td></tr>
<tr class=""bold"">
<td>Local&nbsp;Authority&nbsp;Code</td><td>919</td></tr></tbody></table><div style=""clear: both""></div>
<table id=""mainTable"" class=""styleTable"" style=""width: 100%; margin-top: 20px"">


<thead>
    <tr class=""greyBold"">
<th style=""width: 30%; vertical-align: top;"" scope=""col"">Institution Name</th><th style=""width: 30%; vertical-align: top;"" scope=""col"">Institution Type</th><th style=""width: 20%; vertical-align: top;"" scope=""col"">UKPRN</th><th style=""width: 10%; vertical-align: top;"" scope=""col"">2021/22 Funded Student Numbers</th><th style=""width: 10%; vertical-align: top;"" scope=""col"">2021/22 High Needs Places&#178;</th>    </tr>
    <tr class=""noStyle"">
<th scope=""col"" class=""bold"">Total Student Numbers&#185;</th><th scope=""col"" class=""darkGrey""></th><th style=""border-left: 1px solid #000 !important"" scope=""col"" class=""darkGrey""></th><th scope=""col"" class=""bold center"">28,724</th><th scope=""col"" class=""bold center"">10,750</th>    </tr>
</thead>
<tbody>

<tr>
<td>Aylward Academy</td><td>Academy</td><td>10030654</td><td class=""center"">245</td><td class=""center"">0</td></tr>
<tr>
<td>Aylward Academy</td><td>Academy</td><td>10030654</td><td class=""center"">245</td><td class=""center"">0</td></tr>
<tr>
<td>Barton Peveril College</td><td>Academy</td><td>10000552</td><td class=""center"">3,577</td><td class=""center"">20</td></tr>
<tr>
<td>Barton Peveril College</td><td>Academy</td><td>10000552</td><td class=""center"">3,577</td><td class=""center"">20</td></tr>
<tr>
<td>Brentside High School</td><td>Academy</td><td>10000866</td><td class=""center"">222</td><td class=""center"">0</td></tr>
<tr>
<td>Brentside High School</td><td>Academy</td><td>10000866</td><td class=""center"">222</td><td class=""center"">0</td></tr>
<tr>
<td>St Paul's Catholic College</td><td>Academy</td><td>10006247</td><td class=""center"">213</td><td class=""center"">0</td></tr>
<tr>
<td>The Stourport High School and Sixth Form College</td><td>Academy</td><td>10034690</td><td class=""center"">123</td><td class=""center"">0</td></tr>
<tr>
<td>The Stourport High School and Sixth Form College</td><td>Academy</td><td>10034690</td><td class=""center"">123</td><td class=""center"">0</td></tr>
<tr>
<td>Truro and Penwith College</td><td>Academy</td><td>10007063</td><td class=""center"">4,946</td><td class=""center"">217</td></tr>
<tr>
<td>Truro and Penwith College</td><td>Academy</td><td>10007063</td><td class=""center"">4,946</td><td class=""center"">217</td></tr>
<tr>
<td>Abbey Hill Academy</td><td>Academy Special</td><td>10004756</td><td class=""center"">93</td><td class=""center"">96</td></tr>
<tr>
<td>Abbey Hill Academy</td><td>Academy Special</td><td>10004756</td><td class=""center"">93</td><td class=""center"">96</td></tr>
<tr>
<td>Aylward Academy</td><td>Non maintained special school</td><td>10030654</td><td class=""center"">0</td><td class=""center"">18</td></tr>
<tr>
<td>Barton Peveril College</td><td>Non maintained special school</td><td>10000552</td><td class=""center"">0</td><td class=""center"">18</td></tr>
<tr>
<td>Birtenshaw</td><td>Non maintained special school</td><td>10007063</td><td class=""center"">0</td><td class=""center"">18</td></tr>
<tr>
<td>Derwen College</td><td>Non maintained special school</td><td>10001929</td><td class=""center"">0</td><td class=""center"">18</td></tr>
<tr>
<td>The Stourport High School and Sixth Form College</td><td>Non maintained special school</td><td>10034690</td><td class=""center"">0</td><td class=""center"">18</td></tr>
<tr>
<td>Derwen College</td><td>Special Post-16 Institution</td><td>10001929</td><td class=""center"">93</td><td class=""center"">93</td></tr>
<tr>
<td>Derwen College</td><td>Special Post-16 Institution</td><td>10001929</td><td class=""center"">93</td><td class=""center"">93</td></tr></tbody></table><div style='font-size: 10px; margin-top: 5px'><p>&#185; For some institutions the student numbers are calculated using unrounded numbers. This may result in a rounding difference of 1 between the total student numbers shown and the sum of student numbers.</p><p>&#178; For most institutions, the '2021/22 funded student numbers' total also includes '2021/22 high needs places'. This is not the case for academy special converters, free schools specials and non-maintained special schools.</p></div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .darkGrey, .darkGrey * { background-color: #808080 !important; border: 0 !important } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 1px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_LA_SixthForm =
        @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 100px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2 style=""width: 75%; font-size: 1.6em;"">Local Authority summary of school sixth form<br>revenue funding allocations: 2021 to 2022</h2>
<table id=""topTable"" class=""bold left"" style=""width: 73%; text-align: center !important;"">


<tbody>

<tr>
<td style=""width: 20%;"">Local Authority</td><td>Hertfordshire</td></tr>
<tr>
<td>Local Authority Code</td><td>919</td></tr></tbody></table><div style=""clear: both""></div>
<table id=""2ndTable"" class=""styleTable"" style=""width: 100%; text-align: center !important; border: 0 !important;"">


<tbody>

<tr class=""bold"">
<th style=""width: 60%; border: 0;"" scope=""row""></th><td rowspan=""2"" style=""text-align: right; vertical-align: middle;"" class=""greyBold""><br>Total<br>Academic<br>Year Funding</td><td colspan=""2"" class=""greyBold"">Financial Year (FY) split</td></tr>
<tr class=""bold"">
<th style=""border: 0;"" scope=""row""></th><td style=""text-align: right"" class=""greyBold"">8&nbsp;Months<br>in&nbsp;2021-22&nbsp;FY</td><td style=""text-align: right"" class=""greyBold"">4&nbsp;Months<br>in&nbsp;2022-23&nbsp;FY</td></tr>
<tr>
<th scope=""row"" class=""left bold"">Programme and Student Financial Support Funding - Mainstream School Sixth Forms</th><td style=""text-align: right"">&pound;1,080,095</td><td style=""text-align: right"">&pound;1,111</td><td style=""text-align: right"">&pound;1,113</td></tr>
<tr>
<th scope=""row"" class=""left bold"">Student Financial Support Funding - Maintained Special Schools</th><td style=""text-align: right"">&pound;10,785</td><td style=""text-align: right"">&pound;400</td><td style=""text-align: right"">&pound;500</td></tr>
<tr class=""greyBold"">
<td class=""bold"">Total LA Funding</td><td style=""text-align: right"">&pound;1,090,880</td><td style=""text-align: right"">&pound;1,511</td><td style=""text-align: right"">&pound;1,613</td></tr></tbody></table><div class=""smallBr""></div>
<table id=""3rdTable"" class=""styleTable table3"" style=""width: 100%; text-align: center !important;"">


<thead>
    <tr class=""greyBold"">
<th colspan=""13"" style=""font-size: 1.3em"" scope=""col"">School Sixth Form Institutions</th>    </tr>
    <tr class=""greyBold"">
<th colspan=""3"" scope=""col"">School Sixth Form details</th><th colspan=""6"" scope=""col"">Academic Year (AY) Allocation Information (August 2021 to July 2022)</th><th colspan=""2"" scope=""col"">FY&nbsp;split&nbsp;of&nbsp;total&nbsp;AY&nbsp;allocation</th>    </tr>
    <tr class=""whiteBackgroundBold topAlign"">
<th scope=""col"">UKPRN</th><th style=""width: 15%"" scope=""col"">Institution&nbsp;Name</th><th scope=""col"">LAEstab<br>Number</th><th scope=""col"">Programme<br>Funding</th><th scope=""col"">Student<br>Financial<br>Support<br>Funding</th><th scope=""col"">Industry<br>Placements<br>Funding</th><th scope=""col"">Advanced<br>Maths&nbsp;Premium<br>Funding</th><th scope=""col"">High&nbsp;Value<br>Courses<br>Premium</th><th scope=""col"">Total<br>AY Funding</th><th scope=""col"">8 Months<br>in 2021-22 FY<br><span style='font-size: 0.7em'>(August 21-March 22)</span></th><th scope=""col"">4 Months<br>in 2022-23 FY<br><span style='font-size: 0.7em'>(April 22-July 22)</span></th>    </tr>
</thead>
<tbody>

<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10004756</div></td><td class=""left""><div style=""page-break-inside: avoid;"">Abbey Hill Academy</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;508,152</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;7,724</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;967,724</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr>
<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10004756</div></td><td class=""left""><div style=""page-break-inside: avoid;"">Abbey Hill Academy</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;508,152</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;7,724</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;967,724</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr>
<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10030654</div></td><td class=""left""><div style=""page-break-inside: avoid;"">Aylward Academy</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;1,375,699</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;41,327</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4,200</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;9,200</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;1,430,427</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr>
<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10030654</div></td><td class=""left""><div style=""page-break-inside: avoid;"">Aylward Academy</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;1,375,699</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;41,327</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4,200</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;9,200</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;1,430,427</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr>
<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10000552</div></td><td class=""left""><div style=""page-break-inside: avoid;"">Barton Peveril College</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;15,562,634</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;221,954</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;70,250</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;115,800</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;321,600</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;16,695,943</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr>
<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10000552</div></td><td class=""left""><div style=""page-break-inside: avoid;"">Barton Peveril College</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;15,562,634</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;221,954</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;70,250</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;115,800</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;321,600</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;16,695,943</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr>
<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10000866</div></td><td class=""left""><div style=""page-break-inside: avoid;"">Brentside High School</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;1,153,932</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;26,242</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;24,800</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;1,204,975</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr>
<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10000866</div></td><td class=""left""><div style=""page-break-inside: avoid;"">Brentside High School</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;1,153,932</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;26,242</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;24,800</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;1,204,975</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr>
<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10001929</div></td><td class=""left""><div style=""page-break-inside: avoid;"">Derwen College</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;508,152</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;13,943</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;1,080,095</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr></tbody></table><div style=""page-break-before: always""></div>
<table id=""3rdTable"" class=""styleTable table3"" style=""width: 100%; text-align: center !important;"">


<thead>
    <tr class=""greyBold"">
<th colspan=""13"" style=""font-size: 1.3em"" scope=""col"">School Sixth Form Institutions</th>    </tr>
    <tr class=""greyBold"">
<th colspan=""3"" scope=""col"">School Sixth Form details</th><th colspan=""6"" scope=""col"">Academic Year (AY) Allocation Information (August 2021 to July 2022)</th><th colspan=""2"" scope=""col"">FY&nbsp;split&nbsp;of&nbsp;total&nbsp;AY&nbsp;allocation</th>    </tr>
    <tr class=""whiteBackgroundBold topAlign"">
<th scope=""col"">UKPRN</th><th style=""width: 15%"" scope=""col"">Institution&nbsp;Name</th><th scope=""col"">LAEstab<br>Number</th><th scope=""col"">Programme<br>Funding</th><th scope=""col"">Student<br>Financial<br>Support<br>Funding</th><th scope=""col"">Industry<br>Placements<br>Funding</th><th scope=""col"">Advanced<br>Maths&nbsp;Premium<br>Funding</th><th scope=""col"">High&nbsp;Value<br>Courses<br>Premium</th><th scope=""col"">Total<br>AY Funding</th><th scope=""col"">8 Months<br>in 2021-22 FY<br><span style='font-size: 0.7em'>(August 21-March 22)</span></th><th scope=""col"">4 Months<br>in 2022-23 FY<br><span style='font-size: 0.7em'>(April 22-July 22)</span></th>    </tr>
</thead>
<tbody>

<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10001929</div></td><td class=""left""><div style=""page-break-inside: avoid;"">Derwen College</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;508,152</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;13,943</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;1,080,095</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr>
<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10006247</div></td><td class=""left""><div style=""page-break-inside: avoid;"">St Paul's Catholic College</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;1,044,412</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;12,268</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;27,200</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;1,104,824</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr>
<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10034690</div></td><td class=""left""><div style=""page-break-inside: avoid;"">The Stourport High School and Sixth Form College</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;525,174</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;8,063</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;12,800</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;601,016</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr>
<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10034690</div></td><td class=""left""><div style=""page-break-inside: avoid;"">The Stourport High School and Sixth Form College</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;525,174</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;8,063</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;12,800</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;601,016</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr>
<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10007063</div></td><td class=""left""><div style=""page-break-inside: avoid;"">Truro and Penwith College</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;22,508,598</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;905,211</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;218,875</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;152,800</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;25,803,920</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr>
<tr>
<td class=""left""><div style=""page-break-inside: avoid;"">10007063</div></td><td class=""left""><div style=""page-break-inside: avoid;"">Truro and Penwith College</div></td><td style=""text-align: center""><div style=""page-break-inside: avoid;"">1234</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;22,508,598</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;905,211</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;218,875</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;0</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;152,800</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;25,803,920</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;2</div></td><td class=""right""><div style=""page-break-inside: avoid;"">&pound;4</div></td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total</td><td>&pound;508,152</td><td>&pound;13,943</td><td>&pound;0</td><td>&pound;0</td><td>&pound;0</td><td class=""right"">&pound;1,080,095</td><td class=""right"">&pound;1,111</td><td class=""right"">&pound;1,113</td></tr></tbody></table><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 12px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .topAlign { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .darkGrey, .darkGrey * { background-color: #666 !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 1px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .whiteBackgroundBold td, .whiteBackgroundBold th { background-color: #FFF !important; font-weight: bold !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; } </style></body></html>";

        private readonly string html_1619_SixthForm_Mss =
           @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 100px;'>   <img style='height: 75px' src='data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD//gA7Q1JFQVRPUjogZ2QtanBlZyB2MS4wICh1c2luZyBJSkcgSlBFRyB2NjIpLCBxdWFsaXR5ID0gODIK/9sAQwAGBAQFBAQGBQUFBgYGBwkOCQkICAkSDQ0KDhUSFhYVEhQUFxohHBcYHxkUFB0nHR8iIyUlJRYcKSwoJCshJCUk/9sAQwEGBgYJCAkRCQkRJBgUGCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQk/8AAEQgAkQEsAwEiAAIRAQMRAf/EAB8AAAEFAQEBAQEBAAAAAAAAAAABAgMEBQYHCAkKC//EALUQAAIBAwMCBAMFBQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYnKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX29/j5+v/EAB8BAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKC//EALURAAIBAgQEAwQHBQQEAAECdwABAgMRBAUhMQYSQVEHYXETIjKBCBRCkaGxwQkjM1LwFWJy0QoWJDThJfEXGBkaJicoKSo1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoKDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uLj5OXm5+jp6vLz9PX29/j5+v/aAAwDAQACEQMRAD8A9B/Zcdm8L68WYn/ibP1P/TNK9orxX9lv/kV9e/7Cz/8AotK9qrfE/wARmGG/hRAnA9a8Wu/2gLXSPEj2mqXUEFn9pjR90LL9jXGHWRzglxjdgL/EBzg49nflSMke461+f91f/aPHGsW3iS7MOoYvTbarfXLq8cm1lCS4U7sFGQBQnzEnkYFYG591Q+K9LvNBttcsJzfWd2F+ymAZa4LHChQcck+uAOSSACa4D4s6hd3Y8PCXRdWgMOoxzDZImGYdFBWUZOf4Rlj/AAg848o/Zc8RavrngfXvD9tdRJc6Iwv9OeRVwjENlWJP3TyOnGTz0FLqXjrx9qUqXs+ialHbz6rDqtst79ntysYQAbFllJ7DAGAeTkE4oA+lYvELfabeK80u9sI7k7IpZzGVL84Q7WJUnHGeM8ZyQDR8Vave+FWXXQkt3pEa7b+BF3SW6Z/16AcsF/jX+7yOVIbzn4Uav4v8UavLpniO2v7G2tbiTVW+2QqDcb5cwrG+9gUUhiduAvyryOa0/wBoOTxavh+0Tw9rVnpVhJKI9RllwGWMsBksSMRgbt20ZPA6ZoA9I0LxHpHiazN7o2o22oWyuYzLbuHUMOoyPqPzFaNfnl4R8UXHwu+JNjJ4c1mS8SK6EF23kNHDOpfay+W2Djb6gEHpjGT+hgPHagBaKPyooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKPyoA8V/Zb/5FfXv+ws//otK9okkWGN5JGCogLMx4AA714v+y3/yK+vf9hZ//RaV7SwDKQwyCMEHvW+J/iMwwv8ACieBal4h+IXiLxhqV9Dd3uneHZozbeHbVB5Mt5d4ASTbgM0Y+Z2LjZt7HrXg37UFxov/AAtK9stGRAbcBr10OQ1y/wA0gHsCeR/eLfQfRHjD4l2vhnUPF8wt7xvE8EciWZmtXEVnYpGpMqsw27S+49cu+1egBHyV4ZtdU/eeNEsrTWTBqUNq9nfwfaReSzLI2Cp++fkOcc5YEVgbmx8O/E2r/DjQ9R8RaDqKte6mq6VFbxIHKO2WLSKQcEBRs/vFup2stdFpvwm0q6e8h8Tave3WtwwCWcW93EEhIKgxNkO5Kg4J2gAjABxmu58VeFNI8G/E/wAMavd2mleFtM1m3tpH04rI5W6WSN8FR8mEk2Z5X5Q3TIq5YkW0t8qyX8NyttNLKt4wIGydd5A37Qd0mAx6cggc0AcN4Xn1H4Z67Da2OtahfeDde8y2llt3aC5j8olmWNhysgOdu3AfJGA2QuX8S/iL4wsriyiuhdi4t5P3Gq3qoZby3ADRK8YynAbcT824kEngVt/E/WBJ4AbU7cxWk7azbyw+U0bM84jkdnBQ8FNwBJHJYd812/xA0bwf8N/hhp0/inQLrxHProt3hsGcQ/YZ/Ky4jlVd6oC2AnzY4HSgDlvhHolr43tj4/uNHsbaTw/dRIAkhCXNyTGI2fqyxrkOxJJOX5PG36C8A/EXWtX8b+IPBviPTIbS70vD21zCjRpexZ++EYtgcrj5jnJ9CK+Q/hFql7oXjibTyt/a6TPMDfaM7sZrmEE5Tbsw7IjMxBC7lDDqQK+zPBVxpt9qV3p1vfWut2ulLBcafel1mkgSZXHlmTkkqF+91KuucnJIB29FFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQB4r+y3/yK+vf9hZ//RaV7VXiv7Lf/Ir69/2Fn/8ARaV7VW+J/iMwwv8ACieEfGHV7/xt8Q7L4PxXEekaXrFqJrzUDBulmKbpPKjJIHIRc8d/wOhr37OOkQ+D9J03wXePpGraJcNfWd853NNcFQMysB3Kp0HAXGK3vjJ8KtN8f2VrqUsOqNqOlbpLf+y50huH4+6rv8o557ZIHNfPlncfEvxPat8Ovh54Y1jwvoiSsLu4vncTEk4YyzEAKMfwIMnH8WTWBudR450rX/i6bjwf4q8QeBE13TF36V/Z12TcTzjIdGQk4DhRlcAg7SM4IrDvvEt94SvLhfE/h3U453C6db6jeBoJZrYbGeSTAVCWMYw4Yv2IPBrm/iX8Dbn4Tah4QSw1iMX99Kzy6xdTCC3t50KlQM/dA65OSccAdK9/8e3cPiSHQJl1Wy1dIwkfmaVdKRHdMMNKSG4TphiSq87lbIwAeJ6Z4X1DxrcJ438TSQR+CNCZpbK1muXjF45fIj8y52ltzY3uSeBhR0A9tsdN1z446PY3Oraz4ZtNHguIbxIdEIu7mOZMMFaZiUQg8HapyO+DivNf2rr+61vSPDVra+IdC1CO2I+16ba3amaW5ICh1QHJX7wGMEbj+FfVvgN8QvhHJbeLvhrf3cz+Sj3emBg80Zxlkx92dAcjpu9AetAHVfFXTG8B+Nbrx9bXEVlc2jNPb6fPC09tqHmosU8nyn92+wDJ43bcY6lvTvhwtpaSS2/h7QYtO8Pyb3ikELIzEFQDvLHcCTIAoA2hB2Irzzwlr/iD4/HTLLxV4U1XSLHSp/OvwFCWd84HCMsg38EA7Rux1JBwR9AAADAGBQAtFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQB4r+y3/AMivr3/YWf8A9FpXtVeK/st/8ivr3/YWf/0Wle1Vvif4jMML/CiFFJuXPUUbl/vD86wNzlPiT8NNC+Kfh8aLrqzCNJBNDNA+2SGQAjcCQR0JGCCK8Nuf2HtMaQm28a3kceeFksVc/mHH8q+nQwPQiloA8G8DfsheFPCet2msX+rX+sT2cqzRROixRb1OVLKMk4IBxnHrmveaje4hidY3ljR3+6rMAW+gqSgAooooAKKKKACiiigAoopks8UCb5ZEjXOMu2B+tAD6KQMGUMpBB5BHeloAKKKKACiiigAooqKS6gidUkniR26KzAE0AS0UZzRQAUUVSvtc0rS2Vb/UrK0ZugnnVCfzNAF2iorW8tr2FZrW4iuIm6PE4ZT+IpZLiGJlSSWNGc4UMwBb6etAElFFMlljhQvI6og6sxwBQA+ionu7eONZXniWNvuuXAB+hpZLmGJkWSaNC/ChmA3fT1oAkooooAKKKKAPFf2W/wDkV9e/7Cz/APotK9qrxX9lv/kV9e/7Cz/+i0r2qt8T/EZhhf4UT5K8Y+C7L4jftYar4a1W8v7eyltY5SbSUI4K2sZGCQR19q7/AP4Y88C/9BvxX/4GR/8AxqvPvGNz4ttP2tNVl8EWNje62LWMRw3rbYin2SPcSdy84969B/4SD9pn/oUvB/8A3+/+31gbnefDD4QaH8J01FNFvdVuhqBjMv26ZZNuzdjbtVcfeOfwqfWvjJ8PvDuoNp2p+LdKt7tDteLzd5jPo23O0+xxXmnxL+IPxF8KfA/VL/xZa2GkeIry9XT7Y6c+VSJ1BLg7mw2FkHXjg1pfCT9njwTp/gfTbnXtDtNX1XULZLm5nu1L7WdQ2xQeABnGRyetAHNfGTVtP1z40/CK/wBLvba+tJrsFJ7eQOjjzo+hHFfQL+INHj1ZNGfVbBdTkTelkZ0E7LgncEzuIwDzjsa+U/Gfwxsfhp+0L4Di0Uyx6NqOoxXEFqzllt5BKokVc9j8h9e3YV22uf8AJ5Wgf9ghv/RU9AH0DdXVvY20t1dTxQW8KGSSWVgqRqBksSeAAO9Q6Zq2n61Zpe6XfWt/avkJPbSrJG2Dg4ZSQcEEVz/xX/5Jf4t/7A93/wCiWr50tfHF94I/Y/0p9Mne3vdTvJ7BJkOGjVppWcg9jtQjPbNAH0FrXxm+Hnh6/fT9T8XaVBdRna8Ql3lD6Ntzg+xrpNE1/SfEtguoaLqVpqNo5wJraUSLn0yO/tXlfw0/Zz8DaL4PsF1vQbTVtVuYElu7i7XefMYAlVB4UDOBjnjJqT4e/BG/+GPxI1TVvD2pwReE9QiIbSnd2eN8AggkYOGyASc7WxzQB3fij4k+D/BUqw+IfEWnadMw3CGWUeYR67Blse+KPC/xK8HeNZXh8PeItP1GZBuaGKX94B67Dg498V45ovwn8FeEdZ1nX/jFr3h3VdZ1G4M0X2y5wkcZ7CNyMnt0IAAxivP/ABHqPw7tfjv4FuPhfJDDuvoor82SssB3SKuFzxyrMDt4xjvmgD0z43/Fj+x/HPgKy8PeL7SG1fVGh1mO2u42CIJYQRNydgAMnXHf0rrvi1Y+Dfih8PXtbvxppmn6R9sjLalDcxSRLIuSE3btuTnpnNeRftA/DDwho/xG8Aix0dYR4j1iQap+/lP2ndNDnOWO3PmP93HX6V0P7SHgnw/4B+BM+leG9OGn2TarDOYhK8mXIIJy5J6KO/agD3jRY7TSvDtjFHdxy2lraRqtyWAV41QAPnpggZrlpPjr8M4r02TeNNH84Nt4mymf98fL+teNfGnV9V8QWXw1+F+l3b2kevWtq946n7yEKig+qjDsR3wK9Ttf2cPhjb6Aujt4Ytph5exruRm+0scfe8zOQe/GB7YoA9Htry2vbWO7tZ4p7eRQ6SxMGR19QRwRXPyfEzwVFo8utN4p0f8As6KUwPci6QoJAAdmQeWwQcDmvE/gFc6j8Pvih4u+E9zeS3Wm2sbXliZDkoPlPHpuSRSR0yvvXK/sv/CnRfHkOsav4mhOpWGnXzRWlhK58lZWUGSRlB5JAjHPHHOeMAH0p4X+J/gvxpcta+H/ABHp+oXKjcYI5MSY7kKcEj3ArqK+W/jv4G0H4YeNfAXiXwhYR6PdTamIZY7XKxuAyY+XoMhmBx1Br6koAp61cz2Oj311axedcQW8kkcf99gpIH4kV8nfCP4SaH8b/DGs+MvGPiXVJtaa6ljeRJ1UWgChgzBgeOc44AAwOlfTHxB8faN8N/DVxr+tyMIIyEjijGZJ5DnCKPU4P0AJ7V8b+JvDvi+ysr/x3/YupeGfA/iS7Q3unWFx+8+zswIZlI4ViW25AGWxgAjIB75+yX4k1rXvAuo2uq3k2oQaZfta2d3KSxePaDtyeSBnj0DAdq9xryrw948+HXw80fwV4d8PLM9j4iPl6abZA+9iyhnlJIIO5+e4IIxxivVaAPNP2hvH+ofDr4aXmqaS3l6hcSpZwTYz5LPkl8eoVWx74rg/h9+zF4V8R+GNP8Q+MrvU9d1fVreO8mme7ZVXzFDAAjk4BGSSc+1ewfEfwDpvxL8JXfhzU3eKOfDxzxjLQyKcq4Hf3HcEivCdO8EftC/CS3Fh4Y1HT/E2jQf6m2kKkovoFk2sv+6rkelAFLxd4E1f9m7xfoviLwJc6te6BfXHk32mNulAAwSDtHIK7tpIypHU5rpP2jG3fFX4PMO+rZ/8j21M0T9qTVdA1m30b4o+Dbrw9LMdovIkcRjnG4o3JX1Ks30qD9pnVbKy+Ifwl1a4uY0sYdQa5kuM5RYhNbMXyO2OaAPoi/v7TS7Oa+v7mG1tYEMks0zhEjUdSSeAK4H4gz+Dfin8M9Ysv+Ev0620aV4o7jVIZkeKBllRwCxO3JIUdf4hXjcnjF/2pviGfCUWqto3g+wU3TWwO241IKQM+ncHB+6OcE9PRv2hNB0zwz+zvrmk6PZxWVjbJapFDEMBR9pi/M9yTyaAOF/aZ02y0b9n/wAHabpt+mo2Vpd2kMF2hBFxGttKFcYJGCADxWp+0N/yUP4N/wDYTH/o22rl/jj/AMmt/Dr62H/pJJXUftD/APJRPg3/ANhMf+jbagD3TxF4r0HwjZC91/V7LTLcnCvcyhN59FB5Y+wrF8P/ABf8A+Kb5LDR/Fel3V25wkHm7Hc+ihsFj9K8C+O4sdM+Pmk6r8Q9Ovb7wYbVUgCBjEG2nIIBGcPyy9SMdRxWz4g8AfBn4s6fbRfD3XNB0HXo5UeCS3JikYZ5UwkqSe4IGQR1oA+lKKqaRBd2ulWcF9cLc3cUCJPOq7RLIFAZgO2Tk496t0AeK/st/wDIr69/2Fn/APRaV7VXiv7Lf/Ir69/2Fn/9FpXtVb4n+IzDC/wony3e6/pXhr9snUtR1rUbXTrNLNVae5kCICbSMAZPrXuH/C6Phv8A9Dx4e/8AA6P/ABp3iH4O+AvFmrTaxrnhmyvr+faJJ5C25tqhR0PYAD8Kzf8Ahnr4Wf8AQl6d+b//ABVYG5ynx3bSPjF8KNWg8HarZa5eaPLFftFYzLK2BuBGF7lS5A77avfCD48+Dde8D6ZHquvadpOp2NslvdW97OsJ3IoXcu4gMDjPHTOK7/wl8PPC3gT7V/wjWjW+mC72+eIS37zbnbnJPTcfzrB174B/DTxJqUmpaj4Ts3upTud4XkhDnuSI2AJ98UAeEePPibpfxF/aJ8Bx6FKbnS9K1CGBLoAhJpWlUuVz1UAIM9/piuj+Jes2fgX9qjwx4j12Q2ukzaaYftTA7EJEqc/QsufQHNez2/wm8D2culS23hqwgk0d/MsWjQr5D5BLDB5OQDk5PFaHi3wP4c8dWC2HiTSLbUoEbcglHzRn1VhhlP0IoA80+Nnxp8HwfD3WNL0jWrHWtU1WzltILawmE5AdSGdtmdoVSTz6V5PL4QvfFf7HmjS6fC88+k309+0aDLNGJplfA9g+76Ka+hvD/wAD/h34XFz/AGV4Xs4WuoXt5XdnlcxupVlDOxKggkHGK6fw94a0nwppMWj6JYxWOnwljHbx52ruJY9c9SSaAPPvhv8AHvwR4i8G2F3qHiLTNMv4bdEu7a8uFhdJFUBiAxG5TjIIz19eK5zw98ZvEvxD8e+JovCCQ3HhTR9OleKc2533FyIzsCsfV8kDHIX3rtNY/Z8+GGuX73974RsvPkO5jA8kKsfXajBf0rr/AA94Y0XwnpqaZoWmWunWaHIigQKCfU+p9zzQB8s/s7/8Kv1rTtV1j4h3uk3fiiS8dpTrsy/6sgEMokO1iTuyeSPYVX+KHjzwNqXxc8Aw+FRp9to2i6jG1ze20Sw2pZpYy2CAAQqqCW6c19A658Afhp4i1OXU9R8KWj3crb5HikkhDseSSqMASfXHNXNU+C/w/wBZ0O00O78L2H9nWbmWCGENFsYjBO5CGJOBnJ5wM9KAPJ/2m9XsLbxb8Jtbe6iOmRai1010h3R+V5ls28EdRtGeOoq5+0/4m0XxX8D5b/QtTtNStBqcEZmtpA6hhklcjvgj869g1f4f+Ftf0Gz0DVdEs7zTLJEjtoJl3CEKu1dp6jAGM5qkvwl8Dp4bfwyvh20GjPcfams8tsMuAN3XOcAd6APCfjRYaj4X/wCFX/E+ztJLq10a1tIbxUH3VAVlz6Bsuuexx617LbfHn4bXOgjWv+Et0yKDZvMMkoWdTj7vlffz7AV2n9k2J0waW1pDJYiIQfZ5FDIYwMBSD1GOOa4CT9nH4VS3pvG8H2nmFt21ZpRHn/cD7ce2MUAeafAgXnxF+L/jH4pm1lt9JliayszIMGT7gH4hIxn0LCtL9jP/AJEvxH/2GX/9FpXvOn6XZaTYRWGn2kFpaQrsjggQIiD0AHArO8LeC/D/AIKtZ7Tw9pcOnQXEpmlSLOHfAG45J5wBQB4r+1r/AMfPw+/7DH9Y6+haxPEngrw/4vaybXdLgvzYS+dbebn90/HzDB9h+VbdAHzx+2Ppt6/hzw3rKW73OnabqBa8iA4wwG0t6D5Suf8AaHrXfD4yfCvxN4RknvfEejf2bc25S4srqVVlCkco0R+YntgA+1eh3llbajay2l5bxXNvMpSSKVAyOp6gg8EV52f2cPhU179rPg+08zdu2iaUR5/3N+3HtjFAHyr4A1fQPCPxV0vxLcW2sv4Eg1G4h0u6uVOyFiOGPY7dysQOeAeoxX3hbzw3UEc8EqSxSKHSRGyrKRkEEdRisbVfAnhnWvD6eHb7Q7GXSI9pjsxEFjj29NoXG3Ht6n1q9oeh6f4b0uDStKtha2NuCsUIYsEGc4GSTjnpQB5b+03N4x0zwRba74P1G+tJNMuRLeraOVLwEYJOOoU4z7EntXQeAvjf4K8c6JbXkWu6fZXhjBuLK7nWKWF8cjDEbhnuODXfsiyKUdQysMEEZBFeZ67+zb8L/EN495c+GYreaQ7nNnNJApP+6jBR+AoA87/aq8d+E/EfhK18KaPd2mua/cX0TQR2LCdocZB5XOCc7dvU59qwvjL4VKXXwL8K68nm8RafeoHPzfNao65H4jIr3jwb8FvAXgG5F5oPh63gvAMC5lZppV/3Wcnb+GK3Nd8FeH/EupaZqWr6XBeXmkyedZSyZzA+VbIwfVVPPpQB4h+0F8M5vCX9lfEvwFax2F94dCLcQW0eFa3XhW2jqFHysO6n2q78WfHunfEn9l7V/EOnMAJltVnhzkwSi5i3IfoenqCD3r3m4t4rqCS3njWWKVSjo4yrKRggjuK5Oz+EPgaw0O/0G18O2sWl6iyPdWgZ/LlZCCpI3dQQOnoKAPn344/8mt/Dr62H/pJJXUftD/8AJRPg3/2Ex/6Ntq9m1b4c+FNd8P2Ph3UtFtrrSbDZ9mtHLbItilVxg54UkVPrfgjw94jvdLvtW0uC7udJk82ykfOYGypyuD6ovX0oA858WfGnTdA+J03gTxzo9la+H7m3WW21G6/eRTEgcOpXAXdvXPYgZ615x8fNG+B6+DrvU/Dt1odv4hUqbNNFuFPmNuGQ0aEqBjJzgY9ex+jfFfgbw344s1s/EejWmpxISU85PmjJ67WHzL+BrmdG/Z9+GOgXyX1l4Rs/PjO5DO8k4U+oWRmH6UAX/g1caxdfC7w3PrzTNqL2SmRps72GTsLZ5yU25zzXZ0AYGBwKWgDxX9lv/kV9e/7Cz/8AotK9qrxX9lv/AJFfXv8AsLP/AOi0r2qt8T/EZhhf4UThPE/xx+HvgzWp9E17xHHZahbhTJA1tM5UMoYcqhHIIPWsr/hpn4S/9DfD/wCAdx/8bry+bR9N139szUrLVdPtNQtGslZoLqFZYyRaIQdrAivef+FXeA/+hK8M/wDgsg/+JrA3LPg7x14d+IGnSal4a1JdQtIpTA8qxumHABIw4B6MPzrerz/xV4z8G/BODSrT+xPsUGsXZhii0q0iRPN+UbnAKjoRzyeK7+gBaK4fwn8XdC8Zt4mXTbbUEbw1I0V2J41Xew3/AHMMc/6tuuO1cR/w1h4VudGtLrStF1vUtTu3dYtKt4lacKpxvfaSAD26njpQB7fRXkXw9/aP0Pxr4mHhfUNG1Pw7rMmfJt79QBKQM7c8ENgE4IGfWum+J3xf8M/CnT4rjXJpZLm4z9msrdQ002OpAJACj1JoA7Y8CuQ8PfFjwp4p0LWNd0q+lmsNF8z7bI0DoY9ib2wCMthR2rzvTP2q9JN1br4k8IeIvDljcsFiv7qEmHJ6EnA4+ma5b9mW7063+GPxEu9TgN3pkdzcS3MScmaEQZdRyOq5HUdaAPoLwd4y0bx5oMOu6DcPc6fOzokjRtGSVYqeGAPUGtuvIvDPxO8E+E/goPGfh/Qb6x8OW8rKliir5oYzbCQC5HLHP3qztU/am0RWhi8N+Gdd8Szm3jnuFsYcrbb1DbGYZ+YZwcDAORmgD26ivO/hR8bfD/xYS7gsIbrT9TsgDcWN2AHVc43KR1GeD0IPUcioPih8efDXwyvYdJmhu9W1qcBk06xUM4B6Fiemew5PtQB6XRXhtp+1doEEd2niPw1r3h68hgaeG3u4gDchRnahbb83pkAH1zxXq/gvxVa+N/C+neIrGGaG2v4vNjjmADqMkc4JHb1oA26yPFnivSPBOg3Ou65c/ZdPttvmSbSxGWCgADJJyR0rXr56/aRvJfGvjHwb8KrGRv8AiY3S3l/sP3IgSBn6KJWx7CgD2PwP4+8PfEXR31fw3em7tElaBmaNo2VwASCrAHoQfxroq+bvhIU+E/x+8T/Dxv3OlayPtumofuqQC6qv/AC6/WMV9B61rWn+HNKutW1W6itLG1QyTTSHARR/P6dSaAL1FeBt+1tplzNNPpPgfxPqekwMRJqEUI2gDqccgevJH4V13gz9oHwp498W23hvQ472aW4szeC4ZFVEA6owzuDDpjGPfHNAHp1Fea/Ez48eG/htfxaO8F5rGuTAMmnaegeQA9Nx7Z7Dk+2Kw/CP7Tug634ig8PeIND1bwrf3JCwf2im1HY8AEnBUk9MjHvQB7NRXD/EP4t6J8NtV0Cw1mG4xrczQx3CbRHb7SgLSFiMKN4PGeAa891L9rPS4Gmu9L8F+JNT0WFiraokOyIgHlhkEY+pB+lAHvVFc54D8e6J8RvDcOv6FMz2shKOkg2vC46o47EZH4EHvXm3iP8Aam8P2WuTaN4Y0HWPFlxbkrLJp0eYwRwdp5LD3xj0JoA9soryv4e/tDeHPHuoXGjHT9S0nXoY2caZeRhZJtoyVjOQC2OxwfwzXQ/DP4raB8VdPvbzQ472D7DP5E8F5GqSKcZBwGPB5HXqpoA7OiuQ+JXxP0L4WaRbanriXcqXNwLaGG0jDyO5BPAJHAA9e4rqrWc3NtFOYpITIgfy5MBkyM4OMjIoAlooooAKKKKAPFf2W/8AkV9e/wCws/8A6LSvaq8e/Zp02+0zw3rkd/ZXNo76o7qs8TRll8tOQCORXsNbYj+IzDDfwkfI/jHwpe+M/wBrTVdH0/xBfeH7iS1jcXtkSJFC2kZIGGU4PTrXoP8Awzh4r/6LX4w/7+Sf/HaybHTr0ftm396bS4FqbIAT+WfLJ+yIPvdOtfRtYm58y/tHaNceHtB+GGk3WpXGqT2mpCJ724JMk5Gz5myScn6mvpqvEP2q/COt6/4V0fWdBs5L650K+F09vGhZjGRywUcnBVcgdiT2rNt/2r01+xXTvDngjX7vxPMmxLQxqYUlPGS4OdoPPKjjrigDJ+AH/Hz8af8Ar9k/nc1pfsY6BYW3gDUNbWBDf3d+8LzEfMI0VNqg+mST+NZP7N2ja1pWn/FSDWreYXzPtkcocTSBbgMVOPmBbuPUV137IlldWHwpeG7tpreT+0pzslQo2Nqc4NAGH+0NBFbfGX4T3sMax3MuoiJ5VGGZRPDgE+g3t+ZqjY2UPjn9sHVE1hBcW/h+yElpDKMqCqR449mlZ/ritn9oawu7r4q/CeW3tZ5o4dT3SPHGWEY86DliOnQ9fSqfxg8N+J/hz8VbX4ueFNLl1a1kiEOq2cKkvgKEJIAJ2lQvODhlyeKAPePEXh/TvE+iXmjapbR3FndxNFJG4zwR1HoR1B7GvmT4BW32L4I/FW1DB/JW9j3Dvi1YZ/Sun1D9qOTxbpz6N4C8IeILnxFdp5MfnwqI7Zm43kqTnb15wOOSK574BaRqVj8EfiZa3lncx3Lx3aqjxsGkP2UjjI5yaAM21/5MkuP+vk/+lor3r4EaBYaB8J/DUdjbpGbqxiu5mAwZJZFDMxPfk4+gA7V4fbaXfj9jCey+w3X2v7ST5HlN5n/H6D93GenNfQnwnikg+GPhSKVGjkTSbVWRhgqREuQRQB5D4egisf2yPEEdqiwpNpO+RUGAzGOFiT7k8/Wqv7NltB4u+JvxB8Z6mgn1KK98i3aQZMCO0mcZ6fKiL9AR3rV0mwu1/bB1m8NrOLVtIVROYzsJ8qHjd07Gucu/7e/Zs+Kmu6+uh3mreDPELmaR7Rcm3csWAPYFSzgA4BU9cjgA9H/ag8Oafrfwf1i6uoo/tGmhLq2lI+aNg6ggH3UkY+npWv8As+/8kZ8Kf9ef/szV4j8YvjNqXxf8EajpnhHw7qtpoVtH9r1TUr+MRrsQgrGuCRktt75OOmMmvb/2fgR8GfCgIIP2IH/x5qAO/mlSCJ5ZXVI0UszMcBQOpNfGHg743eHLX41eJPiF4jtdVu1mDW2mJZwLJ5ceQoJ3MuD5agcf32r6A/aR8T3vh34W6jBpkFxPqGq4sIlgjLMquD5jcdBsDDPqRWn8DPBI8B/DHRtLli8u8ki+1XYIwfOk+Yg+6jC/8BoA+ZvjT8a/D3izxZ4X8Y+E7LWLTVtFl/ete26xrKgcMgyrt33gj0avRv2qPFi+Ivh14Oi02crp/iO7jnZx3j2AqD+Lg49V9q9s+JPhGLx14F1rw84G69tmWIn+GUfNG34MFNfLvhfwjr/xQ+Bl14RazuYPEPhK++02EdwhjM8Lhsxgt3zvx9FHegD630HQdP8ADei2mj6ZbR29naRLFHGowAAP1J6k9zXzp4U8O2Hhj9sHVbPTYkhtpbGS5EUYwqNJEjMAO3zEnHvWjof7V39laRDpPijwd4iHii3QQvBDAAs8gGM/MQy5IyRtOO2a5b4Qy+JL/wDabn1XxVaGy1TUdOlvHtDnNtGyqI0YdiEC8Hn15oAw/hR8TrnQ/Gni3xZceBtb8U6rf3jKLmyiL/ZE3MSmQpxn5R9FAra+M/xEv/ix4TOlj4S+K7TUYJUmtL17R2MJBG4cJnBXIx64Pats3Gv/ALNHxC8QX58P3useCdfn+1ebZrua0fJOD2BG4jBxuG0g8EVa8RfH7xH8U47fw58JtB1y0vriZPO1S6hVFtkByem5QD3JPTgAk0Ac18Zo7rxdpvwTtvEMNxDdagRb3qTKUk3M1uj5B5BPJ/GvqyLTLK205dOitYEs0i8lbdUAjCYxt29MY4xXgHx60bU08VfCCFjdanNZ36rc3YjJLsJLfLtgYGSCa+iD0oA+PfhlrNx4Z+A3xVm092hMN6YYdh/1fmbYyR6HB6+1e1fsxeGdP0H4RaPd20EYutTVru5mx80jFiFBPoFAAH19a88+AfgeXxR8PPiP4b1KCezGp3zxxvLGVwdvyuAeoDAH8Kq/Dv4wav8AAXSm8DfELwvrHlWEjiyvLOMOsiFicAsQGXJJBB74IGKAPcvEfwl8N+JvGWk+MbpLqDWNKZTFLbSBBJtbIEgwdw6j6EivKPDsH/Cpv2ndQ0j/AFOjeM4DcW46Ks+S2PrvEgA/6aLUega14t+PXxU0bX7Sw1fw94L0M+bmZ2j+2uDuwQDhskKCBkBQecmum/aj8Nzz+DrHxlpnyar4VvI76Jx18vcoYfgQjfRTQBjeOIv+Fo/tJaB4Yx5uleE4P7RvR1UykqwU+uT5I/Fq+gq8T/Zj0i6v9H1z4h6rHt1LxXfSXAB/ggViFUZ7bi34Ba9soAKKKKACiiigAxiiiigAooooAKTaoOQBS0UAFFFFABRRRQAgVR0AFLgUUUAFFFFABivDPE3if4p/DL4galqE+j6j4y8HX3zW8VlGDLY8524Vc8cjngjHIORXudHWgD5p8e+MvHfxz0seC/C3gLWtD0+8kT7fqGrRGFVQMGx0xjIBOCScYAr6B8KeH7fwp4a0vQrUlodPtY7ZWPVtqgbj7nGfxrU6UtABRRRQAVxnxb0zxjqfg2dfAmpmw1yB1miwF/fqM7o8sCBnOQfUDkV2dFAHgth+0L4q07T0ste+E3it9ciQRv8AZrVmhmcDG4Nt4BPpu+pqz8EvA3iy98ca78UPHNiNN1LVY/s9pYH70EPy9R24RFAPPUkDNe4YHpS0ABAPUUgUDoAPpS0UAFFFFABSEBuoB+tLRQAAAdBXzp8VNS+JfxYvbn4caf4IvdH0l9Q8u51qct5M9vHJkOCVAwcBsAsTgAV9F0UAUNB0W08O6JY6PYJstbGBLeJfRVUAfjxV+iigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAAUUUUAFFFFABRRRQAgpaKKACkoooAWiiigAooooAO1IaKKAFNFFFAAKQ0UUAf/9k=' /><br /><br /></div><h2 style=""width: 75%; font-size: 1.6em;"">Local Authority summary of school sixth form<br>revenue funding allocations: 2021 to 2022</h2>
<table id=""topTable"" class=""bold left"" style=""width: 73%; text-align: center !important;"">


<tbody>

<tr>
<td style=""width: 20%;"">Local Authority</td><td>Hertfordshire</td></tr>
<tr>
<td>Local Authority Code</td><td>919</td></tr></tbody></table><div style=""clear: both""></div>
<table id=""2ndTable"" class=""styleTable"" style=""width: 100%; text-align: center !important; border: 0 !important;"">


<tbody>

<tr class=""bold"">
<th style=""width: 60%; border: 0;"" scope=""row""></th><td rowspan=""2"" style=""text-align: right; vertical-align: middle;"" class=""greyBold""><br>Total<br>Academic<br>Year Funding</td><td colspan=""2"" class=""greyBold"">Financial Year (FY) split</td></tr>
<tr class=""bold"">
<th style=""border: 0;"" scope=""row""></th><td style=""text-align: right"" class=""greyBold"">8&nbsp;Months<br>in&nbsp;2021-22&nbsp;FY</td><td style=""text-align: right"" class=""greyBold"">4&nbsp;Months<br>in&nbsp;2022-23&nbsp;FY</td></tr>
<tr>
<th scope=""row"" class=""left bold"">Programme and Student Financial Support Funding - Mainstream School Sixth Forms</th><td style=""text-align: right"">&pound;0</td><td style=""text-align: right"">&pound;0</td><td style=""text-align: right"">&pound;0</td></tr>
<tr>
<th scope=""row"" class=""left bold"">Student Financial Support Funding - Maintained Special Schools</th><td style=""text-align: right"">&pound;10,785</td><td style=""text-align: right"">&pound;400</td><td style=""text-align: right"">&pound;500</td></tr>
<tr class=""greyBold"">
<td class=""bold"">Total LA Funding</td><td style=""text-align: right"">&pound;10,785</td><td style=""text-align: right"">&pound;400</td><td style=""text-align: right"">&pound;500</td></tr></tbody></table><div class=""smallBr""></div>
<table id=""3rdTable"" class=""styleTable table3"" style=""width: 100%; text-align: center !important;"">


<thead>
    <tr class=""greyBold"">
<th colspan=""13"" style=""font-size: 1.3em"" scope=""col"">School Sixth Form Institutions</th>    </tr>
    <tr class=""greyBold"">
<th colspan=""3"" scope=""col"">School Sixth Form details</th><th colspan=""6"" scope=""col"">Academic Year (AY) Allocation Information (August 2021 to July 2022)</th><th colspan=""2"" scope=""col"">FY&nbsp;split&nbsp;of&nbsp;total&nbsp;AY&nbsp;allocation</th>    </tr>
    <tr class=""whiteBackgroundBold topAlign"">
<th scope=""col"">UKPRN</th><th style=""width: 15%"" scope=""col"">Institution&nbsp;Name</th><th scope=""col"">LAEstab<br>Number</th><th scope=""col"">Programme<br>Funding</th><th scope=""col"">Student<br>Financial<br>Support<br>Funding</th><th scope=""col"">Industry<br>Placements<br>Funding</th><th scope=""col"">Advanced<br>Maths&nbsp;Premium<br>Funding</th><th scope=""col"">High&nbsp;Value<br>Courses<br>Premium</th><th scope=""col"">Total<br>AY Funding</th><th scope=""col"">8 Months<br>in 2021-22 FY<br><span style='font-size: 0.7em'>(August 21-March 22)</span></th><th scope=""col"">4 Months<br>in 2022-23 FY<br><span style='font-size: 0.7em'>(April 22-July 22)</span></th>    </tr>
</thead>
<tbody>

<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total</td><td></td><td></td><td></td><td></td><td></td><td class=""right""></td><td class=""right"">&pound;0</td><td class=""right"">&pound;0</td></tr></tbody></table><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 12px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .topAlign { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .darkGrey, .darkGrey * { background-color: #666 !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 1px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .whiteBackgroundBold td, .whiteBackgroundBold th { background-color: #FFF !important; font-weight: bold !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; } </style></body></html>";

        private readonly string html_1619_Academies_with =
       @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>16 to 19 academies revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your revenue funding allocation statement.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>The Stourport High School and Sixth Form College</td></tr>
<tr>
<td>UKPRN</td><td>10034690</td></tr>
<tr>
<td>Local authority</td><td>Worcestershire</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021/22 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 50%;"">Core programme funding</td><td style=""width: 50%;"" class=""right"">&pound;525,174</td></tr>
<tr>
<td>Condition of funding adjustment</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;12,800</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">High needs student</td><td style=""width: 50%;"" class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">Student financial support</td><td style=""width: 50%;"" class=""right"">&pound;8,063</td></tr>
<tr>
<td style=""width: 50%;"">Teachers' pension scheme grant</td><td style=""width: 50%;"" class=""right"">&pound;682,402</td></tr>
<tr>
<td style=""width: 50%;"">Alternative completion</td><td style=""width: 50%;"" class=""right"">&pound;54,978</td></tr>
<tr>
<td style=""width: 50%;"">Start-up and post-opening grant</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr>
<td style=""width: 50%;"">Maths top up</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total funding allocation</td><td class=""right"">&pound;601,016</td></tr></tbody></table><div style='font-weight: bold; font-size: 14px; margin-top: 5px'>Core programme funding</div>
<table id=""formulaTables"" class="""">


<tbody>

<tr>
<td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAADiSURBVGhD7dsxioQwGEDhmGJBEYLiDQIeYgsL72XvmfZyrsuEhYfjzJTj8L5Kfh6SvwuCYXvmr1iW5ftcWNc1pRQe6LquPJ35L35OlKKqqtu5jizoo4phGPYixlgGBxfaxQIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsKOSc67pOKZXBwYV2sQALsiALsiALsqDQ9/1FTmoBFmRBFmRBFmRBb1OM49i27X5PKYODC+1iARZkQRb0UUXOuWkav1oXFvRmxe7rRJimKcZ4i+7b3zPPc+nvee2/k8eeFdv2C7C2ttUtLoinAAAAAElFTkSuQmCC' /></td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Student&nbsp;numbers<br>for 2021/22</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;student&nbsp;numbers<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">123</td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;"">&nbsp;</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">National funding<br>rate per student<br>(dependent on<br>band)</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;breakdown&nbsp;of<br>funding&nbsp;by&nbsp;band&nbsp;table<br><br></td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;""></td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;514,265</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Retention&nbsp;factor</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">0.96200</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">-&pound;19,594</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;494,672</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Programme&nbsp;cost<br>weighting</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.02400</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;11,798</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;506,470</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Level 3<br>programme maths<br>and English<br>payment</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;Level&nbsp;3<br>programme&nbsp;maths&nbsp;and<br>English&nbsp;payment&nbsp;table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;3,515</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;509,984</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Disadvantage<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;distribution&nbsp;of<br>disadvantage&nbsp;funding<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;14,352</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;524,336</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Large<br>programme<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;large<br>programmes&nbsp;uplift<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;838</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;525,174</td></tr></tbody></table></td><td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAAEKSURBVGhD7dvNaoQwGEZhoyAoEongzYgrBS/MldfkpQliv6GhcJimsyjUMrzPIpiZg2Qgi8Efd11X9iM3DEM8fDLP87qucfKttm23bYuTlBCC2/c9zmhZFhsfRWqleZ7bV1bk8YM0FfQGhXPOxqIoksXnvjnP8+aVflFBKkgFqSAVpIJUkApSQSpIBakgFaSCVJAKUkEqSAWpoGThva+qqus63SsgFaSCVJAKUkEq6G0K59yLwv4+/IuVGhWkglSQClJBKkgF3VyEEJqm6fteVzdIBakgFaSCbi6893Vd66r1ExX0N4UryzIe0nEcNtoufDxwn2JbdRzHzM6RMk2TbebX7538/rdk2QcWtDy+yWdeMAAAAABJRU5ErkJggg==' /></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Area&nbsp;cost<br>allowance</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.00000</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;0</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;525,174</td></tr></tbody></table></td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""3"" scope=""col"">Student numbers</th>    </tr>
</thead>
<tbody>

<tr style=""width: 100%;"">
<td colspan=""2"" style=""width: 80%"">Autumn census</td><td style=""width: 20%; text-align: right"">123</td></tr>
<tr>
<td colspan=""2"">Exceptional variations to lagged student number</td><td style=""width: 15%; text-align: right"">0</td></tr>
<tr class=""greyBold"">
<td colspan=""2"" class=""bold"">Total student numbers for 2021/22</td><td style=""width: 20%; text-align: right"">123</td></tr>
<tr style=""width: 100%;"">
<td style=""width: 65%;"">Student number methodology used</td><td colspan=""2"" style=""width: 35%;"">Autumn census</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Breakdown of funding by band</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 30%"">Funding band<br><br></td><td>Student&nbsp;numbers<br>2019/20</td><td>Proportions&nbsp;used&nbsp;in<br>2021/22 allocation</td><td>Number&nbsp;of&nbsp;students<br>allocated in 2021/22</td><td>National&nbsp;funding&nbsp;rate</td><td style=""width: 20%"">Student&nbsp;funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>104</td><td>99.05%</td><td>122</td><td>&pound;4,188</td><td>&pound;510,218</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4 (Sum of bands 4a and 4b)</td><td>1</td><td>0.95%</td><td>1</td><td>&pound;3,455</td><td>&pound;4,047</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4a</td><td>1</td><td>0.95%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4b</td><td>0</td><td>0.00%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,827</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,234</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Band 1</td><td>Students</td><td>0</td><td>0.00%</td><td>0</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td>FTEs</td><td class=""right"">0.00</td><td class=""greyBold""></td><td>0.00</td><td>&pound;4,188</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total student funding</td><td>105</td><td>100%</td><td>123</td><td></td><td>&pound;514,265</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Level 3 programme maths and English payment</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 42%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Instances per student</th><th style=""width: 14%;"" scope=""col"">Number of instances</th><th style=""width: 10%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 1 year programme</td><td>0.00000</td><td>0.00</td><td>&pound;375</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 2 year programme</td><td>0.03800</td><td>4.69</td><td>&pound;750</td><td>&pound;3,515</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""4"" class=""bold"">Level 3 programme maths and English payment funding total</td><td>&pound;3,515</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""5"" scope=""col"">Distribution of disadvantage funding</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 1</td></tr>
<tr style=""width: 100%;"">
<td colspan=""2"" rowspan=""2"">Economic deprivation funding</td><td>Block 1 factor</td><td>Funding including programme costs</td><td style=""width: 20%;"">Block 1 funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>1.01800</td><td>&pound;509,984</td><td>&pound;9,374</td></tr>
<tr>
<td colspan=""2"" rowspan=""2"">Care leavers</td><td>Number of qualifying students</td><td>Rate per qualifying student</td><td>Care leaver funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>1</td><td>&pound;480</td><td>&pound;480</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 1 funding</td><td>&pound;9,854</td></tr>
<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 2</td></tr>
<tr>
<td colspan=""4"" rowspan=""2"">Total 2021/22 instances attracting funding per student</td><td>Instances per Student</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0.07600</td></tr>
<tr>
<td colspan=""3"" class=""right"">Number of instances in each band</td><td>Block 2 funding rates</td><td>Block 2 funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Total funded instances for 2021/22</td><td>9.37</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the higher rate (bands 4 and 5)</td><td>9.37</td><td>&pound;480</td><td>&pound;4,498</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the lower rate (bands 2 and 3)</td><td>0.00</td><td>&pound;292</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Students attracting the FTE rate<br>(Band 1)</td><td>Students</td><td>0.00</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>0.00</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 2 funding</td><td>&pound;4,498</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""4"">Minimum top up if applicable</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total disadvantage funding (sum of block 1, block 2 and minimum top up)</td><td>&pound;14,352</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Large programme uplift (based on 2018/19 students)</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 40%;""></td><td style=""width: 15%;"">Students meeting large programme uplift criteria</td><td style=""width: 25%;"">Funding uplift per student per year</td><td style=""width: 20%;"">Total large programme uplift (2 years)</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 20% of national rate</td><td>0</td><td>&pound;838</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 10% of national rate</td><td>1</td><td>&pound;419</td><td>&pound;838</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td class=""bold"">Total large programme funding</td><td>1</td><td></td><td>&pound;838</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Condition of funding (CoF)</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 25%"">Funding band<br><br></td><td>National funding rate<br>in 2019/20</td><td>Total students <br> (2019/20 S05)</td><td>National funding rate<br>applied to total students <br> (2019/20 S05)</td><td>Students not meeting<br>CoF (2019/20 S05)</td><td style=""width: 20%"">National funding rate applied to<br>CoF non-compliant students</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>&pound;4,000</td><td>104</td><td>&pound;416,000</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4</td><td>&pound;3,300</td><td>1</td><td>&pound;3,300</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>&pound;2,700</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>&pound;2,133</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"" style=""width: 20%"">Band 1</td><td>Students</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>&pound;4,000</td><td>0.00</td><td>&pound;0</td><td>0.00</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total</td><td>105</td><td>&pound;419,300</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">5% of national rate funding for total 2019/20 S05 students</td><td>&pound;20,965</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">Funding for non-compliant students less 5% of funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""6"" class=""bold"">Final condition of funding adjustment (at 50%)</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Advanced maths premium</caption>


<thead class=""whiteBackground top"">
    <tr>
<th style=""width: 24%;"" scope=""col""></th><th style=""width: 13%;"" scope=""col"">Baseline students</th><th style=""width: 14%;"" scope=""col"">Eligible students</th><th style=""width: 14%;"" scope=""col"">Eligible minus<br>baseline</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Advanced maths premium funding</td><td>65</td><td>44</td><td>0</td><td>&pound;600</td><td class=""bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">High value courses premium</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Qualifying students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>High value courses premium funding</td><td>32</td><td>&pound;400</td><td class=""bold"">&pound;12,800</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Industry placements funding</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Number&nbsp;of&nbsp;students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Industry&nbsp;placements:&nbsp;Capacity&nbsp;and<br>delivery&nbsp;funding&nbsp;(CDF)</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Industry&nbsp;placements:&nbsp;Capacity&nbsp;and&nbsp;delivery&nbsp;funding&nbsp;(CDF)</td><td>0</td><td>&pound;250</td><td class=""bold"">&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">High needs funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">16-19 students</td><td style=""width: 15%"">19-24 students</td><td style=""width: 15%"">Total students</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>High needs element 2 for 2021/22</td><td>0</td><td>0</td><td>0</td><td>&pound;6,000</td><td style=""font-weight: bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">Student financial support funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">Number of funded students</td><td style=""width: 15%"">Instances per student</td><td style=""width: 15%"">Number of instances</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 1: Financial disadvantage</td><td>123</td><td>0.10900</td><td>13.35</td><td>&pound;242</td><td>&pound;3,232</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 2a: Student costs - travel</td><td>123</td><td>0.06200</td><td>7.61</td><td>&pound;483</td><td>&pound;3,678</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Element 2b: Student costs - industry placements</td><td>0</td><td>&pound;49</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Bursary adjustment in respect of free meals</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund</td><td>&pound;6,910</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">2019 to 2020 discretionary bursary fund - baseline for transition</td><td>&pound;10,751</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition lower limit</td><td>&pound;8,063</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition upper limit</td><td>&pound;13,439</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Exceptional adjustment</td><td>&pound;0</td></tr></tbody></table>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Teachers' pension scheme grant</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 20%;"" scope=""col""></th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Payments made to Capita<br>for TPS, financial year<br>2019-2020 rebased at<br>16.4% for the full year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Annual payments<br>increased to 0%<br>equivalent for the full<br>year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">0% uplift for 2020-21</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">0% uplift for 2021-22</th><th style=""vertical-align: top; width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Revised annual cost</td><td>&pound;1,938,115</td><td>&pound;2,788,995</td><td>&pound;86,459</td><td>&pound;86,264</td><td>&pound;2,961,718</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Teachers'&nbsp;pension&nbsp;scheme&nbsp;grant&nbsp;(difference&nbsp;between&nbsp;2019/2020&nbsp;payments&nbsp;at&nbsp;16.4%&nbsp;and revised&nbsp;annual&nbsp;cost)</td><td style=""font-weight: bold"">&pound;682,402</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead class=""greyBold"">
    <tr>
<th colspan=""2"" scope=""col"">Alternative completion</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td>Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Alternative completion funding</td><td style=""font-weight: bold"">&pound;54,978</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Start-up and post-opening grant</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part A</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part B</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - per pupil resources</td><td>-&pound;997</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - leadership disecononomies</td><td>-&pound;997</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td style=""font-weight: bold"">Total start-up and post-opening grant</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Maths top up</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Maths top up funding</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style='font-size: 10px; margin-top: 5px'>The values on your statement are shown rounded to various numbers of decimal places.<br> The calculation of your funding however is done using un-rounded values. This may result in some slight differences when you work through the calculation yourselves.</div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_Academies_without =
       @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>16 to 19 academies revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your revenue funding allocation statement.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>Aylward Academy</td></tr>
<tr>
<td>UKPRN</td><td>10030654</td></tr>
<tr>
<td>Local authority</td><td>Enfield</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021/22 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 50%;"">Core programme funding</td><td style=""width: 50%;"" class=""right"">&pound;1,375,699</td></tr>
<tr>
<td>Condition of funding adjustment</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;4,200</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;9,200</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">High needs student</td><td style=""width: 50%;"" class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">Student financial support</td><td style=""width: 50%;"" class=""right"">&pound;41,327</td></tr>
<tr>
<td style=""width: 50%;"">Teachers' pension scheme grant</td><td style=""width: 50%;"" class=""right"">&pound;682,402</td></tr>
<tr>
<td style=""width: 50%;"">Start-up and post-opening grant</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr>
<td style=""width: 50%;"">Maths top up</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total funding allocation</td><td class=""right"">&pound;1,430,427</td></tr></tbody></table><div style='font-weight: bold; font-size: 14px; margin-top: 5px'>Core programme funding</div>
<table id=""formulaTables"" class="""">


<tbody>

<tr>
<td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAADiSURBVGhD7dsxioQwGEDhmGJBEYLiDQIeYgsL72XvmfZyrsuEhYfjzJTj8L5Kfh6SvwuCYXvmr1iW5ftcWNc1pRQe6LquPJ35L35OlKKqqtu5jizoo4phGPYixlgGBxfaxQIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsKOSc67pOKZXBwYV2sQALsiALsiALsqDQ9/1FTmoBFmRBFmRBFmRBb1OM49i27X5PKYODC+1iARZkQRb0UUXOuWkav1oXFvRmxe7rRJimKcZ4i+7b3zPPc+nvee2/k8eeFdv2C7C2ttUtLoinAAAAAElFTkSuQmCC' /></td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Student&nbsp;numbers<br>for 2021/22</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;student&nbsp;numbers<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">245</td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;"">&nbsp;</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">National funding<br>rate per student<br>(dependent on<br>band)</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;breakdown&nbsp;of<br>funding&nbsp;by&nbsp;band&nbsp;table<br><br></td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;""></td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;1,010,716</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Retention&nbsp;factor</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">0.98800</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">-&pound;12,129</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;998,588</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Programme&nbsp;cost<br>weighting</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.04200</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;41,941</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;1,040,528</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Level 3<br>programme maths<br>and English<br>payment</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;Level&nbsp;3<br>programme&nbsp;maths&nbsp;and<br>English&nbsp;payment&nbsp;table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;15,313</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;1,055,841</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Disadvantage<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;distribution&nbsp;of<br>disadvantage&nbsp;funding<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;172,462</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;1,228,303</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Large<br>programme<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;large<br>programmes&nbsp;uplift<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;0</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;1,228,303</td></tr></tbody></table></td><td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAAEKSURBVGhD7dvNaoQwGEZhoyAoEongzYgrBS/MldfkpQliv6GhcJimsyjUMrzPIpiZg2Qgi8Efd11X9iM3DEM8fDLP87qucfKttm23bYuTlBCC2/c9zmhZFhsfRWqleZ7bV1bk8YM0FfQGhXPOxqIoksXnvjnP8+aVflFBKkgFqSAVpIJUkApSQSpIBakgFaSCVJAKUkEqSAWpoGThva+qqus63SsgFaSCVJAKUkEq6G0K59yLwv4+/IuVGhWkglSQClJBKkgF3VyEEJqm6fteVzdIBakgFaSCbi6893Vd66r1ExX0N4UryzIe0nEcNtoufDxwn2JbdRzHzM6RMk2TbebX7538/rdk2QcWtDy+yWdeMAAAAABJRU5ErkJggg==' /></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Area&nbsp;cost<br>allowance</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.12000</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;147,396</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;1,375,699</td></tr></tbody></table></td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""3"" scope=""col"">Student numbers</th>    </tr>
</thead>
<tbody>

<tr style=""width: 100%;"">
<td colspan=""2"" style=""width: 80%"">Autumn census</td><td style=""width: 20%; text-align: right"">245</td></tr>
<tr>
<td colspan=""2"">Exceptional variations to lagged student number</td><td style=""width: 15%; text-align: right"">0</td></tr>
<tr class=""greyBold"">
<td colspan=""2"" class=""bold"">Total student numbers for 2021/22</td><td style=""width: 20%; text-align: right"">245</td></tr>
<tr style=""width: 100%;"">
<td style=""width: 65%;"">Student number methodology used</td><td colspan=""2"" style=""width: 35%;"">Autumn census</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Breakdown of funding by band</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 30%"">Funding band<br><br></td><td>Student&nbsp;numbers<br>2019/20</td><td>Proportions&nbsp;used&nbsp;in<br>2021/22 allocation</td><td>Number&nbsp;of&nbsp;students<br>allocated in 2021/22</td><td>National&nbsp;funding&nbsp;rate</td><td style=""width: 20%"">Student&nbsp;funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>31</td><td>91.87%</td><td>225</td><td>&pound;4,188</td><td>&pound;942,600</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4 (Sum of bands 4a and 4b)</td><td>2</td><td>7.66%</td><td>19</td><td>&pound;3,455</td><td>&pound;64,802</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4a</td><td>0</td><td>N/A</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4b</td><td>2</td><td>N/A</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>19</td><td>0.48%</td><td>1</td><td>&pound;2,827</td><td>&pound;3,314</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>4</td><td>0.00%</td><td>0</td><td>&pound;2,234</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Band 1</td><td>Students</td><td>34</td><td>0.00%</td><td>0</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td>FTEs</td><td class=""right"">13.16</td><td class=""greyBold""></td><td>0.00</td><td>&pound;4,188</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total student funding</td><td>90</td><td>100%</td><td>245</td><td></td><td>&pound;1,010,716</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Level 3 programme maths and English payment</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 42%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Instances per student</th><th style=""width: 14%;"" scope=""col"">Number of instances</th><th style=""width: 10%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 1 year programme</td><td>0.01100</td><td>2.72</td><td>&pound;375</td><td>&pound;1,021</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 2 year programme</td><td>0.07800</td><td>19.06</td><td>&pound;750</td><td>&pound;14,292</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""4"" class=""bold"">Level 3 programme maths and English payment funding total</td><td>&pound;15,313</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""5"" scope=""col"">Distribution of disadvantage funding</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 1</td></tr>
<tr style=""width: 100%;"">
<td colspan=""2"" rowspan=""2"">Economic deprivation funding</td><td>Block 1 factor</td><td>Funding including programme costs</td><td style=""width: 20%;"">Block 1 funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>1.10900</td><td>&pound;1,055,841</td><td>&pound;115,076</td></tr>
<tr>
<td colspan=""2"" rowspan=""2"">Care leavers</td><td>Number of qualifying students</td><td>Rate per qualifying student</td><td>Care leaver funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 1 funding</td><td>&pound;115,076</td></tr>
<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 2</td></tr>
<tr>
<td colspan=""4"" rowspan=""2"">Total 2021/22 instances attracting funding per student</td><td>Instances per Student</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0.48900</td></tr>
<tr>
<td colspan=""3"" class=""right"">Number of instances in each band</td><td>Block 2 funding rates</td><td>Block 2 funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Total funded instances for 2021/22</td><td>119.78</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the higher rate (bands 4 and 5)</td><td>119.20</td><td>&pound;480</td><td>&pound;57,218</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the lower rate (bands 2 and 3)</td><td>0.57</td><td>&pound;292</td><td>&pound;167</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Students attracting the FTE rate<br>(Band 1)</td><td>Students</td><td>0.00</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>0.00</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 2 funding</td><td>&pound;57,386</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""4"">Minimum top up if applicable</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total disadvantage funding (sum of block 1, block 2 and minimum top up)</td><td>&pound;172,462</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Large programme uplift (based on 2018/19 students)</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 40%;""></td><td style=""width: 15%;"">Students meeting large programme uplift criteria</td><td style=""width: 25%;"">Funding uplift per student per year</td><td style=""width: 20%;"">Total large programme uplift (2 years)</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 20% of national rate</td><td>0</td><td>&pound;838</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 10% of national rate</td><td>0</td><td>&pound;419</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td class=""bold"">Total large programme funding</td><td>0</td><td></td><td>&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Condition of funding (CoF)</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 25%"">Funding band<br><br></td><td>National funding rate<br>in 2019/20</td><td>Total students <br> (2019/20 S05)</td><td>National funding rate<br>applied to total students <br> (2019/20 S05)</td><td>Students not meeting<br>CoF (2019/20 S05)</td><td style=""width: 20%"">National funding rate applied to<br>CoF non-compliant students</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>&pound;4,000</td><td>31</td><td>&pound;416,000</td><td>1</td><td>&pound;4,000</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4</td><td>&pound;3,300</td><td>2</td><td>&pound;6,600</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>&pound;2,700</td><td>19</td><td>&pound;51,300</td><td>1</td><td>&pound;2,700</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>&pound;2,133</td><td>4</td><td>&pound;8,532</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"" style=""width: 20%"">Band 1</td><td>Students</td><td class=""greyBold""></td><td>34</td><td class=""greyBold""></td><td>2</td><td class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>&pound;4,000</td><td>13.16</td><td>&pound;52,620</td><td>0.76</td><td>&pound;3,053</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total</td><td>90</td><td>&pound;243,052</td><td>4</td><td>&pound;9,753</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">5% of national rate funding for total 2019/20 S05 students</td><td>&pound;12,153</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">Funding for non-compliant students less 5% of funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""6"" class=""bold"">Final condition of funding adjustment (at 50%)</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Advanced maths premium</caption>


<thead class=""whiteBackground top"">
    <tr>
<th style=""width: 24%;"" scope=""col""></th><th style=""width: 13%;"" scope=""col"">Baseline students</th><th style=""width: 14%;"" scope=""col"">Eligible students</th><th style=""width: 14%;"" scope=""col"">Eligible minus<br>baseline</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Advanced maths premium funding</td><td>33</td><td>40</td><td>7</td><td>&pound;600</td><td class=""bold"">&pound;4,200</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">High value courses premium</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Qualifying students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>High value courses premium funding</td><td>23</td><td>&pound;400</td><td class=""bold"">&pound;9,200</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Industry placements funding</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Number&nbsp;of&nbsp;students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Industry&nbsp;placements:&nbsp;Capacity&nbsp;and<br>delivery&nbsp;funding&nbsp;(CDF)</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Industry&nbsp;placements:&nbsp;Capacity&nbsp;and&nbsp;delivery&nbsp;funding&nbsp;(CDF)</td><td>0</td><td>&pound;250</td><td class=""bold"">&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">High needs funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">16-19 students</td><td style=""width: 15%"">19-24 students</td><td style=""width: 15%"">Total students</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>High needs element 2 for 2021/22</td><td>0</td><td>0</td><td>0</td><td>&pound;6,000</td><td style=""font-weight: bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">Student financial support funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">Number of funded students</td><td style=""width: 15%"">Instances per student</td><td style=""width: 15%"">Number of instances</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 1: Financial disadvantage</td><td>245</td><td>0.67100</td><td>164.42</td><td>&pound;242</td><td>&pound;39,790</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 2a: Student costs - travel</td><td>245</td><td>0.01300</td><td>3.18</td><td>&pound;483</td><td>&pound;1,537</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Element 2b: Student costs - industry placements</td><td>0</td><td>&pound;49</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Bursary adjustment in respect of free meals</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund</td><td>&pound;41,327</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">2019 to 2020 discretionary bursary fund - baseline for transition</td><td>&pound;39,111</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition lower limit</td><td>&pound;29,334</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition upper limit</td><td>&pound;48,889</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Exceptional adjustment</td><td>&pound;0</td></tr></tbody></table>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Teachers' pension scheme grant</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 20%;"" scope=""col""></th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Payments made to Capita<br>for TPS, financial year<br>2019-2020 rebased at<br>16.4% for the full year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Annual payments<br>increased to 0%<br>equivalent for the full<br>year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">0% uplift for 2020-21</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">0% uplift for 2021-22</th><th style=""vertical-align: top; width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Revised annual cost</td><td>&pound;1,938,115</td><td>&pound;2,788,995</td><td>&pound;86,459</td><td>&pound;86,264</td><td>&pound;2,961,718</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Teachers'&nbsp;pension&nbsp;scheme&nbsp;grant&nbsp;(difference&nbsp;between&nbsp;2019/2020&nbsp;payments&nbsp;at&nbsp;16.4%&nbsp;and revised&nbsp;annual&nbsp;cost)</td><td style=""font-weight: bold"">&pound;682,402</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Start-up and post-opening grant</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part A</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part B</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - per pupil resources</td><td>-&pound;997</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - leadership disecononomies</td><td>-&pound;997</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td style=""font-weight: bold"">Total start-up and post-opening grant</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Maths top up</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Maths top up funding</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style='font-size: 10px; margin-top: 5px'>The values on your statement are shown rounded to various numbers of decimal places.<br> The calculation of your funding however is done using un-rounded values. This may result in some slight differences when you work through the calculation yourselves.</div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_TuitionFunding_Without_MathsTop =
   @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>16 to 19 revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your revenue funding allocation statement.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>Notton House Academy</td></tr>
<tr>
<td>UKPRN</td><td>10064744</td></tr>
<tr>
<td>Local authority</td><td>Bristol City of</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021/22 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 50%;"">Core programme funding</td><td style=""width: 50%;"" class=""right"">&pound;803,165</td></tr>
<tr>
<td>Condition of funding adjustment</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;62,400</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;19,110</td></tr>
<tr>
<td>Care standards</td><td class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">High needs student</td><td style=""width: 50%;"" class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">Student financial support</td><td style=""width: 50%;"" class=""right"">&pound;24,782</td></tr>
<tr>
<td style=""width: 50%;"">Teachers' pension scheme grant</td><td style=""width: 50%;"" class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">16 to 19 Tuition funding</td><td style=""width: 50%;"" class=""right"">&pound;2,000</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total funding allocation</td><td class=""right"">&pound;909,457</td></tr></tbody></table><div style='font-weight: bold; font-size: 14px; margin-top: 5px'>Core programme funding</div>
<table id=""formulaTables"" class="""">


<tbody>

<tr>
<td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAADiSURBVGhD7dsxioQwGEDhmGJBEYLiDQIeYgsL72XvmfZyrsuEhYfjzJTj8L5Kfh6SvwuCYXvmr1iW5ftcWNc1pRQe6LquPJ35L35OlKKqqtu5jizoo4phGPYixlgGBxfaxQIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsKOSc67pOKZXBwYV2sQALsiALsiALsqDQ9/1FTmoBFmRBFmRBFmRBb1OM49i27X5PKYODC+1iARZkQRb0UUXOuWkav1oXFvRmxe7rRJimKcZ4i+7b3zPPc+nvee2/k8eeFdv2C7C2ttUtLoinAAAAAElFTkSuQmCC' /></td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Student&nbsp;numbers<br>for 2021/22</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;student&nbsp;numbers<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">143</td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;"">&nbsp;</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">National funding<br>rate per student<br>(dependent on<br>band)</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;breakdown&nbsp;of<br>funding&nbsp;by&nbsp;band&nbsp;table<br><br></td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;""></td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;594,181</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Retention&nbsp;factor</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">0.98524</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">-&pound;8,770</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;585,410</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Programme&nbsp;cost<br>weighting</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.31972</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;187,167</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;772,578</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Level 3<br>programme maths<br>and English<br>payment</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;Level&nbsp;3<br>programme&nbsp;maths&nbsp;and<br>English&nbsp;payment&nbsp;table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;5,500</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;778,078</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Disadvantage<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;distribution&nbsp;of<br>disadvantage&nbsp;funding<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;25,088</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;803,165</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Large<br>programme<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;large<br>programmes&nbsp;uplift<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;0</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;803,165</td></tr></tbody></table></td><td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAAEKSURBVGhD7dvNaoQwGEZhoyAoEongzYgrBS/MldfkpQliv6GhcJimsyjUMrzPIpiZg2Qgi8Efd11X9iM3DEM8fDLP87qucfKttm23bYuTlBCC2/c9zmhZFhsfRWqleZ7bV1bk8YM0FfQGhXPOxqIoksXnvjnP8+aVflFBKkgFqSAVpIJUkApSQSpIBakgFaSCVJAKUkEqSAWpoGThva+qqus63SsgFaSCVJAKUkEq6G0K59yLwv4+/IuVGhWkglSQClJBKkgF3VyEEJqm6fteVzdIBakgFaSCbi6893Vd66r1ExX0N4UryzIe0nEcNtoufDxwn2JbdRzHzM6RMk2TbebX7538/rdk2QcWtDy+yWdeMAAAAABJRU5ErkJggg==' /></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Area&nbsp;cost<br>allowance</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.00000</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;0</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;803,165</td></tr></tbody></table></td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""3"" scope=""col"">Student numbers</th>    </tr>
</thead>
<tbody>

<tr style=""width: 100%;"">
<td colspan=""2"" style=""width: 80%"">Autumn census</td><td style=""width: 20%; text-align: right"">143</td></tr>
<tr>
<td colspan=""2"">Exceptional variations to lagged student number</td><td style=""width: 15%; text-align: right"">0</td></tr>
<tr class=""greyBold"">
<td colspan=""2"" class=""bold"">Total student numbers for 2021/22</td><td style=""width: 20%; text-align: right"">143</td></tr>
<tr style=""width: 100%;"">
<td style=""width: 65%;"">Student number methodology used</td><td colspan=""2"" style=""width: 35%;"">Autumn census</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Breakdown of funding by band</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 30%"">Funding band<br><br></td><td>Student&nbsp;numbers<br>2019/20</td><td>Proportions&nbsp;used&nbsp;in<br>2021/22 allocation</td><td>Number&nbsp;of&nbsp;students<br>allocated in 2021/22</td><td>National&nbsp;funding&nbsp;rate</td><td style=""width: 20%"">Student&nbsp;funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>149</td><td>95.51%</td><td>137</td><td>&pound;4,188</td><td>&pound;572,011</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4 (Sum of bands 4a and 4b)</td><td>7</td><td>4.49%</td><td>6</td><td>&pound;3,455</td><td>&pound;22,170</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4a</td><td>7</td><td>4.49%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4b</td><td>0</td><td>0.00%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,827</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,234</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Band 1</td><td>Students</td><td>0</td><td>0.00%</td><td>0</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td>FTEs</td><td class=""right"">0.00</td><td class=""greyBold""></td><td>0.00</td><td>&pound;4,188</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total student funding</td><td>156</td><td>100%</td><td>143</td><td></td><td>&pound;594,181</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Level 3 programme maths and English payment</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 42%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Instances per student</th><th style=""width: 14%;"" scope=""col"">Number of instances</th><th style=""width: 10%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 1 year programme</td><td>0.03846</td><td>5.50</td><td>&pound;375</td><td>&pound;2,062</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 2 year programme</td><td>0.03205</td><td>4.58</td><td>&pound;750</td><td>&pound;3,437</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""4"" class=""bold"">Level 3 programme maths and English payment funding total</td><td>&pound;5,500</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""5"" scope=""col"">Distribution of disadvantage funding</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 1</td></tr>
<tr style=""width: 100%;"">
<td colspan=""2"" rowspan=""2"">Economic deprivation funding</td><td>Block 1 factor</td><td>Funding including programme costs</td><td style=""width: 20%;"">Block 1 funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>1.01754</td><td>&pound;778,078</td><td>&pound;13,647</td></tr>
<tr>
<td colspan=""2"" rowspan=""2"">Care leavers</td><td>Number of qualifying students</td><td>Rate per qualifying student</td><td>Care leaver funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 1 funding</td><td>&pound;13,647</td></tr>
<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 2</td></tr>
<tr>
<td colspan=""4"" rowspan=""2"">Total 2021/22 instances attracting funding per student</td><td>Instances per Student</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0.16667</td></tr>
<tr>
<td colspan=""3"" class=""right"">Number of instances in each band</td><td>Block 2 funding rates</td><td>Block 2 funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Total funded instances for 2021/22</td><td>23.83</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the higher rate (bands 4 and 5)</td><td>23.83</td><td>&pound;480</td><td>&pound;11,440</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the lower rate (bands 2 and 3)</td><td>0.00</td><td>&pound;292</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Students attracting the FTE rate<br>(Band 1)</td><td>Students</td><td>0.00</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>0.00</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 2 funding</td><td>&pound;11,440</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""4"">Minimum top up if applicable</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total disadvantage funding (sum of block 1, block 2 and minimum top up)</td><td>&pound;25,088</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Large programme uplift (based on 2018/19 students)</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 40%;""></td><td style=""width: 15%;"">Students meeting large programme uplift criteria</td><td style=""width: 25%;"">Funding uplift per student per year</td><td style=""width: 20%;"">Total large programme uplift (2 years)</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 20% of national rate</td><td>0</td><td>&pound;838</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 10% of national rate</td><td>0</td><td>&pound;419</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td class=""bold"">Total large programme funding</td><td>0</td><td></td><td>&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Condition of funding (CoF)</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 25%"">Funding band<br><br></td><td>National funding rate<br>in 2019/20</td><td>Total students <br> (2019/20 S05)</td><td>National funding rate<br>applied to total students <br> (2019/20 S05)</td><td>Students not meeting<br>CoF (2019/20 S05)</td><td style=""width: 20%"">National funding rate applied to<br>CoF non-compliant students</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>&pound;4,000</td><td>149</td><td>&pound;596,000</td><td>2</td><td>&pound;8,000</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4</td><td>&pound;3,300</td><td>7</td><td>&pound;23,100</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>&pound;2,700</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>&pound;2,133</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"" style=""width: 20%"">Band 1</td><td>Students</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>&pound;4,000</td><td>0.00</td><td>&pound;0</td><td>0.00</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total</td><td>156</td><td>&pound;619,100</td><td>2</td><td>&pound;8,000</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">5% of national rate funding for total 2019/20 S05 students</td><td>&pound;30,955</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">Funding for non-compliant students less 5% of funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""6"" class=""bold"">Final condition of funding adjustment (at 50%)</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Advanced maths premium</caption>


<thead class=""whiteBackground top"">
    <tr>
<th style=""width: 24%;"" scope=""col""></th><th style=""width: 13%;"" scope=""col"">Baseline students</th><th style=""width: 14%;"" scope=""col"">Eligible students</th><th style=""width: 14%;"" scope=""col"">Eligible minus<br>baseline</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Advanced maths premium funding</td><td>98</td><td>54</td><td>0</td><td>&pound;600</td><td class=""bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">High value courses premium</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Qualifying students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>High value courses premium funding</td><td>156</td><td>&pound;400</td><td class=""bold"">&pound;62,400</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Industry placements funding</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Number&nbsp;of&nbsp;students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Industry&nbsp;placements:&nbsp;Capacity&nbsp;and<br>delivery&nbsp;funding&nbsp;(CDF)</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Industry&nbsp;placements:&nbsp;Capacity&nbsp;and&nbsp;delivery&nbsp;funding&nbsp;(CDF)</td><td>91</td><td>&pound;210</td><td class=""bold"">&pound;19,110</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Care standards funding</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td>Eligible students</td><td style=""width: 15%;"">Rate</td><td style=""width: 20%;"" class=""right"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Care standards student funding</td><td>0</td><td>&pound;817</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Care standards institution lump sum funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total care standards funding</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">High needs funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">16-19 students</td><td style=""width: 15%"">19-24 students</td><td style=""width: 15%"">Total students</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>High needs element 2 for 2021/22</td><td>0</td><td>0</td><td>0</td><td>&pound;6,000</td><td style=""font-weight: bold"">&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">Student financial support funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">Number of funded students</td><td style=""width: 15%"">Instances per student</td><td style=""width: 15%"">Number of instances</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 1: Financial disadvantage</td><td>143</td><td>0.10641</td><td>15.22</td><td>&pound;243</td><td>&pound;3,698</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 2a: Student costs - travel</td><td>143</td><td>0.40171</td><td>57.44</td><td>&pound;468</td><td>&pound;26,884</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Element 2b: Student costs - industry placements</td><td>55</td><td>&pound;48</td><td>&pound;2,640</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Bursary adjustment in respect of free meals</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund</td><td>&pound;33,222</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">2019 to 2020 discretionary bursary fund - baseline for transition</td><td>&pound;16,521</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition lower limit</td><td>&pound;8,261</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition upper limit</td><td>&pound;24,782</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Exceptional adjustment</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund total</td><td>&pound;24,782</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Residential Bursary Fund</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Residential Support Scheme</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Residential funding total</td><td>&pound;0</td></tr>
<tr style=""height: 50px"">
<td colspan=""2"" style=""font-weight: bold""></td><td style=""vertical-align: bottom"">Total students</td><td style=""vertical-align: bottom"">Free meals<br>students</td><td style=""vertical-align: bottom"">Proportion of students<br>on free meals</td><td style=""vertical-align: bottom"">Total students in 2021/22 funded<br>for free meals</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals students</td><td>0</td><td>0</td><td>0.00%</td><td>0.00</td></tr>
<tr>
<td colspan=""3""></td><td>Free meals students</td><td style=""vertical-align: bottom"">Free meals rates</td><td style=""vertical-align: bottom"">Free meals funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals higher rate</td><td class=""greyBold""></td><td>0.00</td><td>&pound;358</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals lower rate</td><td class=""greyBold""></td><td>0.00</td><td>&pound;179</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals FTE rate </td><td class=""greyBold""></td><td>0.00</td><td>&pound;358</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Free meals administration</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Exceptional adjustment</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Total free meals funding</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Total student support funding</td><td>&pound;24,782</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Teachers' pension scheme grant</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 20%;"" scope=""col""></th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Payments made to Capita<br>for TPS, financial year<br>2019-2020 rebased at<br>16.4% for the full year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Annual payments<br>increased to 23.6%<br>equivalent for the full<br>year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">1.2% uplift for 2020-21</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">2.1% uplift for 2021-22</th><th style=""vertical-align: top; width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Revised annual cost</td><td>&pound;0</td><td>&pound;0</td><td>&pound;0</td><td>&pound;0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Teachers'&nbsp;pension&nbsp;scheme&nbsp;grant&nbsp;(difference&nbsp;between&nbsp;2019/2020&nbsp;payments&nbsp;at&nbsp;16.4%&nbsp;and revised&nbsp;annual&nbsp;cost)</td><td style=""font-weight: bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">16 to 19 Tuition funding</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>16 to 19 Tuition funding</td><td style=""font-weight: bold"">&pound;2,000</td></tr></tbody></table><div style='font-size: 10px; margin-top: 5px'>The values on your statement are shown rounded to various numbers of decimal places.<br> The calculation of your funding however is done using un-rounded values. This may result in some slight differences when you work through the calculation yourselves.</div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_TuitionFunding_With_MathsTop =
@"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>16 to 19 revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your revenue funding allocation statement.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>University Technical College Norfolk</td></tr>
<tr>
<td>UKPRN</td><td>10047244</td></tr>
<tr>
<td>Local authority</td><td>Norfolk</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021/22 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 50%;"">Core programme funding</td><td style=""width: 50%;"" class=""right"">&pound;803,165</td></tr>
<tr>
<td>Condition of funding adjustment</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;62,400</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;19,110</td></tr>
<tr>
<td>Care standards</td><td class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">High needs student</td><td style=""width: 50%;"" class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">Student financial support</td><td style=""width: 50%;"" class=""right"">&pound;24,782</td></tr>
<tr>
<td style=""width: 50%;"">Teachers' pension scheme grant</td><td style=""width: 50%;"" class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">Maths top up</td><td style=""width: 50%;"" class=""right"">&pound;500</td></tr>
<tr>
<td style=""width: 50%;"">16 to 19 Tuition funding</td><td style=""width: 50%;"" class=""right"">&pound;2,000</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total funding allocation</td><td class=""right"">&pound;909,457</td></tr></tbody></table><div style='font-weight: bold; font-size: 14px; margin-top: 5px'>Core programme funding</div>
<table id=""formulaTables"" class="""">


<tbody>

<tr>
<td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAADiSURBVGhD7dsxioQwGEDhmGJBEYLiDQIeYgsL72XvmfZyrsuEhYfjzJTj8L5Kfh6SvwuCYXvmr1iW5ftcWNc1pRQe6LquPJ35L35OlKKqqtu5jizoo4phGPYixlgGBxfaxQIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsKOSc67pOKZXBwYV2sQALsiALsiALsqDQ9/1FTmoBFmRBFmRBFmRBb1OM49i27X5PKYODC+1iARZkQRb0UUXOuWkav1oXFvRmxe7rRJimKcZ4i+7b3zPPc+nvee2/k8eeFdv2C7C2ttUtLoinAAAAAElFTkSuQmCC' /></td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Student&nbsp;numbers<br>for 2021/22</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;student&nbsp;numbers<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">143</td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;"">&nbsp;</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">National funding<br>rate per student<br>(dependent on<br>band)</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;breakdown&nbsp;of<br>funding&nbsp;by&nbsp;band&nbsp;table<br><br></td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;""></td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;594,181</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Retention&nbsp;factor</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">0.98524</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">-&pound;8,770</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;585,410</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Programme&nbsp;cost<br>weighting</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.31972</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;187,167</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;772,578</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Level 3<br>programme maths<br>and English<br>payment</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;Level&nbsp;3<br>programme&nbsp;maths&nbsp;and<br>English&nbsp;payment&nbsp;table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;5,500</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;778,078</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Disadvantage<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;distribution&nbsp;of<br>disadvantage&nbsp;funding<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;25,088</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;803,165</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Large<br>programme<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;large<br>programmes&nbsp;uplift<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;0</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;803,165</td></tr></tbody></table></td><td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAAEKSURBVGhD7dvNaoQwGEZhoyAoEongzYgrBS/MldfkpQliv6GhcJimsyjUMrzPIpiZg2Qgi8Efd11X9iM3DEM8fDLP87qucfKttm23bYuTlBCC2/c9zmhZFhsfRWqleZ7bV1bk8YM0FfQGhXPOxqIoksXnvjnP8+aVflFBKkgFqSAVpIJUkApSQSpIBakgFaSCVJAKUkEqSAWpoGThva+qqus63SsgFaSCVJAKUkEq6G0K59yLwv4+/IuVGhWkglSQClJBKkgF3VyEEJqm6fteVzdIBakgFaSCbi6893Vd66r1ExX0N4UryzIe0nEcNtoufDxwn2JbdRzHzM6RMk2TbebX7538/rdk2QcWtDy+yWdeMAAAAABJRU5ErkJggg==' /></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Area&nbsp;cost<br>allowance</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.00000</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;0</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;803,165</td></tr></tbody></table></td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""3"" scope=""col"">Student numbers</th>    </tr>
</thead>
<tbody>

<tr style=""width: 100%;"">
<td colspan=""2"" style=""width: 80%"">Autumn census</td><td style=""width: 20%; text-align: right"">143</td></tr>
<tr>
<td colspan=""2"">Exceptional variations to lagged student number</td><td style=""width: 15%; text-align: right"">0</td></tr>
<tr class=""greyBold"">
<td colspan=""2"" class=""bold"">Total student numbers for 2021/22</td><td style=""width: 20%; text-align: right"">143</td></tr>
<tr style=""width: 100%;"">
<td style=""width: 65%;"">Student number methodology used</td><td colspan=""2"" style=""width: 35%;"">Autumn census</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Breakdown of funding by band</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 30%"">Funding band<br><br></td><td>Student&nbsp;numbers<br>2019/20</td><td>Proportions&nbsp;used&nbsp;in<br>2021/22 allocation</td><td>Number&nbsp;of&nbsp;students<br>allocated in 2021/22</td><td>National&nbsp;funding&nbsp;rate</td><td style=""width: 20%"">Student&nbsp;funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">T Level band 9</td><td class=""greyBold""></td><td class=""greyBold""></td><td>0</td><td>&pound;6,108</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">T Level band 8</td><td class=""greyBold""></td><td class=""greyBold""></td><td>0</td><td>&pound;5,584</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">T Level band 7</td><td class=""greyBold""></td><td class=""greyBold""></td><td>0</td><td>&pound;5,061</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">T Level band 6</td><td class=""greyBold""></td><td class=""greyBold""></td><td>0</td><td>&pound;4,363</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total T Level bands</td><td></td><td></td><td>92</td><td></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>149</td><td>95.51%</td><td>137</td><td>&pound;4,188</td><td>&pound;572,011</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4 (Sum of bands 4a and 4b)</td><td>7</td><td>4.49%</td><td>6</td><td>&pound;3,455</td><td>&pound;22,170</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4a</td><td>7</td><td>4.49%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4b</td><td>0</td><td>0.00%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,827</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,234</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Band 1</td><td>Students</td><td>0</td><td>0.00%</td><td>0</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td>FTEs</td><td class=""right"">0.00</td><td class=""greyBold""></td><td>0.00</td><td>&pound;4,188</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total mainstream bands</td><td>156</td><td>100%</td><td>143</td><td></td><td>&pound;594,181</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total student funding</td><td></td><td></td><td>143</td><td></td><td>&pound;594,181</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Level 3 programme maths and English payment</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 42%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Instances per student</th><th style=""width: 14%;"" scope=""col"">Number of instances</th><th style=""width: 10%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 1 year programme</td><td>0.03846</td><td>5.50</td><td>&pound;375</td><td>&pound;2,062</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 2 year programme</td><td>0.03205</td><td>4.58</td><td>&pound;750</td><td>&pound;3,437</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""4"" class=""bold"">Level 3 programme maths and English payment funding total</td><td>&pound;5,500</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""5"" scope=""col"">Distribution of disadvantage funding</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 1</td></tr>
<tr style=""width: 100%;"">
<td colspan=""2"" rowspan=""2"">Economic deprivation funding</td><td>Block 1 factor</td><td>Funding including programme costs</td><td style=""width: 20%;"">Block 1 funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>1.01754</td><td>&pound;778,078</td><td>&pound;13,647</td></tr>
<tr>
<td colspan=""2"" rowspan=""2"">Care leavers</td><td>Number of qualifying students</td><td>Rate per qualifying student</td><td>Care leaver funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 1 funding</td><td>&pound;13,647</td></tr>
<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 2</td></tr>
<tr>
<td colspan=""4"" rowspan=""2"">Total 2021/22 instances attracting funding per student</td><td>Instances per Student</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0.16667</td></tr>
<tr>
<td colspan=""3"" class=""right"">Number of instances in each band</td><td>Block 2 funding rates</td><td>Block 2 funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Total funded instances for 2021/22</td><td>23.83</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the higher rate (bands 4 and 5)</td><td>23.83</td><td>&pound;480</td><td>&pound;11,440</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the lower rate (bands 2 and 3)</td><td>0.00</td><td>&pound;292</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Students attracting the FTE rate<br>(Band 1)</td><td>Students</td><td>0.00</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>0.00</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the T level rate</td><td>0.00</td><td>&pound;650</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 2 funding including T Levels</td><td>&pound;11,440</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""4"">Minimum top up if applicable</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total disadvantage funding (sum of block 1, block 2 and minimum top up)</td><td>&pound;25,088</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Large programme uplift (based on 2018/19 students)</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 40%;""></td><td style=""width: 15%;"">Students meeting large programme uplift criteria</td><td style=""width: 25%;"">Funding uplift per student per year</td><td style=""width: 20%;"">Total large programme uplift (2 years)</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 20% of national rate</td><td>0</td><td>&pound;838</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 10% of national rate</td><td>0</td><td>&pound;419</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td class=""bold"">Total large programme funding</td><td>0</td><td></td><td>&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Condition of funding (CoF)</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 25%"">Funding band<br><br></td><td>National funding rate<br>in 2019/20</td><td>Total students <br> (2019/20 S05)</td><td>National funding rate<br>applied to total students <br> (2019/20 S05)</td><td>Students not meeting<br>CoF (2019/20 S05)</td><td style=""width: 20%"">National funding rate applied to<br>CoF non-compliant students</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>&pound;4,000</td><td>149</td><td>&pound;596,000</td><td>2</td><td>&pound;8,000</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4</td><td>&pound;3,300</td><td>7</td><td>&pound;23,100</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>&pound;2,700</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>&pound;2,133</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"" style=""width: 20%"">Band 1</td><td>Students</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>&pound;4,000</td><td>0.00</td><td>&pound;0</td><td>0.00</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total</td><td>156</td><td>&pound;619,100</td><td>2</td><td>&pound;8,000</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">5% of national rate funding for total 2019/20 S05 students</td><td>&pound;30,955</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">Funding for non-compliant students less 5% of funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""6"" class=""bold"">Final condition of funding adjustment (at 50%)</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Advanced maths premium</caption>


<thead class=""whiteBackground top"">
    <tr>
<th style=""width: 24%;"" scope=""col""></th><th style=""width: 13%;"" scope=""col"">Baseline students</th><th style=""width: 14%;"" scope=""col"">Eligible students</th><th style=""width: 14%;"" scope=""col"">Eligible minus<br>baseline</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Advanced maths premium funding</td><td>98</td><td>54</td><td>0</td><td>&pound;600</td><td class=""bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">High value courses premium</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Qualifying students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>High value courses premium funding</td><td>156</td><td>&pound;400</td><td class=""bold"">&pound;62,400</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Industry placements funding</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Number&nbsp;of&nbsp;students&nbsp;minus&nbsp;final&nbsp;T<br>Level&nbsp;students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Industry&nbsp;placements:&nbsp;Capacity&nbsp;and<br>delivery&nbsp;funding&nbsp;(CDF)</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Industry&nbsp;placements:&nbsp;Capacity&nbsp;and&nbsp;delivery&nbsp;funding&nbsp;(CDF)</td><td>91</td><td>&pound;210</td><td class=""bold"">&pound;19,110</td></tr>
<tr>
<td></td><td>Number of estimated T<br>Level students</td><td>Rate</td><td>Industry placements: T Level<br>funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Industry placements: T Level funding</td><td>92</td><td>&pound;275</td><td>&pound;19,110</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Industry placements funding total</td><td>&pound;19,110</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Care standards funding</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td>Eligible students</td><td style=""width: 15%;"">Rate</td><td style=""width: 20%;"" class=""right"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Care standards student funding</td><td>0</td><td>&pound;817</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Care standards institution lump sum funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total care standards funding</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">High needs funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">16-19 students</td><td style=""width: 15%"">19-24 students</td><td style=""width: 15%"">Total students</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>High needs element 2 for 2021/22</td><td>0</td><td>0</td><td>0</td><td>&pound;6,000</td><td style=""font-weight: bold"">&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">Student financial support funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">Number of funded students</td><td style=""width: 15%"">Instances per student</td><td style=""width: 15%"">Number of instances</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 1: Financial disadvantage</td><td>143</td><td>0.10641</td><td>15.22</td><td>&pound;243</td><td>&pound;3,698</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 2a: Student costs - travel</td><td>143</td><td>0.40171</td><td>57.44</td><td>&pound;468</td><td>&pound;26,884</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Element 2b: Student costs - industry placements</td><td>55</td><td>&pound;48</td><td>&pound;2,640</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Bursary adjustment in respect of free meals</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund</td><td>&pound;33,222</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">2019 to 2020 discretionary bursary fund - baseline for transition</td><td>&pound;16,521</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition lower limit</td><td>&pound;8,261</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition upper limit</td><td>&pound;24,782</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Exceptional adjustment</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund total</td><td>&pound;24,782</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Residential Bursary Fund</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Residential Support Scheme</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Residential funding total</td><td>&pound;0</td></tr>
<tr style=""height: 50px"">
<td colspan=""2"" style=""font-weight: bold""></td><td style=""vertical-align: bottom"">Total students</td><td style=""vertical-align: bottom"">Free meals<br>students</td><td style=""vertical-align: bottom"">Proportion of students<br>on free meals</td><td style=""vertical-align: bottom"">Total students in 2021/22 funded<br>for free meals</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals students</td><td>0</td><td>0</td><td>0.00%</td><td>0.00</td></tr>
<tr>
<td colspan=""3""></td><td>Free meals students</td><td style=""vertical-align: bottom"">Free meals rates</td><td style=""vertical-align: bottom"">Free meals funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals higher rate</td><td class=""greyBold""></td><td>0.00</td><td>&pound;358</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals lower rate</td><td class=""greyBold""></td><td>0.00</td><td>&pound;179</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals FTE rate </td><td class=""greyBold""></td><td>0.00</td><td>&pound;358</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Free meals administration</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Exceptional adjustment</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Total free meals funding</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Total student support funding</td><td>&pound;24,782</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Teachers' pension scheme grant</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 20%;"" scope=""col""></th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Payments made to Capita<br>for TPS, financial year<br>2019-2020 rebased at<br>16.4% for the full year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Annual payments<br>increased to 23.6%<br>equivalent for the full<br>year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">1.2% uplift for 2020-21</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">2.1% uplift for 2021-22</th><th style=""vertical-align: top; width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Revised annual cost</td><td>&pound;0</td><td>&pound;0</td><td>&pound;0</td><td>&pound;0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Teachers'&nbsp;pension&nbsp;scheme&nbsp;grant&nbsp;(difference&nbsp;between&nbsp;2019/2020&nbsp;payments&nbsp;at&nbsp;16.4%&nbsp;and revised&nbsp;annual&nbsp;cost)</td><td style=""font-weight: bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Maths top up</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Maths top up funding</td><td style=""font-weight: bold"">&pound;500</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">16 to 19 Tuition funding</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>16 to 19 Tuition funding</td><td style=""font-weight: bold"">&pound;2,000</td></tr></tbody></table><div style='font-size: 10px; margin-top: 5px'>The values on your statement are shown rounded to various numbers of decimal places.<br> The calculation of your funding however is done using un-rounded values. This may result in some slight differences when you work through the calculation yourselves.</div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_SixthForm_withFreeMealsLine =
       @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD//gA7Q1JFQVRPUjogZ2QtanBlZyB2MS4wICh1c2luZyBJSkcgSlBFRyB2NjIpLCBxdWFsaXR5ID0gODIK/9sAQwAGBAQFBAQGBQUFBgYGBwkOCQkICAkSDQ0KDhUSFhYVEhQUFxohHBcYHxkUFB0nHR8iIyUlJRYcKSwoJCshJCUk/9sAQwEGBgYJCAkRCQkRJBgUGCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQkJCQk/8AAEQgAkQEsAwEiAAIRAQMRAf/EAB8AAAEFAQEBAQEBAAAAAAAAAAABAgMEBQYHCAkKC//EALUQAAIBAwMCBAMFBQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYnKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX29/j5+v/EAB8BAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKC//EALURAAIBAgQEAwQHBQQEAAECdwABAgMRBAUhMQYSQVEHYXETIjKBCBRCkaGxwQkjM1LwFWJy0QoWJDThJfEXGBkaJicoKSo1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoKDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uLj5OXm5+jp6vLz9PX29/j5+v/aAAwDAQACEQMRAD8A9B/Zcdm8L68WYn/ibP1P/TNK9orxX9lv/kV9e/7Cz/8AotK9qrfE/wARmGG/hRAnA9a8Wu/2gLXSPEj2mqXUEFn9pjR90LL9jXGHWRzglxjdgL/EBzg49nflSMke461+f91f/aPHGsW3iS7MOoYvTbarfXLq8cm1lCS4U7sFGQBQnzEnkYFYG591Q+K9LvNBttcsJzfWd2F+ymAZa4LHChQcck+uAOSSACa4D4s6hd3Y8PCXRdWgMOoxzDZImGYdFBWUZOf4Rlj/AAg848o/Zc8RavrngfXvD9tdRJc6Iwv9OeRVwjENlWJP3TyOnGTz0FLqXjrx9qUqXs+ialHbz6rDqtst79ntysYQAbFllJ7DAGAeTkE4oA+lYvELfabeK80u9sI7k7IpZzGVL84Q7WJUnHGeM8ZyQDR8Vave+FWXXQkt3pEa7b+BF3SW6Z/16AcsF/jX+7yOVIbzn4Uav4v8UavLpniO2v7G2tbiTVW+2QqDcb5cwrG+9gUUhiduAvyryOa0/wBoOTxavh+0Tw9rVnpVhJKI9RllwGWMsBksSMRgbt20ZPA6ZoA9I0LxHpHiazN7o2o22oWyuYzLbuHUMOoyPqPzFaNfnl4R8UXHwu+JNjJ4c1mS8SK6EF23kNHDOpfay+W2Djb6gEHpjGT+hgPHagBaKPyooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKPyoA8V/Zb/5FfXv+ws//otK9okkWGN5JGCogLMx4AA714v+y3/yK+vf9hZ//RaV7SwDKQwyCMEHvW+J/iMwwv8ACieBal4h+IXiLxhqV9Dd3uneHZozbeHbVB5Mt5d4ASTbgM0Y+Z2LjZt7HrXg37UFxov/AAtK9stGRAbcBr10OQ1y/wA0gHsCeR/eLfQfRHjD4l2vhnUPF8wt7xvE8EciWZmtXEVnYpGpMqsw27S+49cu+1egBHyV4ZtdU/eeNEsrTWTBqUNq9nfwfaReSzLI2Cp++fkOcc5YEVgbmx8O/E2r/DjQ9R8RaDqKte6mq6VFbxIHKO2WLSKQcEBRs/vFup2stdFpvwm0q6e8h8Tave3WtwwCWcW93EEhIKgxNkO5Kg4J2gAjABxmu58VeFNI8G/E/wAMavd2mleFtM1m3tpH04rI5W6WSN8FR8mEk2Z5X5Q3TIq5YkW0t8qyX8NyttNLKt4wIGydd5A37Qd0mAx6cggc0AcN4Xn1H4Z67Da2OtahfeDde8y2llt3aC5j8olmWNhysgOdu3AfJGA2QuX8S/iL4wsriyiuhdi4t5P3Gq3qoZby3ADRK8YynAbcT824kEngVt/E/WBJ4AbU7cxWk7azbyw+U0bM84jkdnBQ8FNwBJHJYd812/xA0bwf8N/hhp0/inQLrxHProt3hsGcQ/YZ/Ky4jlVd6oC2AnzY4HSgDlvhHolr43tj4/uNHsbaTw/dRIAkhCXNyTGI2fqyxrkOxJJOX5PG36C8A/EXWtX8b+IPBviPTIbS70vD21zCjRpexZ++EYtgcrj5jnJ9CK+Q/hFql7oXjibTyt/a6TPMDfaM7sZrmEE5Tbsw7IjMxBC7lDDqQK+zPBVxpt9qV3p1vfWut2ulLBcafel1mkgSZXHlmTkkqF+91KuucnJIB29FFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQB4r+y3/yK+vf9hZ//RaV7VXiv7Lf/Ir69/2Fn/8ARaV7VW+J/iMwwv8ACieEfGHV7/xt8Q7L4PxXEekaXrFqJrzUDBulmKbpPKjJIHIRc8d/wOhr37OOkQ+D9J03wXePpGraJcNfWd853NNcFQMysB3Kp0HAXGK3vjJ8KtN8f2VrqUsOqNqOlbpLf+y50huH4+6rv8o557ZIHNfPlncfEvxPat8Ovh54Y1jwvoiSsLu4vncTEk4YyzEAKMfwIMnH8WTWBudR450rX/i6bjwf4q8QeBE13TF36V/Z12TcTzjIdGQk4DhRlcAg7SM4IrDvvEt94SvLhfE/h3U453C6db6jeBoJZrYbGeSTAVCWMYw4Yv2IPBrm/iX8Dbn4Tah4QSw1iMX99Kzy6xdTCC3t50KlQM/dA65OSccAdK9/8e3cPiSHQJl1Wy1dIwkfmaVdKRHdMMNKSG4TphiSq87lbIwAeJ6Z4X1DxrcJ438TSQR+CNCZpbK1muXjF45fIj8y52ltzY3uSeBhR0A9tsdN1z446PY3Oraz4ZtNHguIbxIdEIu7mOZMMFaZiUQg8HapyO+DivNf2rr+61vSPDVra+IdC1CO2I+16ba3amaW5ICh1QHJX7wGMEbj+FfVvgN8QvhHJbeLvhrf3cz+Sj3emBg80Zxlkx92dAcjpu9AetAHVfFXTG8B+Nbrx9bXEVlc2jNPb6fPC09tqHmosU8nyn92+wDJ43bcY6lvTvhwtpaSS2/h7QYtO8Pyb3ikELIzEFQDvLHcCTIAoA2hB2Irzzwlr/iD4/HTLLxV4U1XSLHSp/OvwFCWd84HCMsg38EA7Rux1JBwR9AAADAGBQAtFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQB4r+y3/AMivr3/YWf8A9FpXtVeK/st/8ivr3/YWf/0Wle1Vvif4jMML/CiFFJuXPUUbl/vD86wNzlPiT8NNC+Kfh8aLrqzCNJBNDNA+2SGQAjcCQR0JGCCK8Nuf2HtMaQm28a3kceeFksVc/mHH8q+nQwPQiloA8G8DfsheFPCet2msX+rX+sT2cqzRROixRb1OVLKMk4IBxnHrmveaje4hidY3ljR3+6rMAW+gqSgAooooAKKKKACiiigAoopks8UCb5ZEjXOMu2B+tAD6KQMGUMpBB5BHeloAKKKKACiiigAooqKS6gidUkniR26KzAE0AS0UZzRQAUUVSvtc0rS2Vb/UrK0ZugnnVCfzNAF2iorW8tr2FZrW4iuIm6PE4ZT+IpZLiGJlSSWNGc4UMwBb6etAElFFMlljhQvI6og6sxwBQA+ionu7eONZXniWNvuuXAB+hpZLmGJkWSaNC/ChmA3fT1oAkooooAKKKKAPFf2W/wDkV9e/7Cz/APotK9qrxX9lv/kV9e/7Cz/+i0r2qt8T/EZhhf4UT5K8Y+C7L4jftYar4a1W8v7eyltY5SbSUI4K2sZGCQR19q7/AP4Y88C/9BvxX/4GR/8AxqvPvGNz4ttP2tNVl8EWNje62LWMRw3rbYin2SPcSdy84969B/4SD9pn/oUvB/8A3+/+31gbnefDD4QaH8J01FNFvdVuhqBjMv26ZZNuzdjbtVcfeOfwqfWvjJ8PvDuoNp2p+LdKt7tDteLzd5jPo23O0+xxXmnxL+IPxF8KfA/VL/xZa2GkeIry9XT7Y6c+VSJ1BLg7mw2FkHXjg1pfCT9njwTp/gfTbnXtDtNX1XULZLm5nu1L7WdQ2xQeABnGRyetAHNfGTVtP1z40/CK/wBLvba+tJrsFJ7eQOjjzo+hHFfQL+INHj1ZNGfVbBdTkTelkZ0E7LgncEzuIwDzjsa+U/Gfwxsfhp+0L4Di0Uyx6NqOoxXEFqzllt5BKokVc9j8h9e3YV22uf8AJ5Wgf9ghv/RU9AH0DdXVvY20t1dTxQW8KGSSWVgqRqBksSeAAO9Q6Zq2n61Zpe6XfWt/avkJPbSrJG2Dg4ZSQcEEVz/xX/5Jf4t/7A93/wCiWr50tfHF94I/Y/0p9Mne3vdTvJ7BJkOGjVppWcg9jtQjPbNAH0FrXxm+Hnh6/fT9T8XaVBdRna8Ql3lD6Ntzg+xrpNE1/SfEtguoaLqVpqNo5wJraUSLn0yO/tXlfw0/Zz8DaL4PsF1vQbTVtVuYElu7i7XefMYAlVB4UDOBjnjJqT4e/BG/+GPxI1TVvD2pwReE9QiIbSnd2eN8AggkYOGyASc7WxzQB3fij4k+D/BUqw+IfEWnadMw3CGWUeYR67Blse+KPC/xK8HeNZXh8PeItP1GZBuaGKX94B67Dg498V45ovwn8FeEdZ1nX/jFr3h3VdZ1G4M0X2y5wkcZ7CNyMnt0IAAxivP/ABHqPw7tfjv4FuPhfJDDuvoor82SssB3SKuFzxyrMDt4xjvmgD0z43/Fj+x/HPgKy8PeL7SG1fVGh1mO2u42CIJYQRNydgAMnXHf0rrvi1Y+Dfih8PXtbvxppmn6R9sjLalDcxSRLIuSE3btuTnpnNeRftA/DDwho/xG8Aix0dYR4j1iQap+/lP2ndNDnOWO3PmP93HX6V0P7SHgnw/4B+BM+leG9OGn2TarDOYhK8mXIIJy5J6KO/agD3jRY7TSvDtjFHdxy2lraRqtyWAV41QAPnpggZrlpPjr8M4r02TeNNH84Nt4mymf98fL+teNfGnV9V8QWXw1+F+l3b2kevWtq946n7yEKig+qjDsR3wK9Ttf2cPhjb6Aujt4Ytph5exruRm+0scfe8zOQe/GB7YoA9Htry2vbWO7tZ4p7eRQ6SxMGR19QRwRXPyfEzwVFo8utN4p0f8As6KUwPci6QoJAAdmQeWwQcDmvE/gFc6j8Pvih4u+E9zeS3Wm2sbXliZDkoPlPHpuSRSR0yvvXK/sv/CnRfHkOsav4mhOpWGnXzRWlhK58lZWUGSRlB5JAjHPHHOeMAH0p4X+J/gvxpcta+H/ABHp+oXKjcYI5MSY7kKcEj3ArqK+W/jv4G0H4YeNfAXiXwhYR6PdTamIZY7XKxuAyY+XoMhmBx1Br6koAp61cz2Oj311axedcQW8kkcf99gpIH4kV8nfCP4SaH8b/DGs+MvGPiXVJtaa6ljeRJ1UWgChgzBgeOc44AAwOlfTHxB8faN8N/DVxr+tyMIIyEjijGZJ5DnCKPU4P0AJ7V8b+JvDvi+ysr/x3/YupeGfA/iS7Q3unWFx+8+zswIZlI4ViW25AGWxgAjIB75+yX4k1rXvAuo2uq3k2oQaZfta2d3KSxePaDtyeSBnj0DAdq9xryrw948+HXw80fwV4d8PLM9j4iPl6abZA+9iyhnlJIIO5+e4IIxxivVaAPNP2hvH+ofDr4aXmqaS3l6hcSpZwTYz5LPkl8eoVWx74rg/h9+zF4V8R+GNP8Q+MrvU9d1fVreO8mme7ZVXzFDAAjk4BGSSc+1ewfEfwDpvxL8JXfhzU3eKOfDxzxjLQyKcq4Hf3HcEivCdO8EftC/CS3Fh4Y1HT/E2jQf6m2kKkovoFk2sv+6rkelAFLxd4E1f9m7xfoviLwJc6te6BfXHk32mNulAAwSDtHIK7tpIypHU5rpP2jG3fFX4PMO+rZ/8j21M0T9qTVdA1m30b4o+Dbrw9LMdovIkcRjnG4o3JX1Ks30qD9pnVbKy+Ifwl1a4uY0sYdQa5kuM5RYhNbMXyO2OaAPoi/v7TS7Oa+v7mG1tYEMks0zhEjUdSSeAK4H4gz+Dfin8M9Ysv+Ev0620aV4o7jVIZkeKBllRwCxO3JIUdf4hXjcnjF/2pviGfCUWqto3g+wU3TWwO241IKQM+ncHB+6OcE9PRv2hNB0zwz+zvrmk6PZxWVjbJapFDEMBR9pi/M9yTyaAOF/aZ02y0b9n/wAHabpt+mo2Vpd2kMF2hBFxGttKFcYJGCADxWp+0N/yUP4N/wDYTH/o22rl/jj/AMmt/Dr62H/pJJXUftD/APJRPg3/ANhMf+jbagD3TxF4r0HwjZC91/V7LTLcnCvcyhN59FB5Y+wrF8P/ABf8A+Kb5LDR/Fel3V25wkHm7Hc+ihsFj9K8C+O4sdM+Pmk6r8Q9Ovb7wYbVUgCBjEG2nIIBGcPyy9SMdRxWz4g8AfBn4s6fbRfD3XNB0HXo5UeCS3JikYZ5UwkqSe4IGQR1oA+lKKqaRBd2ulWcF9cLc3cUCJPOq7RLIFAZgO2Tk496t0AeK/st/wDIr69/2Fn/APRaV7VXiv7Lf/Ir69/2Fn/9FpXtVb4n+IzDC/wony3e6/pXhr9snUtR1rUbXTrNLNVae5kCICbSMAZPrXuH/C6Phv8A9Dx4e/8AA6P/ABp3iH4O+AvFmrTaxrnhmyvr+faJJ5C25tqhR0PYAD8Kzf8Ahnr4Wf8AQl6d+b//ABVYG5ynx3bSPjF8KNWg8HarZa5eaPLFftFYzLK2BuBGF7lS5A77avfCD48+Dde8D6ZHquvadpOp2NslvdW97OsJ3IoXcu4gMDjPHTOK7/wl8PPC3gT7V/wjWjW+mC72+eIS37zbnbnJPTcfzrB174B/DTxJqUmpaj4Ts3upTud4XkhDnuSI2AJ98UAeEePPibpfxF/aJ8Bx6FKbnS9K1CGBLoAhJpWlUuVz1UAIM9/piuj+Jes2fgX9qjwx4j12Q2ukzaaYftTA7EJEqc/QsufQHNez2/wm8D2culS23hqwgk0d/MsWjQr5D5BLDB5OQDk5PFaHi3wP4c8dWC2HiTSLbUoEbcglHzRn1VhhlP0IoA80+Nnxp8HwfD3WNL0jWrHWtU1WzltILawmE5AdSGdtmdoVSTz6V5PL4QvfFf7HmjS6fC88+k309+0aDLNGJplfA9g+76Ka+hvD/wAD/h34XFz/AGV4Xs4WuoXt5XdnlcxupVlDOxKggkHGK6fw94a0nwppMWj6JYxWOnwljHbx52ruJY9c9SSaAPPvhv8AHvwR4i8G2F3qHiLTNMv4bdEu7a8uFhdJFUBiAxG5TjIIz19eK5zw98ZvEvxD8e+JovCCQ3HhTR9OleKc2533FyIzsCsfV8kDHIX3rtNY/Z8+GGuX73974RsvPkO5jA8kKsfXajBf0rr/AA94Y0XwnpqaZoWmWunWaHIigQKCfU+p9zzQB8s/s7/8Kv1rTtV1j4h3uk3fiiS8dpTrsy/6sgEMokO1iTuyeSPYVX+KHjzwNqXxc8Aw+FRp9to2i6jG1ze20Sw2pZpYy2CAAQqqCW6c19A658Afhp4i1OXU9R8KWj3crb5HikkhDseSSqMASfXHNXNU+C/w/wBZ0O00O78L2H9nWbmWCGENFsYjBO5CGJOBnJ5wM9KAPJ/2m9XsLbxb8Jtbe6iOmRai1010h3R+V5ls28EdRtGeOoq5+0/4m0XxX8D5b/QtTtNStBqcEZmtpA6hhklcjvgj869g1f4f+Ftf0Gz0DVdEs7zTLJEjtoJl3CEKu1dp6jAGM5qkvwl8Dp4bfwyvh20GjPcfams8tsMuAN3XOcAd6APCfjRYaj4X/wCFX/E+ztJLq10a1tIbxUH3VAVlz6Bsuuexx617LbfHn4bXOgjWv+Et0yKDZvMMkoWdTj7vlffz7AV2n9k2J0waW1pDJYiIQfZ5FDIYwMBSD1GOOa4CT9nH4VS3pvG8H2nmFt21ZpRHn/cD7ce2MUAeafAgXnxF+L/jH4pm1lt9JliayszIMGT7gH4hIxn0LCtL9jP/AJEvxH/2GX/9FpXvOn6XZaTYRWGn2kFpaQrsjggQIiD0AHArO8LeC/D/AIKtZ7Tw9pcOnQXEpmlSLOHfAG45J5wBQB4r+1r/AMfPw+/7DH9Y6+haxPEngrw/4vaybXdLgvzYS+dbebn90/HzDB9h+VbdAHzx+2Ppt6/hzw3rKW73OnabqBa8iA4wwG0t6D5Suf8AaHrXfD4yfCvxN4RknvfEejf2bc25S4srqVVlCkco0R+YntgA+1eh3llbajay2l5bxXNvMpSSKVAyOp6gg8EV52f2cPhU179rPg+08zdu2iaUR5/3N+3HtjFAHyr4A1fQPCPxV0vxLcW2sv4Eg1G4h0u6uVOyFiOGPY7dysQOeAeoxX3hbzw3UEc8EqSxSKHSRGyrKRkEEdRisbVfAnhnWvD6eHb7Q7GXSI9pjsxEFjj29NoXG3Ht6n1q9oeh6f4b0uDStKtha2NuCsUIYsEGc4GSTjnpQB5b+03N4x0zwRba74P1G+tJNMuRLeraOVLwEYJOOoU4z7EntXQeAvjf4K8c6JbXkWu6fZXhjBuLK7nWKWF8cjDEbhnuODXfsiyKUdQysMEEZBFeZ67+zb8L/EN495c+GYreaQ7nNnNJApP+6jBR+AoA87/aq8d+E/EfhK18KaPd2mua/cX0TQR2LCdocZB5XOCc7dvU59qwvjL4VKXXwL8K68nm8RafeoHPzfNao65H4jIr3jwb8FvAXgG5F5oPh63gvAMC5lZppV/3Wcnb+GK3Nd8FeH/EupaZqWr6XBeXmkyedZSyZzA+VbIwfVVPPpQB4h+0F8M5vCX9lfEvwFax2F94dCLcQW0eFa3XhW2jqFHysO6n2q78WfHunfEn9l7V/EOnMAJltVnhzkwSi5i3IfoenqCD3r3m4t4rqCS3njWWKVSjo4yrKRggjuK5Oz+EPgaw0O/0G18O2sWl6iyPdWgZ/LlZCCpI3dQQOnoKAPn344/8mt/Dr62H/pJJXUftD/8AJRPg3/2Ex/6Ntq9m1b4c+FNd8P2Ph3UtFtrrSbDZ9mtHLbItilVxg54UkVPrfgjw94jvdLvtW0uC7udJk82ykfOYGypyuD6ovX0oA858WfGnTdA+J03gTxzo9la+H7m3WW21G6/eRTEgcOpXAXdvXPYgZ615x8fNG+B6+DrvU/Dt1odv4hUqbNNFuFPmNuGQ0aEqBjJzgY9ex+jfFfgbw344s1s/EejWmpxISU85PmjJ67WHzL+BrmdG/Z9+GOgXyX1l4Rs/PjO5DO8k4U+oWRmH6UAX/g1caxdfC7w3PrzTNqL2SmRps72GTsLZ5yU25zzXZ0AYGBwKWgDxX9lv/kV9e/7Cz/8AotK9qrxX9lv/AJFfXv8AsLP/AOi0r2qt8T/EZhhf4UThPE/xx+HvgzWp9E17xHHZahbhTJA1tM5UMoYcqhHIIPWsr/hpn4S/9DfD/wCAdx/8bry+bR9N139szUrLVdPtNQtGslZoLqFZYyRaIQdrAivef+FXeA/+hK8M/wDgsg/+JrA3LPg7x14d+IGnSal4a1JdQtIpTA8qxumHABIw4B6MPzrerz/xV4z8G/BODSrT+xPsUGsXZhii0q0iRPN+UbnAKjoRzyeK7+gBaK4fwn8XdC8Zt4mXTbbUEbw1I0V2J41Xew3/AHMMc/6tuuO1cR/w1h4VudGtLrStF1vUtTu3dYtKt4lacKpxvfaSAD26njpQB7fRXkXw9/aP0Pxr4mHhfUNG1Pw7rMmfJt79QBKQM7c8ENgE4IGfWum+J3xf8M/CnT4rjXJpZLm4z9msrdQ002OpAJACj1JoA7Y8CuQ8PfFjwp4p0LWNd0q+lmsNF8z7bI0DoY9ib2wCMthR2rzvTP2q9JN1br4k8IeIvDljcsFiv7qEmHJ6EnA4+ma5b9mW7063+GPxEu9TgN3pkdzcS3MScmaEQZdRyOq5HUdaAPoLwd4y0bx5oMOu6DcPc6fOzokjRtGSVYqeGAPUGtuvIvDPxO8E+E/goPGfh/Qb6x8OW8rKliir5oYzbCQC5HLHP3qztU/am0RWhi8N+Gdd8Szm3jnuFsYcrbb1DbGYZ+YZwcDAORmgD26ivO/hR8bfD/xYS7gsIbrT9TsgDcWN2AHVc43KR1GeD0IPUcioPih8efDXwyvYdJmhu9W1qcBk06xUM4B6Fiemew5PtQB6XRXhtp+1doEEd2niPw1r3h68hgaeG3u4gDchRnahbb83pkAH1zxXq/gvxVa+N/C+neIrGGaG2v4vNjjmADqMkc4JHb1oA26yPFnivSPBOg3Ou65c/ZdPttvmSbSxGWCgADJJyR0rXr56/aRvJfGvjHwb8KrGRv8AiY3S3l/sP3IgSBn6KJWx7CgD2PwP4+8PfEXR31fw3em7tElaBmaNo2VwASCrAHoQfxroq+bvhIU+E/x+8T/Dxv3OlayPtumofuqQC6qv/AC6/WMV9B61rWn+HNKutW1W6itLG1QyTTSHARR/P6dSaAL1FeBt+1tplzNNPpPgfxPqekwMRJqEUI2gDqccgevJH4V13gz9oHwp498W23hvQ472aW4szeC4ZFVEA6owzuDDpjGPfHNAHp1Fea/Ez48eG/htfxaO8F5rGuTAMmnaegeQA9Nx7Z7Dk+2Kw/CP7Tug634ig8PeIND1bwrf3JCwf2im1HY8AEnBUk9MjHvQB7NRXD/EP4t6J8NtV0Cw1mG4xrczQx3CbRHb7SgLSFiMKN4PGeAa891L9rPS4Gmu9L8F+JNT0WFiraokOyIgHlhkEY+pB+lAHvVFc54D8e6J8RvDcOv6FMz2shKOkg2vC46o47EZH4EHvXm3iP8Aam8P2WuTaN4Y0HWPFlxbkrLJp0eYwRwdp5LD3xj0JoA9soryv4e/tDeHPHuoXGjHT9S0nXoY2caZeRhZJtoyVjOQC2OxwfwzXQ/DP4raB8VdPvbzQ472D7DP5E8F5GqSKcZBwGPB5HXqpoA7OiuQ+JXxP0L4WaRbanriXcqXNwLaGG0jDyO5BPAJHAA9e4rqrWc3NtFOYpITIgfy5MBkyM4OMjIoAlooooAKKKKAPFf2W/8AkV9e/wCws/8A6LSvaq8e/Zp02+0zw3rkd/ZXNo76o7qs8TRll8tOQCORXsNbYj+IzDDfwkfI/jHwpe+M/wBrTVdH0/xBfeH7iS1jcXtkSJFC2kZIGGU4PTrXoP8Awzh4r/6LX4w/7+Sf/HaybHTr0ftm396bS4FqbIAT+WfLJ+yIPvdOtfRtYm58y/tHaNceHtB+GGk3WpXGqT2mpCJ724JMk5Gz5myScn6mvpqvEP2q/COt6/4V0fWdBs5L650K+F09vGhZjGRywUcnBVcgdiT2rNt/2r01+xXTvDngjX7vxPMmxLQxqYUlPGS4OdoPPKjjrigDJ+AH/Hz8af8Ar9k/nc1pfsY6BYW3gDUNbWBDf3d+8LzEfMI0VNqg+mST+NZP7N2ja1pWn/FSDWreYXzPtkcocTSBbgMVOPmBbuPUV137IlldWHwpeG7tpreT+0pzslQo2Nqc4NAGH+0NBFbfGX4T3sMax3MuoiJ5VGGZRPDgE+g3t+ZqjY2UPjn9sHVE1hBcW/h+yElpDKMqCqR449mlZ/ritn9oawu7r4q/CeW3tZ5o4dT3SPHGWEY86DliOnQ9fSqfxg8N+J/hz8VbX4ueFNLl1a1kiEOq2cKkvgKEJIAJ2lQvODhlyeKAPePEXh/TvE+iXmjapbR3FndxNFJG4zwR1HoR1B7GvmT4BW32L4I/FW1DB/JW9j3Dvi1YZ/Sun1D9qOTxbpz6N4C8IeILnxFdp5MfnwqI7Zm43kqTnb15wOOSK574BaRqVj8EfiZa3lncx3Lx3aqjxsGkP2UjjI5yaAM21/5MkuP+vk/+lor3r4EaBYaB8J/DUdjbpGbqxiu5mAwZJZFDMxPfk4+gA7V4fbaXfj9jCey+w3X2v7ST5HlN5n/H6D93GenNfQnwnikg+GPhSKVGjkTSbVWRhgqREuQRQB5D4egisf2yPEEdqiwpNpO+RUGAzGOFiT7k8/Wqv7NltB4u+JvxB8Z6mgn1KK98i3aQZMCO0mcZ6fKiL9AR3rV0mwu1/bB1m8NrOLVtIVROYzsJ8qHjd07Gucu/7e/Zs+Kmu6+uh3mreDPELmaR7Rcm3csWAPYFSzgA4BU9cjgA9H/ag8Oafrfwf1i6uoo/tGmhLq2lI+aNg6ggH3UkY+npWv8As+/8kZ8Kf9ef/szV4j8YvjNqXxf8EajpnhHw7qtpoVtH9r1TUr+MRrsQgrGuCRktt75OOmMmvb/2fgR8GfCgIIP2IH/x5qAO/mlSCJ5ZXVI0UszMcBQOpNfGHg743eHLX41eJPiF4jtdVu1mDW2mJZwLJ5ceQoJ3MuD5agcf32r6A/aR8T3vh34W6jBpkFxPqGq4sIlgjLMquD5jcdBsDDPqRWn8DPBI8B/DHRtLli8u8ki+1XYIwfOk+Yg+6jC/8BoA+ZvjT8a/D3izxZ4X8Y+E7LWLTVtFl/ete26xrKgcMgyrt33gj0avRv2qPFi+Ivh14Oi02crp/iO7jnZx3j2AqD+Lg49V9q9s+JPhGLx14F1rw84G69tmWIn+GUfNG34MFNfLvhfwjr/xQ+Bl14RazuYPEPhK++02EdwhjM8Lhsxgt3zvx9FHegD630HQdP8ADei2mj6ZbR29naRLFHGowAAP1J6k9zXzp4U8O2Hhj9sHVbPTYkhtpbGS5EUYwqNJEjMAO3zEnHvWjof7V39laRDpPijwd4iHii3QQvBDAAs8gGM/MQy5IyRtOO2a5b4Qy+JL/wDabn1XxVaGy1TUdOlvHtDnNtGyqI0YdiEC8Hn15oAw/hR8TrnQ/Gni3xZceBtb8U6rf3jKLmyiL/ZE3MSmQpxn5R9FAra+M/xEv/ix4TOlj4S+K7TUYJUmtL17R2MJBG4cJnBXIx64Pats3Gv/ALNHxC8QX58P3useCdfn+1ebZrua0fJOD2BG4jBxuG0g8EVa8RfH7xH8U47fw58JtB1y0vriZPO1S6hVFtkByem5QD3JPTgAk0Ac18Zo7rxdpvwTtvEMNxDdagRb3qTKUk3M1uj5B5BPJ/GvqyLTLK205dOitYEs0i8lbdUAjCYxt29MY4xXgHx60bU08VfCCFjdanNZ36rc3YjJLsJLfLtgYGSCa+iD0oA+PfhlrNx4Z+A3xVm092hMN6YYdh/1fmbYyR6HB6+1e1fsxeGdP0H4RaPd20EYutTVru5mx80jFiFBPoFAAH19a88+AfgeXxR8PPiP4b1KCezGp3zxxvLGVwdvyuAeoDAH8Kq/Dv4wav8AAXSm8DfELwvrHlWEjiyvLOMOsiFicAsQGXJJBB74IGKAPcvEfwl8N+JvGWk+MbpLqDWNKZTFLbSBBJtbIEgwdw6j6EivKPDsH/Cpv2ndQ0j/AFOjeM4DcW46Ks+S2PrvEgA/6aLUega14t+PXxU0bX7Sw1fw94L0M+bmZ2j+2uDuwQDhskKCBkBQecmum/aj8Nzz+DrHxlpnyar4VvI76Jx18vcoYfgQjfRTQBjeOIv+Fo/tJaB4Yx5uleE4P7RvR1UykqwU+uT5I/Fq+gq8T/Zj0i6v9H1z4h6rHt1LxXfSXAB/ggViFUZ7bi34Ba9soAKKKKACiiigAxiiiigAooooAKTaoOQBS0UAFFFFABRRRQAgVR0AFLgUUUAFFFFABivDPE3if4p/DL4galqE+j6j4y8HX3zW8VlGDLY8524Vc8cjngjHIORXudHWgD5p8e+MvHfxz0seC/C3gLWtD0+8kT7fqGrRGFVQMGx0xjIBOCScYAr6B8KeH7fwp4a0vQrUlodPtY7ZWPVtqgbj7nGfxrU6UtABRRRQAVxnxb0zxjqfg2dfAmpmw1yB1miwF/fqM7o8sCBnOQfUDkV2dFAHgth+0L4q07T0ste+E3it9ciQRv8AZrVmhmcDG4Nt4BPpu+pqz8EvA3iy98ca78UPHNiNN1LVY/s9pYH70EPy9R24RFAPPUkDNe4YHpS0ABAPUUgUDoAPpS0UAFFFFABSEBuoB+tLRQAAAdBXzp8VNS+JfxYvbn4caf4IvdH0l9Q8u51qct5M9vHJkOCVAwcBsAsTgAV9F0UAUNB0W08O6JY6PYJstbGBLeJfRVUAfjxV+iigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAAUUUUAFFFFABRRRQAgpaKKACkoooAWiiigAooooAO1IaKKAFNFFFAAKQ0UUAf/9k=' /><br /><br /></div><h2>16 to 19 revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your revenue funding allocation statement.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>Haringey Sixth Form College</td></tr>
<tr>
<td>UKPRN</td><td>10040630</td></tr>
<tr>
<td>Local authority</td><td>Haringey</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021/22 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 50%;"">Core programme funding</td><td style=""width: 50%;"" class=""right"">&pound;1,044,412</td></tr>
<tr>
<td>Condition of funding adjustment</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;27,200</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Care standards</td><td class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">High needs student</td><td style=""width: 50%;"" class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">Student financial support</td><td style=""width: 50%;"" class=""right"">&pound;12,268</td></tr>
<tr>
<td style=""width: 50%;"">Teachers' pension scheme grant</td><td style=""width: 50%;"" class=""right"">&pound;682,402</td></tr>
<tr>
<td style=""width: 50%;"">Alternative completion</td><td style=""width: 50%;"" class=""right"">&pound;20,944</td></tr>
<tr>
<td style=""width: 50%;"">Start-up and post-opening grant</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr>
<td style=""width: 50%;"">Maths top up</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total funding allocation</td><td class=""right"">&pound;1,104,824</td></tr></tbody></table><div style='font-weight: bold; font-size: 14px; margin-top: 5px'>Core programme funding</div>
<table id=""formulaTables"" class="""">


<tbody>

<tr>
<td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAADiSURBVGhD7dsxioQwGEDhmGJBEYLiDQIeYgsL72XvmfZyrsuEhYfjzJTj8L5Kfh6SvwuCYXvmr1iW5ftcWNc1pRQe6LquPJ35L35OlKKqqtu5jizoo4phGPYixlgGBxfaxQIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsKOSc67pOKZXBwYV2sQALsiALsiALsqDQ9/1FTmoBFmRBFmRBFmRBb1OM49i27X5PKYODC+1iARZkQRb0UUXOuWkav1oXFvRmxe7rRJimKcZ4i+7b3zPPc+nvee2/k8eeFdv2C7C2ttUtLoinAAAAAElFTkSuQmCC' /></td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Student&nbsp;numbers<br>for 2021/22</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;student&nbsp;numbers<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">213</td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;"">&nbsp;</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">National funding<br>rate per student<br>(dependent on<br>band)</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;breakdown&nbsp;of<br>funding&nbsp;by&nbsp;band&nbsp;table<br><br></td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;""></td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;884,782</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Retention&nbsp;factor</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">0.99800</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">-&pound;2,062</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;882,721</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Programme&nbsp;cost<br>weighting</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.02000</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;17,725</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;900,446</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Level 3<br>programme maths<br>and English<br>payment</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;Level&nbsp;3<br>programme&nbsp;maths&nbsp;and<br>English&nbsp;payment&nbsp;table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;11,146</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;911,591</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Disadvantage<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;distribution&nbsp;of<br>disadvantage&nbsp;funding<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;20,081</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;931,672</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Large<br>programme<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;large<br>programmes&nbsp;uplift<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;838</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;932,510</td></tr></tbody></table></td><td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAAEKSURBVGhD7dvNaoQwGEZhoyAoEongzYgrBS/MldfkpQliv6GhcJimsyjUMrzPIpiZg2Qgi8Efd11X9iM3DEM8fDLP87qucfKttm23bYuTlBCC2/c9zmhZFhsfRWqleZ7bV1bk8YM0FfQGhXPOxqIoksXnvjnP8+aVflFBKkgFqSAVpIJUkApSQSpIBakgFaSCVJAKUkEqSAWpoGThva+qqus63SsgFaSCVJAKUkEq6G0K59yLwv4+/IuVGhWkglSQClJBKkgF3VyEEJqm6fteVzdIBakgFaSCbi6893Vd66r1ExX0N4UryzIe0nEcNtoufDxwn2JbdRzHzM6RMk2TbebX7538/rdk2QcWtDy+yWdeMAAAAABJRU5ErkJggg==' /></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Area&nbsp;cost<br>allowance</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.12000</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;111,901</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;1,044,412</td></tr></tbody></table></td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""3"" scope=""col"">Student numbers</th>    </tr>
</thead>
<tbody>

<tr style=""width: 100%;"">
<td colspan=""2"" style=""width: 80%"">Autumn census</td><td style=""width: 20%; text-align: right"">213</td></tr>
<tr>
<td colspan=""2"">Exceptional variations to lagged student number</td><td style=""width: 15%; text-align: right"">0</td></tr>
<tr class=""greyBold"">
<td colspan=""2"" class=""bold"">Total student numbers for 2021/22</td><td style=""width: 20%; text-align: right"">213</td></tr>
<tr style=""width: 100%;"">
<td style=""width: 65%;"">Student number methodology used</td><td colspan=""2"" style=""width: 35%;"">Autumn census</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Breakdown of funding by band</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 30%"">Funding band<br><br></td><td>Student&nbsp;numbers<br>2019/20</td><td>Proportions&nbsp;used&nbsp;in<br>2021/22 allocation</td><td>Number&nbsp;of&nbsp;students<br>allocated in 2021/22</td><td>National&nbsp;funding&nbsp;rate</td><td style=""width: 20%"">Student&nbsp;funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>205</td><td>95.35%</td><td>203</td><td>&pound;4,188</td><td>&pound;850,554</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4 (Sum of bands 4a and 4b)</td><td>10</td><td>4.65%</td><td>10</td><td>&pound;3,455</td><td>&pound;34,229</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4a</td><td>10</td><td>4.65%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4b</td><td>0</td><td>0.00%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,827</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,234</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Band 1</td><td>Students</td><td>0</td><td>0.00%</td><td>0</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td>FTEs</td><td class=""right"">0.00</td><td class=""greyBold""></td><td>0.00</td><td>&pound;4,188</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total student funding</td><td>215</td><td>100%</td><td>213</td><td></td><td>&pound;884,782</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Level 3 programme maths and English payment</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 42%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Instances per student</th><th style=""width: 14%;"" scope=""col"">Number of instances</th><th style=""width: 10%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 1 year programme</td><td>0.00900</td><td>1.98</td><td>&pound;375</td><td>&pound;723</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 2 year programme</td><td>0.06500</td><td>13.87</td><td>&pound;750</td><td>&pound;10,403</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""4"" class=""bold"">Level 3 programme maths and English payment funding total</td><td>&pound;11,146</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""5"" scope=""col"">Distribution of disadvantage funding</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 1</td></tr>
<tr style=""width: 100%;"">
<td colspan=""2"" rowspan=""2"">Economic deprivation funding</td><td>Block 1 factor</td><td>Funding including programme costs</td><td style=""width: 20%;"">Block 1 funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>1.01000</td><td>&pound;911,591</td><td>&pound;9,143</td></tr>
<tr>
<td colspan=""2"" rowspan=""2"">Care leavers</td><td>Number of qualifying students</td><td>Rate per qualifying student</td><td>Care leaver funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 1 funding</td><td>&pound;9,143</td></tr>
<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 2</td></tr>
<tr>
<td colspan=""4"" rowspan=""2"">Total 2021/22 instances attracting funding per student</td><td>Instances per Student</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0.10700</td></tr>
<tr>
<td colspan=""3"" class=""right"">Number of instances in each band</td><td>Block 2 funding rates</td><td>Block 2 funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Total funded instances for 2021/22</td><td>22.79</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the higher rate (bands 4 and 5)</td><td>22.79</td><td>&pound;480</td><td>&pound;10,938</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the lower rate (bands 2 and 3)</td><td>0.00</td><td>&pound;292</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Students attracting the FTE rate<br>(Band 1)</td><td>Students</td><td>0.00</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>0.00</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 2 funding</td><td>&pound;10,938</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""4"">Minimum top up if applicable</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total disadvantage funding (sum of block 1, block 2 and minimum top up)</td><td>&pound;20,081</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Large programme uplift (based on 2018/19 students)</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 40%;""></td><td style=""width: 15%;"">Students meeting large programme uplift criteria</td><td style=""width: 25%;"">Funding uplift per student per year</td><td style=""width: 20%;"">Total large programme uplift (2 years)</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 20% of national rate</td><td>0</td><td>&pound;838</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 10% of national rate</td><td>1</td><td>&pound;419</td><td>&pound;838</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td class=""bold"">Total large programme funding</td><td>1</td><td></td><td>&pound;838</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Condition of funding (CoF)</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 25%"">Funding band<br><br></td><td>National funding rate<br>in 2019/20</td><td>Total students <br> (2019/20 R14)</td><td>National funding rate<br>applied to total students <br>(2019/20 R14)</td><td>Students not meeting<br>CoF (2019/20 R14)</td><td style=""width: 20%"">National funding rate applied to<br>CoF non-compliant students</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>&pound;4,000</td><td>205</td><td>&pound;820,000</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4</td><td>&pound;3,300</td><td>10</td><td>&pound;33,000</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>&pound;2,700</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>&pound;2,133</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"" style=""width: 20%"">Band 1</td><td>Students</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>&pound;4,000</td><td>0.00</td><td>&pound;0</td><td>0.00</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total</td><td>215</td><td>&pound;853,000</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">5% of national rate funding for total 2019/20 R14 students</td><td>&pound;42,650</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">Funding for non-compliant students less 5% of funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""6"" class=""bold"">Final condition of funding adjustment (at 50%)</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Advanced maths premium</caption>


<thead class=""whiteBackground top"">
    <tr>
<th style=""width: 24%;"" scope=""col""></th><th style=""width: 13%;"" scope=""col"">Baseline students</th><th style=""width: 14%;"" scope=""col"">Eligible students</th><th style=""width: 14%;"" scope=""col"">Eligible minus<br>baseline</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Advanced maths premium funding</td><td>79</td><td>68</td><td>0</td><td>&pound;600</td><td class=""bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">High value courses premium</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Qualifying students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>High value courses premium funding</td><td>68</td><td>&pound;400</td><td class=""bold"">&pound;27,200</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Industry placements funding</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Number&nbsp;of&nbsp;students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Industry&nbsp;placements:&nbsp;Capacity&nbsp;and<br>delivery&nbsp;funding&nbsp;(CDF)</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Industry&nbsp;placements:&nbsp;Capacity&nbsp;and&nbsp;delivery&nbsp;funding&nbsp;(CDF)</td><td>0</td><td>&pound;250</td><td class=""bold"">&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Care standards funding</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td>Eligible students</td><td style=""width: 15%;"">Rate</td><td style=""width: 20%;"" class=""right"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Care standards student funding</td><td>0</td><td>&pound;817</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Care standards institution lump sum funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total care standards funding</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">High needs funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">16-19 students</td><td style=""width: 15%"">19-24 students</td><td style=""width: 15%"">Total students</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>High needs element 2 for 2021/22</td><td>0</td><td>0</td><td>0</td><td>&pound;6,000</td><td style=""font-weight: bold"">&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">Student financial support funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">Number of funded students</td><td style=""width: 15%"">Instances per student</td><td style=""width: 15%"">Number of instances</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 1: Financial disadvantage</td><td>213</td><td>0.06300</td><td>13.47</td><td>&pound;242</td><td>&pound;3,261</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 2a: Student costs - travel</td><td>213</td><td>0.08500</td><td>18.02</td><td>&pound;483</td><td>&pound;8,705</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Element 2b: Student costs - industry placements</td><td>0</td><td>&pound;49</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Bursary adjustment in respect of free meals</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund</td><td>&pound;11,965</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">2019 to 2020 discretionary bursary fund - baseline for transition</td><td>&pound;16,357</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition lower limit</td><td>&pound;12,268</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition upper limit</td><td>&pound;20,447</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Exceptional adjustment</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Residential Bursary Fund</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Residential Support Scheme</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Residential funding total</td><td>&pound;0</td></tr>
<tr style=""height: 50px"">
<td colspan=""2"" style=""font-weight: bold""></td><td style=""vertical-align: bottom"">Total students</td><td style=""vertical-align: bottom"">Free meals<br>students</td><td style=""vertical-align: bottom"">Proportion of students<br>on free meals</td><td style=""vertical-align: bottom"">Total students in 2021/22 funded<br>for free meals</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals students</td><td>4,995</td><td>652</td><td>13.16%</td><td>650.81</td></tr>
<tr>
<td colspan=""3""></td><td>Free meals students</td><td style=""vertical-align: bottom"">Free meals rates</td><td style=""vertical-align: bottom"">Free meals funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals higher rate</td><td class=""greyBold""></td><td>635.71</td><td>&pound;358</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals lower rate</td><td class=""greyBold""></td><td>7.22</td><td>&pound;179</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Free meals FTE rate </td><td class=""greyBold""></td><td>1.67</td><td>&pound;358</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<th colspan=""5"" style=""text-align: left;"" scope=""row"">Free meals sub-total</th><td>&pound;161,381</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Free meals administration and minimum top up</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Exceptional adjustment</td><td colspan=""4"" class=""greyBold""></td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Total free meals funding</td><td>&pound;422</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Total student support funding</td><td>&pound;12,268</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Teachers' pension scheme grant</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 20%;"" scope=""col""></th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Payments made to Capita<br>for TPS, financial year<br>2019-2020 rebased at<br>16.4% for the full year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Annual payments<br>increased to 0%<br>equivalent for the full<br>year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">0% uplift for 2020-21</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">0% uplift for 2021-22</th><th style=""vertical-align: top; width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Revised annual cost</td><td>&pound;1,938,115</td><td>&pound;2,788,995</td><td>&pound;86,459</td><td>&pound;86,264</td><td>&pound;2,961,718</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Teachers'&nbsp;pension&nbsp;scheme&nbsp;grant&nbsp;(difference&nbsp;between&nbsp;2019/2020&nbsp;payments&nbsp;at&nbsp;16.4%&nbsp;and revised&nbsp;annual&nbsp;cost)</td><td style=""font-weight: bold"">&pound;682,402</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead class=""greyBold"">
    <tr>
<th colspan=""2"" scope=""col"">Alternative completion</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td>Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Alternative completion funding</td><td style=""font-weight: bold"">&pound;20,944</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Start-up and post-opening grant</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part A</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part B</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - per pupil resources</td><td>-&pound;997</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - leadership disecononomies</td><td>-&pound;997</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td style=""font-weight: bold"">Total start-up and post-opening grant</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Maths top up</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Maths top up funding</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style='font-size: 10px; margin-top: 5px'>The values on your statement are shown rounded to various numbers of decimal places.<br> The calculation of your funding however is done using un-rounded values. This may result in some slight differences when you work through the calculation yourselves.</div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_SixthForm_with =
      @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>16 to 19 SSF revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your revenue funding allocation statement.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>St Paul's Catholic College</td></tr>
<tr>
<td>UKPRN</td><td>10006247</td></tr>
<tr>
<td>Local authority</td><td>Surrey</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021/22 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 50%;"">Core programme funding</td><td style=""width: 50%;"" class=""right"">&pound;1,044,412</td></tr>
<tr>
<td>Condition of funding adjustment</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;27,200</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">Student financial support</td><td style=""width: 50%;"" class=""right"">&pound;12,268</td></tr>
<tr>
<td style=""width: 50%;"">Teachers' pension scheme grant</td><td style=""width: 50%;"" class=""right"">&pound;682,402</td></tr>
<tr>
<td style=""width: 50%;"">Alternative completion</td><td style=""width: 50%;"" class=""right"">&pound;20,944</td></tr>
<tr>
<td style=""width: 50%;"">Start-up and post-opening grant</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr>
<td style=""width: 50%;"">Maths top up</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total funding allocation</td><td class=""right"">&pound;1,104,824</td></tr></tbody></table><div style='font-weight: bold; font-size: 14px; margin-top: 5px'>Core programme funding</div>
<table id=""formulaTables"" class="""">


<tbody>

<tr>
<td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAADiSURBVGhD7dsxioQwGEDhmGJBEYLiDQIeYgsL72XvmfZyrsuEhYfjzJTj8L5Kfh6SvwuCYXvmr1iW5ftcWNc1pRQe6LquPJ35L35OlKKqqtu5jizoo4phGPYixlgGBxfaxQIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsKOSc67pOKZXBwYV2sQALsiALsiALsqDQ9/1FTmoBFmRBFmRBFmRBb1OM49i27X5PKYODC+1iARZkQRb0UUXOuWkav1oXFvRmxe7rRJimKcZ4i+7b3zPPc+nvee2/k8eeFdv2C7C2ttUtLoinAAAAAElFTkSuQmCC' /></td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Student&nbsp;numbers<br>for 2021/22</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;student&nbsp;numbers<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">213</td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;"">&nbsp;</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">National funding<br>rate per student<br>(dependent on<br>band)</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;breakdown&nbsp;of<br>funding&nbsp;by&nbsp;band&nbsp;table<br><br></td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;""></td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;884,782</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Retention&nbsp;factor</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">0.99800</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">-&pound;2,062</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;882,721</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Programme&nbsp;cost<br>weighting</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.02000</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;17,725</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;900,446</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Level 3<br>programme maths<br>and English<br>payment</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;Level&nbsp;3<br>programme&nbsp;maths&nbsp;and<br>English&nbsp;payment&nbsp;table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;11,146</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;911,591</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Disadvantage<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;distribution&nbsp;of<br>disadvantage&nbsp;funding<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;20,081</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;931,672</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Large<br>programme<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;large<br>programmes&nbsp;uplift<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;838</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;932,510</td></tr></tbody></table></td><td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAAEKSURBVGhD7dvNaoQwGEZhoyAoEongzYgrBS/MldfkpQliv6GhcJimsyjUMrzPIpiZg2Qgi8Efd11X9iM3DEM8fDLP87qucfKttm23bYuTlBCC2/c9zmhZFhsfRWqleZ7bV1bk8YM0FfQGhXPOxqIoksXnvjnP8+aVflFBKkgFqSAVpIJUkApSQSpIBakgFaSCVJAKUkEqSAWpoGThva+qqus63SsgFaSCVJAKUkEq6G0K59yLwv4+/IuVGhWkglSQClJBKkgF3VyEEJqm6fteVzdIBakgFaSCbi6893Vd66r1ExX0N4UryzIe0nEcNtoufDxwn2JbdRzHzM6RMk2TbebX7538/rdk2QcWtDy+yWdeMAAAAABJRU5ErkJggg==' /></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Area&nbsp;cost<br>allowance</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.12000</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;111,901</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;1,044,412</td></tr></tbody></table></td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""3"" scope=""col"">Student numbers</th>    </tr>
</thead>
<tbody>

<tr style=""width: 100%;"">
<td colspan=""2"" style=""width: 80%"">Autumn census</td><td style=""width: 20%; text-align: right"">213</td></tr>
<tr>
<td colspan=""2"">Exceptional variations to lagged student number</td><td style=""width: 15%; text-align: right"">0</td></tr>
<tr class=""greyBold"">
<td colspan=""2"" class=""bold"">Total student numbers for 2021/22</td><td style=""width: 20%; text-align: right"">213</td></tr>
<tr style=""width: 100%;"">
<td style=""width: 65%;"">Student number methodology used</td><td colspan=""2"" style=""width: 35%;"">Autumn census</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Breakdown of funding by band</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 30%"">Funding band<br><br></td><td>Student&nbsp;numbers<br>2019/20</td><td>Proportions&nbsp;used&nbsp;in<br>2021/22 allocation</td><td>Number&nbsp;of&nbsp;students<br>allocated in 2021/22</td><td>National&nbsp;funding&nbsp;rate</td><td style=""width: 20%"">Student&nbsp;funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>205</td><td>95.35%</td><td>203</td><td>&pound;4,188</td><td>&pound;850,554</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4 (Sum of bands 4a and 4b)</td><td>10</td><td>4.65%</td><td>10</td><td>&pound;3,455</td><td>&pound;34,229</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4a</td><td>10</td><td>4.65%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4b</td><td>0</td><td>0.00%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,827</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,234</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Band 1</td><td>Students</td><td>0</td><td>0.00%</td><td>0</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td>FTEs</td><td class=""right"">0.00</td><td class=""greyBold""></td><td>0.00</td><td>&pound;4,188</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total student funding</td><td>215</td><td>100%</td><td>213</td><td></td><td>&pound;884,782</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Level 3 programme maths and English payment</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 42%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Instances per student</th><th style=""width: 14%;"" scope=""col"">Number of instances</th><th style=""width: 10%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 1 year programme</td><td>0.00900</td><td>1.98</td><td>&pound;375</td><td>&pound;723</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 2 year programme</td><td>0.06500</td><td>13.87</td><td>&pound;750</td><td>&pound;10,403</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""4"" class=""bold"">Level 3 programme maths and English payment funding total</td><td>&pound;11,146</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""5"" scope=""col"">Distribution of disadvantage funding</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 1</td></tr>
<tr style=""width: 100%;"">
<td colspan=""2"" rowspan=""2"">Economic deprivation funding</td><td>Block 1 factor</td><td>Funding including programme costs</td><td style=""width: 20%;"">Block 1 funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>1.01000</td><td>&pound;911,591</td><td>&pound;9,143</td></tr>
<tr>
<td colspan=""2"" rowspan=""2"">Care leavers</td><td>Number of qualifying students</td><td>Rate per qualifying student</td><td>Care leaver funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 1 funding</td><td>&pound;9,143</td></tr>
<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 2</td></tr>
<tr>
<td colspan=""4"" rowspan=""2"">Total 2021/22 instances attracting funding per student</td><td>Instances per Student</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0.10700</td></tr>
<tr>
<td colspan=""3"" class=""right"">Number of instances in each band</td><td>Block 2 funding rates</td><td>Block 2 funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Total funded instances for 2021/22</td><td>22.79</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the higher rate (bands 4 and 5)</td><td>22.79</td><td>&pound;480</td><td>&pound;10,938</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the lower rate (bands 2 and 3)</td><td>0.00</td><td>&pound;292</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Students attracting the FTE rate<br>(Band 1)</td><td>Students</td><td>0.00</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>0.00</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 2 funding</td><td>&pound;10,938</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""4"">Minimum top up if applicable</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total disadvantage funding (sum of block 1, block 2 and minimum top up)</td><td>&pound;20,081</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Large programme uplift (based on 2018/19 students)</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 40%;""></td><td style=""width: 15%;"">Students meeting large programme uplift criteria</td><td style=""width: 25%;"">Funding uplift per student per year</td><td style=""width: 20%;"">Total large programme uplift (2 years)</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 20% of national rate</td><td>0</td><td>&pound;838</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 10% of national rate</td><td>1</td><td>&pound;419</td><td>&pound;838</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td class=""bold"">Total large programme funding</td><td>1</td><td></td><td>&pound;838</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Condition of funding (CoF)</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 25%"">Funding band<br><br></td><td>National funding rate<br>in 2019/20</td><td>Total students <br> (2019/20 S05)</td><td>National funding rate<br>applied to total students <br> (2019/20 S05)</td><td>Students not meeting<br>CoF (2019/20 S05)</td><td style=""width: 20%"">National funding rate applied to<br>CoF non-compliant students</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>&pound;4,000</td><td>205</td><td>&pound;820,000</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4</td><td>&pound;3,300</td><td>10</td><td>&pound;33,000</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>&pound;2,700</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>&pound;2,133</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"" style=""width: 20%"">Band 1</td><td>Students</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>&pound;4,000</td><td>0.00</td><td>&pound;0</td><td>0.00</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total</td><td>215</td><td>&pound;853,000</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">5% of national rate funding for total 2019/20 S05 students</td><td>&pound;42,650</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">Funding for non-compliant students less 5% of funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""6"" class=""bold"">Final condition of funding adjustment (at 50%)</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Advanced maths premium</caption>


<thead class=""whiteBackground top"">
    <tr>
<th style=""width: 24%;"" scope=""col""></th><th style=""width: 13%;"" scope=""col"">Baseline students</th><th style=""width: 14%;"" scope=""col"">Eligible students</th><th style=""width: 14%;"" scope=""col"">Eligible minus<br>baseline</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Advanced maths premium funding</td><td>79</td><td>68</td><td>0</td><td>&pound;600</td><td class=""bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">High value courses premium</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Qualifying students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>High value courses premium funding</td><td>68</td><td>&pound;400</td><td class=""bold"">&pound;27,200</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Industry placements funding</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Number&nbsp;of&nbsp;students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Industry&nbsp;placements:&nbsp;Capacity&nbsp;and<br>delivery&nbsp;funding&nbsp;(CDF)</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Industry&nbsp;placements:&nbsp;Capacity&nbsp;and&nbsp;delivery&nbsp;funding&nbsp;(CDF)</td><td>0</td><td>&pound;250</td><td class=""bold"">&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">Student financial support funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">Number of funded students</td><td style=""width: 15%"">Instances per student</td><td style=""width: 15%"">Number of instances</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 1: Financial disadvantage</td><td>213</td><td>0.06300</td><td>13.47</td><td>&pound;242</td><td>&pound;3,261</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 2a: Student costs - travel</td><td>213</td><td>0.08500</td><td>18.02</td><td>&pound;483</td><td>&pound;8,705</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Element 2b: Student costs - industry placements</td><td>0</td><td>&pound;49</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Bursary adjustment in respect of free meals</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund</td><td>&pound;11,965</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">2019 to 2020 discretionary bursary fund - baseline for transition</td><td>&pound;16,357</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition lower limit</td><td>&pound;12,268</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition upper limit</td><td>&pound;20,447</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Exceptional adjustment</td><td>&pound;0</td></tr></tbody></table>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Teachers' pension scheme grant</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 20%;"" scope=""col""></th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Payments made to Capita<br>for TPS, financial year<br>2019-2020 rebased at<br>16.4% for the full year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Annual payments<br>increased to 0%<br>equivalent for the full<br>year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">0% uplift for 2020-21</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">0% uplift for 2021-22</th><th style=""vertical-align: top; width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Revised annual cost</td><td>&pound;1,938,115</td><td>&pound;2,788,995</td><td>&pound;86,459</td><td>&pound;86,264</td><td>&pound;2,961,718</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Teachers'&nbsp;pension&nbsp;scheme&nbsp;grant&nbsp;(difference&nbsp;between&nbsp;2019/2020&nbsp;payments&nbsp;at&nbsp;16.4%&nbsp;and revised&nbsp;annual&nbsp;cost)</td><td style=""font-weight: bold"">&pound;682,402</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead class=""greyBold"">
    <tr>
<th colspan=""2"" scope=""col"">Alternative completion</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td>Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Alternative completion funding</td><td style=""font-weight: bold"">&pound;20,944</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Start-up and post-opening grant</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part A</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part B</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - per pupil resources</td><td>-&pound;997</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - leadership disecononomies</td><td>-&pound;997</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td style=""font-weight: bold"">Total start-up and post-opening grant</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Maths top up</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Maths top up funding</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style='font-size: 10px; margin-top: 5px'>The values on your statement are shown rounded to various numbers of decimal places.<br> The calculation of your funding however is done using un-rounded values. This may result in some slight differences when you work through the calculation yourselves.</div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_SixthForm_withHighValueCoursesForSchoolAndCollegeLeavers_And_SUP_AND_POG =
  @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>16 to 19 academies revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your revenue funding allocation statement.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>Haringey Sixth Form College</td></tr>
<tr>
<td>UKPRN</td><td>10040631</td></tr>
<tr>
<td>Local authority</td><td>Haringey</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021/22 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 50%;"">Core programme funding</td><td style=""width: 50%;"" class=""right"">&pound;1,044,412</td></tr>
<tr>
<td>Condition of funding adjustment</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;27,200</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">High needs student</td><td style=""width: 50%;"" class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">Student financial support</td><td style=""width: 50%;"" class=""right"">&pound;12,268</td></tr>
<tr>
<td style=""width: 50%;"">Teachers' pension scheme grant</td><td style=""width: 50%;"" class=""right"">&pound;682,402</td></tr>
<tr>
<td style=""width: 50%;"">Alternative completion</td><td style=""width: 50%;"" class=""right"">&pound;20,944</td></tr>
<tr>
<td style=""width: 50%;"">High value courses for school and college leavers</td><td style=""width: 50%;"" class=""right"">&pound;1,100</td></tr>
<tr>
<td style=""width: 50%;"">Start-up and post-opening grant</td><td style=""width: 50%;"" class=""right"">&pound;997</td></tr>
<tr>
<td style=""width: 50%;"">Maths top up</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total funding allocation</td><td class=""right"">&pound;1,104,824</td></tr></tbody></table><div style='font-weight: bold; font-size: 14px; margin-top: 5px'>Core programme funding</div>
<table id=""formulaTables"" class="""">


<tbody>

<tr>
<td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAADiSURBVGhD7dsxioQwGEDhmGJBEYLiDQIeYgsL72XvmfZyrsuEhYfjzJTj8L5Kfh6SvwuCYXvmr1iW5ftcWNc1pRQe6LquPJ35L35OlKKqqtu5jizoo4phGPYixlgGBxfaxQIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsKOSc67pOKZXBwYV2sQALsiALsiALsqDQ9/1FTmoBFmRBFmRBFmRBb1OM49i27X5PKYODC+1iARZkQRb0UUXOuWkav1oXFvRmxe7rRJimKcZ4i+7b3zPPc+nvee2/k8eeFdv2C7C2ttUtLoinAAAAAElFTkSuQmCC' /></td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Student&nbsp;numbers<br>for 2021/22</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;student&nbsp;numbers<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">213</td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;"">&nbsp;</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">National funding<br>rate per student<br>(dependent on<br>band)</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;breakdown&nbsp;of<br>funding&nbsp;by&nbsp;band&nbsp;table<br><br></td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;""></td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;884,782</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Retention&nbsp;factor</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">0.99800</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">-&pound;2,062</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;882,721</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Programme&nbsp;cost<br>weighting</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.02000</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;17,725</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;900,446</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Level 3<br>programme maths<br>and English<br>payment</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;Level&nbsp;3<br>programme&nbsp;maths&nbsp;and<br>English&nbsp;payment&nbsp;table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;11,146</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;911,591</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Disadvantage<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;distribution&nbsp;of<br>disadvantage&nbsp;funding<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;20,081</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;931,672</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Large<br>programme<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;large<br>programmes&nbsp;uplift<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;838</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;932,510</td></tr></tbody></table></td><td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAAEKSURBVGhD7dvNaoQwGEZhoyAoEongzYgrBS/MldfkpQliv6GhcJimsyjUMrzPIpiZg2Qgi8Efd11X9iM3DEM8fDLP87qucfKttm23bYuTlBCC2/c9zmhZFhsfRWqleZ7bV1bk8YM0FfQGhXPOxqIoksXnvjnP8+aVflFBKkgFqSAVpIJUkApSQSpIBakgFaSCVJAKUkEqSAWpoGThva+qqus63SsgFaSCVJAKUkEq6G0K59yLwv4+/IuVGhWkglSQClJBKkgF3VyEEJqm6fteVzdIBakgFaSCbi6893Vd66r1ExX0N4UryzIe0nEcNtoufDxwn2JbdRzHzM6RMk2TbebX7538/rdk2QcWtDy+yWdeMAAAAABJRU5ErkJggg==' /></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Area&nbsp;cost<br>allowance</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.12000</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;111,901</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;1,044,412</td></tr></tbody></table></td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""3"" scope=""col"">Student numbers</th>    </tr>
</thead>
<tbody>

<tr style=""width: 100%;"">
<td colspan=""2"" style=""width: 80%"">Autumn census</td><td style=""width: 20%; text-align: right"">213</td></tr>
<tr>
<td colspan=""2"">Exceptional variations to lagged student number</td><td style=""width: 15%; text-align: right"">0</td></tr>
<tr class=""greyBold"">
<td colspan=""2"" class=""bold"">Total student numbers for 2021/22</td><td style=""width: 20%; text-align: right"">213</td></tr>
<tr style=""width: 100%;"">
<td style=""width: 65%;"">Student number methodology used</td><td colspan=""2"" style=""width: 35%;"">Autumn census</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Breakdown of funding by band</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 30%"">Funding band<br><br></td><td>Student&nbsp;numbers<br>2019/20</td><td>Proportions&nbsp;used&nbsp;in<br>2021/22 allocation</td><td>Number&nbsp;of&nbsp;students<br>allocated in 2021/22</td><td>National&nbsp;funding&nbsp;rate</td><td style=""width: 20%"">Student&nbsp;funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>205</td><td>95.35%</td><td>203</td><td>&pound;4,188</td><td>&pound;850,554</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4 (Sum of bands 4a and 4b)</td><td>10</td><td>4.65%</td><td>10</td><td>&pound;3,455</td><td>&pound;34,229</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4a</td><td>10</td><td>4.65%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4b</td><td>0</td><td>0.00%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,827</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,234</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Band 1</td><td>Students</td><td>0</td><td>0.00%</td><td>0</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td>FTEs</td><td class=""right"">0.00</td><td class=""greyBold""></td><td>0.00</td><td>&pound;4,188</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total student funding</td><td>215</td><td>100%</td><td>213</td><td></td><td>&pound;884,782</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Level 3 programme maths and English payment</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 42%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Instances per student</th><th style=""width: 14%;"" scope=""col"">Number of instances</th><th style=""width: 10%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 1 year programme</td><td>0.00900</td><td>1.98</td><td>&pound;375</td><td>&pound;723</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 2 year programme</td><td>0.06500</td><td>13.87</td><td>&pound;750</td><td>&pound;10,403</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""4"" class=""bold"">Level 3 programme maths and English payment funding total</td><td>&pound;11,146</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""5"" scope=""col"">Distribution of disadvantage funding</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 1</td></tr>
<tr style=""width: 100%;"">
<td colspan=""2"" rowspan=""2"">Economic deprivation funding</td><td>Block 1 factor</td><td>Funding including programme costs</td><td style=""width: 20%;"">Block 1 funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>1.01000</td><td>&pound;911,591</td><td>&pound;9,143</td></tr>
<tr>
<td colspan=""2"" rowspan=""2"">Care leavers</td><td>Number of qualifying students</td><td>Rate per qualifying student</td><td>Care leaver funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 1 funding</td><td>&pound;9,143</td></tr>
<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 2</td></tr>
<tr>
<td colspan=""4"" rowspan=""2"">Total 2021/22 instances attracting funding per student</td><td>Instances per Student</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0.10700</td></tr>
<tr>
<td colspan=""3"" class=""right"">Number of instances in each band</td><td>Block 2 funding rates</td><td>Block 2 funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Total funded instances for 2021/22</td><td>22.79</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the higher rate (bands 4 and 5)</td><td>22.79</td><td>&pound;480</td><td>&pound;10,938</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the lower rate (bands 2 and 3)</td><td>0.00</td><td>&pound;292</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Students attracting the FTE rate<br>(Band 1)</td><td>Students</td><td>0.00</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>0.00</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 2 funding</td><td>&pound;10,938</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""4"">Minimum top up if applicable</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total disadvantage funding (sum of block 1, block 2 and minimum top up)</td><td>&pound;20,081</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Large programme uplift (based on 2018/19 students)</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 40%;""></td><td style=""width: 15%;"">Students meeting large programme uplift criteria</td><td style=""width: 25%;"">Funding uplift per student per year</td><td style=""width: 20%;"">Total large programme uplift (2 years)</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 20% of national rate</td><td>0</td><td>&pound;838</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 10% of national rate</td><td>1</td><td>&pound;419</td><td>&pound;838</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td class=""bold"">Total large programme funding</td><td>1</td><td></td><td>&pound;838</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Condition of funding (CoF)</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 25%"">Funding band<br><br></td><td>National funding rate<br>in 2019/20</td><td>Total students <br> (2019/20 R14)</td><td>National funding rate<br>applied to total students <br>(2019/20 R14)</td><td>Students not meeting<br>CoF (2019/20 R14)</td><td style=""width: 20%"">National funding rate applied to<br>CoF non-compliant students</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>&pound;4,000</td><td>205</td><td>&pound;820,000</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4</td><td>&pound;3,300</td><td>10</td><td>&pound;33,000</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>&pound;2,700</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>&pound;2,133</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"" style=""width: 20%"">Band 1</td><td>Students</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>&pound;4,000</td><td>0.00</td><td>&pound;0</td><td>0.00</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total</td><td>215</td><td>&pound;853,000</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">5% of national rate funding for total 2019/20 R14 students</td><td>&pound;42,650</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">Funding for non-compliant students less 5% of funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""6"" class=""bold"">Final condition of funding adjustment (at 50%)</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Advanced maths premium</caption>


<thead class=""whiteBackground top"">
    <tr>
<th style=""width: 24%;"" scope=""col""></th><th style=""width: 13%;"" scope=""col"">Baseline students</th><th style=""width: 14%;"" scope=""col"">Eligible students</th><th style=""width: 14%;"" scope=""col"">Eligible minus<br>baseline</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Advanced maths premium funding</td><td>79</td><td>68</td><td>0</td><td>&pound;600</td><td class=""bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">High value courses premium</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Qualifying students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>High value courses premium funding</td><td>68</td><td>&pound;400</td><td class=""bold"">&pound;27,200</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Industry placements funding</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Number&nbsp;of&nbsp;students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Industry&nbsp;placements:&nbsp;Capacity&nbsp;and<br>delivery&nbsp;funding&nbsp;(CDF)</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Industry&nbsp;placements:&nbsp;Capacity&nbsp;and&nbsp;delivery&nbsp;funding&nbsp;(CDF)</td><td>0</td><td>&pound;250</td><td class=""bold"">&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">High needs funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">16-19 students</td><td style=""width: 15%"">19-24 students</td><td style=""width: 15%"">Total students</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>High needs element 2 for 2021/22</td><td>0</td><td>0</td><td>0</td><td>&pound;6,000</td><td style=""font-weight: bold"">&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">Student financial support funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">Number of funded students</td><td style=""width: 15%"">Instances per student</td><td style=""width: 15%"">Number of instances</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 1: Financial disadvantage</td><td>213</td><td>0.06300</td><td>13.47</td><td>&pound;242</td><td>&pound;3,261</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 2a: Student costs - travel</td><td>213</td><td>0.08500</td><td>18.02</td><td>&pound;483</td><td>&pound;8,705</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Element 2b: Student costs - industry placements</td><td>0</td><td>&pound;49</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Bursary adjustment in respect of free meals</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund</td><td>&pound;11,965</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">2019 to 2020 discretionary bursary fund - baseline for transition</td><td>&pound;16,357</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition lower limit</td><td>&pound;12,268</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition upper limit</td><td>&pound;20,447</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Exceptional adjustment</td><td>&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Teachers' pension scheme grant</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 20%;"" scope=""col""></th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Payments made to Capita<br>for TPS, financial year<br>2019-2020 rebased at<br>16.4% for the full year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Annual payments<br>increased to 0%<br>equivalent for the full<br>year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">0% uplift for 2020-21</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">0% uplift for 2021-22</th><th style=""vertical-align: top; width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Revised annual cost</td><td>&pound;1,938,115</td><td>&pound;2,788,995</td><td>&pound;86,459</td><td>&pound;86,264</td><td>&pound;2,961,718</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Teachers'&nbsp;pension&nbsp;scheme&nbsp;grant&nbsp;(difference&nbsp;between&nbsp;2019/2020&nbsp;payments&nbsp;at&nbsp;16.4%&nbsp;and revised&nbsp;annual&nbsp;cost)</td><td style=""font-weight: bold"">&pound;682,402</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">High value courses for school and college leavers</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 20%;"" scope=""col""></th><th style=""vertical-align: top; width: 10%"" scope=""col"">Eligible students</th><th style=""vertical-align: top; width: 10%"" scope=""col"">Baseline</th><th style=""vertical-align: top; width: 10%"" scope=""col"">Students above baseline</th><th style=""vertical-align: top; width: 10%"" scope=""col"">Students funded in 2020/21</th><th style=""vertical-align: top; width: 10%"" scope=""col"">Additional<br>students funded in<br>2021/22</th><th style=""vertical-align: top; width: 10%"" scope=""col"">Rate</th><th style=""vertical-align: top; width: 20%"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Uplift for additional students </td><td>1</td><td>21</td><td>0</td><td>0</td><td>6</td><td>&pound;0</td><td style=""font-weight: bold"" class=""right"">&pound;1,100</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead class=""greyBold"">
    <tr>
<th colspan=""2"" scope=""col"">Alternative completion</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td>Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Alternative completion funding</td><td style=""font-weight: bold"">&pound;20,944</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Start-up and post-opening grant</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part A</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part B</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - per pupil resources</td><td>-&pound;997</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - leadership disecononomies</td><td>-&pound;997</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td style=""font-weight: bold"">Total start-up and post-opening grant</td><td style=""font-weight: bold"">&pound;997</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Maths top up</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Maths top up funding</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style='font-size: 10px; margin-top: 5px'>The values on your statement are shown rounded to various numbers of decimal places.<br> The calculation of your funding however is done using un-rounded values. This may result in some slight differences when you work through the calculation yourselves.</div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_SixthForm_without =
       @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>16 to 19 SSF revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your revenue funding allocation statement.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>Brentside High School</td></tr>
<tr>
<td>UKPRN</td><td>10000866</td></tr>
<tr>
<td>Local authority</td><td>Ealing</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021/22 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 50%;"">Core programme funding</td><td style=""width: 50%;"" class=""right"">&pound;1,153,932</td></tr>
<tr>
<td>Condition of funding adjustment</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;24,800</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">Student financial support</td><td style=""width: 50%;"" class=""right"">&pound;26,242</td></tr>
<tr>
<td style=""width: 50%;"">Teachers' pension scheme grant</td><td style=""width: 50%;"" class=""right"">&pound;682,402</td></tr>
<tr>
<td style=""width: 50%;"">Start-up and post-opening grant</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr>
<td style=""width: 50%;"">Maths top up</td><td style=""width: 50%;"" class=""right"">-&pound;997</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total funding allocation</td><td class=""right"">&pound;1,204,975</td></tr></tbody></table><div style='font-weight: bold; font-size: 14px; margin-top: 5px'>Core programme funding</div>
<table id=""formulaTables"" class="""">


<tbody>

<tr>
<td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAADiSURBVGhD7dsxioQwGEDhmGJBEYLiDQIeYgsL72XvmfZyrsuEhYfjzJTj8L5Kfh6SvwuCYXvmr1iW5ftcWNc1pRQe6LquPJ35L35OlKKqqtu5jizoo4phGPYixlgGBxfaxQIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsyIIsKOSc67pOKZXBwYV2sQALsiALsiALsqDQ9/1FTmoBFmRBFmRBFmRBb1OM49i27X5PKYODC+1iARZkQRb0UUXOuWkav1oXFvRmxe7rRJimKcZ4i+7b3zPPc+nvee2/k8eeFdv2C7C2ttUtLoinAAAAAElFTkSuQmCC' /></td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Student&nbsp;numbers<br>for 2021/22</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;student&nbsp;numbers<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">222</td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;"">&nbsp;</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">National funding<br>rate per student<br>(dependent on<br>band)</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;breakdown&nbsp;of<br>funding&nbsp;by&nbsp;band&nbsp;table<br><br></td></tr>
<tr class=""greyBold"">
<td style=""height: 25px;""></td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;921,137</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Retention&nbsp;factor</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">0.98200</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">-&pound;16,848</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;904,289</td></tr></tbody></table></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Programme&nbsp;cost<br>weighting</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.03900</td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;34,860</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;939,149</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Level 3<br>programme maths<br>and English<br>payment</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;Level&nbsp;3<br>programme&nbsp;maths&nbsp;and<br>English&nbsp;payment&nbsp;table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;19,629</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;958,778</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Disadvantage<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;distribution&nbsp;of<br>disadvantage&nbsp;funding<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;71,518</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;1,030,297</td></tr></tbody></table></td><td style=""font-size: 24px;"" class=""bold"">+</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Large<br>programme<br>funding</td></tr>
<tr>
<td style=""font-size: 11px; height: 51px;"">See&nbsp;large<br>programmes&nbsp;uplift<br>table<br><br></td></tr>
<tr>
<td style=""text-align: right; height: 25px;"">&pound;0</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;1,030,297</td></tr></tbody></table></td><td><img style='height: 200px' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAsAAAF/CAIAAAAQCqQSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAHYcAAB2HAY/l8WUAAAEKSURBVGhD7dvNaoQwGEZhoyAoEongzYgrBS/MldfkpQliv6GhcJimsyjUMrzPIpiZg2Qgi8Efd11X9iM3DEM8fDLP87qucfKttm23bYuTlBCC2/c9zmhZFhsfRWqleZ7bV1bk8YM0FfQGhXPOxqIoksXnvjnP8+aVflFBKkgFqSAVpIJUkApSQSpIBakgFaSCVJAKUkEqSAWpoGThva+qqus63SsgFaSCVJAKUkEq6G0K59yLwv4+/IuVGhWkglSQClJBKkgF3VyEEJqm6fteVzdIBakgFaSCbi6893Vd66r1ExX0N4UryzIe0nEcNtoufDxwn2JbdRzHzM6RMk2TbebX7538/rdk2QcWtDy+yWdeMAAAAABJRU5ErkJggg==' /></td><td style=""font-size: 16px;"" class=""bold"">X</td><td>
<table class=""styleTable"" style=""width: 115px; border: 1px solid #000;"">


<tbody>

<tr>
<td style=""text-align: center; height: 85px"">Area&nbsp;cost<br>allowance</td></tr>
<tr>
<td style=""text-align: right; height: 51px;"">1.12000</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;123,636</td></tr>
<tr>
<td style=""text-align: right; height: 25px"">&pound;1,153,932</td></tr></tbody></table></td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""3"" scope=""col"">Student numbers</th>    </tr>
</thead>
<tbody>

<tr style=""width: 100%;"">
<td colspan=""2"" style=""width: 80%"">Autumn census</td><td style=""width: 20%; text-align: right"">222</td></tr>
<tr>
<td colspan=""2"">Exceptional variations to lagged student number</td><td style=""width: 15%; text-align: right"">0</td></tr>
<tr class=""greyBold"">
<td colspan=""2"" class=""bold"">Total student numbers for 2021/22</td><td style=""width: 20%; text-align: right"">222</td></tr>
<tr style=""width: 100%;"">
<td style=""width: 65%;"">Student number methodology used</td><td colspan=""2"" style=""width: 35%;"">Autumn census</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Breakdown of funding by band</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 30%"">Funding band<br><br></td><td>Student&nbsp;numbers<br>2019/20</td><td>Proportions&nbsp;used&nbsp;in<br>2021/22 allocation</td><td>Number&nbsp;of&nbsp;students<br>allocated in 2021/22</td><td>National&nbsp;funding&nbsp;rate</td><td style=""width: 20%"">Student&nbsp;funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>233</td><td>94.72%</td><td>210</td><td>&pound;4,188</td><td>&pound;880,604</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4 (Sum of bands 4a and 4b)</td><td>13</td><td>5.28%</td><td>12</td><td>&pound;3,455</td><td>&pound;40,553</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4a</td><td>13</td><td>5.28%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4b</td><td>0</td><td>0.00%</td><td colspan=""3"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,827</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>0</td><td>0.00%</td><td>0</td><td>&pound;2,234</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Band 1</td><td>Students</td><td>0</td><td>0.00%</td><td>0</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td>FTEs</td><td class=""right"">0.00</td><td class=""greyBold""></td><td>0.00</td><td>&pound;4,188</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""2"" class=""bold"">Total student funding</td><td>246</td><td>100%</td><td>222</td><td></td><td>&pound;921,137</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Level 3 programme maths and English payment</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 42%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Instances per student</th><th style=""width: 14%;"" scope=""col"">Number of instances</th><th style=""width: 10%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 1 year programme</td><td>0.04900</td><td>10.83</td><td>&pound;375</td><td>&pound;4,061</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Level 3 programme maths and English payment - 2 year programme</td><td>0.09400</td><td>20.76</td><td>&pound;750</td><td>&pound;15,568</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""4"" class=""bold"">Level 3 programme maths and English payment funding total</td><td>&pound;19,629</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""5"" scope=""col"">Distribution of disadvantage funding</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 1</td></tr>
<tr style=""width: 100%;"">
<td colspan=""2"" rowspan=""2"">Economic deprivation funding</td><td>Block 1 factor</td><td>Funding including programme costs</td><td style=""width: 20%;"">Block 1 funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>1.00000</td><td>&pound;958,778</td><td>&pound;44,228</td></tr>
<tr>
<td colspan=""2"" rowspan=""2"">Care leavers</td><td>Number of qualifying students</td><td>Rate per qualifying student</td><td>Care leaver funding</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 1 funding</td><td>&pound;44,228</td></tr>
<tr>
<td colspan=""5"" style=""font-weight: bold"">Disadvantage block 2</td></tr>
<tr>
<td colspan=""4"" rowspan=""2"">Total 2021/22 instances attracting funding per student</td><td>Instances per Student</td></tr>
<tr class=""column1OnwardsRightAlign"">
<td>0.25600</td></tr>
<tr>
<td colspan=""3"" class=""right"">Number of instances in each band</td><td>Block 2 funding rates</td><td>Block 2 funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Total funded instances for 2021/22</td><td>56.85</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the higher rate (bands 4 and 5)</td><td>56.85</td><td>&pound;480</td><td>&pound;27,290</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Students attracting the lower rate (bands 2 and 3)</td><td>0.00</td><td>&pound;292</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"">Students attracting the FTE rate<br>(Band 1)</td><td>Students</td><td>0.00</td><td colspan=""2"" class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>0.00</td><td>&pound;480</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total block 2 funding</td><td>&pound;27,290</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""4"">Minimum top up if applicable</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""4"" class=""bold"">Total disadvantage funding (sum of block 1, block 2 and minimum top up)</td><td>&pound;71,518</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""4"" scope=""col"">Large programme uplift (based on 2018/19 students)</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 40%;""></td><td style=""width: 15%;"">Students meeting large programme uplift criteria</td><td style=""width: 25%;"">Funding uplift per student per year</td><td style=""width: 20%;"">Total large programme uplift (2 years)</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 20% of national rate</td><td>0</td><td>&pound;838</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Large programme uplift at 10% of national rate</td><td>0</td><td>&pound;419</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td class=""bold"">Total large programme funding</td><td>0</td><td></td><td>&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""7"" scope=""col"">Condition of funding (CoF)</th>    </tr>
</thead>
<tbody>

<tr>
<td colspan=""2"" style=""width: 25%"">Funding band<br><br></td><td>National funding rate<br>in 2019/20</td><td>Total students <br> (2019/20 S05)</td><td>National funding rate<br>applied to total students <br> (2019/20 S05)</td><td>Students not meeting<br>CoF (2019/20 S05)</td><td style=""width: 20%"">National funding rate applied to<br>CoF non-compliant students</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 5</td><td>&pound;4,000</td><td>233</td><td>&pound;932,000</td><td>2</td><td>&pound;8,000</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 4</td><td>&pound;3,300</td><td>13</td><td>&pound;42,900</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 3</td><td>&pound;2,700</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""2"">Band 2</td><td>&pound;2,133</td><td>0</td><td>&pound;0</td><td>0</td><td>&pound;0</td></tr>
<tr class=""columns3OnwardsRightAlign"">
<td rowspan=""2"" style=""width: 20%"">Band 1</td><td>Students</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td><td>0</td><td class=""greyBold""></td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>FTEs</td><td>&pound;4,000</td><td>0.00</td><td>&pound;0</td><td>0.00</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""3"" class=""bold"">Total</td><td>246</td><td>&pound;974,900</td><td>2</td><td>&pound;8,000</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">5% of national rate funding for total 2019/20 S05 students</td><td>&pound;48,745</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""6"">Funding for non-compliant students less 5% of funding</td><td>&pound;0</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td colspan=""6"" class=""bold"">Final condition of funding adjustment (at 50%)</td><td>&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Advanced maths premium</caption>


<thead class=""whiteBackground top"">
    <tr>
<th style=""width: 24%;"" scope=""col""></th><th style=""width: 13%;"" scope=""col"">Baseline students</th><th style=""width: 14%;"" scope=""col"">Eligible students</th><th style=""width: 14%;"" scope=""col"">Eligible minus<br>baseline</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Advanced maths premium funding</td><td>86</td><td>83</td><td>0</td><td>&pound;600</td><td class=""bold"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">High value courses premium</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Qualifying students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>High value courses premium funding</td><td>62</td><td>&pound;400</td><td class=""bold"">&pound;24,800</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Industry placements funding</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 51%;"" scope=""col""></th><th style=""width: 14%;"" scope=""col"">Number&nbsp;of&nbsp;students</th><th style=""width: 15%;"" scope=""col"">Rate</th><th style=""width: 20%;"" scope=""col"">Industry&nbsp;placements:&nbsp;Capacity&nbsp;and<br>delivery&nbsp;funding&nbsp;(CDF)</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Industry&nbsp;placements:&nbsp;Capacity&nbsp;and&nbsp;delivery&nbsp;funding&nbsp;(CDF)</td><td>0</td><td>&pound;250</td><td class=""bold"">&pound;0</td></tr></tbody></table><div style=""page-break-before: always""></div><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""6"" scope=""col"">Student financial support funding</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 20%""></td><td style=""width: 15%"">Number of funded students</td><td style=""width: 15%"">Instances per student</td><td style=""width: 15%"">Number of instances</td><td style=""width: 15%"">Rate</td><td style=""width: 20%"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 1: Financial disadvantage</td><td>222</td><td>0.28200</td><td>62.63</td><td>&pound;242</td><td>&pound;15,156</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Element 2a: Student costs - travel</td><td>222</td><td>0.01800</td><td>3.99</td><td>&pound;483</td><td>&pound;1,928</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""3"">Element 2b: Student costs - industry placements</td><td>0</td><td>&pound;49</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Bursary adjustment in respect of free meals</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign greyBold"">
<td colspan=""5"" class=""bold"">Discretionary bursary fund</td><td>&pound;17,084</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">2019 to 2020 discretionary bursary fund - baseline for transition</td><td>&pound;34,990</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition lower limit</td><td>&pound;26,242</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Transition upper limit</td><td>&pound;43,737</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Exceptional adjustment</td><td>&pound;0</td></tr></tbody></table>
<table class=""styleTable"" style=""width: 100%;"">
<caption class="""">Teachers' pension scheme grant</caption>


<thead class=""whiteBackground"">
    <tr>
<th style=""width: 20%;"" scope=""col""></th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Payments made to Capita<br>for TPS, financial year<br>2019-2020 rebased at<br>16.4% for the full year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">Annual payments<br>increased to 0%<br>equivalent for the full<br>year</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">0% uplift for 2020-21</th><th style=""vertical-align: top; width: 15%;"" scope=""col"">0% uplift for 2021-22</th><th style=""vertical-align: top; width: 20%;"" scope=""col"">Funding</th>    </tr>
</thead>
<tbody>

<tr class=""columns2OnwardsRightAlign"">
<td>Revised annual cost</td><td>&pound;1,938,115</td><td>&pound;2,788,995</td><td>&pound;86,459</td><td>&pound;86,264</td><td>&pound;2,961,718</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td colspan=""5"">Teachers'&nbsp;pension&nbsp;scheme&nbsp;grant&nbsp;(difference&nbsp;between&nbsp;2019/2020&nbsp;payments&nbsp;at&nbsp;16.4%&nbsp;and revised&nbsp;annual&nbsp;cost)</td><td style=""font-weight: bold"">&pound;682,402</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Start-up and post-opening grant</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part A</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Start-up grant - part B</td><td>&pound;0</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - per pupil resources</td><td>-&pound;997</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Post opening grant - leadership disecononomies</td><td>-&pound;997</td></tr>
<tr class=""greyBold columns2OnwardsRightAlign"">
<td style=""font-weight: bold"">Total start-up and post-opening grant</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style=""page-break-before: always""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Maths top up</th>    </tr>
</thead>
<tbody>

<tr>
<td></td><td style=""width: 20%;"">Funding</td></tr>
<tr class=""columns2OnwardsRightAlign"">
<td>Maths top up funding</td><td style=""font-weight: bold"">-&pound;997</td></tr></tbody></table><div style='font-size: 10px; margin-top: 5px'>The values on your statement are shown rounded to various numbers of decimal places.<br> The calculation of your funding however is done using un-rounded values. This may result in some slight differences when you work through the calculation yourselves.</div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_IndicativeStatement =
           @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>INDICATIVE: 16 to 19 academies revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your statement and your revenue funding allocation.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>Truro and Penwith College</td></tr>
<tr>
<td>UKPRN</td><td>10007063</td></tr>
<tr>
<td>Local authority</td><td>Cornwall</td></tr>
<tr>
<td>Open date</td><td>01 January 2000</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021 to 2022 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 80%;"">Programme funding</td><td style=""width: 20%;"" class=""right"">&pound;22,508,598</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;152,800</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;218,875</td></tr>
<tr>
<td>High needs student</td><td class=""right"">&pound;1,302,000</td></tr>
<tr>
<td>Student financial support</td><td class=""right"">&pound;905,211</td></tr>
<tr>
<td>Alternative completion</td><td class=""right"">&pound;34,034</td></tr>
<tr>
<td>Start-up and post-opening grant</td><td class=""right"">-&pound;997</td></tr>
<tr>
<td>Maths top up</td><td class=""right"">-&pound;997</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold;"">Total funding allocation</td><td class=""right"">&pound;25,803,920</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Start-up and post-opening grant</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 80%;""></td><td style=""width: 20%;"" class=""right"">Funding</td></tr>
<tr>
<td>Start-up grant - part A</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Start-up grant - part B</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Post opening grant - per pupil resources</td><td class=""right"">-&pound;997</td></tr>
<tr>
<td>Post opening grant - leadership diseconomies</td><td class=""right"">-&pound;997</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total start-up and post-opening grant</td><td style=""font-weight: bold;"" class=""right"">-&pound;997</td></tr></tbody></table><div class=""smallBr""></div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_Provider_Indicative_WithoutHighValueCourses_2223 =
         @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>INDICATIVE: 16 to 19 academies revenue funding allocation statement: 2022 to 2023</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2022-to-2023'>supporting guides</a> to help you understand your statement and your revenue funding allocation.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>St Mary's Catholic School</td></tr>
<tr>
<td>UKPRN</td><td>10064744</td></tr>
<tr>
<td>Local authority</td><td>Hertfordshire</td></tr>
<tr>
<td>Open date</td><td>01 November 2021</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2022 to 2023 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 80%;"">Programme funding</td><td style=""width: 20%;"" class=""right"">&pound;803,165</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;62,400</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;19,110</td></tr>
<tr>
<td>High needs student</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Student financial support</td><td class=""right"">&pound;24,782</td></tr>
<tr>
<td>Start-up and post-opening grant</td><td class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">16 to 19 Tuition funding</td><td style=""width: 50%;"" class=""right"">&pound;2,000</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold;"">Total funding allocation</td><td class=""right"">&pound;909,457</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Start-up and post-opening grant</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 80%;""></td><td style=""width: 20%;"" class=""right"">Funding</td></tr>
<tr>
<td>Start-up grant - part A</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Start-up grant - part B</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Post opening grant - per pupil resources</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Post opening grant - leadership diseconomies</td><td class=""right"">&pound;0</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total start-up and post-opening grant</td><td style=""font-weight: bold;"" class=""right"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";

        private readonly string html_1619_Provider_Indicative_WithHighValueCourses =
     @"<html><head></head><body><div style='float: left; width: 170px; margin-left: 5px; margin-top: -5px; height: 150px;'>   <img style='height: 75px' src='data:image/jpeg;base64,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' /><br /><br /></div><h2>INDICATIVE: 16 to 19 academies revenue funding allocation statement: 2021 to 2022</h2><span style='font-size: 10px;'>Please download our <a href='https://www.gov.uk/government/publications/16-to-19-funding-allocations-supporting-documents-for-2021-to-2022'>supporting guides</a> to help you understand your statement and your revenue funding allocation.</span><br><br>
<table class=""styleTable bold left"" style=""width: 73%; text-align: center !important; border: 2px solid #000;"">


<tbody>

<tr>
<td style=""width: 20%;"">Name</td><td>St Mary's Catholic School</td></tr>
<tr>
<td>UKPRN</td><td>10064744</td></tr>
<tr>
<td>Local authority</td><td>Hertfordshire</td></tr>
<tr>
<td>Open date</td><td>01 November 2021</td></tr></tbody></table><br>
<table class=""styleTable"" style=""width: 100%; border: 2px solid #000;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Summary of 2021 to 2022 funding allocation</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 80%;"">Programme funding</td><td style=""width: 20%;"" class=""right"">&pound;803,165</td></tr>
<tr>
<td>Advanced maths premium</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>High value courses premium</td><td class=""right"">&pound;62,400</td></tr>
<tr>
<td>Industry placements</td><td class=""right"">&pound;19,110</td></tr>
<tr>
<td>High needs student</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Student financial support</td><td class=""right"">&pound;24,782</td></tr>
<tr>
<td>High value courses for school and college leavers</td><td class=""right"">&pound;500</td></tr>
<tr>
<td>Start-up and post-opening grant</td><td class=""right"">&pound;0</td></tr>
<tr>
<td style=""width: 50%;"">16 to 19 Tuition funding</td><td style=""width: 50%;"" class=""right"">&pound;2,000</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold;"">Total funding allocation</td><td class=""right"">&pound;909,457</td></tr></tbody></table><div class=""smallBr""></div>
<table class=""styleTable"" style=""width: 100%;"">


<thead>
    <tr>
<th colspan=""2"" scope=""col"">Start-up and post-opening grant</th>    </tr>
</thead>
<tbody>

<tr>
<td style=""width: 80%;""></td><td style=""width: 20%;"" class=""right"">Funding</td></tr>
<tr>
<td>Start-up grant - part A</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Start-up grant - part B</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Post opening grant - per pupil resources</td><td class=""right"">&pound;0</td></tr>
<tr>
<td>Post opening grant - leadership diseconomies</td><td class=""right"">&pound;0</td></tr>
<tr class=""greyBold"">
<td style=""font-weight: bold"">Total start-up and post-opening grant</td><td style=""font-weight: bold;"" class=""right"">&pound;0</td></tr></tbody></table><div class=""smallBr""></div><style>body { font-family: Arial; } h2 { font-size: 20px; font-weight: normal; } h3 { font-size: 16px; margin: 20px 0; } table td, table th, table caption { font-size: 13px; padding: 3px; } #formulaTables th, #formulaTables td { padding: 1px; } table.styleTable caption, table.styleTable thead th, .greyBold, .greyBold td { text-align: left; font-weight: bold; } .greyBold > td:nth-child(1) { font-weight: normal; } table.styleTable { border-collapse: collapse; } table.styleTable tbody td, .top { vertical-align: top; } table.styleTable, table.styleTable th, table.styleTable td { border: 2px solid #000; } table.styleTable thead th, .greyBold, table caption { background-color: #CCC; } .noStyle, .noStyle * { background-color: #FFF !important; } .noBold, .noBold * { font-weight: normal !important; } .bold, .bold * { font-weight: bold !important; } .center, .center * { text-align: center !important; } .right, .right * { text-align: right !important; }.left, .left * { text-align: left !important; } #page2FirstTable td { width: 25%; } table caption { border: 2px solid #000; border-bottom: 0; } .whiteBackground td, .whiteBackground th { background-color: #FFF !important; font-weight: normal !important; } .column1OnwardsRightAlign > td:nth-child(1), .column1OnwardsRightAlign > td:nth-child(2), .column1OnwardsRightAlign > td:nth-child(3), .column1OnwardsRightAlign > td:nth-child(4), .column1OnwardsRightAlign > td:nth-child(5), .column1OnwardsRightAlign > td:nth-child(6), .column1OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns2OnwardsRightAlign > td:nth-child(2), .columns2OnwardsRightAlign > td:nth-child(3), .columns2OnwardsRightAlign > td:nth-child(4), .columns2OnwardsRightAlign > td:nth-child(5), .columns2OnwardsRightAlign > td:nth-child(6), .columns2OnwardsRightAlign > td:nth-child(7) { text-align: right }.columns3OnwardsRightAlign > td:nth-child(3), .columns3OnwardsRightAlign > td:nth-child(4), .columns3OnwardsRightAlign > td:nth-child(5), .columns3OnwardsRightAlign > td:nth-child(6), .columns3OnwardsRightAlign > td:nth-child(7) { text-align: right } .columns4OnwardsRightAlign > td:nth-child(4), .columns4OnwardsRightAlign > td:nth-child(5), .columns4OnwardsRightAlign > td:nth-child(6), .columns4OnwardsRightAlign > td:nth-child(7) { text-align: right } .smallBr { height: 10px; }</style></body></html>";


        private string GetChecksum(string inputString)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(inputString)));
            }
        }

        private async Task<string> GetHtml(string providerFundingId, string layoutId, string json, string layoutName, string fundingStreamCode, int fundingStreamId, string fundingViewScope, string year = "AY-2122", string fundingId = null)
        {
            var cosmosClient = new CosmosClient(_applicationConfiguration.CosmosDbConfiguration.ConnectionString);
            var container = cosmosClient
                .GetDatabase(_applicationConfiguration.CosmosDbConfiguration.DatabaseName)
                .GetContainer(_applicationConfiguration.CosmosDbConfiguration.LayoutCollection);

            var cosmosDocument = await GetItemFromCosmos(container, layoutId);
            var existsInCosmos = cosmosDocument != null;
            var cosmosFileContents = existsInCosmos ? JsonConvert.SerializeObject(cosmosDocument["Data"]) : null;
            var cosmosChecksum = existsInCosmos ? GetChecksum(cosmosFileContents) : null;

            var fileSystemPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace(@"file:\", string.Empty);
            fileSystemPath = Path.Combine(fileSystemPath, "FundingUIModels/PDF", json);
            var fileSystemFileContents = File.ReadAllText(fileSystemPath);
            var fileContentsDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileSystemFileContents);
            var reserialisedFileSystemFileContents = JsonConvert.SerializeObject(fileContentsDictionary);
            var fileSystemChecksum = GetChecksum(reserialisedFileSystemFileContents);

            if (!existsInCosmos || fileSystemChecksum != cosmosChecksum)
            {
                var item = new Dictionary<string, object>
                {
                    {
                        "Data", fileContentsDictionary
                    },
                    {
                        "LayoutName", layoutName
                    },
                    {
                        "FundingStreamId", fundingStreamId
                    },
                    {
                        "FundingViewType", "Pdf"
                    },
                    {
                        "FundingViewScope", fundingViewScope
                    },
                    {
                        "CreatedDate", DateTime.Now
                    },
                    {
                        "LastModifiedDateTime", DateTime.Now
                    },
                    {
                        "DeletedDateTime", null
                    },
                    {
                        "LastModifiedBy",  "Manually created"
                    },
                    {
                        "CollectionName", _applicationConfiguration.CosmosDbConfiguration.LayoutCollection
                    },
                    {
                        "id", layoutId
                    }
                };

                await container.UpsertItemAsync(item, new PartitionKey(layoutId));
            }

            var basePath = "view-latest-funding/api/external/render";

            if (!string.IsNullOrEmpty(providerFundingId))
            {
                basePath += $"?providerfundingid={providerFundingId}";
            }
            else
            {
                basePath += $"?fundingid={fundingId}";
            }

            var requestUri = new Uri(
                new Uri(_baseUrl),
                $"{basePath}&fundingStreamCode={fundingStreamCode}&fundingPeriodCode={year}&cutoffDate=2030-01-01&layoutId={layoutId}");

            var response = await _httpClient.GetAsync(requestUri);
            var actualHtml = await response.Content.ReadAsStringAsync();

            return actualHtml;
        }
    }
}