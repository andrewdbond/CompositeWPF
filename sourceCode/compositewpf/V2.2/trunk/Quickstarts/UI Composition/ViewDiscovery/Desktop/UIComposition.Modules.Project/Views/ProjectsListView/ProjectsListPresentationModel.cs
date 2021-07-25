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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace UIComposition.Modules.Project
{

    public class ProjectsListPresentationModel : INotifyPropertyChanged
    {
        private int employeeId;
        private ObservableCollection<BusinessEntities.Project> projects;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<BusinessEntities.Project> Projects
        {
            get
            {
                return this.projects;
            }
            set
            {
                if (this.projects != value)
                {
                    this.projects = value;
                    this.OnPropertyChanged("Projects");
                }
            }
        }

        public int EmployeeId
        {
            get
            {
                return this.employeeId;
            }
            set
            {
                if (this.employeeId != value)
                {
                    this.employeeId = value;
                    this.OnPropertyChanged("EmployeeId");
                }
            }
        }

        public static string HeaderInfo
        {
            get { return "Current Projects"; }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler Handler = this.PropertyChanged;
            if (Handler != null) Handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

