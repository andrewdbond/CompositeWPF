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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Automation;
using Core;
using Core.UIItems;
using Core.UIItems.Finders;
using Core.UIItems.WindowItems;
using Commanding.AcceptanceTests.TestInfrastructure;
using Commanding.AcceptanceTests;
using Commanding.AcceptanceTests.Helpers;
using Commanding.AcceptanceTests.ApplicationObserver;

namespace Commanding.Tests.AcceptanceTests.AutomatedTests.ModuleFixtures
{
    [TestClass]
    [DeploymentItem(@".\Commanding\bin\Debug")]
    [DeploymentItem(@".\Commanding.Tests.AcceptanceTests\bin\Debug")]
    public partial class OrderModuleFixture:FixtureBase
    {
        #region Additional test attributes
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
        #endregion

        private void PopulateOrderDetailsWithData()
        {
            AutomationElement quantityTextBox = Window.AutomationElement.SearchInRawTreeByName(TestDataInfrastructure.GetControlId("QuantityTextBox"));
            SetTextBoxValue(quantityTextBox, TestDataInfrastructure.GetTestInputData("DefaultQuantity"));

            AutomationElement priceTextBox = Window.AutomationElement.SearchInRawTreeByName(TestDataInfrastructure.GetControlId("PriceTextBox"));
            SetTextBoxValue(priceTextBox, TestDataInfrastructure.GetTestInputData("DefaultPrice"));                     

            AutomationElement shippingTextBox = Window.AutomationElement.SearchInRawTreeByName(TestDataInfrastructure.GetControlId("ShippingTextBox"));
            SetTextBoxValue(shippingTextBox, TestDataInfrastructure.GetTestInputData("DefaultShipping"));
          
        }
        private static void SetTextBoxValue(AutomationElement element, string value)
        {
            ValuePattern valuePattern = element.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
            valuePattern.SetValue(value);
        }
        private void PopulateOrderDetailsWithInvalidData()
        {
            AutomationElement quantityTextBox = Window.AutomationElement.SearchInRawTreeByName(TestDataInfrastructure.GetControlId("QuantityTextBox"));
            SetTextBoxValue(quantityTextBox, TestDataInfrastructure.GetTestInputData("InvalidQuantity"));

            AutomationElement priceTextBox = Window.AutomationElement.SearchInRawTreeByName(TestDataInfrastructure.GetControlId("PriceTextBox"));
            SetTextBoxValue(priceTextBox, TestDataInfrastructure.GetTestInputData("InvalidPrice"));
        }
    }
}
