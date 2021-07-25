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
using Microsoft.Phone.Controls;
using Microsoft.Practices.Prism.TestSupport;
using Microsoft.Silverlight.Testing;
using Microsoft.Silverlight.Testing.Harness;

namespace Microsoft.Practices.Prism.Tests
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            LogProvider fileLogProvider = new FileLogProvider();
            var settings = UnitTestSystem.CreateDefaultSettings();
            settings.LogProviders.Add(fileLogProvider);

            Content = UnitTestSystem.CreateTestPage(settings);
            BackKeyPress += (x, xe) => xe.Cancel = (Content as IMobileTestPage).NavigateBack();
        }
    }
}
