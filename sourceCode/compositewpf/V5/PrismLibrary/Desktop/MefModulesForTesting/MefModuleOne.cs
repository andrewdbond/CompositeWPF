// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace MefModulesForTesting
{
    [ModuleExport("MefModuleOne", typeof(MefModuleOne))]
    public class MefModuleOne : IModule
    {
        public bool WasInitialized = false;
        public void Initialize()
        {
            WasInitialized = true;
        }
    }
}
