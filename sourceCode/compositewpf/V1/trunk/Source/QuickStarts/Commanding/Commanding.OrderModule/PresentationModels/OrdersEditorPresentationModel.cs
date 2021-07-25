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
using System.Globalization;
using Commanding.Modules.Order.Views;
using Microsoft.Practices.Composite.Events;

namespace Commanding.Modules.Order.PresentationModels
{
    public class OrdersEditorPresentationModel
    {
        private const int InitialOrdersCount = 3;
        private readonly OrdersCommandProxy commandProxy;

        public OrdersEditorView View { get; private set; }
        public ObservableCollection<OrderPresentationModel> Orders { get; private set; }

        public OrdersEditorPresentationModel(OrdersEditorView view, OrdersCommandProxy commandProxy)
        {
            this.commandProxy = commandProxy;
            Orders = new ObservableCollection<OrderPresentationModel>();

            PopulateOrders();

            View = view;
            view.Model = this;
        }

        private void PopulateOrders()
        {
            foreach (OrderPresentationModel order in GetOrdersToEdit())
            {
                order.Saved += OrderSaved;
                commandProxy.SaveAllOrdersCommands.RegisterCommand(order.SaveOrderCommand);
                Orders.Add(order);
            }
        }

        void OrderSaved(object sender, DataEventArgs<OrderPresentationModel> e)
        {
            if (e != null && e.Value != null)
            {
                OrderPresentationModel order = e.Value;
                if (Orders.Contains(order))
                {
                    order.Saved -= OrderSaved;
                    commandProxy.SaveAllOrdersCommands.UnregisterCommand(order.SaveOrderCommand);
                    Orders.Remove(order);
                }
            }
        }

        private static IEnumerable<OrderPresentationModel> GetOrdersToEdit()
        {
            List<OrderPresentationModel> orders = new List<OrderPresentationModel>(InitialOrdersCount);
            for (int i = 1; i <= InitialOrdersCount; i++)
            {
                string orderName = String.Format(CultureInfo.CurrentCulture, "Order {0}", i);
                orders.Add(new OrderPresentationModel { OrderName = orderName });
            }
            return orders;
        }
    }
}