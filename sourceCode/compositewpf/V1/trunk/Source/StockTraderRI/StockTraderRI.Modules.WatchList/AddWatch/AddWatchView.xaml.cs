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
using System.Windows.Input;

namespace StockTraderRI.Modules.Watch.AddWatch
{
    /// <summary>
    /// Interaction logic for AddWatchControl.xaml
    /// </summary>
    public partial class AddWatchView : UserControl, IAddWatchView
    {
        public AddWatchView()
        {
            InitializeComponent();
        }

        #region IAddWatchView Members

        public void SetAddWatchCommand(ICommand addWatchCommand)
        {
            this.DataContext = addWatchCommand;
        }

        #endregion
    }
}
