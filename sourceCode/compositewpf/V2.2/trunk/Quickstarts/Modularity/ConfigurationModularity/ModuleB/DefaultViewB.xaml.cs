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
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Modularity;

namespace ModuleB
{
    /// <summary>
    /// Interaction logic for DefaultViewB.xaml
    /// </summary>
    public partial class DefaultViewB : UserControl
    {
        private readonly IModuleManager moduleManager;

        public DefaultViewB(IModuleManager moduleManager)
        {
            this.moduleManager = moduleManager;

            InitializeComponent();
        }

        private void OnLoadModuleCClick(object sender, RoutedEventArgs e)
        {
            moduleManager.LoadModule("ModuleC");
        }
    }
}
