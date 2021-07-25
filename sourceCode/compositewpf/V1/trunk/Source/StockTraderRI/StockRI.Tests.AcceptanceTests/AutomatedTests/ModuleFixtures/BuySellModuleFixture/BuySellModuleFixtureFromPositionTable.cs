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
using Core.UIItems.ListBoxItems;
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
        /// Launch the Buy/Sell Screen for buy - submit, cancel, submit all, cancel all buttons and tab with appropriate controls display 
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position table and Select Buy Option
        /// 3. Validate that the Buy/Sell Screen loads with the related data
        /// 4. Validate that the default values are correct (Identify the default values)
        /// 
        /// Expected Result:
        /// Submit, Cancel, Submit All, Cancel All buttons and Tab with appropriate controls ahould displayed
        ///
        /// </summary>
        [TestMethod]
        [WorkItem(16962)]
        public void BuySellScreenLoadForBuyFromPositionTable()
        {
            string symbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, symbol);

            Order model =
               new Order(symbol, 0,
               String.Empty, 0,
               String.Empty,
               BuySellEnum.Buy.ToString());

            ValidateBuySellPanelControls(BuySellEnum.Buy);
            ValidateBuySellPanelData(model);
        }

        /// <summary>
        /// Launch the Buy/Sell Screen for buy - submit, cancel, submit all, cancel all buttons and tab with appropriate controls display 
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position Table and Select Sell Option
        /// 3. Validate that the Buy/Sell Screen loads with the related data
        /// 4. Validate that the default values are correct (Identify the default values)
        /// 
        /// Expected Result:
        /// Submit, Cancel, Submit All, Cancel All buttons and Tab with appropriate controls ahould displayed
        ///
        /// </summary>
        [TestMethod]
        [WorkItem(16945)]
        public void BuySellScreenLoadForSellFromPositionTable()
        {
            string symbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);

            Order model =
               new Order(symbol, 0,
               String.Empty, 0,
               String.Empty,
               BuySellEnum.Sell.ToString());

            //validate if loaded screen has correct pre-defined data loaded and the necessary controls
            ValidateBuySellPanelControls(BuySellEnum.Sell);
            ValidateBuySellPanelData(model);
        }

        /// <summary>
        /// Check if the Submit button click buys the stock
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position Table and select Buy Option
        /// 3. Enter the data for Required fields (Identify the required fields)
        /// 4. Click on "Submit" button
        /// 
        /// Expected Result:
        /// Submit button click should buys the stock.
        ///
        /// </summary>
        [TestMethod]
        [WorkItem(16746)]
        public void BuyStockFromPositionTableByClickingSubmit()
        {
            string symbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, symbol);

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

            // Validate if selected tab in Buy/Sell Panel disappears.
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
        /// Check if the Submit button click sells the stock
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields (Identify the required fields)
        /// 4. Click on "Submit" button
        /// 
        /// Expected Result:
        /// Submit button click should sells the stock.
        ///
        /// </summary>
        [TestMethod]
        [WorkItem(16747)]
        public void SellStockFromPositionTableByClickingSubmit()
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
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(model);

            Button submitButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitButton"));
            submitButton.Click();

            //give time for submit processing
            System.Threading.Thread.Sleep(2000);

            // Validate if the expander Buy/Sell Panel disappears.
            // AND
            // Validate if the sell transactions was successful 
            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox)));
            Assert.IsNull(buySellSymbolGroup);

            //validate if the sell transaction was successful
            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(symbol)).Equals(model));

        }

        /// <summary>
        /// Check if the Submit All button click buys multiple stocks
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a selected stock in Position table and select Buy Option
        /// 3. Enter the data for Required fields (Identify the required fields)
        /// 4. Repeat steps 2 & 3 for different stocks, at least 2.
        /// 5. Click on "Submit All" button in Buy/Sell Screen
        /// 
        /// Expected Result:
        /// Submit All button click should buys multiple stocks
        ///
        /// </summary>
        [TestMethod]
        [WorkItem(16748)]
        public void BuyMultipleStocksFromPositionTableByClickingSubmitAll()
        {
            string symbol;
            string anotherSymbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            anotherSymbol = TestDataInfrastructure.GetTestInputData("PositionSymbol");

            /////////////////////////////////
            //Enter Share 1 Details
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, symbol);
            Order model = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(model);
            /////////////////////////////////

            /////////////////////////////////
            //Enter Share 2 Details
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, anotherSymbol);
            Order anotherModel = new Order(
                                        anotherSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherModel);
            /////////////////////////////////

            Button submitAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitAllButton"));
            submitAllButton.Click();

            //give time for submit processing
            System.Threading.Thread.Sleep(2000);

            // Validate if all expander Buy/Sell Panels disappear.
            // AND
            // Validate if the buy transactions were successful
            //SearchCriteria.ByAutomationId("Expander").AndControlType(typeof(GroupBox))
            GroupBox buySellSymbolGroup = Window.Get<GroupBox>("CompositeExpander");
            Assert.IsNull(buySellSymbolGroup);

            //validate if the buy transactions were successful
            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsNotNull(orders.Find(o => o.Symbol.Equals(symbol)));
            Assert.IsNotNull(orders.Find(o => o.Symbol.Equals(anotherSymbol)));

            // Validate the correctness of data.
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(symbol)).Equals(model));
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(anotherSymbol)).Equals(anotherModel));
        }

        /// <summary>
        /// Check if the Submit All button click sells multiple stocks
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a selected stock in Position Table and select Sell Option
        /// 3. Enter the data for Required fields (Identify the required fields)
        /// 4. Repeat steps 2 & 3 for different stocks, at least 2.
        /// 5. Click on "Submit All" button in Buy/Sell Screen
        /// 
        /// Expected Result:
        /// Submit All button click should sells multiple stocks
        ///
        /// </summary>        
        [TestMethod]
        [WorkItem(16749)]
        public void SellMultipleStocksFromPositionTableByClickingSubmitAll()
        {
            string symbol;
            string anotherSymbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            anotherSymbol = TestDataInfrastructure.GetTestInputData("PositionSymbol");

            /////////////////////////////////
            //Enter Share 1 Details
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order model = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(model);
            /////////////////////////////////

            /////////////////////////////////
            //Enter Share 2 Details
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, anotherSymbol);
            Order anotherModel = new Order(
                                        anotherSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherModel);

            Button submitAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitAllButton"));
            submitAllButton.Click();

            //give time for submit processing
            System.Threading.Thread.Sleep(2000);

            // Validate if expander Buy/Sell Panels disappear.
            // AND
            // Validate if the sell transactions were successful
            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox)));
            Assert.IsNull(buySellSymbolGroup);

            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsNotNull(orders.Find(o => o.Symbol.Equals(symbol) || o.Symbol.Equals(anotherSymbol)));

            // Validate the correctness of data.
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(symbol)).Equals(model));
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(anotherSymbol)).Equals(anotherModel));
        }

        /// <summary>
        /// Checks if Cancel button clicks after entering stock buy details not buys the stock.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position table and select Buy Option
        /// 3. Enter the data for Required fields (Identify the required fields)
        /// 4. Click on "Cancel" button
        /// 
        /// Expected Result:
        /// Cancel button clicks should not buy the entered stock.
        /// Tab should be be closed
        /// </summary>
        [TestMethod]
        [WorkItem(16750)]
        public void LaunchBuySellPanelFromPositionTableAndEnterStockBuyDetailsAndClickCancel()
        {
            string symbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, symbol);
            Order model = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(model);

            Button cancelButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellCancelButton"));
            cancelButton.Click();

            //give time for cancel processing
            System.Threading.Thread.Sleep(2000);

            // Validate if expander Buy/Sell Panel disappears.
            // AND
            // Validate if the buy transaction did not proceed.
            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox)));
            Assert.IsNull(buySellSymbolGroup);

            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsNull(orders.Find(o => o.Symbol.Equals(symbol)));
        }

        /// <summary>
        /// Checks if Cancel All button clicks after entering mutliple stock sell details not sells the stock
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields (Identify the required fields)
        /// 4. Repeate 2 & 3 for at least 2 different stocks
        /// 5. Click on "Cancel All" button
        /// 
        /// Expected Result:
        /// 
        /// Cancel All button clicks should not sell the multiple entered stocks.
        /// Tab should be be closed
        /// </summary>
        [TestMethod]
        [WorkItem(16751)]
        public void LaunchBuySellPanelFromPositionTableAndEnterMultipleStocksSellDetailsAndClickCancelAll()
        {
            string symbol;
            string anotherSymbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            anotherSymbol = TestDataInfrastructure.GetTestInputData("PositionSymbol");

            /////////////////////////////////
            //Enter Share 1 Details
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order model = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(model);
            /////////////////////////////////

            /////////////////////////////////
            //Enter Share 2 Details
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, anotherSymbol);
            Order anotherModel = new Order(
                                        anotherSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherModel);

            Button cancelAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellCancelAllButton"));
            cancelAllButton.Click();

            //give time for cancel processing
            System.Threading.Thread.Sleep(2000);

            // Validate if expander Buy/Sell Panels disappear.
            // AND
            // Validate if the buy transaction did not proceed.
            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox)));
            Assert.IsNull(buySellSymbolGroup);

            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsNull(orders.Find(o => o.Symbol.Equals(symbol) || o.Symbol.Equals(anotherSymbol)));
        }

        /// <summary>
        /// Check if the 'Buy At Last' button click buy the stock
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position table and select Buy Option
        /// 3. Enter the data for Required fields (Identify the required fields)
        /// 4. Select the "Buy At Last"
        /// 5. Validate that the data on the screen matches the last price
        /// 
        /// Expected Result:
        /// 'Buy At Last' button click should buy the stock, if data on the screen matches the  last price
        /// </summary>
        //TODO: Buy At Last button is not part of the Buy/Sell Panel yet
        [Ignore]
        [TestMethod]
        public void BuyStockFromPositionTableByClickingBuyAtLast()
        {
            string symbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, symbol);
            Order model = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(model);

            Button buyAtLastButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellBuyLastButton"));
            buyAtLastButton.Click();

            //get the last price for the share
            List<Market> market = TestDataInfrastructure.GetData<MarketDataProvider, Market>();
            model.LimitStopPrice = market.Find(m => m.TickerSymbol.Equals(symbol)).LastPrice;

            Button submitButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitButton"));
            submitButton.Click();

            //give time for submit processing
            System.Threading.Thread.Sleep(2000);

            // Validate if the tab disappears.
            Tab buySellSymbolTab = Window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Buy.ToString() + " " + symbol).AndControlType(typeof(Tab)));
            Assert.IsNull(buySellSymbolTab);

            //validate if the buy transaction was successful
            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(symbol)).Equals(model));

        }

        /// <summary>
        /// Check if the 'Sell At Last' button click sell the stock
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position Table and select Sell Option
        /// 3. Enter the data for Required fields (Identify the required fields)
        /// 4. Select the "Sell At Last"
        /// 5. Validate that the data on the screen matches the last price
        /// 
        /// Expected Result:
        /// 'Sell At Last' button click should sell the stock, if data on the screen matches the  last price
        /// </summary>
        //TODO: Sell At Last button is not part of the Buy/Sell Panel yet
        [Ignore]
        [TestMethod]
        public void SellStockFromPositionTableByClickingSellAtLast()
        {
            string symbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order model = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellTfimeInForce"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(model);

            Button sellAtLastButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSellLastButton"));
            sellAtLastButton.Click();

            //get the last price for the share
            List<Market> market = TestDataInfrastructure.GetData<MarketDataProvider, Market>();
            model.LimitStopPrice = market.Find(m => m.TickerSymbol.Equals(symbol)).LastPrice;

            Button submitButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitButton"));
            submitButton.Click();

            //give time for submit processing
            System.Threading.Thread.Sleep(2000);

            // Validate if the tab disappears.
            Tab buySellSymbolTab = Window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Sell.ToString() + " " + symbol).AndControlType(typeof(Tab)));
            Assert.IsNull(buySellSymbolTab);

            //validate if the buy transaction was successful
            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(symbol)).Equals(model));

        }

        /// <summary>
        /// Checks if Toolbar Sumbit button click buys the stock
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a selected stock in Position table and select Buy Option
        /// 3. Enter the data for Required fields (Identify the required fields)
        /// 4. Click on "Submit" button in Buy/Sell Screen
        /// 
        /// Expected Result:
        /// Toolbar Sumbit button click should buy the stock
        /// </summary>
        [TestMethod]
        [WorkItem(16754)]
        public void BuyStockFromPositionTableByClickingToolbarSubmit()
        {
            string symbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, symbol);
            Order model = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(model);

            Button submitButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("ToolbarSubmitButton"));
            submitButton.Click();

            //give time for submit processing
            //System.Threading.Thread.Sleep(2000);

            // Validate if extender Buy/Sell Panel disappears.
            // AND
            // Validate if the buy transactions were successful
            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox)));
            Assert.IsNull(buySellSymbolGroup);

            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(symbol)).Equals(model));

        }

        /// <summary>
        /// Check if the Toolbar 'Submit All' button click buys multiple stocks
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a selected stock in Position Table and select Sell Option
        /// 3. Enter the data for Required fields (Identify the required fields)
        /// 4. Repeat steps 2 & 3 for different stocks, at least 2.
        /// 5. Click on "Submit All" button in Buy/Sell Screen
        /// 
        /// Expected Result:
        /// Toolbar 'Submit All' button click should buys multiple stocks
        /// 
        /// </summary>
        [TestMethod]
        [WorkItem(16754)]
        public void BuyMultipleStocksFromPositionTableByClickingToolbarSubmitAll()
        {
            string symbol;
            string anotherSymbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            anotherSymbol = TestDataInfrastructure.GetTestInputData("PositionSymbol");

            /////////////////////////////////
            //Enter Share 1 Details
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, symbol);
            Order model = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(model);
            /////////////////////////////////

            /////////////////////////////////
            //Enter Share 2 Details
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, anotherSymbol);
            Order anotherModel = new Order(
                                        anotherSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherModel);
            /////////////////////////////////

            Button submitAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("ToolbarSubmitAllButton"));
            submitAllButton.Click();

            //give time for submit processing
            System.Threading.Thread.Sleep(2000);

            // Validate if all expander Buy/Sell Panels disappear.
            // AND
            // Validate if the buy transactions were successful
            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox)));
            Assert.IsNull(buySellSymbolGroup);

            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsNotNull(orders.Find(o => o.Symbol.Equals(symbol)));
            Assert.IsNotNull(orders.Find(o => o.Symbol.Equals(anotherSymbol)));

            // Validate the correctness of data.
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(symbol)).Equals(model));
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(anotherSymbol)).Equals(anotherModel));
        }

        /// <summary>
        /// Checks if Toolbar 'Cancel' button clicks after entering stock buy details not buy the stocks
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on the selected stock in Watch List and select Buy Option
        /// 3. Enter the data for Required fields (Identify the required fields)
        /// 4. Click on "Cancel" button
        /// 
        /// Expected Result:
        /// Toolbar 'Cancel' button clicks should not buy the enter stock.
        /// Tab should be be closed.
        /// 
        /// </summary>
        [TestMethod]
        [WorkItem(16756)]
        public void LaunchBuySellPanelFromPositionTableAndEnterStockBuyDetailsAndClickToolbarCancel()
        {
            string symbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, symbol);
            Order model = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(model);

            Button cancelButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("ToolbarCancelButton"));
            cancelButton.Click();

            //give time for cancel processing
            System.Threading.Thread.Sleep(2000);

            // Validate if expander Buy/Sell Panel disappears.
            // AND
            // Validate if the buy transaction did not proceed.
            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox)));
            Assert.IsNull(buySellSymbolGroup);

            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsNull(orders.Find(o => o.Symbol.Equals(symbol)));
        }

        /// <summary>
        /// Checks if Toolbar 'Cancel All' button clicks after entering mutliple stock sell details not sell the stocks
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields (Identify the required fields)
        /// 4. Repeate 2 & 3 for at least 2 different stocks
        /// 5. Click on "Cancel All" button
        /// 
        /// Expected Result:
        /// Toolbar 'Cancel All' button clicks should not sell the multiple entered stocks
        /// Tab should be be closed
        /// </summary>
        [TestMethod]
        [WorkItem(16757)]
        public void LaunchBuySellPanelFromPositionTableAndEnterMultipleStocksSellDetailsAndClickToolbarCancelAll()
        {
            string symbol;
            string anotherSymbol;
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            anotherSymbol = TestDataInfrastructure.GetTestInputData("PositionSymbol");

            /////////////////////////////////
            //Enter Share 1 Details
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order model = new Order(
                                        symbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(model);
            /////////////////////////////////

            /////////////////////////////////
            //Enter Share 2 Details
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, anotherSymbol);
            Order anotherModel = new Order(
                                        anotherSymbol,
                                        Decimal.Parse(TestDataInfrastructure.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("BuySellOrderType"),
                                        int.Parse(TestDataInfrastructure.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        TestDataInfrastructure.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherModel);
            /////////////////////////////////

            Button cancelAllButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("ToolbarCancelAllButton"));
            cancelAllButton.Click();

            //give time for cancel processing
            System.Threading.Thread.Sleep(2000);

            // Validate if all tabs in Buy/Sell Panel disappears.
            // AND
            // Validate if the sell transaction did not proceed.
            GroupBox buySellSymbolGroup = Window.Get<GroupBox>(SearchCriteria.ByAutomationId("CompositeExpander")
                                                                                .AndControlType(typeof(GroupBox)));
            Assert.IsNull(buySellSymbolGroup);

            List<Order> orders = TestDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsNull(orders.Find(o => o.Symbol.Equals(symbol) || o.Symbol.Equals(anotherSymbol)));
        }
    }
}