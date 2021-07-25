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
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace UIComposition.Modules.Project.Services
{
    public class ProjectService : IProjectService
    {
        private Dictionary<int, ObservableCollection<BusinessEntities.Project>> projects;

        public ProjectService()
        {
            projects = new Dictionary<int, ObservableCollection<BusinessEntities.Project>>();

            ObservableCollection<BusinessEntities.Project> projectsEmployee1 = new ObservableCollection<BusinessEntities.Project>();

            BusinessEntities.Project project1 = new BusinessEntities.Project() { ProjectName = "Project 1", Role = "Architect" };
            BusinessEntities.Project project2 = new BusinessEntities.Project() { ProjectName = "Project 2", Role = "Developer" };

            projectsEmployee1.Add(project1);
            projectsEmployee1.Add(project2);

            projects.Add(1, projectsEmployee1);

            ObservableCollection<BusinessEntities.Project> projectsEmployee2 = new ObservableCollection<BusinessEntities.Project>();
            projectsEmployee2.Add(project1);
            projectsEmployee2.Add(project2);
            projectsEmployee2.Add(new BusinessEntities.Project() { ProjectName = "Project 3", Role = "Dev Lead" });

            projects.Add(2, projectsEmployee2);

            ObservableCollection<BusinessEntities.Project> projectsEmployee3 = new ObservableCollection<BusinessEntities.Project>();

            projects.Add(3, projectsEmployee3);

        }

        public ObservableCollection<BusinessEntities.Project> RetrieveProjects(int employeeId)
        {
            if (projects.ContainsKey(employeeId))
                return projects[employeeId];

            return null;
        }
    }
}
