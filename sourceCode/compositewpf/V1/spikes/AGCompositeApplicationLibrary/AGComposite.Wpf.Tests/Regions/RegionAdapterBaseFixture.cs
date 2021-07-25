//===============================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
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
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Regions;
using Microsoft.Practices.Composite.Wpf.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Wpf.Tests.Regions
{
    [TestClass]
    public class RegionAdapterBaseFixture
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IncorrectTypeThrows()
        {
            IRegionAdapter adapter = new TestableRegionAdapterBase();
            adapter.Initialize(new ContentControl());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullObjectThrows()
        {
            IRegionAdapter adapter = new TestableRegionAdapterBase();
            adapter.Initialize(null);
        }


        [TestMethod]
        public void CreateRegionReturnValueIsPassedToAdapt()
        {
            var regionTarget = new MockRegionTarget();
            var adapter = new TestableRegionAdapterBase();

            adapter.Initialize(regionTarget);

            Assert.AreSame(adapter.CreateRegionReturnValue, adapter.AdaptArgumentRegion);
            Assert.AreSame(regionTarget, adapter.adaptArgumentRegionTarget);
        }

        [TestMethod]
        public void CreateRegionReturnValueIsPassedToAttachBehaviors()
        {
            var regionTarget = new MockRegionTarget();
            var adapter = new TestableRegionAdapterBase();

            adapter.Initialize(regionTarget);

            Assert.AreSame(adapter.CreateRegionReturnValue, adapter.AttachBehaviorsArgumentRegion);
            Assert.AreSame(regionTarget, adapter.attachBehaviorsArgumentTargetToAdapt);
        }

        [TestMethod]
        public void AttachesCollectionActiveAwareBehaviorToActiveViews()
        {
            var objectToAdapt = new MockRegionTarget();
            var adapter = new TestableRegionAdapterBase();
            var region = (MockRegion)adapter.Initialize(objectToAdapt);
            var activeAwareObject = new ActiveAwareObject();
            Assert.IsFalse(activeAwareObject.IsActive);

            region.MockActiveViews.Items.Add(activeAwareObject);

            Assert.IsTrue(activeAwareObject.IsActive);

            region.MockActiveViews.Items.Remove(activeAwareObject);

            Assert.IsFalse(activeAwareObject.IsActive);
        }

        class ActiveAwareObject : IActiveAware
        {
            public bool IsActive { get; set; }
            public event EventHandler IsActiveChanged;
        }

        class TestableRegionAdapterBase : RegionAdapterBase<MockRegionTarget>
        {
            public IRegion CreateRegionReturnValue = new MockRegion();
            public IRegion AdaptArgumentRegion;
            public MockRegionTarget adaptArgumentRegionTarget;
            public IRegion AttachBehaviorsArgumentRegion;
            public MockRegionTarget attachBehaviorsArgumentTargetToAdapt;

            protected override void Adapt(IRegion region, MockRegionTarget regionTarget)
            {
                AdaptArgumentRegion = region;
                adaptArgumentRegionTarget = regionTarget;
            }

            protected override IRegion CreateRegion()
            {
                return CreateRegionReturnValue;
            }

            protected override void AttachBehaviors(IRegion region, MockRegionTarget regionTarget)
            {
                AttachBehaviorsArgumentRegion = region;
                attachBehaviorsArgumentTargetToAdapt = regionTarget;
                base.AttachBehaviors(region, regionTarget);
            }
        }

        class MockRegionTarget
        {
        }
    }


}
