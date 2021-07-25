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
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Tests.Modularity
{
    [TestClass]
    public class ConfigurationModuleEnumeratorFixture
    {
        [TestMethod]
        public void CanInitConfigModuleEnumerator()
        {
            MockConfigurationStore store = new MockConfigurationStore();
            IModuleEnumerator enumerator = new ConfigurationModuleEnumerator(store);
            Assert.IsNotNull(enumerator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullConfigurationStoreThrows()
        {
            IModuleEnumerator enumerator = new ConfigurationModuleEnumerator(null);
        }

        [TestMethod]
        public void CanInitWithDefaultConstructor()
        {
            IModuleEnumerator enumerator = new ConfigurationModuleEnumerator();
            Assert.IsNotNull(enumerator);
        }


        [TestMethod]
        public void ShouldReturnAListOfModuleInfo()
        {
            MockConfigurationStore store = new MockConfigurationStore();
            store.Modules = new[] { new ModuleConfigurationElement(@"MocksModules\MockModuleA.dll", "Microsoft.Composite.Tests.Mocks.Modules.MockModuleA", "MockModuleA", false) };

            IModuleEnumerator enumerator = new ConfigurationModuleEnumerator(store);

            ModuleInfo[] modules = enumerator.GetModules();

            Assert.IsNotNull(modules);
            Assert.AreEqual(1, modules.Length);
            Assert.IsNotNull(modules[0].AssemblyFile);
            Assert.IsTrue(modules[0].AssemblyFile.Contains(@"MocksModules\MockModuleA.dll"));
            Assert.IsNotNull(modules[0].ModuleType);
            Assert.AreEqual("Microsoft.Composite.Tests.Mocks.Modules.MockModuleA", modules[0].ModuleType);
            Assert.IsFalse(modules[0].StartupLoaded);
        }

        [TestMethod]
        public void GetZeroModules()
        {
            MockConfigurationStore store = new MockConfigurationStore();
            IModuleEnumerator enumerator = new ConfigurationModuleEnumerator(store);

            ModuleInfo[] modules = enumerator.GetModules();

            Assert.AreEqual(0, modules.Length);
        }

        [TestMethod]
        public void EnumeratesThreeModulesWithDependencies()
        {
            var store = new MockConfigurationStore();
            var module1 = new ModuleConfigurationElement("Module1.dll", "Test.Module1", "Module1", false);
            module1.Dependencies = new ModuleDependencyCollection(
                new[] { new ModuleDependencyConfigurationElement("Module2") });

            var module2 = new ModuleConfigurationElement("Module2.dll", "Test.Module2", "Module2", false);
            module2.Dependencies = new ModuleDependencyCollection(
                new[] { new ModuleDependencyConfigurationElement("Module3") });

            var module3 = new ModuleConfigurationElement("Module3.dll", "Test.Module3", "Module3", false);
            store.Modules = new[] { module3, module2, module1 };

            IModuleEnumerator enumerator = new ConfigurationModuleEnumerator(store);

            var modules = new List<ModuleInfo>(enumerator.GetModules());

            Assert.AreEqual(3, modules.Count);
            Assert.IsTrue(modules.Exists(module => module.ModuleName == "Module1"));
            Assert.IsTrue(modules.Exists(module => module.ModuleName == "Module2"));
            Assert.IsTrue(modules.Exists(module => module.ModuleName == "Module3"));
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void EnumerateThrowsIfDuplicateNames()
        {
            MockConfigurationStore store = new MockConfigurationStore();
            var module1 = new ModuleConfigurationElement("Module1.dll", "Test.Module1", "Module1", false);
            var module2 = new ModuleConfigurationElement("Module2.dll", "Test.Module2", "Module1", false);
            store.Modules = new[] { module2, module1 };
            IModuleEnumerator enumerator = new ConfigurationModuleEnumerator(store);

            ModuleInfo[] modules = enumerator.GetModules();
        }

        [TestMethod]
        public void EnumerateNotThrowsIfDuplicateAssemblyFile()
        {
            MockConfigurationStore store = new MockConfigurationStore();
            var module1 = new ModuleConfigurationElement("Module1.dll", "Test.Module1", "Module1", false);
            var module2 = new ModuleConfigurationElement("Module1.dll", "Test.Module2", "Module2", false);
            store.Modules = new[] { module2, module1 };
            IModuleEnumerator enumerator = new ConfigurationModuleEnumerator(store);

            Assert.AreEqual(2, enumerator.GetModules().Length);
        }

        [TestMethod]
        public void GetStartupLoadedModulesDoesntRetrieveOnDemandLoaded()
        {
            MockConfigurationStore store = new MockConfigurationStore();
            var module1 = new ModuleConfigurationElement("Module1.dll", "Test.Module1", "Module1", false);

            IModuleEnumerator enumerator = new ConfigurationModuleEnumerator(store);
            store.Modules = new[] { module1 };

            Assert.AreEqual<int>(1, enumerator.GetModules().Length);
            Assert.AreEqual<int>(0, enumerator.GetStartupLoadedModules().Length);
        }

        [TestMethod]
        public void GetModuleReturnsOnlySpecifiedModule()
        {
            MockConfigurationStore store = new MockConfigurationStore();
            var module1 = new ModuleConfigurationElement("Module1.dll", "Test.Module1", "Module1", false);

            IModuleEnumerator enumerator = new ConfigurationModuleEnumerator(store);
            store.Modules = new[] { module1 };

            var module = enumerator.GetModule("Module1");

            Assert.IsNotNull(module);
            Assert.AreEqual("Module1", module.ModuleName);
        }

        [TestMethod]
        public void GetModulesNotThrownIfModuleSectionIsNotDeclared()
        {
            MockNullConfigurationStore store = new MockNullConfigurationStore();

            IModuleEnumerator enumerator = new ConfigurationModuleEnumerator(store);

            var modules = enumerator.GetModules();

            Assert.IsNotNull(modules);
            Assert.AreEqual(0, modules.Length);
        }
    }

    internal class MockNullConfigurationStore : ConfigurationStore
    {
        public override ModulesConfigurationSection RetrieveModuleConfigurationSection()
        {
            return null;
        }
    }
}