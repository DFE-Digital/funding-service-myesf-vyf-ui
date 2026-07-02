using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.IO;
using System.Reflection;

namespace PDS.ViewYourFunding.Web.Tests.Unit
{
    /// <summary>
    /// The JsonMappingTests class.
    /// </summary>
    [TestClass]
    public class JsonMappingTests
    {
        /// <summary>
        /// Fundings the UI model json files exist and can be parsed.
        /// </summary>
        /// <param name="path">The path.</param>
        [TestMethod, TestCategory("Unit")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion1.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion2.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion3.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion4.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion5.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion6.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion7.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion8.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion9.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion1_Organisation.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion2_Organisation.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion3_Organisation.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion4_Organisation.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion5_Organisation.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion6_Organisation.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion7_Organisation.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion8_Organisation.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion9_Organisation.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/PSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion1.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/PSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion1_Organisation.json")]
        [DataRow("Views/FundingUIModels/Spreadsheet/PSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_ModelVersion1_Provider.json")]
        [DataRow("Views/FundingUIModels/ViewData/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_Organisation.json")]
        [DataRow("Views/FundingUIModels/ViewData/PSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_Organisation.json")]
        [DataRow("Views/FundingUIModels/ViewData/DSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_OrganisationSummary.json")]
        [DataRow("Views/FundingUIModels/ViewData/PSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_OrganisationSummary.json")]
        [DataRow("Views/FundingUIModels/ViewData/PSG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_Provider.json")]
        public void FundingUIModelJsonFiles_ExistAndCanBeParsed(string path)
        {
            var p = Path.GetDirectoryName(Assembly.GetAssembly(typeof(JsonMappingTests)).Location);

            // Act
            var readAsString = File.ReadAllText(Path.Combine(p, path));
            UiModel model = null;

            Action act = () => { model = JsonConvert.DeserializeObject<UiModel>(readAsString); };

            // Assert
            act.Should().NotThrow();
            model.Should().NotBeNull();
        }
    }
}