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
using AcceptanceTestLibrary.TestEntityBase;
using AcceptanceTestLibrary.ApplicationHelper;

namespace Modularity.Tests.AcceptanceTest.TestEntities.Page
{
    public static class Shell<TApp>
        where TApp : AppLauncherBase, new()
    {
        public static AutomationElement Window
        {
            get { return PageBase<TApp>.Window; }
            set { PageBase<TApp>.Window = value; }
        }

        public static AutomationElement ModuleCButton
        {
            get { return PageBase<TApp>.FindControlByContent("LoadModuleC"); }
        }

        public static AutomationElement ModuleXButton
        {
            get { return PageBase<TApp>.FindControlByContent("LoadModuleX"); }
        }

        public static AutomationElement ModuleA
        {
            get { return PageBase<TApp>.FindControlByContent("ModuleALoaded"); }
        }

        public static AutomationElement ModuleB
        {
            get { return PageBase<TApp>.FindControlByContent("ModuleBLoaded"); }
        }

        public static AutomationElement ModuleC
        {
            get { return PageBase<TApp>.FindControlByContent("ModuleCLoaded"); }
        }

        public static AutomationElement ModuleD
        {
            get { return PageBase<TApp>.FindControlByContent("ModuleDLoaded"); }
        }

        public static AutomationElement ModuleY
        {
            get { return PageBase<TApp>.FindControlByContent("ModuleYLoaded"); }
        }

        public static AutomationElement ModuleZ
        {
            get { return PageBase<TApp>.FindControlByContent("ModuleZLoaded"); }
        }

        public static AutomationElement ModuleX
        {
            get { return PageBase<TApp>.FindControlByContent("ModuleXLoaded"); }
        }

        public static AutomationElement ModuleW
        {
            get { return PageBase<TApp>.FindControlByContent("ModuleWLoaded"); }
        }
        
    }
}
