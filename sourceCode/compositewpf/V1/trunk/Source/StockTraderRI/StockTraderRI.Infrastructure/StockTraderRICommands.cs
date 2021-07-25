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
using Microsoft.Practices.Composite.Wpf.Commands;

namespace StockTraderRI.Infrastructure
{

    public static class StockTraderRICommands
    {
        public static CompositeCommand SubmitOrderCommand = new CompositeCommand(true);
        public static CompositeCommand CancelOrderCommand = new CompositeCommand(true);
        public static CompositeCommand SubmitAllOrdersCommand = new CompositeCommand();
        public static CompositeCommand CancelAllOrdersCommand = new CompositeCommand();
    }

    public class StockTraderRICommandProxy
    {
        virtual public CompositeCommand SubmitOrderCommand
        {
            get { return StockTraderRICommands.SubmitOrderCommand; }
        }

        virtual public CompositeCommand CancelOrderCommand
        {
            get { return StockTraderRICommands.CancelOrderCommand; }
        }

        virtual public CompositeCommand SubmitAllOrdersCommand
        {
            get { return StockTraderRICommands.SubmitAllOrdersCommand; }
        }

        virtual public CompositeCommand CancelAllOrdersCommand
        {
            get { return StockTraderRICommands.CancelAllOrdersCommand; }
        }
    }
}
