//===============================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modularity.AcceptanceTests.TestInfrastructure;
using Modularity.AcceptanceTests.ApplicationObserver;
using Core.UIItems;
using Core.UIItems.Finders;

namespace Modularity.AcceptanceTests.AutomatedTests
{
    /// <summary>
    /// Summary description for ModuleBFixture
    /// </summary>
    [TestClass]
    [DeploymentItem(@".\ConfigurationModularity\ConfigurationModularity\bin\Debug")]
    [DeploymentItem(@".\Modularity.Tests.AcceptanceTests\bin\Debug")]
    public class ConfigurationModularityModuleBFixture : ConfigurationModularityFixtureBase
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            // Check whether any exception occured during previous application launches. 
            // If so, fail the test case.
            if (StateDiagnosis.IsFailed)
            {
                Assert.Fail(TestDataInfrastructure.GetTestInputData("ApplicationLoadFailure"));
            }

            base.TestInitialize();
        }

        /// <summary>
        /// TestCleanup performs clean-up activities after each test method execution
        /// </summary>
        [TestCleanup()]
        public void MyTestCleanup()
        {
            base.TestCleanup();
        }

        /// <summary>
        /// Click "Load Module C" button.
        /// 
        /// Repro Steps:
        /// 1. Launch Modularity QS application
        /// 2. click on "Load Module C" button
        /// 
        /// Expected Result:
        /// On click of the button Module C content gets displayed
        /// </summary>
        [TestMethod]
        public void ClickLoadModuleCButton()
        {
            Button button = Window.Get<Button>(
                SearchCriteria
                    .ByText(TestDataInfrastructure.GetControlId("LoadModuleCButtonText"))
                    .AndControlType(typeof(Button)));
            
            Assert.IsNotNull(button);

            button.Click();

            Label moduleCContent = Window.Get<Label>(
                SearchCriteria
                    .ByText(TestDataInfrastructure.GetTestInputData("ModuleCContent"))
                    .AndControlType(typeof(Label)));

            Assert.IsNotNull(moduleCContent);
        }
    }
}
