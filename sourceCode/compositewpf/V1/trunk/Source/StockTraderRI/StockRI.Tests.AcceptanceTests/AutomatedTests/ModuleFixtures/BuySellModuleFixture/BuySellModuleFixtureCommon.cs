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
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Automation;
using Core;
using Core.UIItems;
using Core.UIItems.Finders;
using Core.UIItems.ListBoxItems;
using Core.UIItems.MenuItems;
using Core.UIItems.TabItems;
using StockTraderRI.AcceptanceTests.TestInfrastructure;
using StockTraderRI.AcceptanceTests.AutomatedTests;
using StockTraderRI.AcceptanceTests.Helpers;
using StockTraderRI.AcceptanceTests.TestInfrastructure.MockModels;
using StockTraderRI.AcceptanceTests.ApplicationObserver;
using System.Globalization;


namespace StockTraderRI.AcceptanceTests.AutomatedTests.ModuleFixtures
{
    [TestClass]
    [DeploymentItem(@".\StockTraderRI\bin\Debug")]
    [DeploymentItem(@".\StockRI.Tests.AcceptanceTests\bin\Debug")]   
    public partial class BuySellModuleFixture : FixtureBase
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

            // Delete the Submitted orders
            string filePath = ConfigHandler.GetValue("OrderProcessingFile");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// TestCleanup performs clean-up activities after each test method execution
        /// </summary>
        [TestCleanup()]
        public void MyTestCleanup()
        {
            base.TestCleanup();
        }

        #region Private Helper methods

