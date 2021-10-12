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

namespace ContosoSoftware.PowerApps.UIAutomation.Tests.ControlPerformance
{
    [TestClass]
    public class LookupPerformance
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
        #region Lookups
        [TestCategory("Tab")]
        [TestCategory("TabExample")]
        [TestCategory("ScreenshotExample")]
        [TestMethod]
        public void TestSwitchTabWithSingleQuoteInTabName() {

            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client)) {
                Trace.WriteLine("Login");
                xrmApp.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                //Trace.WriteLine("OpenApp");
                //xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);
                //Trace.WriteLine("OpenSubArea");
                //xrmApp.Navigation.OpenSubArea("Service", "Test Harnesses");
                Trace.WriteLine("Click New Test Harness");
                xrmApp.CommandBar.ClickCommand("New");

                ContosoSoftware.PowerApps.UIAutomation.Objects.Form formHelper = new Objects.Form(client);
                ///html/body/div[7]/div/div/div/div[3]/div/div/div/div[2]/button
                xrmApp.Entity.SelectForm("Multiple Tab Form");
                //xrmApp.Entity.SelectTab("Single Quote' Tab");
                formHelper.SelectTab("Single Quote' Tab");
                TakeScreenshot(client, "Save");
                Trace.WriteLine("Save");
                xrmApp.Entity.Save();
                Trace.WriteLine("Take Screenshot");
                TakeScreenshot(client, "Save");


            }
        }

        [TestCategory("Lookup")]
        [TestCategory("LookupExample")]
        [TestCategory("ScreenshotExample")]
        [TestMethod]
        public void TestNewLookupFlow() {

            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client)) {
                Trace.WriteLine("Login");
                xrmApp.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                //Trace.WriteLine("OpenApp");
                //xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);
                //Trace.WriteLine("OpenSubArea");
                //xrmApp.Navigation.OpenSubArea("Service", "Test Harnesses");
                Trace.WriteLine("Click New Test Harness");
                xrmApp.CommandBar.ClickCommand("New");
                Trace.WriteLine("ThinkTime for 5 seconds");
                xrmApp.ThinkTime(5000);
                LookupItem alternateralesrep = new LookupItem { Name = "pfe_customer" };
                Trace.WriteLine("Lookup.Search method start " + DateTime.Now);
                
                //ootb
                xrmApp.Lookup.Search(alternateralesrep, "Adventure Works (sample)");

                xrmApp.Entity.ClearValue(alternateralesrep);
                //custom
                ContosoSoftware.PowerApps.UIAutomation.Objects.Lookup lookup = new Objects.Lookup(client);
                lookup.Search(alternateralesrep, "Adventure Works (sample)");

                ContosoSoftware.PowerApps.UIAutomation.Objects.DateTimeControlCustom datetime = new Objects.DateTimeControlCustom(client);
                datetime.SetTime();
              


                TakeScreenshot(client, "Save");
                Trace.WriteLine("Save");
                xrmApp.Entity.Save();
                Trace.WriteLine("Take Screenshot");
                TakeScreenshot(client, "Save");


            }
        }



        [TestCategory("Lookup")]
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
                LookupItem customer = new LookupItem { Name = "contactid", Value = "Rene Valdes (sample)" };
                Trace.WriteLine("SetLookupItem with SetValue");
                xrmApp.Entity.SetValue(customer);
                //xrmApp.Lookup.Search(customer, customer.Value);
                xrmApp.Entity.SelectLookup(customer); //This does not set the valye, this only pops up the lookup
                //Lookup Example
                //========================
                //Use LookupItem
                //Call Search method
                //Call SelectLookup method
                xrmApp.Entity.ClearValue(customer);
                Trace.WriteLine("New LookupItem created " + DateTime.Now);
                LookupItem alternateralesrep = new LookupItem { Name = "customerid" };
                Trace.WriteLine("Lookup.Search method start " + DateTime.Now);
                xrmApp.Lookup.Search(alternateralesrep, "Contoso Assembly");
                Trace.WriteLine("Lookup.Search method end " + DateTime.Now);
                Trace.WriteLine("Lookup.SelectLookup method start " + DateTime.Now);
                xrmApp.Entity.SelectLookup(alternateralesrep);
                Trace.WriteLine("Lookup.SelectLookup method end " + DateTime.Now);
                //Lookup Example
                //========================


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
