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
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Market.Services;
using StockTraderRI.Modules.Market.TrendLine;

namespace StockTraderRI.Modules.Market
{
    public class MarketModule : IModule
    {
        IUnityContainer _container;

        public MarketModule(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();
        }

        protected void RegisterViewsAndServices()
        {
            _container.RegisterType<IMarketHistoryService, MarketHistoryService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMarketFeedService, MarketFeedService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ITrendLineView, TrendLineView>();
            _container.RegisterType<ITrendLinePresenter, TrendLinePresenter>();
        }

        #endregion
    }
}
