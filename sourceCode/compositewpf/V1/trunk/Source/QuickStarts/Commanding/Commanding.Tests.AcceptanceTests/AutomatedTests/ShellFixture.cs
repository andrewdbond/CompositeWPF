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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commanding.AcceptanceTests.TestInfrastructure;
using Commanding.AcceptanceTests.ApplicationObserver;
using Core.UIItems.ListBoxItems;
using Core.UIItems;
using Core.UIItems.Finders;

namespace Commanding.AcceptanceTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    [DeploymentItem(@".\Commanding\bin\Debug")]
    [DeploymentItem(@".\Commanding.Tests.AcceptanceTests\bin\Debug")]
    public class ShellFixture : FixtureBase
    {
        #region Additional test attributes
        
        // Use TestInitialize to run code before running each test 
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
        
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup() 
        {
            base.TestCleanup();
        }
        
        #endregion
        /// <summary>
        /// Launch the Commanding QS application and check for the display of required controls
        /// 
        /// Repro Steps:
        /// 1. Launch the Commanding QS application.
        /// 2. Check for the display items on the default page of the application
        /// 
        /// Expected Result:
        /// Required controls should be loaded on the shell window with the default data
        [TestMethod]
        public void ApplicationLaunch()
        {
            //check if the controls expected to be loaded on the Shell Window is loaded properly with the default data, as expected           
            const int initialOrdersCount = 3;

            Button saveAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("SaveAllToolBarButton"));
            Assert.IsNotNull(saveAllButton);

            Button saveButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("SaveButton"));
            Assert.IsNotNull(saveButton);
           
            Label orderNameLabel = (Label)Window.Get(SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("OrderNameLabel"))
                                                             .AndByText(TestDataInfrastructure.GetTestInputData("DefaultOrder"))
                                                             .AndControlType(typeof(Label)));
            Assert.IsNotNull(orderNameLabel);

            Label dateLabel = Window.Get<Label>(TestDataInfrastructure.GetControlId("DateLabel"));
            Assert.IsNotNull(dateLabel);

            Label quantityLabel = Window.Get<Label>(TestDataInfrastructure.GetControlId("QuantityLabel"));
            Assert.IsNotNull(quantityLabel);

            Label priceLabel = Window.Get<Label>(TestDataInfrastructure.GetControlId("PriceLabel"));
            Assert.IsNotNull(priceLabel);

            Label shippingLabel = Window.Get<Label>(TestDataInfrastructure.GetControlId("ShippingLabel"));
            Assert.IsNotNull(shippingLabel);

            Label totalLabel = Window.Get<Label>(TestDataInfrastructure.GetControlId("TotalLabel"));
            Assert.IsNotNull(totalLabel);

            TextBox deliveryDateTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("DeliveryDateTextBox"));       
            Assert.IsNotNull(deliveryDateTextBox);
            
            TextBox priceTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("PriceTextBox"));
            Assert.IsNotNull(priceTextBox);
            Assert.AreEqual(priceTextBox.Text, TestDataInfrastructure.GetTestInputData("DefaultData"));

            TextBox quantityTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("QuantityTextBox"));
            Assert.IsNotNull(quantityTextBox);
            Assert.AreEqual(quantityTextBox.Text, TestDataInfrastructure.GetTestInputData("DefaultData"));

            TextBox shippingTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("ShippingTextBox"));
            Assert.IsNotNull(shippingTextBox);
            Assert.AreEqual(shippingTextBox.Text, TestDataInfrastructure.GetTestInputData("DefaultData"));

            TextBox totalTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("TotalTextBox"));
            Assert.IsNotNull(totalTextBox);
            Assert.AreEqual(totalTextBox.Text, TestDataInfrastructure.GetTestInputData("DefaultData"));

            ListBox orderView = Window.Get<ListBox>(TestDataInfrastructure.GetControlId("OrderListView"));
            Assert.IsNotNull(orderView);
            Assert.AreEqual(orderView.Items.Count, initialOrdersCount);
        }
       
        
    }
}
