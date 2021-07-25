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
using System.Globalization;
using Microsoft.Practices.Composite.Wpf.Commands;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Models;
using StockTraderRI.Modules.Position.Properties;

namespace StockTraderRI.Modules.Position.Orders
{
    public class OrderDetailsPresentationModel : IOrderDetailsPresentationModel, INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly IAccountPositionService accountPositionService;
        private readonly IOrdersService ordersService;
        private TransactionInfo _transactionInfo;
        private int? _shares = 0;
        private OrderType _orderType = OrderType.Market;
        private decimal? _stopLimitPrice = 0;
        private TimeInForce _timeInForce;

        private readonly IDictionary<string, string> errors = new Dictionary<string, string>();
        
        public OrderDetailsPresentationModel(IOrderDetailsView view, IAccountPositionService accountPositionService, IOrdersService ordersService)
        {
            View = view;
            this.accountPositionService = accountPositionService;
            this.ordersService = ordersService;

            _transactionInfo = new TransactionInfo();

            //use localizable enum descriptions
            AvailableOrderTypes = new List<ValueDescription<OrderType>>
                                        {
                                            new ValueDescription<OrderType>(OrderType.Limit, Resources.OrderType_Limit),
                                            new ValueDescription<OrderType>(OrderType.Market, Resources.OrderType_Market),
                                            new ValueDescription<OrderType>(OrderType.Stop, Resources.OrderType_Stop)
                                        };

            AvailableTimesInForce = new List<ValueDescription<TimeInForce>>
                                          {
                                              new ValueDescription<TimeInForce>(TimeInForce.EndOfDay, Resources.TimeInForce_EndOfDay),
                                              new ValueDescription<TimeInForce>(TimeInForce.ThirtyDays, Resources.TimeInForce_ThirtyDays)
                                          };

            View.Model = this;
            ValidateModel();

            SubmitCommand = new DelegateCommand<object>(Submit, CanSubmit);
            CancelCommand = new DelegateCommand<object>(Cancel);
            SubmitCommand.IsActive = view.IsActive;
            CancelCommand.IsActive = view.IsActive;

            view.IsActiveChanged += view_IsActiveChanged;
        }

        public event EventHandler CloseViewRequested = delegate { };
        public IOrderDetailsView View { get; set; }
        public IList<ValueDescription<OrderType>> AvailableOrderTypes { get; set; }
        public IList<ValueDescription<TimeInForce>> AvailableTimesInForce { get; set; }

        void OnPropertyChanged(string propertyName)
        {
            ValidateModel();
            SubmitCommand.RaiseCanExecuteChanged();
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public TransactionInfo TransactionInfo
        {
            get { return _transactionInfo; }
            set
            {
                _transactionInfo = value;
                OnPropertyChanged("TransactionType");
                OnPropertyChanged("TickerSymbol");
            }
        }

        public TransactionType TransactionType
        {
            get { return _transactionInfo.TransactionType; }
            set
            {
                if (_transactionInfo.TransactionType != value)
                {
                    _transactionInfo.TransactionType = value;
                    OnPropertyChanged("TransactionType");
                }
            }
        }

        public string TickerSymbol
        {
            get { return _transactionInfo.TickerSymbol; }
            set
            {
                if (_transactionInfo.TickerSymbol != value)
                {
                    _transactionInfo.TickerSymbol = value;
                    OnPropertyChanged("TickerSymbol");
                }
            }
        }

        public int? Shares
        {
            get { return _shares; }
            set
            {
                if (_shares == null || _shares != value)
                {
                    _shares = value;
                    OnPropertyChanged("Shares");
                }
            }
        }

        public OrderType OrderType
        {
            get { return _orderType; }
            set
            {
                if (!value.Equals(_orderType))
                {
                    _orderType = value;
                    OnPropertyChanged("OrderType");
                }
            }
        }

        public decimal? StopLimitPrice
        {
            get
            {
                return _stopLimitPrice;
            }
            set
            {
                if (_stopLimitPrice == null || value != _stopLimitPrice)
                {
                    _stopLimitPrice = value;
                    OnPropertyChanged("StopLimitPrice");
                }
            }
        }

        public TimeInForce TimeInForce
        {
            get { return _timeInForce; }
            set
            {
                if (value != _timeInForce)
                {
                    _timeInForce = value;
                    OnPropertyChanged("TimeInForce");
                }

                _timeInForce = value;
            }
        }

        public DelegateCommand<object> SubmitCommand { get; private set;}
        public DelegateCommand<object> CancelCommand { get; private set;}

        public bool HasErrors()
        {
            return (errors.Count > 0);
        }

        private void ValidateModel()
        {
            ClearErrors();
            if (!Shares.HasValue || Shares <= 0)
            {
                this["Shares"] = Resources.InvalidSharesRange;
            }
            else if (TransactionType == TransactionType.Sell
                && !HoldsEnoughShares(TickerSymbol, Shares))
            {
                this["Shares"] = String.Format(CultureInfo.InvariantCulture, Resources.NotEnoughSharesToSell, Shares);
            }

            if (!StopLimitPrice.HasValue || StopLimitPrice <= 0)
            {
                this["StopLimitPrice"] = Resources.InvalidStopLimitPrice;
            }
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                if (errors.ContainsKey(columnName))
                    return errors[columnName];

                return null;
            }
            set
            {
                errors[columnName] = value;
            }
        }

        #endregion

        internal void ClearErrors()
        {
            errors.Clear();
        }

        void view_IsActiveChanged(object sender, EventArgs e)
        {
            SubmitCommand.IsActive = View.IsActive;
            CancelCommand.IsActive = View.IsActive;
        }

        bool CanSubmit(object parameter)
        {
            return !HasErrors();
        }

        private bool HoldsEnoughShares(string symbol, int? sharesToSell)
        {
            if (!sharesToSell.HasValue) return false;

            foreach (AccountPosition accountPosition in accountPositionService.GetAccountPositions())
            {
                if (accountPosition.TickerSymbol.Equals(symbol, StringComparison.OrdinalIgnoreCase))
                {
                    if (accountPosition.Shares >= sharesToSell)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        void Submit(object parameter)
        {
            if (!CanSubmit(parameter))
                throw new InvalidOperationException();

            var order = new Order();
            order.TransactionType = this.TransactionType;
            order.OrderType = this.OrderType;
            order.Shares = this.Shares.HasValue ? this.Shares.Value : 0;
            order.StopLimitPrice = this.StopLimitPrice.HasValue ? this.StopLimitPrice.Value : 0;
            order.TickerSymbol = this.TickerSymbol;
            order.TimeInForce = this.TimeInForce;

            ordersService.Submit(order);

            CloseViewRequested(this, EventArgs.Empty);
        }


        void Cancel(object parameter)
        {
            CloseViewRequested(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate {};
    }
}
