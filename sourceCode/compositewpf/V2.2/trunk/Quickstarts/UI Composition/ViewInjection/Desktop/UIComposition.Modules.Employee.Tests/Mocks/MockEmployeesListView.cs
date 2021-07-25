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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Events;

namespace UIComposition.Modules.Employee.Tests.Mocks
{
    class MockEmployeesListView : Control, IEmployeesListView
    {
        private ObservableCollection<BusinessEntities.Employee> _model;

        public ObservableCollection<BusinessEntities.Employee> Model
        {
            get { return _model; }
            set { _model = value; }
        }

        internal void RaiseEmployeeSelected()
        {
            EmployeeSelected(this, null);
        }

        internal void RaiseEmployeeSelected(BusinessEntities.Employee employee)
        {
            EmployeeSelected(this, new DataEventArgs<BusinessEntities.Employee>(employee));
        }

        public event EventHandler<DataEventArgs<BusinessEntities.Employee>> EmployeeSelected = delegate { };
    }
}
