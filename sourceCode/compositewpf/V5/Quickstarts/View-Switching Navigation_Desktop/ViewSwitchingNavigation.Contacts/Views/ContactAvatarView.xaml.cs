// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace ViewSwitchingNavigation.Contacts.Views
{
    [Export("ContactAvatarView")]
    public partial class ContactAvatarView : UserControl
    {
        public ContactAvatarView()
        {
            InitializeComponent();
        }
    }
}
