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
using UIComposition.Modules.Project.Services;

namespace UIComposition.Modules.Project
{
    public class ProjectsListPresenter : IProjectsListPresenter
    {
        private readonly IProjectService projectService;
        readonly ProjectsListPresentationModel model;

        public ProjectsListPresenter(IProjectsListView view, IProjectService projectService)
        {
            this.View = view;
            this.projectService = projectService;
            this.model = new ProjectsListPresentationModel();
            this.model.PropertyChanged += this.model_PropertyChanged;
            view.Model = this.model;
        }

        void model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EmployeeId")
            {
                model.Projects = this.projectService.RetrieveProjects(this.model.EmployeeId);
            }
        }

        public IProjectsListView View { get; set; }
    }
}