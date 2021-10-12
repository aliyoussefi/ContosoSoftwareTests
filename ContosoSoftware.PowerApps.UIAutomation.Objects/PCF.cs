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
    public class PCF : Element
    {
        #region prop
        private readonly WebClient _client;
        #endregion
        #region ctor
        public PCF(WebClient client) : base() {
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
        /// <summary>
        /// Find Element
        /// </summary>
        /// <param name="control">LookupItem with the name of the lookup field</param>
        /// <param name="searchCriteria">Criteria used for search</param>
        public BrowserCommandResult<PcfControl> FindContainer(PcfContainerControl control) {
            //BrowserPage browserPage = new BrowserPage();
            return _client.Execute(GetOptions("Search Lookup Record"), driver =>
            {
                
                Trace.WriteLine("WaitForTransaction " + DateTime.Now);
                //driver.WaitForTransaction();

     
                Trace.WriteLine("ValidateFormContext " + DateTime.Now);
                IWebElement pcfContainer = _client.Browser.Driver.FindElement(By.XPath(ContosoAppElements.Xpath[ContosoAppReference.PcfContainer.Container].Replace("[NAME]", control.Name)));
                PcfControl typedPcfContainer = new PcfControl()
                {
                    Name = pcfContainer.GetAttribute("class"),
                    Value = pcfContainer.GetAttribute("data-lp-id"),
                    Type = (pcfContainer.GetAttribute("data-id").Contains("field",StringComparison.OrdinalIgnoreCase)) ? "Input" :"Grid"
                };
                //var existingInputs = fieldContainer.FindElements(By.XPath(ContosoAppElements.Xpath[ContosoAppReference.PcfContainer.InputControlList]));
                var pcfContainerInputs = pcfContainer.FindElements(By.TagName("input"));
                var pcfContainerActions = pcfContainer.FindElements(By.TagName("button"));
                if (pcfContainerInputs != null && pcfContainerInputs.Count > 0) {
                    typedPcfContainer.Inputs = new List<PcfControlInput>();
                    foreach (IWebElement input in pcfContainerInputs) {
                        PcfControlInput pcfControlInput = new PcfControlInput()
                        {
                            Name = input.GetAttribute("placeholder"),
                            Value = input.Text,
                            Type = "input"
                        };
                        typedPcfContainer.Inputs.Add(pcfControlInput);
                    }
                }
                if (pcfContainerActions != null && pcfContainerActions.Count > 0) {
                    typedPcfContainer.Actions = new List<PcfControlAction>();
                    foreach (IWebElement action in pcfContainerActions) {
                        PcfControlAction pcfControlAction = new PcfControlAction()
                        {
                            Name = action.GetAttribute("class"),
                            Value = action.Text,
                            Type = "button"
                        };
                        typedPcfContainer.Actions.Add(pcfControlAction);
                    }
                }

                //TrySetValue(driver, fieldContainer, control);
                Trace.WriteLine("TryFindElement " + DateTime.Now);
                //bool found = fieldContainer.TryFindElement(By.TagName("input"), out input);
                string value = control.Value?.Trim();
                //if (found) {
                //    Trace.WriteLine("SetInputValue " + DateTime.Now);
                //    SetInputValue(driver, input, value);
                //}

                //Trace.WriteLine("TrySetValue " + DateTime.Now);
                //TrySetValue(fieldContainer, control);

                Trace.WriteLine("WaitForTransaction " + DateTime.Now);
                driver.WaitForTransaction();

                return typedPcfContainer;
            });
        }

        public BrowserCommandResult<PcfContainerControl> DocumentContainer(PcfContainerControl control) {
            return _client.Execute(GetOptions("Search Lookup Record"), driver =>
            {

                //Trace.WriteLine("WaitForTransaction " + DateTime.Now);
                //driver.WaitForTransaction();

                //IWebElement fieldContainer = null;
                //Trace.WriteLine("ValidateFormContext " + DateTime.Now);
                //fieldContainer = ValidateFormContext(driver, FormContextType.Entity, control.Name, fieldContainer);
                //Trace.WriteLine("TryRemoveLookupValue " + DateTime.Now);
                //TryRemoveLookupValue(driver, fieldContainer, control);
                ////TrySetValue(driver, fieldContainer, control);
                //IWebElement input;
                //Trace.WriteLine("TryFindElement " + DateTime.Now);
                //bool found = fieldContainer.TryFindElement(By.TagName("input"), out input);
                //string value = control.Value?.Trim();
                //if (found) {
                //    Trace.WriteLine("SetInputValue " + DateTime.Now);
                //    SetInputValue(driver, input, value);
                //}

                //Trace.WriteLine("TrySetValue " + DateTime.Now);
                //TrySetValue(fieldContainer, control);

                //Trace.WriteLine("WaitForTransaction " + DateTime.Now);
                //driver.WaitForTransaction();

                return new PcfContainerControl();
            });
        }

        public BrowserCommandResult<bool> SetMultipickValue(string field, string value) {
            return _client.Execute(GetOptions("SetMultipickValue"), driver =>
            {
                Actions action = new Actions(_client.Browser.Driver);
                _client.Browser.Driver.FindElement(By.XPath("")).SendKeys(value);
                _client.Browser.ThinkTime(2000);
                action.SendKeys(Keys.ArrowDown).Perform();
                action.SendKeys(Keys.Enter).Perform();



                return true;
            });
        }
        
        #endregion
        #region PCF Helpers
 
        #endregion
    }
}
