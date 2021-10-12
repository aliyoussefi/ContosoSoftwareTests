using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoSoftware.PowerApps.UIAutomation.Tests.Telemetry
{
    [TestClass]
    public class TelemetryTests
    {
        private static string _username = "";
        private static string _password = "";
        private static BrowserType _browserType;
        private static Uri _xrmUri;
        public static ILogger _logger { get; set; }
        public TestContext TestContext { get; set; }

        private static TestContext _testContext;

        [ClassInitialize]
        public static void Initialize(TestContext TestContext)
        {
            _testContext = TestContext;

            _username = _testContext.Properties["OnlineUsername"].ToString();
            _password = _testContext.Properties["OnlinePassword"].ToString();
            _xrmUri = new Uri(_testContext.Properties["OnlineCrmUrl"].ToString());
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties["BrowserType"].ToString());
            _logger = new TraceLogger();
        }
        #region Telemetry
        [TestCategory("Telemetry")]
        [TestMethod]
        public void Test_CapturePerformanceCenterMarkers()
        {
            
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                _logger.Log("Login");
                xrmApp.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                TelemetryExtensions myExtensions = new TelemetryExtensions(client, _logger);
                myExtensions.EnablePerformanceCenter();
                Trace.WriteLine("OpenApp");
                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);
                Trace.WriteLine("OpenSubArea");
                xrmApp.Navigation.OpenSubArea("Service", "Cases");
                Trace.WriteLine("Click New Case");
                xrmApp.CommandBar.ClickCommand("New Case");
                Trace.WriteLine("ThinkTime for 5 seconds");
                xrmApp.ThinkTime(5000);
                myExtensions.ShowHidePerformanceWidget();

                //MetaData Example
                _logger.Log("Get Metadata markers");
                Dictionary<string, string> metadataMarkers = myExtensions.GetMetadataMarkers();
                _logger.Log(metadataMarkers);
                _logger.Log("Get Performance markers");
                //Perf Markers Example
                Dictionary<string, double> kpiMarkers = myExtensions.GetPerformanceMarkers("EditForm");

                TakeScreenshot(client, "Save");
                Trace.WriteLine("Save");
                xrmApp.Entity.Save();
                Trace.WriteLine("Take Screenshot");
                TakeScreenshot(client, "Save");


            }
        }

        [TestCategory("CustomerService")]
        [TestCategory("LookupExample")]
        [TestCategory("ScreenshotExample")]
        [TestMethod]
        public void Test_CreateCaseWithAttachment()
        {

            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                Trace.WriteLine("Login");
                xrmApp.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                Trace.WriteLine("OpenApp");
                xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);
                Trace.WriteLine("OpenSubArea");
                xrmApp.Navigation.OpenSubArea("Service", "Cases");
                Trace.WriteLine("Click New Case");
                xrmApp.Grid.OpenRecord(0);
                Trace.WriteLine("ThinkTime for 5 seconds");
                xrmApp.ThinkTime(5000);

                //Add to Timeline
                xrmApp.Timeline.AddNote("Automating Notes", "here is automated note from EasyRepro.");

                TakeScreenshot(client, "Save");
                
                Trace.WriteLine("Save");
                xrmApp.Entity.Save();
                Trace.WriteLine("Take Screenshot");
                TakeScreenshot(client, "Save");


            }
        }

        private void TakeScreenshot(WebClient client, string fileName)
        {
            ScreenshotImageFormat fileFormat = ScreenshotImageFormat.Tiff;  // Image Format -> Png, Jpeg, Gif, Bmp and Tiff.
            string strFileName = String.Format("Screenshot_{0}.{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileFormat);
            client.Browser.TakeWindowScreenShot(strFileName, fileFormat);
            _testContext.AddResultFile(strFileName);
        }
        #endregion
    }
}
