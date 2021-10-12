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
    public class Lookup : Element
    {
        #region prop
        private readonly WebClient _client;
        #endregion
        #region ctor
        public Lookup(WebClient client) : base() {
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
        /// Searches records in a lookup control
        /// </summary>
        /// <param name="control">LookupItem with the name of the lookup field</param>
        /// <param name="searchCriteria">Criteria used for search</param>
        public BrowserCommandResult<bool> Search(LookupItem control, string searchCriteria) {
            //BrowserPage browserPage = new BrowserPage();
            return _client.Execute(GetOptions("Search Lookup Record"), driver =>
            {
                //Click in the field and enter values
                control.Value = searchCriteria;
                //SetValue(control, FormContextType.Entity);
                Trace.WriteLine("WaitForTransaction " + DateTime.Now);
                driver.WaitForTransaction();

                IWebElement fieldContainer = null;
                Trace.WriteLine("ValidateFormContext " + DateTime.Now);
                fieldContainer = ValidateFormContext(driver, FormContextType.Entity, control.Name, fieldContainer);
                Trace.WriteLine("TryRemoveLookupValue " + DateTime.Now);
                TryRemoveLookupValue(driver, fieldContainer, control);
                //TrySetValue(driver, fieldContainer, control);
                IWebElement input;
                Trace.WriteLine("TryFindElement " + DateTime.Now);
                bool found = fieldContainer.TryFindElement(By.TagName("input"), out input);
                string value = control.Value?.Trim();
                if (found) {
                    Trace.WriteLine("SetInputValue " + DateTime.Now);
                    SetInputValue(driver, input, value);
                }

                Trace.WriteLine("TrySetValue " + DateTime.Now);
                TrySetValue(fieldContainer, control);

                Trace.WriteLine("WaitForTransaction " + DateTime.Now);
                driver.WaitForTransaction();

                return true;
            });
        }
        #endregion
        #region Lookup Helpers
        // Used by SetValue methods to determine the field context
        private IWebElement ValidateFormContext(IWebDriver driver, FormContextType formContextType, string field, IWebElement fieldContainer) {
            if (formContextType == FormContextType.QuickCreate) {
                // Initialize the quick create form context
                // If this is not done -- element input will go to the main form due to new flyout design
                var formContext = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.QuickCreate.QuickCreateFormContext]));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));
            }
            else if (formContextType == FormContextType.Entity) {
                // Initialize the entity form context
                var formContext = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.FormContext]));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));
            }
            else if (formContextType == FormContextType.BusinessProcessFlow) {
                // Initialize the Business Process Flow context
                var formContext = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.BusinessProcessFlowFormContext]));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.TextFieldContainer].Replace("[NAME]", field)));
            }
            else if (formContextType == FormContextType.Header) {
                // Initialize the Header context
                var formContext = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.HeaderContext]));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));
            }
            else if (formContextType == FormContextType.Dialog) {
                // Initialize the Dialog context
                var formContext = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Dialogs.DialogContext]));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));
            }

            return fieldContainer;
        }
        private void SetInputValue(IWebDriver driver, IWebElement input, string value, TimeSpan? thinktime = null) {
            // Repeat set value if expected value is not set
            // Do this to ensure that the static placeholder '---' is removed 
            driver.RepeatUntil(() =>
            {
                input.Clear();
                input.Click();
                input.SendKeys(Keys.Control + "a");
                input.SendKeys(Keys.Control + "a");
                input.SendKeys(Keys.Backspace);
                input.SendKeys(value);
                driver.WaitForTransaction();
            },
                d => input.GetAttribute("value").IsValueEqualsTo(value),
                TimeSpan.FromSeconds(9), 3,
                failureCallback: () => throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {value}. Actual: {input.GetAttribute("value")}")
            );

            driver.WaitForTransaction();
        }
        private static void TryRemoveLookupValue(IWebDriver driver, IWebElement fieldContainer, LookupItem control, bool removeAll = true, bool isHeader = false) {
            var controlName = control.Name;
            fieldContainer.Hover(driver);

            var xpathDeleteExistingValues = By.XPath(AppElements.Xpath[AppReference.Entity.LookupFieldDeleteExistingValue].Replace("[NAME]", controlName));
            var existingValues = fieldContainer.FindElements(xpathDeleteExistingValues);

            var xpathToExpandButton = By.XPath(AppElements.Xpath[AppReference.Entity.LookupFieldExpandCollapseButton].Replace("[NAME]", controlName));
            bool success = fieldContainer.TryFindElement(xpathToExpandButton, out var expandButton);
            if (success) {
                expandButton.Click(true);

                var count = existingValues.Count;
                fieldContainer.WaitUntil(x => x.FindElements(xpathDeleteExistingValues).Count > count);
            }
            else if (!isHeader && !success) {
                var xpathToHoveExistingValue = By.XPath(AppElements.Xpath[AppReference.Entity.LookupFieldHoverExistingValue].Replace("[NAME]", controlName));
                var found = fieldContainer.TryFindElement(xpathToHoveExistingValue, out var existingList);
                if (found)
                    existingList.Click(true);
            }

            fieldContainer.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookupSearchButton].Replace("[NAME]", controlName)));

            existingValues = fieldContainer.FindElements(xpathDeleteExistingValues);
            if (existingValues.Count == 0)
                return;

            if (removeAll) {
                // Removes all selected items

                while (existingValues.Count > 0) {
                    foreach (var v in existingValues)
                        v.Click(true);

                    existingValues = fieldContainer.FindElements(xpathDeleteExistingValues);
                }

                return;
            }

            // Removes an individual item by value or index
            var value = control.Value;
            if (value == null)
                throw new InvalidOperationException($"No value or index has been provided for the LookupItem {controlName}. Please provide an value or an empty string or an index and try again.");

            if (value == string.Empty) {
                var index = control.Index;
                if (index >= existingValues.Count)
                    throw new InvalidOperationException($"Field '{controlName}' does not contain {index + 1} records. Please provide an index value less than {existingValues.Count}");

                existingValues[index].Click(true);
                return;
            }

            var existingValue = existingValues.FirstOrDefault(v => v.GetAttribute("aria-label").EndsWith(value));
            if (existingValue == null)
                throw new InvalidOperationException($"Field '{controlName}' does not contain a record with the name:  {value}");

            existingValue.Click(true);
        }
        private void TrySetValue(ISearchContext container, LookupItem control) {
            string value = control.Value;
            if (value == null)
                control.Value = string.Empty;
            // throw new InvalidOperationException($"No value has been provided for the LookupItem {control.Name}. Please provide a value or an empty string and try again.");
            
            if (control.Value == string.Empty) {
                Trace.WriteLine("SetLookupByIndex " + DateTime.Now);
                SetLookupByIndex(container, control);
            }
            else {
                Trace.WriteLine("SetLookUpByValue " + DateTime.Now);
                SetLookUpByValue(container, control);
            }
        }
        private void SetLookupByIndex(ISearchContext container, LookupItem control) {
            var controlName = control.Name;
            var xpathToControl = By.XPath(AppElements.Xpath[AppReference.Entity.LookupResultsDropdown].Replace("[NAME]", controlName));
            Trace.WriteLine("WaitUntilVisible for " + AppReference.Entity.LookupResultsDropdown + DateTime.Now);
            var lookupResultsDialog = container.WaitUntilVisible(xpathToControl);

            var xpathFieldResultListItem = By.XPath(AppElements.Xpath[AppReference.Entity.LookupFieldResultListItem].Replace("[NAME]", controlName));
            Trace.WriteLine("WaitUntil for " + AppReference.Entity.LookupFieldResultListItem + DateTime.Now);
            container.WaitUntil(d => d.FindElements(xpathFieldResultListItem).Count > 0);

            Trace.WriteLine("GetListItems " + DateTime.Now);
            var items = GetListItems(lookupResultsDialog, control);
            if (items.Count == 0)
                throw new InvalidOperationException($"No results exist in the Recently Viewed flyout menu. Please provide a text value for {controlName}");

            int index = control.Index;
            if (index >= items.Count)
                throw new InvalidOperationException($"Recently Viewed list does not contain {index} records. Please provide an index value less than {items.Count}");

            var selectedItem = items.ElementAt(index);
            selectedItem.Click(true);
        }
        private void SetLookUpByValue(ISearchContext container, LookupItem control) {
            var controlName = control.Name;
            var xpathToText = AppElements.Xpath[AppReference.Entity.LookupFieldNoRecordsText].Replace("[NAME]", controlName);
            var xpathToResultList = AppElements.Xpath[AppReference.Entity.LookupFieldResultList].Replace("[NAME]", controlName);
            var bypathResultList = By.XPath(xpathToText + "|" + xpathToResultList);
            Trace.WriteLine("WaitUntilAvailable for " + AppReference.Entity.LookupFieldResultList + DateTime.Now);
            container.WaitUntilAvailable(bypathResultList, TimeSpan.FromSeconds(1));

            var byPathToFlyout = By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookupMenu].Replace("[NAME]", controlName));
            Trace.WriteLine("WaitUntilClickable for " + AppReference.Entity.TextFieldLookupMenu + DateTime.Now);
            var flyoutDialog = container.WaitUntilClickable(byPathToFlyout);
            Trace.WriteLine("GetListItems " + DateTime.Now);
            var items = GetListItems(flyoutDialog, control);

            if (items.Count == 0)
                throw new InvalidOperationException($"List does not contain a record with the name:  {control.Value}");

            int index = control.Index;
            if (index >= items.Count)
                throw new InvalidOperationException($"List does not contain {index + 1} records. Please provide an index value less than {items.Count} ");

            var selectedItem = items.ElementAt(index);
            selectedItem.Click(true);
        }
        private static ICollection<IWebElement> GetListItems(IWebElement container, LookupItem control) {
            var name = control.Name;
            var xpathToItems = By.XPath(AppElements.Xpath[AppReference.Entity.LookupFieldResultListItem].Replace("[NAME]", name));

            Trace.WriteLine("WaitUntil for .//li/div/label/span" + DateTime.Now);
            //wait for complete the search
            container.WaitUntil(d => d.FindVisible(By.XPath(".//li/div/label/span"))?.Text?.Contains(control.Value, StringComparison.OrdinalIgnoreCase) == true, TimeSpan.FromSeconds(1));

            Trace.WriteLine("WaitUntil for " + AppReference.Entity.LookupFieldResultListItem + DateTime.Now);
            ICollection<IWebElement> result = container.WaitUntil(
                d => d.FindElements(xpathToItems),
                failureCallback: () => throw new InvalidOperationException($"No Results Matching {control.Value} Were Found.")
                );
            return result;
        }
        #endregion
    }
}
