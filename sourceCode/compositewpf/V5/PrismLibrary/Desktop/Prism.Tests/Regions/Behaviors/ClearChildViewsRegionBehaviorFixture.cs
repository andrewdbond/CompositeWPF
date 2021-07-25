// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.Practices.Prism.Composition.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.Composition.Tests.Regions.Behaviors
{
    [TestClass]
    public class ClearChildViewsRegionBehaviorFixture
    {
        [TestMethod]
        public void WhenClearChildViewsPropertyIsNotSet_ThenChildViewsRegionManagerIsNotCleared()
        {
            var regionManager = new MockRegionManager();

            var region = new Region();
            region.RegionManager = regionManager;

            var behavior = new ClearChildViewsRegionBehavior();
            behavior.Region = region;
            behavior.Attach();

            var childView = new MockFrameworkElement();
            region.Add(childView);

            Assert.AreEqual(regionManager, childView.GetValue(RegionManager.RegionManagerProperty));

            region.RegionManager = null;

            Assert.AreEqual(regionManager, childView.GetValue(RegionManager.RegionManagerProperty));
        }

        [TestMethod]
        public void WhenClearChildViewsPropertyIsTrue_ThenChildViewsRegionManagerIsCleared()
        {
            var regionManager = new MockRegionManager();

            var region = new Region();
            region.RegionManager = regionManager;

            var behavior = new ClearChildViewsRegionBehavior();
            behavior.Region = region;
            behavior.Attach();

            var childView = new MockFrameworkElement();
            region.Add(childView);

            ClearChildViewsRegionBehavior.SetClearChildViews(childView, true);

            Assert.AreEqual(regionManager, childView.GetValue(RegionManager.RegionManagerProperty));

            region.RegionManager = null;

            Assert.IsNull(childView.GetValue(RegionManager.RegionManagerProperty));
        }

        [TestMethod]
        public void WhenRegionManagerChangesToNotNullValue_ThenChildViewsRegionManagerIsNotCleared()
        {
            var regionManager = new MockRegionManager();

            var region = new Region();
            region.RegionManager = regionManager;

            var behavior = new ClearChildViewsRegionBehavior();
            behavior.Region = region;
            behavior.Attach();

            var childView = new MockFrameworkElement();
            region.Add(childView);

            childView.SetValue(ClearChildViewsRegionBehavior.ClearChildViewsProperty, true);

            Assert.AreEqual(regionManager, childView.GetValue(RegionManager.RegionManagerProperty));

            region.RegionManager = new MockRegionManager();

            Assert.IsNotNull(childView.GetValue(RegionManager.RegionManagerProperty));
        }
    }
}
