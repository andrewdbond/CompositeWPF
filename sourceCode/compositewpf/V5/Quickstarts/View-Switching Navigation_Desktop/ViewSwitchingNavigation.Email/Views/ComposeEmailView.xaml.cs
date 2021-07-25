// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Windows.Controls;
using ViewSwitchingNavigation.Email.ViewModels;

namespace ViewSwitchingNavigation.Email.Views
{
    [Export("ComposeEmailView")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ComposeEmailView : UserControl
    {
        public ComposeEmailView()
        {
            InitializeComponent();
        }

        [Import]
        public ComposeEmailViewModel ViewModel
        {
            set { this.DataContext = value; }
        }
    }
}
