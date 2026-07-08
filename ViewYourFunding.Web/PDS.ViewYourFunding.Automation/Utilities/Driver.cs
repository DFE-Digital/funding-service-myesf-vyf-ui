using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

namespace ViewYourFunding.Automation.Utilities
{
    /// <summary>
    /// The Driver class.
    /// </summary>
    public class Driver
    {
        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static IWebDriver Instance { get; set; }

        /// <summary>
        /// Gets the download directory.
        /// </summary>
        /// <value>
        /// The download directory.
        /// </value>
        public static string DownloadDirectory => Path.Combine(Environment.CurrentDirectory, "Downloads");

        /// <summary>
        /// Initialises this instance.
        /// </summary>
        public static void Initialise()
        {
            CleanUpBeforeTest();

            var options = new ChromeOptions();
            options.AddArgument("no-sandbox");
            options.AddArgument("--incognito");
            options.AddUserProfilePreference("download.default_directory", DownloadDirectory);
            var driverLocation = Environment.GetEnvironmentVariable("ChromeWebDriver") ?? AppDomain.CurrentDomain.BaseDirectory;
            Instance = new ChromeDriver(driverLocation, options, TimeSpan.FromSeconds(180));
        }

        /// <summary>
        /// Cleans up before test.
        /// </summary>
        public static void CleanUpBeforeTest()
        {
            if (!Directory.Exists("Errors"))
            {
                Directory.CreateDirectory("Errors");
            }

            foreach (var file in Directory.GetFiles("Errors"))
            {
                File.Delete(file);
            }

            if (!Directory.Exists(DownloadDirectory))
            {
                Directory.CreateDirectory(DownloadDirectory);
            }

            foreach (var file in Directory.GetFiles(DownloadDirectory))
            {
                File.Delete(file);
            }
        }

        /// <summary>
        /// Cleans up after test.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="signOut">if set to <c>true</c> [sign out].</param>
        public static void CleanUpAfterTest(TestContext context, bool signOut = true)
        {
            if (context.CurrentTestOutcome == UnitTestOutcome.Error ||
                context.CurrentTestOutcome == UnitTestOutcome.Failed)
            {
                SaveScreenshotAndPageSource(context, $"{context.FullyQualifiedTestClassName}_{context.TestName}");
            }

            if (signOut)
            {
                Navigation.TrySignout();
                Instance.Manage().Cookies.DeleteAllCookies();
            }
        }

        /// <summary>
        /// Saves the screenshot and page source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="fileName">Name of the file.</param>
        public static void SaveScreenshotAndPageSource(TestContext context, string fileName)
        {
            var source = Instance.PageSource;
            var screenshot = Instance.TakeScreenshot();

            var fullDirectoryPath = Path.Combine(Path.GetFullPath("."), "Errors");

            var backslashCount = fullDirectoryPath.ToCharArray().Count(o => o.Equals('\\'));
            var length = fullDirectoryPath.Length;

            var charsLeft = 260 - length - backslashCount;

            if (charsLeft > 0)
            {
                if (fileName.Length > charsLeft && fileName.LastIndexOf(".") > 0)
                {
                    fileName = fileName.Substring(fileName.LastIndexOf("."));
                }

                if (fileName.Length > charsLeft)
                {
                    fileName = fileName.Substring(0, charsLeft - 7);
                }

                var imageFileName = $"{fullDirectoryPath}/{fileName}.png";
                var htmlFileName = $"{fullDirectoryPath}/{fileName}.html";

                screenshot.SaveAsFile(imageFileName);
                File.WriteAllText(htmlFileName, source);

                context.AddResultFile(imageFileName);
                context.AddResultFile(htmlFileName);
            }
        }

        /// <summary>
        /// Tears down.
        /// </summary>
        public static void TearDown()
        {
            Instance?.Close();
            Instance?.Quit();
        }

        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The download file.</returns>
        public static string DownloadFile(string url, string fileName)
        {
            var localPath = Path.Combine(DownloadDirectory, fileName);

            using (HttpClient client = new HttpClient(new HttpClientHandler { UseCookies = false }))
            {
                client.DefaultRequestHeaders.Add("Cookie", CookieString);
                using HttpResponseMessage response = client.GetAsync(url).Result;
                using Stream streamToReadFrom = response.Content.ReadAsStreamAsync().Result;
            }

            return localPath;
        }

        /// <summary>
        /// Gets the cookie string.
        /// </summary>
        /// <value>
        /// The cookie string.
        /// </value>
        private static string CookieString
        {
            get
            {
                var cookies = Instance.Manage().Cookies.AllCookies;
                return string.Join("; ", cookies.Select(c => $"{c.Name}={c.Value}"));
            }
        }
    }
}