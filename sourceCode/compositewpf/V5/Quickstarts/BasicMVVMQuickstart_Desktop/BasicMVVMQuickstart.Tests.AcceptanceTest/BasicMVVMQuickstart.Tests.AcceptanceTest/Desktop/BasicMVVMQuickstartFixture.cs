// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using AcceptanceTestLibrary.Common.Desktop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BasicMVVMQuickstart.Tests.AcceptanceTest.TestEntities.Page;
using AcceptanceTestLibrary.Common;
using System.Reflection;
using System.Threading;
using BasicMVVMQuickstart.Tests.AcceptanceTest.TestEntities.Page;
using BasicMVVMQuickstart.Tests.AcceptanceTest.TestEntities.Assertion;
using AcceptanceTestLibrary.ApplicationHelper;
namespace BasicMVVMQuickstart.Tests.AcceptanceTest
{
    
   #if DEBUG
    [DeploymentItem(@"..\..\..\..\BasicMVVMQuickstart_Desktop\bin\Debug", "WPF")]
    [DeploymentItem(@"TestData","TestData")]
#else
    [DeploymentItem(@"..\..\..\..\BasicMVVMQuickstart_Desktop\bin\Release", "WPF")]
    [DeploymentItem(@"TestData","TestData")]
#endif
    [TestClass]
    public class BasicMVVMQuickstartFixture : FixtureBase<WpfAppLauncher>
    {
        #region Additional test attributes
        [TestInitialize]
        public void TestInitialize()
        {
            string currentOutputPath = (new System.IO.DirectoryInfo(Assembly.GetExecutingAssembly().Location)).Parent.FullName;
            BasicMVVMQuickstartPage<WpfAppLauncher>.Window = base.LaunchApplication(currentOutputPath + GetDesktopApplication(), GetDesktopApplicationTitle())[0];
            BasicMVVMQuickstartPage<WpfAppLauncher>.Window.SetFocus();
            Thread.Sleep(1000);
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            UnloadApplication();           
        }
        #endregion

        [TestMethod]
        public void DesktopApplicationLoadTest()
        {
            //check if window handle object is not null
            Assert.IsNotNull(BasicMVVMQuickstartPage<WpfAppLauncher>.Window, "BasicMVVM QS is not launched.");
        }

        [TestMethod]
        public void DesktopControlsLoadTest()
        {
            BasicMVVMQuickstartAssertion<WpfAppLauncher>.AssertDesktopControlLoad();
        }

        [TestMethod]
        public void DesktopProcessQuestionaireByClickingSubmit()
        {
            BasicMVVMQuickstartAssertion<WpfAppLauncher>.AssertProcessQuestionaireByClickingSubmit();
        }

        [TestMethod]
        public void DesktopProcessQuestionaireClickingReset()
        {
            BasicMVVMQuickstartAssertion<WpfAppLauncher>.AssertProcessQuestionaireByClickingReset();
        }
      
        #region Private methods

        private static string GetDesktopApplication()
        {
            return ConfigHandler.GetValue("BasicMVVMWpfAppLocation");

        }


        private static string GetDesktopApplicationTitle()
        {
            return new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("DesktopApplicationTitle");
        }
        #endregion

    }
}
