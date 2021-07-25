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
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Wpf.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.News.Article;
using StockTraderRI.Modules.News.Controllers;
using StockTraderRI.Modules.News.Tests.Mocks;

namespace StockTraderRI.Modules.News.Tests.Controllers
{
    [TestClass]
    public class NewsControllerFixture
    {
        [TestMethod]
        public void ShowNewsResolvesPresenterAndCallsSetTickerSymbolOnItAndAddsNamedViewToRegion()
        {
            var regionManager = new MockRegionManager();
            var presenter = new MockArticlePresentationModel();
            var eventAggregator = new MockEventAggregator();
            eventAggregator.AddMapping<TickerSymbolSelectedEvent>(new MockTickerSymbolSelectedEvent());
            var controller = new NewsController(regionManager, presenter, eventAggregator);

            controller.ShowNews("Test");

            Assert.IsNotNull(presenter.SetTickerSymbolArgumentCompanySymbol);
            Assert.AreEqual("Test", presenter.SetTickerSymbolArgumentCompanySymbol);
        }

        [TestMethod]
        public void ControllerShowNewsWhenRasingGlobalEvent()
        {
            var presenter = new MockArticlePresentationModel();
            var eventAggregator = new MockEventAggregator();
            var regionManager = new MockRegionManager();
            regionManager.Regions.Add("NewsRegion", new MockNewsRegion());
            var tickerSymbolSelectedEvent = new MockTickerSymbolSelectedEvent();
            eventAggregator.AddMapping<TickerSymbolSelectedEvent>(tickerSymbolSelectedEvent);
            var controller = new NewsController(regionManager, presenter, eventAggregator);

            controller.Run();

            Assert.IsNotNull(tickerSymbolSelectedEvent.SubscribeArgumentAction);

            tickerSymbolSelectedEvent.SubscribeArgumentAction("TEST_SYMBOL");
            Assert.AreEqual("TEST_SYMBOL", presenter.SetTickerSymbolArgumentCompanySymbol);
        }

        [TestMethod]
        public void ShouldNotifyReaderWhenCurrentNewsArticleChanges()
        {
            var presenter = new MockArticlePresentationModel();
            var eventAggregator = new MockEventAggregator();
            eventAggregator.AddMapping<TickerSymbolSelectedEvent>(new MockTickerSymbolSelectedEvent());
            var newsReaderPresenter = new MockNewsReaderPresenter();
            var regionManager = new MockRegionManager();
            regionManager.Regions.Add("NewsRegion", new MockNewsRegion());
            var controller = new NewsController(regionManager, presenter, eventAggregator, newsReaderPresenter);

            controller.CurrentNewsArticleChanged(new NewsArticle() { Title = "SomeTitle", Body = "Newsbody" });

            Assert.IsTrue(newsReaderPresenter.SetNewsArticleCalled);
        }

        [TestMethod]
        public void ControllerShowNewsViewWhenArticlePresenterReceivesEvent()
        {
            var presenter = new MockArticlePresentationModel();
            var eventAggregator = new MockEventAggregator();
            eventAggregator.AddMapping<TickerSymbolSelectedEvent>(new MockTickerSymbolSelectedEvent());
            var newsReaderPresenter = new MockNewsReaderPresenter();

            var controller = new NewsController(new MockRegionManager(), presenter, eventAggregator, newsReaderPresenter);

            controller.ShowNewsReader();

            Assert.IsTrue(newsReaderPresenter.ShowWasCalled);
        }

        class MockArticlePresentationModel : IArticlePresentationModel
        {
            public MockArticleView MockArticleView = new MockArticleView();
            public string SetTickerSymbolArgumentCompanySymbol;
            public void SetTickerSymbol(string companySymbol)
            {
                SetTickerSymbolArgumentCompanySymbol = companySymbol;
            }

            public IArticleView View
            {
                get { return MockArticleView; }
            }

            public INewsController Controller { get; set; }

        }

        internal class MockTickerSymbolSelectedEvent : TickerSymbolSelectedEvent
        {
            public Action<string> SubscribeArgumentAction;
            public Predicate<string> SubscribeArgumentFilter;
            public override SubscriptionToken Subscribe(Action<string> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<string> filter)
            {
                SubscribeArgumentAction = action;
                SubscribeArgumentFilter = filter;
                return null;
            }
        }
    }

    internal class MockNewsReaderPresenter : INewsReaderPresenter
    {
        public bool SetNewsArticleCalled { get; set; }

        public bool ShowWasCalled { get; private set; }

        public void SetNewsArticle(NewsArticle article)
        {
            SetNewsArticleCalled = true;
        }

        public void Show()
        {
            ShowWasCalled = true;
        }
    }


}
