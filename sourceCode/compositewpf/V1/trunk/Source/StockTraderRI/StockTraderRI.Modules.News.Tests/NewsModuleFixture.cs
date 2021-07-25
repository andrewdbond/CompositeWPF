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

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.News.Article;
using StockTraderRI.Modules.News.Controllers;
using StockTraderRI.Modules.News.Services;
using StockTraderRI.Modules.News.Tests.Mocks;

namespace StockTraderRI.Modules.News.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class NewsModuleFixture
    {
        [TestMethod]
        public void NewsModuleRegistersNewsViewAndNewsFeedService()
        {
            var container = new MockUnityContainer();
            TestableNewsModule newsModule = new TestableNewsModule(container);

            newsModule.InvokeRegisterViewsAndServices();

            Assert.AreEqual(typeof(ArticleView), container.Types[typeof(IArticleView)]);
            Assert.AreEqual(typeof(NewsFeedService), container.Types[typeof(INewsFeedService)]);
            Assert.AreEqual(typeof(NewsController), container.Types[typeof(INewsController)]);
            Assert.AreEqual(typeof(ArticlePresentationModel), container.Types[typeof(IArticlePresentationModel)]);
            Assert.AreEqual(typeof(NewsReaderPresenter), container.Types[typeof(INewsReaderPresenter)]);
            Assert.AreEqual(typeof(NewsReader), container.Types[typeof(INewsReaderView)]);
        }

        [TestMethod]
        public void InitCallsRunOnNewsController()
        {
            MockUnityResolver container = new MockUnityResolver();
            var controller = new MockNewsController();
            container.Bag.Add(typeof(INewsController), controller);
            var newsModule = new NewsModule(container);

            newsModule.Initialize();

            Assert.IsTrue(controller.RunCalled);
        }


        internal class TestableNewsModule : NewsModule
        {
            public TestableNewsModule(IUnityContainer container)
                : base(container)
            {

            }

            public void InvokeRegisterViewsAndServices()
            {
                base.RegisterViewsAndServices();
            }
        }
    }
}
