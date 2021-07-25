// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Modularity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.Composition.Tests.Modularity
{
    [TestClass]
    public class ModuleAttributeFixture
    {
        [TestMethod]
        public void StartupLoadedDefaultsToTrue()
        {
            var moduleAttribute = new ModuleAttribute();

            Assert.AreEqual(false, moduleAttribute.OnDemand);
        }

        [TestMethod]
        public void CanGetAndSetProperties()
        {
            var moduleAttribute = new ModuleAttribute();
            moduleAttribute.ModuleName = "Test";
            moduleAttribute.OnDemand = true;

            Assert.AreEqual("Test", moduleAttribute.ModuleName);
            Assert.AreEqual(true, moduleAttribute.OnDemand);
        }

        [TestMethod]
        public void ModuleDependencyAttributeStoresModuleName()
        {
            var moduleDependencyAttribute = new ModuleDependencyAttribute("Test");

            Assert.AreEqual("Test", moduleDependencyAttribute.ModuleName);
        }
    }
}