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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Commanding.Modules.Order.Properties;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Commanding.Modules.Order.PresentationModels
{
    public partial class OrderPresentationModel : INotifyPropertyChanged
    {
        private readonly IDictionary<string, string> errors = new Dictionary<string, string>();
        private int? quantity;
        private decimal? price;
        private DateTime deliveryDate;
        private decimal? shipping;

        public OrderPresentationModel()
        {
            this.SaveOrderCommand = new DelegateCommand<object>(this.Save, this.CanSave);
            this.DeliveryDate = DateTime.Now;
            this.PropertyChanged += this.OnPropertyChangedEvent;
            this.Validate();
        }

        public event EventHandler<DataEventArgs<OrderPresentationModel>> Saved;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public string OrderName { get; set; }

        public DelegateCommand<object> SaveOrderCommand { get; private set; }

        public DateTime DeliveryDate
        {
            get { return this.deliveryDate; }
            set
            {
                if (this.deliveryDate != value)
                {
                    this.deliveryDate = value;
                    this.OnPropertyChanged("DeliveryDate");
                }
            }
        }

        public int? Quantity
        {
            get { return this.quantity; }
            set
            {
                if (this.quantity != value)
                {
                    this.quantity = value;
                    this.OnPropertyChanged("Quantity");
                }
            }
        }

        public decimal? Price
        {
            get { return this.price; }
            set
            {
                if (this.price != value)
                {
                    this.price = value;
                    this.OnPropertyChanged("Price");
                }
            }
        }

        public decimal? Shipping
        {
            get
            {
                return this.shipping;
            }

            set
            {
                if (this.shipping != value)
                {
                    this.shipping = value;
                    this.OnPropertyChanged("Shipping");
                }
            }
        }

        public decimal Total
        {
            get
            {
                if (this.Price != null && this.Quantity != null)
                {
                    return (this.Price.Value * this.Quantity.Value) + (this.Shipping ?? 0);
                }

                return 0;
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (this.errors.ContainsKey(columnName))
                {
                    return this.errors[columnName];
                }

                return null;
            }

            set
            {
                this.errors[columnName] = value;
            }
        }

        public string Error
        {
            get
            {
                // Not implemented because we are not consuming it. 
                // Instead, we are displaying error messages at the item level. 
                throw new NotImplementedException();
            }
        }

        private void OnPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            if (propertyName == "Price" || propertyName == "Quantity" || propertyName == "Shipping")
            {
                this.OnPropertyChanged("Total");
            }

            this.Validate();
            this.SaveOrderCommand.RaiseCanExecuteChanged();
        }

        private void ClearError(string columnName)
        {
            if (this.errors.ContainsKey(columnName))
            {
                this.errors.Remove(columnName);
            }
        }

        private bool CanSave(object arg)
        {
            return this.errors.Count == 0 && this.Quantity > 0;
        }

        private void Validate()
        {
            if (this.Price == null || this.Price <= 0)
            {
                this["Price"] = Resources.InvalidPriceRange;
            }
            else
            {
                this.ClearError("Price");
            }

            if (this.Quantity == null || this.Quantity <= 0)
            {
                this["Quantity"] = Resources.InvalidQuantityRange;
            }
            else
            {
                this.ClearError("Quantity");
            }
        }

        private void Save(object obj)
        {
            // Save the order here.
            Console.WriteLine(String.Format(CultureInfo.InvariantCulture, "{0} saved.", this.OrderName));

            // Notify that the order was saved
            this.OnSaved(new DataEventArgs<OrderPresentationModel>(this));
        }

        private void OnSaved(DataEventArgs<OrderPresentationModel> e)
        {
            EventHandler<DataEventArgs<OrderPresentationModel>> savedHandler = this.Saved;
            if (savedHandler != null)
            {
                savedHandler(this, e);
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