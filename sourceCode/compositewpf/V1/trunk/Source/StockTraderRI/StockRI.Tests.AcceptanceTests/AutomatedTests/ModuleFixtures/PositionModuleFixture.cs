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
using Core.UIItems;
using StockTraderRI.AcceptanceTests.TestInfrastructure.MockModels;
using Core.UIItems.ListViewItems;
using Core.UIItems.Finders;
using StockTraderRI.AcceptanceTests.TestInfrastructure;
using StockTraderRI.AcceptanceTests.Helpers;
using System.Globalization;

namespace StockTraderRI.AcceptanceTests.AutomatedTests
{
    [TestClass]
    [DeploymentItem(@".\StockTraderRI\bin\Debug")]
    [DeploymentItem(@".\StockRI.Tests.AcceptanceTests\bin\Debug")]
    public class PositionModuleFixture : FixtureBase
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
        /// The current account position view should have symbol details with 6 columns 
        /// (Symbol, Shares, Last, Cost Basis, Market Value, Gain Loss %) 
        /// 
        /// Repro Steps:
        /// 1. Launch the Stock Trader application
        /// 2. Check for the following columns: 
        /// (Symbol, Shares, Last, Cost Basis, Market Value, Gain Loss %) 
        /// 
        /// Expected Result:
        /// Account Position Table should have 6 columns and the column names should be as follows:
        /// Symbol, Shares, Last, Cost Basis, Market Value, Gain Loss %
        /// </summary>
        [TestMethod]
        public void AccountPositionTableColumns()
        {
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            ListViewHeader listHeader = list.Header;

            Assert.AreEqual(6, listHeader.Columns.Count);

            //The Columns in the Position Table are partly coming from two different XML files and 
            //the rest are computed columns. the only place where the column names are defined is the 
            //PositionView XAML file, hence the hard-coding.

            Assert.AreEqual(TestDataInfrastructure.GetTestInputData("PositionTableSymbol"), listHeader.Columns[0].Name);
            Assert.AreEqual(TestDataInfrastructure.GetTestInputData("PositionTableShares"), listHeader.Columns[1].Name);
            Assert.AreEqual(TestDataInfrastructure.GetTestInputData("PositionTableLast"), listHeader.Columns[2].Name);
            Assert.AreEqual(TestDataInfrastructure.GetTestInputData("PositionTableCost"), listHeader.Columns[3].Name);
            Assert.AreEqual(TestDataInfrastructure.GetTestInputData("PositionTableMarketValue"), listHeader.Columns[4].Name);
            Assert.AreEqual(TestDataInfrastructure.GetTestInputData("PositionTableGainLoss"), listHeader.Columns[5].Name);
            
        }

        /// <summary>
        /// The current account position view should display details of symbols from the AccountPosition.xml
        /// 
        /// Repro Steps:
        /// 1. Launch the Stock Trader application
        /// 2. Check the number of symbols displayed in the account position table.
        /// 
        /// Expected Result:
        /// As many rows as present in AccountPosition.xml is displayed.
        /// </summary>
        [TestMethod]
        public void AccountPositionTableRowCount()
        {
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            //read number of account positions from the AccountPosition.xml data file
            int positionRowCount = TestDataInfrastructure.GetCount<AccountPositionDataProvider, AccountPosition>();

            Assert.AreEqual(positionRowCount, list.Rows.Count);
        }

        /// <summary>
        /// The current account position view should display derived data of symbols 
        /// (after processing data from AccountPosition.xml and Market.xml)
        /// 
        /// Repro Steps:
        /// 1. Launch the Stock Trader application
        /// 2. Check the derived data (like Market Value and Gain Loss %) of symbols displayed in the account position table.
        /// 
        /// Expected Result:
        /// Market Value = shares * lastPrice
        /// Gain Loss % = (lastPrice * Shares - CostBasis) * 100 / CostBasis
        /// </summary>
        [TestMethod]
        public void AccountPositionDerivedData()
        {
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            ListViewRow listRow = null;
            string symbol;
           
            int count = 0;
            int numberOfRetries = 3;

            string share;
            string lastPrice;
            string marketPrice;
            string gainLoss;
            string costBasis;

            bool isMarketPriceMatching = false;
            bool isGainLossMatching = false;

            //test driven by the number of rows displayed in the Account Position table in the UI
            for (int i = 0; i < list.Rows.Count; i++)
            {
                listRow = list.Rows[i];
                symbol = listRow.Cells[TestDataInfrastructure.GetTestInputData("PositionTableSymbol")].Text;
               
                while (count < numberOfRetries)
                {
                    //input columns
                    share = listRow.Cells[TestDataInfrastructure.GetTestInputData("PositionTableShares")].Text;
                    lastPrice = listRow.Cells[TestDataInfrastructure.GetTestInputData("PositionTableLast")].Text;
                    costBasis = listRow.Cells[TestDataInfrastructure.GetTestInputData("PositionTableCost")].Text;
                    
                    //computed columns
                    marketPrice = listRow.Cells[TestDataInfrastructure.GetTestInputData("PositionTableMarketValue")].Text;
                    gainLoss = listRow.Cells[TestDataInfrastructure.GetTestInputData("PositionTableGainLoss")].Text;

                    if (!isMarketPriceMatching)
                    {
                        if (Convert.ToDouble(marketPrice, CultureInfo.CurrentCulture).ToString("0.00", CultureInfo.CurrentCulture)
                            .Equals((Convert.ToDouble(share, CultureInfo.CurrentCulture) * Convert.ToDouble(lastPrice, CultureInfo.CurrentCulture)).ToString("0.00", CultureInfo.CurrentCulture)))
                        {
                            isMarketPriceMatching = true;
                        }
                    }

                    if (!isGainLossMatching)
                    {
                        if (Convert.ToDouble(gainLoss, CultureInfo.CurrentCulture).ToString("0.00", CultureInfo.CurrentCulture)
                            .Equals(Math.Round((Convert.ToDouble(lastPrice, CultureInfo.CurrentCulture) * Convert.ToDouble(share, CultureInfo.CurrentCulture) - Convert.ToDouble(costBasis, CultureInfo.CurrentCulture)) / Convert.ToDouble(costBasis, CultureInfo.CurrentCulture) * 100, 2).ToString("0.00", CultureInfo.CurrentCulture)))
                        {
                            isGainLossMatching = true;
                        }
                    }

                    if (isMarketPriceMatching && isGainLossMatching)
                    {
                        break;
                    }

                    count++;
                }

                //if either the Market Price or the Gain-Loss value does not match, then the test case fails
                Assert.IsTrue(isMarketPriceMatching && isGainLossMatching, String.Format(CultureInfo.CurrentCulture, "Computed Value for {0} is not correct", symbol));
            }
        }
    }
}
