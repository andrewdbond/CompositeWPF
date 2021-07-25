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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Wpf.Commands;
using Microsoft.Practices.Composite.Wpf.Events;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Watch.Properties;
using StockTraderRI.Modules.Watch.Services;

namespace StockTraderRI.Modules.Watch.WatchList
{
    public class WatchListPresentationModel : IWatchListPresentationModel, INotifyPropertyChanged, IHeaderInfoProvider<string>
    {
        private ObservableCollection<WatchItem> _watchListItems;

        public WatchListPresentationModel(IWatchListView view, IWatchListService watchListService, IMarketFeedService marketFeedService, IEventAggregator eventAggregator)
        {
            View = view;
            this.HeaderInfo = Resources.WatchListTitle;
            this.WatchListItems = new ObservableCollection<WatchItem>();
            View.Model = this;

            this.marketFeedService = marketFeedService;

            this.watchList = watchListService.RetrieveWatchList();
            watchList.CollectionChanged += delegate { PopulateWatchItemsList(watchList); };
            PopulateWatchItemsList(watchList);

            eventAggregator.GetEvent<MarketPricesUpdatedEvent>().Subscribe(MarketPricesUpdated, ThreadOption.UIThread);
            RemoveWatchCommand = new DelegateCommand<string>(RemoveWatch);
        }


        public ObservableCollection<WatchItem> WatchListItems
        {
            get { return _watchListItems; }
            set
            {
                if (_watchListItems != value)
                {
                    _watchListItems = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("WatchListItems"));
                }
            }
        }

        public string HeaderInfo { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        private void MarketPricesUpdated(IDictionary<string, decimal> updatedPriceList)
        {
            foreach (WatchItem watchItem in this.WatchListItems)
            {
                if (updatedPriceList.ContainsKey(watchItem.TickerSymbol))
                    watchItem.CurrentPrice = updatedPriceList[watchItem.TickerSymbol];
            }
        }

        private void RemoveWatch(string tickerSymbol)
        {
            watchList.Remove(tickerSymbol);
        }

        private void PopulateWatchItemsList(ObservableCollection<string> watchItemsList)
        {
            this.WatchListItems.Clear();
            foreach (string tickerSymbol in watchItemsList)
            {
                decimal? currentPrice;
                try
                {
                    currentPrice = marketFeedService.GetPrice(tickerSymbol);
                }
                catch (ArgumentException)
                {
                    currentPrice = null;
                }
                this.WatchListItems.Add(new WatchItem(tickerSymbol, currentPrice));
            }
        }

        public IWatchListView View { get; private set; }
        public ICommand RemoveWatchCommand { get; private set; }
        private readonly IMarketFeedService marketFeedService;
        private readonly ObservableCollection<string> watchList;
    }
}
