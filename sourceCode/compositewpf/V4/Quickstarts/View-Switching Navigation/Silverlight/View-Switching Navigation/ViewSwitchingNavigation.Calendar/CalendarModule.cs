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
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using ViewSwitchingNavigation.Calendar.Views;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Calendar
{
    [ModuleExport(typeof(CalendarModule))]
    public class CalendarModule : IModule
    {
        [Import]
        public IRegionManager RegionManager;

        public void Initialize()
        {
            // todo: 11 - Pre-populating regions with items
            // Items may be placed in regions prior to navigating to them.  In this case, since we're registering
            // the type CalendarNavigationItemView, it will be created and added to the region when the MainNavigationRegion 
            // is loaded.  Even though it is created when the control associated with the region becomes visible,
            // the view itself won't be visible until it's navigated to. 
            //
            // Anything placed in a region in this manner will not be added to the navigation journal.
            this.RegionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(CalendarNavigationItemView));
        }
    }
}
