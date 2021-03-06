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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptanceTestLibrary.Common;
using System.Windows.Automation;
using Modularity.Tests.AcceptanceTest.TestEntities.Page;
using AcceptanceTestLibrary.UIAWrapper;
using Modularity.Tests.AcceptanceTest.TestEntities.Assertion;
using AcceptanceTestLibrary.ApplicationHelper;

namespace Modularity.Tests.AcceptanceTest.TestEntities.Action
{
    public static class ShellAction<TApp>
        where TApp : AppLauncherBase, new()
    {
        public static void LoadModuleCOnDemand()
        {
            Shell<TApp>.ModuleCButton.Click();
        }

        public static void LoadModuleXOnDemand()
        {
            Shell<TApp>.ModuleXButton.Click();
        }
        
    }
}
