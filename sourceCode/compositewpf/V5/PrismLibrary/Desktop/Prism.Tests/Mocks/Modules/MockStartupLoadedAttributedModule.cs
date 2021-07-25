// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.Prism.Modularity;

namespace Microsoft.Practices.Prism.Composition.Tests.Mocks.Modules
{
    public class MockStartupLoadedAttributedModule
    {
        [Module(ModuleName = "TestModule", StartupLoaded = false)]
        public class MockAttributedModule : IModule
        {
            public void Initialize()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
