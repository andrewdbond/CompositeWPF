// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace Microsoft.Practices.Prism.UnityExtensions.Tests.Mocks
{
    public class DependantA : IDependantA
    {
        public DependantA(IDependantB dependantB)
        {
            MyDependantB = dependantB;
        }

        public IDependantB MyDependantB { get; set; }
    }

    public interface IDependantA
    {
        IDependantB MyDependantB { get; }
    }
}