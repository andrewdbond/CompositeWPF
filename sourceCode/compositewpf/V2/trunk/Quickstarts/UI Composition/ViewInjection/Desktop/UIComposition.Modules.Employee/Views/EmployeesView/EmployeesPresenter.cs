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
using UIComposition.Modules.Employee.Controllers;
using Microsoft.Practices.Composite.Events;

namespace UIComposition.Modules.Employee
{
    public class EmployeesPresenter
    {
        private IEmployeesListPresenter listPresenter;
        private IEmployeesController employeeController;

        public EmployeesPresenter(
            IEmployeesView view,
            IEmployeesListPresenter listPresenter,
            IEmployeesController employeeController)
        {
            this.View = view;
            this.listPresenter = listPresenter;
            this.listPresenter.EmployeeSelected += new EventHandler<DataEventArgs<BusinessEntities.Employee>>(this.OnEmployeeSelected);
            this.employeeController = employeeController;

            View.SetHeader(listPresenter.View);
        }

        public IEmployeesView View { get; set; }

        private void OnEmployeeSelected(object sender, DataEventArgs<BusinessEntities.Employee> e)
        {
            employeeController.OnEmployeeSelected(e.Value);
        }
    }
}
