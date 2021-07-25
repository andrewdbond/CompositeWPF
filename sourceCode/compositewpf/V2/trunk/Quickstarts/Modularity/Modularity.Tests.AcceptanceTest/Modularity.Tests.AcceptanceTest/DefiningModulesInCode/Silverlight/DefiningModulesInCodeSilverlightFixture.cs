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

using System.Threading;
using System.IO;
using System.Diagnostics;

using System.Windows;
using System.Windows.Forms;

using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Text;
using System.Windows.Automation.Provider;

using AcceptanceTestLibrary.Common;
using Modularity.Tests.AcceptanceTest.TestEntities.Page;
using AcceptanceTestLibrary.ApplicationObserver;
using AcceptanceTestLibrary.Common.Silverlight;
using Modularity.Tests.AcceptanceTest.TestEntities.Assertion;
using Modularity.Tests.AcceptanceTest.TestEntities.Action;
using System.Configuration;
using AcceptanceTestLibrary.ApplicationHelper;
using System.Collections.Specialized;
using AcceptanceTestLibrary.TestEntityBase;
using System.Reflection;

namespace Modularity.Tests.AcceptanceTest.DefiningModulesInCode.Silverlight
{
    /// <summary>
    /// Summary description for SilverlightAcceptanceTest
    /// </summary>
#if DEBUG
    [DeploymentItem(@"..\DefiningModulesInCodeQuickstart\Silverlight\Shell\bin\Debug\", "Silverlight")]
    [DeploymentItem(@".\Modularity.Tests.AcceptanceTest\bin\Debug\")]
#else
    [DeploymentItem(@"..\DefiningModulesInCodeQuickstart\Silverlight\Shell\bin\Release\", "Silverlight")]
    [DeploymentItem(@".\Modularity.Tests.AcceptanceTest\bin\Release\")]
#endif
    [TestClass]
    public class DefiningModulesInCodeSilverlightFixture : FixtureBase<SilverlightAppLauncher>
    {
        private const int BACKTRACKLENGTH = 4;

        #region Additional test attributes

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            string currentOutputPath = (new System.IO.DirectoryInfo(Assembly.GetExecutingAssembly().Location)).Parent.FullName;
            Shell<SilverlightAppLauncher>.Window = 
                base.LaunchApplication(currentOutputPath + GetSilverlightApplication(), GetBrowserTitle())[0];
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            PageBase<SilverlightAppLauncher>.DisposeWindow();
            SilverlightAppLauncher.UnloadBrowser(GetBrowserTitle());
        }

        #endregion

        #region Test Methods
        [TestMethod]
        public void DefineModulesInCodeSilverlightApplicationLoadTest()
        {
            Assert.IsNotNull(Shell<SilverlightAppLauncher>.Window, "Defining modules in code is not launched.");
        }
          
        [TestMethod]
        public void DefineModulesInCodeSilverlightModuleLoadTest()
        {
            ShellAssertion<SilverlightAppLauncher>.AssertDefiningModulesInCode();
        }

        [TestMethod]
        public void DefineModulesInCodeSilverlightLoadModuleCOnDemand()
        {
            ShellAction<SilverlightAppLauncher>.LoadModuleCOnDemand();
            ShellAssertion<SilverlightAppLauncher>.AssertOnDemandDefineModulesInCodeCLoading();
        }
        #endregion

        #region private Methods

        private static string GetSilverlightApplication()
        {
            return ConfigHandler.GetValue("DefineModulesInCodeSilverlightAppLocation");
        }

        private static string GetSilverlightApplicationPath(int backTrackLength)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            if (!String.IsNullOrEmpty(currentDirectory) && Directory.Exists(currentDirectory))
            {
                for (int iIndex = 0; iIndex < backTrackLength; iIndex++)
                {
                    currentDirectory = Directory.GetParent(currentDirectory).ToString();
                }
            }
            return currentDirectory + GetSilverlightApplication();
        }

        private static string GetBrowserTitle()
        {
            return new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("SilverlightAppTitleDefineModulesInCode");
        }
        #endregion
    }
}
