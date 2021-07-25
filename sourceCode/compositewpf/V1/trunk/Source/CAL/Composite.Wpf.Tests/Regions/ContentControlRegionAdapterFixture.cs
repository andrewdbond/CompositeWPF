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
    public class ContentControlRegionAdapterFixture
    {
        [TestMethod]
        public void AdapterAssociatesSelectorWithRegionActiveViews()
        {
            var control = new ContentControl();
            IRegionAdapter adapter = new TestableContentControlRegionAdapter();

            MockRegion region = (MockRegion)adapter.Initialize(control);
            Assert.IsNotNull(region);

            Assert.IsNull(control.Content);
            region.MockActiveViews.Items.Add(new object());

            Assert.IsNotNull(control.Content);
            Assert.AreSame(control.Content, region.ActiveViews.ElementAt(0));

            region.MockActiveViews.Items.Add(new object());
            Assert.AreSame(control.Content, region.ActiveViews.ElementAt(0));

            region.MockActiveViews.Items.RemoveAt(0);
            Assert.AreSame(control.Content, region.ActiveViews.ElementAt(0));

            region.MockActiveViews.Items.RemoveAt(0);
            Assert.IsNull(control.Content);
        }


        [TestMethod]
        public void ControlWithExistingContentThrows()
        {
            var control = new ContentControl() { Content = new object() };

            IRegionAdapter adapter = new TestableContentControlRegionAdapter();

            try
            {
                var region = (MockRegion)adapter.Initialize(control);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                StringAssert.Contains(ex.Message, "ContentControl's Content property is not empty.");
            }
        }

        [TestMethod]
        public void ControlWithExistingBindingOnItemsSourceWithNullValueThrows()
        {
            var control = new ContentControl();
            Binding binding = new Binding("ObjectContents");
            binding.Source = new SimpleModel() { ObjectContents = null };
            BindingOperations.SetBinding(control, ContentControl.ContentProperty, binding);

            IRegionAdapter adapter = new TestableContentControlRegionAdapter();

            try
            {
                var region = (MockRegion)adapter.Initialize(control);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                StringAssert.Contains(ex.Message, "ContentControl's Content property is not empty.");
            }
        }

        class SimpleModel
        {
            public Object ObjectContents { get; set; }
        }

        internal class TestableContentControlRegionAdapter : ContentControlRegionAdapter
        {
            private MockRegion region = new MockRegion();

            protected override IRegion CreateRegion()
            {
                return region;
            }
        }
    }
}