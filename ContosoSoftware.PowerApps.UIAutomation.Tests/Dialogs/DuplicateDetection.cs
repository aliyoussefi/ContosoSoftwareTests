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

namespace ContosoSoftware.PowerApps.UIAutomation.Tests.CustomerService
{
    [TestClass]
    public class DuplicateDetectionTests
    {
        private static string _username = "";
        private static string _password = "";
        private static BrowserType _browserType;
        private static Uri _xrmUri;

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
        }
        #region CustomerService
        [TestCategory("CustomerService")]
        [TestCategory("LookupExample")]
        [TestCategory("ScreenshotExample")]
        [TestMethod]
        public void TestCreateAccount()
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
                xrmApp.CommandBar.ClickCommand("New Case");
                Trace.WriteLine("ThinkTime for 5 seconds");
                xrmApp.ThinkTime(5000);
                Trace.WriteLine("SetValue Title");
                xrmApp.Entity.SetValue("title", TestSettings.GetRandomString(5, 10));
                Trace.WriteLine("Switch to Details");
                xrmApp.Entity.SelectTab("Details");
                Trace.WriteLine("new LookupItem");
                LookupItem customer = new LookupItem { Name = "customerid", Value = "Test Lead" };
                Trace.WriteLine("SetLookupItem with SetValue");
                xrmApp.Entity.SetValue(customer);

            //Lookup:

                LookupItem alternateralesrep = new LookupItem { Name = "customerid" };
                xrmApp.ThinkTime(5000);
                xrmApp.Lookup.Search(alternateralesrep, "Test Lead");
                xrmApp.ThinkTime(5000);
                xrmApp.Entity.SelectLookup(alternateralesrep);
                xrmApp.ThinkTime(5000);

            //Date:

                xrmApp.Entity.SetValue("aac_rateeffectivedate", DateTime.Parse("10/10/2020"));


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
        }
        #endregion
    }
}
