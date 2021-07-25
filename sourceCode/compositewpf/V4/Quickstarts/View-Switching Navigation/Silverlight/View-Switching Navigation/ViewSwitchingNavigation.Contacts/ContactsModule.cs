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
using ViewSwitchingNavigation.Contacts.Views;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Contacts
{
    [ModuleExport(typeof(ContactsModule))]
    public class ContactsModule : IModule
    {
        [Import]
        public IRegionManager RegionManager;

        public void Initialize()
        {
            // todo: 16 - Contacts with two 'views'
            // 
            // The contacts module offers two navigation options.  One to show contacts with the details informatin
            // and the other to show contact avatars.  Each of these really navigate to the same 'view'
            // but provide additional information as part of the query string so the view can decide to 
            // display details or avatars.
            this.RegionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(ContactsAvatarNavigationItemView));
            this.RegionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(ContactsDetailNavigationItemView));
        }
    }
}
