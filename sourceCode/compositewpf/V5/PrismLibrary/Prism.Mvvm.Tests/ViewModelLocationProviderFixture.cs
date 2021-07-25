// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Tests.Mocks.ViewModels;
using Microsoft.Practices.Prism.Mvvm.Tests.Mocks.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Practices.Prism.Mvvm.Tests
{
    [TestClass]
    public class ViewModelLocationProviderFixture
    {
        [TestMethod]
        public void ShouldLocateViewModelWithDefaultSettings()
        {
            ResetViewModelLocationProvider();

            Mock view = new Mock();
            Assert.IsNull(view.DataContext);

            ViewModelLocationProvider.AutoWireViewModelChanged(view);
            Assert.IsNotNull(view.DataContext);
            Assert.IsInstanceOfType(view.DataContext, typeof(MockViewModel));
        }

        [TestMethod]
        public void ShouldUseCustomDefaultViewModelFactoryWhenSet()
        {
            ResetViewModelLocationProvider();

            Mock view = new Mock();
            Assert.IsNull(view.DataContext);

            object mockObject = new object();
            ViewModelLocationProvider.SetDefaultViewModelFactory(viewType => mockObject);

            ViewModelLocationProvider.AutoWireViewModelChanged(view);
            Assert.IsNotNull(view.DataContext);
            Assert.ReferenceEquals(view.DataContext, mockObject);
        }

        [TestMethod]
        public void ShouldUseCustomDefaultViewTypeToViewModelTypeResolverWhenSet()
        {
            ResetViewModelLocationProvider();

            Mock view = new Mock();
            Assert.IsNull(view.DataContext);

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType => typeof(ViewModelLocationProviderFixture));

            ViewModelLocationProvider.AutoWireViewModelChanged(view);
            Assert.IsNotNull(view.DataContext);
            Assert.IsInstanceOfType(view.DataContext, typeof(ViewModelLocationProviderFixture));
        }

        [TestMethod]
        public void ShouldUseCustomFactoryWhenSet()
        {
            ResetViewModelLocationProvider();

            Mock view = new Mock();
            Assert.IsNull(view.DataContext);

            string viewModel = "Test String";
            ViewModelLocationProvider.Register(view.GetType().ToString(), () => viewModel);

            ViewModelLocationProvider.AutoWireViewModelChanged(view);
            Assert.IsNotNull(view.DataContext);
            Assert.ReferenceEquals(view.DataContext, viewModel);
        }

        private static void ResetViewModelLocationProvider()
        {
            Type staticType = typeof(ViewModelLocationProvider);
            ConstructorInfo ci = staticType.TypeInitializer;
            object[] parameters = new object[0];
            ci.Invoke(null, parameters);
        }
    }
}
