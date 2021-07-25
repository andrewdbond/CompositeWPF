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
        private readonly IRegionViewRegistry regionViewRegistry;
        private readonly IRegionManager regionManager;

        public EmployeeModule(IUnityContainer container, IRegionViewRegistry regionViewRegistry, IRegionManager regionManager)
        {
            this.container = container;
            this.regionViewRegistry = regionViewRegistry;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            this.RegisterTypesAndServices();

            this.RegisterViewsWithRegions();
        }

        private void RegisterViewsWithRegions()
        {
            // There are 2 ways to register views. 

            // Note: We have used Presenter first development here. The presenter is responsible for creating the views.
            // So we're registering a delegate that will resolve the presenter through the container and return the view 
            // If you want to register a view directly, you can use the following syntax:
            // this.regionManager.RegisterViewWithRegion("RegionName", typeof(MyView));

            // Method 1: Calling the regionViewRegistry directly
            this.regionViewRegistry.RegisterViewWithRegion(RegionNames.SelectionRegion
                , () => this.container.Resolve<EmployeesListPresenter>().View);

            // Method 2: Extentionmethods on the RegionManager
            // Registering types for pull based composition. To make this functionality easier to find, the regionmanager
            // has extentionmethods for easy access. 
            // Under the covers, the regionViewRegistry is used to register types with views. Calling the methods 
            // on the registry directly is equivalent to calling the regionmanager.
            this.regionManager.RegisterViewWithRegion(RegionNames.MainRegion,
                                                      () => this.container.Resolve<EmployeesPresenter>().View);


        }

        protected void RegisterTypesAndServices()
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
