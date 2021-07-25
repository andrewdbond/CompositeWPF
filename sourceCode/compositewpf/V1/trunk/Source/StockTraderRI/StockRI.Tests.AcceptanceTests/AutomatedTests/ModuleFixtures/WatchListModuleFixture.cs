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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Automation;
using Core.UIItems;
using Core.UIItems.ListViewItems;
using Core.UIItems.Finders;
using Core.UIItems.TabItems;
using Core.UIItems.MenuItems;
using StockTraderRI.AcceptanceTests.TestInfrastructure.MockModels;
using StockTraderRI.AcceptanceTests.TestInfrastructure;
using StockTraderRI.AcceptanceTests.Helpers;
using System.Globalization;

namespace StockTraderRI.AcceptanceTests.AutomatedTests
{
    [TestClass]
    [DeploymentItem(@".\StockTraderRI\bin\Debug")]
    [DeploymentItem(@".\StockRI.Tests.AcceptanceTests\bin\Debug")]
    public class WatchListModuleFixture : FixtureBase
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
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
        /// On hover over watch list, watch list window should be displayed.
        /// 
        /// Repro Steps:
        /// 
        /// 1 Launch the Stock Trader application.
        /// 2 Mouse Hover over the watch list
        /// 3 Check the watch list window is not collapsed, when hover on watch list
        /// 
        /// Expected Result:
        /// When mouse hover over on watch list, Watch list window should be displayed
        /// </summary>
        [Ignore]
        [TestMethod]        
        public void WatchListHoverShow()
        {
            ListView watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));
            UIItem watchListTab = Window.GetWatchListRegionHeader();
            watchListTab.Hover();
            watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));

            Assert.IsFalse(IsWatchListWindowCollapsed(watchList));

            //**** if WatchList button loses focus, the WatchList window should collapse back ****
            //to lose focus on the WatchList button, hover over the Account Position List
            UIItem otherControl = Window.Get<UIItem>(TestDataInfrastructure.GetControlId("WatchListAddTextBox"));
            otherControl.Hover();
            
            //WatchList window should collapse
            Assert.IsTrue(IsWatchListWindowCollapsed(watchList));
        }

        /// <summary>
        /// Check on watch list click , watch list window should be displayed.
        /// 
        /// Repro Steps:
        /// 1 Launch the Stock Trader application.
        /// 2 Click on Watch list
        /// 3 Check watch list window is not collapsed
        /// 4 Click on different region 
        /// 5 Check watch list window is collapse
        /// 
        /// Expected Result:
        /// On watchList click: watchlist window should be displayed.
        /// </summary>
        [Ignore]
        [TestMethod]      
        public void WatchListClickShow()
        {
            ListView watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));
            UIItem watchListTab = Window.GetWatchListRegionHeader();
            watchListTab.Click();
            watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));

            Assert.IsFalse(IsWatchListWindowCollapsed(watchList));

            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            list.Click();
            
            Assert.IsTrue(IsWatchListWindowCollapsed(watchList));
        }

        /// <summary>
        /// Check on pin watch list panel, watch list window should pinned (display visible) and 
        /// on unpin watch list panel, watch list window should get unpinned (display collapsed).
        /// 
        /// Repro Steps:
        /// 1 Launch the Stock Trader application
        /// 2 Click on watch list pin image
        /// 3 Check watch list window should not collapse
        /// 4 Click on watch list pin image again
        /// 5 Check watch list window is collapsed
        /// 
        /// Expected Result:
        /// On pin watch list panel: watch list window should get pinned (display visible)
        /// On unpin watch list panel: watch list window should get unpinned (display collapsed)
        /// </summary>
        //TODO: Find a way to get the pin toggle button 
        [Ignore]
        [TestMethod]
        public void WatchListPin()
        {
            ListView watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));
            UIItem watchListTabHeader = Window.GetWatchListRegionHeader();
            watchListTabHeader.Click();
            watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));

            //pin the watch list panel
            //Button pinImage = window.Get<Button>(TestDataInfrastructure.GetControlId("WatchListHideButton"));
            Button pinImage = Window.Get<Button>("HeaderAutoHideButton");
            pinImage.Click();

            Assert.IsFalse(IsWatchListWindowCollapsed(watchList));
            Assert.IsFalse(watchListTabHeader.Visible);

            //to lose focus on the WatchList button, click somewhere on the Account Position List
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            list.Click();
            
            Assert.IsFalse(IsWatchListWindowCollapsed(watchList));

            //unpin the watch list panel (by clicking on the pin again)
            pinImage.Click();
            
            Assert.IsTrue(watchListTabHeader.Visible);
            Assert.IsTrue(IsWatchListWindowCollapsed(watchList));
        }

        /// <summary>
        /// Check if the entered symbol gets added to the watchlist
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Enter a valid Symbol in the Add WatchList Textbox
        /// 3. Click on the Add WatchList button
        /// 4. Check if the entered Symbol has got added to the WatchList - by checking the count and
        /// by checking if the WatchList listview has the entered Symbol
        /// 
        /// Expected Result:
        /// watchlist should have the newly added symbol
        [TestMethod]
        public void AddWatchList()
        {
            ListView watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));
            int watchListCountBeforeClick = watchList.Rows.Count;
            
            TextBox addWatchTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("WatchListAddTextBox"));
            addWatchTextBox.Text = TestDataInfrastructure.GetTestInputData("WatchListSymbol");
            
            Button addWatchListButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("WatchListAddButton"));
            addWatchListButton.Click();

            Assert.AreEqual(watchListCountBeforeClick + 1, watchList.Rows.Count);

            //also check if the added symbol appears in the Watchlist window
            Assert.IsNotNull(watchList.Rows.Find(r => r.Cells[0].Text == addWatchTextBox.Text));
        }

        /// <summary>
        /// Check if the Symbol added to WatchList, using the Add WatchList button, has correct last price
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Enter a valid Symbol in the Add WatchList Textbox
        /// 3. Click on the Add WatchList button
        /// 4. Check if the entered Symbol has got added to the WatchList - by checking the count
        /// 5. Check if the newly added Symbol in the WatchList has correct last price as mentioned in the Market.xml
        /// 
        /// Expected Result:
        /// 1. Successful addition of entered Symbol to the WatchList
        /// 2. Added Symbol has the correct last price displayed in the WatchList
        /// </summary>
        [TestMethod]
        public void WatchListTableContent()
        {
           
            ListView watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));
            int watchListCountBeforeClick = watchList.Rows.Count;

            TextBox addWatchTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("WatchListAddTextBox"));
            addWatchTextBox.Text = TestDataInfrastructure.GetTestInputData("WatchListSymbol");

            Button addWatchListButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("WatchListAddButton"));
            addWatchListButton.Click();

            Assert.AreEqual(watchListCountBeforeClick + 1, watchList.Rows.Count);

            List<Market> market = TestDataInfrastructure.GetData<MarketDataProvider, Market>();

            //check if the added symbol has the correct last price value displayed
            Assert.IsNotNull(watchList.Rows.Find(r => 
                ((r.Cells[0].Text == addWatchTextBox.Text)
                && (r.Cells[1].Text == market.Find(m => m.TickerSymbol == addWatchTextBox.Text).LastPrice.ToString(CultureInfo.CurrentCulture))
                )));
        }

        /// <summary>
        /// Check if the same symbol can be added twice to the watchlist
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Enter a valid Symbol in the Add WatchList Textbox
        /// 3. Click on the Add WatchList button
        /// 4. Check if the entered Symbol has got added to the WatchList - by checking the count and
        /// by checking if the WatchList listview has the entered Symbol
        /// 5. Click on the Add WatchList button again
        /// 6. Check if the Symbol has not got added to the WatchList again.
        /// 
        /// Expected Result:
        /// Same Symbol should not get added twice to the WatchList
        [TestMethod]
        public void AddSymbolTwiceToWatchList()
        {
            ListView watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));
            int watchListCountBeforeClick = watchList.Rows.Count;

            TextBox addWatchTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("WatchListAddTextBox"));
            addWatchTextBox.Text = TestDataInfrastructure.GetTestInputData("WatchListSymbol");

            Button addWatchListButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("WatchListAddButton"));
            //click the Add to WatchList button twice and check if the symbol gets added twice
            addWatchListButton.Click();
            addWatchListButton.Click();

            Assert.AreEqual(watchListCountBeforeClick + 1, watchList.Rows.Count);
            //only one instance of the Symbol should be there in the watch list
            Assert.AreEqual(1, watchList.Rows.FindAll(r => r.Cells[0].Text == addWatchTextBox.Text).Count);
        }

        /// <summary>
        /// Check if the an invalid symbol gets added to the watchlist
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Enter a invalid Symbol in the Add WatchList Textbox
        /// 3. Click on the Add WatchList button
        /// 4. Check if the entered invalid Symbol does not get added to the WatchList - by checking the count and
        /// by checking if the WatchList listview does not have the entered invalid Symbol
        /// 
        /// Expected Result:
        /// Invalid Symbol should not get added to the WatchList
        [TestMethod]
        public void AddInvalidSymbolToWatchList()
        {
            ListView watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));
            int watchListCountBeforeClick = watchList.Rows.Count;

            TextBox addWatchTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("WatchListAddTextBox"));
            addWatchTextBox.Text = TestDataInfrastructure.GetTestInputData("InvalidSymbol");

            Button addWatchListButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("WatchListAddButton"));
            addWatchListButton.Click();

            List<AccountPosition> position = TestDataInfrastructure.GetData<AccountPositionDataProvider, AccountPosition>();
            //ensure that the entered symbol is indeed invalid
            Assert.IsNull(position.Find(ap => ap.TickerSymbol.Equals(addWatchTextBox.Text)));

            Assert.AreEqual(watchListCountBeforeClick, watchList.Rows.Count);
            //also check if the invalid symbol does not appear in the Watchlist window
            Assert.IsNull(watchList.Rows.Find(r => r.Cells[0].Text == addWatchTextBox.Text));
        }

        /// <summary>
        /// Check if the entered symbol gets added to the watchlist and the Watchlist expands to display
        /// on click of watchlist region header.
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Enter a valid Symbol in the Add WatchList Textbox
        /// 3. Click on the Add WatchList button
        /// 4. Check if the entered Symbol has got added to the WatchList - by checking the count
        /// 5. Click on Watchlist region header.
        /// 6. Check if the watch list expanded
        /// 
        /// Expected Result:
        /// Watchlist should have the newly added symbol and the
        /// Watchlist should be expanded again on click on region header
        [Ignore]
        [TestMethod]       
        public void AddingWatchListItemFromToolBarAddsRowToListAndExpandsWatchList()
        {
            Window.GetWatchListRegionHeader().Click();
            ListView watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));
            int oldCount = watchList.Rows.Count;
            TextBox addWatchListTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("WatchListAddTextBox"));
            addWatchListTextBox.Text = TestDataInfrastructure.GetTestInputData("WatchListSymbol");

            Button addWatchListButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("WatchListAddButton"));
            addWatchListButton.Click();

            Window.GetWatchListRegionHeader().Click();
            Assert.AreEqual(oldCount + 1, watchList.Rows.Count);
            Assert.IsFalse(IsWatchListWindowCollapsed(watchList));
        }

        /// <summary>
        /// Check if the symbol with Leading Blank Space gets added to the watchlist
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Enter a Symbol with Leading Blank Space in the Add WatchList Textbox
        /// 3. Click on the Add WatchList button
        /// 4. Check if the entered symbol with Leading Blank Space does not get added to the WatchList - by checking the count and
        /// by checking if the WatchList listview does not have the entered symbol with Leading Blank Space.
        /// 
        /// Expected Result:
        /// Symbol should get added to the WatchList without the leading blank spaces
        [TestMethod]
        public void AddSymbolWithLeadingBlankSpacesToWatchList()
        {
            ListView watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));
            int watchListCountBeforeClick = watchList.Rows.Count;

            TextBox addWatchTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("WatchListAddTextBox"));
            addWatchTextBox.Text = TestDataInfrastructure.GetTestInputData("LeadingBlankSpacePositionSymbol");

            Button addWatchListButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("WatchListAddButton"));
            addWatchListButton.Click();

            List<AccountPosition> position = TestDataInfrastructure.GetData<AccountPositionDataProvider, AccountPosition>();
            Assert.IsNotNull(position.Find(ap => ap.TickerSymbol.Equals(addWatchTextBox.Text.TrimStart())));

            Assert.AreEqual(watchListCountBeforeClick + 1, watchList.Rows.Count);
            //also check if the Leading Blank Space symbol does not appear in the Watchlist window
            Assert.IsNotNull(watchList.Rows.Find(r => r.Cells[0].Text == addWatchTextBox.Text.TrimStart()));
        }

        /// <summary>
        /// Check if the symbol with Trailing Blank Space gets added to the watchlist
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Enter a symbol with Trailing Blank Space in the Add WatchList Textbox
        /// 3. Click on the Add WatchList butto
        /// 4. Check if the entered symbol with Trailing Blank Space does not get added to the WatchList - by checking the count and
        /// by checking if the WatchList listview does not have the entered symbol with Trailing Blank Space.
        /// 
        /// Expected Result:
        /// symbol should get added to the WatchList without Trailing Blank Space
        [TestMethod]
        public void AddSymbolWithTrailingBlankSpacesToWatchList()
        {
            ListView watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));
            int watchListCountBeforeClick = watchList.Rows.Count;

            TextBox addWatchTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("WatchListAddTextBox"));
            addWatchTextBox.Text = TestDataInfrastructure.GetTestInputData("TrailingBlankSpacePositionSymbol");

            Button addWatchListButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("WatchListAddButton"));
            addWatchListButton.Click();

            List<AccountPosition> position = TestDataInfrastructure.GetData<AccountPositionDataProvider, AccountPosition>();
            Assert.IsNotNull(position.Find(ap => ap.TickerSymbol.Equals(addWatchTextBox.Text.TrimEnd())));

            Assert.AreEqual(watchListCountBeforeClick + 1, watchList.Rows.Count);
            //also check if the symbol with Trailing Blank Space does not appear in the Watchlist window
            Assert.IsNotNull(watchList.Rows.Find(r => r.Cells[0].Text == addWatchTextBox.Text.TrimEnd()));
        }

        /// <summary>
        /// Check if the Space Character gets added to the watchlist
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Enter a Space Character in the Add WatchList Textbox
        /// 3. Click on the Add WatchList button
        /// 4. Check if the Space Character does not get added to the WatchList - by checking the count and
        /// by checking if the WatchList listview does not have the Space Character.
        /// 
        /// Expected Result:
        /// Space Character should not get added to the WatchList
        [TestMethod]
        public void AddSpaceCharacterToWatchList()
        {
            ListView watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));
            int watchListCountBeforeClick = watchList.Rows.Count;

            TextBox addWatchTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("WatchListAddTextBox"));
            addWatchTextBox.Text = TestDataInfrastructure.GetTestInputData("SpaceCharacter");

            Button addWatchListButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("WatchListAddButton"));
            addWatchListButton.Click();

            List<AccountPosition> position = TestDataInfrastructure.GetData<AccountPositionDataProvider, AccountPosition>();
            Assert.IsNull(position.Find(ap => ap.TickerSymbol.Equals(addWatchTextBox.Text)));

            Assert.AreEqual(watchListCountBeforeClick, watchList.Rows.Count);
            //also check if the Space Character does not appear in the Watchlist window
            Assert.IsNull(watchList.Rows.Find(r => r.Cells[0].Text == addWatchTextBox.Text));
        }

        /// <summary>
        /// Check if the Special characters symbol gets added to the watchlist
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Enter a Special characters symbol in the Add WatchList Textbox
        /// 3. Click on the Add WatchList button
        /// 4. Check if the entered Special characters symbol does not get added to the WatchList - by checking the count and
        /// by checking if the WatchList listview does not have the entered special characters symbol.
        /// 
        /// Expected Result:
        /// Special characters symbol should not get added to the WatchList
        [TestMethod]
        public void AddSpecialCharactersSymbolToWatchList()
        {
            ListView watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));
            int watchListCountBeforeClick = watchList.Rows.Count;

            TextBox addWatchTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("WatchListAddTextBox"));
            addWatchTextBox.Text = TestDataInfrastructure.GetTestInputData("SpecialCharacterSymbol");

            Button addWatchListButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("WatchListAddButton"));
            addWatchListButton.Click();

            List<AccountPosition> position = TestDataInfrastructure.GetData<AccountPositionDataProvider, AccountPosition>();
            Assert.IsNull(position.Find(ap => ap.TickerSymbol.Equals(addWatchTextBox.Text)));

            Assert.AreEqual(watchListCountBeforeClick, watchList.Rows.Count);
            //also check if the special characters symbol does not appear in the Watchlist window
            Assert.IsNull(watchList.Rows.Find(r => r.Cells[0].Text == addWatchTextBox.Text));
        }
        
        /// <summary>
        /// Remove an item from the WatchList ListView using the Remove Contextmenu item
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTrader RI
        /// 2. Add a symbol to the WatchList
        /// 3. Pin the WatchList Panel
        /// 4. Invoke Remove ContextMenu from the added symbol in the WatchList Panel
        /// 
        /// Expected Result;
        /// Check if the selected symbol is removed from the WatchList Panel
        /// </summary>
        [TestMethod]
        public void RemoveSymbolFromWatchList()
        {
            TextBox addWatchTextBox = Window.Get<TextBox>(TestDataInfrastructure.GetControlId("WatchListAddTextBox"));
            addWatchTextBox.Text = TestDataInfrastructure.GetTestInputData("WatchListSymbol");

            Button addWatchListButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("WatchListAddButton"));
            addWatchListButton.Click();

            ListView watchList = Window.Get<ListView>(TestDataInfrastructure.GetControlId("WatchListView"));
            int watchListCountBeforeRemoveClick = watchList.Rows.Count;

            //right click on the added symbol in the WatchList panel and click remove
            watchList.Rows[0].RightClick();
            System.Threading.Thread.Sleep(1000);
            Window.PopupMenu("Remove").Click();
            System.Threading.Thread.Sleep(1000);


            Assert.AreEqual(watchListCountBeforeRemoveClick - 1, watchList.Rows.Count);
        }

        #region Private Helper methods
        //TODO: Need to remove when [Ignore] tests are cleaned up
        private static bool IsWatchListWindowCollapsed(UIItem item)
        {
            return !item.Visible;
        }

        #endregion
    }
}
