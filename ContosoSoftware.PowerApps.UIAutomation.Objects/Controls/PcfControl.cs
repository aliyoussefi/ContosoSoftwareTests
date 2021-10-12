using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoSoftware.PowerApps.UIAutomation.Objects.Controls {
    public class PcfControl {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public PcfContainerControl Container { get; set; }
        public List<PcfControlInput> Inputs { get; set; }
        public List<PcfControlAction> Actions { get; set; }
        public PcfControlGrid Grid { get; set; }
    }
    public class PcfControlInput : PcfControl {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
    public class PcfControlAction : PcfControl {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class PcfControlGrid : PcfControl {
        public string Type { get; set; }
        public List<PcfControlGridRecord> Records { get; set; }
        public List<PcfControlInput> Inputs { get; set; }
        public List<PcfControlAction> Actions { get; set; }
    }
    public class PcfControlGridRecord : PcfControl {
        public string Name { get; set; }
        public string Value { get; set; }
        public List<PcfControlInput> Inputs { get; set; }
    }
    public class PcfContainerControl {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        
    }
}
