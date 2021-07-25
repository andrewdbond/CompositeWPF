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
using AcceptanceTestLibrary.Common.Desktop;
using System.Reflection;
using System.Threading;
using Commanding.Tests.AcceptanceTest.TestEntities.Page;
using Commanding.Tests.AcceptanceTest.TestEntities.Assertion;
using AcceptanceTestLibrary.ApplicationHelper;

namespace Commanding.Tests.AcceptanceTest.Desktop
{
    /// <summary>
    /// Summary description for CommandingDesktopFixture
    /// </summary>

#if DEBUG
    [DeploymentItem(@"..\Desktop\Commanding\bin\Debug", "WPF")]
    [DeploymentItem(@".\Commanding.Tests.AcceptanceTest\bin\Debug")]
#else
    [DeploymentItem(@"..\Desktop\Commanding\bin\Release", "WPF")]
    [DeploymentItem(@".\Commanding.Tests.AcceptanceTest.Desktop\bin\Release")]
#endif
    [TestClass]
    public class CommandingDesktopFixture : FixtureBase<WpfAppLauncher>
    {
        #region Additional test attributes
        [TestInitialize]
        public void TestInitialize()
        {
            string currentOutputPath = (new System.IO.DirectoryInfo(Assembly.GetExecutingAssembly().Location)).Parent.FullName;
            CommandingPage<WpfAppLauncher>.LaunchApplication(currentOutputPath + GetDesktopApplication(), GetDesktopApplicationTitle());
            Thread.Sleep(15000);
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            CommandingPage<WpfAppLauncher>.DisposeApplication();
        }
        #endregion

        [TestMethod]
        public void DesktopApplicationLoadTest()
        {
            //check if window handle object is not null
            Assert.IsNotNull(CommandingPage<WpfAppLauncher>.DesktopWindow, "Commanding QS is not launched.");
        }

        [TestMethod]
        public void DesktopControlsLoadTest()
        {
            CommandingAssertion<WpfAppLauncher>.AssertDesktopControlLoad();
        }

        [TestMethod]
        public void DesktopProcessOrderByClickingSave()
        {
            CommandingAssertion<WpfAppLauncher>.AssertProcessOrderByClickingSave();
        }
        [TestMethod]
        
        public void DesktopAttemptSaveAfterMakingAnOrderInvalid()
        {
            CommandingAssertion<WpfAppLauncher>.AssertAttemptSaveAfterMakingAnOrderInvalid();
        }
        [TestMethod]
        public void DesktopProcessMultipleOrdersByClickingToolBarSaveAll()
        {
            CommandingAssertion<WpfAppLauncher>.AssertProcessMultipleOrdersByClickingToolBarSaveAll();
        }

        [TestMethod]
        public void DesktopAttemptToolBarSaveAllForMultipleValidOrdersAndOneInvalidOrder()
        {
            CommandingAssertion<WpfAppLauncher>.AssertAttemptToolBarSaveAllForMultipleValidOrdersAndOneInvalidOrder();
        }

        [TestMethod]
        public void DesktopAttemptSaveAfterSaveAllOrders()
        {
            InvokeDesktopAttemptSaveAfterSaveAllOrders();

        }
        [TestMethod]
        
        public void DesktopAttemptSaveAfterChangingQuantityNull()
        {
            InvokeDesktopAttemptSaveAfterChangingQuantityNull();

        }
        [TestMethod]
       
        public void DesktopAttemptSaveAfterChangingPriceNull()
        {
            InvokeDesktopAttemptSaveAfterChangingPriceNull();

        }

        #region Private methods

        private static string GetDesktopApplication()
        {
            return ConfigHandler.GetValue("CommandingWpfAppLocation");
        }

        private static string GetDesktopApplicationTitle()
        {
            return new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("DesktopApplicationTitle");
        }
        private static void InvokeDesktopAttemptSaveAfterSaveAllOrders()
        {
            CommandingAssertion<WpfAppLauncher>.AssertProcessMultipleOrdersByClickingToolBarSaveAll();
            CommandingAssertion<WpfAppLauncher>.AssertDesktopSaveButton();
        }
        private static void InvokeDesktopAttemptSaveAfterChangingQuantityNull()
        {
            CommandingAssertion<WpfAppLauncher>.AssertDesktopAttemptSaveAfterChangingQuantityNull();
        }
        private static void InvokeDesktopAttemptSaveAfterChangingPriceNull()
        {
            CommandingAssertion<WpfAppLauncher>.AssertDesktopAttemptSaveAfterChangingPriceNull();
        }

        #endregion
    }
}
