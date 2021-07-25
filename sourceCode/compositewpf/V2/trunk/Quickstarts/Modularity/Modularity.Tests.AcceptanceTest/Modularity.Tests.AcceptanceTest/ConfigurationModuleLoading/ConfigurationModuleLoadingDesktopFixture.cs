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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using System.Diagnostics;
using System.Threading;

using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Text;
using System.Windows.Automation.Provider;

using AcceptanceTestLibrary.ApplicationHelper;
using AcceptanceTestLibrary.Common;
using Modularity.Tests.AcceptanceTest.TestEntities.Page;
using AcceptanceTestLibrary.ApplicationObserver;
using AcceptanceTestLibrary.Common.Desktop;
using Modularity.Tests.AcceptanceTest.TestEntities.Assertion;
using Modularity.Tests.AcceptanceTest.TestEntities.Action;
using System.Reflection;


namespace Modularity.Tests.AcceptanceTest.ConfigurationModuleLoading.Desktop
{
#if DEBUG
    [DeploymentItem(@"..\ConfigurationModularity\ConfigurationModularity\bin\Debug\")]
    [DeploymentItem(@".\Modularity.Tests.AcceptanceTest\bin\Debug\")]
#else
    [DeploymentItem(@"..\ConfigurationModularity\ConfigurationModularity\bin\Release\")]
    [DeploymentItem(@".\Modularity.Tests.AcceptanceTest\bin\Release\")]
#endif
    [TestClass]
    public class ConfigurationModuleLoadingDesktopFixture : FixtureBase<WpfAppLauncher>
    {
        #region Additional test attributes

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            string currentOutputPath = (new System.IO.DirectoryInfo(Assembly.GetExecutingAssembly().Location)).Parent.FullName;
            Shell<WpfAppLauncher>.Window = base.LaunchApplication(currentOutputPath + GetDesktopApplication(), GetDesktopApplicationProcess())[0];
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            Process p = WpfAppLauncher.GetCurrentAppProcess();
            base.UnloadApplication(p);
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void ConfigurationModuleDesktopApplicationLoadTest()
        {
            //check if window handle object is not null
            Assert.IsNotNull(Shell<WpfAppLauncher>.Window, "Configuration Module Loading is not launched.");
        }

        [TestMethod]
        public void ConfigurationModuleDesktopModuleLoadTest()
        {
            ShellAssertion<WpfAppLauncher>.AssertConfigurationModuleLoading();
        }

        [TestMethod]
        public void ConfigurationModuleDesktopLoadModuleCOnDemand()
        {
            ShellAction<WpfAppLauncher>.LoadModuleCOnDemand();
            ShellAssertion<WpfAppLauncher>.AssertOnDemandConfigurationModuleCLoading();
        }
        #endregion

        #region Private Methods
        private static string GetDesktopApplicationProcess()
        {
            return ConfigHandler.GetValue("ConfigurationModuleWpfAppProcessName");
        }

        private static string GetDesktopApplication()
        {
            return ConfigHandler.GetValue("ConfigurationModuleWpfAppLocation");
        }
        #endregion
    }
}
