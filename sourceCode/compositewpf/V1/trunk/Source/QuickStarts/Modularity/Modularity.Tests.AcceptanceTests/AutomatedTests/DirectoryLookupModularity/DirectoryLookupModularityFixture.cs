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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modularity.AcceptanceTests.ApplicationObserver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modularity.AcceptanceTests.Helpers;
using Core.UIItems.Finders;
using Core.UIItems;
using Modularity.AcceptanceTests.TestInfrastructure;

namespace Modularity.AcceptanceTests.AutomatedTests
{
    [TestClass]
    [DeploymentItem(@".\DirectoryLookupModularity\DirectoryLookupModularity\bin\Debug")]
    [DeploymentItem(@".\Modularity.Tests.AcceptanceTests\bin\Debug")]
    public class DirectoryLookupModularityFixture : DirectoryLookupModularityFixtureBase
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
        /// Test the launch of Modularity QS
        /// 
        /// Repro Steps:
        /// 1. Launch the QS application
        /// 2. Check the controls loaded in the primary window
        /// 
        /// Expected Result:
        /// Check if the modules are loaded in the correct order
        /// Check if the button is displayed
        /// </summary>
        [TestMethod]
        public void ApplicationLaunch()
        {
            // perform Directory Sweep and get all the participating modules
            List<Module> lm = TestDataInfrastructure.GetData<DirectoryLookupModuleDataProvider, Module>();
            List<Module> sortedList = lm.SortModulesInLoadOrder();
            Label moduleContent;

            foreach (Module m in sortedList)
            {
                moduleContent = Window.Get<Label>(
                                   SearchCriteria
                                       .ByText(TestDataInfrastructure.GetTestInputData(m.ModuleName + TestDataInfrastructure.GetTestInputData("ContentString")))
                                       .AndControlType(typeof(Label)));

                if (m.AllowsDelayLoading)
                {
                    Assert.IsNull(moduleContent);
                }
                else
                {
                    Assert.IsNotNull(moduleContent);
                }
            }
        }
    }
}
