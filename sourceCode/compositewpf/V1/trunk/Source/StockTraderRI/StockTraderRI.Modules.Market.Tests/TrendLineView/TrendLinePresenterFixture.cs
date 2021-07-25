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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Market.Tests.Mocks;
using StockTraderRI.Modules.Market.TrendLine;

namespace StockTraderRI.HistoricalModule.Tests
{
    /// <summary>
    /// Unit tests for TrendLinePresenter
    /// </summary>
    [TestClass]
    public class TrendLinePresenterFixture
    {
        [TestMethod]
        public void CanInitPresenter()
        {
            ITrendLineView view = new MockTrendLineView();
            IMarketHistoryService historyService = new MockMarketHistoryService();
            TrendLinePresenter presenter = new TrendLinePresenter(view, historyService);

            Assert.IsNotNull(presenter);
        }

        [TestMethod]
        public void OnTickerSymbolSelectedUpdatesChartTitle()
        {
            MockTrendLineView view = new MockTrendLineView();
            MockMarketHistoryService historyService = new MockMarketHistoryService();
            TrendLinePresenter presenter = new TrendLinePresenter(view, historyService);

            presenter.OnTickerSymbolSelected("TEST");

            Assert.AreEqual("TEST", view.ChartTitlePassed);
        }

        [TestMethod]
        public void OnTickerSymbolSelectedUpdatesLineChartWithPriceHistoryData()
        {
            MockTrendLineView view = new MockTrendLineView();
            MockMarketHistoryService historyService = new MockMarketHistoryService();
            TrendLinePresenter presenter = new TrendLinePresenter(view, historyService);

            presenter.OnTickerSymbolSelected("TEST");

            Assert.IsNotNull(view.HistoryCollectionPassed);
            Assert.AreEqual(historyService.Data.Count, view.HistoryCollectionPassed.Count);
            Assert.AreEqual(historyService.Data[0], view.HistoryCollectionPassed[0]);
        }
    }
}
