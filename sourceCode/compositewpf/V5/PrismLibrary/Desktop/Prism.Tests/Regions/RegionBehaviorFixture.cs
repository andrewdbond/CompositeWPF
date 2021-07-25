// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Composition.Tests.Mocks;
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.Composition.Tests.Regions
{
    [TestClass]
    public class RegionBehaviorFixture
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotChangeRegionAfterAttach()
        {
            TestableRegionBehavior regionBehavior = new TestableRegionBehavior();

            regionBehavior.Region = new MockPresentationRegion();

            regionBehavior.Attach();
            regionBehavior.Region = new MockPresentationRegion();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldFailWhenAttachedWithoutRegion()
        {
            TestableRegionBehavior regionBehavior = new TestableRegionBehavior();
            regionBehavior.Attach();
        }

        [TestMethod]
        public void ShouldCallOnAttachWhenAttachMethodIsInvoked()
        {
            TestableRegionBehavior regionBehavior = new TestableRegionBehavior();

            regionBehavior.Region = new MockPresentationRegion();

            regionBehavior.Attach();

            Assert.IsTrue(regionBehavior.onAttachCalled);
        }

        private class TestableRegionBehavior : RegionBehavior
        {
            public bool onAttachCalled;

            protected override void OnAttach()
            {
                onAttachCalled = true;
            }
        }
    }

    
}
