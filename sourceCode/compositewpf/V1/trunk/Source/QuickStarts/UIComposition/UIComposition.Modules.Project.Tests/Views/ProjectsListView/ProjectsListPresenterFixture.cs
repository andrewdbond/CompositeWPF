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

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UIComposition.Modules.Project.Tests.Mocks;
using UIComposition.Infrastructure;

namespace UIComposition.Modules.Project.Tests
{
    [TestClass]
    public class ProjectsListPresenterFixture
    {
        MockProjectsListView view;
        MockProjectService projectService;

        [TestInitialize]
        public void SetUp()
        {
            view = new MockProjectsListView();
            projectService = new MockProjectService();
        }

        [TestMethod]
        public void CanInitPresenter()
        {
            ProjectsListPresenter presenter = CreatePresenter();

            Assert.IsNotNull(presenter);
            Assert.AreEqual<IProjectsListView>(view, presenter.View);
        }

        [TestMethod]
        public void SetProjectsShouldSetModelOnTheView()
        {
            int employeeId = 10;
            Assert.IsNull(view.Model);
            Assert.IsFalse(projectService.RetrieveProjectsCalled);
           
            ProjectsListPresenter presenter = CreatePresenter();
            presenter.SetProjects(employeeId);

            Assert.IsNotNull(view.Model);
            Assert.IsTrue(projectService.RetrieveProjectsCalled);
            Assert.AreEqual(10, projectService.EmployeeId);
            Assert.AreEqual(projectService.RetrieveProjects(employeeId).Count, view.Model.Projects.Count);
        }

        private ProjectsListPresenter CreatePresenter()
        {
            ProjectsListPresenter presenter = new ProjectsListPresenter(view, projectService);
            return presenter;
        }
    }
}