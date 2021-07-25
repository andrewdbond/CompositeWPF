// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace ViewSwitchingNavigation.Email.Model
{
    public interface IEmailService
    {
        IAsyncResult BeginGetEmailDocuments(AsyncCallback callback, object userState);
        IEnumerable<EmailDocument> EndGetEmailDocuments(IAsyncResult result);

        IAsyncResult BeginSendEmailDocument(EmailDocument email, AsyncCallback callback, object userState);
        void EndSendEmailDocument(IAsyncResult result);

        EmailDocument GetEmailDocument(Guid id);
    }
}
