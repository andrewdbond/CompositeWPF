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
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Models;

namespace StockTraderRI.Modules.Position.Orders
{
    public class OrderCompositePresentationModel : DependencyObject, IOrderCompositePresentationModel, IHeaderInfoProvider<string>
    {
        private readonly IOrderCompositeView _view;
        private readonly IOrderDetailsPresentationModel _orderDetailsPresentationModel;

        public static readonly DependencyProperty HeaderInfoProperty =
            DependencyProperty.Register("HeaderInfo", typeof(string), typeof(OrderCompositePresentationModel));

        public OrderCompositePresentationModel(IOrderCompositeView orderCompositeView, IOrderDetailsPresentationModel orderDetailsPresentationModel,
                                         OrderCommandsView orderCommandsView)
        {
            _orderDetailsPresentationModel = orderDetailsPresentationModel;
            _orderDetailsPresentationModel.CloseViewRequested += _orderPresenter_CloseViewRequested;
            _view = orderCompositeView;
            orderCommandsView.Model = _orderDetailsPresentationModel;
            this.OrderDetailsView = _orderDetailsPresentationModel.View;
            this.OrderCommandsView = orderCommandsView;
            _view.IsActiveChanged += compositeView_IsActiveChanged;
            _view.Model = this;

        }

        void compositeView_IsActiveChanged(object sender, EventArgs e)
        {
            _orderDetailsPresentationModel.View.IsActive = _view.IsActive;
        }

        void _orderPresenter_CloseViewRequested(object sender, EventArgs e)
        {
            OnCloseViewRequested(sender, e);
        }

        private void SetTransactionInfo(TransactionInfo transactionInfo)
        {
            //This instance of TransactionInfo acts as a "shared model" between this view and the order details view.
            //The scenario says that these 2 views are decoupled, so they don't share the presentation model, they are only tied
            //with this TransactionInfo
            _orderDetailsPresentationModel.TransactionInfo = transactionInfo;

            //Bind the CompositeOrderView header to a string representation of the TransactionInfo shared instance (we expect the details presenter to modify it from user interaction).
            MultiBinding binding = new MultiBinding();
            binding.Bindings.Add(new Binding("TransactionType") { Source = transactionInfo });
            binding.Bindings.Add(new Binding("TickerSymbol") { Source = transactionInfo });
            binding.Converter = new OrderHeaderConverter();
            BindingOperations.SetBinding(this, HeaderInfoProperty, binding);
        }

        protected virtual void OnCloseViewRequested(object sender, EventArgs e)
        {
            CloseViewRequested(sender, e);
        }

        public event EventHandler CloseViewRequested = delegate { };

        public IOrderCompositeView View
        {
            get
            {
                return _view;
            }
        }

        public ICommand SubmitCommand
        {
            get { return _orderDetailsPresentationModel.SubmitCommand; }
        }

        public ICommand CancelCommand
        {
            get { return _orderDetailsPresentationModel.CancelCommand; }
        }

        public TransactionInfo TransactionInfo
        {
            get { return _orderDetailsPresentationModel.TransactionInfo; }
            set { SetTransactionInfo(value);}

        }

        public int Shares
        {
            get { return _orderDetailsPresentationModel.Shares ?? 0; }
        }

        public string HeaderInfo
        {
            get { return (string)GetValue(HeaderInfoProperty); }
            set { SetValue(HeaderInfoProperty, value); }
        }

        public object OrderDetailsView { get; private set; }
        public object OrderCommandsView { get; private set; }

        public class OrderHeaderConverter : IMultiValueConverter
        {
            /// <summary>
            /// Converts a <see cref="TransactionType"/> and a ticker symbol to a header that can be placed on the TabItem header
            /// </summary>
            /// <param name="values">values[0] should be of type <see cref="TransactionType"/>. values[1] should be a string with the ticker symbol</param>
            /// <param name="targetType"></param>
            /// <param name="parameter"></param>
            /// <param name="culture"></param>
            /// <returns>Returns a human readable string with the transaction type and ticker symbol</returns>
            public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return values[0].ToString() + " " + values[1].ToString();
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    }
}