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

using Microsoft.Practices.Composite.UnityExtensions.Tests.Mocks;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.UnityExtensions.Tests
{
    [TestClass]
    public class UnityContainerAdapterFixture
    {
        [TestMethod]
        public void CanRegisterAndResolveUsingContainerAdapter()
        {
            IUnityContainer container = new UnityContainer();
            IContainerFacade containerAdapter = new UnityContainerAdapter(container);

            container.RegisterType<IService, MockService>();
            IService mockService = containerAdapter.Resolve(typeof(IService)) as IService;

            Assert.IsInstanceOfType(mockService, typeof(IService));
        }

        [TestMethod]
        public void CanRegisterAndResolveSingletonUsingContainerAdapter()
        {
            IUnityContainer container = new UnityContainer();
            IContainerFacade containerAdapter = new UnityContainerAdapter(container);


            container.RegisterType<IService, MockService>(new ContainerControlledLifetimeManager());
            IService mockService1 = containerAdapter.Resolve(typeof(IService)) as IService;
            IService mockService2 = containerAdapter.Resolve(typeof(IService)) as IService;

            Assert.IsNotNull(mockService1);
            Assert.AreSame(mockService1, mockService2);
        }

        [TestMethod]
        public void CanResolveCascadingDependencies()
        {
            IUnityContainer container = new UnityContainer();
            IContainerFacade containerAdapter = new UnityContainerAdapter(container);

            container.RegisterType<IDependantA, DependantA>();
            container.RegisterType<IDependantB, DependantB>();
            container.RegisterType<IService, MockService>(new ContainerControlledLifetimeManager());

            IDependantA dependantA = containerAdapter.Resolve(typeof(IDependantA)) as IDependantA;
            Assert.IsNotNull(dependantA);
            Assert.IsInstanceOfType(dependantA, typeof(IDependantA));
            Assert.IsNotNull(dependantA.MyDependantB);
            Assert.IsInstanceOfType(dependantA.MyDependantB, typeof(IDependantB));
            Assert.IsNotNull(dependantA.MyDependantB.MyService);
            Assert.IsInstanceOfType(dependantA.MyDependantB.MyService, typeof(IService));

        }

        [TestMethod]
        public void TryResolveShouldResolveTheElementIfElementExist()
        {
            IUnityContainer container = new UnityContainer();
            IContainerFacade containerAdapter = new UnityContainerAdapter(container);

            container.RegisterType<IService, MockService>(new ContainerControlledLifetimeManager());

            object dependantA = containerAdapter.TryResolve(typeof(IService));
            Assert.IsNotNull(dependantA);
        }

        [TestMethod]
        public void TryResolveShouldReturnNullIfElementNotExist()
        {
            IUnityContainer container = new UnityContainer();
            IContainerFacade containerAdapter = new UnityContainerAdapter(container);

            object dependantA = containerAdapter.TryResolve(typeof(IDependantA));
            Assert.IsNull(dependantA);
        }

    }
}