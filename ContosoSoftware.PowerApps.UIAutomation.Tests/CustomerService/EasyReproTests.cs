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

namespace ContosoSoftware.PowerApps.UIAutomation.Tests.EasyReproApp
{
    [TestClass]
    public class EasyReproTests
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
        [TestCategory("EasyReproTests")]
        [TestCategory("AllAttributes")]
        [TestCategory("ScreenshotExample")]
        [TestMethod]
        public void TestOpenAndInteractWithAllTypesOfAttributesInEasyReproUCI()
        {

            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                //Login
                Trace.WriteLine("Login");
                xrmApp.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());
                //Open App
                Trace.WriteLine("OpenApp");
                //xrmApp.Navigation.OpenApp(UCIAppName.EasyRepro);
                //Open Test Harnesses
                Trace.WriteLine("OpenSubArea");
                xrmApp.Navigation.OpenSubArea("Entities", "Test Harness");
                //New Harness
                Trace.WriteLine("Click New Harness");
                xrmApp.CommandBar.ClickCommand("New");
                //Wait
                Trace.WriteLine("ThinkTime for 5 seconds");
                xrmApp.ThinkTime(5000);
                //Work with Form
                #region Attributes
                #region Auto Number
                //Work with AutoNumber
                Trace.WriteLine("SetValue pfe_autonumber");
                xrmApp.Entity.SetValue("pfe_autonumber", TestSettings.GetRandomString(5, 10));
                #endregion
                #region Currency
                //Work with Currency
                Trace.WriteLine("SetValue pfe_currency");
                xrmApp.Entity.SetValue("pfe_currency", TestSettings.GetRandomString(5, 10));
                #endregion
                #region Customer
                //Work with Lookup
                //Trace.WriteLine("SetValue pfe_customer");
                //LookupItem pfe_customer = new LookupItem() { Name = "pfe_customer", Value = "Adventure Works (Sample)", Index = 0 };
                //xrmApp.Entity.SetValue(pfe_customer);
                #endregion
                #region DateTime
                try {
                    //Work with DateTime
                    Trace.WriteLine("SetValue pfe_dateandtime");
                    DateTime expectedDate = DateTime.Today.AddDays(1).AddHours(10);
                    xrmApp.Entity.SetValue("pfe_dateandtime", expectedDate);
                    DateTime? date = xrmApp.Entity.GetValue(new DateTimeControl("pfe_dateandtime"));
                }
                catch (Exception ex){
                    string friendlyMessage = "Error occured in pfe_dateandtime";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw;
                }
                try
                {
                    //Work with DateOnly
                    Trace.WriteLine("SetValue pfe_dateonly");
                    DateTime expectedDate = DateTime.Today.AddDays(1).AddHours(10);
                    xrmApp.Entity.SetValue("pfe_dateonly", expectedDate);
                    DateTime? date = xrmApp.Entity.GetValue(new DateTimeControl("pfe_dateonly"));
                }
                catch (Exception ex) {
                    string friendlyMessage = "Error occured in pfe_dateonly";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Decimal
                try {
                    //Work with Decimal
                    Trace.WriteLine("SetValue pfe_decimalnumber");
                    xrmApp.Entity.SetValue("pfe_decimalnumber", ".001");
                    string decimalReturn = xrmApp.Entity.GetValue("pfe_decimalnumber");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_decimalnumber";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Duration
                try {
                    //Work with Duration
                    Trace.WriteLine("SetValue pfe_duration");
                    xrmApp.Entity.SetValue("pfe_duration", "1 hour");
                    string decimalReturn = xrmApp.Entity.GetValue("pfe_duration");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_duration";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Email
                try {
                    //Work with Email
                    Trace.WriteLine("SetValue pfe_email");
                    xrmApp.Entity.SetValue("pfe_email", "easyreprosupport@contoso.com");
                    string emailReturn = xrmApp.Entity.GetValue("pfe_email");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_email";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region File
                try {
                    //Work with File
                    Trace.WriteLine("SetValue pfe_file");
                    xrmApp.Entity.SetValue("pfe_file", "easyreprosupport@contoso.com");
                    string fileReturn = xrmApp.Entity.GetValue("pfe_file");
                }
                catch (Exception ex){
                    string friendlyMessage = "Error occured in pfe_file";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Floating Point Number
                try {
                    //Work with Floating Point Number
                    Trace.WriteLine("SetValue pfe_floatingpointnumber");
                    xrmApp.Entity.SetValue("pfe_floatingpointnumber", "100.001");
                    string pfe_floatingpointnumberReturn = xrmApp.Entity.GetValue("pfe_floatingpointnumber");
                }
                catch (Exception ex)
                {
                    string friendlyMessage = "Error occured in pfe_floatingpointnumber";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region File
                try {
                    //Work with Image
                    Trace.WriteLine("SetValue pfe_image");
                    xrmApp.Entity.SetValue("pfe_image", "test.jpg");
                    string imageReturn = xrmApp.Entity.GetValue("pfe_image");
                }
                catch (Exception ex) {
                    string friendlyMessage = "Error occured in pfe_image";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Language
                try {
                    //Work with Language
                    Trace.WriteLine("SetValue pfe_language");
                    xrmApp.Entity.SetValue("pfe_language", "Spanish");
                    string languageReturn = xrmApp.Entity.GetValue("pfe_language");
                }
                catch (Exception ex) {
                    string friendlyMessage = "Error occured in pfe_language";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Lookup
                try {
                    //Work with Lookup
                    Trace.WriteLine("SetValue pfe_lookup");
                    LookupItem pfe_customer = new LookupItem() { Name = "pfe_lookup", Value = "Adventure Works (Sample)", Index = 0 };
                    xrmApp.Entity.SetValue(pfe_customer);
                }
                catch (Exception ex) {
                    string friendlyMessage = "Error occured in pfe_lookup";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Multi Line Textbox
                try {
                    //Work with Multi Line Textbox
                    Trace.WriteLine("SetValue pfe_multilinetext");
                    xrmApp.Entity.SetValue("pfe_multilinetext", "Spanish");
                    string multiLineTextReturn = xrmApp.Entity.GetValue("pfe_multilinetext");
                }
                catch (Exception ex) {
                    string friendlyMessage = "Error occured in pfe_multilinetext";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Multi Select Option Set
                try {
                    //Work with Multi Select Option Set
                    Trace.WriteLine("SetValue pfe_multiselectoptionset");
                    xrmApp.Entity.SetValue("pfe_multiselectoptionset", "Spanish");
                    string multiselectOptionSetReturn = xrmApp.Entity.GetValue("pfe_multiselectoptionset");
                }
                catch (Exception ex) {
                    string friendlyMessage = "Error occured in pfe_multiselectoptionset";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Text
                try {
                    //Work with Text
                    Trace.WriteLine("SetValue pfe_name");
                    xrmApp.Entity.SetValue("pfe_name", TestSettings.GetRandomString(5, 10));
                    string textReturn = xrmApp.Entity.GetValue("pfe_name");
                }
                catch (Exception ex) {
                    string friendlyMessage = "Error occured in pfe_name";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Option Set
                try {
                    //Work with Option Set
                    Trace.WriteLine("SetValue pfe_optionset");
                    xrmApp.Entity.SetValue(new OptionSet { Name = "pfe_optionset", Value = "Allowed" });
                    string textReturn = xrmApp.Entity.GetValue("pfe_optionset");
                }
                catch (Exception ex) {
                    string friendlyMessage = "Error occured in pfe_optionset";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Text Area
                try {
                    //Work with Text Area
                    Trace.WriteLine("SetValue pfe_textarea");
                    xrmApp.Entity.SetValue("pfe_textarea", "test text area");
                    string textAreaReturn = xrmApp.Entity.GetValue("pfe_textarea");
                }
                catch (Exception ex) {
                    string friendlyMessage = "Error occured in pfe_textarea";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Ticker Symbol
                try {
                    //Work with tickersymbol
                    Trace.WriteLine("SetValue pfe_tickersymbol");
                    xrmApp.Entity.SetValue("pfe_tickersymbol", "test text area");
                    string tickersymbolReturn = xrmApp.Entity.GetValue("pfe_tickersymbol");
                }
                catch (Exception ex) {
                    string friendlyMessage = "Error occured in pfe_tickersymbol";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Time Zone
                try {
                    //Work with pfe_timezone
                    Trace.WriteLine("SetValue pfe_timezone");
                    xrmApp.Entity.SetValue("pfe_timezone", "test text area");
                    string pfe_timezoneReturn = xrmApp.Entity.GetValue("pfe_timezone");
                }
                catch (Exception ex) {
                    string friendlyMessage = "Error occured in pfe_timezone";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Two Options
                try {
                    //Work with pfe_twooptions
                    Trace.WriteLine("SetValue pfe_twooptions");
                    xrmApp.Entity.SetValue(new BooleanItem() { Name = "pfe_twooptions", Value = true });
                    string pfe_timezoneReturn = xrmApp.Entity.GetValue("pfe_twooptions");
                }
                catch (Exception ex) {
                    string friendlyMessage = "Error occured in pfe_twooptions";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Url
                try {
                    //Work with pfe_url
                    Trace.WriteLine("SetValue pfe_url");
                    xrmApp.Entity.SetValue("pfe_url", "https://microsoft.com");
                    string pfe_urlReturn = xrmApp.Entity.GetValue("pfe_url");
                }
                catch (Exception ex) {
                    string friendlyMessage = "Error occured in pfe_url";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #region Whole Number
                try {
                    //Work with pfe_wholenumber
                    Trace.WriteLine("SetValue pfe_wholenumber");
                    xrmApp.Entity.SetValue("pfe_wholenumber", "10000");
                    string pfe_wholenumberReturn = xrmApp.Entity.GetValue("pfe_wholenumber");
                }
                catch (Exception ex) {
                    string friendlyMessage = "Error occured in pfe_wholenumber";
                    Trace.WriteLine(friendlyMessage + " | " + ex.Message);
                    TakeScreenshot(client, friendlyMessage);
                    //throw; 
                }
                #endregion
                #endregion

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
            string strFileName = String.Format("Screenshot_{0}.{1}.{2}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), fileName,fileFormat);
            client.Browser.TakeWindowScreenShot(strFileName, fileFormat);
            _testContext.AddResultFile(strFileName);
        }
        #endregion
    }
}
