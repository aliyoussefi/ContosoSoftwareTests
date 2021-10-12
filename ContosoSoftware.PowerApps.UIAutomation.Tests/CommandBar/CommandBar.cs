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

namespace ContosoSoftware.PowerApps.UIAutomation.Tests.TestArtifacts {
    [TestClass]
    public class CommandBarTests {

        private static string _username = "";
        private static string _password = "";
        private static string _mfaSecretKey = "";
        private static BrowserType _browserType;
        private static Uri _xrmUri;

        public TestContext TestContext { get; set; }

        private static TestContext _testContext;


        [ClassInitialize]
        public static void Initialize(TestContext TestContext) {
            _testContext = TestContext;

            _username = _testContext.Properties["OnlineUsername"].ToString();
            _password = _testContext.Properties["OnlinePassword"].ToString();
            _xrmUri = new Uri(_testContext.Properties["OnlineCrmUrl"].ToString());
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties["BrowserType"].ToString());
            _mfaSecretKey = _testContext.Properties["MfaSecretKey"].ToString();
        }

        [TestCategory("CommandBar")]
        [TestMethod]
        public void TestWorkWithSubCommand() {

            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client)) {
                Trace.WriteLine("Login");
                xrmApp.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                //Trace.WriteLine("OpenApp");
                //xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);
                //Trace.WriteLine("OpenSubArea");
                //xrmApp.Navigation.OpenSubArea("Service", "Test Harnesses");
                Trace.WriteLine("Click New Test Harness");

                ContosoSoftware.PowerApps.UIAutomation.Objects.Form formHelper = new Objects.Form(client);
                ///html/body/div[7]/div/div/div/div[3]/div/div/div/div[2]/button
                xrmApp.Entity.SelectForm("Multiple Tab Form");
                //xrmApp.Entity.SelectTab("Single Quote' Tab");
                formHelper.SelectTab("Single Quote' Tab");
                Trace.WriteLine("Save");

                string strFileName = String.Empty;

                Trace.WriteLine("Creating TIFF Screenshot.");
                ScreenshotImageFormat fileFormat = ScreenshotImageFormat.Tiff;  // Image Format -> Png, Jpeg, Gif, Bmp and Tiff.
                strFileName = String.Format("Screenshot_{0}.{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileFormat);
                _testContext.AddResultFile(strFileName);

                Trace.WriteLine("Creating BMP Screenshot.");
                fileFormat = ScreenshotImageFormat.Bmp;  // Image Format -> Png, Jpeg, Gif, Bmp and Tiff.
                strFileName = String.Format("Screenshot_{0}.{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileFormat);
                _testContext.AddResultFile(strFileName);

                Trace.WriteLine("Creating GIF Screenshot.");
                fileFormat = ScreenshotImageFormat.Gif;  // Image Format -> Png, Jpeg, Gif, Bmp and Tiff.
                strFileName = String.Format("Screenshot_{0}.{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileFormat);
                _testContext.AddResultFile(strFileName);

                Trace.WriteLine("Creating JPEG Screenshot.");
                fileFormat = ScreenshotImageFormat.Jpeg;  // Image Format -> Png, Jpeg, Gif, Bmp and Tiff.
                strFileName = String.Format("Screenshot_{0}.{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileFormat);
                _testContext.AddResultFile(strFileName);

                Trace.WriteLine("Creating PNG Screenshot.");
                fileFormat = ScreenshotImageFormat.Png;  // Image Format -> Png, Jpeg, Gif, Bmp and Tiff.
                strFileName = String.Format("Screenshot_{0}.{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileFormat);
                _testContext.AddResultFile(strFileName);

            }
        }
    }
}
