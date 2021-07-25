//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Core.UIItems;
using Core.UIItems.TabItems;
using Core.UIItems.Finders;
using AcceptanceTestLibrary.ApplicationObserver;
using AcceptanceTestLibrary.Common;
using AcceptanceTestLibrary.Common.Desktop;
using System.Reflection;
using UIComposition.Tests.AcceptanceTest.TestEntities.Page;
using AcceptanceTestLibrary.ApplicationHelper;
using UIComposition.Tests.AcceptanceTest.TestEntities.Assertion;

namespace UIComposition.Tests.AcceptanceTest.ViewDiscovery.Desktop
{
#if DEBUG
    [DeploymentItem(@"..\ViewDiscovery\Desktop\UIComposition\bin\Debug", @"TDWPF")]
    [DeploymentItem(@".\UIComposition.Tests.AcceptanceTest\bin\Debug")]
#else
    [DeploymentItem(@"..\ViewDiscovery\Desktop\UIComposition\bin\Release", @"TDWPF")]
    [DeploymentItem(@".\UIComposition.Tests.AcceptanceTest\bin\Release")]
#endif
    
    [TestClass]
    public class ViewDiscoveryEmployeeModuleDesktopFixture : FixtureBase<WpfAppLauncher>
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            string currentOutputPath = (new System.IO.DirectoryInfo(Assembly.GetExecutingAssembly().Location)).Parent.FullName;
            UICompositionPage<WpfAppLauncher>.LaunchApplication(currentOutputPath + GetDesktopApplication(), GetDesktopApplicationTitle());
            Thread.Sleep(5000);
        }

        /// <summary>
        /// TestCleanup performs clean-up activities after each test method execution
        /// </summary>
        [TestCleanup()]
        public void MyTestCleanup()
        {
            UICompositionPage<WpfAppLauncher>.DisposeApplication();
        }

        #region Test Methods
        [TestMethod]
        public void DesktopViewDiscoveryUILoadTest()
        {
            //check if window handle object is not null
            Assert.IsNotNull(UICompositionPage<WpfAppLauncher>.DesktopWindow, "ViewDiscovery UI composition is not launched.");
        }

        /// <summary>
        /// Validate if the details view of the selected employee are displayed correctly.
        /// 
        /// Repro Steps:
        /// 1. Launch the QS application
        /// 2. Click on the first employee row in the Employee List table
        /// 3. Check if the Details View Tab is displayed, and the number of tab items is 3.
        /// 4. Check if the tab items headers match with "General", "Location" and "Current Projects"
        /// 
        /// Expected Result:
        /// Details View Tab is dispalyed with 3 tab items. 
        /// The tab items headers match with "General", "Location" and "Current Projects"
        /// </summary>
        [TestMethod]
        public void DesktopViewDiscoveryValidateEmployeeSelection()
        {
            UICompositionAssertion<WpfAppLauncher>.AssertEmployeeSelection();
        }

        /// <summary>
        /// Validate General details in the General Tab for selected employee
        /// 
        /// Repro Steps:
        /// 1. Launch the QS Application
        /// 2. Select the first employee row in the Employee List table
        /// 3. Check if the details of the selected employee are displayed in the General tab
        /// 
        /// Expected results:
        /// Employee First Name, Last Name, Phone and Email are correctly displayed in the General Tab
        /// </summary>
        [TestMethod]
        public void DesktopViewDiscoveryValidateEmployeeDetailsGeneralSection()
        {
            UICompositionAssertion<WpfAppLauncher>.AssertEmployeeGeneralData();
        }

        /// <summary>
        /// Validate Location details in the Location Tab for selected employee
        /// 
        /// Repro Steps:
        /// 1. Launch the QS Application
        /// 2. Select the first employee row in the Employee List table
        /// 3. Check if the location details of the selected employee are displayed in the Location tab
        /// 
        /// Expected results:
        /// Loaction Tab has a frame with required data
        /// </summary>
        [TestMethod]
        public void DesktopViewDiscoveryValidateEmployeeDetailsLocationSection()
        {
            UICompositionAssertion<WpfAppLauncher>.AssertEmployeeLocationData();
        }

        /// <summary>
        /// Validate Current Projects details in the Current Projects Tab for selected employee
        /// 
        /// Repro Steps:
        /// 1. Launch the QS Application
        /// 2. Select the first employee row in the Employee List table
        /// 3. Check if the Current Projects of the selected employee are displayed in the Current Projects tab
        /// 
        /// Expected results:
        /// Current Project and Role of the selected Employee are correctly displayed in the Current Projects Tab
        /// </summary>
        [TestMethod]
        public void DesktopViewDiscoveryValidateEmployeeDetailsCurrentProjectsSection()
        {
            UICompositionAssertion<WpfAppLauncher>.AssertEmployeeProjectsData();
        }
        #endregion

        #region Private methods

        private static string GetDesktopApplication()
        {
            return ConfigHandler.GetValue("ViewDiscoveryWpfAppLocation");
        }

        private static string GetDesktopApplicationTitle()
        {
            return new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ViewDiscoveryDesktopApplicationTitle");
        }

        #endregion
    }

}
