// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace ViewSwitchingNavigation.Calendar.Model
{
    public interface ICalendarService
    {
        IAsyncResult BeginGetMeetings(AsyncCallback callback, object userState);
        IEnumerable<Meeting> EndGetMeetings(IAsyncResult result);
    }
}
