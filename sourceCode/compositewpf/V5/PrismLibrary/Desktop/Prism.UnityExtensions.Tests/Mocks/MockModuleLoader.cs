// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Modularity;

namespace Microsoft.Practices.Prism.UnityExtensions.Tests.Mocks
{
    internal class MockModuleInitializer : IModuleInitializer
    {
        public bool LoadCalled;

        public void Initialize(ModuleInfo moduleInfo)
        {
            LoadCalled = true;
        }
    }
}