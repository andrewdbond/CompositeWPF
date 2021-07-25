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
using System.Windows.Automation;
using Core;
using Core.UIItems.Finders;
using Core.UIItems;
using Core.UIItems.TabItems;
using Core.UIItems.WindowItems;
using StockTraderRI.AcceptanceTests.TestInfrastructure;
using StockTraderRI.AcceptanceTests.AutomatedTests;
using StockTraderRI.AcceptanceTests.Helpers;
using StockTraderRI.AcceptanceTests.TestInfrastructure.MockModels;

namespace StockTraderRI.AcceptanceTests.Test.AutomatedTests.ModuleFixtures
{
    /// <summary>
    /// Summary description for HistoryModuleFixture
    /// </summary>
    [TestClass]
    [DeploymentItem(@".\StockTraderRI\bin\Debug")]
    [DeploymentItem(@".\StockRI.Tests.AcceptanceTests\bin\Debug")]    
    public class MarketModuleFixture : FixtureBase
    {
        public MarketModuleFixture()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region TestInfrastructre

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

        #endregion 

        // BVT - Validate that the default data comes up in the history trend view 
        [Ignore]
        [TestMethod]
        public void ValidateDefaultCompositeTrendView()
        {

            // Data Assumptions : 
            // Trend data composite is calculated based on all the data - hence they all need the same time span and data sampling
            // We should have some BVT test that validates the test and Sample data ARE OK with our assumption
            // - Validate our Data Set!

            // Get the PositionTable 
            // Get ahold of the following trendlineView data
            //    - Get the Title, Validate the default it should be "FAKEINDEX"
            //    - Caluculate the composite data set
            //    - Get the Min Max on the Y axis
            //    - Validate that the test calculated data is bounded by the Y Data range displayed in the view
            //    - Validate the data range and Get the Min Max date value in our Test time span match the displayed time span
            //    

            // Loop the the rows
            //  - Select the first row 
            //    need to force an invoke
            //  

            Assert.IsNotNull(Window.AutomationElement.SearchInRawTreeByName(TestDataInfrastructure.GetTestInputData("DefaultCompositeTrendView")));
            
            List<MarketHistoryItem> history = TestDataInfrastructure.GetData<MarketHistoryDataProvider, MarketHistoryItem>();
            List<MarketHistoryItem> defaultItems = history.FindAll(i => i.TickerSymbol.Equals(TestDataInfrastructure.GetTestInputData("DefaultCompositeTrendView")));
            
            Assert.IsTrue(ValidateXAxisDataRange(defaultItems));
            Assert.IsTrue(ValidateYAxisDataRange(defaultItems));
        }

        // BVT - Select each data row - validate that the correct legends come up
        [Ignore]
        [TestMethod]
        public void ValidateMarketTrendDataPerSelectedPositionRow()
        {

            // Data Assumptions : 
            // Trend data composite is calculated based on all the data - hence they all need the same time span and data sampling
            // We should have some BVT test that validates the test and Sample data ARE OK with our assumption
            // - Validate our Data Set!

            // Get the PositionTable 
            // Will loop through the PositionTable and select each row and validate that Market Trend data is updated
            // Loop
            //    - Get ahold of the  trendlineView data for that row
            //    - Get the Title, Validate the default it should be "XXX" : the stock symbol what ever it is
            //    - Get the data set
            //    - Get the Min Max on the Y axis
            //    - Validate that the test data setis bounded by the Y Data range displayed in the view
            //    - Validate the data range and Get the Min Max date value in our Test time span match the displayed time span
            //    

            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            string selectedSymbol;
            List<MarketHistoryItem> history = TestDataInfrastructure.GetData<MarketHistoryDataProvider, MarketHistoryItem>();

            foreach (ListViewRow row in list.Rows)
            {
                row.Click();
                
                //give time for the graph to be redrawn for the selected symbol
                System.Threading.Thread.Sleep(2000);

                selectedSymbol = row.Cells[0].Text;

                Assert.IsNotNull(Window.AutomationElement.SearchInRawTreeByName(selectedSymbol));

                List<MarketHistoryItem> defaultItems = history.FindAll(i => i.TickerSymbol.Equals(selectedSymbol));

                Assert.IsTrue(ValidateXAxisDataRange(defaultItems));
                Assert.IsTrue(ValidateYAxisDataRange(defaultItems));
            }
        }


        // Boundary Tests :
        //  Validate Data 
        //  Negative selection?  Do these make sense
        // - Stock doesn't exist
        // - Market Service is dead
        //

        #region Private Helper Methods

        /// <summary>
        /// Validate if the Y - Axis range matches with the data from the MarketHistory.xml file
        /// </summary>
        /// <param name="defaultItems"></param>
        private static bool ValidateYAxisDataRange(List<MarketHistoryItem> defaultItems)
        {
            defaultItems.Sort((a, b) => a.MarketItem.CompareTo(b.MarketItem));

            //TODO: validate bounds
            return true;
        }

        /// <summary>
        /// Validate if the X - Axis range matches with the data from the MarketHistory.xml file
        /// </summary>
        /// <param name="defaultItems"></param>
        /// <returns></returns>
        private bool ValidateXAxisDataRange(List<MarketHistoryItem> defaultItems)
        {
            defaultItems.Sort((a, b) => a.Date.CompareTo(b.Date));
            //TODO: find a better mechanism to check the X-Axis data
            return defaultItems.FindAll(i => Window.AutomationElement.SearchInRawTreeByName(i.Date.ToString()) != null).Count.Equals(defaultItems.Count);

            //TODO: also verify the lower and upper bound of the X-Axis - but how?
        }

        #endregion
    }
}
