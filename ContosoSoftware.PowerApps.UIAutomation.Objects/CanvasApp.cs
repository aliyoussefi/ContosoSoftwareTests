/*
# This Sample Code is provided for the purpose of illustration only and is not intended to be used in a production environment. 
# THIS SAMPLE CODE AND ANY RELATED INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, 
# INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE. 
# We grant You a nonexclusive, royalty-free right to use and modify the Sample Code and to reproduce and distribute the object code form of the Sample Code, provided that. 
# You agree: 
# (i) to not use Our name, logo, or trademarks to market Your software product in which the Sample Code is embedded; 
# (ii) to include a valid copyright notice on Your software product in which the Sample Code is embedded; 
# and (iii) to indemnify, hold harmless, and defend Us and Our suppliers from and against any claims or lawsuits, including attorneys’ fees, that arise or result from the use or distribution of the Sample Code 
*/
using ContosoSoftware.PowerApps.UIAutomation.Objects.Controls;
using ContosoSoftware.PowerApps.UIAutomation.Objects.DTO;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
//using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoSoftware.PowerApps.UIAutomation.Objects
{
    public class CanvasApp : Element
    {
        #region prop
        private readonly WebClient _client;
        public IWebElement _appFrame {
            get {
                if (_appFrame == null) {
                    _appFrame = establishFrameContext(_client);
                }
                return _appFrame;
            }
            set {
                _appFrame = value;
            }
        }
        #endregion
        #region ctor
        public CanvasApp(WebClient client) : base() {
            _client = client;
        }
        #endregion
        #region Public
        #region Defaults
        internal BrowserCommandOptions GetOptions(string commandName) {
            return new BrowserCommandOptions(Constants.DefaultTraceSource,
                commandName,
                Constants.DefaultRetryAttempts,
                Constants.DefaultRetryDelay,
                null,
                true,
                typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        }
        #endregion

        public BrowserCommandResult<bool> SetValue(Controls.TextBox textboxName, string value) {

            return _client.Execute(GetOptions("Set Canvas App Textbox Value"), driver =>
            {
                Trace.WriteLine("Find Textbox " + DateTime.Now);
                IWebElement textbox = _client.Browser.Driver.FindElement(By.XPath(ContosoAppElements.Xpath[ContosoAppReference.AppMagicTextbox.Container].Replace("[NAME]", textboxName.Name)));
                textbox.SendKeys(value);
                return true;
            });
        }

        private IWebElement establishFrameContext(WebClient client) {
            //Find App Container
            //Wait Until IFrame Class = publishedAppIframe AND aria-hidden="false"
            client.Browser.Driver.WaitUntilVisible(By.XPath(ContosoAppElements.Xpath[ContosoAppReference.AppMagicApp.Frame]));
            IWebElement appFrame = client.Browser.Driver.FindElement(By.XPath(ContosoAppElements.Xpath[ContosoAppReference.AppMagicApp.Frame]));
            client.Browser.Driver.SwitchTo().Frame(appFrame);
            return appFrame;
        }

        public BrowserCommandResult<bool> Click(Controls.Button buttonName) {

            return _client.Execute(GetOptions("Click Canvas App Button"), driver =>
            {
                Trace.WriteLine("Find Button Container" + DateTime.Now);

                IWebElement appFrame = establishFrameContext(_client);
                
                IWebElement buttonContainer = _client.Browser.Driver.FindElement(By.XPath(ContosoAppElements.Xpath[ContosoAppReference.AppMagicButton.Container].Replace("[NAME]", buttonName.Name)));
                Trace.WriteLine("Find Button " + DateTime.Now);
                IWebElement button = buttonContainer.FindElement(By.TagName("button"));
                Trace.WriteLine("Click Button " + DateTime.Now);
                button.Click();
                return true;
            });
        }

        public BrowserCommandResult<bool> Click(Controls.Label labelName) {

            return _client.Execute(GetOptions("Click Canvas App Button"), driver =>
            {
                Trace.WriteLine("Find Button Container" + DateTime.Now);

                IWebElement appFrame = establishFrameContext(_client);

                IWebElement labelContainer = _client.Browser.Driver.FindElement(By.XPath(ContosoAppElements.Xpath[ContosoAppReference.AppMagicButton.Container].Replace("[NAME]", labelName.Name)));
                Trace.WriteLine("Find Button " + DateTime.Now);
                IWebElement button = labelContainer.FindElement(By.TagName("button"));
                Trace.WriteLine("Click Button " + DateTime.Now);
                button.Click();
                return true;
            });
        }




        #endregion
        #region PCF Helpers

        #endregion
    }
}
