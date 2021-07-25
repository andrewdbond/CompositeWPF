// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Windows.Controls;
using ViewSwitchingNavigation.Email.ViewModels;

namespace ViewSwitchingNavigation.Email.Views
{
    [Export("EmailView")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class EmailView : UserControl
    {
        public EmailView()
        {
            InitializeComponent();
        }

        [Import]
        public EmailViewModel ViewModel
        {
            set { this.DataContext = value; }
        }
    }
}
