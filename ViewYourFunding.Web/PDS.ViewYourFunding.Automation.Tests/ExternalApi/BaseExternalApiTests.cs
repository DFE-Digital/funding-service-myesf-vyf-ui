using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using PDS.ViewYourFunding.Automation.Tests.Config;
using PDS.ViewYourFunding.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Automation.Tests.ExternalApi
{
    public class BaseExternalApiTests
    {
        private static HttpClient _httpClient = null;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly string _baseUrl;
        private readonly string _secretKey;

        public bool CanWriteExpectedHtml { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseExternalApiTests"/> class.
        /// </summary>
        public BaseExternalApiTests()
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

            bool.TryParse(config["CanWriteExpectedHtml"], out var canWriteExpectedHtml);

            CanWriteExpectedHtml = canWriteExpectedHtml;
        }

        protected static void DeleteUriCsv(string nameofTestClass)
        {
            var automationTestAssemblyFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var uriPath = Path.Combine(automationTestAssemblyFolderPath, "ExternalApi", "ExpectedHtml", nameofTestClass, "uri.csv");

            try
            {
                if (File.Exists(uriPath))
                {
                    File.Delete(uriPath);
                }
            }
            catch (Exception)
            {
            }
        }

        protected string ReplaceCurrentDate(string data)
        {
            var regexPattern = "([0-9]{2} [a-zA-Z]+ [0-9]{4}(?=<\\/div>))|(CURRENTDATE)";

            var result = Regex.Replace(data, regexPattern, DateTime.Now.ToString("dd MMMM yyyy"));

            return result;
        }

        protected string GetChecksum(string inputString)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(inputString)));
        }

        protected async Task<string> GetHtml(string providerFundingId, string layoutId, string schema, string fundingPeriodCode = "AC-2122", string fundingStreamCode = "GAG")
        {
            var cosmosClient = new CosmosClient(_applicationConfiguration.CosmosDbConfiguration.ConnectionString, new CosmosClientOptions { ConnectionMode = _applicationConfiguration.CosmosDbConfiguration.CosmosConnectionMode == "Gateway" ? ConnectionMode.Gateway : ConnectionMode.Direct });
            var container = cosmosClient
                .GetDatabase(_applicationConfiguration.CosmosDbConfiguration.DatabaseName)
                .GetContainer(_applicationConfiguration.CosmosDbConfiguration.LayoutCollection);

            var cosmosDocument = await GetItemFromCosmos(container, layoutId);
            var existsInCosmos = cosmosDocument != null;
            var cosmosFileContents = existsInCosmos ? JsonConvert.SerializeObject(cosmosDocument["Data"]) : null;
            var cosmosChecksum = existsInCosmos ? GetChecksum(cosmosFileContents) : null;

            var fileSystemPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace(@"file:\", string.Empty);
            fileSystemPath = Path.Combine(fileSystemPath, "FundingUIModels/PDF", schema);
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
                        "LayoutName", "GAG Provider Pdf Layout"
                    },
                    {
                        "FundingStreamId", 3
                    },
                    {
                        "FundingViewType", "Pdf"
                    },
                    {
                        "FundingViewScope", "Provider"
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

            var requestUri = new Uri(
                new Uri(_baseUrl),
                $"view-latest-funding/api/external/render?providerfundingid={providerFundingId}&fundingStreamCode={fundingStreamCode}&fundingPeriodCode={fundingPeriodCode}& cutoffDate=2030-01-01&layoutId={layoutId}");

            var response = await _httpClient.GetAsync(requestUri);
            var actualHtml = await response.Content.ReadAsStringAsync();

            return actualHtml;
        }

        protected async Task<string> GetExpectedHtmlAsync(string nameofTestClass, string fileName)
        {
            //If ExternalApi Expected Html Files added as embeded resources, then the below code fetch details
            using (var stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream($"PDS.ViewYourFunding.Automation.Tests.ExternalApi.ExpectedHtml.{nameofTestClass}.{fileName}.html"))
            {
                if (stream != null)
                {
                    var streamReader = new StreamReader(stream, Encoding.UTF8);
                    var html = streamReader.ReadToEnd();
                    return html;
                }
            }

            //If ExternalApi Expected Html Files missed to added as Embedded resources, however Copied to Output Directory, below code will send result
            var automationTestAssemblyFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(automationTestAssemblyFolderPath, "ExternalApi", "ExpectedHtml", nameofTestClass, fileName + ".html");

            if (File.Exists(filePath))
            {
                return await File.ReadAllTextAsync(filePath);
            }
            else
            {
                return null;
            }
        }

        protected async Task WriteExpectedHtmlAsync(string nameofTestClass, string fileName, string expectedHtml, string providerFundingId, string layoutId, string schema, string fundingPeriodCode = "AC-2122", string fundingStreamCode = "GAG")
        {
            if (CanWriteExpectedHtml)
            {
                var automationTestAssemblyFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var filePath = Path.Combine(automationTestAssemblyFolderPath, "ExternalApi", "ExpectedHtml", nameofTestClass, fileName + ".html");
                await File.WriteAllTextAsync(filePath, expectedHtml);
                var requestUri = new Uri(
                    new Uri(_baseUrl),
                    $"view-latest-funding/api/external/render?providerfundingid={providerFundingId}&fundingStreamCode={fundingStreamCode}&fundingPeriodCode={fundingPeriodCode}& cutoffDate=2030-01-01&layoutId={layoutId}");

                var requestUriFilePath = Path.Combine(automationTestAssemblyFolderPath, "ExternalApi", "ExpectedHtml", nameofTestClass, $"uri.csv");
                await File.AppendAllTextAsync(requestUriFilePath, fileName + "," + requestUri.AbsoluteUri + "\r\n");
            }
        }

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
    }
}