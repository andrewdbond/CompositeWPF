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

namespace UIComposition.Modules.Employee.Controllers
{
    public class EmployeesController : IEmployeesController
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

        public EmployeesController(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public virtual void OnEmployeeSelected(BusinessEntities.Employee employee)
        {
            // We are still using Push based composition here, to show the usage of scoped regionmanagers. 
            // When an employee gets selected, we are creating a new instance of the EmployeeDetailsView and are pushing it in the region. 
            // this would be hard to do with the 'out of the box' functionality of the pull based composition, because we
            // have a requirement where we want to push a view into the region at a specific point in time, and not when
            // the region decides it needs to pull a view into it.

            IRegion detailsRegion = regionManager.Regions[RegionNames.DetailsRegion];
            object existingView = detailsRegion.GetView(employee.EmployeeId.ToString(CultureInfo.InvariantCulture));

            // See if the view already exists in the region. 
            if (existingView == null)
            {
                // the view does not exist yet. Create it and push it into the region
                IEmployeesDetailsPresenter detailsPresenter = this.container.Resolve<IEmployeesDetailsPresenter>();
                detailsPresenter.SetSelectedEmployee(employee);

                // the details view should receive it's own scoped region manager, therefore Add overload using 'true' (see notes below).
                detailsRegion.Add(detailsPresenter.View, employee.EmployeeId.ToString(CultureInfo.InvariantCulture), true);

                detailsRegion.Activate(detailsPresenter.View);
            }
            else
            {
                // The view already exists. Just show it. 
                detailsRegion.Activate(existingView);
            }

            //************************************************************************************************
            // Note on using Scoped Regionmanagers:
            // Scoped regionmanagers are used to have multiple instances of the same region in memory at the same time. 
            // Since a region gets registered with a regionmanager, the name has to be unique. 
            // In this example, we are keeping several instances of the EmployeeDetailsView in memory. Each view has it's own TabRegion, 
            // so each view needs it's own regionmanager. This is still hard to do with pull based composition. 
            //************************************************************************************************

            //************************************************************************************************
            // Note on Push based vs Pull based composition. 
            // Prism V2 is going to support both Push based (V1 style) and Pull based (New in V2) composition. 
            //
            // In pull based composition, as soon as the region get's added to the visual tree, it will be populated with new instances of 
            // registered views. In Pull based composition, you get less control over when views are added (only on load of the region) 
            // but this allows for an easier programming model. 
            //
            // In push based composition, you have to write some code that, at a certain point in time, finds a region that's already
            // created (might not be visible yet) and adds views to it. Using Push based composition, you have more control over when
            // you want to add views to a particular region, but at the expense of more complexity.
            //************************************************************************************************
        }
    }
}