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

using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UIComposition.Modules.Employee.Controllers;
using UIComposition.Modules.Employee.Tests.Mocks;
using UIComposition.Modules.Project;

namespace UIComposition.Modules.Employee.Tests.Controllers
{
    [TestClass]
    public class EmployeesControllerFixture
    {
        IUnityContainer container;
        IRegionManager regionManager;

        [TestInitialize]
        public void SetUp()
        {
            container = new MockUnityContainer();
            regionManager = new MockRegionManager();
        }

        [TestMethod]
        public void CanInitController()
        {
            IEmployeesController controller = CreateController();

            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void ControllerAddsViewWhenShowDetailsIfNotPresent()
        {
            container.RegisterType<IEmployeesDetailsPresenter, MockEmployeesDetailsPresenter>();
            container.RegisterType<IProjectsListPresenter, MockProjectsListPresenter>();

            var regionManager = new MockRegionManager();
            var detailsRegion = new MockRegion();
            regionManager.Regions.Add(RegionNames.DetailsRegion, detailsRegion);
            var scopedRegionManager = new MockRegionManager();
            scopedRegionManager.Regions.Add(RegionNames.TabRegion, new MockRegion());
            detailsRegion.AddReturnValue = scopedRegionManager;

            BusinessEntities.Employee employee1 = new BusinessEntities.Employee(10) { LastName = "Mock1", FirstName = "Employee1" };
            BusinessEntities.Employee employee2 = new BusinessEntities.Employee(11) { LastName = "Mock2", FirstName = "Employee2" };

            EmployeesController controller = new EmployeesController(container, regionManager);

            Assert.AreEqual<int>(0, detailsRegion.ViewsCount);

            controller.OnEmployeeSelected(employee1);
            controller.OnEmployeeSelected(employee2);

            Assert.AreEqual<int>(2, detailsRegion.ViewsCount);
        }

        [TestMethod]
        public void ControllerDoesNotAddViewWhenShowDetailsIfIsAlreadyPresent()
        {
            container.RegisterType<IEmployeesDetailsPresenter, MockEmployeesDetailsPresenter>();
            container.RegisterType<IProjectsListPresenter, MockProjectsListPresenter>();

            var regionManager = new MockRegionManager();
            var detailsRegion = new MockRegion();
            regionManager.Regions.Add(RegionNames.DetailsRegion, detailsRegion);
            var scopedRegionManager = new MockRegionManager();
            scopedRegionManager.Regions.Add(RegionNames.TabRegion, new MockRegion());
            detailsRegion.AddReturnValue = scopedRegionManager;

            BusinessEntities.Employee employee = new BusinessEntities.Employee(10) { LastName = "Con", FirstName = "Aaron" };

            EmployeesController controller = new EmployeesController(container, regionManager);

            Assert.AreEqual<int>(0, detailsRegion.ViewsCount);

            controller.OnEmployeeSelected(employee);
            controller.OnEmployeeSelected(employee);

            Assert.AreEqual<int>(1, detailsRegion.ViewsCount);
            Assert.IsTrue(detailsRegion.ActivateCalled);
        }

        [TestMethod]
        public void ShowTheNewlyAddedViewInTheDetailsRegion()
        {
            container.RegisterType<IEmployeesDetailsPresenter, MockEmployeesDetailsPresenter>();
            container.RegisterType<IProjectsListPresenter, MockProjectsListPresenter>();

            var regionManager = new MockRegionManager();
            var detailsRegion = new MockRegion();
            regionManager.Regions.Add(RegionNames.DetailsRegion, detailsRegion);
            var scopedRegionManager = new MockRegionManager();
            scopedRegionManager.Regions.Add(RegionNames.TabRegion, new MockRegion());
            detailsRegion.AddReturnValue = scopedRegionManager;

            BusinessEntities.Employee employee1 = new BusinessEntities.Employee(10) { LastName = "Mock1", FirstName = "Employee1" };

            EmployeesController controller = new EmployeesController(container, regionManager);

            controller.OnEmployeeSelected(employee1);

            Assert.AreEqual<int>(1, detailsRegion.ViewsCount);
            Assert.IsTrue(detailsRegion.ActivateCalled);
        }


        private IEmployeesController CreateController()
        {
            return new EmployeesController(container, regionManager);
        }
    }
}