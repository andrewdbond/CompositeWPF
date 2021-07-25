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
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation;
using Microsoft.Practices.Composite.Presentation.Regions;

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

        /// <summary>
        /// This object will hold the regioncontext for the view. You can also subscribe
        /// to change notifications to detect when this property changes
        /// </summary>
        public ObservableObject<object> ObservableRegionContext
        {
            get
            {
                return RegionContext.GetObservableContext(this);
            }
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
