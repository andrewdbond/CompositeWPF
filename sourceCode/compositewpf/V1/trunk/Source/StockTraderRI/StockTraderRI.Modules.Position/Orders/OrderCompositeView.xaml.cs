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
using System.Windows.Controls;
using StockTraderRI.Modules.Position.Interfaces;

namespace StockTraderRI.Modules.Position.Orders
{
    /// <summary>
    /// Interaction logic for OrderCompositeView.xaml
    /// </summary>
    public partial class OrderCompositeView : UserControl, IOrderCompositeView
    {
        public OrderCompositeView()
        {
            InitializeComponent();
        }

        public IOrderCompositePresentationModel Model
        {
            set
            {
                DataContext = value;
            }
        }

        #region IActiveAware Members

        private bool _isActive = false;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    IsActiveChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler IsActiveChanged = delegate { };

        #endregion

    }
}
