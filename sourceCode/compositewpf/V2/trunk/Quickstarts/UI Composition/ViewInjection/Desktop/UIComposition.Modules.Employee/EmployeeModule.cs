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
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using UIComposition.Modules.Employee.Controllers;
using UIComposition.Modules.Employee.Services;

namespace UIComposition.Modules.Employee
{
    public class EmployeeModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

        public EmployeeModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            this.RegisterViewsAndServices();

            EmployeesPresenter presenter = this.container.Resolve<EmployeesPresenter>();

            IRegion mainRegion = this.regionManager.Regions[RegionNames.MainRegion];
            mainRegion.Add(presenter.View);
        }

        protected void RegisterViewsAndServices()
        {
            this.container.RegisterType<IEmployeesController, EmployeesController>();

            this.container.RegisterType<IEmployeeService, EmployeeService>(new ContainerControlledLifetimeManager());

            this.container.RegisterType<IEmployeesView, EmployeesView>();

            this.container.RegisterType<IEmployeesListView, EmployeesListView>();
            this.container.RegisterType<IEmployeesListPresenter, EmployeesListPresenter>();

            this.container.RegisterType<IEmployeesDetailsView, EmployeesDetailsView>();
            this.container.RegisterType<IEmployeesDetailsPresenter, EmployeesDetailsPresenter>();
        }
    }
}
