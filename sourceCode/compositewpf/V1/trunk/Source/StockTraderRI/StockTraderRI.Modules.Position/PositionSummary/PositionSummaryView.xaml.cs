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
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Events;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Position.Interfaces;

namespace StockTraderRI.Modules.Position.PositionSummary
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PositionSummaryView : UserControl, IPositionSummaryView
    {
        IPositionSummaryPresentationModel _model;

        public PositionSummaryView()
        {
            InitializeComponent();

            positionGrid.PositionSelected += positionGrid_PositionSelected;
        }

        void positionGrid_PositionSelected(object sender, DataEventArgs<string> e)
        {
            TickerSymbolSelected(this, e);
        }

        #region IPositionSummaryView Members

        public event EventHandler<DataEventArgs<string>> TickerSymbolSelected = delegate { };

        public IPositionSummaryPresentationModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                pieChart.ItemsSource = _model.PositionSummaryItems;
                DataContext = _model;
            }
        }

        public void ShowTrendLine(ITrendLineView view)
        {
            lineChartPanel.Children.Clear();
            lineChartPanel.Children.Add((UIElement)view);
        }

        #endregion
    }
}
