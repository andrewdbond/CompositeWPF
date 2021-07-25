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
using Microsoft.Practices.Composite.Modularity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Tests.Modularity
{
    [TestClass]
    public class ConfigurationStoreFixture
    {
        public ConfigurationStoreFixture()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        [DeploymentItem(@"Mocks\Configuration\OneModule\App.config", @"Mocks\Configuration\OneModule")]
        public void ReadsOneModuleAppConfig()
        {
            ConfigurationStore store = new ConfigurationStore(@"Mocks\Configuration\OneModule");

            ModulesConfigurationSection section = store.RetrieveModuleConfigurationSection();

            Assert.IsNotNull(section.Modules);
            Assert.AreEqual(1, section.Modules.Count);
            Assert.IsNotNull(section.Modules[0].AssemblyFile);
            Assert.AreEqual("MockModuleA", section.Modules[0].ModuleName);
            Assert.IsNotNull(section.Modules[0].AssemblyFile);
            Assert.IsTrue(section.Modules[0].AssemblyFile.Contains(@"MocksModules\MockModuleA.dll"));
            Assert.IsNotNull(section.Modules[0].ModuleType);
            Assert.IsTrue(section.Modules[0].StartupLoaded);
            Assert.AreEqual("Microsoft.Practices.Composite.Tests.Mocks.Modules.MockModuleA", section.Modules[0].ModuleType);
        }

        [TestMethod]
        [DeploymentItem(@"Mocks\Configuration\TwoModulesWithDependency\App.config", @"Mocks\Configuration\TwoModulesWithDependency")]
        public void ReadsTwoModulesWithDependency()
        {
            ConfigurationStore store = new ConfigurationStore(@"Mocks\Configuration\TwoModulesWithDependency");

            ModulesConfigurationSection section = store.RetrieveModuleConfigurationSection();

            Assert.AreEqual(2, section.Modules.Count);
            Assert.AreEqual("MockModuleA", section.Modules[0].ModuleName);
            Assert.IsTrue(section.Modules[0].AssemblyFile.Contains(@"MocksModules\MockModuleA.dll"));
            Assert.AreEqual("Microsoft.Practices.Composite.Tests.Mocks.Modules.MockModuleA", section.Modules[0].ModuleType);
            Assert.IsTrue(section.Modules[0].StartupLoaded);
            Assert.AreEqual("MockModuleB", section.Modules[0].Dependencies[0].ModuleName);
            Assert.AreEqual("MockModuleB", section.Modules[1].ModuleName);
            Assert.IsTrue(section.Modules[1].AssemblyFile.Contains(@"MocksModules\MockModuleB.dll"));
            Assert.AreEqual("Microsoft.Practices.Composite.Tests.Mocks.Modules.MockModuleB", section.Modules[1].ModuleType);
            Assert.IsTrue((section.Modules[1].StartupLoaded));
        }
    }
}