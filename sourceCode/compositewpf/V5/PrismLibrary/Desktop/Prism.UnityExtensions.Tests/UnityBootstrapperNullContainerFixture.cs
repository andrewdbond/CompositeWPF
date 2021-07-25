// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Windows;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.UnityExtensions.Tests
{
    [TestClass]
    public class UnityBootstrapperNullContainerFixture : BootstrapperFixtureBase
    {
        [TestMethod]
        public void RunThrowsWhenNullContainerCreated()
        {
            var bootstrapper = new NullContainerBootstrapper();

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IUnityContainer");
        }

        private class NullContainerBootstrapper : UnityBootstrapper
        {
            protected override IUnityContainer CreateContainer()
            {
                return null;
            }
            protected override DependencyObject CreateShell()
            {
                throw new NotImplementedException();
            }

            protected override void InitializeShell()
            {
                throw new NotImplementedException();
            }
        }    
    }
}