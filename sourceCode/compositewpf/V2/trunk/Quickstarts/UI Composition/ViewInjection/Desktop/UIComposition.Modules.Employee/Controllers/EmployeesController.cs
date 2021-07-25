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
using System.Globalization;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using UIComposition.Modules.Project;

namespace UIComposition.Modules.Employee.Controllers
{
    public class EmployeesController : IEmployeesController
    {
        private IUnityContainer container;
        private IRegionManager regionManager;

        public EmployeesController(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public virtual void OnEmployeeSelected(BusinessEntities.Employee employee)
        {
            IRegion detailsRegion = regionManager.Regions[RegionNames.DetailsRegion];
            object existingView = detailsRegion.GetView(employee.EmployeeId.ToString(CultureInfo.InvariantCulture));

            if (existingView == null)
            {
                IProjectsListPresenter projectsListPresenter = this.container.Resolve<IProjectsListPresenter>();
                projectsListPresenter.SetProjects(employee.EmployeeId);

                IEmployeesDetailsPresenter detailsPresenter = this.container.Resolve<IEmployeesDetailsPresenter>();
                detailsPresenter.SetSelectedEmployee(employee);

                IRegionManager detailsRegionManager = detailsRegion.Add(detailsPresenter.View, employee.EmployeeId.ToString(CultureInfo.InvariantCulture), true);
                IRegion region = detailsRegionManager.Regions[RegionNames.TabRegion];
                region.Add(projectsListPresenter.View, "CurrentProjectsView");
                detailsRegion.Activate(detailsPresenter.View);
            }
            else
            {
                detailsRegion.Activate(existingView);
            }
        }
    }
}
