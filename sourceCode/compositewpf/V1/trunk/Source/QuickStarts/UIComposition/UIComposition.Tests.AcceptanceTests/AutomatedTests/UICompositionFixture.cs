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
using UIComposition.AcceptanceTests.ApplicationObserver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UIComposition.AcceptanceTests.Helpers;
using Core.UIItems.Finders;
using Core.UIItems;
using UIComposition.AcceptanceTests.TestInfrastructure;

namespace UIComposition.AcceptanceTests.AutomatedTests
{
    [TestClass]
    [DeploymentItem(@".\UIComposition\bin\Debug")]
    [DeploymentItem(@".\UIComposition.Tests.AcceptanceTests\bin\Debug")] 
    public class UICompositionFixture: FixtureBase
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
        /// Test the launch of CompositionUI QS
        /// 
        /// Repro Steps:
        /// 1. Launch the QS application
        /// 2. Check the controls loaded in the primary window
        /// 
        /// Expected Result:
        /// Select Employee label is found.
        /// Employee List table is found with 2 rows and 2 columns. Send Mail, Call and Previous Projects button are dispalyed.
        /// </summary>
        [TestMethod]
        [WorkItem(16909)]
        public void ApplicationLaunch()
        {
            SearchCriteria searchCriteria = SearchCriteria.ByText(TestDataInfrastructure.GetTestInputData("SelectEmployeeLabelText")).AndControlType(typeof(Label));
            Label selectEmployeeLabel = Window.Get<Label>(searchCriteria);
            Assert.IsNotNull(selectEmployeeLabel);

            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("EmployeesList"));
            Assert.IsNotNull(list, TestDataInfrastructure.GetTestInputData("EmployeeListNotFound"));

            Assert.AreEqual(2, list.Rows.Count, TestDataInfrastructure.GetTestInputData("EmployeeListIncorrectRowCount"));
            Assert.AreEqual(2, list.Header.Columns.Count, TestDataInfrastructure.GetTestInputData("EmployeeListIncorrectColumnCount"));

            /* Buttons have been removed
            searchCriteria = SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("SendEmailButton")).AndControlType(typeof(Button));
            Button sendEmailButton = window.Get<Button>(searchCriteria);
            Assert.IsNotNull(sendEmailButton, TestDataInfrastructure.GetTestInputData("SendMailButtonNotFound"));

            searchCriteria = SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("CallButton")).AndControlType(typeof(Button));
            Button callButton = window.Get<Button>(searchCriteria);
            Assert.IsNotNull(callButton, TestDataInfrastructure.GetTestInputData("CallButtonNotFound"));

            searchCriteria = SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("PastProjectsButton")).AndControlType(typeof(Button));
            Button pastProjectsButton = window.Get<Button>(searchCriteria);
            Assert.IsNotNull(callButton, TestDataInfrastructure.GetTestInputData("PastProjectsButtonNotfound"));
            */ 
        }
    }
}
