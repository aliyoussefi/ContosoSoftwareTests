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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ContosoSoftware.PowerApps.UIAutomation.Objects.DTO {
    public class ContosoAppReference {

        public class ModalForm {
            public static string Container = "ModalForm_Container";
        }

        public class Entity {
            public static string TabList = "Entity_TabList";
            public static string Tab = "Entity_Tab";
        }

        public class PcfContainer {
            public static string Container = "PCF_Container";
            public static string InputControlList = "PCF_InputControlList";
            public static string InputControl = "PCF_InputControl";
        }

        public class AppMagicApp {
            public static string Frame = "AppMagic_App_Frame";
        }

        public class AppMagicTextbox {
            public static string Container = "AppMagic_Textbox_Container";
            public static string Input = "AppMagic_Textbox_Input";
        }

        public class AppMagicButton {
            public static string Container = "AppMagic_Button_Container";
        }
    }

    public class ContosoAppElements {
        public static Dictionary<string, string> Xpath = new Dictionary<string, string>()
        {
            {"AppMagic_App_Container", "//*[@class='publishedAppIframe']"},
            {"AppMagic_App_Frame", "//iframe[@class='publishedAppIframe' && @aria-hidden='false']"},
            //App Magic Textbox
            {"AppMagic_Textbox_Container","//input[@appmagic-control=\"[NAME]textbox\"]" },
            {"AppMagic_Textbox_Input","AppMagic_Textbox_Container"},


            //App Magic Button
            {"AppMagic_Button_Container","//div[@data-control-name=\"[NAME]\"]" },

            { "Entity_TabList", "//ul[contains(@id, \"tablist\")]" },
            { "Entity_Tab", ".//li[@title=\"{0}\"]" },
            { "Entity_MoreTabs", ".//button[@data-id='more_button']" },
            { "Entity_MoreTabsMenu", "//div[@id='__flyoutRootNode']" },
            { "Entity_SubTab", "//div[@id=\"__flyoutRootNode\"]//span[text()=\"{0}\"]" },

            //Modal Form
            { "ModalForm_Container", "//div[contains(@id, 'defaultDialogChromeView')]]" },

            //PCF Controls
            //{ "PCF_Container", "//div[contains(@class, \"customControl\") and contains(@data-id, \"{0}\"]" },
            { "PCF_Container", "//div[contains(@class, \"[NAME]\")]" },
            { "PCF_InputControlList", "//input" },
            { "PCF_InputControl", "//div[contains(@class, \"customControl\")]//input[@id=\"{0}\"]" },
        };
    }
}
