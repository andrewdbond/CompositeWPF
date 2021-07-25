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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.News.Article;

namespace StockTraderRI.Modules.News.Tests.NewsView
{
    /// <summary>
    /// Summary description for NewsReaderPresenterFixture
    /// </summary>
    [TestClass]
    public class NewsReaderPresenterFixture
    {
        public NewsReaderPresenterFixture()
        {
        }

     
        [TestMethod]
        public void ShowInformsViewToShow()
        {
            var view = new MockNewsReaderView();
            var presenter = new NewsReaderPresenter(view);

            presenter.Show();

            Assert.IsTrue(view.ShowViewWasCalled);

        }

        [TestMethod]
        public void SetNewsArticlesSetsViewModel()
        {
            var view = new MockNewsReaderView();
            var presenter = new NewsReaderPresenter(view);

            NewsArticle article = new NewsArticle() { Title = "My Title", Body = "My Body" };
            presenter.SetNewsArticle(article);

            Assert.AreSame(article,view.Model);

        }
    }

    internal class MockNewsReaderView : INewsReaderView
    {
        public bool ShowViewWasCalled { get; private set; }

        private NewsArticle model;

        public void ShowView()
        {
            ShowViewWasCalled = true;
        }

        public NewsArticle Model
        {
            get { return this.model; }
            set { this.model = value; }
        }

        public void SetNewsArticle(NewsArticle article)
        {
            this.Model = article;
        }

    }
}
