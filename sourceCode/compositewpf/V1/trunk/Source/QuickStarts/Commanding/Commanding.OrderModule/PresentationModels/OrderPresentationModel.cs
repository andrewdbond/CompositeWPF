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
using Commanding.Modules.Order.Properties;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Wpf.Commands;

namespace Commanding.Modules.Order.PresentationModels
{
    public class OrderPresentationModel : IDataErrorInfo, INotifyPropertyChanged
    {
        private readonly IDictionary<string, string> errors = new Dictionary<string, string>();
        private int _quantity;
        private decimal _price;
        private DateTime _deliveryDate;
        private decimal _shipping;
        private decimal _total;

        public event EventHandler<DataEventArgs<OrderPresentationModel>> Saved;

        ///<summary>
        ///Occurs when a property value changes.
        ///</summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public string OrderName { get; set; }
        public DelegateCommand<object> SaveOrderCommand { get; private set; }

        public OrderPresentationModel()
        {
            SaveOrderCommand = new DelegateCommand<object>(Save, CanSave);
            DeliveryDate = DateTime.Now;
            PropertyChanged += OnPropertyChangedEvent;
            Validate();
        }

        private void OnPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            if (propertyName == "Price" || propertyName == "Quantity" || propertyName == "Shipping")
            {
                Total = (Price * Quantity) + Shipping;
            }
            Validate();
            SaveOrderCommand.RaiseCanExecuteChanged();
        }

        public DateTime DeliveryDate
        {
            get { return _deliveryDate; }
            set
            {
                if (_deliveryDate != value)
                {
                    _deliveryDate = value;
                    OnPropertyChanged("DeliveryDate");
                }
            }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

        public decimal Price
        {
            get { return _price; }
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged("Price");
                }
            }
        }

        public decimal Shipping
        {
            get { return _shipping; }
            set
            {
                if (_shipping != value)
                {
                    _shipping = value;
                    OnPropertyChanged("Shipping");
                }
            }
        }


        public decimal Total
        {
            get { return _total; }
            set
            {
                if (_total != value)
                {
                    _total = value;
                    OnPropertyChanged("Total");
                }
            }
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

        public string Error
        {
            get
            {
                // Not implemented because we are not consuming it. 
                // Instead, we are displaying error messages at the item level. 
                throw new NotImplementedException();
            }
        }

        private void ClearError(string columnName)
        {
            if (errors.ContainsKey(columnName))
                errors.Remove(columnName);
        }



        private bool CanSave(object arg)
        {
            return this.errors.Count == 0 && Quantity > 0;
        }

        private void Validate()
        {
            if (this.Price <= 0)
            {
                this["Price"] = Resources.InvalidPriceRange;
            }
            else
            {
                ClearError("Price");
            }

            if (this.Quantity <= 0)
            {
                this["Quantity"] = Resources.InvalidQuantityRange;
            }
            else
            {
                ClearError("Quantity");
            }
        }

        private void Save(object obj)
        {
            //Save the order here.
            Console.WriteLine(String.Format(CultureInfo.InvariantCulture, "{0} saved.", OrderName));

            //Notify that the order was saved
            OnSaved(new DataEventArgs<OrderPresentationModel>(this));
        }

        private void OnSaved(DataEventArgs<OrderPresentationModel> e)
        {
            EventHandler<DataEventArgs<OrderPresentationModel>> savedHandler = Saved;
            if (savedHandler != null) savedHandler(this, e);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler Handler = PropertyChanged;
            if (Handler != null) Handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}