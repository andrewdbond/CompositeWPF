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


using UIComposition.Modules.Employee.PresentationModels;

namespace UIComposition.Modules.Employee
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for EmployeesDetailsView.xaml
    /// </summary>
    public partial class EmployeesDetailsView : UserControl, IEmployeesDetailsView
    {
        public EmployeesDetailsView()
        {
            InitializeComponent();
        }

        public EmployeesDetailsPresentationModel Model
        {
            get { return this.DataContext as EmployeesDetailsPresentationModel; }
            set { this.DataContext = value; }
        }

        private void Frame_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            ErrorText.Visibility = System.Windows.Visibility.Visible;
            ErrorText.Text = e.Exception.Message;
            e.Handled = true;
        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            ErrorText.Visibility = System.Windows.Visibility.Hidden;
            ErrorText.Text = string.Empty;
        }

    }
}
