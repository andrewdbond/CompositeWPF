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
using Core.UIItems.Finders;
using Core.UIItems;
using Core.UIItems.MenuItems;
using Core.UIItems.TabItems;
using Core.UIItems.WindowItems;
using StockTraderRI.AcceptanceTests.TestInfrastructure;
using StockTraderRI.AcceptanceTests.AutomatedTests;
using StockTraderRI.AcceptanceTests.Helpers;
using StockTraderRI.AcceptanceTests.TestInfrastructure.MockModels;
using System.IO;
using System.Globalization;

namespace StockTraderRI.AcceptanceTests.AutomatedTests.ModuleFixtures
{
    public partial class BuySellModuleFixture
    {
        /// <summary>
        /// Check if user can sell more than the number of shares he/she holds.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and select Sell Option
        /// 3. Get number of shares for selected symbol and enter {no. of shares + 1} in the shares field
        /// 5. Click on "Submit" button
        /// 
        /// Expected Result:
        /// User should not sell the share more than the number of shares he holds.
        /// </summary>
        [TestMethod]
        public void AttemptSellMoreThanHeldNumberOfShares()
        {
            string symbol;
            string positionTableSymbolHeader = TestDataInfrastructure.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = TestDataInfrastructure.GetTestInputData("PositionTableShares");
            int symbolShare = 0;

            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            
            symbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(symbol)).Cells[positionTableSharesHeader].Text, CultureInfo.CurrentCulture);
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order symbolOrder = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        symbolShare + 1,
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(symbolOrder);

