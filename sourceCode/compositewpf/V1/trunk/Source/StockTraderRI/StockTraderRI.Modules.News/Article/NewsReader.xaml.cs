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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using StockTraderRI.Infrastructure.Models;

namespace StockTraderRI.Modules.News.Article
{
    /// <summary>
    /// Interaction logic for NewsReader.xaml
    /// </summary>
    public partial class NewsReader : Window, INewsReaderView
    {
        public NewsReader()
        {
            InitializeComponent();
            this.Closing += NewsReader_Closing;
            this.Topmost = true;
        }

        void NewsReader_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            //work around for not being able to hide a window during closing. This behavior was needed in WPF to ensure consistent window
            //visiblity state
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (DispatcherOperationCallback)delegate(object o)
            {
                Hide();
                return null;
            }, null);

        }

        public NewsArticle Article
        {
            set { DataContext = value; }
        }

        public void ShowView()
        {
            if (this.Owner == null)
            {
                this.Owner = Application.Current.MainWindow;
            }
            this.Show();
        }

        public NewsArticle Model
        {
            get { return DataContext as NewsArticle; }
            set { DataContext = value; }
        }
    }
}
