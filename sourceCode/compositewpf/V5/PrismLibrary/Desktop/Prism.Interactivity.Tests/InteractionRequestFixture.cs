// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.Interactivity.Tests
{
    [TestClass]
    public class InteractionRequestFixture
    {
        [TestMethod]
        public void WhenANotificationIsRequested_ThenTheEventIsRaisedWithTheSuppliedContext()
        {
            var request = new InteractionRequest<Notification>();
            object suppliedContext = null;
            request.Raised += (o, e) => suppliedContext = e.Context;

            var context = new Notification();

            request.Raise(context, c => { });

            Assert.AreSame(context, suppliedContext);
        }

        [TestMethod]
        public void WhenTheEventCallbackIsExecuted_ThenTheNotifyCallbackIsInvokedWithTheSuppliedContext()
        {
            var request = new InteractionRequest<Notification>();
            Action eventCallback = null;
            request.Raised += (o, e) => eventCallback = e.Callback;

            var context = new Notification();
            object suppliedContext = null;

            request.Raise(context, c => { suppliedContext = c; });

            eventCallback();

            Assert.AreSame(context, suppliedContext);
        }
    }
}
