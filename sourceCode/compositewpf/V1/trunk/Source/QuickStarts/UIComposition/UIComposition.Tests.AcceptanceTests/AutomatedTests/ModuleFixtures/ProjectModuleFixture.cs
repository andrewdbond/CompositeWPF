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

using Core;
using Core.UIItems;
using Core.UIItems.WindowItems;
using Core.UIItems.TabItems;
using Core.UIItems.Finders;
using Core.AutomationElementSearch;
using Core.UIItems.ListViewItems;
using System.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UIComposition.AcceptanceTests.ApplicationObserver;
using UIComposition.AcceptanceTests.Helpers;

using System.Threading;
using UIComposition.AcceptanceTests.TestInfrastructure;

namespace UIComposition.AcceptanceTests
{
    [TestClass]
    [DeploymentItem(@".\UIComposition\bin\Debug")]
    [DeploymentItem(@".\UIComposition.Tests.AcceptanceTests\bin\Debug")] 
    public class ProjectModuleFixture : FixtureBase
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

    }
}
