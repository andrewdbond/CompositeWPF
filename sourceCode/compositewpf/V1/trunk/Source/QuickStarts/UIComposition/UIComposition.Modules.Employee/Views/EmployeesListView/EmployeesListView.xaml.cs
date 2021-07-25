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
    using System.Collections.ObjectModel;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for EmployeesListView.xaml
    /// </summary>
    public partial class EmployeesListView : UserControl, IEmployeesListView
    {
        public EmployeesListView()
        {
            InitializeComponent();
        }

        public ObservableCollection<BusinessEntities.Employee> Model
        {
            get { return this.DataContext as ObservableCollection<BusinessEntities.Employee>; }
            set { this.DataContext = value; }
        }

        public event EventHandler<DataEventArgs<BusinessEntities.Employee>> EmployeeSelected = delegate { };

        private void EmployeesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                BusinessEntities.Employee selected = e.AddedItems[0] as BusinessEntities.Employee;
                if (selected != null)
                {
                    EmployeeSelected(this, new DataEventArgs<BusinessEntities.Employee>(selected));
                }
            }
        }
    }
}
