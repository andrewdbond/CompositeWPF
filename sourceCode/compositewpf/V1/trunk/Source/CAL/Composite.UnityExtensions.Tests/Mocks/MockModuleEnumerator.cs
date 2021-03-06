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

using Microsoft.Practices.Composite.Modularity;

namespace Microsoft.Practices.Composite.UnityExtensions.Tests.Mocks
{
    class MockModuleEnumerator : IModuleEnumerator
    {
        public bool GetStartupLoadedModulesCalled;
        public ModuleInfo[] StartupLoadedModules = new ModuleInfo[0];

        public ModuleInfo[] GetModules()
        {
            throw new System.NotImplementedException();
        }

        public ModuleInfo[] GetStartupLoadedModules()
        {
            GetStartupLoadedModulesCalled = true;
            return StartupLoadedModules;
        }

        public ModuleInfo GetModule(string moduleName)
        {
            throw new System.NotImplementedException();
        }
    }
}