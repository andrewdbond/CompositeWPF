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
    public class StaticModuleEnumeratorFixture
    {
        [TestMethod]
        public void CanAddModuleToEnumerator()
        {
            var enumerator = new StaticModuleEnumerator();
            enumerator.AddModule(typeof(MockModule));

            var modules = enumerator.GetModules();
            Assert.AreEqual(1, modules.Length);
            Assert.AreEqual(typeof(MockModule).Name, modules[0].ModuleName);
            Assert.AreEqual(typeof(MockModule).FullName, modules[0].ModuleType);
            Assert.AreEqual(typeof(MockModule).Assembly.Location, modules[0].AssemblyFile);
        }

        [TestMethod]
        public void CanPassDependantModules()
        {
            var enumerator = new StaticModuleEnumerator();
            enumerator.AddModule(typeof(MockModule), "DependentModule");

            var modules = enumerator.GetModules();
            Assert.IsNotNull(modules[0].DependsOn);
            Assert.AreEqual(1, modules[0].DependsOn.Count);
            Assert.AreEqual("DependentModule", modules[0].DependsOn[0]);
        }

        [TestMethod]
        public void GetStartupModulesReturnsAllModules()
        {
            var enumerator = new StaticModuleEnumerator();
            enumerator.AddModule(typeof(MockModule));
            enumerator.AddModule(typeof(AnotherModule));

            var startupModules = enumerator.GetStartupLoadedModules();
            Assert.AreEqual(2, startupModules.Length);

            var allModules = enumerator.GetModules();
            Assert.AreSame(allModules[0], startupModules[0]);
        }

        [TestMethod]
        public void GetModuleReturnsModuleInfoForModule()
        {
            var enumerator = new StaticModuleEnumerator();
            enumerator.AddModule(typeof(MockModule));
            enumerator.AddModule(typeof(AnotherModule));

            var module = enumerator.GetModule(typeof(MockModule).Name);
            Assert.AreEqual(typeof(MockModule).FullName, module.ModuleType);
        }

        [TestMethod]
        public void FluentInterfaceShouldReturnTheSameInstance()
        {
            var enumerator = new StaticModuleEnumerator();

            Assert.AreSame(enumerator, enumerator.AddModule(typeof(MockModule)));
        }

        class MockModule : IModule
        {
            public void Initialize()
            {
                throw new NotImplementedException();
            }
        }

        class AnotherModule : IModule
        {
            public void Initialize()
            {
                throw new NotImplementedException();
            }
        }
    }
}