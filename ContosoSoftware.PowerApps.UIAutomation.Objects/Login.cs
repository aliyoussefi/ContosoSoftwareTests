using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ContosoSoftware.PowerApps.UIAutomation.Objects {
    public class Login {
        #region prop
        private readonly WebClient _client;
        #endregion
        #region ctor
        public Login(WebClient client) : base() {
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
        public BrowserCommandResult<bool> MfaLogin(Uri uri, SecureString username, SecureString password) {


            return true;
        }

        #endregion

    }
}
