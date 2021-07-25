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
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Wpf.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.Position.PositionSummary;
using StockTraderRI.Modules.Position.Tests.Mocks;


namespace StockTraderRI.Modules.Position.Tests.PositionSummary
{
    [TestClass]
    public class PositionSummaryPresentationModelFixture
    {
        MockPositionSummaryView view;
        MockAccountPositionService accountPositionService;
        MockMarketFeedService marketFeedService;
        MockMarketHistoryService marketHistoryService;
        MockTrendLinePresenter trendLinePresenter;
        MockOrdersController ordersController;
        MockEventAggregator eventAggregator;

        [TestInitialize]
        public void SetUp()
        {
            view = new MockPositionSummaryView();
            accountPositionService = new MockAccountPositionService();
            marketFeedService = new MockMarketFeedService();
            marketHistoryService = new MockMarketHistoryService();
            trendLinePresenter = new MockTrendLinePresenter();
            ordersController = new MockOrdersController();
            this.eventAggregator = new MockEventAggregator();
        }

        [TestMethod]
        public void CanInitPresentationModel()
        {
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());

            PositionSummaryPresentationModel presentationModel = CreatePresentationModel();

            Assert.AreEqual(view, presentationModel.View);

        }

        [TestMethod]
        public void PresenterGeneratesModelFromPositionModelMarketAndNewsFeeds()
        {
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());

            accountPositionService.AddPosition(new AccountPosition("FUND0", 300m, 1000));
            accountPositionService.AddPosition(new AccountPosition("FUND1", 200m, 100));
            marketFeedService.SetPrice("FUND0", 30.00m);
            marketFeedService.SetPrice("FUND1", 20.00m);

            var presentationModel = CreatePresentationModel();

