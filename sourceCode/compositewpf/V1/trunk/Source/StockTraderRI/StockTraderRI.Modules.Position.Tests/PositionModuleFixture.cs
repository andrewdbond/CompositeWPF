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

using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Position.Controllers;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.PositionSummary;
using StockTraderRI.Modules.Position.Services;
using StockTraderRI.Modules.Position.Tests.Mocks;

namespace StockTraderRI.Modules.Position.Tests
{
    [TestClass]
    public class PositionModuleFixture
    {
        [TestMethod]
        [DeploymentItem("Data", "Data")]
        public void RegisterViewsServicesRegistersViewsAndServices()
        {
            MockUnityContainer container = new MockUnityContainer();

            TestablePositionModule module = new TestablePositionModule(container, new MockRegionManager());

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(typeof(AccountPositionService), container.Types[typeof(IAccountPositionService)]);
            Assert.AreEqual(typeof(PositionSummaryView), container.Types[typeof(IPositionSummaryView)]);
            Assert.AreEqual(typeof(PositionSummaryPresentationModel), container.Types[typeof(IPositionSummaryPresentationModel)]);
            Assert.AreEqual(typeof(OrdersView), container.Types[typeof(IOrdersView)]);
            Assert.AreEqual(typeof(OrdersPresentationModel), container.Types[typeof(IOrdersPresentationModel)]);
            Assert.AreEqual(typeof(OrderDetailsView), container.Types[typeof(IOrderDetailsView)]);
            Assert.AreEqual(typeof(OrderDetailsPresentationModel), container.Types[typeof(IOrderDetailsPresentationModel)]);
            Assert.AreEqual(typeof(OrderCommandsView), container.Types[typeof(IOrderCommandsView)]);
            Assert.AreEqual(typeof(OrderCompositeView), container.Types[typeof(IOrderCompositeView)]);
            Assert.AreEqual(typeof(OrderCompositePresentationModel), container.Types[typeof(IOrderCompositePresentationModel)]);
            Assert.AreEqual(typeof(OrdersController), container.Types[typeof(IOrdersController)]);
            Assert.AreEqual(typeof(XmlOrdersService), container.Types[typeof(IOrdersService)]);

        }

        [TestMethod]
        public void InitAddsOrdersToolbarViewToToolbarRegion()
        {
            MockRegion toolbarRegion = new MockRegion();
            MockRegion mainRegion = new MockRegion();
            MockRegionManager regionManager = new MockRegionManager();
            var container = new MockUnityResolver();
            container.Bag.Add(typeof(IOrdersController), new MockOrdersController());
            container.Bag.Add(typeof(IPositionSummaryPresentationModel), new MockPositionSummaryPresenter());
            PositionModule module = new PositionModule(container, regionManager);
            regionManager.Regions.Add("MainRegion", mainRegion);
            regionManager.Regions.Add("CollapsibleRegion", new MockRegion());
            regionManager.Regions.Add("MainToolbarRegion", toolbarRegion);

            Assert.AreEqual(0, toolbarRegion.AddedViews.Count);
            Assert.AreEqual(0, mainRegion.AddedViews.Count);

            module.Initialize();

            Assert.AreEqual(1, mainRegion.AddedViews.Count);
            Assert.AreEqual(1, toolbarRegion.AddedViews.Count);

        }


        internal class TestablePositionModule : PositionModule
        {
            public TestablePositionModule(IUnityContainer container, IRegionManager regionManager)
                : base(container, regionManager)
            {
            }

            public void InvokeRegisterViewsAndServices()
            {
                base.RegisterViewsAndServices();
            }
        }
    }

    internal class MockPositionSummaryPresenter : IPositionSummaryPresentationModel
    {
        #region IPositionSummaryPresenter Members

        private IPositionSummaryView _view = new MockPositionSummaryView();

        public IPositionSummaryView View
        {
            get { return _view; }
            set { _view = value; }
        }


        public System.Collections.ObjectModel.ObservableCollection<PositionSummaryItem> PositionSummaryItems
        {
            get { throw new System.NotImplementedException(); }
        }


        public Microsoft.Practices.Composite.Wpf.Commands.DelegateCommand<string> BuyCommand
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public Microsoft.Practices.Composite.Wpf.Commands.DelegateCommand<string> SellCommand
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        #endregion

        #region IPositionSummaryPresentationModel Members


        public string HeaderInfo
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}
