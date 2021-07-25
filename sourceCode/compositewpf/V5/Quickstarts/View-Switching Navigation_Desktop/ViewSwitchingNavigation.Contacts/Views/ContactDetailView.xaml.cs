// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace ViewSwitchingNavigation.Contacts.Views
{
    [Export("ContactDetailView")]
    public partial class ContactDetailView : UserControl
    {
        public ContactDetailView()
        {
            InitializeComponent();
        }
    }
}