            Assert.AreEqual<decimal>(30.00m, presentationModel.PositionSummaryItems.First(x => x.TickerSymbol == "FUND0").CurrentPrice);
            Assert.AreEqual<long>(1000, presentationModel.PositionSummaryItems.First(x => x.TickerSymbol == "FUND0").Shares);
            Assert.AreEqual<decimal>(20.00m, presentationModel.PositionSummaryItems.First(x => x.TickerSymbol == "FUND1").CurrentPrice);
            Assert.AreEqual<long>(100, presentationModel.PositionSummaryItems.First(x => x.TickerSymbol == "FUND1").Shares);

        }

        [TestMethod]
        public void CanSetPresentationModelIntoView()
        {
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());

            var presentationModel = CreatePresentationModel();

            Assert.AreSame(presentationModel, view.Model);
        }


        [TestMethod]
        public void PresenterUpdatesDataWithMarketUpdates()
        {
            var marketPricesUpdatedEvent = new MockMarketPricesUpdatedEvent();
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(marketPricesUpdatedEvent);

            marketFeedService.SetPrice("FUND0", 30.00m);
            accountPositionService.AddPosition("FUND0", 25.00m, 1000);
            marketFeedService.SetPrice("FUND1", 20.00m);
            accountPositionService.AddPosition("FUND1", 15.00m, 100);
            var presentationModel = CreatePresentationModel();

            var updatedPriceList = new Dictionary<string, decimal> { { "FUND0", 50.00m } };

            Assert.IsNotNull(marketPricesUpdatedEvent.SubscribeArgumentAction);
            Assert.AreEqual(ThreadOption.UIThread, marketPricesUpdatedEvent.SubscribeArgumentThreadOption);


            marketPricesUpdatedEvent.SubscribeArgumentAction(updatedPriceList);

            Assert.AreEqual<decimal>(50.00m, presentationModel.PositionSummaryItems.First(x => x.TickerSymbol == "FUND0").CurrentPrice);
        }

        [TestMethod]
        public void MarketUpdatesPresenterPositionUpdatesButCollectionDoesNot()
        {
            var marketPricesUpdatedEvent = new MockMarketPricesUpdatedEvent();
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(marketPricesUpdatedEvent);

            marketFeedService.SetPrice("FUND1", 20.00m);
            accountPositionService.AddPosition("FUND1", 15.00m, 100);

            var presentationModel = CreatePresentationModel();

            bool presentationModelCollectionUpdated = false;
            presentationModel.PositionSummaryItems.CollectionChanged += delegate
              {
                  presentationModelCollectionUpdated = true;
              };

            bool presentationModelItemUpdated = false;
            presentationModel.PositionSummaryItems.First(p => p.TickerSymbol == "FUND1").PropertyChanged += delegate
               {
                   presentationModelItemUpdated = true;
               };

            marketPricesUpdatedEvent.SubscribeArgumentAction(new Dictionary<string, decimal> { { "FUND1", 50m } });

            Assert.IsFalse(presentationModelCollectionUpdated);
            Assert.IsTrue(presentationModelItemUpdated);
        }

        [TestMethod]
        public void PresenterPopulatesSummaryCollectionWithMarketHistory()
        {
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());
            marketFeedService.SetPrice("FUND1", 20.00m);
            accountPositionService.AddPosition("FUND1", 15.00m, 100);
            var presentationModel = CreatePresentationModel();

            Assert.AreEqual(2, presentationModel.PositionSummaryItems.First(p => p.TickerSymbol == "FUND1").PriceMarketHistory.Count);
        }

        [TestMethod]
        public void AccountPositionModificationUpdatesPM()
        {
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());
            marketFeedService.SetPrice("FUND0", 20.00m);
            accountPositionService.AddPosition("FUND0", 150.00m, 100);
            var presentationModel = CreatePresentationModel();

            bool presentationModelItemUpdated = false;
            presentationModel.PositionSummaryItems.First(p => p.TickerSymbol == "FUND0").PropertyChanged += delegate
               {
                   presentationModelItemUpdated = true;
               };

            AccountPosition accountPosition = accountPositionService.GetAccountPositions().First<AccountPosition>(p => p.TickerSymbol == "FUND0");
            accountPosition.Shares += 11;
            accountPosition.CostBasis = 25.00m;

            Assert.IsTrue(presentationModelItemUpdated);
            Assert.AreEqual(111, presentationModel.PositionSummaryItems.First(p => p.TickerSymbol == "FUND0").Shares);
            Assert.AreEqual(25.00m, presentationModel.PositionSummaryItems.First(p => p.TickerSymbol == "FUND0").CostBasis);
        }

        [TestMethod]
        public void WhenPositionRowSelectedSymbolsTrendDataShowsInLineChart()
        {
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());
            eventAggregator.AddMapping<TickerSymbolSelectedEvent>(new MockTickerSymbolSelectedEvent());
            var presentationModel = CreatePresentationModel();

            view.SelectFUND0Row();

            Assert.IsTrue(trendLinePresenter.TickerSymbolSelected);
            Assert.AreEqual("FUND0", trendLinePresenter.SelectedTickerSymbol);
        }

        [TestMethod]
        public void TickerSymbolSelectedPublishesEvent()
        {
            var tickerSymbolSelectedEvent = new MockTickerSymbolSelectedEvent();
            eventAggregator.AddMapping<TickerSymbolSelectedEvent>(tickerSymbolSelectedEvent);
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());
            var presentationModel = CreatePresentationModel();

            view.SelectFUND0Row();

            Assert.IsTrue(tickerSymbolSelectedEvent.PublishCalled);
            Assert.AreEqual("FUND0", tickerSymbolSelectedEvent.PublishArgumentPayload);
        }

        [TestMethod]
        public void ControllerCommandsSetIntoPresentationModel()
        {
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());
            var presentationModel = CreatePresentationModel();

            Assert.AreSame(presentationModel.BuyCommand, ordersController.BuyCommand);
            Assert.AreSame(presentationModel.SellCommand, ordersController.SellCommand);
        }

        private PositionSummaryPresentationModel CreatePresentationModel()
        {
            return new PositionSummaryPresentationModel(view, accountPositionService
                                                , marketFeedService
                                                , marketHistoryService
                                                , trendLinePresenter
                                                , ordersController
                                                , eventAggregator);
        }


    }

    internal class MockTickerSymbolSelectedEvent : TickerSymbolSelectedEvent
    {
        public bool PublishCalled;
        public string PublishArgumentPayload;


        public override void Publish(string payload)
        {
            PublishCalled = true;
            PublishArgumentPayload = payload;
        }
    }

    internal class MockMarketPricesUpdatedEvent : MarketPricesUpdatedEvent
    {
        public Action<IDictionary<string, decimal>> SubscribeArgumentAction;
        public Predicate<IDictionary<string, decimal>> SubscribeArgumentFilter;
        public ThreadOption SubscribeArgumentThreadOption;

        public override SubscriptionToken Subscribe(Action<IDictionary<string, decimal>> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<IDictionary<string, decimal>> filter)
        {
            SubscribeArgumentAction = action;
            SubscribeArgumentFilter = filter;
            SubscribeArgumentThreadOption = threadOption;
            return null;
        }
    }
}

/*
 * updates view when position added/removed
 * 
 * presentationModel does NOT update view when market feed does not relate to positions (filtering)
 * 
 * market update with volume change should be reflected in presentation model
 */
