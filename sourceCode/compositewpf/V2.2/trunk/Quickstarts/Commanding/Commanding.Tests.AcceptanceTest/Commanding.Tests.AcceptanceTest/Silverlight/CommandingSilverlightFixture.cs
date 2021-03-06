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
using System.Threading;
using System.IO;
using AcceptanceTestLibrary.TestEntityBase;
using Commanding.Tests.AcceptanceTest.TestEntities.Page;
using Commanding.Tests.AcceptanceTest.TestEntities.Assertion;
using AcceptanceTestLibrary.ApplicationHelper;

namespace Commanding.Tests.AcceptanceTest.Silverlight
{
    /// <summary>
    /// Summary description for CommandingSilverlightFixture
    /// </summary>
    /// 
#if DEBUG
    [DeploymentItem(@"..\SilverLight\Commanding\bin\Debug", "SL")]
    [DeploymentItem(@".\Commanding.Tests.AcceptanceTest\bin\Debug")]
#else
    [DeploymentItem(@"..\SilverLight\Commanding\bin\Release", "SL")]
    [DeploymentItem(@".\Commanding.Tests.AcceptanceTest.Desktop\bin\Release")]
#endif

    [TestClass]
    public class CommandingSilverlightFixture : FixtureBase<SilverlightAppLauncher>
    {
        private const int BACKTRACKLENGTH = 4;

        #region Additional test attributes
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            string currentOutputPath = (new System.IO.DirectoryInfo(Assembly.GetExecutingAssembly().Location)).Parent.FullName;
            CommandingPage<SilverlightAppLauncher>.Window = base.LaunchApplication(currentOutputPath + GetSilverlightApplication(), GetBrowserTitle())[0];
            Thread.Sleep(5000);
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            PageBase<SilverlightAppLauncher>.DisposeWindow();
            SilverlightAppLauncher.UnloadBrowser(GetBrowserTitle());
        }

        
        #endregion
        #region TestMethods
        
        [TestMethod]
        public void SilverlightApplicationLaunchTest()
        {
            Assert.IsNotNull(CommandingPage<SilverlightAppLauncher>.Window, "Commanding application is not launched.");
        }

        [TestMethod]
        public void SilverLightControlsLoadTest()
        {
            //InvokeSilverLightControls();
            CommandingAssertion<SilverlightAppLauncher>.AssertSilverLightControlsLoadTest();
        }

        [TestMethod]
        public void SilverLightProcessOrderByClickingSave()
        {
            CommandingAssertion<SilverlightAppLauncher>.AssertSLProcessOrderByClickingSave();
        }
       
        [TestMethod]
        public void SilverLightAttemptSaveAfterMakingAnOrderInvalid()
        {
            CommandingAssertion<SilverlightAppLauncher>.AssertSLAttemptSaveAfterMakingAnOrderInvalid();
        }

        [TestMethod]
        public void SilverLightProcessMultipleOrdersByClickingToolBarSaveAll()
        {
            CommandingAssertion<SilverlightAppLauncher>.AssertSLProcessMultipleOrdersByClickingToolBarSaveAll();
        }
        [TestMethod]
        public void SilverLightAttemptToolBarSaveAllForMultipleValidOrdersAndOneInvalidOrder()
        {
            CommandingAssertion<SilverlightAppLauncher>.AssertSLAttemptToolBarSaveAllForMultipleValidOrdersAndOneInvalidOrder();
        }
        [TestMethod]
        
        public void SilverLightAttemptSaveAfterSaveAllOrders()
        {
            InvokeSilverLightAttemptSaveAfterSaveAllOrders();
            
        }
        [TestMethod]
        
        public void SilverLightAttemptSaveAfterChangingQuantityNull()
        {
            InvokeSilverLightAttemptSaveAfterChangingQuantityNull();

        }
        [TestMethod]
       
        public void SilverLightAttemptSaveAfterChangingPriceNull()
        {
            InvokeSilverLightAttemptSaveAfterChangingPriceNull();

        }
        #endregion 
        #region private Methods
        private static void InvokeSilverLightControls()
        {
            CommandingAssertion<SilverlightAppLauncher>.AssertSilverLightControlsLoadTest();
        }

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

        private static void InvokeSilverLightAttemptSaveAfterSaveAllOrders()
        {
            CommandingAssertion<SilverlightAppLauncher>.AssertSLProcessMultipleOrdersByClickingToolBarSaveAll();
            CommandingAssertion<SilverlightAppLauncher>.AssertSaveButton();
        }
        private static void InvokeSilverLightAttemptSaveAfterChangingQuantityNull()
        {
            CommandingAssertion<SilverlightAppLauncher>.AssertSLAttemptSaveAfterChangingQuantityNull();
        }
        private static void InvokeSilverLightAttemptSaveAfterChangingPriceNull()
        {
            CommandingAssertion<SilverlightAppLauncher>.AssertSLAttemptSaveAfterChangingPriceNull();
        }
        #endregion
    }
}
