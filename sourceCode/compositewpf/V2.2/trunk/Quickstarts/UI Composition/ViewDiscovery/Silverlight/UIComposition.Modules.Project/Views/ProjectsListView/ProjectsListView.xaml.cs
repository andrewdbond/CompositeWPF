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
using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Presentation.Regions;

namespace UIComposition.Modules.Project
{
    public partial class ProjectsListView : UserControl, IProjectsListView
    {
        public ProjectsListView()
        {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += this.RegionContextChanged;
        }

        private void RegionContextChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Model.EmployeeId = (int)RegionContext.GetObservableContext(this).Value;
        }

        public ProjectsListPresentationModel Model
        {
            get { return this.DataContext as ProjectsListPresentationModel; }
            set { this.DataContext = value; }
        }
    }
}
