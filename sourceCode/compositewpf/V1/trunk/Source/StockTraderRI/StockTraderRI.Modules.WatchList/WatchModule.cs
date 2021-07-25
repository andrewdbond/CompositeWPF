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
using StockTraderRI.Modules.Watch.AddWatch;
using StockTraderRI.Modules.Watch.Services;
using StockTraderRI.Modules.Watch.WatchList;

namespace StockTraderRI.Modules.Watch
{
    public class WatchModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public WatchModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();

            IWatchListPresentationModel watchListPresentationModel = _container.Resolve<IWatchListPresentationModel>();
            _regionManager.Regions[RegionNames.WatchRegion].Add(watchListPresentationModel.View);
            IAddWatchPresenter addWatchPresenter = _container.Resolve<IAddWatchPresenter>();
            _regionManager.Regions[RegionNames.MainToolbarRegion].Add(addWatchPresenter.View);
        }

        protected void RegisterViewsAndServices()
        {
            _container.RegisterType<IWatchListService, WatchListService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IWatchListView, WatchListView>();
            _container.RegisterType<IWatchListPresentationModel, WatchListPresentationModel>();
            _container.RegisterType<IAddWatchView, AddWatchView>();
            _container.RegisterType<IAddWatchPresenter, AddWatchPresenter>();
        }

        #endregion
    }
}
