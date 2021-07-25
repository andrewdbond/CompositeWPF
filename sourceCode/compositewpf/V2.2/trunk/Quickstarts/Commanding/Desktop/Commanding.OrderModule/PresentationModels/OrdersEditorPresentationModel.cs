//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System.Collections.ObjectModel;
using System.ComponentModel;
using Commanding.Modules.Order.Services;
using Commanding.Modules.Order.Views;
using Microsoft.Practices.Composite.Events;

namespace Commanding.Modules.Order.PresentationModels
{
    public class OrdersEditorPresentationModel : INotifyPropertyChanged
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly OrdersCommandProxy commandProxy;
        private OrderPresentationModel selectedOrder;

        public OrdersEditorPresentationModel(OrdersEditorView view, IOrdersRepository ordersRepository, OrdersCommandProxy commandProxy)
        {
            this.ordersRepository = ordersRepository;
            this.commandProxy = commandProxy;
            this.Orders = new ObservableCollection<OrderPresentationModel>();
            this.PopulateOrders();

            this.View = view;
            view.Model = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public OrdersEditorView View { get; private set; }

        public ObservableCollection<OrderPresentationModel> Orders { get; private set; }

        public OrderPresentationModel SelectedOrder
        {
            get
            {
                return this.selectedOrder;
            }

            set
            {
                if (this.selectedOrder != value)
                {
                    this.selectedOrder = value;
                    this.OnPropertyChanged("SelectedOrder");
                }
            }
        }

        private void PopulateOrders()
        {
            foreach (Services.Order order in this.ordersRepository.GetOrdersToEdit())
            {
                var orderPresentationModel = new OrderPresentationModel()
                {
                    OrderName = order.Name,
                    DeliveryDate = order.DeliveryDate
                };
                orderPresentationModel.Saved += this.OrderSaved;
                this.commandProxy.SaveAllOrdersCommand.RegisterCommand(orderPresentationModel.SaveOrderCommand);
                this.Orders.Add(orderPresentationModel);
            }

            this.SelectedOrder = this.Orders[0];
        }

        private void OrderSaved(object sender, DataEventArgs<OrderPresentationModel> e)
        {
            if (e != null && e.Value != null)
            {
                OrderPresentationModel order = e.Value;
                if (this.Orders.Contains(order))
                {
                    order.Saved -= this.OrderSaved;
                    this.commandProxy.SaveAllOrdersCommand.UnregisterCommand(order.SaveOrderCommand);
                    this.Orders.Remove(order);
                    if (this.Orders.Count > 0)
                    {
                        this.SelectedOrder = this.Orders[0];
                    }
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}