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
using Microsoft.Practices.Composite.Regions;
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
        MockRegionManager regionManager;

        [TestInitialize]
        public void SetUp()
        {
            container = new MockUnityContainer();
            regionManager = new MockRegionManager();
        }

        [TestMethod]
        public void RegisterViewsAndServices()
        {
            TestableEmployeeModule module = CreateTestableModule();

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(typeof(EmployeesController), container.Types[typeof(IEmployeesController)]);
            Assert.AreEqual(typeof(EmployeeService), container.Types[typeof(IEmployeeService)]);
            Assert.AreEqual(typeof(EmployeesView), container.Types[typeof(IEmployeesView)]);
            Assert.AreEqual(typeof(EmployeesListView), container.Types[typeof(IEmployeesListView)]);
            Assert.AreEqual(typeof(EmployeesListPresenter), container.Types[typeof(IEmployeesListPresenter)]);
            Assert.AreEqual(typeof(EmployeesDetailsView), container.Types[typeof(IEmployeesDetailsView)]);
            Assert.AreEqual(typeof(EmployeesDetailsPresenter), container.Types[typeof(IEmployeesDetailsPresenter)]);
        }

        [TestMethod]
        public void InitializeShouldCallRegisterAndViewServices()
        {
            TestableEmployeeModule module = CreateTestableModule();

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
        public void InitializeShouldAddEmployeesViewToRegion()
        {
            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterInstance<IRegionManager>(regionManager);
            MockRegion mainRegion = new MockRegion();
            MockRegion mainToolbar = new MockRegion();

            regionManager.Regions.Add(RegionNames.MainRegion, mainRegion);
            regionManager.Regions.Add(RegionNames.MainToolBar, mainToolbar);

            EmployeeModule module = CreateModule();

            Assert.AreEqual(0, mainRegion.ViewsCount);

            module.Initialize();

            Assert.AreEqual(1, mainRegion.ViewsCount);
        }

        private TestableEmployeeModule CreateTestableModule()
        {
            TestableEmployeeModule module = new TestableEmployeeModule(container, regionManager);
            return module;
        }

        private EmployeeModule CreateModule()
        {
            EmployeeModule module = new EmployeeModule(container, regionManager);
            return module;
        }
    }

    class TestableEmployeeModule : EmployeeModule
    {
        public TestableEmployeeModule(IUnityContainer container, IRegionManager regionManager)
            : base(container, regionManager)
        {

        }

        public new void Initialize()
        {
            InvokeRegisterViewsAndServices();
        }

        public void InvokeRegisterViewsAndServices()
        {
            base.RegisterViewsAndServices();
        }
    }
}
