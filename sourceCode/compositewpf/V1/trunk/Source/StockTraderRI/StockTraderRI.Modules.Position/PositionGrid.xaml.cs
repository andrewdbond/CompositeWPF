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
using Microsoft.Practices.Composite.Events;
using StockTraderRI.Modules.Position.PositionSummary;

namespace StockTraderRI.Modules.Position
{
    /// <summary>
    /// Interaction logic for PositionGrid.xaml
    /// </summary>
    public partial class PositionGrid : UserControl
    {
        public event EventHandler<DataEventArgs<string>> PositionSelected = delegate { };

        public PositionGrid()
        {
            InitializeComponent();
        }

        private void _positionTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO: Add Selected value binding to presentation model to remove code behind
            if (e.AddedItems.Count > 0)
            {
                PositionSummaryItem currentPosition = e.AddedItems[0] as PositionSummaryItem;
                if (currentPosition != null)
                {
                    PositionSelected(this, new DataEventArgs<string>(currentPosition.TickerSymbol));
                }
            }
        }

    }
}
