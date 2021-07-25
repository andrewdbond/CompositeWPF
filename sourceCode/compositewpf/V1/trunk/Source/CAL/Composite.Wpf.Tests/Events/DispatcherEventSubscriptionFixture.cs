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
using System.Threading;
using System.Windows.Threading;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Wpf.Events;
using Microsoft.Practices.Composite.Wpf.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Wpf.Tests.Events
{
    [TestClass]
    public class DispatcherEventSubscriptionFixture
    {
        [TestMethod]
        public void ShouldReceiveDelegateOnDifferentThread()
        {
            int threadId = -1;
            int calledThreadId = -1;
            ManualResetEvent setupEvent = new ManualResetEvent(false);
            bool completed = false;
            DispatcherEventSubscription<object> eventSubscription = null;

            IDelegateReference actionDelegateReference = new MockDelegateReference()
            {
                Target = (Action<object>)(arg =>
                {
                    calledThreadId = Thread.CurrentThread.ManagedThreadId;
                    completed = true;
                })
            };

            IDelegateReference filterDelegateReference = new MockDelegateReference
            {
                Target = (Predicate<object>)(arg => true)
            };

            Thread mockUIThread = new Thread(delegate()
                                                 {
                                                     threadId = Thread.CurrentThread.ManagedThreadId;
                                                     eventSubscription = new DispatcherEventSubscription<object>(actionDelegateReference, filterDelegateReference, Dispatcher.CurrentDispatcher);
                                                     setupEvent.Set();

                                                     while (!completed)
                                                     {
                                                         WPFThreadHelper.DoEvents();
                                                     }
                                                 }
                );

            mockUIThread.Start();
            setupEvent.WaitOne(5000, false);

            var executionStrategy = eventSubscription.GetExecutionStrategy();
            Assert.IsNotNull(executionStrategy);

            executionStrategy.Invoke(new object[0]);

            bool joined = mockUIThread.Join(5000);
            completed = true;
            Assert.IsTrue(joined);

            Assert.AreEqual(threadId, calledThreadId);
        }

        [TestMethod]
        public void ShouldPassParametersCorrectly()
        {
            int threadId = -1;
            int calledThreadId = -1;
            ManualResetEvent setupEvent = new ManualResetEvent(false);
            bool completed = false;
            object receivedArgument1 = null;

            IDelegateReference actionDelegateReference = new MockDelegateReference()
            {
                Target =
                    (Action<object>)(arg1 =>
                                         {
                                             calledThreadId
                                                 =
                                                 Thread
                                                     .
                                                     CurrentThread
                                                     .
                                                     ManagedThreadId;
                                             receivedArgument1
                                                 =
                                                 arg1;
                                             completed
                                                 =
                                                 true;
                                         })
            };
            IDelegateReference filterDelegateReference = new MockDelegateReference
            {
                Target = (Predicate<object>)delegate
                                           {
                                               return true;
                                           }
            };

            DispatcherEventSubscription<object> eventSubscription = null;
            Thread mockUIThread = new Thread(delegate()
                                                 {
                                                     threadId = Thread.CurrentThread.ManagedThreadId;
                                                     eventSubscription =
                                                         new DispatcherEventSubscription<object>(actionDelegateReference, filterDelegateReference, Dispatcher.CurrentDispatcher);
                                                     setupEvent.Set();

                                                     while (!completed)
                                                     {
                                                         WPFThreadHelper.DoEvents();
                                                     }
                                                 }
                );

            mockUIThread.Start();

            setupEvent.WaitOne(5000, false);


            var executionStrategy = eventSubscription.GetExecutionStrategy();
            Assert.IsNotNull(executionStrategy);

            object argument1 = new object();

            executionStrategy.Invoke(new[] { argument1 });

            bool joined = mockUIThread.Join(5000);
            completed = true;
            Assert.IsTrue(joined);

            Assert.AreEqual(threadId, calledThreadId);
            Assert.AreSame(argument1, receivedArgument1);
        }
    }
}
