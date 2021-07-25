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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Wpf.Tests.Mocks;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Wpf.Tests.Regions
{
    [TestClass]
    public class RegionManagerFixture
    {
        [TestMethod]
        public void CanAddRegion()
        {
            IRegion region1 = new MockRegion();

            RegionManager regionManager = new RegionManager();
            regionManager.Regions.Add("MainRegion", region1);

            IRegion region2 = regionManager.Regions["MainRegion"];
            Assert.AreSame(region1, region2);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ShouldFailIfRegionDoesntExists()
        {
            RegionManager regionManager = new RegionManager();
            IRegion region = regionManager.Regions["nonExistentRegion"];
        }

        [TestMethod]
        public void CanCheckTheExistenceOfARegion()
        {
            RegionManager regionManager = new RegionManager();
            bool result = regionManager.Regions.ContainsKey("noRegion");

            Assert.IsFalse(result);

            IRegion region = new MockRegion();

            regionManager.Regions.Add("noRegion", region);

            result = regionManager.Regions.ContainsKey("noRegion");

            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddingMultipleRegionsWithSameNameThrowsArgumentException()
        {
            var regionManager = new RegionManager();
            regionManager.Regions.Add("region name", new MockRegion());
            regionManager.Regions.Add("region name", new MockRegion());
        }

        [TestMethod]
        public void AddPassesItselfAsTheRegionManagerOfTheRegion()
        {
            var regionManager = new RegionManager();
            var region = new MockRegion();
            regionManager.Regions.Add("region", region);

            Assert.AreSame(regionManager, region.RegionManager);
        }

        [TestMethod]
        public void CreateRegionManagerCreatesANewInstance()
        {
            var regionManager = new RegionManager();
            var createdRegionManager = regionManager.CreateRegionManager();
            Assert.IsNotNull(createdRegionManager);
            Assert.IsInstanceOfType(createdRegionManager, typeof(RegionManager));
            Assert.AreNotSame(regionManager, createdRegionManager);
        }

        [TestMethod]
        public void ShouldAttachNewRegionByUsingRegisteredRegionAdapterMappings()
        {
            var mappings = new RegionAdapterMappings();
            var mockRegionAdapter = new MockRegionAdapter();
            mappings.RegisterMapping(typeof(DependencyObject), mockRegionAdapter);
            var regionManager = new RegionManager(mappings);
            var control = new ContentControl();

            regionManager.AttachNewRegion(control, "TestRegionName");

            Assert.IsTrue(mockRegionAdapter.InitializeCalled);
            Assert.AreEqual(control, mockRegionAdapter.InitializeArgument);
        }

        [TestMethod]
        public void CanRemoveRegion()
        {
            var regionManager = new RegionManager();
            IRegion region = new MockRegion();
            regionManager.Regions.Add("TestRegion", region);

            regionManager.Regions.Remove("TestRegion");

            Assert.IsFalse(regionManager.Regions.ContainsKey("TestRegion"));
        }

        [TestMethod]
        public void ShouldRemoveRegionManagerWhenRemoving()
        {
            var regionManager = new RegionManager();
            var region = new MockRegion();
            regionManager.Regions.Add("TestRegion", region);

            regionManager.Regions.Remove("TestRegion");

            Assert.IsNull(region.RegionManager);
        }

        //TODO: Fix failing test
        [TestMethod, Ignore]
        public void RegionGetsAddedInRegionManagerWhenAddedIntoAScope()
        {
            var mappings = new RegionAdapterMappings();
            mappings.RegisterMapping(typeof(DependencyObject), new MockRegionAdapter());

            RegionManager regionManager = new RegionManager(mappings);
            var regionScopeControl = new ContentControl();
            RegionManager.SetRegionManager(regionScopeControl, regionManager);

            var control = new ContentControl();
            control.SetValue(RegionManager.RegionNameProperty, "TestRegion");

            Assert.IsFalse(regionManager.Regions.ContainsKey("TestRegion"));
            regionScopeControl.Content = control;
            Assert.IsTrue(regionManager.Regions.ContainsKey("TestRegion"));
            Assert.IsNotNull(regionManager.Regions["TestRegion"]);
        }

        //TODO: Fix failing test
        [TestMethod, Ignore]
        public void RegionGetsRemovedFromRegionManagerWhenRemovedFromScope()
        {
            var mappings = new RegionAdapterMappings();
            mappings.RegisterMapping(typeof(DependencyObject), new MockRegionAdapter());

            RegionManager regionManager = new RegionManager(mappings);
            var regionScopeControl = new ContentControl();
            RegionManager.SetRegionManager(regionScopeControl, regionManager);
            var control = new ItemsControl();
            control.SetValue(RegionManager.RegionNameProperty, "TestRegion");
            regionScopeControl.Content = control;
            Assert.IsTrue(regionManager.Regions.ContainsKey("TestRegion"));

            regionScopeControl.Content = null;

            Assert.IsFalse(regionManager.Regions.ContainsKey("TestRegion"));
        }

        /*
         * TODO: AttachNewRegion should throw if mappings is null
         */
    }
}