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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Wpf.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Wpf.Tests.Events
{
    [TestClass]
    public class WpfEventFixture
    {
        [TestMethod]
        public void CanSubscribeAndRaiseEvent()
        {
            TestableCompositeWpfEvent<string> compositeWpfEvent = new TestableCompositeWpfEvent<string>();
            bool published = false;
            compositeWpfEvent.Subscribe(delegate { published = true; }, ThreadOption.PublisherThread, true, delegate { return true; });
            compositeWpfEvent.Publish(null);

            Assert.IsTrue(published);
        }

        [TestMethod]
        public void CanSubscribeAndRaiseCustomEvent()
        {
            var customEvent = new TestableCompositeWpfEvent<Payload>();
            Payload payload = new Payload();
            Payload received = null;
            customEvent.Subscribe(delegate(Payload args) { received = args; });

            customEvent.Publish(payload);

            Assert.AreSame(received, payload);
        }

        [TestMethod]
        public void CanHaveMultipleSubscribersAndRaiseCustomEvent()
        {
            var customEvent = new TestableCompositeWpfEvent<Payload>();
            Payload payload = new Payload();
            Payload received1 = null;
            Payload received2 = null;
            customEvent.Subscribe(delegate(Payload args) { received1 = args; });
            customEvent.Subscribe(delegate(Payload args) { received2 = args; });

            customEvent.Publish(payload);

            Assert.AreSame(received1, payload);
            Assert.AreSame(received2, payload);
        }

        [TestMethod]
        public void SubscriberReceivesNotificationOnDispatcherThread()
        {
            TestableCompositeWpfEvent<string> compositeWpfEvent = new TestableCompositeWpfEvent<string>();
            int threadId = -1;
            int calledThreadId = -1;
            ManualResetEvent setupEvent = new ManualResetEvent(false);
            bool completed = false;
            Thread mockUIThread = new Thread(delegate()
                                                 {
                                                     threadId = Thread.CurrentThread.ManagedThreadId;
                                                     compositeWpfEvent.SettableUIDispatcher = Dispatcher.CurrentDispatcher;
                                                     setupEvent.Set();

                                                     while (!completed)
                                                     {
                                                         WPFThreadHelper.DoEvents();
                                                     }
                                                 }
                );
            mockUIThread.Start();
            string receivedPayload = null;
            setupEvent.WaitOne(5000, false);
            compositeWpfEvent.Subscribe(delegate(string args)
                                     {
                                         calledThreadId = Thread.CurrentThread.ManagedThreadId;
                                         receivedPayload = args;
                                         completed = true;
                                     }, ThreadOption.UIThread, true);

            compositeWpfEvent.Publish("Test Payload");

            bool joined = mockUIThread.Join(5000);
            completed = true;
            Assert.IsTrue(joined);

            Assert.AreEqual(threadId, calledThreadId);
            Assert.AreSame("Test Payload", receivedPayload);
        }



        [TestMethod]
        public void SubscriberReceivesNotificationOnDifferentThread()
        {
            TestableCompositeWpfEvent<string> compositeWpfEvent = new TestableCompositeWpfEvent<string>();
            int calledThreadId = -1;
            ManualResetEvent completeEvent = new ManualResetEvent(false);
            compositeWpfEvent.Subscribe(delegate
                                     {
                                         calledThreadId = Thread.CurrentThread.ManagedThreadId;
                                         completeEvent.Set();
                                     }, ThreadOption.BackgroundThread);

            compositeWpfEvent.Publish(null);
            completeEvent.WaitOne(5000, false);
            Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, calledThreadId);
        }

        [TestMethod]
        public void PayloadGetPassedInBackgroundHandler()
        {
            var customEvent = new TestableCompositeWpfEvent<Payload>();
            Payload payload = new Payload();
            ManualResetEvent backgroundWait = new ManualResetEvent(false);

            Payload backGroundThreadReceived = null;
            customEvent.Subscribe(delegate(Payload passedPayload)
                                      {
                                          backGroundThreadReceived = passedPayload;
                                          backgroundWait.Set();
                                      }, ThreadOption.BackgroundThread, true);

            customEvent.Publish(payload);
            bool eventSet = backgroundWait.WaitOne(5000, false);
            Assert.IsTrue(eventSet);
            Assert.AreSame(backGroundThreadReceived, payload);
        }

        [TestMethod]
        public void SubscribeTakesExecuteDelegateThreadOptionAndFilter()
        {
            TestableCompositeWpfEvent<string> compositeWpfEvent = new TestableCompositeWpfEvent<string>();
            string receivedValue = null;
            compositeWpfEvent.Subscribe(delegate(string value) { receivedValue = value; });

            compositeWpfEvent.Publish("test");

            Assert.AreEqual("test", receivedValue);

        }

        [TestMethod]
        public void FilterEnablesActionTarget()
        {
            TestableCompositeWpfEvent<string> compositeWpfEvent = new TestableCompositeWpfEvent<string>();
            bool goodFilterPublished = false;
            bool badFilterPublished = false;
            compositeWpfEvent.Subscribe(delegate { goodFilterPublished = true; }, ThreadOption.PublisherThread, true, delegate { return true; });
            compositeWpfEvent.Subscribe(delegate { badFilterPublished = true; }, ThreadOption.PublisherThread, true, delegate { return false; });

            compositeWpfEvent.Publish("test");

            Assert.IsTrue(goodFilterPublished);
            Assert.IsFalse(badFilterPublished);

        }

        [TestMethod]
        public void SubscribeDefaultsThreadOptionAndNoFilter()
        {
            TestableCompositeWpfEvent<string> compositeWpfEvent = new TestableCompositeWpfEvent<string>();
            int calledThreadID = -1;

            compositeWpfEvent.Subscribe(delegate { calledThreadID = Thread.CurrentThread.ManagedThreadId; });

            compositeWpfEvent.Publish("test");

            Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, calledThreadID);
        }

        [TestMethod]
        public void ShouldUnsubscribeFromPublisherThread()
        {
            var compositeWpfEvent = new TestableCompositeWpfEvent<string>();

            Action<string> actionEvent = delegate(string args) { };
            compositeWpfEvent.Subscribe(
                actionEvent,
                ThreadOption.PublisherThread);

            Assert.IsTrue(compositeWpfEvent.Contains(actionEvent));
            compositeWpfEvent.Unsubscribe(actionEvent);
            Assert.IsFalse(compositeWpfEvent.Contains(actionEvent));
        }

        [TestMethod]
        public void UnsubscribeShouldNotFailWithNonSubscriber()
        {
            TestableCompositeWpfEvent<string> compositeWpfEvent = new TestableCompositeWpfEvent<string>();

            Action<string> subscriber = delegate { };
            compositeWpfEvent.Unsubscribe(subscriber);
        }

        [TestMethod]
        public void ShouldUnsubscribeFromBackgroundThread()
        {
            var compositeWpfEvent = new TestableCompositeWpfEvent<string>();

            Action<string> actionEvent = delegate(string args) { };
            compositeWpfEvent.Subscribe(
                actionEvent,
                ThreadOption.BackgroundThread);

            Assert.IsTrue(compositeWpfEvent.Contains(actionEvent));
            compositeWpfEvent.Unsubscribe(actionEvent);
            Assert.IsFalse(compositeWpfEvent.Contains(actionEvent));
        }

        [TestMethod]
        public void ShouldUnsubscribeFromUIThread()
        {
            var compositeWpfEvent = new TestableCompositeWpfEvent<string>();

            Action<string> actionEvent = delegate(string args) { };
            compositeWpfEvent.Subscribe(
                actionEvent,
                ThreadOption.UIThread);

            Assert.IsTrue(compositeWpfEvent.Contains(actionEvent));
            compositeWpfEvent.Unsubscribe(actionEvent);
            Assert.IsFalse(compositeWpfEvent.Contains(actionEvent));
        }

        [TestMethod]
        public void ShouldUnsubscribeASingleDelegate()
        {
            var compositeWpfEvent = new TestableCompositeWpfEvent<string>();

            int callCount = 0;

            Action<string> actionEvent = delegate { callCount++; };
            compositeWpfEvent.Subscribe(actionEvent);
            compositeWpfEvent.Subscribe(actionEvent);

            compositeWpfEvent.Publish(null);
            Assert.AreEqual<int>(2, callCount);

            callCount = 0;
            compositeWpfEvent.Unsubscribe(actionEvent);
            compositeWpfEvent.Publish(null);
            Assert.AreEqual<int>(1, callCount);
        }

        [TestMethod]
        public void ShouldNotExecuteOnGarbageCollectedDelegateReferenceWhenNotKeepAlive()
        {
            var compositeWpfEvent = new TestableCompositeWpfEvent<string>();

            ExternalAction externalAction = new ExternalAction();
            compositeWpfEvent.Subscribe(externalAction.ExecuteAction);

            compositeWpfEvent.Publish("testPayload");
            Assert.AreEqual("testPayload", externalAction.PassedValue);

            WeakReference actionEventReference = new WeakReference(externalAction);
            externalAction = null;
            GC.Collect();
            Assert.IsFalse(actionEventReference.IsAlive);

            compositeWpfEvent.Publish("testPayload");
        }

        [TestMethod]
        public void ShouldNotExecuteOnGarbageCollectedFilterReferenceWhenNotKeepAlive()
        {
            var compositeWpfEvent = new TestableCompositeWpfEvent<string>();

            bool wasCalled = false;
            Action<string> actionEvent = delegate { wasCalled = true; };

            ExternalFilter filter = new ExternalFilter();
            compositeWpfEvent.Subscribe(actionEvent, ThreadOption.PublisherThread, false, filter.AlwaysTrueFilter);

            compositeWpfEvent.Publish("testPayload");
            Assert.IsTrue(wasCalled);

            wasCalled = false;
            WeakReference filterReference = new WeakReference(filter);
            filter = null;
            GC.Collect();
            Assert.IsFalse(filterReference.IsAlive);

            compositeWpfEvent.Publish("testPayload");
            Assert.IsFalse(wasCalled);
        }

        [TestMethod]
        public void CanAddDescriptionWhileEventIsFiring()
        {
            var compositeWpfEvent = new TestableCompositeWpfEvent<string>();
            Action<string> emptyDelegate = delegate { };
            compositeWpfEvent.Subscribe(delegate { compositeWpfEvent.Subscribe(emptyDelegate); });

            Assert.IsFalse(compositeWpfEvent.Contains(emptyDelegate));

            compositeWpfEvent.Publish(null);

            Assert.IsTrue((compositeWpfEvent.Contains(emptyDelegate)));
        }

        [TestMethod]
        public void InlineDelegateDeclarationsDoesNotGetCollectedIncorrectlyWithWeakReferences()
        {
            var compositeWpfEvent = new TestableCompositeWpfEvent<string>();
            bool published = false;
            compositeWpfEvent.Subscribe(delegate { published = true; }, ThreadOption.PublisherThread, false, delegate { return true; });
            GC.Collect();
            compositeWpfEvent.Publish(null);

            Assert.IsTrue(published);
        }

        [TestMethod]
        public void ShouldNotGarbageCollectDelegateReferenceWhenUsingKeepAlive()
        {
            var compositeWpfEvent = new TestableCompositeWpfEvent<string>();

            var externalAction = new ExternalAction();
            compositeWpfEvent.Subscribe(externalAction.ExecuteAction, ThreadOption.PublisherThread, true);

            WeakReference actionEventReference = new WeakReference(externalAction);
            externalAction = null;
            GC.Collect();
            GC.Collect();
            Assert.IsTrue(actionEventReference.IsAlive);

            compositeWpfEvent.Publish("testPayload");

            Assert.AreEqual("testPayload", ((ExternalAction)actionEventReference.Target).PassedValue);
        }

        [TestMethod]
        public void RegisterReturnsTokenThatCanBeUsedToUnsubscribe()
        {
            var compositeWpfEvent = new TestableCompositeWpfEvent<string>();
            Action<string> action = delegate { };

            var token = compositeWpfEvent.Subscribe(action);
            compositeWpfEvent.Unsubscribe(token);

            Assert.IsFalse(compositeWpfEvent.Contains(action));
        }

        [TestMethod]
        public void ContainsShouldSearchByToken()
        {
            var compositeWpfEvent = new TestableCompositeWpfEvent<string>();
            Action<string> action = delegate { };
            var token = compositeWpfEvent.Subscribe(action);

            Assert.IsTrue(compositeWpfEvent.Contains(token));

            compositeWpfEvent.Unsubscribe(action);
            Assert.IsFalse(compositeWpfEvent.Contains(token));
        }

        [TestMethod]
        public void SubscribeDefaultsToPublisherThread()
        {
            var compositeWpfEvent = new TestableCompositeWpfEvent<string>();
            Action<string> action = delegate { };
            var token = compositeWpfEvent.Subscribe(action, true);

            Assert.AreEqual(1, compositeWpfEvent.BaseSubscriptions.Count);
            Assert.AreEqual(typeof(EventSubscription<string>), compositeWpfEvent.BaseSubscriptions.ElementAt(0).GetType());
        }


        class ExternalFilter
        {
            public bool AlwaysTrueFilter(string value)
            {
                return true;
            }
        }

        class ExternalAction
        {
            public string PassedValue;
            public void ExecuteAction(string value)
            {
                PassedValue = value;
            }
        }

        class TestableCompositeWpfEvent<TPayload> : CompositeWpfEvent<TPayload>
        {
            private Dispatcher _uiDispatcher;
            protected override Dispatcher UIDispatcher
            {
                get
                {
                    if (_uiDispatcher == null)
                        return Dispatcher.CurrentDispatcher;
                    return _uiDispatcher;
                }
            }

            public Dispatcher SettableUIDispatcher
            {
                set { _uiDispatcher = value; }
            }

            public ICollection<IEventSubscription> BaseSubscriptions
            {
                get { return base.Subscriptions; }
            }
        }

        class Payload { }
    }
}