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
using Microsoft.Practices.Composite.Presentation.Regions.Behaviors;
using Microsoft.Practices.Composite.Regions;
using UIComposition.Modules.Employee.PresentationModels;

namespace UIComposition.Modules.Employee
{
    public partial class EmployeesView : UserControl, IEmployeesView
    {
        public EmployeesView()
        {
            InitializeComponent();

            // This code sample shows how you could get access to a region and replace default behaviors with custom behaviors. 
            // In this example, the AutoPopulateBehavior is replaced by a custom autopopulate behavior. This way, you can implement
            // your own pull based mechanism, or extend the existing one. 

            //RegionExtensions.GetObservableRegion(this.SelectionPanel).PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
            //{
            //    if (args.PropertyName == "Value")
            //    {
            //        // The region has just been created, but (Very Important), the default behaviors have not been added yet. 
            //        // This gives you a chance to add your own custom behaviors. If you use the same key as a default behavior
            //        // it will replace a default behavior. 
            //        IRegion region = RegionExtensions.GetObservableRegion(this.SelectionPanel).Value;
            //        region.Behaviors.Add(AutoPopulateRegionBehavior.BehaviorKey, new CustomAutoPopulateBehavior());
            //    }
            //};

        }

        public EmployeesPresentationModel Model
        {
            get
            {
                return (EmployeesPresentationModel)this.DataContext;
            }
            set
            {
                this.DataContext = value;
            }
        }
    }
}
