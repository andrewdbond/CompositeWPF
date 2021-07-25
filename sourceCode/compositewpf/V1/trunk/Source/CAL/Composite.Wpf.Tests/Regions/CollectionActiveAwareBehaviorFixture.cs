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
using System.Collections.ObjectModel;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Wpf.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Wpf.Tests.Regions
{
    [TestClass]
    public class CollectionActiveAwareBehaviorFixture
    {
        [TestMethod]
        public void SetsIsActivePropertyOnIActiveAwareObjects()
        {
            var collection = new ObservableCollection<object>();
            var behavior = new CollectionActiveAwareBehavior(collection);
            behavior.Attach();

            ActiveAwareObject activeAwareObject = new ActiveAwareObject();

            Assert.IsFalse(activeAwareObject.IsActive);
            collection.Add(activeAwareObject);

            Assert.IsTrue(activeAwareObject.IsActive);

            collection.Remove(activeAwareObject);
            Assert.IsFalse(activeAwareObject.IsActive);
        }

        [TestMethod]
        public void DetachStopsListeningForChanges()
        {
            var collection = new ObservableCollection<object>();
            var behavior = new CollectionActiveAwareBehavior(collection);
            behavior.Attach();
            behavior.Detach();
            ActiveAwareObject activeAwareObject = new ActiveAwareObject();

            collection.Add(activeAwareObject);

            Assert.IsFalse(activeAwareObject.IsActive);
        }

        [TestMethod]
        public void DoesNotThrowWhenAddingNonActiveAwareObjects()
        {
            var collection = new ObservableCollection<object>();
            var behavior = new CollectionActiveAwareBehavior(collection);
            behavior.Attach();

            collection.Add(new object());
        }

        class ActiveAwareObject : IActiveAware
        {
            public bool IsActive { get; set; }
            public event EventHandler IsActiveChanged;
        }
    }
}
