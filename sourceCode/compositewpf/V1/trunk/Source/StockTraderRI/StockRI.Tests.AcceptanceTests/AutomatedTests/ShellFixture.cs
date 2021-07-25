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
using System.Text;

using Core;
using Core.UIItems;
using Core.UIItems.WindowItems;
using Core.UIItems.TabItems;
using Core.UIItems.Finders;
using Core.AutomationElementSearch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.UIItems.ListViewItems;
using System.Windows.Automation;
using StockTraderRI.AcceptanceTests.TestInfrastructure;
using StockTraderRI.AcceptanceTests.Helpers;
using StockTraderRI.AcceptanceTests.ApplicationObserver;

namespace StockTraderRI.AcceptanceTests.AutomatedTests
{
    [TestClass]
    [DeploymentItem(@".\StockTraderRI\bin\Debug")]
    [DeploymentItem(@".\StockRI.Tests.AcceptanceTests\bin\Debug")]
    public class ShellFixture : FixtureBase
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
        /// Launch the application - a current position view, News buttons, Pie Chart, Graph and no tab display.
        /// 
        /// Repro Steps:
        /// 1. Launch the Stock Trader application.
        /// 2. Check for the following display items on the default page of the application: current position view, 
        ///     News buttons, Pie Chart, Graph and tab display
        /// 
        /// Expected Result:
        /// Current position view, News buttons, Pie Chart, Graph should be displayed. Tab should not be displayed.
        /// </summary>
        [TestMethod]
        public void ApplicationLaunch()
        {
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            Assert.IsNotNull(list);

            //read number of account positions from the AccountPosition.xml data file
            int positionRowCount = TestDataInfrastructure.GetCount<AccountPositionDataProvider, AccountPosition>();

            Assert.AreEqual(positionRowCount, list.Rows.Count);

            // News button is no longer on grid
            //SearchCriteria searchCriteria = SearchCriteria.ByText(TestDataInfrastructure.GetControlId("NewsButtonName")).AndControlType(typeof(Button));
            //Button button = window.Get<Button>(searchCriteria);
            //Assert.IsNotNull(button);

            //pie chart and line chart could not be automated
        }
    }
}

