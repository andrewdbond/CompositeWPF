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
using System.Windows.Controls;
using System.Windows;
using Commanding.Modules.Order.PresentationModels;

namespace Commanding.Modules.Order.Views
{
    public partial class OrdersEditorView : UserControl
    {
        public OrdersEditorView()
        {
            InitializeComponent();
        }

        public OrdersEditorPresentationModel Model
        {
            get { return DataContext as OrdersEditorPresentationModel; }
            set
            {
                DataContext = value;
                value.PropertyChanged += ModelPropertyChanged;
            }
        }

        void ModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedOrder")
            {
                this.SelectedOrderControl.Visibility = this.Model.SelectedOrder != null ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
