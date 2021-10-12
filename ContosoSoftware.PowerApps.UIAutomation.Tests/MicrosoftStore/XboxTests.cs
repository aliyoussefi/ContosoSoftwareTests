using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoSoftware.PowerApps.UIAutomation.Tests.MicrosoftStore
{
    [TestClass]
    public class XboxTests
    {
        [TestMethod]
        [TestCategory("Halo")]
        [TestCategory("Sales")]
        [Priority(1)]
        public void NavigateToMicrosoftStore()
        {

            //Click on link
            //Click on Devices
            
        }

        [TestMethod]
        [TestCategory("Halo")]
        [TestCategory("Sales")]
        [Priority(2)]
        [TestProperty("Genre", "Shooter")]
        [TestProperty("Association", "CFC")]
        public void ConfirmRatingForHalo()
        {

            //Click on link
            //Click on Devices
            //Click on New Xbox Series X
            //Click ON Games
            //Click on Learn More
            //Confirm Rating.
        }

        [TestMethod]
        [TestCategory("Halo")]
        [TestCategory("Sales")]
        [Priority(2)]
        [TestProperty("Genre", "Shooter")]
        [TestProperty("Association", "CFC")]
        public void ConfirmXobxSeriesXIsInStock() {

            //Click on link
            //Click on Devices
            //Click on New Xbox Series X
            //Click ON Games
            //Click on Learn More
            //Confirm Rating.
        }

        [TestMethod]
        [TestCategory("Minecraft")]
        [TestCategory("Sales")]
        [Priority(2)]
        [TestProperty("Genre", "Sandbox")]
        [TestProperty("Association", "AgNewMexico")]
        public void ConfirmRatingForMinecraft()
        {

            //Click on link
            //Click on Devices
            //Click on New Xbox Series X
            //Click ON Games
            //Click on Learn More
            //Confirm Rating.
        }
    }
}
