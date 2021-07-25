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
using System.Windows.Input;
using Microsoft.Practices.Composite.Wpf.Commands;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.Position.Models;

namespace StockTraderRI.Modules.Position.Orders
{
    public interface IOrderDetailsPresentationModel
    {
        event EventHandler CloseViewRequested;
        IOrderDetailsView View { get; set; }
        TransactionInfo TransactionInfo { get; set; }
        TransactionType TransactionType { get; }
        string TickerSymbol { get; }
        int? Shares { get; }
        decimal? StopLimitPrice { get; }
        DelegateCommand<object> SubmitCommand { get; }
        DelegateCommand<object> CancelCommand { get; }
    }
}
