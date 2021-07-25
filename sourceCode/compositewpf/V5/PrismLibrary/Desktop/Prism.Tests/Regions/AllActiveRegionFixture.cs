// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.Composition.Tests.Regions
{
    [TestClass]
    public class AllActiveRegionFixture
    {
        [TestMethod]
        public void AddingViewsToRegionMarksThemAsActive()
        {
            IRegion region = new AllActiveRegion();
            var view = new object();

            region.Add(view);

            Assert.IsTrue(region.ActiveViews.Contains(view));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeactivateThrows()
        {
            IRegion region = new AllActiveRegion();
            var view = new object();
            region.Add(view);

            region.Deactivate(view);
        }


    }
}