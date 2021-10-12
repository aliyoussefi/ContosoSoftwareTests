using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoSoftware.PowerApps.UIAutomation.Tests
{
    public class TraceLogger : ILogger
    {
        public void Log(Dictionary<string, string> dictionaryMessages)
        {
            foreach (KeyValuePair<string, string> dict in dictionaryMessages)
            {
                this.Log(dict.Key + " " + dict.Value);
            }
        }

        public void Log(string message)
        {
            Trace.WriteLine(message + " " + DateTime.Now.ToUniversalTime());
        }

        public void Log()
        {
            throw new NotImplementedException();
        }

        public void Log(string message, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
