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

using System.Windows.Controls;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;

namespace StockTraderRI.Modules.Market.TrendLine
{
    /// <summary>
    /// Interaction logic for TrendLineView.xaml
    /// </summary>
    public partial class TrendLineView : UserControl, ITrendLineView
    {
        public TrendLineView()
        {
            InitializeComponent();
        }

        #region ITrendLineView Members

        public void UpdateLineChart(MarketHistoryCollection historyCollection)
        {
            lineChart.ItemsSource = historyCollection;

        }

        public void SetChartTitle(string chartTitle)
        {
            lineChart.Title = chartTitle;
        }

        #endregion
    }


}
