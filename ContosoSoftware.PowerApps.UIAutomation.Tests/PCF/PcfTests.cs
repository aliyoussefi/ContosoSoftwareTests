using ContosoSoftware.PowerApps.UIAutomation.Objects;
using ContosoSoftware.PowerApps.UIAutomation.Objects.Controls;
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

namespace ContosoSoftware.PowerApps.UIAutomation.Tests.ControlPerformance {
    [TestClass]
    public class PcfTests {
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
        #region PCF
        [TestCategory("PCF")]
        [TestCategory("PCFExample")]
        [TestMethod]
        public void TestWorkWithPCFControlOnForm() {

            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client)) {
                Trace.WriteLine("Login");

                //ContosoSoftware.PowerApps.UIAutomation.Objects.Login login = new Login(client);
                //login.MfaLogin(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                xrmApp.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                //Trace.WriteLine("OpenApp");
                //xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);
                Trace.WriteLine("OpenSubArea");
                xrmApp.Navigation.OpenSubArea("App Insights", "PA Control Framework Testers");
                Trace.WriteLine("Click New PA Control Framework Testers");
                xrmApp.CommandBar.ClickCommand("New");
                //customControl SampleNamespace TSLinearInputControl SampleNamespace.TSLinearInputControl


                var pcfBotControl = "";

                ContosoSoftware.PowerApps.UIAutomation.Objects.PCF pcfControl = new Objects.PCF(client);
                PcfControl pcfControl1 =  pcfControl.FindContainer(new Objects.Controls.PcfContainerControl() { Name = "QnAMakerControl" });


                pcfControl.SetMultipickValue("lazard_somtthing", "test");

                //var pcfBotControl = xrmApp.Entity.GetValue("dyw_multiplelinesoftext");
                    xrmApp.Entity.SetValue("dyw_multiplelinesoftext", "Whoose House?");

                client.Browser.Driver.FindElement(By.XPath("//button[@class=\"ask_Button_Style\"]")).Click();
                //Simulate User waiting for 3 seconds
                xrmApp.ThinkTime(3000);
                var pcfBotControlAnswer = client.Browser.Driver.FindElement(By.XPath("//label[@class=\"answer_Input_Style\"]")).Text;
                //answer_Input_Style


                //Side Pane
                //SidePanelIFrame
                client.Browser.Driver.SwitchTo().Frame("SidePanelIFrame");
                if (client.Browser.Driver.HasElement(By.XPath("//input[@data-id=\"webchat-sendbox-input\"]"))) {
                    IWebElement chat = client.Browser.Driver.FindElement(By.XPath("//ul[@aria-relevant=\"additions text\"]"));
                    client.Browser.Driver.FindElement(By.XPath("//input[@data-id=\"webchat-sendbox-input\"]")).SendKeys("what is zach's title");
                    client.Browser.Driver.FindElement(By.XPath("//button[@title=\"Send\"]")).Click();
                    xrmApp.ThinkTime(6000);
                    IList<IWebElement> chatResponses = chat.FindElements(By.TagName("li"));
                    foreach (IWebElement chatResponse in chatResponses) {
                        Debug.WriteLine(chatResponse.Text);
                    }
                }
                Trace.WriteLine("Save");
                xrmApp.Entity.Save();
                Trace.WriteLine("Take Screenshot");
                //TakeScreenshot(client, "Save");


            }
        }

        [TestCategory("PCF")]
        [TestCategory("NavigateTo")]
        [TestMethod]
        public void TestWorkWithNavigateToFormContext() {

            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client)) {
                Trace.WriteLine("Login");

                //ContosoSoftware.PowerApps.UIAutomation.Objects.Login login = new Login(client);
                //login.MfaLogin(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                xrmApp.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                //Trace.WriteLine("OpenApp");
                //xrmApp.Navigation.OpenApp(UCIAppName.CustomerService);
                Trace.WriteLine("OpenSubArea");
                xrmApp.Navigation.OpenSubArea("App Insights", "PA Control Framework Testers");
                Trace.WriteLine("Click New PA Control Framework Testers");
                xrmApp.CommandBar.ClickCommand("New");
                xrmApp.ThinkTime(5000);
                xrmApp.Entity.SelectForm("NavigateTo Reference");
                xrmApp.CommandBar.ClickCommand("NavigateTo");

                Trace.WriteLine("Clicked NavigateTo");
                ContosoSoftware.PowerApps.UIAutomation.Objects.Entity entity = new Objects.Entity(client);
                entity.SetValue("name", "test for Sanat-PCF Test", Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO.FormContextType.Dialog);
                //client.SetValue("name", "test", Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO.FormContextType.Dialog);
                xrmApp.Entity.SetValue("name", "test");
                client.SetValue(new BooleanItem()
                {
                    Name = "fcb_bool",
                    Value = true
                }, Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO.FormContextType.Dialog);
                
            }
        
        }
        #endregion
    }
}
