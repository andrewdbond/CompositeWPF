// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Modularity;
using System;

namespace Microsoft.Practices.Prism.Composition.Tests.Mocks.Modules
{
    public class MockModuleA : IModule
    {
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class DummyClass
    {
    }
}
