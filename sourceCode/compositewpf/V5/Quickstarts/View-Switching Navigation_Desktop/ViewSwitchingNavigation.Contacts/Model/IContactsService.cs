// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace ViewSwitchingNavigation.Contacts.Model
{
    public interface IContactsService
    {
        IAsyncResult BeginGetContacts(AsyncCallback callback, object userState);
        IEnumerable<Contact> EndGetContacts(IAsyncResult asyncResult);
    }
}
