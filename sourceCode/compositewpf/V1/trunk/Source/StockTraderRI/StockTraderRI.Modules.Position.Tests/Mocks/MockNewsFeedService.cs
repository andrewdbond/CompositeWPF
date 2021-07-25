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
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;

namespace StockTraderRI.Modules.Position.Tests.Mocks
{
    public class MockNewsFeedService : INewsFeedService
    {
        readonly Dictionary<string, List<NewsArticle>> newsData = new Dictionary<string, List<NewsArticle>>();

        internal void UpdateNews(string tickerSymbol, NewsArticle newsArticle)
        {
            newsData.Add(tickerSymbol, new List<NewsArticle>() {newsArticle});
            Updated(this, new NewsFeedEventArgs(tickerSymbol,newsArticle.Title));
        }

        #region INewsFeedService Members

        public IList<NewsArticle> GetNews(string tickerSymbol)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<NewsFeedEventArgs> Updated = delegate { };

        public bool HasNews(string tickerSymbol)
        {
            return newsData.ContainsKey(tickerSymbol);
        }

        #endregion
    }
}
