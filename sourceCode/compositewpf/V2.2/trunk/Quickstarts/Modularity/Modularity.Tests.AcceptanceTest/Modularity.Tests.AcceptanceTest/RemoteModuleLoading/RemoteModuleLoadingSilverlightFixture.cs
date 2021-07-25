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
using System.Text.RegularExpressions;
using AcceptanceTestLibrary.TestEntityBase;

namespace Modularity.Tests.AcceptanceTest.RemoteModuleLoading.Silverlight
{
    /// <summary>
    /// Summary description for SilverlightAcceptanceTest
    /// </summary>
    
#if DEBUG
    [DeploymentItem(@"..\RemoteModularityQuickstart\RemoteModuleLoading.Silverlight\Bin\Debug\")]
    [DeploymentItem(@".\Modularity.Tests.AcceptanceTest\bin\Debug\")]
#else
    [DeploymentItem(@"..\RemoteModularityQuickstart\RemoteModuleLoading.Silverlight\Bin\Release\")]
    [DeploymentItem(@".\Modularity.Tests.AcceptanceTest\bin\Release\")]
#endif
    [TestClass]
    public class RemoteModuleLoadingSilverlightFixture : FixtureBase<SilverlightAppLauncher>
    {
        #region Additional test attributes

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            //Launch Cassini server
            const int BACKTRACKLENGTH = 5;

            string pp = GetAbsolutePath(BACKTRACKLENGTH);

            //Parameter list as follows.
            //1. Port number of host application 2. Absolute path of the Silverlight Host.

            base.StartWebServer(GetPortNumber(GetSilverlightApplication()), GetAbsolutePath(BACKTRACKLENGTH) + GetSilverlightApplicationHostPath());
            Shell<SilverlightAppLauncher>.Window = base.LaunchApplication(GetSilverlightApplication(), GetBrowserTitle())[0];
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            PageBase<SilverlightAppLauncher>.DisposeWindow();
            SilverlightAppLauncher.UnloadBrowser(GetBrowserTitle());

            base.StopWebServer();
        }

        #endregion

        #region Test Methods
        [TestMethod]
        public void RemoteModuleSilverlightApplicationLoadTest()
        {
            Assert.IsNotNull(Shell<SilverlightAppLauncher>.Window, "Remote Module Loading is not launched.");
        }
          
        [TestMethod]
        public void RemoteModuleSilverlightModuleLoadTest()
        {
            ShellAssertion<SilverlightAppLauncher>.AssertRemoteModuleLoading();
        }

        [TestMethod]
        public void RemoteModuleSilverlightLoadModuleXOnDemand()
        {
            ShellAction<SilverlightAppLauncher>.LoadModuleXOnDemand();
            ShellAssertion<SilverlightAppLauncher>.AssertOnDemandRemoteModuleXLoading();
        }
        #endregion

        #region Private Methods

        private static string GetSilverlightApplication()
        {
            return ConfigHandler.GetValue("RemoteModuleSilverlightAppLocation");
        }

        private static string GetSilverlightApplicationHostPath()
        {
            return ConfigHandler.GetValue("RemoteModuleSilverlightAppHostRelativeLocation");
        }

        private static string GetBrowserTitle()
        {
            return new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("SilverlightAppTitleRemoteModule");
        }

        private static string GetAbsolutePath(int backTrackLength)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            if (!String.IsNullOrEmpty(currentDirectory) && Directory.Exists(currentDirectory))
            {
                for (int iIndex = 0; iIndex < backTrackLength; iIndex++)
                {
                    currentDirectory = Directory.GetParent(currentDirectory).ToString();
                }
            }

            return currentDirectory;
        }

        /// <summary>
        /// Extract the Port number from a URL of the format http://ServerName:PortNumber/SitePath
        /// </summary>
        /// <param name="url">URL of the above format</param>
        /// <returns>port number sub-string</returns>
        private static string GetPortNumber(string url)
        {
            string urlPattern = @"^(?<proto>\w+)://[^/]+?(?<port>:\d+)?/";
            Regex urlExpression = new Regex(urlPattern, RegexOptions.Compiled);
            Match urlMatch = urlExpression.Match(url);
            return urlMatch.Groups["port"].Value;
        }
        #endregion
    }
}
