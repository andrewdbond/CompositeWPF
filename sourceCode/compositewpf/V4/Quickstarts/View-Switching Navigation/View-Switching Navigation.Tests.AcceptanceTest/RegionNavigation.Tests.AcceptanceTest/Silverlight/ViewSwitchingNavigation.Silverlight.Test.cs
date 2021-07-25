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
using AcceptanceTestLibrary.Common;
using AcceptanceTestLibrary.Common.Silverlight;
using System.Reflection;
using ViewSwitchingNavigation.Tests.AcceptanceTest.TestEntities.Page;
using System.Threading;
using AcceptanceTestLibrary.TestEntityBase;
using AcceptanceTestLibrary.ApplicationHelper;
using System.IO;
using System.Text.RegularExpressions;
using ViewSwitchingNavigation.Tests.AcceptanceTest.TestEntities.Assertion;

namespace ViewSwitchingNavigation.Tests.AcceptanceTest
{
    #if DEBUG

    [DeploymentItem(@".\RegionNavigation.Tests.AcceptanceTest\bin\Debug")]
    [DeploymentItem(@"..\Silverlight\RegionNavigation\RegionNavigation\Bin\Debug", "SL")]
    #else
    [DeploymentItem(@".\RegionNavigation.Tests.AcceptanceTest\bin\Release")]
    [DeploymentItem(@"..\Silverlight\RegionNavigation\RegionNavigation\Bin\Release", "SL")]
#endif
    [TestClass]
    public class RegionNavigation_Silverlight_Test : FixtureBase<SilverlightAppLauncher>
    {
        private const int BACKTRACKLENGTH = 5;

        #region Additional test attributes

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
           
            string pp = GetAbsolutePath(BACKTRACKLENGTH);

            //Parameter list as follows.
            //1. Port number of host application 2. Absolute path of the Silverlight Host.

            base.StartWebServer(GetPortNumber(GetSilverlightApplication()), GetAbsolutePath(BACKTRACKLENGTH) + GetSilverlightApplicationHostPath());
            ViewSwitchingNavigationPage<SilverlightAppLauncher>.Window = base.LaunchApplication(GetSilverlightApplication(), GetBrowserTitle())[0];
            Thread.Sleep(5000);
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            PageBase<SilverlightAppLauncher>.DisposeWindow();
            base.StopWebServer();
            SilverlightAppLauncher.UnloadBrowser(GetBrowserTitle());
           
        }
        #endregion

        [TestMethod]
        public void SilverlightRegionNavigationLaunch()
        {

          
            Assert.IsNotNull(ViewSwitchingNavigationPage<SilverlightAppLauncher>.Window, "Region Navigation application is not launched.");
        }

        [TestMethod]
        public void SilverlightRN_MailRegionLoad()
        {
            ViewSwitchingNavigationAssertion<SilverlightAppLauncher>.AssertRN_MailRegionOnLoad();
        }

        [TestMethod]
        public void SilverlightRN_SelectAMessage()
        {
            ViewSwitchingNavigationAssertion<SilverlightAppLauncher>.AssertRN_SelectAMessage();
        }

        [TestMethod]
        public void SilverlightRN_LoadCalendar()
        {
            ViewSwitchingNavigationAssertion<SilverlightAppLauncher>.AssertRN_LoadCalendar();
        }

        [TestMethod]
        public void SilverlightRN_LoadContactDetails()
        {
            ViewSwitchingNavigationAssertion<SilverlightAppLauncher>.AssertRN_LoadContactDetails();
        }

        [TestMethod]
        public void SilverlightRN_LoadContactAvatars()
        {
            ViewSwitchingNavigationAssertion<SilverlightAppLauncher>.AssertRN_LoadContactAvatars();
        }

        [TestMethod]
        public void SilverlightRN_SendEmailFromContactDetailsPage()
        {
            ViewSwitchingNavigationAssertion<SilverlightAppLauncher>.AssertRN_SendEmailFromContactDetailsPage();
        }

        [TestMethod]
        public void SilverlightRN_SendEmailFromContactsAvatarsPage()
        {
            ViewSwitchingNavigationAssertion<SilverlightAppLauncher>.AssertRN_SendEmailFromContactsAvatarPage();
        }
        [TestMethod]
        public void SilverlightRN_SendNewEmail()
        {
            ViewSwitchingNavigationAssertion<SilverlightAppLauncher>.AssertRN_SendNewEmail();
        }
        [TestMethod]
        public void SilverlightRN_ReplyAnEmail()
        {
            ViewSwitchingNavigationAssertion<SilverlightAppLauncher>.AssertRN_ReplyAnEmail();
        }
        
        public void SilverlightRN_ClickHyperlinkOnCalendar()
        {
            ViewSwitchingNavigationAssertion<SilverlightAppLauncher>.AssertRN_ClickHyperlinkOnCalendar();
        }
        [TestMethod]
        public void SilverlightRN_OpenEmail()
        {
            ViewSwitchingNavigationAssertion<SilverlightAppLauncher>.AssertRN_OpenEmail();
        }
        #region Helper Methods
        private static string GetSilverlightApplication()
        {
            return ConfigHandler.GetValue("SilverlightAppLocation");
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
            return new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("SilverlightAppTitle");
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

        private static string GetSilverlightApplicationHostPath()
        {
            return ConfigHandler.GetValue("SilverlightAppHostRelativeLocation");
        }
        #endregion
    }
}
