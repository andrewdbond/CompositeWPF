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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComposition.Modules.Project.Services;
using System.Collections.ObjectModel;

namespace UIComposition.Modules.Project.Tests.Mocks
{
    class MockProjectService : IProjectService
    {
        public bool RetrieveProjectsCalled;
        public int EmployeeId;

        public ObservableCollection<BusinessEntities.Project> RetrieveProjects(int employeeId)
        {
            RetrieveProjectsCalled = true;
            EmployeeId = employeeId;
            return new ObservableCollection<BusinessEntities.Project>();
        }
    }
}
