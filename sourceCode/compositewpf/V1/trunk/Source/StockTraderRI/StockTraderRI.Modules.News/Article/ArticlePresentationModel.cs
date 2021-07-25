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
using System.ComponentModel;
using System.Windows.Data;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.News.Controllers;

namespace StockTraderRI.Modules.News.Article
{

    public class ArticlePresentationModel : IArticlePresentationModel, INotifyPropertyChanged
    {
        private ICollectionView _articles;
        private readonly INewsFeedService newsFeedService;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ArticlePresentationModel(IArticleView view, INewsFeedService newsFeedService)
        {
            View = view;
            View.Model = this;
            this.newsFeedService = newsFeedService;
            View.ShowNewsReader += View_ShowNewsReader;
        }

        void View_ShowNewsReader(object sender, EventArgs e)
        {
            this.Controller.ShowNewsReader();
        }

        void Articles_CurrentChanged(object sender, EventArgs e)
        {

            if (this.Articles == null)
            {
                Controller.CurrentNewsArticleChanged(null);
            }
            else
            {
                Controller.CurrentNewsArticleChanged((NewsArticle)this.Articles.CurrentItem);
            }
        }

        public IArticleView View { get; private set; }

        public INewsController Controller { get; set; }

        public void SetTickerSymbol(string companySymbol)
        {
            if (this.Articles != null)
            {
                this.Articles.CurrentChanged -= Articles_CurrentChanged;
            }

            IList<NewsArticle> newsArticles = newsFeedService.GetNews(companySymbol);

            if (newsArticles == null)
            {
                this.Articles = null;
                Articles_CurrentChanged(null, null);
            }
            else
            {
                this.Articles = CollectionViewSource.GetDefaultView(newsArticles);
                this.Articles.CurrentChanged += Articles_CurrentChanged;
                Articles_CurrentChanged(null, null);
            }
        }

        public ICollectionView Articles
        {
            get { return _articles; }
            set
            {
                if (_articles != value)
                {
                    _articles = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Articles"));
                }
            }
        }
    }
}
