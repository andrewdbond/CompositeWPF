//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UIComposition.Modules.Employee.Controllers;
using UIComposition.Modules.Employee.Services;
using UIComposition.Modules.Employee.Tests.Mocks;

namespace UIComposition.Modules.Employee.Tests
{
    [TestClass]
    public class EmployeeModuleFixture
    {
        MockUnityContainer container;
        MockRegionContentRegistry regionViewRegistry;
        MockRegionManager regionManager;

        [TestInitialize]
        public void SetUp()
        {
            container = new MockUnityContainer();
            regionViewRegistry = new MockRegionContentRegistry();
            regionManager = new MockRegionManager();
        }

        [TestMethod]
        public void ShouldRegisterViewsAndServices()
        {
            var module = CreateModule();

            module.Initialize();

            Assert.AreEqual(typeof(EmployeesController), container.Types[typeof(IEmployeesController)]);
            Assert.AreEqual(typeof(EmployeeService), container.Types[typeof(IEmployeeService)]);
            Assert.AreEqual(typeof(EmployeesView), container.Types[typeof(IEmployeesView)]);
            Assert.AreEqual(typeof(EmployeesListView), container.Types[typeof(IEmployeesListView)]);
            Assert.AreEqual(typeof(EmployeesListPresenter), container.Types[typeof(IEmployeesListPresenter)]);
            Assert.AreEqual(typeof(EmployeesDetailsView), container.Types[typeof(IEmployeesDetailsView)]);
            Assert.AreEqual(typeof(EmployeesDetailsPresenter), container.Types[typeof(IEmployeesDetailsPresenter)]);
        }

        [TestMethod]
        public void InitializeShouldRegisterEmployeesViewForRegion()
        {
            List<string> regionNames = new List<string>();
            Func<object> getContentDelegate = null;
            regionViewRegistry.RegisterContent = (name, del) =>
                                                        {
                                                            regionNames.Add(name);
                                                            getContentDelegate = del;
                                                        };

            EmployeeModule module = CreateModule();

            Assert.AreEqual(0, regionNames.Count);

            module.Initialize();

            Assert.IsTrue(regionNames.Contains(RegionNames.MainRegion));
            Assert.IsTrue(regionNames.Contains(RegionNames.SelectionRegion));

            Assert.AreEqual(2, regionNames.Count);
        }

        [TestMethod]
        public void RegisteredContentShouldReturnCorrectView()
        {
            Dictionary<string, Func<object>> getContentDelegates = new Dictionary<string, Func<object>>();
            regionViewRegistry.RegisterContent = (name, del) =>
            {
                getContentDelegates.Add(name, del);
            };

            EmployeeModule module = CreateModule();
            module.Initialize();

            Assert.AreEqual(2, getContentDelegates.Count);

            Assert.IsInstanceOfType(getContentDelegates[RegionNames.MainRegion]() , typeof(IEmployeesView));
            Assert.IsInstanceOfType(getContentDelegates[RegionNames.SelectionRegion]() , typeof(IEmployeesListView));
        }

        private EmployeeModule CreateModule()
        {
            // Setup the service locator so it will return the regionViewRegistry. This is required
            // for the regionManager
            ServiceLocator.SetLocatorProvider(
                () => new MockServiceLocator() {GetInstance = (type) => regionViewRegistry});

            EmployeeModule module = new EmployeeModule(container, regionViewRegistry, regionManager);
            return module;
        }

        public class MockRegionContentRegistry : IRegionViewRegistry
        {
            public Action<string, Func<object>> RegisterContent;

            event EventHandler<ViewRegisteredEventArgs> IRegionViewRegistry.ContentRegistered
            {
                add { throw new NotImplementedException(); }
                remove { throw new NotImplementedException(); }
            }

            IEnumerable<object> IRegionViewRegistry.GetContents(string regionName)
            {
                throw new NotImplementedException();
            }

            void IRegionViewRegistry.RegisterViewWithRegion(string regionName, Type viewType)
            {
                throw new NotImplementedException();
            }

            void IRegionViewRegistry.RegisterViewWithRegion(string regionName, Func<object> getContentDelegate)
            {
                if (RegisterContent != null)
                    RegisterContent(regionName, getContentDelegate);
            }
        }
    }
}
