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
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Position.Controllers;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.PositionSummary;
using StockTraderRI.Modules.Position.Services;

namespace StockTraderRI.Modules.Position
{
    public class PositionModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public PositionModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();

            IPositionSummaryPresentationModel presentationModel = _container.Resolve<IPositionSummaryPresentationModel>();
            IRegion mainRegion = _regionManager.Regions[RegionNames.MainRegion];
            mainRegion.Add(presentationModel.View);

            IRegion mainToolbarRegion = _regionManager.Regions[RegionNames.MainToolbarRegion];
            mainToolbarRegion.Add(new OrdersToolBar());
        }

        protected void RegisterViewsAndServices()
        {
            _container.RegisterType<IAccountPositionService, AccountPositionService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPositionSummaryView, PositionSummaryView>();
            _container.RegisterType<IPositionSummaryPresentationModel, PositionSummaryPresentationModel>();
            _container.RegisterType<IOrdersView, OrdersView>();
            _container.RegisterType<IOrdersPresentationModel, OrdersPresentationModel>();
            _container.RegisterType<IOrderDetailsView, OrderDetailsView>();
            _container.RegisterType<IOrderDetailsPresentationModel, OrderDetailsPresentationModel>();
            _container.RegisterType<IOrderCommandsView, OrderCommandsView>();
            _container.RegisterType<IOrderCompositeView, OrderCompositeView>();
            _container.RegisterType<IOrderCompositePresentationModel, OrderCompositePresentationModel>();
            _container.RegisterType<IOrdersController, OrdersController>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrdersService, XmlOrdersService>(new ContainerControlledLifetimeManager());

        }

        #endregion
    }
}
