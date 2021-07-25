//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UIComposition.Infrastructure;
using UIComposition.Modules.Project.Services;
using UIComposition.Modules.Project.Tests.Mocks;

namespace UIComposition.Modules.Project.Tests
{
    [TestClass]
    public class ProjectModuleFixture
    {

        MockUnityContainer container;
        MockRegionManager regionManager;

        [TestInitialize]
        public void SetUp()
        {
            container = new MockUnityContainer();
            regionManager = new MockRegionManager();
        }
        [TestMethod]
        public void RegisterViewsAndServices()
        {
            var module = CreateProjectModule();

            module.Initialize();

            Assert.AreEqual(typeof(ProjectService), container.Types[typeof(IProjectService)]);
            Assert.AreEqual(typeof(ProjectsListView), container.Types[typeof(IProjectsListView)]);
            Assert.AreEqual(typeof(ProjectsListPresenter), container.Types[typeof(IProjectsListPresenter)]);
        }

        [TestMethod]
        public void InitializeShouldCallRegisterAndViewServices()
        {
            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterInstance<IRegionManager>(regionManager);
            MockRegion mainToolbar = new MockRegion();

            regionManager.Regions.Add(RegionNames.MainToolBar, mainToolbar);

            ProjectModule module = CreateProjectModule();

            module.Initialize();

            Assert.AreEqual(typeof(ProjectService), container.Types[typeof(IProjectService)]);
            Assert.AreEqual(typeof(ProjectsListView), container.Types[typeof(IProjectsListView)]);
            Assert.AreEqual(typeof(ProjectsListPresenter), container.Types[typeof(IProjectsListPresenter)]);
        }

        private ProjectModule CreateProjectModule()
        {
            ProjectModule module = new ProjectModule(container, new MockRegionContentRegistry());
            return module;
        }

        private class MockRegionContentRegistry : IRegionViewRegistry
        {
            public event System.EventHandler<ViewRegisteredEventArgs> ContentRegistered;



            public System.Collections.Generic.IEnumerable<object> GetContents(string regionName)
            {
                throw new System.NotImplementedException();
            }




            public void RegisterViewWithRegion(string regionName, System.Type viewType)
            {
                throw new System.NotImplementedException();
            }

            public void RegisterViewWithRegion(string regionName, System.Func<object> getContentDelegate)
            {
            }
        }
    }
}

