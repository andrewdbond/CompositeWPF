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
using Microsoft.Practices.Unity;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.News.Article;
using StockTraderRI.Modules.News.Controllers;
using StockTraderRI.Modules.News.Services;

namespace StockTraderRI.Modules.News
{
    public class NewsModule : IModule
    {
        private IUnityContainer _container;

        public NewsModule(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();
            INewsController controller = _container.Resolve<INewsController>();
            controller.Run();

        }
        #endregion

        protected void RegisterViewsAndServices()
        {
            _container.RegisterType<INewsFeedService, NewsFeedService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<INewsController, NewsController>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IArticleView, ArticleView>();
            _container.RegisterType<IArticlePresentationModel, ArticlePresentationModel>();
            _container.RegisterType<INewsReaderView, NewsReader>();
            _container.RegisterType<INewsReaderPresenter, NewsReaderPresenter>();
        }


    }
}
