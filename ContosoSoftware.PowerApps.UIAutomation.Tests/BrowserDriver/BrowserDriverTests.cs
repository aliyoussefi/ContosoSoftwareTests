using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoSoftware.PowerApps.UIAutomation.Tests
{
    [TestClass]
    public class BrowserDriverTests
    {
        private static string _username = "";
        private static string _password = "";
        private static BrowserType _browserType;
        private static Uri _xrmUri;
        private static string _browserVersion = "";
        private static string _driversPath = "";
        public TestContext TestContext { get; set; }

        private static TestContext _testContext;

        [ClassInitialize]
        public static void Initialize(TestContext TestContext) {
            _testContext = TestContext;

            _username = _testContext.Properties["OnlineUsername"].ToString();
            _password = _testContext.Properties["OnlinePassword"].ToString();
            _xrmUri = new Uri(_testContext.Properties["OnlineCrmUrl"].ToString());
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties["BrowserType"].ToString());
            _browserVersion = _testContext.Properties["BrowserVersion"].ToString();
            _driversPath = _testContext.Properties["DriversPath"].ToString();
        }
        [TestMethod]
        public void UseWebDriverManager()
        {

            //The NuGet packages rely on versions of .NET Framework not used by EasyRepro
            //WebDriverManager
        }

        [TestCategory("DriverTests")]
        [TestMethod]
        public void UseDriversPath() {
            string[] browserVersionSplit = _browserVersion.Split('.');

            switch (browserVersionSplit[0]) {
                case "86":
                    TestSettings.SharedOptions.DriversPath = Path.Combine(Directory.GetCurrentDirectory()) + @"BrowserDriver\Drivers\86";
                    TestSettings.Options.DriversPath = Path.Combine(Directory.GetCurrentDirectory()) + @"BrowserDriver\Drivers\86";
                    break;
                case "87":
                    TestSettings.SharedOptions.DriversPath = Path.Combine(Directory.GetCurrentDirectory()) + @"BrowserDriver\Drivers\87";
                    TestSettings.Options.DriversPath = Path.Combine(Directory.GetCurrentDirectory()) + @"BrowserDriver\Drivers\87";
                    break;
                default:
                    //TestSettings.Options.DriversPath = "";
                    break;
            }
            //Path.Combine(Directory.GetCurrentDirectory())
            if (!String.IsNullOrEmpty(_driversPath)) {
                TestSettings.SharedOptions.DriversPath = _driversPath;
                TestSettings.Options.DriversPath = _driversPath;
            }
            Debug.WriteLine("Debug DriversPath runsettings=" + _driversPath);
            using (var browser = new InteractiveBrowser(TestSettings.Options)) {
                Debug.WriteLine("Debug SharedOptions.DriversPath=" + TestSettings.SharedOptions.DriversPath);
                browser.Driver.Navigate().GoToUrl("http://www.bing.com");
            }

        }

    }
}
