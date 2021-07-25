// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Modularity;

namespace Microsoft.Practices.Prism.Composition.Tests.Mocks.Modules
{
    [Module(ModuleName = "DependencyModule")]
    public class DependencyModule : IModule
    {
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }
    }
}
