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
using System.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commanding.AcceptanceTests;
using Core.UIItems.ListBoxItems;
using Commanding.AcceptanceTests.TestInfrastructure;
using Core.UIItems;
using Core.UIItems.WindowItems;
using Core.UIItems.Finders;
using Commanding.Tests.AcceptanceTests.AutomatedTests;
using System.Globalization;

namespace Commanding.Tests.AcceptanceTests.AutomatedTests.ModuleFixtures
{     
    public partial class OrderModuleFixture 
    {
        /// <summary>
        /// Processing a valid Order by clicking on the save button
        /// 
        /// Repro Steps:
        /// 1. Launch the Commanding QS application.
        /// 2. Populate the order detail fields with valid data
        /// 3. Save button must be enabled.
        /// 4. On clicking Save, the selected order must be removed from the list.
        /// 
        /// Expected Result:
        /// User must be able to Save the order and 
        /// the saved order must be removed from the list after processing.
        [TestMethod]
        public void ProcessOrderByClickingSave()
        {
            //Populate the order details fileds with valid data
            PopulateOrderDetailsWithData();

            //Get the handle of the Orders List view
            ListBox orderView = (ListBox)Window.Get(SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("OrderListView"))
                                                             .AndControlType(typeof(ListBox)));
            int orderCount = orderView.Items.Count;
            
            //Get the handle of the save button
            Button saveButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("SaveButton"));
            //check if the save button is enabled
            Assert.IsTrue(saveButton.Enabled);
            //click on the save button
            saveButton.Click();

            //check if the saved order is removed from the listview
            Assert.AreEqual(orderView.Items.Count,orderCount-1);
        }

        /// <summary>
        /// Processing multiple orders by clicking on the toolbar save all button
        /// 
        /// Repro Steps:
        /// 1. Launch the Commanding QS application.
        /// 2. Populate the order detail fields with valid data for the selected order
        /// 3. select the orders one by one and for each order,populate the details with valid data
        /// 4. Click on the toolbar save all button.
        /// 
        /// Expected Result:
        /// User must be able to Save all the orders and 
        /// the order list must be made empty after processing.
        [TestMethod]
        public void ProcessMultipleOrdersByClickingToolBarSaveAll()
        {
            int count=0;
            //Get the hanlde of the Order list view
            ListBox orderView = Window.Get<ListBox>(TestDataInfrastructure.GetControlId("OrderListView"));            
            int orderCount = orderView.Items.Count;

            //Select the orders in the list view one by one and POpulate valid order details for every order
            while(count < orderCount)
            {
                orderView.Items[count].Select();
                Label orderNameLabel = (Label)Window.Get(SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("OrderNameLabel"))
                                                                .AndByText(TestDataInfrastructure.GetTestInputData("Order") + (count+1))
                                                                .AndControlType(typeof(Label)));
                Assert.IsNotNull(orderNameLabel);
                PopulateOrderDetailsWithData();
                count++;
            }

            //Get the hanlde of the toolbar Save all button
            Button saveAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("SaveAllToolBarButton"));
            //check if the toolbar save all button is enabled
            Assert.IsTrue(saveAllButton.Enabled);
            //click on the tollbar save all button
            saveAllButton.Click();

            //check if all the orders are saved and removed from the listview
            Assert.AreEqual(orderView.Items.Count.ToString(CultureInfo.InvariantCulture), TestDataInfrastructure.GetTestInputData("DefaultData"));
        }

        /// <summary>
        /// Attempt to save after making an order invalid
        /// 
        /// Repro Steps:
        /// 1. Launch the Commanding QS application.
        /// 2. Populate the order detail fields with valid data for the selected order
        /// 3. check if the save button is enabled
        /// 4. Populate the order detail fields with valid data for the selected order
        /// 5. check if the save button is disabled.
        /// 
        /// Expected Result:
        /// User must be able to Save the order when valid data is entered and 
        /// save button must be disabled whenever invalid details are entered.
        [TestMethod]
        public void AttemptSaveAfterMakingAnOrderInvalid()
        {
            //Populate the order details fileds with valid data
            PopulateOrderDetailsWithData();
           
            //Get the hanlde of the save button
            Button saveButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("SaveButton"));
            //check if the save button is enabled
            Assert.IsTrue(saveButton.Enabled);

            //Populate the order details fileds with invalid data
            PopulateOrderDetailsWithInvalidData();

            //check if the save button is disabled
            Assert.IsFalse(saveButton.Enabled);
        }

        /// <summary>
        /// Attempt toolbar save all for multiple valid orders and one invalid order
        /// 
        /// Repro Steps:
        /// 1. Launch the Commanding QS application.
        /// 2. select the orders one by one and for each order,populate the details with valid data
        /// 3. check if the save all button is enabled
        /// 4. Populate the order detail fields with invalid data for the selected order
        /// 5. check if the saveall toolbar button is disabled.
        /// 
        /// Expected Result:
        /// User must be able to Save all the orders when valid data is entered and 
        /// save all button must be disabled whenever invalid details are entered even for one order.
        [TestMethod]
        public void AttemptToolBarSaveAllForMultipleValidOrdersAndOneInvalidOrder()
        {
            int count = 0;

            //Get the hanlde of the order list view
            ListBox orderView = Window.Get<ListBox>(TestDataInfrastructure.GetControlId("OrderListView"));            
            int orderCount = orderView.Items.Count;

            //Select the orders in the list view one by one and POpulate valid order details for every order
            while (count < orderCount)
            {
                orderView.Items[count].Select();
                Label orderNameLabel = (Label)Window.Get(SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("OrderNameLabel"))
                                                                .AndByText(TestDataInfrastructure.GetTestInputData("Order") + (count + 1))
                                                                .AndControlType(typeof(Label)));
                Assert.IsNotNull(orderNameLabel);
                PopulateOrderDetailsWithData();
                count++;
            }

            //Get the hanlde of te toolbar save all button
            Button saveAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("SaveAllToolBarButton"));
            //check if the toolbar save all button is enabled
            Assert.IsTrue(saveAllButton.Enabled);

            //POpulate the selected ordere details with invalid data
            PopulateOrderDetailsWithInvalidData();

            //check if the toolbar save all button is disabled
            Assert.IsFalse(saveAllButton.Enabled);
        } 
    }
}
