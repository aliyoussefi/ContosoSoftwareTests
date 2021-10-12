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
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoSoftware.PowerApps.UIAutomation.Objects
{
    public class BusinessProcessFlow : Element
    {
        #region prop
        private readonly WebClient _client;
        #endregion
        #region ctor
        public BusinessProcessFlow(WebClient client) : base() {
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
        /// Validates popup and clicks first record.
        /// </summary>
        /// <returns></returns>
        public BrowserCommandResult<bool> ValidateRecordAndClick() {
            //BrowserPage browserPage = new BrowserPage();
            return _client.Execute(GetOptions("Validate Record"), driver =>
            {
                //Look for Popup
                if (driver.HasElement(By.XPath("//div[@id='MscrmControls.Containers.ProcessStageControl-processCrossEntityFlyoutContainer']"))) {
                    var popupContainer = driver.FindElement(By.XPath("//div[@id='MscrmControls.Containers.ProcessStageControl-processCrossEntityFlyoutContainer']"));
                    //Check for records
                    if (popupContainer.FindElements(By.XPath("//label[@data-id='MscrmControls.Containers.ProcessStageControl-processCrossEntityItem']")).Count > 0) {
                        var popupRecords = popupContainer.FindElements(By.XPath("//label[@data-id='MscrmControls.Containers.ProcessStageControl-processCrossEntityItem']"));
                        if (popupRecords.Count > 1) throw new Exception("Container contains more than one record.");
                        Debug.Write("Attempting to click on record");
                        popupRecords.First().Click();
                    }
                    else {
                        throw new Exception("Found Container but did not find any records.");
                    }
                    //find record


                }
                else {
                    return false;
                }

                return true;
            });
        }
        #endregion
    }
}
