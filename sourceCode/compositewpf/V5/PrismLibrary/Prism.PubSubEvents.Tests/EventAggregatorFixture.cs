// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.PubSubEvents.Tests
{
    [TestClass]
    public class EventAggregatorFixture
    {
        [TestMethod]
        public void GetReturnsSingleInstancesOfSameEventType()
        {
            var eventAggregator = new EventAggregator();
            var instance1 = eventAggregator.GetEvent<MockEventBase>();
            var instance2 = eventAggregator.GetEvent<MockEventBase>();

            Assert.AreSame(instance2, instance1);
        }

        public class MockEventBase : EventBase { }
    }
}