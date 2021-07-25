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

using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Modules.Watch;
using StockTraderRI.Modules.Watch.AddWatch;
using StockTraderRI.Modules.Watch.Services;
using StockTraderRI.Modules.Watch.WatchList;
using StockTraderRI.Modules.WatchList.Tests.Mocks;

namespace StockTraderRI.Modules.WatchList.Tests
{
    [TestClass]
    public class WatchModuleFixture
    {
        [TestMethod]
        public void RegisterViewsServicesRegistersWatchListView()
        {
            var container = new MockUnityContainer();

            var module = new TestableWatchModule(container, new MockRegionManager());

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(typeof(WatchListService), container.Types[typeof(IWatchListService)]);
            Assert.AreEqual(typeof(WatchListView), container.Types[typeof(IWatchListView)]);
            Assert.AreEqual(typeof(WatchListPresentationModel), container.Types[typeof(IWatchListPresentationModel)]);
            Assert.AreEqual(typeof(AddWatchView), container.Types[typeof(IAddWatchView)]);
            Assert.AreEqual(typeof(AddWatchPresenter), container.Types[typeof(IAddWatchPresenter)]);
        }

        [TestMethod]
        public void InitAddsAddWatchControlViewToToolbarRegion()
        {
            var toolbarRegion = new MockRegion();
            var collapsibleRegion = new MockRegion();
            var regionManager = new MockRegionManager();
            var container = new MockUnityResolver();
            container.Bag.Add(typeof(IAddWatchPresenter), new MockAddWatchPresenter());
            container.Bag.Add(typeof(IWatchListPresentationModel), new MockWatchListPresentationModel());
            IModule module = new WatchModule(container, regionManager);
            regionManager.Regions.Add("WatchRegion", collapsibleRegion);
            regionManager.Regions.Add("MainToolbarRegion", toolbarRegion);

            Assert.AreEqual(0, toolbarRegion.AddedViews.Count);
            Assert.AreEqual(0, collapsibleRegion.AddedViews.Count);

            module.Initialize();

            Assert.AreEqual(1, toolbarRegion.AddedViews.Count);
            Assert.AreEqual(1, collapsibleRegion.AddedViews.Count);
        }

        internal class TestableWatchModule : WatchModule
        {
            public TestableWatchModule(IUnityContainer container, IRegionManager regionManager)
                : base(container, regionManager)
            {
            }

            public void InvokeRegisterViewsAndServices()
            {
                base.RegisterViewsAndServices();
            }
        }

        class MockAddWatchPresenter : IAddWatchPresenter
        {
            private IAddWatchView _view = new MockAddWatchView();
            public IAddWatchView View
            {
                get { return _view; }
            }
        }

        class MockWatchListPresentationModel : IWatchListPresentationModel
        {
            private IWatchListView _view = new MockWatchListView();

            public IWatchListView View
            {
                get { return _view; }
            }
        }

    }
}
