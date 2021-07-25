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
using System.Windows.Automation;
using Modularity.Tests.AcceptanceTest.TestEntities.Page;
using AcceptanceTestLibrary.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcceptanceTestLibrary.ApplicationHelper;

namespace Modularity.Tests.AcceptanceTest.TestEntities.Assertion
{
    public static class ShellAssertion<TApp>
        where TApp : AppLauncherBase, new()
    {

        #region Defining modules in code
        //check if Modules A, B and D have been loaded as expected
        public static void AssertDefiningModulesInCode()
        {
            InternalAssertControls();
        }

        public static void AssertOnDemandDefineModulesInCodeCLoading()
        {
            InternalAssertControl(Shell<TApp>.ModuleC, "Loading of Module C failed");
        }

        #endregion

        #region Remote Module Loading
        //check if Modules Y and Z have been loaded as expected
        public static void AssertRemoteModuleLoading()
        {
            InternalAssertControl(Shell<TApp>.ModuleY, "Loading of Module Y failed");
            InternalAssertControl(Shell<TApp>.ModuleZ, "Loading of Module Z failed");
            InternalAssertControl(Shell<TApp>.ModuleW, "Loading of Module W failed");
            
        }

        public static void AssertOnDemandRemoteModuleXLoading()
        {
            InternalAssertControl(Shell<TApp>.ModuleX, "Loading of Module X failed");
        }
        #endregion

        #region Directory Lookup Module Loading
        //check if Modules A, B and D have been loaded as expected
        public static void AssertDirectoryLookupModuleLoading()
        {
            InternalAssertControls();
        }

        public static void AssertOnDemandDirectoryLookupModuleCLoading()
        {
            InternalAssertControl(Shell<TApp>.ModuleC, "Loading of Module C failed");
        }

        #endregion

        #region Configuration Module Loading
        //check if Modules A, B and D have been loaded as expected
        public static void AssertConfigurationModuleLoading()
        {
            InternalAssertControls();
        }

        public static void AssertOnDemandConfigurationModuleCLoading()
        {
            InternalAssertControl(Shell<TApp>.ModuleC, "Loading of Module C failed");
        }

        #endregion

        private static void InternalAssertControls()
        {
            InternalAssertControl(Shell<TApp>.ModuleA, "Loading of Module A failed");
            InternalAssertControl(Shell<TApp>.ModuleB, "Loading of Module B failed");
            InternalAssertControl(Shell<TApp>.ModuleD, "Loading of Module D failed");
        }
        private static void InternalAssertControl(AutomationElement control, string message)
        {
            Assert.IsNotNull(control, message);
        }


    }
}
