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
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Wpf.Commands;
using Microsoft.Practices.Composite.Wpf.Events;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.Position.Controllers;
using StockTraderRI.Modules.Position.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace StockTraderRI.Modules.Position.PositionSummary
{
    public class PositionSummaryPresentationModel : IPositionSummaryPresentationModel, INotifyPropertyChanged
    {
        private readonly ITrendLinePresenter _trendLinePresenter;

        public event EventHandler<DataEventArgs<string>> TickerSymbolSelected = delegate { };

        public PositionSummaryPresentationModel(IPositionSummaryView view, IAccountPositionService accountPositionService
                                        , IMarketFeedService marketFeedSvc
                                        , IMarketHistoryService marketHistorySvc
                                        , ITrendLinePresenter trendLinePresenter
                                        , IOrdersController ordersController
                                        , IEventAggregator eventAggregator)
        {
            View = view;
            AccountPositionSvc = accountPositionService;
            MarketHistorySvc = marketHistorySvc;
            EventAggregator = eventAggregator;
            MarketFeedSvc = marketFeedSvc;

            PositionSummaryItems = new ObservableCollection<PositionSummaryItem>();

            PopulatePresentationModel();
            BuyCommand = ordersController.BuyCommand;
            SellCommand = ordersController.SellCommand;

            View.Model = this;
            _trendLinePresenter = trendLinePresenter;
            View.ShowTrendLine(trendLinePresenter.View);

            //Initially show the FAKEINDEX
            trendLinePresenter.OnTickerSymbolSelected("FAKEINDEX");

            eventAggregator.GetEvent<MarketPricesUpdatedEvent>().Subscribe(MarketPricesUpdated, ThreadOption.UIThread);

            InitializeEvents();

        }

        public string HeaderInfo
        {
            get { return "POSITION"; }
        }

        private void InitializeEvents()
        {
            AccountPositionSvc.Updated += PositionSummaryItems_Updated;
            View.TickerSymbolSelected += View_TickerSymbolSelected;
        }

        private void View_TickerSymbolSelected(object sender, DataEventArgs<string> e)
        {
            _trendLinePresenter.OnTickerSymbolSelected(e.Value);

            EventAggregator.GetEvent<TickerSymbolSelectedEvent>().Publish(e.Value);
        }

        private void PositionSummaryItems_Updated(object sender, AccountPositionModelEventArgs e)
        {
            if (e.AcctPosition != null)
            {
                PositionSummaryItem positionSummaryItem = PositionSummaryItems.First(p => p.TickerSymbol == e.AcctPosition.TickerSymbol);

                if (positionSummaryItem != null)
                {
                    positionSummaryItem.Shares = e.AcctPosition.Shares;
                    positionSummaryItem.CostBasis = e.AcctPosition.CostBasis;
                }
            }
        }

        private void MarketPricesUpdated(IDictionary<string, decimal> priceList)
        {
            foreach (PositionSummaryItem position in PositionSummaryItems)
            {
                if (priceList.ContainsKey(position.TickerSymbol))
                {
                    position.CurrentPrice = priceList[position.TickerSymbol];
                }
            }
        }

        private void PopulatePresentationModel()
        {
            PositionSummaryItem positionSummaryItem;
            foreach (AccountPosition accountPosition in AccountPositionSvc.GetAccountPositions())
            {
                positionSummaryItem = new PositionSummaryItem(accountPosition.TickerSymbol, accountPosition.CostBasis, accountPosition.Shares, MarketFeedSvc.GetPrice(accountPosition.TickerSymbol));
                positionSummaryItem.PriceMarketHistory = MarketHistorySvc.GetPriceHistory(accountPosition.TickerSymbol);
                PositionSummaryItems.Add(positionSummaryItem);
            }
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public IPositionSummaryView View { get; set; }
        public ObservableCollection<PositionSummaryItem> PositionSummaryItems { get; set; }

        public DelegateCommand<string> BuyCommand { get; private set; }
        public DelegateCommand<string> SellCommand { get; private set; }

        private IAccountPositionService AccountPositionSvc { get; set; }
        private IMarketHistoryService MarketHistorySvc { get; set; }
        private IEventAggregator EventAggregator { get; set; }
        private IMarketFeedService MarketFeedSvc { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate {};

        #endregion
    }
}
