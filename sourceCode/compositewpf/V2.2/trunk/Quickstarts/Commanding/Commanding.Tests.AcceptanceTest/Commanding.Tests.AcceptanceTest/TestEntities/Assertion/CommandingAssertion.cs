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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcceptanceTestLibrary.Common;
using AcceptanceTestLibrary.UIAWrapper;
using AcceptanceTestLibrary.ApplicationHelper;

using System.Windows.Automation;
//using System.Windows.Automation;
using System.Windows.Automation.Text;
using System.Windows.Automation.Provider;
using Commanding.Tests.AcceptanceTest.TestEntities.Page;
using Core.UIItems.ListBoxItems;
using Core.UIItems;
using Core.UIItems.WindowItems;
using Core.UIItems.Finders;
using System.Globalization;
using System.Diagnostics;

namespace Commanding.Tests.AcceptanceTest.TestEntities.Assertion
{

    public static class CommandingAssertion<TApp>
        where TApp : AppLauncherBase, new()
    {

        #region Desktop
        public static void AssertDesktopControlLoad()
        {
            const int initialOrdersCount = 3;
            Assert.IsNotNull(CommandingPage<TApp>.SaveAllToolBarButton,"Save All Button Not loaded");
            Assert.IsNotNull(CommandingPage<TApp>.SaveButton, "Save Button Not loaded");
            Assert.IsNotNull(CommandingPage<TApp>.DateLabel, "Delivery Date Label Not loaded");
            Assert.IsNotNull(CommandingPage<TApp>.DeliveryDateTextBox, "Delivery Date TextBox Not loaded");
            Assert.IsNotNull(CommandingPage<TApp>.OrderNameLabel, "Order Name Label Not loaded");
            Assert.IsNotNull(CommandingPage<TApp>.PriceLabel, "Price Label Not loaded");
            Assert.IsNotNull(CommandingPage<TApp>.PriceTextBox, "Price Text Box Not loaded");
            Assert.IsNotNull(CommandingPage<TApp>.QuantityTextBox, "Quantity Text Box Not loaded");
            Assert.IsNotNull(CommandingPage<TApp>.QuantityLabel, "Quantity Label Not loaded");
            Assert.IsNotNull(CommandingPage<TApp>.ShippingTextBox, "Shipping Text Box Not loaded");
            Assert.IsNotNull(CommandingPage<TApp>.ShippingLabel, "Shipping Label Not loaded");
            Assert.IsNotNull(CommandingPage<TApp>.TotalTextBox, "Total Text Box Not loaded");
            Assert.IsNotNull(CommandingPage<TApp>.TotalLabel, "Total Label Not loaded");
            Assert.IsNotNull(CommandingPage<TApp>.OrderListView,"OrderListView is Null");
            Assert.AreEqual(CommandingPage<TApp>.OrderListView.Items.Count, initialOrdersCount,"Row Count of the List View Not Equal");
        }

        public static void AssertProcessOrderByClickingSave()
        {
            PopulateOrderDetailsWithData();
            ListBox orderView = CommandingPage<TApp>.OrderListView;
            Button saveButton = CommandingPage<TApp>.SaveButton;
            System.Threading.Thread.Sleep(2000);
            int orderCount = orderView.Items.Count;
            //check if the save button is enabled
            Assert.IsTrue(saveButton.Enabled, "Save Button Disabled for valid Order Details");
            //click on the save button
            saveButton.Click();
            Assert.AreEqual(orderView.Items.Count, orderCount - 1, "ProcessOrderByClickingSave Failed as the count of Orders is not decreased");
        }

        public static void AssertAttemptSaveAfterMakingAnOrderInvalid()
        {
            //Populate the order details fileds with valid data
            PopulateOrderDetailsWithData();

            //Get the hanlde of the save button
            Button saveButton = CommandingPage<TApp>.SaveButton;
            //check if the save button is enabled
            Assert.IsTrue(saveButton.Enabled,"Save Button is Disabled for Valid Order details");

            //Populate the order details fileds with invalid data
            PopulateOrderDetailsWithInvalidData();

            //check if the save button is disabled
            Assert.IsFalse(saveButton.Enabled, "Save Button is Enabled for InValid Order details");
        }

        public static void AssertProcessMultipleOrdersByClickingToolBarSaveAll()
        {
            int count = 0;
            //Get the hanlde of the Order list view
            ListBox orderView = CommandingPage<TApp>.OrderListView;
            int orderCount = orderView.Items.Count;

            //Select the orders in the list view one by one and POpulate valid order details for every order
            while (count < orderCount)
            {
                orderView.Items[count].Select();
                Label orderNameLabel = CommandingPage<TApp>.OrderNameLabel;
                Assert.IsNotNull(orderNameLabel);
                PopulateOrderDetailsWithData();
                count++;
            }

            System.Threading.Thread.Sleep(2000);

            //Get the hanlde of the toolbar Save all button
            Button saveAllButton = CommandingPage<TApp>.SaveAllToolBarButton;

            //check if the toolbar save all button is enabled
            Assert.IsTrue(saveAllButton.Enabled, "SaveAll Button is Disabled for Valid order details");
            //click on the tollbar save all button
            saveAllButton.Click();

            //check if all the orders are saved and removed from the listview
            Assert.AreEqual(orderView.Items.Count.ToString(CultureInfo.InvariantCulture), new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("DefaultData"),"Save All Button Functionality Dosent works for Valid Order details.");
        }

        public static void AssertAttemptToolBarSaveAllForMultipleValidOrdersAndOneInvalidOrder()
        {
            int count = 0;

            //Get the hanlde of the order list view
            ListBox orderView = CommandingPage<TApp>.OrderListView;
            int orderCount = orderView.Items.Count;

            //Select the orders in the list view one by one and POpulate valid order details for every order
            while (count < orderCount)
            {
                orderView.Items[count].Select();
                Label orderNameLabel = CommandingPage<TApp>.OrderNameLabel;
                Assert.IsNotNull(orderNameLabel,"Order Name is Null");
                PopulateOrderDetailsWithData();
                count++;
            }

            System.Threading.Thread.Sleep(2000);

            //Get the hanlde of te toolbar save all button
            Button saveAllButton = CommandingPage<TApp>.SaveAllToolBarButton;

            //check if the toolbar save all button is enabled
            Assert.IsTrue(saveAllButton.Enabled, "SaveAll Button is Disabled for Valid order details");

            //POpulate the selected ordere details with invalid data
            PopulateOrderDetailsWithInvalidData();

            //check if the toolbar save all button is disabled
            Assert.IsFalse(saveAllButton.Enabled,"SaveAll Button is Enabled for Invalid order details");
        }

        public static void AssertDesktopSaveButton()
        {
            Assert.IsNull(CommandingPage<TApp>.SaveButton, "Save Button Enabled Even after all orders were Saved");
        }

        public static void AssertDesktopAttemptSaveAfterChangingQuantityNull()
        {
            PopulateOrderDetailsWithData();
            CommandingPage<TApp>.QuantityTextBox.SetValue(string.Empty);
            CommandingPage<TApp>.PriceTextBox.Click();
            Assert.IsFalse(CommandingPage<TApp>.SaveButton.Enabled, "Save Button Enabled for Null Quantity");
        }
        public static void AssertDesktopAttemptSaveAfterChangingPriceNull()
        {
            PopulateOrderDetailsWithData();
            CommandingPage<TApp>.PriceTextBox.SetValue(string.Empty);
            CommandingPage<TApp>.QuantityTextBox.Click();
            Assert.IsFalse(CommandingPage<TApp>.SaveButton.Enabled, "Save Button Enabled for Null Price");
        }
        #endregion

        #region Desktop Private Methods
        private static void PopulateOrderDetailsWithData()
        {
            CommandingPage<TApp>.QuantityTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("DefaultQuantity"));
            CommandingPage<TApp>.PriceTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("DefaultPrice"));
            CommandingPage<TApp>.ShippingTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("DefaultShipping"));
        }

        private static void PopulateOrderDetailsWithInvalidData()
        {
            CommandingPage<TApp>.QuantityTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("InvalidQuantity"));
            CommandingPage<TApp>.PriceTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("InvalidPrice"));
        }
        #endregion

        #region SilverLight


        public static void AssertSilverLightControlsLoadTest()
        {
            AutomationElement aeSaveAllToolBarButton = CommandingPage<TApp>.aeSaveAllToolBarButton;
            Assert.IsNotNull(aeSaveAllToolBarButton, "SaveAll Button Not Loaded");
            AutomationElement aeSaveButton = CommandingPage<TApp>.aeSaveButton;
            Assert.IsNotNull(aeSaveButton, "Save Button Not Loaded");
            AutomationElement aeOrderListView = CommandingPage<TApp>.aeOrderListView;
            Assert.IsNotNull(aeOrderListView, "Order List View  Not Loaded");
            AutomationElement aeDeliveryDateTextBox = CommandingPage<TApp>.aeDeliveryDateTextBox;
            Assert.IsNotNull(aeDeliveryDateTextBox, "Delivery Date TextBox Not Loaded");
            AutomationElement aePriceTextBox = CommandingPage<TApp>.aePriceTextBox;
            Assert.IsNotNull(aePriceTextBox, "Price TextBox Not Loaded");
            AutomationElement aeQuantityTextBox = CommandingPage<TApp>.aeQuantityTextBox;
            Assert.IsNotNull(aeQuantityTextBox, "Quantity TextBox Not Loaded");
            AutomationElement aeShippingTextBox = CommandingPage<TApp>.aeShippingTextBox;
            Assert.IsNotNull(aeShippingTextBox, "Shipping TextBox Not Loaded");
            AutomationElement aeTotalTextBox = CommandingPage<TApp>.aeTotalTextBox;
            Assert.IsNotNull(aeTotalTextBox, "Total TextBox Not Loaded");
        }

        public static void AssertSLProcessOrderByClickingSave()
        {
            SLPopulateOrderDetailsWithData();
            AutomationElement aeorderView = CommandingPage<TApp>.aeOrderListView;
            AutomationElement aesaveButton = CommandingPage<TApp>.aeSaveButton;            
            aesaveButton.Click();
            System.Threading.Thread.Sleep(2000);
            AutomationElement orderview = aeorderView.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, GetDataFromTestDataFile("Orderone")));
            Assert.IsNull(orderview, "Save Button Clicked and the orders not saved and the Order 1 exists");	
        }

        public static void AssertSLAttemptSaveAfterMakingAnOrderInvalid()
        {
            SLPopulateOrderDetailsWithInvalidData();
            AutomationElement aeorderView = CommandingPage<TApp>.aeOrderListView;
            AutomationElement aesaveButton = CommandingPage<TApp>.aeSaveButton;
            Assert.IsFalse(aesaveButton.Current.IsEnabled,"Save Button is Enabled for Invalid Order details");                        
        }

        public static void AssertSLProcessMultipleOrdersByClickingToolBarSaveAll()
        {            
            AutomationElement aeorderView = CommandingPage<TApp>.aeOrderListView;
            AutomationElement aeSaveAllButton = CommandingPage<TApp>.aeSaveAllToolBarButton;
            AutomationElement orderview = aeorderView.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, GetDataFromTestDataFile("Orderone")));
            AutomationElementCollection orderviews = aeorderView.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(orderviews.Count == 3, "Total order Items Count is Not equal to Three");
            //Populate Order 1 with Valid Data
            SLPopulateOrderDetailsWithData();
            Core.InputDevices.Mouse.Instance.Location = new System.Windows.Point((int)Math.Floor(orderviews[0].Current.BoundingRectangle.X), (int)Math.Floor(orderviews[0].Current.BoundingRectangle.Y));
            System.Threading.Thread.Sleep(2000);
            SelectionItemPattern pattern = orderviews[1].GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
            pattern.Select();
            //Populate Order 2 with Valid Data
            SLPopulateOrderDetailsWithData();
            Core.InputDevices.Mouse.Instance.Location = new System.Windows.Point((int)Math.Floor(orderviews[1].Current.BoundingRectangle.X), (int)Math.Floor(orderviews[1].Current.BoundingRectangle.Y));
            System.Threading.Thread.Sleep(2000);
            pattern = orderviews[2].GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
            pattern.Select();
            //Populate Order 3 with Valid Data
            SLPopulateOrderDetailsWithData();
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(aeSaveAllButton.Current.IsEnabled,"Save All Button Disabled for All Valid Order details");
            aeSaveAllButton.Click();
            System.Threading.Thread.Sleep(2000);
            AutomationElementCollection orderviewFinal = aeorderView.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
            Assert.IsTrue(orderviewFinal.Count == 0, "Order Items Still exists in View Even after Clicking SaveAll Button" );            
        }

        public static void AssertSLAttemptToolBarSaveAllForMultipleValidOrdersAndOneInvalidOrder()
        {
            AutomationElement aeorderView = CommandingPage<TApp>.aeOrderListView;
            AutomationElement aeSaveAllButton = CommandingPage<TApp>.aeSaveAllToolBarButton;
            AutomationElement orderview = aeorderView.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, GetDataFromTestDataFile("Orderone")));
            AutomationElementCollection orderviews = aeorderView.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(orderviews.Count == 3, "Total order Items Count is Not equal to Three");
            //Populate Order 1 with Valid Data
            SLPopulateOrderDetailsWithData();
            Core.InputDevices.Mouse.Instance.Location = new System.Windows.Point((int)Math.Floor(orderviews[0].Current.BoundingRectangle.X), (int)Math.Floor(orderviews[0].Current.BoundingRectangle.Y));
            System.Threading.Thread.Sleep(4000);
            SelectionItemPattern pattern = orderviews[1].GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
            pattern.Select();
            //Populate Order 2 with InValid Data
            SLPopulateOrderDetailsWithInvalidData();
            Core.InputDevices.Mouse.Instance.Location = new System.Windows.Point((int)Math.Floor(orderviews[1].Current.BoundingRectangle.X), (int)Math.Floor(orderviews[1].Current.BoundingRectangle.Y));
            System.Threading.Thread.Sleep(2000);
            pattern = orderviews[2].GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
            pattern.Select();
            //Populate Order 3 with Valid Data
            SLPopulateOrderDetailsWithData();
            Assert.IsFalse(aeSaveAllButton.Current.IsEnabled, "Save All Button Enabled for one of an Invalid Order details");            
        }

        public static void AssertSaveButton()
        {
            AutomationElement aeSaveButton = CommandingPage<TApp>.aeSaveButton;
            Assert.IsTrue(aeSaveButton.Current.IsOffscreen, "Save Button Enabled Even after all orders were Saved");
        }

        public static void AssertSLAttemptSaveAfterChangingQuantityNull()
        {
            SLPopulateOrderDetailsWithData();
            CommandingPage<TApp>.aeQuantityTextBox.SetValue(string.Empty);
            CommandingPage<TApp>.aePriceTextBox.SetFocus();
            AutomationElement aeSaveButton = CommandingPage<TApp>.aeSaveButton;
            Assert.IsFalse(aeSaveButton.Current.IsEnabled, "Save Button Enabled for Null Quantity ");

        }
        public static void AssertSLAttemptSaveAfterChangingPriceNull()
        {
            SLPopulateOrderDetailsWithData();
            CommandingPage<TApp>.aePriceTextBox.SetValue(string.Empty);
            CommandingPage<TApp>.aeQuantityTextBox.SetFocus();
            AutomationElement aeSaveButton = CommandingPage<TApp>.aeSaveButton;
            Assert.IsFalse(aeSaveButton.Current.IsEnabled, "Save Button Enabled for Null Price ");
        }

            
        #endregion 
 
        #region SilverLight Private Methods
        private static void SLPopulateOrderDetailsWithData()
        {
            CommandingPage<TApp>.aeQuantityTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("DefaultQuantity"));
            CommandingPage<TApp>.aePriceTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("DefaultPrice"));
            CommandingPage<TApp>.aeShippingTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("DefaultShipping"));
        }

        private static void SLPopulateOrderDetailsWithInvalidData()
        {
            CommandingPage<TApp>.aeQuantityTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("InvalidQuantity"));
            CommandingPage<TApp>.aePriceTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("InvalidPrice"));
        }
        
        private static string GetDataFromTestDataFile(string keyName)
        {
            return new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue(keyName);
        }
        #endregion

         
    }
}
