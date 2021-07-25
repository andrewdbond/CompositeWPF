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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Regions;
using Microsoft.Practices.Composite.Wpf.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Wpf.Tests.Regions
{
    [TestClass]
    public class SelectorRegionAdapterFixture
    {
        [TestMethod]
        public void AdapterAssociatesSelectorWithRegion()
        {
            var control = new TabControl();
            IRegionAdapter adapter = new TestableSelectorRegionAdapter();

            IRegion region = adapter.Initialize(control);
            Assert.IsNotNull(region);

            Assert.AreSame(control.ItemsSource, region.Views);
        }

        [TestMethod]
        public void ShouldMoveAlreadyExistingContentInControlToRegion()
        {
            var control = new TabControl();
            var view = new object();
            control.Items.Add(view);
            IRegionAdapter adapter = new TestableSelectorRegionAdapter();

            var region = (MockRegion)adapter.Initialize(control);

            Assert.AreEqual(1, region.MockViews.Count());
            Assert.AreSame(view, region.MockViews.ElementAt(0));
            Assert.AreSame(view, control.Items[0]);
        }

        [TestMethod]
        public void ControlWithExistingItemSourceThrows()
        {
            TabControl tabControl = new TabControl() { ItemsSource = new List<string>() };

            IRegionAdapter adapter = new TestableSelectorRegionAdapter();

            try
            {
                var region = (MockRegion)adapter.Initialize(tabControl);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                StringAssert.Contains(ex.Message, "ItemsControl's ItemsSource property is not empty.");
            }
        }

        [TestMethod]
        public void ControlWithExistingBindingOnItemsSourceWithNullValueThrows()
        {
            TabControl tabControl = new TabControl();
            Binding binding = new Binding("Enumerable");
            binding.Source = new SimpleModel() { Enumerable = null };
            BindingOperations.SetBinding(tabControl, TabControl.ItemsSourceProperty, binding);

            IRegionAdapter adapter = new TestableSelectorRegionAdapter();

            try
            {
                var region = (MockRegion)adapter.Initialize(tabControl);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                StringAssert.Contains(ex.Message, "ItemsControl's ItemsSource property is not empty.");
            }
        }

        [TestMethod]
        public void AdapterSynchronizesSelectorSelectionWithRegion()
        {
            TabControl tabControl = new TabControl();
            object model1 = new object();
            object model2 = new object();
            IRegionAdapter adapter = new SelectorRegionAdapter();

            var region = adapter.Initialize(tabControl);
            region.Add(model1);
            region.Add(model2);

            Assert.IsFalse(region.ActiveViews.Contains(model2));

            tabControl.SelectedItem = model2;

            Assert.IsTrue(region.ActiveViews.Contains(model2));
            Assert.IsFalse(region.ActiveViews.Contains(model1));

            tabControl.SelectedItem = model1;

            Assert.IsTrue(region.ActiveViews.Contains(model1));
            Assert.IsFalse(region.ActiveViews.Contains(model2));
        }

        [TestMethod]
        public void AdapterDoesNotPreventRegionFromBeingGarbageCollected()
        {
            TabControl tabControl = new TabControl();
            object model = new object();
            IRegionAdapter adapter = new SelectorRegionAdapter();

            var region = adapter.Initialize(tabControl);
            region.Add(model);

            WeakReference regionWeakReference = new WeakReference(region);
            WeakReference controlWeakReference = new WeakReference(tabControl);
            Assert.IsTrue(regionWeakReference.IsAlive);
            Assert.IsTrue(controlWeakReference.IsAlive);

            region = null;
            tabControl = null;
            GC.Collect();
            GC.Collect();

            Assert.IsFalse(regionWeakReference.IsAlive);
            Assert.IsFalse(controlWeakReference.IsAlive);
        }

        [TestMethod]
        public void AdapterDoesNotPreventRegionFromBeingGarbageCollectedWhenSettingItemsSourceToNull()
        {
            TabControl tabControl = new TabControl();
            object model = new object();
            IRegionAdapter adapter = new SelectorRegionAdapter();

            var region = adapter.Initialize(tabControl);
            region.Add(model);

            WeakReference regionWeakReference = new WeakReference(region);
            Assert.IsTrue(regionWeakReference.IsAlive);

            tabControl.ItemsSource = null;
            region = null;
            GC.Collect();
            GC.Collect();

            Assert.IsFalse(regionWeakReference.IsAlive);
        }

        [TestMethod]
        public void AdapterDoesNotPreventControlFromBeingGarbageCollectedWhenSettingItemsSourceToNull()
        {
            TabControl tabControl = new TabControl();
            object model = new object();
            IRegionAdapter adapter = new SelectorRegionAdapter();

            var region = adapter.Initialize(tabControl);
            region.Add(model);

            WeakReference controlWeakReference = new WeakReference(tabControl);
            Assert.IsTrue(controlWeakReference.IsAlive);

            tabControl.ItemsSource = null;
            tabControl = null;
            GC.Collect();
            GC.Collect();

            Assert.IsFalse(controlWeakReference.IsAlive);
        }

        [TestMethod]
        public void RegionDoesNotGetUpdatedIfTheItemsSourceWasManuallyChanged()
        {
            TabControl tabControl = new TabControl();
            object model = new object();
            IRegionAdapter adapter = new SelectorRegionAdapter();

            var region = adapter.Initialize(tabControl);
            region.Add(model);

            tabControl.ItemsSource = null;
            tabControl.Items.Add(model);
            Assert.IsFalse(region.ActiveViews.Contains(model));

            tabControl.SelectedItem = model;

            Assert.IsFalse(region.ActiveViews.Contains(model), "The region should not be updated");
        }

        [TestMethod]
        public void ActivatingTheViewShouldUpdateTheSelectedItem()
        {
            TabControl tabControl = new TabControl();
            var view1 = new object();
            var view2 = new object();

            IRegionAdapter adapter = new SelectorRegionAdapter();

            var region = adapter.Initialize(tabControl);
            region.Add(view1);
            region.Add(view2);

            Assert.AreNotEqual(view1, tabControl.SelectedItem);

            region.Activate(view1);

            Assert.AreEqual(view1, tabControl.SelectedItem);

            region.Activate(view2);

            Assert.AreEqual(view2, tabControl.SelectedItem);
        }

        [TestMethod]
        public void DeactivatingTheSelectedViewShouldUpdateTheSelectedItem()
        {
            TabControl tabControl = new TabControl();
            var view1 = new object();
            IRegionAdapter adapter = new SelectorRegionAdapter();
            var region = adapter.Initialize(tabControl);
            region.Add(view1);

            region.Activate(view1);

            Assert.AreEqual(view1, tabControl.SelectedItem);

            region.Deactivate(view1);

            Assert.AreNotEqual(view1, tabControl.SelectedItem);
        }

        class SimpleModel
        {
            public IEnumerable Enumerable { get; set; }
        }

        internal class TestableSelectorRegionAdapter : SelectorRegionAdapter
        {
            private MockRegion region = new MockRegion();

            protected override IRegion CreateRegion()
            {
                return region;
            }
        }
    }
}