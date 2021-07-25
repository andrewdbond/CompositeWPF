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

using Microsoft.Practices.Composite.Events;

namespace UIComposition.Modules.Employee
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using UIComposition.Modules.Employee.Services;

    public class EmployeesListPresenter : IEmployeesListPresenter
    {
        public event EventHandler<DataEventArgs<BusinessEntities.Employee>> EmployeeSelected = delegate { };

        public EmployeesListPresenter(IEmployeesListView view,
            IEmployeeService employeeService)
        {
            this.View = view;
            this.View.EmployeeSelected += delegate(object sender, DataEventArgs<BusinessEntities.Employee> e)
            {
                EmployeeSelected(sender, e);
            };
            view.Model = employeeService.RetrieveEmployees();
        }

        public IEmployeesListView View { get; set; }
    }
}
