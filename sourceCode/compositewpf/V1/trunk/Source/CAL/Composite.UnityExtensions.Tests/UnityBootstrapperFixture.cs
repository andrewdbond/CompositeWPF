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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Logging;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.UnityExtensions.Tests.Mocks;
using Microsoft.Practices.Composite.Wpf.Regions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.UnityExtensions.Tests
{
    [TestClass]
    public class UnityBootstrapperFixture
    {
        [TestMethod]
        public void CanCreateConcreteBootstrapper()
        {
            new DefaultBootstrapper();
        }

        [TestMethod]
        public void CanRunBootstrapper()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();
        }

        [TestMethod]
        public void ShouldInitializeContainer()
        {
            var bootstrapper = new DefaultBootstrapper();
            var container = bootstrapper.GetBaseContainer();

            Assert.IsNull(container);

            bootstrapper.Run();

            container = bootstrapper.GetBaseContainer();

            Assert.IsNotNull(container);
            Assert.IsInstanceOfType(container, typeof(UnityContainer));
        }

        [TestMethod]
        public void ShouldCallInitializeModules()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.InitializeModulesCalled);
        }

        [TestMethod]
        public void ShouldRegisterDefaultMappings()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            Assert.IsNotNull(bootstrapper.DefaultRegionAdapterMappings);
            Assert.IsNotNull(bootstrapper.DefaultRegionAdapterMappings.GetMapping(typeof(ItemsControl)));
            Assert.IsNotNull(bootstrapper.DefaultRegionAdapterMappings.GetMapping(typeof(ContentControl)));
            Assert.IsNotNull(bootstrapper.DefaultRegionAdapterMappings.GetMapping(typeof(Selector)));
        }

        [TestMethod]
        public void ShouldCallGetLogger()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.GetLoggerCalled);
        }

        [TestMethod]
        public void ShouldCallGetEnumerator()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.GetEnumeratorCalled);
        }

        [TestMethod]
        public void ShouldCallCreateSell()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.CreateShellCalled);
        }

        [TestMethod]
        public void ShouldCallConfigureTypeMappings()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureContainerCalled);
        }

        [TestMethod]
        public void ShouldCallConfigureRegionAdapterMappings()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureRegionAdapterMappingsCalled);
        }

        [TestMethod]
        public void NullLoggerThrows()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.ReturnNullDefaultLogger = true;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "ILoggerFacade");
        }

        [TestMethod]
        public void NullModuleEnumeratorThrowsOnDefaultModuleInitialization()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.ModuleEnumerator = null;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IModuleEnumerator");
        }

        [TestMethod]
        public void NotOvewrittenGetModuleEnumeratorThrowsOnDefaultModuleInitialization()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.OverrideGetModuleEnumerator = false;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IModuleEnumerator");
        }

        [TestMethod]
        public void GetLoggerShouldHaveDefault()
        {
            var bootstrapper = new DefaultBootstrapper();
            Assert.IsNull(bootstrapper.DefaultLogger);
            bootstrapper.Run();

            Assert.IsNotNull(bootstrapper.DefaultLogger);
            Assert.IsInstanceOfType(bootstrapper.DefaultLogger, typeof(TraceLogger));
        }

        [TestMethod]
        public void ShouldAssignRegionManagerToReturnedShell()
        {
            var bootstrapper = new DefaultBootstrapper();
            var shell = new DependencyObject();
            bootstrapper.CreateShellReturnValue = shell;

            Assert.IsNull(RegionManager.GetRegionManager(shell));

            bootstrapper.Run();

            Assert.IsNotNull(RegionManager.GetRegionManager(shell));
        }

        [TestMethod]
        public void ShouldNotFailIfReturnedNullShell()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.CreateShellReturnValue = null;
            bootstrapper.Run();
        }

        [TestMethod]
        public void NullModuleLoaderThrowsOnDefaultModuleInitialization()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleEnumerator), bootstrapper.ModuleEnumerator);
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoader), null);

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IModuleLoader");
        }

        [TestMethod]
        public void ShouldRegisterDefaultTypeMappings()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleEnumerator), bootstrapper.ModuleEnumerator);
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoader), new MockModuleLoader());

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.MockUnityContainer.Instances.ContainsKey(typeof(ILoggerFacade)));
            Assert.AreSame(bootstrapper.Logger, bootstrapper.MockUnityContainer.Instances[typeof(ILoggerFacade)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Instances.ContainsKey(typeof(IUnityContainer)));
            Assert.AreSame(bootstrapper.MockUnityContainer, bootstrapper.MockUnityContainer.Instances[typeof(IUnityContainer)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(IContainerFacade)));
            Assert.AreEqual(typeof(UnityContainerAdapter), bootstrapper.MockUnityContainer.Types[typeof(IContainerFacade)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(IModuleLoader)));
            Assert.AreEqual(typeof(ModuleLoader), bootstrapper.MockUnityContainer.Types[typeof(IModuleLoader)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(IRegionManager)));
            Assert.AreEqual(typeof(RegionManager), bootstrapper.MockUnityContainer.Types[typeof(IRegionManager)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(IEventAggregator)));
            Assert.AreEqual(typeof(EventAggregator), bootstrapper.MockUnityContainer.Types[typeof(IEventAggregator)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(RegionAdapterMappings)));
            Assert.AreEqual(typeof(RegionAdapterMappings), bootstrapper.MockUnityContainer.Types[typeof(RegionAdapterMappings)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Instances.ContainsKey(typeof(IModuleEnumerator)));
            Assert.AreSame(bootstrapper.ModuleEnumerator, bootstrapper.MockUnityContainer.Instances[typeof(IModuleEnumerator)]);
        }

        [TestMethod]
        public void ShouldCallGetStartupLoadedModules()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleEnumerator), bootstrapper.ModuleEnumerator);
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoader), new MockModuleLoader());

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ModuleEnumerator.GetStartupLoadedModulesCalled);
        }

        [TestMethod]
        public void ShouldCallInitializeOnModuleLoader()
        {
            var bootstrapper = new MockedBootstrapper();

            var moduleLoader = new MockModuleLoader();
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleEnumerator), new MockModuleEnumerator());
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoader), moduleLoader);

            bootstrapper.Run();

            Assert.IsTrue(moduleLoader.InitializeCalled);
        }

        [TestMethod]
        public void ShouldCallInitializeOnModuleLoaderWithStartupModules()
        {
            var bootstrapper = new MockedBootstrapper();
            var moduleLoader = new MockModuleLoader();

            bootstrapper.ModuleEnumerator.StartupLoadedModules = new[] { new ModuleInfo("asm", "type", "name") };

            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleEnumerator), bootstrapper.ModuleEnumerator);
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoader), moduleLoader);


            bootstrapper.Run();

            Assert.IsNotNull(moduleLoader.InitializeArgumentModuleInfos);
            Assert.AreEqual(1, moduleLoader.InitializeArgumentModuleInfos.Length);
            Assert.AreEqual("name", moduleLoader.InitializeArgumentModuleInfos[0].ModuleName);
        }

        [TestMethod]
        public void ReturningNullContainerThrows()
        {
            var bootstrapper = new MockedBootstrapper();
            bootstrapper.MockUnityContainer = null;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IUnityContainer");
        }

        [TestMethod]
        public void ShouldCallTheMethodsInOrder()
        {
            var bootstrapper = new TestableOrderedBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(CompareOrder("LoggerFacade", "CreateContainer", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("CreateContainer", "ConfigureContainer", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("ConfigureContainer", "GetModuleEnumerator", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("GetModuleEnumerator", "ConfigureRegionAdapterMappings", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("ConfigureRegionAdapterMappings", "CreateShell", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("CreateShell", "InitializeModules", bootstrapper.OrderedMethodCallList) < 0);
        }

        [TestMethod]
        public void ShouldLogBootstrapperSteps()
        {
            var bootstrapper = new TestableOrderedBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.Logger.Messages;

            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Creating Unity container")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Configuring container")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Configuring region adapters")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Creating shell")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Initializing modules")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Bootstrapper sequence completed")));
        }

        [TestMethod]
        public void ShouldNotRegisterDefaultServicesAndTypes()
        {
            var bootstrapper = new NonconfiguredBootstrapper();
            bootstrapper.Run(false);


            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IEventAggregator)));
            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IRegionManager)));
            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(RegionAdapterMappings)));
            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IContainerFacade)));
            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IModuleLoader)));
        }

        [TestMethod]
        public void ShoudLogRegisterTypeIfMissingMessage()
        {
            var bootstrapper = new TestableOrderedBootstrapper();
            bootstrapper.AddCustomTypeMappings = true;
            bootstrapper.Run();
            var messages = bootstrapper.Logger.Messages;

            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Type 'IRegionManager' was already registered by the application")));
        }

        private static int CompareOrder(string firstString, string secondString, IList<string> list)
        {
            return list.IndexOf(firstString).CompareTo(list.IndexOf(secondString));
        }

        private static void AssertExceptionThrownOnRun(UnityBootstrapper bootstrapper, Type expectedExceptionType, string expectedExceptionMessageSubstring)
        {
            bool exceptionThrown = false;
            try
            {
                bootstrapper.Run();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expectedExceptionType, ex.GetType());
                StringAssert.Contains(ex.Message, expectedExceptionMessageSubstring);
                exceptionThrown = true;
            }
            if (!exceptionThrown)
            {
                Assert.Fail("Exception not thrown.");
            }
        }
    }

    class NonconfiguredBootstrapper : UnityBootstrapper
    {
        private MockUnityContainer mockContainer;

        protected override void InitializeModules()
        {
        }

        protected override IUnityContainer CreateContainer()
        {
            mockContainer = new MockUnityContainer();
            return mockContainer;
        }

        protected override DependencyObject CreateShell()
        {
            return null;
        }

        public bool HasRegisteredType(Type t)
        {
            return mockContainer.Types.ContainsKey(t);
        }
    }

    class DefaultBootstrapper : UnityBootstrapper
    {
        public bool GetEnumeratorCalled;
        public bool GetLoggerCalled;
        public bool InitializeModulesCalled;
        public bool CreateShellCalled;
        public bool ReturnNullDefaultLogger;
        public bool OverrideGetModuleEnumerator = true;
        public IModuleEnumerator ModuleEnumerator = new MockModuleEnumerator();
        public ILoggerFacade DefaultLogger;
        public RegionAdapterMappings DefaultRegionAdapterMappings;
        public DependencyObject CreateShellReturnValue;
        public bool ConfigureContainerCalled;
        public bool ConfigureRegionAdapterMappingsCalled;

        public IUnityContainer GetBaseContainer()
        {
            return base.Container;
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            ConfigureRegionAdapterMappingsCalled = true;
            var regionAdapterMappings = base.ConfigureRegionAdapterMappings();

            DefaultRegionAdapterMappings = regionAdapterMappings;

            return regionAdapterMappings;
        }

        protected override void ConfigureContainer()
        {
            ConfigureContainerCalled = true;
            base.ConfigureContainer();
        }

        protected override IModuleEnumerator GetModuleEnumerator()
        {
            GetEnumeratorCalled = true;
            if (OverrideGetModuleEnumerator)
            {
                return ModuleEnumerator;
            }
            else
            {
                return base.GetModuleEnumerator();
            }
        }

        protected override ILoggerFacade LoggerFacade
        {
            get
            {
                GetLoggerCalled = true;
                if (ReturnNullDefaultLogger)
                {
                    return null;
                }
                else
                {
                    DefaultLogger = base.LoggerFacade;
                    return DefaultLogger;
                }
            }
        }

        protected override void InitializeModules()
        {
            InitializeModulesCalled = true;
            base.InitializeModules();
        }

        protected override DependencyObject CreateShell()
        {
            CreateShellCalled = true;

            return CreateShellReturnValue;
        }
    }

    class MockedBootstrapper : UnityBootstrapper
    {
        public MockUnityContainer MockUnityContainer = new MockUnityContainer();
        public MockModuleEnumerator ModuleEnumerator = new MockModuleEnumerator();
        public MockLoggerAdapter Logger = new MockLoggerAdapter();

        protected override IUnityContainer CreateContainer()
        {
            return this.MockUnityContainer;
        }

        protected override IModuleEnumerator GetModuleEnumerator()
        {
            return ModuleEnumerator;
        }

        protected override ILoggerFacade LoggerFacade
        {
            get { return Logger; }
        }

        protected override DependencyObject CreateShell()
        {
            return null;
        }
    }

    class TestableOrderedBootstrapper : UnityBootstrapper
    {
        public IList<string> OrderedMethodCallList = new List<string>();
        public MockLoggerAdapter Logger = new MockLoggerAdapter();
        public bool AddCustomTypeMappings;

        protected override IUnityContainer CreateContainer()
        {
            OrderedMethodCallList.Add("CreateContainer");
            return base.CreateContainer();
        }

        protected override ILoggerFacade LoggerFacade
        {
            get
            {
                OrderedMethodCallList.Add("LoggerFacade");
                return Logger;
            }
        }

        protected override IModuleEnumerator GetModuleEnumerator()
        {
            OrderedMethodCallList.Add("GetModuleEnumerator");
            return new MockModuleEnumerator();
        }

        protected override void ConfigureContainer()
        {
            OrderedMethodCallList.Add("ConfigureContainer");
            if (AddCustomTypeMappings)
            {
                RegisterTypeIfMissing(typeof(IRegionManager), typeof(MockRegionManager), true);
            }
            base.ConfigureContainer();
        }

        protected override void InitializeModules()
        {
            OrderedMethodCallList.Add("InitializeModules");
            base.InitializeModules();
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            OrderedMethodCallList.Add("ConfigureRegionAdapterMappings");
            return base.ConfigureRegionAdapterMappings();
        }

        protected override DependencyObject CreateShell()
        {
            OrderedMethodCallList.Add("CreateShell");
            return null;
        }
    }
}