        private void LaunchBuySellPanelFromPositionTable(BuySellEnum buySell, string symbol)
        {
            //first select the Position Tab
            Tab positionBuySellTab = Window.Get<Tab>("PositionBuySellTab");
            positionBuySellTab.Pages.Find(p => p.NameMatches("POSITION")).Select();

            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));

            switch (buySell)
            {
                case BuySellEnum.Buy:
                    //right click on the added symbol in the Position Table and click Buy
                    list.Rows.Find(r => r.Cells[0].Text.Equals(symbol)).RightClick();
                    System.Threading.Thread.Sleep(1000);
                    Window.PopupMenu("Buy").Click();
                    break;
                case BuySellEnum.Sell:
                    //TODO: validate if Symbol can be sold

                    //right click on the added symbol in the Position Table and click sell
                    list.Rows.Find(r => r.Cells[0].Text.Equals(symbol)).RightClick();
                    System.Threading.Thread.Sleep(1000);
                    Window.PopupMenu("Sell").Click();                    
                    break;
            }
        }

        //validate if the screen has all the required controls and default values
        private void ValidateBuySellPanelControls(BuySellEnum buySell)
        {
            SelectBuySellTabPage();

            TextBox symbolTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellSymbolTextBox"));
            Assert.IsNotNull(symbolTextBox);

            RadioButton buyRadButton = Window.Get<RadioButton>(TestDataInfrastructure.GetControlId("BuyRadio"));
            Assert.IsNotNull(buyRadButton);
            RadioButton sellRadButton = Window.Get<RadioButton>(TestDataInfrastructure.GetControlId("SellRadio"));
            Assert.IsNotNull(sellRadButton);

            //check if Order type combobox is present
            WPFComboBox orderTypeComboBox = Window.Get<WPFComboBox>(TestDataInfrastructure.GetControlId("BuySellOrderTypeCombo"));
            Assert.IsNotNull(orderTypeComboBox);

            //check if shares textbox is present
            TextBox shareTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellSharesTextBox"));
            Assert.IsNotNull(shareTextBox);

            //check if limit / stop price  textbox is present            
            TextBox limitStopPriceTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellStopLimitPriceTextBox"));
            Assert.IsNotNull(limitStopPriceTextBox);

            WPFComboBox timeInForceComboBox = Window.Get<WPFComboBox>(TestDataInfrastructure.GetControlId("BuySellTimeInForceCombo"));
            Assert.IsNotNull(timeInForceComboBox);

            switch (buySell)
            {
                case BuySellEnum.Buy:
                    Assert.IsTrue(buyRadButton.IsSelected);
                    break;
                case BuySellEnum.Sell:
                    Assert.IsTrue(sellRadButton.IsSelected);
                    break;
            }

            //Button buyLastButton = window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellBuyLastButton"));
            //Assert.IsNotNull(buyLastButton);

            //Button sellLastButton = window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSellLastButton"));
            //Assert.IsNotNull(sellLastButton);

            Button submitButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitButton"));
            Assert.IsNotNull(submitButton);

            Button cancelButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellCancelButton"));
            Assert.IsNotNull(cancelButton);

            //check if Submit All and Cancel All buttons are present
            Button submitAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitAllButton"));
            Assert.IsNotNull(submitAllButton);

            Button cancelAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellCancelAllButton"));
            Assert.IsNotNull(cancelAllButton);
        }

        private void ValidateBuySellPanelData(Order model)
        {
            SelectBuySellTabPage();

            TextBox symbolTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellSymbolTextBox"));
            Assert.AreEqual(model.Symbol, symbolTextBox.Text);

            if (!String.IsNullOrEmpty(model.OrderType))
            {
                WPFComboBox orderTypeComboBox = Window.Get<WPFComboBox>(TestDataInfrastructure.GetControlId("BuySellOrderTypeCombo"));
                Assert.AreEqual(model.OrderType, orderTypeComboBox.SelectedItem.Text);
            }

            TextBox shareTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellSharesTextBox"));
            Assert.AreEqual(model.NumberOfShares.ToString(CultureInfo.CurrentCulture), shareTextBox.Text);

            TextBox limitStopPriceTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellStopLimitPriceTextBox"));
            Assert.AreEqual(model.LimitStopPrice.ToString(CultureInfo.CurrentCulture), limitStopPriceTextBox.Text);

            if (!String.IsNullOrEmpty(model.TimeInForce))
            {
                WPFComboBox timeInForceComboBox = Window.Get<WPFComboBox>(TestDataInfrastructure.GetControlId("BuySellTimeInForceCombo"));
                Assert.AreEqual(timeInForceComboBox.SelectedItem.Text, model.FormattedTimeInForce);
            }

            switch (model.TransactionType)
            {
                case "Buy":
                    RadioButton buyRadButton = Window.Get<RadioButton>(TestDataInfrastructure.GetControlId("BuyRadio"));
                    Assert.IsTrue(buyRadButton.IsSelected);
                    break;
                case "Sell":
                    RadioButton sellRadButton = Window.Get<RadioButton>(TestDataInfrastructure.GetControlId("SellRadio"));
                    Assert.IsTrue(sellRadButton.IsSelected);
                    break;
            }
        }

        private void PopulateBuySellPanelWithData(Order model)
        {
            SelectBuySellTabPage();

            ListBox listBox = Window.Get<ListBox>(TestDataInfrastructure.GetControlId("OrdersListBox"));
            int listItemsCount = listBox.Items.Count;
            GroupBox buySellSymbolGroup = null;

            //collapse all the CompositeExpander except the last one which is supposedly the latest
            if (listItemsCount > 1)
            {
                int groupboxTracker = 0;               

                for (int i = 0; i < listItemsCount; i++)
                {
                    if (null != listBox.Items[i])
                    {
                        buySellSymbolGroup = Window.TryGet<GroupBox>(SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("CompositeExpander"))
                                                                                    .AndControlType(typeof(GroupBox))
                                                                                    .AndIndex(i));

                        if (null != buySellSymbolGroup)
                        {
                            groupboxTracker = i;
                            buySellSymbolGroup.DoubleClick();
                        }
                    }
                }

                buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("CompositeExpander"))
                                                                                .AndControlType(typeof(GroupBox))
                                                                                .AndIndex(groupboxTracker));
                buySellSymbolGroup.DoubleClick();
            }

            if (buySellSymbolGroup != null)
            {
                AutomationElement symbolTextBox = buySellSymbolGroup.AutomationElement.SearchInRawTreeByName(TestDataInfrastructure.GetControlId("BuySellSymbolTextBox"));  
                SetTextBoxValue(symbolTextBox, model.Symbol);

                AutomationElement orderTypeComboBox = buySellSymbolGroup.AutomationElement.SearchInRawTreeByName(TestDataInfrastructure.GetControlId("BuySellOrderTypeCombo"));
                SelectItemInComboBox(orderTypeComboBox, model.OrderType);

                AutomationElement shareTextBox = buySellSymbolGroup.AutomationElement.SearchInRawTreeByName(TestDataInfrastructure.GetControlId("BuySellSharesTextBox"));
                SetTextBoxValue(shareTextBox, model.NumberOfShares.ToString(CultureInfo.InvariantCulture));

                AutomationElement limitStopPriceTextBox = buySellSymbolGroup.AutomationElement.SearchInRawTreeByName(TestDataInfrastructure.GetControlId("BuySellStopLimitPriceTextBox"));
                SetTextBoxValue(limitStopPriceTextBox, model.LimitStopPrice.ToString(CultureInfo.InvariantCulture));

                AutomationElement timeInForceComboBox = buySellSymbolGroup.AutomationElement.SearchInRawTreeByName(TestDataInfrastructure.GetControlId("BuySellTimeInForceCombo"));
                SelectItemInComboBox(timeInForceComboBox, model.FormattedTimeInForce);

                switch (model.TransactionType)
                {
                    case "Buy":
                        string buyRadioControlName = TestDataInfrastructure.GetControlId("BuyRadio");
                        AutomationElement buyRadioButtonElement = buySellSymbolGroup.AutomationElement.SearchInRawTreeByName(buyRadioControlName);
                        SelectRadioButton(buyRadioButtonElement);
                        break;
                    case "Sell":
                        string sellRadioControlName = TestDataInfrastructure.GetControlId("SellRadio");
                        AutomationElement sellRadioButtonElement = buySellSymbolGroup.AutomationElement.SearchInRawTreeByName(sellRadioControlName);
                        SelectRadioButton(sellRadioButtonElement);
                        break;
                }
            }
        }

        private static void SelectItemInComboBox(AutomationElement element, String selectedItemName)
        {            
            // Get the mapped item name.
            String itemName = TestDataInfrastructure.GetControlId(selectedItemName);

            // Set focus to ensure that the object is selected.
            element.SetFocus();

            // Expand the ComboBox using ExpandCollapsePattern.
            ExpandCollapsePattern expandPattern = element.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            expandPattern.Expand();

            // get the desired automation element
            PropertyCondition itemNameCondition = new PropertyCondition(AutomationElement.NameProperty, itemName);
            PropertyCondition itemTypeCondition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem);
            AndCondition itemCondition = new AndCondition(itemNameCondition, itemTypeCondition);

            AutomationElement item = element.FindFirst(TreeScope.Descendants, itemCondition);
            if (item != null)
            {
                // select combo box item
                ((SelectionItemPattern)item.GetCurrentPattern(SelectionItemPattern.Pattern)).Select();
            }
        }

        private static void SetTextBoxValue(AutomationElement element, string value)
        {
            ValuePattern valuePattern = element.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
            valuePattern.SetValue(value);
        }

        private static void SelectRadioButton(AutomationElement element)
        {
            SelectionItemPattern itemPattern = element.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
            itemPattern.Select();
        }

        private void SelectBuySellTabPage()
        {
            System.Threading.Thread.Sleep(2000); 
            Tab positionBuySellTab = Window.Get<Tab>(SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("PositionBuySellTab"))
                                                                   .AndControlType(typeof(Tab)));            
            positionBuySellTab.Pages.Find(p => p.NameMatches(TestDataInfrastructure.GetTestInputData("BuySellTab"))).Select();
        }

        // TODO: Required a single method to toggle between tabs.
        private void SelectPositionTabPage()
        {
            Tab positionBuySellTab = Window.Get<Tab>(SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("PositionBuySellTab"))
                                                                   .AndControlType(typeof(Tab)));
            positionBuySellTab.Pages.Find(p => p.NameMatches(TestDataInfrastructure.GetTestInputData("PositionTab"))).Select();
        }

        #endregion
    }
}
