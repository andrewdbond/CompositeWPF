// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.


namespace Microsoft.Practices.Prism.UnityExtensions.Tests.Mocks
{
    public class DependantB : IDependantB
    {
        public DependantB(IService service)
        {
            MyService = service;
        }

        public IService MyService { get; set; }
    }

    public interface IDependantB
    {
        IService MyService { get; }
    }
}