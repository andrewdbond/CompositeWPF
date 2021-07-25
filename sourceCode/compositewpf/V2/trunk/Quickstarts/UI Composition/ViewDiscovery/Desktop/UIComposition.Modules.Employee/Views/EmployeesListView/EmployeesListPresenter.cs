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
using Microsoft.Practices.Composite.Events;
using UIComposition.Modules.Employee.Services;

namespace UIComposition.Modules.Employee
{
    public class EmployeesListPresenter : IEmployeesListPresenter
    {

        public EmployeesListPresenter(IEmployeesListView view,
            IEmployeeService employeeService)
        {
            this.View = view;
            this.View.EmployeeSelected += delegate(object sender, DataEventArgs<BusinessEntities.Employee> e)
            {
                // Update the value of the regioncontext. This regioncontext is two way bound to the 
                // SelectedEmployee property of the EmployeePresentationModel. So the control that's hosting the region
                // (EmployeesView) will get notified of the selection change through databinding. 
                this.View.ObservableRegionContext.Value = e.Value;
            };
            view.Model = employeeService.RetrieveEmployees();
        }

        public IEmployeesListView View { get; set; }

    }
}
