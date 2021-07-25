// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Composition.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.Composition.Tests.Events
{
    [TestClass]
    public class DispatcherEventSubscriptionFixture
    {
        [TestMethod]
        public void ShouldCallInvokeOnDispatcher()
        {
            DispatcherEventSubscription<object> eventSubscription = null;

            Microsoft.Practices.Prism.PubSubEvents.IDelegateReference actionDelegateReference = new MockDelegateReference()
            {
                Target = (Action<object>)(arg =>
                {
                    return;
                })
            };

            Microsoft.Practices.Prism.PubSubEvents.IDelegateReference filterDelegateReference = new MockDelegateReference
            {
                Target = (Predicate<object>)(arg => true)
            };
            var mockDispatcher = new MockDispatcher();
            eventSubscription = new DispatcherEventSubscription<object>(actionDelegateReference, filterDelegateReference, mockDispatcher);

            eventSubscription.GetExecutionStrategy().Invoke(new object[0]);

            Assert.IsTrue(mockDispatcher.InvokeCalled);
        }

        [TestMethod]
        public void ShouldPassParametersCorrectly()
        {
            Microsoft.Practices.Prism.PubSubEvents.IDelegateReference actionDelegateReference = new MockDelegateReference()
            {
                Target =
                    (Action<object>)(arg1 =>
                    {
                        return;
                    })
            };
            Microsoft.Practices.Prism.PubSubEvents.IDelegateReference filterDelegateReference = new MockDelegateReference
            {
                Target = (Predicate<object>)(arg => true)
            };

            var mockDispatcher = new MockDispatcher();
            DispatcherEventSubscription<object> eventSubscription = new DispatcherEventSubscription<object>(actionDelegateReference, filterDelegateReference, mockDispatcher);

            var executionStrategy = eventSubscription.GetExecutionStrategy();
            Assert.IsNotNull(executionStrategy);

            object argument1 = new object();

            executionStrategy.Invoke(new[] { argument1 });

            Assert.AreSame(argument1, mockDispatcher.InvokeArg);
        }
    }

    internal class MockDispatcher : IDispatcherFacade
    {
        public bool InvokeCalled;
        public object InvokeArg;

        public void BeginInvoke(Delegate method, object arg)
        {
            InvokeCalled = true;
            InvokeArg = arg;
        }
    }
}
