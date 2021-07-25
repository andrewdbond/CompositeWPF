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

using System.Windows;
using StockTraderRI.Infrastructure;

namespace StockTraderRI.Modules.Position.Models
{
    public class TransactionInfo : DependencyObject
    {
        public TransactionInfo() {}

        public TransactionInfo(string tickerSymbol, TransactionType transactionType)
        {
            TickerSymbol = tickerSymbol;
            TransactionType = transactionType;
        }

        public static readonly DependencyProperty TickerSymbolProperty =
            DependencyProperty.Register("TickerSymbol", typeof(string), typeof(TransactionInfo));

        public static readonly DependencyProperty TransactionTypeProperty =
            DependencyProperty.Register("TransactionType", typeof(TransactionType), typeof(TransactionInfo));

        public string TickerSymbol
        {
            get { return (string)GetValue(TickerSymbolProperty); }
            set { SetValue(TickerSymbolProperty, value); }
        }

        public TransactionType TransactionType
        {
            get { return (TransactionType)GetValue(TransactionTypeProperty); }
            set { SetValue(TransactionTypeProperty, value); }
        }

    }
}