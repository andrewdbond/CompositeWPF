// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.Practices.Prism.Composition.Tests.Mocks;
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.Composition.Tests.Regions
{
    [TestClass]
    public class RegionBehaviorFactoryFixture
    {
        [TestMethod]
        public void CanRegisterType()
        {
            RegionBehaviorFactory factory = new RegionBehaviorFactory(null);

            factory.AddIfMissing("key1", typeof(MockRegionBehavior));
            factory.AddIfMissing("key2", typeof(MockRegionBehavior));

            Assert.AreEqual(2, factory.Count());
            Assert.IsTrue(factory.ContainsKey("key1"));
            
        }

        [TestMethod]
        public void WillNotAddTypesWithDuplicateKeys()
        {
            RegionBehaviorFactory factory = new RegionBehaviorFactory(null);

            factory.AddIfMissing("key1", typeof(MockRegionBehavior));
            factory.AddIfMissing("key1", typeof(MockRegionBehavior));

            Assert.AreEqual(1, factory.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTypeThatDoesNotInheritFromIRegionBehaviorThrows()
        {
            RegionBehaviorFactory factory = new RegionBehaviorFactory(null);

            factory.AddIfMissing("key1", typeof(object));
        }

        [TestMethod]
        public void CanCreateRegisteredType()
        {
            var expectedBehavior = new MockRegionBehavior();

            RegionBehaviorFactory factory = new RegionBehaviorFactory(new MockServiceLocator() { GetInstance = (t) => expectedBehavior });

            factory.AddIfMissing("key1", typeof(MockRegionBehavior));
            var behavior = factory.CreateFromKey("key1");

            Assert.AreSame(expectedBehavior, behavior);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateWithUnknownKeyThrows()
        {
            RegionBehaviorFactory factory = new RegionBehaviorFactory(null);

            factory.CreateFromKey("Key1");
        }

    }
}
