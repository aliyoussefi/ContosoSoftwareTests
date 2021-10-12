using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoSoftware.PowerApps.UIAutomation.Tests.TestArtifacts {
    [TestClass]
    public class TestArtifacts {

        private static string _username = "";
        private static string _password = "";
        private static string _mfaSecretKey = "";
        private static BrowserType _browserType;
        private static Uri _xrmUri;

        //does not have valid TestContext property. TestContext must be of type TestContext, must be non-static, public and must not be read-only. For example: public TestContext TestContext.

        //private static TestContext testContextInstance;
        //public static TestContext TestContext {
        //    get { return testContextInstance; }
        //    set { testContextInstance = value; }
        //}

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void Initialize(TestContext context) {
            //TestContext = context;
            _username = context.Properties["OnlineUsername"].ToString();
            _password = context.Properties["OnlinePassword"].ToString();
            _xrmUri = new Uri(context.Properties["OnlineCrmUrl"].ToString());
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), context.Properties["BrowserType"].ToString());
            _mfaSecretKey = context.Properties["MfaSecretKey"].ToString();
        }

        [TestCategory("TestArtifacts")]
        [TestCategory("Screenshots")]
        [TestMethod]
        public void TestWorkWithScreenshots() {

            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client)) {
                Trace.WriteLine("Login");

                //ContosoSoftware.PowerApps.UIAutomation.Objects.Login login = new Login(client);
                //login.MfaLogin(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                xrmApp.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());

                string strFileName = String.Empty;

                //This does not render in DevOps Pipeline
                Trace.WriteLine("Creating TIFF Screenshot.");
                ScreenshotImageFormat fileFormat = ScreenshotImageFormat.Tiff;  // Image Format -> Png, Jpeg, Gif, Bmp and Tiff.
                strFileName = String.Format("Screenshot_{0}.{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileFormat);
                client.Browser.TakeWindowScreenShot(strFileName, fileFormat);
                TestContext.AddResultFile(strFileName);

                Trace.WriteLine("Creating BMP Screenshot.");
                fileFormat = ScreenshotImageFormat.Bmp;  // Image Format -> Png, Jpeg, Gif, Bmp and Tiff.
                strFileName = String.Format("Screenshot_{0}.{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileFormat);
                client.Browser.TakeWindowScreenShot(strFileName, fileFormat);
                TestContext.AddResultFile(strFileName);

                Trace.WriteLine("Creating GIF Screenshot.");
                fileFormat = ScreenshotImageFormat.Gif;  // Image Format -> Png, Jpeg, Gif, Bmp and Tiff.
                strFileName = String.Format("Screenshot_{0}.{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileFormat);
                client.Browser.TakeWindowScreenShot(strFileName, fileFormat);
                TestContext.AddResultFile(strFileName);

                Trace.WriteLine("Creating JPEG Screenshot.");
                fileFormat = ScreenshotImageFormat.Jpeg;  // Image Format -> Png, Jpeg, Gif, Bmp and Tiff.
                strFileName = String.Format("Screenshot_{0}.{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileFormat);
                client.Browser.TakeWindowScreenShot(strFileName, fileFormat);
                TestContext.AddResultFile(strFileName);

                Trace.WriteLine("Creating PNG Screenshot.");
                fileFormat = ScreenshotImageFormat.Png;  // Image Format -> Png, Jpeg, Gif, Bmp and Tiff.
                strFileName = String.Format("Screenshot_{0}.{1}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileFormat);
                client.Browser.TakeWindowScreenShot(strFileName, fileFormat);
                TestContext.AddResultFile(strFileName);

                string fileName = client.Browser.Driver.Title + "_Source_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".html";
                Trace.WriteLine("Creating Page Source file with name " + fileName);
                System.IO.File.AppendAllText(fileName, client.Browser.Driver.PageSource);
                TestContext.AddResultFile(fileName);

            }
        }
    }
}
