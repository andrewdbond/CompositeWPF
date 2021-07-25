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
using Microsoft.Practices.Composite.Events;
using UIComposition.Modules.Employee.Controllers;
using UIComposition.Modules.Employee.PresentationModels;

namespace UIComposition.Modules.Employee
{
    public class EmployeesPresenter
    {
        private IEmployeesListPresenter listPresenter;
        private IEmployeesController employeeController;
        private EmployeesPresentationModel model;

        public EmployeesPresenter(
            IEmployeesView view,
            IEmployeesController employeeController)
        {
            this.View = view;
            this.employeeController = employeeController;

            this.model = new EmployeesPresentationModel();
            this.model.PropertyChanged += Model_PropertyChanged;

            this.View.Model = model;
            
        }

        void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedEmployee")
            {
                employeeController.OnEmployeeSelected(this.View.Model.SelectedEmployee);
            }
        }

        public IEmployeesView View { get; set; }

    }
}