            Button submitButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitButton"));
            Assert.IsFalse(submitButton.Enabled);
        }

        /// <summary>
        /// Check if user can sell multiple shares more than number of shares he holds.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields and enter {number of shares + 1} in shares field
        /// 4. Repeat steps 2 & 3 for different stocks
        /// 5. Click on "Submit All" button
        /// 
        /// Expected Result:
        /// User should not sell the multiple shares more than the number of shares he holds.
        /// </summary>
        [TestMethod]
        public void AttemptSellMoreThanHeldNumberOfSharesForMultipleShares()
        {
            string symbol;
            string anotherSymbol;
            int symbolShare = 0;
            int anotherSymbolShare = 0;
            string positionTableSymbolHeader = TestDataInfrastructure.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = TestDataInfrastructure.GetTestInputData("PositionTableShares");
            
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            anotherSymbol = TestDataInfrastructure.GetTestInputData("PositionSymbol");
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));

            symbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(symbol)).Cells[positionTableSharesHeader].Text, CultureInfo.CurrentCulture);
            anotherSymbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(anotherSymbol)).Cells[positionTableSharesHeader].Text, CultureInfo.CurrentCulture);

            //Enter details for first symbol
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order symbolOrder = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        symbolShare + 1,
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(symbolOrder);
            ////////////////////////

            //Enter details for second symbol
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, anotherSymbol);
            Order anotherSymbolOrder = new Order(
                                        anotherSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        anotherSymbolShare + 1,
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherSymbolOrder);
            ////////////////////////

            Button submitAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitAllButton"));
            Assert.IsFalse(submitAllButton.Enabled);
        }

        /// <summary>
        /// Check if user can sell multiple shares more than number of shares he holds.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields and enter {number of shares + 1} in shares field
        /// 4. Repeat steps 2 & 3 for different stocks and enter only held number of shares.
        /// 5. Click on "Submit All" button
        /// 
        /// Expected Result:
        /// User should not sell the multiple shares more than the number of shares he holds.
        /// </summary>
        [TestMethod]
        public void AttemptSellMoreThanHeldNumberOfSharesForOneShareAndHeldNumberOfSharesForAnotherUsingSubmitAll()
        {
            string symbol;
            string anotherSymbol;
            int symbolShare = 0;
            int anotherSymbolShare = 0;
            string positionTableSymbolHeader = TestDataInfrastructure.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = TestDataInfrastructure.GetTestInputData("PositionTableShares");

            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            anotherSymbol = TestDataInfrastructure.GetTestInputData("PositionSymbol");
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));

            symbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(symbol)).Cells[positionTableSharesHeader].Text, CultureInfo.CurrentCulture);
            anotherSymbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(anotherSymbol)).Cells[positionTableSharesHeader].Text, CultureInfo.CurrentCulture);

            //Enter details for first symbol
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order symbolOrder = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        symbolShare + 1,
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(symbolOrder);
            ////////////////////////
    
            //Enter details for second symbol
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, anotherSymbol);
            Order anotherSymbolOrder = new Order(
                                        anotherSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        anotherSymbolShare,
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherSymbolOrder);
            ////////////////////////

            Button submitAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitAllButton"));
            Assert.IsFalse(submitAllButton.Enabled);
        }

        /// <summary>
        /// Check if user can sell multiple shares more than number of shares he holds.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields and enter {number of shares + 1} in shares field
        /// 4. Repeat steps 2 & 3 for different stocks and enter only held number of shares.
        /// 5. Click on "Submit All" button
        /// 
        /// Expected Result:
        /// User should not sell the multiple shares more than the number of shares he holds.
        /// </summary>
        [TestMethod]
        public void AttemptSellMoreThanHeldNumberOfSharesForOneShareAndHeldNumberOfSharesForAnotherUsingSubmit()
        {
            string symbol;
            string anotherSymbol;
            int symbolShare = 0;
            int anotherSymbolShare = 0;
            string positionTableSymbolHeader = TestDataInfrastructure.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = TestDataInfrastructure.GetTestInputData("PositionTableShares");

            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            anotherSymbol = TestDataInfrastructure.GetTestInputData("PositionSymbol");
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));

            symbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(symbol)).Cells[positionTableSharesHeader].Text, CultureInfo.CurrentCulture);
            anotherSymbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(anotherSymbol)).Cells[positionTableSharesHeader].Text, CultureInfo.CurrentCulture);

            //Enter details for first symbol
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order symbolOrder = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        symbolShare + 1,
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(symbolOrder);
            ////////////////////////

            //Enter details for second symbol
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, anotherSymbol);
            Order anotherSymbolOrder = new Order(
                                        anotherSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        anotherSymbolShare,
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherSymbolOrder);
            ////////////////////////

            Button submitButton = Window.Get<Button>(
                SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("BuySellSubmitButton"))
                .AndIndex(0));
            Assert.IsFalse(submitButton.Enabled);

            submitButton = Window.Get<Button>(
                SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("BuySellSubmitButton"))
                .AndIndex(1));
            Assert.IsTrue(submitButton.Enabled);

            submitButton.Click();

            //validate if the buy transaction was successful
            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(anotherSymbol)).Equals(anotherSymbolOrder));
        }

        /// <summary>
        /// Check if user can sell share across multiple sell requests.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields with half the number of shares
        /// 4. Repeat steps 2 & 3
        /// 5. Click on "Submit All" button
        /// 
        /// Expected Result:
        /// User should sell share across multiple sell requests.
        /// </summary>
        [TestMethod]
        public void SellShareSpreadAcrossMultipleSellRequests()
        {
            string symbol;
            int symbolShare = 0;
            string positionTableSymbolHeader = TestDataInfrastructure.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = TestDataInfrastructure.GetTestInputData("PositionTableShares");

            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            symbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(symbol)).Cells[positionTableSharesHeader].Text, CultureInfo.CurrentCulture);

            //Enter symbol details with shares / 2 in first tab
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order sellSymbolOrder = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        Convert.ToInt32(symbolShare / 2),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(sellSymbolOrder);
            ///////////////////////////////

            //Enter symbol details with shares / 2 in second tab
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            PopulateBuySellPanelWithData(sellSymbolOrder);
            ///////////////////////////////

            Button submitAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitAllButton"));
            submitAllButton.Click();

            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox)));
            Assert.IsNull(buySellSymbolGroup);

            //validate if the sell transaction was successful
            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsTrue(orders.FindAll(o => o.Equals(sellSymbolOrder)).Count.Equals(2));
        }

        /// <summary>
        /// Check if user can sell share across multiple sell requests.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields with half the number of shares
        /// 4. Repeat steps 2 & 3 with step 3 changed to enter more than half number of shares
        /// 5. Check if the "Submit All" button can be clicked (enabled)
        /// 
        /// Expected Result:
        /// User should not be allowed to sell more than held number of shares across multiple sell requests.
        /// </summary>
        [TestMethod]
        public void AttemptSellShareSpreadAcrossMultipleSellRequestsWithAllNumberOfSharesAddingToMoreThanHeldNumberOfShares()
        {
            string symbol;
            int symbolShare = 0;

            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            symbolShare = (int)list.GetData(symbol, PositionTableColumnHeader.NumberOfShares);

            //Enter symbol details with shares / 2 in first tab
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order sellSymbolOrder = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        Convert.ToInt32(symbolShare / 2),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(sellSymbolOrder);
            ///////////////////////////////

            //Enter symbol details with shares / 2 in second tab
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order sellSymbolAnotherOrder = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        Convert.ToInt32(symbolShare / 2) + 1,
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(sellSymbolAnotherOrder);
            ///////////////////////////////

            Button submitAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitAllButton"));
            Assert.IsFalse(submitAllButton.Enabled);
            submitAllButton.Click();

            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander").AndControlType(typeof(GroupBox)));
            Assert.IsNotNull(buySellSymbolGroup);

            //validate if the sell transaction was successful
            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsNull(orders.Find(o => o.Equals(sellSymbolOrder)));
            Assert.IsNull(orders.Find(o => o.Equals(sellSymbolAnotherOrder)));
        }

        /// <summary>
        /// Check if user can buy share across multiple buy requests.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and Select buy Option
        /// 4. Enter the data for Required fields.
        /// 4. Repeat steps 2 & 3 for the same stock
        /// 5. Click on "Submit All" button
        /// 
        /// Expected Result:
        /// User should buy share across multiple buy requests.
        /// </summary>
        [TestMethod]
        public void BuyShareSpreadAcrossMultipleBuyRequests()
        {
            string symbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");

            //Enter details for buying share in one tab
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, symbol);
            Order buySymbolOrder = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(buySymbolOrder);
            ///////////////////////////////

            //Enter details for buying same share in second tab
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, symbol);
            PopulateBuySellPanelWithData(buySymbolOrder);
            ///////////////////////////////

            Button submitAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitAllButton"));
            submitAllButton.Click();

            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox)));
            Assert.IsNull(buySellSymbolGroup);

            //validate if the sell transaction was successful
            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsTrue(orders.FindAll(o => o.Equals(buySymbolOrder)).Count.Equals(2));
        }

        /// <summary>
        /// Check if user can buy the share by launching buy/sell panel with sell request.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position Table and Select sell Option
        /// 3. Enter the data for Required fields and select buy option
        /// 4. Click on "Submit" button
        /// 
        /// Expected Result:
        /// User should buy the share.
        /// </summary>
        [TestMethod]
        public void LaunchBuySellPanelForSellFromPositionTableAndBuyTheShare()
        {
            string symbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");

            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order model = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(model);

            Button submitButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitButton"));
            submitButton.Click();

            //give time for submit processing
            System.Threading.Thread.Sleep(2000);

            // Validate if extender Buy/Sell Panel disappears.
            // AND
            // Validate if the buy transactions were successful
            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox)));
            Assert.IsNull(buySellSymbolGroup);

            //validate if the buy transaction was successful
            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(symbol)).Equals(model));
        }

        /// <summary>
        /// Check if user can buy and sell the shares for invalid symbol
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position Table and Select buy Option
        /// 3. Enter the data for Required fields with invalid symbol
        /// 4  Right-Click on the selected stock in Position Table and Select sell Option
        /// 5. Enter the data for Required fields with invalid symbol
        /// 6. Check for "Submit All" button disable. 
        /// 
        ///
        /// Expected Result:
        /// User should not buy and sell the shares for invalid symbol.
        /// "Submit All" button should be disable.
        /// </summary>
        [TestMethod]
        public void AttemptInvalidBuyAndInvalidSellSimultaneouslyWithMultipleRequests()
        {
            string buySymbol;
            string sellSymbol;
            string invalidSymbol;

            buySymbol = TestDataInfrastructure.GetTestInputData("Symbol");
            sellSymbol = TestDataInfrastructure.GetTestInputData("PositionSymbol3");
            invalidSymbol = TestDataInfrastructure.GetTestInputData("InvalidSymbol");

            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, buySymbol);
            Order buyModel = new Order(
                                        invalidSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(buyModel);

            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, sellSymbol);
            Order sellModel = new Order(
                                        invalidSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(sellModel);

            Button submitAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitAllButton"));
            Assert.IsFalse(submitAllButton.Enabled);
        }

        /// <summary>
        /// NOT VALID IN THE CURRENT SCENARIO -  A BUG IS ALREADY LOGGED AGAINST THIS.
        /// Check if user can sell the stock when buy/sell panel contains invalid buy and valid sell data
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Get the number of shares for a symbol.
        /// 3. Right-Click on the selected stock in Position Table and Select buy Option
        /// 4  Enter the data for Required fields with invalid buy symbol
        /// 5. Right-Click on the selected stock in Position Table and Select sell Option
        /// 6. Enter the data for Required fields with number of shares for a symbol.
        /// 7. Click on "Submit All"
        ///
        /// Expected Result:
        /// User should sell the shares.
        /// User should not buy the shares for invalid symbol.
        /// </summary>
        [TestMethod]
        [Ignore]
        public void AttemptInvalidBuyAndValidSellSimultaneouslyWithMultipleRequests()
        {
            string buySymbol;
            string sellSymbol;
            string invalidBuySymbol;
            int sellSymbolShare = 0;
            string positionTableSymbolHeader = TestDataInfrastructure.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = TestDataInfrastructure.GetTestInputData("PositionTableShares");

            buySymbol = TestDataInfrastructure.GetTestInputData("Symbol");
            sellSymbol = TestDataInfrastructure.GetTestInputData("PositionSymbol1");
            invalidBuySymbol = TestDataInfrastructure.GetTestInputData("InvalidSymbol");

            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            sellSymbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(sellSymbol)).Cells[positionTableSharesHeader].Text, CultureInfo.CurrentCulture);

            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, buySymbol);
            Order buyModel = new Order(
                                        invalidBuySymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(buyModel);

            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, sellSymbol);
            Order sellModel = new Order(
                                        sellSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        sellSymbolShare,
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(sellModel);

            Button submitAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitAllButton"));
            submitAllButton.Click();

            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander").AndControlType(typeof(GroupBox)));
            Assert.IsNotNull(buySellSymbolGroup);
            
            //validate the transaction was successful
            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsNull(orders.Find(o => o.Symbol.Equals(buySymbol)));
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(sellSymbol)).Equals(sellModel));
        }

        /// <summary>
        /// Check if user can buy the stock when buy/sell panel contains valid buy and invalid sell data
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position Table and Select buy Option
        /// 3  Enter the data for Required fields 
        /// 4. Right-Click on the selected stock in Position Table and Select sell Option
        /// 5. Enter the data for Required fields with invalid sell symbol
        /// 6. check for submit All button disable.
        ///
        /// Expected Result:
        /// User should not buy the shares.
        /// "Submit All" button should be disable
        /// </summary>
        [TestMethod]
        public void AttemptValidBuyAndInvalidSellSimultaneouslyWithMultipleRequests()
        {
            string buySymbol;
            string sellSymbol;
            string invalidSellSymbol;

            buySymbol = TestDataInfrastructure.GetTestInputData("Symbol");
            sellSymbol = TestDataInfrastructure.GetTestInputData("PositionSymbol1");
            invalidSellSymbol = TestDataInfrastructure.GetTestInputData("InvalidSymbol");

            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, buySymbol);
            Order buyModel = new Order(
                                        buySymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(buyModel);

            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, sellSymbol);
            Order sellModel = new Order(
                                        invalidSellSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                        );
            PopulateBuySellPanelWithData(sellModel);

            Button submitAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitAllButton"));
            Assert.IsFalse(submitAllButton.Enabled);
        }

        /// <summary>
        /// Check if user can buy the multi selected symbols shares.
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Right-Click on the selected stocks in Position Table and Select buy Option
        /// 3. Enter the data for Required fields for symbol.
        /// 4. Get the handle of another symbol's buy/sell panel for buy 
        /// 5. Enter the data for Required fields for symbol.
        /// 6. Click on "Submit All" button
        ///
        /// Expected Result:
        /// User should buy the multi-selected symbol shares.
        /// 
        /// Test case ignored as the "multi-select" functionality is out-of-scope for the RI application.
        /// </summary>
        [TestMethod]
        [Ignore]
        public void MultiSelectSharesFromPositionTableAndBuyAll()
        {
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));

            //handle multiple row selection in watchlist and open buysell panel for buy
            list.MultiSelect(0);
            list.MultiSelect(1);
            list.Rows[0].RightClick();

            System.Threading.Thread.Sleep(1000);
            Window.PopupMenu("Buy").Click();

            //check for first expander panel
            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox))
                                                                                .AndIndex(0));
            Assert.IsNotNull(buySellSymbolGroup);

            //check for second expander panel
            buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox))
                                                                                .AndIndex(1));
            Assert.IsNotNull(buySellSymbolGroup);
        }

        /// <summary>
        /// Check if user can sell the multi selected symbols shares.
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Get the number of shares for a symbols.
        /// 3. Right-Click on the selected stocks in Position Table and Select sell Option
        /// 4. Enter the data for Required fields along with number of shares for symbol.
        /// 5. Get the handle of another symbol's buy/sell panel for sell 
        /// 6. Enter the data for Required fields along with number of shares for symbol.
        /// 7. Click on "Submit All" button
        ///
        /// Expected Result:
        /// User should sell the multi-selected symbol shares.
        ///
        /// Test case ignored as the "multi-select" functionality is out-of-scope for the RI application.
        /// </summary>
        [TestMethod]
        [Ignore]
        public void MultiSelectSharesFromPositionTableAndSellAll()
        {
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));

            list.MultiSelect(0);
            list.MultiSelect(1);
            list.Rows[0].RightClick();

            System.Threading.Thread.Sleep(1000);
            Window.PopupMenu("Sell").Click();

            //check for first expander panel
            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox))
                                                                                .AndIndex(0));
            Assert.IsNotNull(buySellSymbolGroup);

            //check for second expander panel
            buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox))
                                                                                .AndIndex(1));
            Assert.IsNotNull(buySellSymbolGroup);
        }

        /// <summary>
        /// Check if user can sell and buy the same share simultaneously
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Get the number of shares for a symbol
        /// 3. Right-Click on the selected stock in Position Table and Select sell Option
        /// 4. Enter the data for Required fields with number of shares for a symbol 
        /// 5. Right-Click on the same selected stock in Position Table and Select buy Option
        /// 6. Enter the data for Required fields
        /// 5. Click on "Submit All" button
        /// 
        /// Expected Result:
        /// User should buy and sell same share simultaneously
        /// </summary>
        [TestMethod]
        public void BuyAndSellSameShareSimultaneouslyWithMultipleRequests()
        {
            string symbol;
            int symbolShare = 0;

            string positionTableSymbolHeader = TestDataInfrastructure.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = TestDataInfrastructure.GetTestInputData("PositionTableShares");

            symbol = TestDataInfrastructure.GetTestInputData("Symbol");

            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            symbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(symbol)).Cells[positionTableSharesHeader].Text, CultureInfo.CurrentCulture);

            //Buy the share
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, symbol);
            Order buyModel = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        symbolShare,
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(buyModel);
            ///////////////////////////////
            
            //Sell the share
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order sellModel = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        symbolShare,
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(sellModel);
            ///////////////////////////////

            Button submitAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitAllButton"));
            submitAllButton.Click();

            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox)));
            Assert.IsNull(buySellSymbolGroup);

            //validate if the sell and buy transaction was successful
            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsNotNull(orders.Find(o => o.Equals(buyModel)));
            Assert.IsNotNull(orders.Find(o => o.Equals(sellModel)));
        }

        /// <summary>
        /// Check if user can sell and buy different shares across multiple requests.
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Get the number of shares for a symbols.
        /// 3. Right-Click on the selected stock in Position Table and Select Sell Option
        /// 4. Enter the data for Required fields with symbol's number of shares.
        /// 5. Repeat steps 3 & 4 for different stock
        /// 6. Right-Click on the selected stock in Position Table and Select buy Option
        /// 7. Enter the data for Required fields.
        /// 8. Repeat steps 6 & 7 for different stock.
        /// 9. Click on "Submit All" button
        /// 
        /// Expected Result:
        /// User should sell and buy different shares simultaneously with multiple buy/sell requests.
        [TestMethod]
        public void BuyAndSellDifferentSharesSimultaneouslyWithMultipleRequests()
        {
            string sellSymbol;
            string sellAnotherSymbol;
            string buySymbol;
            string buyAnotherSymbol;
            int symbolShare = 0;
            int anotherSymbolShare = 0;
            string positionTableSymbolHeader = TestDataInfrastructure.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = TestDataInfrastructure.GetTestInputData("PositionTableShares");

            sellSymbol = TestDataInfrastructure.GetTestInputData("Symbol");
            sellAnotherSymbol = TestDataInfrastructure.GetTestInputData("PositionSymbol");
            buySymbol = TestDataInfrastructure.GetTestInputData("PositionSymbol1");
            buyAnotherSymbol = TestDataInfrastructure.GetTestInputData("PositionSymbol3");

            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            symbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(sellSymbol)).Cells[positionTableSharesHeader].Text, CultureInfo.CurrentCulture);
            anotherSymbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(sellAnotherSymbol)).Cells[positionTableSharesHeader].Text, CultureInfo.CurrentCulture);

            //Sell a share
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, sellSymbol);
            Order sellModel = new Order(
                                        sellSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        symbolShare,
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(sellModel);
            //////////////////////////////////////////////

            //Sell another share
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, sellAnotherSymbol);
            Order anotherSellModel = new Order(
                                        sellAnotherSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        anotherSymbolShare,
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherSellModel);
            /////////////////////////////////////////////////////////////

            //Buy a share
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, buySymbol);
            Order buyModel = new Order(
                                         buySymbol,
                                         Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                         TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                         int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                         TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                         BuySellEnum.Buy.ToString()
                                     );
            PopulateBuySellPanelWithData(buyModel);
            /////////////////////////////////////////////////////////////

            //Buy another share
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, buyAnotherSymbol);
            Order anotherBuyModel = new Order(
                                        buyAnotherSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherBuyModel);
            /////////////////////////////////////////////////////////////

            Button submitAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitAllButton"));
            submitAllButton.Click();

            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox)));
            Assert.IsNull(buySellSymbolGroup);

            //validate if the sell transaction was successful
            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsNotNull(orders.Find(o => o.Equals(sellModel)));
            Assert.IsNotNull(orders.Find(o => o.Equals(anotherSellModel)));
            Assert.IsNotNull(orders.Find(o => o.Equals(buyModel)));
            Assert.IsNotNull(orders.Find(o => o.Equals(anotherBuyModel)));
        }

        /// <summary>
        /// Check if overwriting valid number of shares with an invalid string disables the "Submit" button
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields
        /// 4. Overwrite the number of shares with an invalid string
        /// 5. Check if the "Submit" button is enabled
        /// 
        /// Expected Result:
        /// the "Submit" button should be disabled
        /// </summary>
        [TestMethod]
        public void AttemptSellStockByEnteringValidValuesFirstAndThenOverwritingWithInvalidValues()
        {
            string symbol;
            int numberOfShares = 0;

            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);

            // Select the position table
            SelectPositionTabPage();

            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            numberOfShares = (int)list.GetData(symbol, PositionTableColumnHeader.NumberOfShares);

            Order model = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        numberOfShares,
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(model);

            //overwrite invalid "number of shares" value
            
            TextBox shareTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellSharesTextBox"));
            shareTextBox.Text = TestDataInfrastructure.GetTestInputData("InvalidString");

            //check if the submit button is disabled
            Button submitButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitButton"));
            submitButton.Focus();
            Assert.IsFalse(submitButton.Enabled);
        }
    }
}
